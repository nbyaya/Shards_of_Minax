using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class KingsBest : WoodenChest
    {
        [Constructable]
        public KingsBest()
        {
            Name = "King's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateDiamond("Diamond of the Crown"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Royal Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("King's Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Eagle Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Realm", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Mead"), 0.08);
            AddItem(CreateGoldItem("Golden Zloty"), 0.16);
            AddItem(CreateColoredItem<PlateGorget>("Gorget of the Defender", 2126), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Lion Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Telescope>("King's Trusted Telescope"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Royal Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateDiamond(string name)
        {
            Diamond diamond = new Diamond();
            diamond.Name = name;
            return diamond;
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "I hereby decree that this chest belongs to me and my heirs forever.";
            note.TitleString = "King Sigismund III Vasa’s Decree";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Royal Treasury";
            map.Bounds = new Rectangle2D(5000, 5000, 400, 400);
            map.NewPin = new Point2D(5100, 5150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Szczerbiec";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "King's Best";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        public KingsBest(Serial serial) : base(serial)
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
