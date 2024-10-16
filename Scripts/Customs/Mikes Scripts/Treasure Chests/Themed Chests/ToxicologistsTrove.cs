using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ToxicologistsTrove : WoodenChest
    {
        [Constructable]
        public ToxicologistsTrove()
        {
            Name = "Toxicologist's Trove";
            Hue = 1268;

            // Add items to the chest
            AddItem(CreateSapphire(), 0.19);
            AddItem(CreateSimpleNote(), 0.17);
            AddItem(CreateNamedItem<TreasureLevel2>("Poisoned Prize"), 0.15);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Vial of Venomous Fate"), 0.19);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.19);
            AddItem(CreateColoredItem<GreaterHealPotion>("Antidote of Last Resort", 1266), 0.12);
            AddItem(CreateNamedItem<DeadlyPoisonPotion>("Acidic Elixir"), 0.14);
            AddItem(CreateNamedItem<Spyglass>("Green Venom Viewer"), 0.13);
            AddItem(CreateNamedItem<Shaft>("Wand of Toxic Fumes"), 0.16);
            AddItem(CreateArmor(), 0.16);
            AddItem(CreateStrawHat(), 0.20);
            AddItem(CreatePlateLegs(), 0.20);
            AddItem(CreateTwoHandedAxe(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateSapphire()
        {
            Sapphire sapphire = new Sapphire();
            sapphire.Name = "Sapphire of Serpents";
            return sapphire;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "One drop can change fate.";
            note.TitleString = "Toxicologist's Journal";
            return note;
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

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Toxicologist's Protective Gear";
            armor.Hue = Utility.Random(1, 1269);
            armor.BaseArmorRating = Utility.Random(25, 60);
            return armor;
        }

        private Item CreateStrawHat()
        {
            StrawHat hat = new StrawHat();
            hat.Name = "Herbalist's Protective Hat";
            hat.Hue = Utility.RandomMinMax(280, 1280);
            hat.Attributes.BonusDex = 10;
            hat.SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
            hat.SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
            return hat;
        }

        private Item CreatePlateLegs()
        {
            PlateLegs plateLegs = new PlateLegs();
            plateLegs.Name = "Fortune's PlateLegs";
            plateLegs.Hue = Utility.RandomMinMax(700, 950);
            plateLegs.BaseArmorRating = Utility.Random(45, 75);
            plateLegs.AbsorptionAttributes.EaterEnergy = 10;
            plateLegs.ArmorAttributes.MageArmor = 1;
            plateLegs.Attributes.Luck = 175;
            plateLegs.SkillBonuses.SetValues(0, SkillName.DetectHidden, 10.0);
            plateLegs.ColdBonus = 10;
            plateLegs.EnergyBonus = 15;
            plateLegs.FireBonus = 5;
            plateLegs.PhysicalBonus = 15;
            plateLegs.PoisonBonus = 5;
            return plateLegs;
        }

        private Item CreateTwoHandedAxe()
        {
            TwoHandedAxe axe = new TwoHandedAxe();
            axe.Name = "Buster Sword Replica";
            axe.Hue = Utility.RandomMinMax(500, 700);
            axe.MinDamage = Utility.Random(30, 70);
            axe.MaxDamage = Utility.Random(70, 120);
            axe.Attributes.BonusStr = 20;
            axe.Attributes.AttackChance = 5;
            axe.WeaponAttributes.HitLightning = 20;
            axe.WeaponAttributes.HitHarm = 10;
            axe.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            return axe;
        }

        public ToxicologistsTrove(Serial serial) : base(serial)
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
