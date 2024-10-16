using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialWoodenChestIvan : WoodenChest
    {
        [Constructable]
        public SpecialWoodenChestIvan()
        {
            Name = "Ivan’s Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Ruble"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Vodka of the Tsars", 1153), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Ivan’s Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Earrings of Anastasia"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Romanovs", 2126), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Crimean Champagne"), 0.08);
            AddItem(CreateGoldItem("Golden Denga"), 0.16);
            AddItem(CreateColoredItem<Boots>("Boots of the Oprichniki", 1171), 0.19);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Necklace of Catherine the Great"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Ivan’s Strategic Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Rasputin’s Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateHelmet(), 0.20);
            AddItem(CreateCuirass(), 0.30);
            AddItem(CreateWarMace(), 0.30);
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
            note.NoteString = "I am Ivan, the Terrible, the first Tsar of Russia, the conqueror of Kazan and Astrakhan, the defender of Orthodoxy";
            note.TitleString = "Ivan’s Testament";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Ivan’s Tomb";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new WarMace());
            weapon.Name = "Ivan’s Axe";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateHelmet()
        {
            BaseArmor helmet = Utility.RandomList<BaseArmor>(new PlateHelm());
            helmet.Name = "Ivan’s Helmet";
            helmet.Hue = Utility.RandomList(1, 1788);
            helmet.BaseArmorRating = Utility.Random(30, 70);
            return helmet;
        }

        private Item CreateCuirass()
        {
            PlateChest cuirass = new PlateChest();
            cuirass.Name = "Cuirass of Peter the Great";
            cuirass.Hue = Utility.RandomMinMax(600, 1600);
            cuirass.BaseArmorRating = Utility.Random(60, 90);
            cuirass.AbsorptionAttributes.EaterFire = 30;
            cuirass.ArmorAttributes.LowerStatReq = 10;
            cuirass.Attributes.BonusStr = 10;
            cuirass.Attributes.AttackChance = 10;
            cuirass.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            cuirass.FireBonus = 20;
            cuirass.PoisonBonus = 25;
            return cuirass;
        }

        private Item CreateWarMace()
        {
            WarMace warMace = new WarMace();
            warMace.Name = "Scepter of Alexander Nevsky";
            warMace.Hue = Utility.RandomMinMax(50, 250);
            warMace.MinDamage = Utility.Random(30, 80);
            warMace.MaxDamage = Utility.Random(80, 120);
            warMace.Attributes.BonusDex = 10;
            warMace.Attributes.SpellDamage = 5;
            warMace.Slayer = SlayerName.OrcSlaying;
            warMace.WeaponAttributes.HitLightning = 30;
            warMace.SkillBonuses.SetValues(0, SkillName.Macing, 25.0);
            return warMace;
        }

        public SpecialWoodenChestIvan(Serial serial) : base(serial)
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
