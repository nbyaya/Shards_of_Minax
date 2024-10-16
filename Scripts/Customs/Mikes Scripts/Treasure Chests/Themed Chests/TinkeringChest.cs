using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TinkeringTreasureChest : WoodenChest
    {
        [Constructable]
        public TinkeringTreasureChest()
        {
            Name = "Chest of the Master Tinkerer";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateTinkeringItem<GoldBracelet>("Tinkering Bracelet"), 0.50);
            AddItem(CreateTinkeringItem<GoldRing>("Tinkering Ring"), 0.50);
            AddItem(CreateTinkeringItem<ThighBoots>("Tinkering Boots"), 0.50);
            AddItem(CreateTinkeringItem<PlateGloves>("Tinkering Gauntlets"), 0.50);
            AddItem(CreateTinkeringHat(), 0.50);
            AddItem(CreateTinkeringItem<Cloak>("Tinkering Cloak"), 0.50);
            AddItem(CreateTinkeringItem<LeatherArms>("Tinkering Leather Arms"), 0.50);
            AddItem(CreateTinkeringItem<LeatherLegs>("Tinkering Leather Legs"), 0.50);
            AddItem(CreateTinkeringItem<LeatherGorget>("Tinkering Leather Gorget"), 0.50);
            AddItem(CreateTinkeringItem<LeatherChest>("Tinkering Leather Chest"), 0.50);
            AddItem(new Gold(Utility.Random(1000, 5000)), 1.0);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateTinkeringItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = Utility.Random(1, 1788);

            // Add attributes if applicable
            AddTinkeringBonus(item);

            return item;
        }

        private void AddTinkeringBonus(Item item)
        {
            if (item is BaseClothing)
            {
                BaseClothing clothing = item as BaseClothing;
                clothing.Attributes.BonusDex = 5;
                clothing.SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.Random(5, 20));
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = item as BaseArmor;
                armor.Attributes.BonusDex = 5;
                armor.SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.Random(5, 20));
            }
            else if (item is BaseJewel)
            {
                BaseJewel jewel = item as BaseJewel;
                jewel.Attributes.BonusDex = 5;
                jewel.SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.Random(5, 20));
            }
        }

        private Item CreateTinkeringHat()
        {
            Cap hat = new Cap();
            hat.Name = "Tinkering Hat";
            hat.Hue = Utility.Random(1, 1788);
            hat.Attributes.BonusDex = 5;
            hat.SkillBonuses.SetValues(0, SkillName.Tinkering, Utility.Random(5, 20));
            return hat;
        }

        public TinkeringTreasureChest(Serial serial) : base(serial)
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
