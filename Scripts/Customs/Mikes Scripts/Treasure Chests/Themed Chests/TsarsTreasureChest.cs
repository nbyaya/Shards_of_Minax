using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TsarsTreasureChest : WoodenChest
    {
        [Constructable]
        public TsarsTreasureChest()
        {
            Name = "Tsar's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Tsar's Golden Coin"), 0.16);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Romanovs", 0), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Tsar's Finest Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Tsar's Crown Jewels"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Catherine the Great"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Kremlin", 1153), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Vintage Crimean Wine"), 0.08);
            AddItem(CreateColoredItem<FurBoots>("Boots of the Winter Palace", 1150), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Ivan the Terrible"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Tsar's Royal Telescope"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            if (hue != 0)
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
            note.NoteString = "I have abdicated the throne and left this treasure for my loyal followers.";
            note.TitleString = "Tsar Nicholas II’s Last Letter";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Tsar’s Secret Vault";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Peter the Great";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Tsar’s Guard";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        public TsarsTreasureChest(Serial serial) : base(serial)
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
