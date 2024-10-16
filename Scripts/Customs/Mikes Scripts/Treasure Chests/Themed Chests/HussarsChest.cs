using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class HussarsChest : WoodenChest
    {
        [Constructable]
        public HussarsChest()
        {
            Name = "Hussar's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Diamond>("Topaz of the Winged"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Hussar's Vodka", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Hussar's Loot"), 0.17);
            AddItem(CreateNamedItem<SilverBracelet>("Silver Cross Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 7500)), 0.15);
            AddItem(CreateColoredItem<Amethyst>("Amethyst of the Brave", 2129), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old Brandy", 1153), 0.08);
            AddItem(CreateGoldItem("Silver Florin"), 0.16);
            AddItem(CreateColoredItem<PlateHelm>("Helm of the Winged", 1154), 0.17);
            AddItem(CreateColoredItem<FeatheredHat>("Hat of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateColoredItem<PlateArms>("Arms of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateColoredItem<PlateLegs>("Legs of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateColoredItem<PlateChest>("Chest of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateColoredItem<PlateGloves>("Gloves of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateColoredItem<Cloak>("Cloak of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateColoredItem<Boots>("Boots of the Winged", Utility.RandomMinMax(1154, 1166)), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Eagle Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Hussar's Trusted Compass"), 0.13);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Hussar's Remedy"), 0.20);
            AddRandomWeapons();
            AddRandomArmor();
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private void AddRandomWeapons()
        {
            for (int i = 0; i < Utility.RandomMinMax(1, 3); i++)
            {
                BaseWeapon weapon = Utility.RandomList<BaseWeapon>(
                    new Longsword { Name = "Koncierz", Hue = Utility.RandomList(1, 1788), MaxDamage = Utility.Random(30, 70) },
                    new Longsword { Name = "Karabela", Hue = Utility.RandomList(1, 1788), MaxDamage = Utility.Random(30, 70) },
                    new Longsword { Name = "Sabre", Hue = Utility.RandomList(1, 1788), MaxDamage = Utility.RandomMinMax(30, 70) }
                );
                DropItem(weapon);
            }
        }

        private void AddRandomArmor()
        {
            for (int i = 0; i < Utility.RandomMinMax(1, 3); i++)
            {
                BaseArmor armor = Utility.RandomList<BaseArmor>(
                    new PlateChest { Name = "Hussar's Best", Hue = Utility.RandomMinMax(1154, 1166), BaseArmorRating = Utility.Random(30, 70) },
                    new PlateChest { Name = "Hussar's Shield", Hue = Utility.RandomMinMax(1154, 1166), BaseArmorRating = Utility.Random(30, 70) }
                );
                DropItem(armor);
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
            note.NoteString = "For God and country we fight and die.";
            note.TitleString = "Hussar's Motto";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Hussar's Camp";
            map.Bounds = new Rectangle2D(6000, 6000, 400, 400);
            map.NewPin = new Point2D(6100, 6150);
            map.Protected = true;
            return map;
        }

        public HussarsChest(Serial serial) : base(serial)
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
