using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialWoodenChestTomoe : WoodenChest
    {
        [Constructable]
        public SpecialWoodenChestTomoe()
        {
            Name = "Tomoe’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Koban"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fine Sake", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Tomoe’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Tomoe"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Minamoto Clan", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Sake"), 0.08);
            AddItem(CreateGoldItem("Ryo"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Samurai", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of Amaterasu"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Tomoe’s Sharp-eyed Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Shinto Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateKimono(), 0.30);
            AddItem(CreateKatana(), 0.30);
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
            note.NoteString = "I am Tomoe Gozen, the female samurai who fought in the Genpei War. I served under Minamoto no Yoshinaka and was his lover and one of his finest warriors. I was skilled in archery, swordsmanship, and horsemanship. I killed many enemies and took many heads as trophies. I survived the war and became a nun in later life.";
            note.TitleString = "Tomoe’s Memoir";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Tomoe’s Grave";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            Dagger weapon = new Dagger();
            weapon.Name = "Tomoe’s Naginata";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Tomoe’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateKimono()
        {
            FancyDress kimono = new FancyDress();
            kimono.Name = "Kimono of the Samurai";
            kimono.Hue = Utility.Random(1, 1788);
            kimono.Attributes.BonusStr = 10;
            kimono.SkillBonuses.SetValues(0, SkillName.Swords, 10.0);
            kimono.ClothingAttributes.DurabilityBonus = 5;
			kimono.SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
            return kimono;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Masamune’s Blade";
            katana.Hue = Utility.Random(1, 1788);
            katana.MinDamage = Utility.Random(30, 80);
            katana.MaxDamage = Utility.Random(80, 120);
            katana.Attributes.BonusDex = 10;
            katana.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return katana;
        }

        public SpecialWoodenChestTomoe(Serial serial) : base(serial)
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
