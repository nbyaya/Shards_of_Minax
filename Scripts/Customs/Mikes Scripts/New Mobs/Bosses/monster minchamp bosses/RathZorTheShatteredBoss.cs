using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a rath'zor the shattered corpse")]
    public class RathZorTheShatteredBoss : RathZorTheShattered
    {
        private DateTime m_NextRealityShatter;
        private DateTime m_NextDimensionalRift;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RathZorTheShatteredBoss()
            : base()
        {
            Name = "Rath'Zor the Shattered";
            Hue = 1767; // Unique hue for Rath'Zor
            Body = 22; // ElderGazer body
            BaseSoundID = 377;

            // Enhanced Stats for Boss
            SetStr(1200); // Upper range of strength for boss-tier
            SetDex(255); // Upper range of dexterity
            SetInt(250); // High intelligence for more power

            SetHits(12000); // High health for a boss-tier creature

            SetDamage(35, 50); // Increased damage output

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100;

            Tamable = false; // Boss should not be tamable
            MinTameSkill = 0; // Ensure no one can tame

            m_AbilitiesInitialized = false; // Initialize flag
            XmlAttach.AttachTo(this, new XmlRandomAbility()); PackItem(new BossTreasureBox());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            this.AddLoot(LootPack.FilthyRich, 2); // Rich loot
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8); // Add additional gems for the boss-tier loot
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific behavior could be added here
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
