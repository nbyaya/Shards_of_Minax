using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a red wizard's corpse")]
    public class SzassTam : BaseCreature
    {
        private DateTime m_NextInfernoBurst;
        private DateTime m_NextFlameShield;
        private DateTime m_NextFireball;
        private DateTime m_FlameShieldEnd;
        private bool m_AbilitiesInitialized;
        private bool m_FlameShieldActive;

        [Constructable]
        public SzassTam()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Szass Tam";
            Body = 78;
            Hue = 2093;
			BaseSoundID = 412;

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

            m_AbilitiesInitialized = false;
            m_FlameShieldActive = false;
        }

        public SzassTam(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextInfernoBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFlameShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFireball = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextInfernoBurst)
                {
                    InfernoBurst();
                }

                if (DateTime.UtcNow >= m_NextFlameShield && !m_FlameShieldActive)
                {
                    ActivateFlameShield();
                }

                if (DateTime.UtcNow >= m_NextFireball)
                {
                    CastFireball();
                }
            }

            if (m_FlameShieldActive && DateTime.UtcNow >= m_FlameShieldEnd)
            {
                DeactivateFlameShield();
            }

            if (Utility.RandomDouble() < 0.1)
            {
                ShowFlickerEffect();
            }
        }

        private void InfernoBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Szass Tam summons a fiery inferno! *");
            PlaySound(0x208);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 100, 0, 0);
                    m.SendMessage("You are engulfed by a massive burst of flames!");
                    Effects.SendLocationEffect(m.Location, m.Map, 0x36BD, 30, 10);
                }
            }

            m_NextInfernoBurst = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void ActivateFlameShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Szass Tam activates a powerful Flame Shield! *");
            PlaySound(0x1E3);

            FixedParticles(0x373A, 9, 32, 5030, EffectLayer.Waist);
            m_FlameShieldActive = true;
            m_FlameShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void DeactivateFlameShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Szass Tam's Flame Shield fades away. *");
            m_FlameShieldActive = false;
        }

        private void CastFireball()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Szass Tam casts a blazing fireball! *");
            PlaySound(0x1F5);
            
            m_NextFireball = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void ShowFlickerEffect()
        {
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_FlameShieldActive)
            {
                damage = 0;
                from.SendMessage("Your attack is deflected by the Flame Shield!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false;
            m_FlameShieldActive = false;
        }
    }
}
