using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a crystal warden boss corpse")]
    public class CrystalWardenBoss : CrystalWarden
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CrystalWardenBoss()
            : base()
        {
            Name = "Crystal Warden Overlord";
            Title = "the Supreme Guardian";

            // Enhance stats to be at boss level (similar to Barracoon's stats or higher)
            SetStr(1200); // Enhanced strength
            SetDex(255); // Max dexterity for agility
            SetInt(250); // Increased intelligence

            SetHits(12000); // Higher health
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Improved resistances
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (more notorious)

            VirtualArmor = 100; // Stronger armor

            Tamable = false; // Boss can't be tamed
            ControlSlots = 0; // Not tamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_AbilitiesInitialized = false; // Flag to initialize abilities
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

        public CrystalWardenBoss(Serial serial) : base(serial)
        {
        }
    }
}
