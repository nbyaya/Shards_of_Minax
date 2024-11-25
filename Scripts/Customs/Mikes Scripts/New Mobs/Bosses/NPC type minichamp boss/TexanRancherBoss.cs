using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Texan Ranch Boss")]
    public class TexanRancherBoss : TexanRancher
    {
        [Constructable]
        public TexanRancherBoss() : base()
        {
            // Set the name and title for the boss
            Name = "Texan Ranch Boss";
            Title = "the Cattle King";

            // Enhance stats to be boss-tier
            SetStr(1200); // High strength
            SetDex(255);  // High dexterity
            SetInt(250);  // High intelligence

            SetHits(10000); // High health

            SetDamage(25, 40); // Higher damage

            // Set resistance values to be higher
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Set skills to higher values for the boss
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 18000;  // Higher fame for a boss
            Karma = -18000; // Negative karma for a boss

            VirtualArmor = 70; // High virtual armor

            // Attach the XmlRandomAbility to add extra random abilities
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
            // Additional boss logic can go here, such as special behaviors or spells
        }

        public TexanRancherBoss(Serial serial) : base(serial)
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
