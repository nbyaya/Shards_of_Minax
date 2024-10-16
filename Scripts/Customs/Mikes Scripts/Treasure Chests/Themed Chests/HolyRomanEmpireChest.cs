using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class HolyRomanEmpireChest : WoodenChest
    {
        [Constructable]
        public HolyRomanEmpireChest()
        {
            Name = "Treasure Chest of the Holy Roman Empire";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateSapphire(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Charlemagne’s Wine", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Imperial Treasure"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Eagle Pendant"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Holy Lance", 1775), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Martin Luther’s Beer", 0), 0.08);
            AddItem(CreateGoldItem("Golden Thaler"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Crusader", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Cross Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Frederick Barbarossa’s Spyglass"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of the Black Death"), 0.20);
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

        private Item CreateSapphire()
        {
            Sapphire sapphire = new Sapphire();
            sapphire.Name = "Sapphire of the Imperial Crown";
            return sapphire;
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
            note.NoteString = "The empire is in danger! We must defend it from the invaders!";
            note.TitleString = "Otto I’s Letter";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Imperial Vault";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Teutonic Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Imperial Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        public HolyRomanEmpireChest(Serial serial) : base(serial)
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
