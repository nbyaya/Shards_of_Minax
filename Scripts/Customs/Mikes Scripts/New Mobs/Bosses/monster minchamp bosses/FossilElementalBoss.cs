using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a fossil elemental overlord corpse")]
    public class FossilElementalBoss : FossilElemental
    {
        private DateTime m_NextBoneArmor;
        private DateTime m_NextFossilBurst;
        private bool m_BoneArmorActive;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FossilElementalBoss() : base()
        {
            Name = "Fossil Elemental Overlord";
            Title = "the Ancient Warden";

            // Update stats to match or exceed Barracoon's values
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255);  // Matching Barracoon's upper dexterity
            SetInt(250);  // Matching Barracoon's upper intelligence

            SetHits(12000); // Set to a high value for boss difficulty
            SetDamage(29, 38); // Similar to Barracoon's damage range

            SetResistance(ResistanceType.Physical, 75, 80); // Increased resistance
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100; // Higher virtual armor for increased tankiness

            Tamable = false;
            ControlSlots = 3;

            m_AbilitiesInitialized = false; // Flag for ability initialization

            // Attach random abilities
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

            // Additional boss logic could be added here (like a special attack phase)
        }

        public FossilElementalBoss(Serial serial) : base(serial)
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

        }
    }
}
