using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpartanTreasureChest : WoodenChest
    {
        [Constructable]
        public SpartanTreasureChest()
        {
            Name = "Spartan Treasure Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Emerald>("Emerald of Sparta"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Spartan Courage", 1175), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Spartan Glory"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Lion Necklace"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Diamond>("Diamond of Thermopylae", 1159), 0.12);
            AddItem(CreateColoredItem<GreaterHealPotion>("Spartan Fury", 1175), 0.08);
            AddItem(CreateGoldItem("Spartan Obolus"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Warrior", 1176), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Spartan Helmet Earrings"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Sextant>("Spartan Navigation Tool"), 0.13);
            AddItem(CreateNamedItem<GreaterStrengthPotion>("Bottle of Hercules' Strength"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateShield(), 0.30);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreateScimitar(), 0.20);
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

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateColoredItem<T>(string name, int hue) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            item.Hue = hue;
            return item;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Come back with your shield or on it!";
            note.TitleString = "King Leonidas’ Letter";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Hidden Oracle of Delphi";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Dory of the Phalanx";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateShield()
        {
            BaseArmor shield = Utility.RandomList<BaseArmor>(new WoodenShield(), new MetalShield());
            shield.Name = "Spartan Shield";
            shield.Hue = Utility.RandomList(1, 1788);
            shield.BaseArmorRating = Utility.Random(30, 70);
            return shield;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Denim Jacket of Reflection";
            tunic.Hue = Utility.RandomMinMax(500, 1500);
            tunic.Attributes.ReflectPhysical = 10;
            tunic.Attributes.DefendChance = 7;
            tunic.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            return tunic;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Locke's Adventurer Leather";
            chest.Hue = Utility.RandomMinMax(200, 700);
            chest.BaseArmorRating = Utility.Random(30, 65);
            chest.AbsorptionAttributes.EaterFire = 15;
            chest.Attributes.BonusDex = 20;
            chest.Attributes.DefendChance = 10;
            chest.Attributes.NightSight = 1;
            chest.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            chest.ColdBonus = 5;
            chest.EnergyBonus = 10;
            chest.FireBonus = 5;
            chest.PhysicalBonus = 20;
            chest.PoisonBonus = 5;
            return chest;
        }

        private Item CreateScimitar()
        {
            Scimitar scimitar = new Scimitar();
            scimitar.Name = "Harpē Blade";
            scimitar.Hue = Utility.RandomMinMax(250, 450);
            scimitar.MinDamage = Utility.Random(25, 70);
            scimitar.MaxDamage = Utility.Random(70, 100);
            scimitar.Attributes.Luck = 100;
            scimitar.Attributes.AttackChance = 10;
            scimitar.Slayer = SlayerName.Ophidian;
            scimitar.WeaponAttributes.HitDispel = 25;
            scimitar.WeaponAttributes.HitMagicArrow = 20;
            scimitar.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            return scimitar;
        }

        public SpartanTreasureChest(Serial serial) : base(serial)
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
