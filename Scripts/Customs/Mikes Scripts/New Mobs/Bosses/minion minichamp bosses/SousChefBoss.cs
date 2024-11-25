using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master sous chef")]
    public class SousChefBoss : SousChef
    {
        [Constructable]
        public SousChefBoss() : base()
        {
            Name = "Master Sous Chef";
            Title = "the Culinary Overlord";

            // Update stats to match or exceed the desired boss stats
            SetStr(500);  // Enhanced strength
            SetDex(150);  // Enhanced dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(15, 25); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 60, 70); // Higher resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Cooking, 120.0); // Maxed-out cooking skill
            SetSkill(SkillName.MagicResist, 90.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.0);

            Fame = 12000;  // Higher fame for the boss
            Karma = -12000;  // Negative karma for the boss

            VirtualArmor = 60;  // Higher armor for the boss

            // Attach a random ability to the boss
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
            // You can add additional boss-specific behaviors here if necessary
        }

        public SousChefBoss(Serial serial) : base(serial)
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
