using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BuccaneersChest : WoodenChest
    {
        [Constructable]
        public BuccaneersChest()
        {
            Name = "Buccaneer's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Fire", 2117), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Buccaneer's Brew", 2116), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Buccaneer's Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Skull Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Topaz of the Flame", 2118), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Fiery Buccaneer's Rum", 2116), 0.08);
            AddItem(CreateGoldItem("Burning Doubloon"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Raider", 2119), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Dagger Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Ruby>("Captain's Trusty Compass"), 0.10);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Antidote"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateCap(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateBattleAxe(), 0.20);
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
            note.NoteString = "Yarr! Ye have stumbled upon me treasure! But don’t ye dare touch it or ye’ll face me wrath!";
            note.TitleString = "Captain Buccaneer's Threat";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Buccaneer's Hidden Cave";
            map.Bounds = new Rectangle2D(7000, 7200, 600, 600);
            map.NewPin = new Point2D(7100, 7150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new BattleAxe());
            weapon.Name = "The Scourge";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Buccaneer's Glory";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Concoction Curator's Cap";
            cap.Hue = Utility.RandomMinMax(100, 1100);
            cap.Attributes.CastSpeed = 1;
            cap.Attributes.Luck = 20;
            cap.SkillBonuses.SetValues(0, SkillName.Alchemy, 25.0);
            return cap;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs legs = new PlateLegs();
            legs.Name = "Blaze PlateLegs";
            legs.Hue = Utility.RandomMinMax(500, 600);
            legs.BaseArmorRating = Utility.Random(45, 80);
            legs.AbsorptionAttributes.ResonanceFire = 20;
            legs.Attributes.BonusDex = 25;
            legs.FireBonus = 25;
            legs.EnergyBonus = 10;
            legs.PoisonBonus = -5;
            legs.PhysicalBonus = 15;
            return legs;
        }

        private Item CreateBattleAxe()
        {
            BattleAxe axe = new BattleAxe();
            axe.Name = "Rune Axe";
            axe.Hue = Utility.RandomMinMax(500, 700);
            axe.MinDamage = Utility.Random(30, 60);
            axe.MaxDamage = Utility.Random(60, 100);
            axe.Attributes.SpellChanneling = 1;
            axe.Attributes.BonusMana = 10;
            axe.Slayer = SlayerName.ElementalHealth;
            axe.WeaponAttributes.HitLightning = 35;
            axe.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return axe;
        }

        public BuccaneersChest(Serial serial) : base(serial)
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
