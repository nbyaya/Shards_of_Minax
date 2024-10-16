using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RevolutionaryChess : WoodenChest
    {
        [Constructable]
        public RevolutionaryChess()
        {
            Name = "Revolutionary Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateDiamond(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Patriot’s Wine", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Founding Fathers’ Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Eagle Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of Independence", 1157), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Benjamin Franklin’s Favorite"), 0.08);
            AddItem(CreateGoldItem("Continental Dollar"), 0.16);
            AddItem(CreateColoredItem<TricorneHat>("Hat of the Minuteman", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Snake Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Crossbow>("Paul Revere’s Musket"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Tea"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreateLeatherGloves(), 0.20);
            AddItem(CreateWarMace(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateDiamond()
        {
            Diamond diamond = new Diamond();
            diamond.Name = "Diamond of Liberty";
            return diamond;
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
            note.NoteString = "Give me liberty or give me death!";
            note.TitleString = "Patrick Henry’s Speech";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Secret Bunker";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "The Liberty Bell";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Sons of Liberty";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Mapmaker's Insightful Muffler";
            muffler.Hue = Utility.RandomMinMax(320, 1320);
            muffler.Attributes.BonusInt = 10;
            muffler.Attributes.Luck = 10;
            muffler.SkillBonuses.SetValues(0, SkillName.Cartography, 20.0);
            muffler.SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
            return muffler;
        }

        private Item CreateLeatherGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Vyr's Grasping Gauntlets";
            gloves.Hue = Utility.RandomMinMax(150, 450);
            gloves.BaseArmorRating = Utility.Random(30, 60);
            gloves.AbsorptionAttributes.EaterEnergy = 10;
            gloves.ArmorAttributes.MageArmor = 1;
            gloves.Attributes.BonusMana = 50;
            gloves.Attributes.SpellDamage = 20;
            gloves.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            gloves.EnergyBonus = 20;
            gloves.FireBonus = 10;
            gloves.PhysicalBonus = 10;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateWarMace()
        {
            WarMace warMace = new WarMace();
            warMace.Name = "Teutonic WarMace";
            warMace.Hue = Utility.RandomMinMax(200, 400);
            warMace.MinDamage = Utility.Random(30, 80);
            warMace.MaxDamage = Utility.Random(80, 120);
            warMace.Attributes.DefendChance = 15;
            warMace.Attributes.BonusHits = 10;
            warMace.Slayer = SlayerName.OrcSlaying;
            warMace.WeaponAttributes.HitManaDrain = 20;
            warMace.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            return warMace;
        }

        public RevolutionaryChess(Serial serial) : base(serial)
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
