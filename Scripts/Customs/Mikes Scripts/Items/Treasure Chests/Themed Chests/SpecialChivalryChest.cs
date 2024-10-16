using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialChivalryChest : WoodenChest
    {
        [Constructable]
        public SpecialChivalryChest()
        {
            Name = "Chest of Chivalry";
            Hue = Utility.Random(1, 1157);

            // Add items to the chest
            AddItem(CreateChivalryCloak(), 0.25);
            AddItem(CreateChivalryHelmet(), 0.20);
            AddItem(CreateChivalryArmor(), 0.30);
            AddItem(CreateChivalryRing(), 0.15);
            AddItem(CreateChivalryShield(), 0.10);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateChivalryCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Cloak of Chivalry";
            cloak.Hue = Utility.Random(1157, 1161);
            cloak.Attributes.BonusStr = 5;
            cloak.Attributes.BonusDex = 5;
            cloak.Attributes.BonusInt = 5;
            cloak.Attributes.LowerManaCost = 10;
            cloak.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            return cloak;
        }

        private Item CreateChivalryHelmet()
        {
            Helmet helmet = new Helmet();
            helmet.Name = "Helmet of Valor";
            helmet.Hue = Utility.Random(1157, 1161);
            helmet.Attributes.BonusStr = 3;
            helmet.Attributes.BonusDex = 3;
            helmet.Attributes.BonusInt = 3;
            helmet.Attributes.LowerManaCost = 5;
            helmet.SkillBonuses.SetValues(0, SkillName.Chivalry, 15.0);
            return helmet;
        }

        private Item CreateChivalryArmor()
        {
            PlateChest armor = new PlateChest();
            armor.Name = "Armor of Chivalry";
            armor.Hue = Utility.Random(1157, 1161);
            armor.Attributes.BonusStr = 10;
            armor.Attributes.BonusDex = 10;
            armor.Attributes.BonusInt = 10;
            armor.Attributes.LowerManaCost = 20;
            armor.SkillBonuses.SetValues(0, SkillName.Chivalry, 30.0);
            return armor;
        }

        private Item CreateChivalryRing()
        {
            GoldRing ring = new GoldRing();
            ring.Name = "Ring of Righteousness";
            ring.Hue = Utility.Random(1157, 1161);
            ring.Attributes.BonusStr = 2;
            ring.Attributes.BonusDex = 2;
            ring.Attributes.BonusInt = 2;
            ring.Attributes.LowerManaCost = 5;
            ring.SkillBonuses.SetValues(0, SkillName.Chivalry, 10.0);
            return ring;
        }

        private Item CreateChivalryShield()
        {
            WoodenShield shield = new WoodenShield();
            shield.Name = "Shield of the Just";
            shield.Hue = Utility.Random(1157, 1161);
            shield.Attributes.BonusStr = 5;
            shield.Attributes.BonusDex = 5;
            shield.Attributes.BonusInt = 5;
            shield.Attributes.LowerManaCost = 15;
            shield.SkillBonuses.SetValues(0, SkillName.Chivalry, 25.0);
            return shield;
        }

        public SpecialChivalryChest(Serial serial) : base(serial)
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
