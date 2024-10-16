using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialWoodenChestOisin : WoodenChest
    {
        [Constructable]
        public SpecialWoodenChestOisin()
        {
            Name = "Oisin’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Harp"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Irish Whiskey", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Oisin’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Niamh"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Fianna", 2126), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Mead"), 0.08);
            AddItem(CreateGoldItem("Golden Torc"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Warrior", 1172), 0.19);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Necklace of Danu"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Oisin’s Sight Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Druidic Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateChestplate(), 0.30);
            AddItem(CreateArms(), 0.30);
            AddItem(CreateKryss(), 0.30);
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
            note.NoteString = "I am Oisin, son of Fionn mac Cumhaill, and this is my tale of Tir na nOg";
            note.TitleString = "Oisin’s Story";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Oisin’s Cave";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Spear());
            weapon.Name = "Oisin’s Spear";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            Cloak armor = new Cloak();
            armor.Name = "Oisin’s Cloak";
            armor.Hue = Utility.RandomList(1, 2998);
            return armor;
        }

        private Item CreateChestplate()
        {
            PlateChest chestplate = new PlateChest();
            chestplate.Name = "Chestplate of Fionn";
            chestplate.Hue = Utility.RandomMinMax(600, 1600);
            chestplate.Attributes.DefendChance = 10;
            chestplate.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            chestplate.PhysicalBonus = 20;
            return chestplate;
        }

        private Item CreateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Arms of the Fianna";
            arms.Hue = Utility.RandomMinMax(1, 1000);
            arms.BaseArmorRating = Utility.Random(60, 90);
            arms.AbsorptionAttributes.EaterFire = 30;
            arms.ArmorAttributes.ReactiveParalyze = 1;
            arms.Attributes.BonusStr = 20;
            arms.Attributes.AttackChance = 10;
            arms.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            arms.ColdBonus = 20;
            arms.EnergyBonus = 20;
            arms.FireBonus = 25;
            arms.PhysicalBonus = 25;
            arms.PoisonBonus = 25;
            return arms;
        }

        private Item CreateKryss()
        {
            Kryss kryss = new Kryss();
            kryss.Name = "Dagger of Lugh";
            kryss.Hue = Utility.RandomMinMax(50, 250);
            kryss.MinDamage = Utility.Random(30, 80);
            kryss.MaxDamage = Utility.Random(80, 120);
            kryss.Attributes.BonusDex = 10;
            kryss.Attributes.SpellDamage = 5;
            kryss.WeaponAttributes.HitLightning = 30;
            kryss.SkillBonuses.SetValues(0, SkillName.Fencing, 25.0);
            return kryss;
        }

        public SpecialWoodenChestOisin(Serial serial) : base(serial)
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
