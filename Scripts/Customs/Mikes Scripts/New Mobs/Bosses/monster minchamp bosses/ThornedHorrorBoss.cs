using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a thorned horror boss corpse")]
    public class ThornedHorrorBoss : ThornedHorror
    {
        private DateTime m_NextThornBarrage;
        private DateTime m_NextThornyEmbrace;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ThornedHorrorBoss()
            : base("Thorned Horror Boss")
        {
            // Set enhanced stats based on Barracoon's stats or better
            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300);   // Increased dexterity
            SetInt(250, 350);   // Increased intelligence

            SetHits(15000); // Higher health
            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 110.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;

            // Attach the XmlRandomAbility for dynamic effects
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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if needed
        }

        public ThornedHorrorBoss(Serial serial)
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

            m_AbilitiesInitialized = false; // Reset the abilities flag on deserialization
        }
    }
}
