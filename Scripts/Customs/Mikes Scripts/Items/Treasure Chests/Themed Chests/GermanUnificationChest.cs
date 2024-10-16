using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GermanUnificationChest : WoodenChest
    {
        [Constructable]
        public GermanUnificationChest()
        {
            Name = "Treasure Chest of the German Unification";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Sapphire of the Iron Chancellor"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Bismarck’s Schnapps", 1486), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("German Unity Treasure"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Pickelhaube Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Franco-Prussian War", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Kaiser Wilhelm’s Champagne"), 0.08);
            AddItem(CreateGoldItem("Golden Reichsmark"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Iron Cross", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Eagle Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Von Moltke’s Spyglass"), 0.13);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Bottle of Mustard Gas"), 0.20);
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

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "The question of the German nationality is not a question of the present or of the future but a question of the past.";
            note.TitleString = "Otto von Bismarck’s Speech";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Berlin Palace";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateWeapon()
        {
            Crossbow weapon = new Crossbow(); // Assuming LugerPistol is a valid type
            weapon.Name = "Luger Pistol";
            weapon.Hue = Utility.Random(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70); // Adjust as needed
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = new CloseHelm(); // Assuming PickelhaubeHelmet is a valid type
            armor.Name = "Pickelhaube Helmet";
            armor.Hue = Utility.Random(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70); // Adjust as needed
            return armor;
        }

        public GermanUnificationChest(Serial serial) : base(serial)
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
