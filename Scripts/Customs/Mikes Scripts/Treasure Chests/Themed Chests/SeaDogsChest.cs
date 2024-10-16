using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SeaDogsChest : WoodenChest
    {
        [Constructable]
        public SeaDogsChest()
        {
            Name = "Sea Dog's Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreatePearl(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Sea Dog's Grog", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Sea Dog's Loot"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Anchor Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of the Sea", 1152), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Salty Sea Dog's Rum", 1153), 0.08);
            AddItem(CreateGoldItem("Sunken Doubloon"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Sailor", 1154), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Seashell Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Captain's Reliable Sextant"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Healing Salve"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreatePlainDress(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateBlackStaff(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreatePearl()
        {
            Ruby pearl = new Ruby();
            pearl.Name = "Pearl of the Ocean";
            return pearl;
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
            note.NoteString = "Ahoy matey! Ye have found me treasure! But beware of the curse that lies within!";
            note.TitleString = "Captain Sea Dog's Warning";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Sea Dog's Secret Island";
            map.Bounds = new Rectangle2D(5000, 5200, 500, 500);
            map.NewPin = new Point2D(5100, 5150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "The Kraken";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Sea Dog's Pride";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreatePlainDress()
        {
            PlainDress dress = new PlainDress();
            dress.Name = "Potionmaster's Plain Dress";
            dress.Hue = Utility.RandomMinMax(200, 1200);
            dress.Attributes.BonusInt = 10;
            dress.Attributes.EnhancePotions = 15;
            dress.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            return dress;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Inferno PlateChest";
            plateChest.Hue = Utility.RandomMinMax(500, 600);
            plateChest.BaseArmorRating = Utility.Random(50, 85);
            plateChest.AbsorptionAttributes.EaterFire = 25;
            plateChest.ArmorAttributes.SelfRepair = 10;
            plateChest.Attributes.BonusStr = 30;
            plateChest.FireBonus = 30;
            plateChest.EnergyBonus = 10;
            plateChest.PoisonBonus = -5;
            plateChest.PhysicalBonus = 20;
            return plateChest;
        }

        private Item CreateBlackStaff()
        {
            BlackStaff staff = new BlackStaff();
            staff.Name = "Mage's Staff";
            staff.Hue = Utility.RandomMinMax(100, 300);
            staff.MinDamage = Utility.Random(25, 55);
            staff.MaxDamage = Utility.Random(55, 85);
            staff.Attributes.BonusInt = 20;
            staff.Attributes.SpellDamage = 10;
            staff.Slayer = SlayerName.ElementalBan;
            staff.WeaponAttributes.HitFireball = 40;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return staff;
        }

        public SeaDogsChest(Serial serial) : base(serial)
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
