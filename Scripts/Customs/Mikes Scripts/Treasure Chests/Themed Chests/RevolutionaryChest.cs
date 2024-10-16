using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RevolutionaryChest : WoodenChest
    {
        [Constructable]
        public RevolutionaryChest()
        {
            Name = "Revolutionary Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
			AddItem(CreateNamedItem<MaxxiaScroll>("Declaration Of Independence"), 0.20);
            AddItem(CreateNamedItem<Sapphire>("Liberty Stone"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Patriot’s Brew", 1776), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Colonial Coins"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Eagle Pendant"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Enlightenment", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Boston Tea Party"), 0.08);
            AddItem(CreateGoldItem("Tax Stamp"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Minuteman", 1618), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Liberty Bell Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Paul Revere’s Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Benjamin Franklin’s Elixir"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateKilt(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateGnarledStaff(), 0.20);
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
            note.NoteString = "We hold these truths to be self-evident…";
            note.TitleString = "Thomas Jefferson’s Notes";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to George Washington’s Secret Camp";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "The Patriot";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "The Continental";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateKilt()
        {
            Kilt kilt = new Kilt();
            kilt.Name = "Shepherd's Kilt";
            kilt.Hue = Utility.RandomMinMax(800, 1800);
            kilt.ClothingAttributes.DurabilityBonus = 4;
            kilt.Attributes.BonusInt = 10;
            kilt.SkillBonuses.SetValues(0, SkillName.Herding, 25.0);
            kilt.SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
            return kilt;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Naj's Arcane Vestment";
            plateChest.Hue = Utility.RandomMinMax(250, 750);
            plateChest.BaseArmorRating = Utility.Random(50, 85);
            plateChest.AbsorptionAttributes.EaterFire = 15;
            plateChest.ArmorAttributes.MageArmor = 1;
            plateChest.Attributes.BonusInt = 20;
            plateChest.Attributes.SpellDamage = 10;
            plateChest.SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 15;
            plateChest.FireBonus = 20;
            plateChest.PhysicalBonus = 10;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateGnarledStaff()
        {
            GnarledStaff staff = new GnarledStaff();
            staff.Name = "Staff of Rain's Wrath";
            staff.Hue = Utility.RandomMinMax(100, 300);
            staff.MinDamage = Utility.Random(20, 60);
            staff.MaxDamage = Utility.Random(60, 100);
            staff.Attributes.BonusMana = 15;
            staff.Attributes.CastSpeed = 1;
            staff.Slayer = SlayerName.WaterDissipation;
            staff.WeaponAttributes.HitManaDrain = 25;
            staff.WeaponAttributes.HitEnergyArea = 20;
            staff.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            return staff;
        }

        public RevolutionaryChest(Serial serial) : base(serial)
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
