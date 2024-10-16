using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EmperorJustinianCache : WoodenChest
    {
        [Constructable]
        public EmperorJustinianCache()
        {
            Name = "Emperor Justinian's Cache";
            Hue = Utility.Random(1, 1688);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Byzantine Solidus"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Byzantine Chalice Wine", 1150), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Justinian’s Archive"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of Theodora"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of Constantinople", 2110), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Chrysotriklinos Elixir"), 0.08);
            AddItem(CreateGoldItem("Golden Nomisma"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of Procopius", 1165), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Ring of Hagia Sophia"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Justinian’s Insightful Monocle"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Nika Riot Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateBoots(), 0.30);
            AddItem(CreateGauntlets(), 0.30);
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
            note.NoteString = "I am Justinian, Emperor of the Byzantines, ruler of the East and West, protector of the Roman Empire";
            note.TitleString = "Justinian's Proclamation";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to The Byzantine Palace";
            map.Bounds = new Rectangle2D(2000, 2000, 500, 500);
            map.NewPin = new Point2D(2200, 2200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Blade of the Basilica";
            weapon.Hue = Utility.RandomList(1, 1688);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Byzantine Lamellar";
            armor.Hue = Utility.RandomList(1, 1688);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateBoots()
        {
            ThighBoots boots = new ThighBoots();
            boots.Name = "Boots of Belisarius";
            boots.Hue = Utility.RandomMinMax(590, 1500);
            boots.ClothingAttributes.DurabilityBonus = 5;
            boots.Attributes.DefendChance = 10;
            boots.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return boots;
        }

        private Item CreateGauntlets()
        {
            PlateGloves gauntlets = new PlateGloves();
            gauntlets.Name = "Gauntlets of Iconoclasm";
            gauntlets.Hue = Utility.RandomMinMax(1, 990);
            gauntlets.BaseArmorRating = Utility.Random(60, 90);
            gauntlets.AbsorptionAttributes.EaterPoison = 30;
            gauntlets.ArmorAttributes.ReactiveParalyze = 1;
            gauntlets.Attributes.BonusDex = 20;
            gauntlets.Attributes.AttackChance = 10;
            gauntlets.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            gauntlets.ColdBonus = 20;
            gauntlets.EnergyBonus = 20;
            gauntlets.FireBonus = 20;
            gauntlets.PhysicalBonus = 25;
            gauntlets.PoisonBonus = 25;
            return gauntlets;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Constantine's Defender";
            longsword.Hue = Utility.RandomMinMax(45, 245);
            longsword.MinDamage = Utility.Random(30, 80);
            longsword.MaxDamage = Utility.Random(80, 120);
            longsword.Attributes.BonusStr = 10;
            longsword.Attributes.SpellDamage = 5;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitFireball = 30;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return longsword;
        }

        public EmperorJustinianCache(Serial serial) : base(serial)
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
