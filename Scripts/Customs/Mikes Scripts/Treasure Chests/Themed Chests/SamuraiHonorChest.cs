using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SamuraiHonorChest : WoodenChest
    {
        [Constructable]
        public SamuraiHonorChest()
        {
            Name = "Samurai's Honor";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Yen Coins"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Sake of the Shogunate", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Ninja's Secret Stash"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Koi Bracelet of Kyoto"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<Painting4NorthArtifact>("Ukiyo-e Painting", 2119), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Green Tea", 0), 0.08); // BottleArtifact with no hue
            AddItem(CreateGoldItem("Edo Period Gold Koban"), 0.15);
            AddItem(CreateNamedItem<GoldRing>("Cherry Blossom Ring"), 0.18);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Samurai's Navigational Tool"), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of Bushido"), 0.22);
            AddItem(CreateWeapon(), 0.22);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateFan(), 0.32);
            AddItem(CreateFancyDrawer(), 0.30);
            AddItem(CreateBow(), 0.30);
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
            note.NoteString = "In homage to the Land of the Rising Sun";
            note.TitleString = "Japanese Proverb";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Mt. Fuji";
            map.Bounds = new Rectangle2D(2500, 2500, 800, 800);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Katana());
            weapon.Name = "Katana of the Legendary Samurai";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            Robe armor = new Robe();
            armor.Name = "Kimono of the Geisha";
            armor.Hue = Utility.RandomList(1, 1788);
            return armor;
        }

        private Item CreateFan()
        {
            Tessen fan = new Tessen();
            fan.Name = "Fan of the Daimyo";
            fan.Hue = Utility.RandomMinMax(600, 1600);
            fan.Attributes.DefendChance = 10;
            fan.SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
            return fan;
        }

        private Item CreateFancyDrawer()
        {
            FancyShirt drawer = new FancyShirt();
            drawer.Name = "Japanese Tansu Chest";
            drawer.Hue = Utility.RandomMinMax(1, 1000);
            drawer.Attributes.BonusDex = 20;
            drawer.Attributes.BonusMana = 15;
            drawer.SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0);
            return drawer;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Yumi of the Archer";
            bow.Hue = Utility.RandomMinMax(50, 250);
            bow.MinDamage = Utility.Random(30, 80);
            bow.MaxDamage = Utility.Random(80, 120);
            bow.Attributes.BonusDex = 10;
            bow.Attributes.RegenHits = 3;
            bow.WeaponAttributes.HitLightning = 30;
            bow.WeaponAttributes.SelfRepair = 5;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
            return bow;
        }

        public SamuraiHonorChest(Serial serial) : base(serial)
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
