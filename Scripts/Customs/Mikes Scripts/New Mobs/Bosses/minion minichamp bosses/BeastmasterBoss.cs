using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the beastmaster overlord")]
    public class BeastmasterBoss : Beastmaster
    {
        [Constructable]
        public BeastmasterBoss() : base()
        {
            Name = "Beastmaster Overlord";
            Title = "the Supreme Beastmaster";

            // Update stats to match or exceed Barracoon's level
            SetStr(1000); // Upper strength for a boss-tier
            SetDex(250);  // Upper dexterity for a boss-tier
            SetInt(400);  // Upper intelligence for a boss-tier

            SetHits(8000); // Increase health significantly for a boss
            SetDamage(30, 50); // Increase damage range for more danger

            // Increase resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 65, 80);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0); // Increase magic resistance to make it harder
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.AnimalTaming, 120.0);
            SetSkill(SkillName.AnimalLore, 120.0);

            Fame = 25000; // High fame for a boss-tier creature
            Karma = -25000; // Negative karma to fit the evil boss theme

            VirtualArmor = 80; // Increase virtual armor to make it more durable

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

        public BeastmasterBoss(Serial serial) : base(serial)
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
