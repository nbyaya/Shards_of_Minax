using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class NinjaChest : WoodenChest
    {
        [Constructable]
        public NinjaChest()
        {
            Name = "Ninja Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Shuriken>("Ninja Star"), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Sake"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Ninja’s Loot"), 0.17);
            AddItem(CreateNamedItem<SilverRing>("Silver Dragon Ring"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateNamedItem<SmokeBomb>("Ninja’s Escape"), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Ninja’s Sake"), 0.08);
            AddItem(CreateGoldItem("Ninja Coin"), 0.16);
            AddItem(CreateColoredItem<LeatherNinjaHood>("Hood of the Shadow", 1109), 0.19);
            AddItem(CreateColoredItem<LeatherNinjaJacket>("Jacket of the Shadow", 1109), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Kama>("Ninja’s Blade"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Brew"), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreateStuddedChest(), 0.20);
            AddItem(CreateWarAxe(), 0.20);
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
            note.NoteString = "The ninja code is the way of the shadow.";
            note.TitleString = "Ninja’s Secret";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Ninja’s Hideout";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherNinjaHood(), new LeatherNinjaJacket());
            armor.Name = "Ninja’s Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Woodworker's Insightful Cap";
            cap.Hue = Utility.RandomMinMax(500, 1400);
            cap.Attributes.BonusInt = 10;
            cap.Attributes.CastSpeed = 1;
            cap.SkillBonuses.SetValues(0, SkillName.Carpentry, 20.0);
            return cap;
        }

        private Item CreateStuddedChest()
        {
            StuddedChest chest = new StuddedChest();
            chest.Name = "Tabula Rasa";
            chest.Hue = Utility.RandomMinMax(900, 950);
            chest.BaseArmorRating = Utility.Random(25, 65);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateWarAxe()
        {
            WarAxe axe = new WarAxe();
            axe.Name = "Bul-Kathos' Tribal Guardian";
            axe.Hue = Utility.RandomMinMax(100, 300);
            axe.MinDamage = Utility.Random(30, 70);
            axe.MaxDamage = Utility.Random(70, 110);
            axe.Attributes.BonusStr = 10;
            axe.Attributes.RegenStam = 3;
            axe.Slayer = SlayerName.OrcSlaying;
            axe.WeaponAttributes.HitFireball = 30;
            axe.WeaponAttributes.BattleLust = 20;
            axe.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            return axe;
        }

        public NinjaChest(Serial serial) : base(serial)
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
