using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CivilWorChest : WoodenChest
    {
        [Constructable]
        public CivilWorChest()
        {
            Name = "Civil War Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateAmethyst(), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Soldier’s Courage", 1159), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Lincoln’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Star Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Topaz of Freedom", 1166), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Jefferson Davis’ Reserve"), 0.10);
            AddItem(CreateGoldItem("Confederate Dollar"), 0.10);
            AddItem(CreateColoredItem<Boots>("Boots of the General", 1176), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Rose Earrings"), 0.15);
            AddItem(CreateMap(), 0.05);
            AddItem(CreateNamedItem<Crossbow>("Ulysses S. Grant’s Rifle"), 0.10);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Antidote"), 0.15);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateCrossbow(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateAmethyst()
        {
            Amethyst amethyst = new Amethyst();
            amethyst.Name = "Amethyst of Unity";
            return amethyst;
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
            note.NoteString = "Four score and seven years ago…";
            note.TitleString = "Lincoln’s Gettysburg Address";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Underground Railroad";
            map.Bounds = new Rectangle2D(2000, -200, -400, -400);
            map.NewPin = new Point2D(1800, -300);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "The Emancipator";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MinDamage = Utility.Random(10, 20);
            weapon.MaxDamage = Utility.Random(30, 60);
            return weapon;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Thief's Shadow Tunic";
            tunic.Hue = Utility.RandomMinMax(1100, 1900);
            tunic.Attributes.BonusDex = 15;
            tunic.Attributes.NightSight = 1;
            tunic.SkillBonuses.SetValues(0, SkillName.Stealing, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
            return tunic;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs plateLegs = new PlateLegs();
            plateLegs.Name = "Blackthorne's Spur";
            plateLegs.Hue = Utility.RandomMinMax(10, 300);
            plateLegs.BaseArmorRating = Utility.Random(40, 75);
            plateLegs.AbsorptionAttributes.EaterPoison = 25;
            plateLegs.Attributes.RegenStam = 15;
            plateLegs.Attributes.Luck = 40;
            plateLegs.SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
            plateLegs.ColdBonus = 10;
            plateLegs.EnergyBonus = 5;
            plateLegs.FireBonus = 10;
            plateLegs.PhysicalBonus = 15;
            plateLegs.PoisonBonus = 30;
            return plateLegs;
        }

        private Item CreateCrossbow()
        {
            Crossbow crossbow = new Crossbow();
            crossbow.Name = "Hanseatic Crossbow";
            crossbow.Hue = Utility.RandomMinMax(150, 350);
            crossbow.MinDamage = Utility.Random(20, 60);
            crossbow.MaxDamage = Utility.Random(60, 100);
            crossbow.Attributes.AttackChance = 10;
            crossbow.Attributes.LowerRegCost = 20;
            crossbow.Slayer = SlayerName.ReptilianDeath;
            crossbow.WeaponAttributes.HitLightning = 30;
            crossbow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return crossbow;
        }

        public CivilWorChest(Serial serial) : base(serial)
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
