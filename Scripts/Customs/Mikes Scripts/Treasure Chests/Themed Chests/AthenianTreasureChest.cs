using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AthenianTreasureChest : WoodenChest
    {
        [Constructable]
        public AthenianTreasureChest()
        {
            Name = "Athenian Treasure Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateColoredItem<Sapphire>("Sapphire of Athena", 2004), 0.20);
            AddItem(CreateColoredItem<GreaterHealPotion>("Ambrosia of the Gods", 1157), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Athenian Wisdom"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Owl Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of the Acropolis", 1153), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Nectar of the Muses"), 0.08);
            AddItem(CreateGoldItem("Athenian Drachma"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Philosopher", 1154), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Laurel Ring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Telescope>("Aristarchus’ Astronomical Device"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Asclepius’ Healing"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateFancyShirt(), 0.20);
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
            note.NoteString = "We have won a great victory over the Persians at Marathon!";
            note.TitleString = "General Miltiades’ Journal";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Parthenon’s Secret Chamber";
            map.Bounds = new Rectangle2D(3000, 3200, 400, 400);
            map.NewPin = new Point2D(3100, 3350);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = new Longsword();
            weapon.Name = "Xiphos of the Hoplite";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Athenian Armor";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateFancyShirt()
        {
            FancyShirt shirt = new FancyShirt();
            shirt.Name = "Crop Top Mystic";
            shirt.Hue = Utility.RandomMinMax(50, 950);
            shirt.Attributes.BonusInt = 10;
            shirt.Attributes.SpellDamage = 8;
            shirt.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            shirt.SkillBonuses.SetValues(1, SkillName.Inscribe, 12.0);
            return shirt;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Magitek Infused Plate";
            plateChest.Hue = Utility.RandomMinMax(50, 500);
            plateChest.BaseArmorRating = Utility.Random(50, 85);
            plateChest.AbsorptionAttributes.EaterEnergy = 25;
            plateChest.ArmorAttributes.MageArmor = 1;
            plateChest.Attributes.SpellDamage = 10;
            plateChest.Attributes.CastRecovery = 2;
            plateChest.SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            plateChest.ColdBonus = 10;
            plateChest.EnergyBonus = 20;
            plateChest.FireBonus = 15;
            plateChest.PhysicalBonus = 10;
            plateChest.PoisonBonus = 10;
            return plateChest;
        }

        private Item CreateWarHammer()
        {
            WarHammer warHammer = new WarHammer();
            warHammer.Name = "Aegis Shield";
            warHammer.Hue = Utility.RandomMinMax(200, 400);
            warHammer.MinDamage = Utility.Random(20, 60);
            warHammer.MaxDamage = Utility.Random(60, 90);
            warHammer.Attributes.DefendChance = 15;
            warHammer.Attributes.ReflectPhysical = 10;
            warHammer.Slayer = SlayerName.GargoylesFoe;
            warHammer.WeaponAttributes.SelfRepair = 3;
            warHammer.WeaponAttributes.ResistPhysicalBonus = 20;
            warHammer.SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            return warHammer;
        }

        public AthenianTreasureChest(Serial serial) : base(serial)
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
