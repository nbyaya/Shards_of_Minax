using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a stormcaller ettin corpse")]
    public class StormcallerEttin : BaseCreature
    {
        private DateTime m_NextThunderStrike;
        private DateTime m_NextStormShield;
        private DateTime m_NextElectricSurge;
        private DateTime m_StormShieldEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormcallerEttin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a stormcaller ettin";
            Body = 18;
            BaseSoundID = 367;
            Hue = 1559; // Electric blue hue

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public StormcallerEttin(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}		

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 4; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextStormShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextElectricSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextThunderStrike)
                {
                    ThunderStrike();
                }

                if (DateTime.UtcNow >= m_NextStormShield && m_StormShieldEnd == DateTime.MinValue)
                {
                    StormShield();
                }

                if (DateTime.UtcNow >= m_NextElectricSurge)
                {
                    ElectricSurge();
                }
            }

            if (DateTime.UtcNow >= m_StormShieldEnd && m_StormShieldEnd != DateTime.MinValue)
            {
                EndStormShield();
            }
        }

        private void ThunderStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Stormcaller Ettin unleashes a Thunder Strike! *");
            PlaySound(0x29);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    m.BoltEffect(0);
                }
            }

            // Create lightning storm effect
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-5, 5);
                int y = Y + Utility.RandomMinMax(-5, 5);
                int z = Z;

                Effects.SendLocationEffect(new Point3D(x, y, z), Map, 0x3967, 15);
            }

            m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void StormShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Stormcaller Ettin creates a Storm Shield! *");
            PlaySound(0x217);
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            m_StormShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextStormShield = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void EndStormShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Shield dissipates *");
            m_StormShieldEnd = DateTime.MinValue;
        }

        private void ElectricSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Stormcaller Ettin releases an Electric Surge! *");
            PlaySound(0x5C);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    m.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Waist);
                    m.PlaySound(0x201);

                    m.Paralyze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextElectricSurge = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_StormShieldEnd > DateTime.UtcNow && from != null && from != this && from.Alive && !from.IsDeadBondedPet && 0.2 > Utility.RandomDouble())
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Shield deflects the attack! *");
                from.FixedParticles(0x37CC, 1, 40, 97, 3, 9917, EffectLayer.Waist);
                from.PlaySound(0x51D);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextThunderStrike = DateTime.UtcNow;
            m_NextStormShield = DateTime.UtcNow;
            m_NextElectricSurge = DateTime.UtcNow;
            m_StormShieldEnd = DateTime.MinValue;
            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
