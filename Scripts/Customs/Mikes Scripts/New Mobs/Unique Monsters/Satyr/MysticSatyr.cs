using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mystic satyr's corpse")]
    public class MysticSatyr : BaseCreature
    {
        private DateTime m_NextEnchantedMelody;
        private DateTime m_NextIllusionaryHymn;
        private DateTime m_NextManaSurge;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MysticSatyr()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic satyr";
            Body = 271; // Satyr body
            Hue = 2321; // Unique hue
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
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public MysticSatyr(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextEnchantedMelody = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextIllusionaryHymn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextManaSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEnchantedMelody)
                {
                    EnchantedMelody();
                }

                if (DateTime.UtcNow >= m_NextIllusionaryHymn)
                {
                    IllusionaryHymn();
                }

                if (DateTime.UtcNow >= m_NextManaSurge)
                {
                    ManaSurge();
                }
            }
        }

        private void EnchantedMelody()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays an enchanted melody *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You feel rejuvenated by the Mystic Satyr's melody!");
                    m.Mana += 10; // Example effect: Restore mana
                }
            }
            // Reset the next ability time with a new random interval
            Random rand = new Random();
            m_NextEnchantedMelody = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
        }

        private void IllusionaryHymn()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sings an illusionary hymn *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("You are disoriented by the Mystic Satyr's hymn!");
                    // Example: Reduce hit chance by 10%
                    m.SendMessage("Your attacks are less accurate!");
                }
            }
            // Reset the next ability time with a new random interval
            Random rand = new Random();
            m_NextIllusionaryHymn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 90));
        }

        private void ManaSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Channels a mana surge *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m.Mana < m.ManaMax)
                {
                    m.SendMessage("You feel a surge of mana flowing through you!");
                    m.Mana += 20; // Example effect: Restore mana
                }
            }
            // Reset the next ability time with a new random interval
            Random rand = new Random();
            m_NextManaSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(40, 120));
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
