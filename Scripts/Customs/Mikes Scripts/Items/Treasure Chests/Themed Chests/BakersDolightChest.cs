using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BakersDolightChest : WoodenChest
    {
        [Constructable]
        public BakersDolightChest()
        {
            Name = "Baker's Delight";
            Hue = Utility.Random(1, 1500);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Golden Baker's Coins"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fine Vanilla Extract", 1159), 0.14);
            AddItem(CreateNamedItem<TreasureLevel2>("Secret Recipe Box"), 0.16);
            AddItem(CreateNamedItem<GoldBracelet>("Baker's Lucky Bracelet"), 0.48);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 2500)), 0.15);
            AddItem(CreateNamedItem<ApplePie>("Grandma's Apple Pie"), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old Yeast Bottle", 1159), 0.09);
            AddItem(CreateGoldItem("Baker's Guild Token"), 0.17);
            AddItem(CreateNamedItem<Garlic>("Premium Olive Oil"), 0.18);
            AddItem(CreateNamedItem<GoldRing>("Ring of the Master Baker"), 0.16);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<RollingPin>("Enchanted Rolling Pin"), 0.12);
            AddItem(CreateNamedItem<WhiteRose>("Flour Blossom"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of Baking Mastery"), 0.20);
            AddItem(CreateWeapon(), 0.19);
            AddItem(CreateNamedItem<Longsword>("Magic Taco"), 0.28);
            AddItem(CreateCookies(), 0.29);
            AddItem(CreateMuffins(), 0.27);
            AddItem(CreateCake(), 0.27);
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
            note.NoteString = "Bake with love, and the world will love you back!";
            note.TitleString = "Baking Wisdom";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Legendary Bakery";
            map.Bounds = new Rectangle2D(2500, 2500, 700, 700);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new ShortSpear();
            weapon.Name = "Baker's Wooden Spoon";
            weapon.Hue = Utility.RandomList(1, 1500);
            weapon.MaxDamage = Utility.Random(10, 40);
            return weapon;
        }

        private Item CreateCookies()
        {
            FancyShirt cookies = new FancyShirt();
            cookies.Name = "Cookies Shirt";
            cookies.Hue = Utility.RandomList(1, 1500);
            cookies.SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
            return cookies;
        }

        private Item CreateMuffins()
        {
            PlateGloves muffins = new PlateGloves();
            muffins.Name = "Oven Mitts";
            muffins.Hue = Utility.RandomList(1, 1000);
            muffins.BaseArmorRating = Utility.Random(20, 50);
            muffins.AbsorptionAttributes.EaterFire = 20;
            muffins.ArmorAttributes.ReactiveParalyze = 1;
            muffins.Attributes.BonusDex = 10;
            muffins.Attributes.AttackChance = 5;
            muffins.SkillBonuses.SetValues(0, SkillName.Cooking, 15.0);
            muffins.FireBonus = 20;
            muffins.PhysicalBonus = 15;
            muffins.PoisonBonus = 10;
            return muffins;
        }

        private Item CreateCake()
        {
            Mace cake = new Mace();
            cake.Name = "Magical Layer Cake";
            cake.Hue = Utility.RandomMinMax(25, 200);
            cake.MinDamage = Utility.Random(15, 50);
            cake.MaxDamage = Utility.Random(50, 90);
            cake.Attributes.BonusDex = 7;
            cake.Attributes.SpellDamage = 3;
            cake.WeaponAttributes.HitMagicArrow = 20;
            cake.WeaponAttributes.SelfRepair = 3;
            cake.SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            return cake;
        }

        public BakersDolightChest(Serial serial) : base(serial)
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
