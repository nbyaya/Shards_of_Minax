using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class DoctorsBag : WoodenChest
    {
        [Constructable]
        public DoctorsBag()
        {
            Name = "Doctor's Bag";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateDiamondItem("Diamond of Healing"), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Fine Vintage Wine"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Doctor's Stash"), 0.17);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Caduceus Bracelet"), 0.50);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Ruby>("Ruby of Vitality", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Old Medicinal Wine"), 0.08);
            AddItem(CreateGoldItem("Golden Stethoscope"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Shoes of the Healer", 1618), 0.19);
            AddItem(CreateNamedItem<GoldRing>("Golden Band of Life"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<Scissors>("Doctor's Trusted Scissors"), 0.13);
            AddItem(CreateNamedItem<GreaterHealPotion>("Bottle of Miracle Cure"), 0.20);
            AddItem(CreateWeapon(), 0.20);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateWarFork(), 0.20);
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
            note.NoteString = "I have discovered a new cure for the plague!";
            note.TitleString = "Doctor's Notes";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to the Doctor's Secret Lab";
            map.Bounds = new Rectangle2D(1000, 1200, 200, 200);
            map.NewPin = new Point2D(1100, 1150);
            map.Protected = true;
            return map;
        }

        private Item CreateWeapon()
        {
            BaseWeapon weapon = Utility.RandomList<BaseWeapon>(new WarFork());
            weapon.Name = "Surgeon's Scalpel";
            weapon.Hue = Utility.RandomList(1, 1788);
            weapon.MaxDamage = Utility.Random(30, 70);
            return weapon;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Doctor's Coat";
            armor.Hue = Utility.RandomList(1, 1788);
            armor.BaseArmorRating = Utility.Random(30, 70);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Beastmaster's Tunic";
            tunic.Hue = Utility.RandomMinMax(250, 1250);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusInt = 10;
            tunic.Attributes.RegenStam = 5;
            tunic.SkillBonuses.SetValues(0, SkillName.AnimalLore, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Veterinary, 25.0);
            return tunic;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Wraith's Bane";
            plateChest.Hue = Utility.RandomMinMax(10, 300);
            plateChest.BaseArmorRating = Utility.Random(40, 80);
            plateChest.AbsorptionAttributes.EaterCold = 20;
            plateChest.ArmorAttributes.DurabilityBonus = -15;
            plateChest.Attributes.IncreasedKarmaLoss = 20;
            plateChest.Attributes.Luck = -50;
            plateChest.SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            plateChest.ColdBonus = 20;
            plateChest.EnergyBonus = 5;
            plateChest.FireBonus = 5;
            plateChest.PhysicalBonus = 10;
            plateChest.PoisonBonus = 15;
            return plateChest;
        }

        private Item CreateWarFork()
        {
            WarFork warFork = new WarFork();
            warFork.Name = "Dragon Claw";
            warFork.Hue = Utility.RandomMinMax(300, 550);
            warFork.MinDamage = Utility.Random(20, 60);
            warFork.MaxDamage = Utility.Random(60, 100);
            warFork.Attributes.BonusDex = 15;
            warFork.Attributes.RegenStam = 3;
            warFork.Slayer = SlayerName.DragonSlaying;
            warFork.WeaponAttributes.HitLeechHits = 20;
            warFork.WeaponAttributes.BattleLust = 10;
            warFork.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            return warFork;
        }

        public DoctorsBag(Serial serial) : base(serial)
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
