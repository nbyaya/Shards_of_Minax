using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a zor'thul corpse")]
    public class ZorThulBoss : ZorThul
    {
        private DateTime m_NextWhisperOfMadness;
        private DateTime m_NextHallucinogenicGaze;
        private DateTime m_NextRealityWarp;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ZorThulBoss() : base()
        {
            Name = "Zor'Thul the Whispering";
            Title = "the Supreme Whisperer";
            Hue = 1758; // Unique hue for Zor'Thul
            BaseSoundID = 377;

            // Enhance stats to be at least as strong as Barracoon's or better
            SetStr(1200); // Upper end of Barracoon's strength
            SetDex(255); // Upper end of Barracoon's dexterity
            SetInt(750); // Upper end of Barracoon's intelligence

            SetHits(12000); // Enhanced health
            SetDamage(35, 45); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;

            Tamable = false;
            ControlSlots = 0;
            MinTameSkill = 0;

            // Attach a random ability for extra challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // ✅ REQUIRED for deserialization!
        public ZorThulBoss(Serial serial) : base(serial)
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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Additional boss-specific AI or abilities could go here
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
        }
    }
}
