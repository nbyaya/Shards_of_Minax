using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a gentle satyr's corpse")]
    public class SummonedGentleSatyr : BaseCreature
    {
        private DateTime m_NextRestorativeMelody;
        private DateTime m_NextSerenitySong;
        private DateTime m_NextSoothingChorus;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SummonedGentleSatyr()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gentle satyr";
            Body = 271; // Satyr body
            Hue = 2325; // Unique hue for Gentle Satyr
			this.BaseSoundID = 0x586;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedGentleSatyr(Serial serial)
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
                    m_NextRestorativeMelody = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSerenitySong = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSoothingChorus = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRestorativeMelody)
                {
                    PerformRestorativeMelody();
                }

                if (DateTime.UtcNow >= m_NextSerenitySong)
                {
                    PerformSerenitySong();
                }

                if (DateTime.UtcNow >= m_NextSoothingChorus)
                {
                    PerformSoothingChorus();
                }
            }
        }

        private void PerformRestorativeMelody()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a soothing melody *");
            PlaySound(0x4F0); // Sound for music
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m.Alive)
                {
                    m.Hits += 10;
                    m.Mana += 10;
                }
            }

            m_NextRestorativeMelody = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void PerformSerenitySong()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sings a calming song *");
            PlaySound(0x4F0); // Sound for music
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature)
                {
                    // Here, instead of changing weapon damage, we'll reduce the creature's aggression
                    if (m is BaseCreature creature)
                    {
                        creature.AI = AIType.AI_Animal; // Change to a non-aggressive AI
                    }
                }
            }

            m_NextSerenitySong = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void PerformSoothingChorus()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sings a soothing chorus *");
            PlaySound(0x4F0); // Sound for music
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m.Alive)
                {
                    m.Hits += 20;
                    m.Mana += 20;
                    // Cure poison if present
                    if (m.Poison != null)
                        m.Poison = null;
                }
            }

            m_NextSoothingChorus = DateTime.UtcNow + TimeSpan.FromMinutes(3);
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
