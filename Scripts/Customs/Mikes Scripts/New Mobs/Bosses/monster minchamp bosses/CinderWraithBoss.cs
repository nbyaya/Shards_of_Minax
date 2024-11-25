using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a cinder wraith boss corpse")]
    public class CinderWraithBoss : CinderWraith
    {
        private DateTime m_NextAshCloud;
        private DateTime m_NextFlareBurst;
        private DateTime m_NextEmberWraith;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CinderWraithBoss()
            : base()
        {
            Name = "Cinder Wraith Overlord";
            Title = "the Infernal Fury";
            Hue = 1664; // Unique hue for the wraith
            BaseSoundID = 838;

            // Enhanced stats to match boss-tier
            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling

            Fame = 24000; // High fame
            Karma = -24000; // Negative karma (Boss behavior)

            VirtualArmor = 100; // Enhanced virtual armor

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 0;

            m_AbilitiesInitialized = false; // Initialize flag

            // Attach the XmlRandomAbility for enhanced behavior
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Additional loot (rich drops)
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public CinderWraithBoss(Serial serial)
            : base(serial)
        {
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
