using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EuropeanRelicsChest : WoodenChest
    {
        [Constructable]
        public EuropeanRelicsChest()
        {
            Name = "European Relics";
            Hue = Utility.Random(1, 1789);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("European Ducats"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("French Bordeaux", 1169), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Knight's Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of the Renaissance"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("German Beer Stein"), 0.08);
            AddItem(CreateGoldItem("European Guilders"), 0.16);
            AddItem(CreateColoredItem<Clock>("Gothic Grandfather Clock", 1187), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Signet Ring of the Vatican"), 0.17);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Explorer's Monocular"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of European Vitality"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateElegantArmoire(), 0.30);
            AddItem(CreateBooks(), 0.30);
            AddItem(CreateLongsword(), 0.30);
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
            note.NoteString = "From the heart of Europe, where history and culture intertwine";
            note.TitleString = "European Chronicle";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Louvre";
            map.Bounds = new Rectangle2D(2500, 2500, 700, 700);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "European Longsword";
            weapon.Hue = Utility.RandomList(1, 1789);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Armor of European Crusaders";
            armor.Hue = Utility.RandomList(1, 1789);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateElegantArmoire()
        {
            FancyShirt armoire = new FancyShirt();
            armoire.Name = "Renaissance Wardrobe";
            armoire.Hue = Utility.RandomMinMax(601, 1601);
            armoire.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return armoire;
        }

        private Item CreateBooks()
        {
            PlateGloves books = new PlateGloves();
            books.Name = "Gloves of Shakespeare";
            books.Hue = Utility.RandomMinMax(1, 1001);
            books.BaseArmorRating = Utility.Random(60, 90);
            books.AbsorptionAttributes.EaterFire = 30;
            books.ArmorAttributes.ReactiveParalyze = 1;
            books.Attributes.BonusDex = 20;
            books.Attributes.AttackChance = 10;
            books.SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
            books.ColdBonus = 20;
            books.EnergyBonus = 20;
            books.FireBonus = 20;
            books.PhysicalBonus = 25;
            books.PoisonBonus = 25;
            return books;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Excalibur Replica";
            longsword.Hue = Utility.RandomMinMax(51, 251);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.WeaponAttributes.HitLightning = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public EuropeanRelicsChest(Serial serial) : base(serial)
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
