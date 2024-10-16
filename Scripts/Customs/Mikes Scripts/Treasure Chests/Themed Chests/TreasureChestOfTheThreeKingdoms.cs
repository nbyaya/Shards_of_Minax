using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TreasureChestOfTheThreeKingdoms : WoodenChest
    {
        [Constructable]
        public TreasureChestOfTheThreeKingdoms()
        {
            Name = "Treasure Chest of the Three Kingdoms";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Sapphire of the Silla"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Goguryeo’s Finest Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Baekje’s Royal Treasure"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Dragon Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Balhae", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Silla’s Mead"), 0.08);
            AddItem(CreateGoldItem("Golden Crown of Gaya"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Hwarang", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Phoenix Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Balhae’s Naval Spyglass"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of Goguryeo’s Poison"), 0.20);
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
            note.NoteString = "We have conquered the lands of our enemies and unified the peninsula!";
            note.TitleString = "King Munmu’s Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Tomb of King Muryeong";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Hwangnyongsa’s Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Balhae’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        public TreasureChestOfTheThreeKingdoms(Serial serial) : base(serial)
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
