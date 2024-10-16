using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AbyssalPlaneChest : WoodenChest
    {
        [Constructable]
        public AbyssalPlaneChest()
        {
            Name = "Abyssal Plane Chest";
            Hue = 2159;

            // Add items to the chest
            AddItem(CreateColoredItem<Sapphire>("Heart of the Abyss", 2336), 0.18);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel3>("Deepsea Treasure"), 0.13);
            AddItem(CreateColoredItem<GoldEarrings>("Ocean's Tear Earring", 1265), 0.17);
            AddItem(new Gold(Utility.Random(1, 6200)), 0.17);
            AddItem(CreateNamedItem<Apple>("Drenched Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Deepsea Elixir"), 0.14);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Tidal", 1156), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Seer of the Abyss"), 0.14);
            AddItem(CreateNamedItem<Shaft>("Wand of the Tides"), 0.13);
            AddItem(CreateArmor(), 0.13);
            AddItem(CreateStrawHat(), 0.20);
            AddItem(CreatePlateHelm(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "In the depths, secrets remain whispered.";
            note.TitleString = "Abyssal Whispers";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Abyssal Guardian's Plate";
            armor.Hue = Utility.RandomMinMax(1, 1160);
            armor.BaseArmorRating = Utility.Random(28, 64);
            return armor;
        }

        private Item CreateStrawHat()
        {
            StrawHat hat = new StrawHat();
            hat.Name = "Nature's Muffler";
            hat.Hue = Utility.RandomMinMax(350, 1350);
            hat.ClothingAttributes.DurabilityBonus = 3;
            hat.Attributes.BonusDex = 10;
            hat.Attributes.DefendChance = 5;
            hat.SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
            hat.SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);
            return hat;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Despair's Shadow";
            helm.Hue = Utility.RandomMinMax(10, 300);
            helm.BaseArmorRating = Utility.Random(35, 75);
            helm.AbsorptionAttributes.EaterEnergy = 20;
            helm.ArmorAttributes.DurabilityBonus = -10;
            helm.Attributes.IncreasedKarmaLoss = 15;
            helm.Attributes.Luck = -45;
            helm.SkillBonuses.SetValues(0, SkillName.Hiding, 10.0);
            helm.ColdBonus = 15;
            helm.EnergyBonus = 20;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 15;
            return helm;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Tri-lithium Blade";
            longsword.Hue = Utility.RandomMinMax(250, 450);
            longsword.MinDamage = Utility.Random(35, 65);
            longsword.MaxDamage = Utility.Random(65, 95);
            longsword.Attributes.SpellDamage = 10;
            longsword.Attributes.DefendChance = 5;
            longsword.Slayer = SlayerName.DragonSlaying;
            longsword.WeaponAttributes.HitLightning = 25;
            longsword.WeaponAttributes.SelfRepair = 5;
            longsword.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return longsword;
        }

        public AbyssalPlaneChest(Serial serial) : base(serial)
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
