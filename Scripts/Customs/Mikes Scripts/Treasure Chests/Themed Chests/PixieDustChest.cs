using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class PixieDustChest : WoodenChest
    {
        [Constructable]
        public PixieDustChest()
        {
            Name = "Pixie Dust Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateMaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Amethyst>("Amethyst of the Moon", 1174), 0.15);
            AddItem(CreateColoredItem<GreaterHealPotion>("Pixie Punch", 1174), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Pixie Loot"), 0.20);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Dragonfly Necklace"), 0.25);
            AddItem(CreateSimpleNote(), 0.30);
            AddItem(new Gold(Utility.Random(1, 10000)), 0.20);
            AddItem(CreateColoredItem<Amethyst>("Topaz of the Fire", 1176), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Old Pixie Liquor", 1174), 0.10);
            AddItem(CreateGoldItem("Pixie Token"), 0.18);
            AddItem(CreateColoredItem<Boots>("Boots of the Imp", 1174), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Star Earrings"), 0.15);
            AddItem(CreateMap(), 0.10);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Cleansing Dew"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateSandals(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateMagusRod(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateMaxxiaScroll()
        {
            MaxxiaScroll scroll = new MaxxiaScroll();
            scroll.Amount = Utility.Random(1, 3);
            return scroll;
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
            note.NoteString = "Beware of the pixies! They are mischievous and love to play tricks!";
            note.TitleString = "A warning from the fairies";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Pixie Hideout";
            map.Bounds = new Rectangle2D(2000, -200, 2400, -200);
            map.NewPin = new Point2D(2200, -100);
            map.Protected = false;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Dagger());
            weapon.Name = "Pixie Dagger";
            weapon.Hue = Utility.Random(1, 1788);
            weapon.MaxDamage = Utility.Random(40, 80);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest(), new LeatherLegs(), new LeatherGloves(), new LeatherArms());
            armor.Name = "Pixie Outfit";
            armor.Hue = Utility.Random(1, 1788);
            armor.BaseArmorRating = Utility.Random(40, 80);
            return armor;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Healer's Blessed Sandals";
            sandals.Hue = Utility.Random(250, 1250);
            sandals.Attributes.LowerManaCost = 10;
            sandals.SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
            return sandals;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Fortune's PlateChest";
            plateChest.Hue = Utility.Random(700, 950);
            plateChest.BaseArmorRating = Utility.Random(50, 80);
            plateChest.AbsorptionAttributes.EaterCold = 10;
            plateChest.ArmorAttributes.DurabilityBonus = 20;
            plateChest.Attributes.Luck = 200;
            plateChest.ColdBonus = 15;
            plateChest.EnergyBonus = 10;
            plateChest.FireBonus = 10;
            plateChest.PhysicalBonus = 15;
            plateChest.PoisonBonus = 5;
            return plateChest;
        }

        private Item CreateMagusRod()
        {
            BlackStaff magusRod = new BlackStaff();
            magusRod.Name = "Magus Rod";
            magusRod.Hue = Utility.Random(250, 450);
            magusRod.MinDamage = Utility.Random(15, 45);
            magusRod.MaxDamage = Utility.Random(45, 75);
            magusRod.Attributes.BonusInt = 15;
            magusRod.Attributes.SpellChanneling = 1;
            magusRod.Slayer = SlayerName.ElementalBan;
            magusRod.WeaponAttributes.MageWeapon = 2;
            magusRod.SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
            magusRod.SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
            return magusRod;
        }

        public PixieDustChest(Serial serial) : base(serial)
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
