using System;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a storm bone corpse")]
    public class StormBone : BaseCreature
    {
        private DateTime m_NextThunderStrike;
        private DateTime m_NextStormCall;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormBone()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm bone";
            Body = 57; // BoneKnight body
            Hue = 2363; // Unique hue for storm effect
			BaseSoundID = 451;

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

            // Equip with a sword and shield as examples
            PackItem(new Scimitar());
            PackItem(new WoodenShield());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public StormBone(Serial serial)
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
                    m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStormCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextThunderStrike)
                {
                    ThunderStrike();
                }

                if (DateTime.UtcNow >= m_NextStormCall)
                {
                    StormCall();
                }
            }
        }

        private void ThunderStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thunder Strike! *");
            PlaySound(0x29F); // Lightning sound

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                    m.FixedEffect(0x376A, 10, 16); // Lightning effect
                    m.SendMessage("You are struck by a powerful thunderbolt!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void StormCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Storm Call! *");
            PlaySound(0x20C); // Thunder sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.Damage(Utility.RandomMinMax(8, 15), this);
                    m.SendMessage("A storm engulfs you with lightning!");
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 2023);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5045);

            m_NextStormCall = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
