using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArcheryBonusChest : WoodenChest
    {
        [Constructable]
        public ArcheryBonusChest()
        {
            Name = "Archery Bonus Chest";
            Hue = Utility.Random(1, 1157);

            // Add items to the chest
            AddItem(CreateBow(), 0.20);
            AddItem(CreateArcheryGloves(), 0.25);
            AddItem(CreateArcheryHelmet(), 0.20);
            AddItem(CreateArcheryBoots(), 0.20);
            AddItem(CreateArcheryBracelet(), 0.15);
            AddItem(CreateArcheryRing(), 0.10);
            AddItem(CreateArrows(), 0.30);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateBow()
        {
            CompositeBow bow = new CompositeBow();
            bow.Name = "Bow of Precision";
            bow.Attributes.BonusDex = 10;
            bow.Attributes.BonusStr = 5;
            bow.Attributes.BonusInt = 5;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 10.0);
            return bow;
        }

        private Item CreateArcheryGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Gloves of Precision";
            gloves.Attributes.BonusDex = 5;
            gloves.SkillBonuses.SetValues(0, SkillName.Archery, 10.0);
            return gloves;
        }

        private Item CreateArcheryHelmet()
        {
            LeatherCap helmet = new LeatherCap();
            helmet.Name = "Helmet of the Sharpshooter";
            helmet.Attributes.BonusDex = 7;
            helmet.Attributes.BonusInt = 3;
            helmet.SkillBonuses.SetValues(0, SkillName.Archery, 12.0);
            return helmet;
        }

        private Item CreateArcheryBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Boots of Speed";
            boots.Attributes.BonusDex = 8;
            boots.SkillBonuses.SetValues(0, SkillName.Archery, 10.0);
            return boots;
        }

        private Item CreateArcheryBracelet()
        {
            GoldRing ring = new GoldRing();
            ring.Name = "Bracelet of Precision";
            ring.Attributes.BonusDex = 6;
            ring.SkillBonuses.SetValues(0, SkillName.Archery, 8.0);
            return ring;
        }

        private Item CreateArcheryRing()
        {
            GoldRing ring = new GoldRing();
            ring.Name = "Ring of the Archer";
            ring.Attributes.BonusDex = 5;
            ring.SkillBonuses.SetValues(0, SkillName.Archery, 5.0);
            return ring;
        }

        private Item CreateArrows()
        {
            Arrow arrows = new Arrow(Utility.RandomMinMax(20, 50));
            arrows.Name = "Arrows of the Mark";
            return arrows;
        }

        public ArcheryBonusChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
