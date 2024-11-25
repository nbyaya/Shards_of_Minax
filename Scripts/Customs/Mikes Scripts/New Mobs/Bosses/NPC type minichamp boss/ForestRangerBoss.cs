using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the forest guardian")]
    public class ForestRangerBoss : ForestRanger
    {
        [Constructable]
        public ForestRangerBoss() : base()
        {
            Name = "Forest Guardian";
            Title = "the Protector of the Woods";

            // Update stats to match or exceed a typical boss
            SetStr(500, 700);  // Increased strength
            SetDex(500, 700);  // Increased dexterity
            SetInt(500, 700);  // Increased intelligence
            SetHits(12000);    // Increased health
            SetDamage(200, 350); // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 75, 90);

            SetSkill(SkillName.Magery, 150.0, 200.0);
            SetSkill(SkillName.Fencing, 150.0, 200.0);
            SetSkill(SkillName.Macing, 150.0, 200.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Swords, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 150.0, 200.0);
            SetSkill(SkillName.Wrestling, 150.0, 200.0);

            Fame = 10000;
            Karma = 10000;

            VirtualArmor = 90; // Increased armor for tankiness

            // Attach the XmlRandomAbility for added functionality
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override loot generation to include 5 MaxxiaScrolls in addition to standard loot
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
            // Custom boss logic could be added here if needed
        }

        public ForestRangerBoss(Serial serial) : base(serial)
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
