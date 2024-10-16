using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RiverRaftersChest : WoodenChest
    {
        [Constructable]
        public RiverRaftersChest()
        {
            Name = "River Rafters Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateEmerald(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("River Water", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel1>("Rafters Loot"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Fish Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the River", 2117), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old River Rum", 0), 0.08);
            AddItem(CreateGoldItem("Wet Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Riverwalker", 1153), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Fishhook Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<FishingPole>("Rafters Fishing Pole"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Water"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateWarHammer(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Emerald of the Rapids";
            return emerald;
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
            note.NoteString = "We found this chest floating down the river!";
            note.TitleString = "Rafters Log";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the River’s End";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "River Blade";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Rafters Gear";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Forester's Tunic";
            tunic.Hue = Utility.RandomMinMax(150, 1000);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusDex = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.Camping, 20.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
            return tunic;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Blade Dancer's PlateChest";
            plateChest.Hue = Utility.RandomMinMax(600, 900);
            plateChest.BaseArmorRating = Utility.Random(50, 85);
            plateChest.AbsorptionAttributes.EaterDamage = 15;
            plateChest.ArmorAttributes.SelfRepair = 10;
            plateChest.Attributes.BonusStr = 15;
            plateChest.Attributes.DefendChance = 10;
            plateChest.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 20;
            plateChest.PhysicalBonus = 20;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateWarHammer()
        {
            WarHammer warHammer = new WarHammer();
            warHammer.Name = "Heartbreaker Sunder";
            warHammer.Hue = Utility.RandomMinMax(300, 500);
            warHammer.MinDamage = Utility.Random(40, 80);
            warHammer.MaxDamage = Utility.Random(80, 120);
            warHammer.Attributes.BonusStr = 20;
            warHammer.Attributes.WeaponSpeed = 5;
            warHammer.Slayer = SlayerName.EarthShatter;
            warHammer.WeaponAttributes.HitHarm = 25;
            warHammer.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return warHammer;
        }

        public RiverRaftersChest(Serial serial) : base(serial)
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
