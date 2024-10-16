using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AngelBlessingChest : WoodenChest
    {
        [Constructable]
        public AngelBlessingChest()
        {
            Name = "Angel’s Blessing";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateDiamondItem("Diamond of the Heavens"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Angel’s Nectar", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Angel’s Bounty"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Angel Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.15);
            AddItem(CreateColoredItem<Amethyst>("Amethyst of the Divine", 1174), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Angel’s Wine"), 0.08);
            AddItem(CreateGoldItem("Blessed Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Seraphim", 1153), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Angel Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateColoredItem<Feather>("Angel’s Feather", 1153), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Grace"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateHat(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateDiamondItem(string name)
        {
            Diamond diamond = new Diamond();
            diamond.Name = name;
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
            note.NoteString = "May the angels watch over you!";
            note.TitleString = "Angel’s Message";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Angel’s Sanctuary";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Angel’s Wrath";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Angel’s Protection";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateHat()
        {
            WizardsHat hat = new WizardsHat();
            hat.Name = "Ranger's Hat";
            hat.Hue = Utility.RandomMinMax(100, 1100);
            hat.Attributes.BonusStr = 5;
            hat.Attributes.NightSight = 1;
            hat.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            return hat;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs plateLegs = new PlateLegs();
            plateLegs.Name = "Stormforged Leggings";
            plateLegs.Hue = Utility.RandomMinMax(550, 850);
            plateLegs.BaseArmorRating = Utility.Random(45, 75);
            plateLegs.AbsorptionAttributes.EaterCold = 10;
            plateLegs.ArmorAttributes.MageArmor = 1;
            plateLegs.Attributes.RegenStam = 5;
            plateLegs.Attributes.BonusHits = 20;
            plateLegs.SkillBonuses.SetValues(0, SkillName.Tinkering, 10.0);
            plateLegs.EnergyBonus = 20;
            plateLegs.ColdBonus = 10;
            plateLegs.FireBonus = 10;
            plateLegs.PhysicalBonus = 10;
            plateLegs.PoisonBonus = 5;
            return plateLegs;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Glass Sword of Valor";
            longsword.Hue = Utility.RandomMinMax(50, 150);
            longsword.MinDamage = 500;
            longsword.MaxDamage = 1000;
            longsword.Attributes.BonusStr = 50;
            longsword.Attributes.Luck = 250;
            longsword.Slayer = SlayerName.OrcSlaying;
            longsword.Slayer2 = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.SelfRepair = -1;
            return longsword;
        }

        public AngelBlessingChest(Serial serial) : base(serial)
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
