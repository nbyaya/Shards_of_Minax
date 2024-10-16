using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BakersDelightChest : WoodenChest
    {
        [Constructable]
        public BakersDelightChest()
        {
            Name = "Baker’s Delight";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Cookie"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("French Champagne", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Baker’s Secret"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of the Pastry Chef"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Pearl of the Oyster", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Port Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Croissant"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Baker", 1175), 0.19);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Necklace of the Master Baker"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Baker’s Eye Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Vanilla Extract"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateWizardHat(), 0.30);
            AddItem(CreateMace(), 0.30);
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
            note.NoteString = "This is a recipe for the famous chocolate cake that won the Grand Baking Contest of 2023";
            note.TitleString = "Chocolate Cake Recipe";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Hidden Bakery";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Dagger weapon = new Dagger();
            weapon.Name = "Baker’s Knife";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            FullApron armor = new FullApron();
            armor.Name = "Baker’s Apron";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.SkillBonuses.SetValues(0, SkillName.Cooking, 40.0);
            return armor;
        }

        private Item CreateWizardHat()
        {
            StrawHat hat = new StrawHat();
            hat.Name = "Hat of the Grand Champion";
            hat.Hue = Utility.RandomMinMax(600, 1600);
            hat.SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            return hat;
        }

        private Item CreateMace()
        {
            Mace mace = new Mace();
            mace.Name = "Pan of Justice";
            mace.Hue = Utility.RandomMinMax(1, 1000);
            mace.MinDamage = Utility.Random(30, 80);
            mace.MaxDamage = Utility.Random(80, 120);
            mace.Attributes.BonusStr = 10;
            mace.Attributes.SpellDamage = 5;
            mace.WeaponAttributes.HitFireball = 30;
            mace.WeaponAttributes.SelfRepair = 5;
            mace.SkillBonuses.SetValues(0, SkillName.Cooking, 25.0);
            return mace;
        }

        public BakersDelightChest(Serial serial) : base(serial)
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
