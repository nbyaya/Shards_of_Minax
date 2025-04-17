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
            SetStr(1200);
            SetDex(255);
            SetInt(250);

            SetHits(12000);

            SetDamage(35, 50);

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

            Tamable = false;
            MinTameSkill = 0;

            m_AbilitiesInitialized = false;

            XmlAttach.AttachTo(this, new XmlRandomAbility());
            PackItem(new BossTreasureBox());
        }

        // 🔧 THIS is the missing constructor that fixes the warning!
        public RathZorTheShatteredBoss(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());

            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
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
