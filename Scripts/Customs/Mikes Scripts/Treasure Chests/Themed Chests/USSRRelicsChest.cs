using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class USSRRelicsChest : WoodenChest
    {
        [Constructable]
        public USSRRelicsChest()
        {
            Name = "USSR Relics";
            Hue = Utility.Random(1, 1789);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateGoldItem("Soviet Rubles"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Stalingrad Brandy", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Comrade's Cache"), 0.18);
            AddItem(CreateNamedItem<GoldBracelet>("Bracelet of the Proletariat"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.16);
            AddItem(CreateColoredItem<GreaterHealPotion>("Soviet Kvas", 1159), 0.09);
            AddItem(CreateGoldItem("Red Star Coin"), 0.17);
            AddItem(CreateColoredItem<StrawHat>("Ushanka of the Red Army", 1177), 0.20);
            AddItem(CreateNamedItem<GoldRing>("Order of Lenin Medal"), 0.18);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Spyglass>("Sputnik's Viewfinder"), 0.14);
            AddItem(CreateNamedItem<RedArmoire>("Soviet State Archive"), 0.15);
            AddItem(CreatePotion(), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.30);
            AddItem(CreateRedRose(), 0.30);
            AddItem(CreateClock(), 0.30);
            AddItem(CreateHammer(), 0.30);
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
            note.NoteString = "For the Workers and Farmers of the Soviet Union";
            note.TitleString = "Soviet Manifesto";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Kremlin";
            map.Bounds = new Rectangle2D(2500, 2500, 800, 800);
            map.NewPin = new Point2D(2700, 2700);
            map.Protected = true;
            return map;
        }

        private Item CreatePotion()
        {
            GreaterHealPotion potion = new GreaterHealPotion();
            potion.Name = "Red Square Elixir";
            return potion;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new ShepherdsCrook();
            weapon.Name = "Sickle of Revolution";
            weapon.Hue = Utility.RandomList(1, 1789);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new CloseHelm(), new PlateLegs(), new PlateChest(), new PlateHelm());
            armor.Name = "Comrade's Uniform";
            armor.Hue = Utility.RandomList(1, 1789);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateRedRose()
        {
            Robe rose = new Robe();
            rose.Name = "Rose of the Revolution";
            rose.Hue = Utility.RandomMinMax(600, 1601);
            rose.ClothingAttributes.DurabilityBonus = 5;
            rose.Attributes.DefendChance = 10;
            rose.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            return rose;
        }

        private Item CreateClock()
        {
            PlateArms clock = new PlateArms();
            clock.Name = "Arms of Moscow";
            clock.Hue = Utility.RandomMinMax(1, 1001);
            clock.BaseArmorRating = Utility.Random(60, 90);
            clock.AbsorptionAttributes.EaterEnergy = 30;
            clock.ArmorAttributes.ReactiveParalyze = 1;
            clock.Attributes.BonusStam = 20;
            clock.Attributes.AttackChance = 10;
            clock.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            clock.ColdBonus = 20;
            clock.EnergyBonus = 20;
            clock.FireBonus = 20;
            clock.PhysicalBonus = 25;
            clock.PoisonBonus = 25;
            return clock;
        }

        private Item CreateHammer()
        {
            BoneHarvester hammer = new BoneHarvester();
            hammer.Name = "Sickle of the Worker";
            hammer.Hue = Utility.RandomMinMax(51, 251);
            hammer.MinDamage = Utility.Random(30, 80);
            hammer.MaxDamage = Utility.Random(80, 120);
            hammer.Attributes.BonusDex = 10;
            hammer.Attributes.SpellDamage = 5;
            hammer.WeaponAttributes.HitFireArea = 30;
            hammer.WeaponAttributes.SelfRepair = 5;
            hammer.SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
            return hammer;
        }

        public USSRRelicsChest(Serial serial) : base(serial)
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
