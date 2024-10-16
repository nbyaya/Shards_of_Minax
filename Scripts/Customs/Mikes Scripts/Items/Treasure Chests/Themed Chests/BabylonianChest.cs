using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BabylonianChest : WoodenChest
    {
        [Constructable]
        public BabylonianChest()
        {
            Name = "Babylon's Bounty";
            Hue = Utility.Random(1, 1789);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Babylonian Shekels"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Beer of Babylon", 1160), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Cuneiform Relics"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Nebuchadnezzar"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateNamedItem<GreaterHealPotion>("Date Wine of Mesopotamia"), 0.09);
            AddItem(CreateGoldItem("Babylonian Trade Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Euphrates", 1177), 0.18);
            AddItem(CreateNamedItem<GoldRing>("Ring of Marduk"), 0.18);
            AddItem(CreateMap(), 0.06);
            AddItem(CreateNamedItem<Spyglass>("Astronomer's Lens of Babylon"), 0.14);
            AddItem(CreateNamedItem<GreaterHealPotion>("Elixir of the Tigris"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateArcticBeacon(), 0.30);
            AddItem(CreateRobeOfTheDragon(), 0.30);
            AddItem(CreateBladeOfTheValkyrie(), 0.30);
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
            note.NoteString = "From the Hanging Gardens to the Tower of Babel, Babylon's wonders are many";
            note.TitleString = "Babylonian Chronicles";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Gates of Ishtar";
            map.Bounds = new Rectangle2D(3000, 3000, 700, 700);
            map.NewPin = new Point2D(3200, 3200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger());
            weapon.Name = "Dagger of Gilgamesh";
            weapon.Hue = Utility.RandomList(1, 1789);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherArms(), new LeatherLegs(), new LeatherCap());
            armor.Name = "Enkidu's Hide";
            armor.Hue = Utility.RandomList(1, 1789);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateArcticBeacon()
        {
            Kilt beacon = new Kilt();
            beacon.Name = "Model Ziggurat";
            beacon.Hue = Utility.RandomMinMax(600, 1601);
            beacon.ClothingAttributes.DurabilityBonus = 5;
            beacon.Attributes.DefendChance = 10;
            beacon.SkillBonuses.SetValues(0, SkillName.Cartography, 20.0);
            return beacon;
        }

        private Item CreateRobeOfTheDragon()
        {
            Robe robe = new Robe();
            robe.Name = "Robe of the Dragon";
            robe.Hue = Utility.RandomMinMax(1, 2901);
            robe.Attributes.BonusInt = 20;
            robe.Attributes.AttackChance = 10;
            robe.SkillBonuses.SetValues(0, SkillName.Magery, 80.0);
            return robe;
        }

        private Item CreateBladeOfTheValkyrie()
        {
            BoneHarvester blade = new BoneHarvester();
            blade.Name = "Lugal's Sickle";
            blade.Hue = Utility.RandomMinMax(50, 251);
            blade.MinDamage = Utility.Random(30, 80);
            blade.MaxDamage = Utility.Random(80, 120);
            blade.Attributes.BonusStr = 10;
            blade.Attributes.SpellDamage = 5;
            blade.WeaponAttributes.HitFireArea = 30;
            blade.WeaponAttributes.SelfRepair = 5;
            blade.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return blade;
        }

        public BabylonianChest(Serial serial) : base(serial)
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
