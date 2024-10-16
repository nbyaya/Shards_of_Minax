using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BismarcksTreasureChest : WoodenChest
    {
        [Constructable]
        public BismarcksTreasureChest()
        {
            Name = "Bismarck's Treasure";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateGoldItem("Golden Reichsmark"), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Rhenish Wine", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Bismarck's Legacy"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Bracelet of Bismarck"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the German Empire", 2117), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Prussian Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Thaler"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Diplomat", 1175), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Ring of the Kaiser"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Bismarck's Strategic Spyglass"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Germanic Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreatePlateGorget(), 0.30);
            AddItem(CreatePlateLegs(), 0.30);
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
            note.NoteString = "I am Bismarck, the Iron Chancellor of Germany, the master of Realpolitik and the unifier of the German states";
            note.TitleString = "Bismarck’s Memoir";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Bismarck's Mausoleum";
            map.Bounds = new Rectangle2D(1000, 1000, 400, 400);
            map.NewPin = new Point2D(1200, 1200);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new Longsword());
            weapon.Name = "Bismarck's Sword";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Bismarck's Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreatePlateGorget()
        {
            PlateGorget gorget = new PlateGorget();
            gorget.Name = "Collar of the Iron Cross";
            gorget.Hue = Utility.RandomMinMax(600, 1600);
            gorget.Attributes.DefendChance = 10;
            gorget.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            gorget.PhysicalBonus = 20;
            return gorget;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs legs = new PlateLegs();
            legs.Name = "Legplates of the Reich";
            legs.Hue = Utility.RandomMinMax(1, 1000);
            legs.BaseArmorRating = Utility.Random(60, 90);
            legs.AbsorptionAttributes.EaterPoison = 30;
            legs.ArmorAttributes.ReactiveParalyze = 1;
            legs.Attributes.BonusDex = 20;
            legs.Attributes.AttackChance = 10;
            legs.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            legs.ColdBonus = 20;
            legs.EnergyBonus = 20;
            legs.FireBonus = 20;
            legs.PhysicalBonus = 25;
            legs.PoisonBonus = 25;
            return legs;
        }

        private Item CreateWarMace()
        {
            WarMace warMace = new WarMace();
            warMace.Name = "Hammer of Unity";
            warMace.Hue = Utility.RandomMinMax(50, 250);
            warMace.MinDamage = Utility.Random(30, 80);
            warMace.MaxDamage = Utility.Random(80, 120);
            warMace.Attributes.BonusStr = 10;
            warMace.Attributes.SpellDamage = 5;
            warMace.Slayer = SlayerName.OrcSlaying;
            warMace.WeaponAttributes.HitFireball = 30;
            warMace.WeaponAttributes.SelfRepair = 5;
            warMace.SkillBonuses.SetValues(0, SkillName.Macing, 25.0);
            return warMace;
        }

        public BismarcksTreasureChest(Serial serial) : base(serial)
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
