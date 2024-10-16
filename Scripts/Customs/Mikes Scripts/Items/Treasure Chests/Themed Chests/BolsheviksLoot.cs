using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BolsheviksLoot : WoodenChest
    {
        [Constructable]
        public BolsheviksLoot()
        {
            Name = "Bolshevik’s Loot";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateAmber(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Vodka of the People", 1175), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Bolshevik’s Red Treasure"), 0.17);
            AddItem(CreateNamedItem<SilverBracelet>("Silver Bracelet of Lenin"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Topaz of the Soviet Union", 1173), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Stalin’s Secret Stash"), 0.08);
            AddItem(CreateGoldItem("Golden Hammer and Sickle"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Red Army", 1176), 0.19);
            AddItem(CreateNamedItem<SilverEarrings>("Silver Earrings of Trotsky"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Lenin’s Magnifying Glass"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Antidote"), 0.20);
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

        private Item CreateAmber()
        {
            Amber amber = new Amber();
            amber.Name = "Amber of the Revolution";
            return amber;
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
            note.NoteString = "The proletarians have nothing to lose but their chains.";
            note.TitleString = "The Communist Manifesto";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Kremlin’s Armory";
            map.Bounds = new Rectangle2D(2000, 2000, 400, 400);
            map.NewPin = new Point2D(2100, 2150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Crossbow()); // Replace with a more appropriate weapon if needed
            weapon.Name = "Kalashnikov";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherArms(), new LeatherLegs(), new LeatherCap()); // Replace with appropriate armor if needed
            armor.Name = "Bolshevik’s Pride";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        public BolsheviksLoot(Serial serial) : base(serial)
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
