using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EmeraldIsleChest : WoodenChest
    {
        [Constructable]
        public EmeraldIsleChest()
        {
            Name = "Emerald Isle Chest";
            Hue = 2278;

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Irish Heart Gem", 0), 0.05);
            AddItem(CreateSimpleNote(), 0.15);
            AddItem(CreateNamedItem<TreasureLevel2>("Green Isle's Fortune"), 0.12);
            AddItem(CreateColoredItem<GoldEarrings>("Celtic Knot Earring", 1367), 0.12);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.20);
            AddItem(CreateNamedItem<Apple>("Shamrock Apple"), 0.08);
            AddItem(CreateNamedItem<GreaterHealPotion>("Irish Stout"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Green Hills", 1270), 0.17);
            AddItem(CreateRandomInstrument(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Seafarer's Scope"), 0.13);
            AddItem(CreateRandomWand(), 0.15);
            AddItem(CreateArmor(), 0.2);
            AddItem(CreateTunic(), 0.2);
            AddItem(CreateHelm(), 0.2);
            AddItem(CreateDagger(), 0.2);
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
            note.NoteString = "The beauty of Ireland lies in its emerald meadows and timeless tales.";
            note.TitleString = "Leprechaun's Chronicle";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            // Assuming a random instrument is a type of BaseInstrument with a random name
            Harp instrument = new Harp();
            instrument.Name = "Celtic Harp";
            return instrument;
        }

        private Item CreateRandomWand()
        {
            // Assuming a random wand is a type of BaseWand with a random name
            Shaft wand = new Shaft();
            wand.Name = "Wand of Irish Magic";
            return wand;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Gaelic Defender's Armor";
            armor.Hue = Utility.Random(1, 1288);
            armor.BaseArmorRating = Utility.Random(20, 60);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Beastmaster's Tunic";
            tunic.Hue = Utility.Random(250, 1250);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusDex = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
            return tunic;
        }

        private Item CreateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Stormforged Helm";
            helm.Hue = Utility.Random(550, 850);
            helm.BaseArmorRating = Utility.Random(45, 75);
            helm.AbsorptionAttributes.EaterEnergy = 20;
            helm.ArmorAttributes.SelfRepair = 3;
            helm.Attributes.ReflectPhysical = 10;
            helm.Attributes.BonusInt = 10;
            helm.SkillBonuses.SetValues(0, SkillName.ItemID, 20.0);
            helm.EnergyBonus = 20;
            helm.ColdBonus = 5;
            helm.FireBonus = 5;
            helm.PhysicalBonus = 10;
            helm.PoisonBonus = 5;
            return helm;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Blade of the Stars";
            dagger.Hue = Utility.Random(100, 200);
            dagger.MinDamage = Utility.Random(25, 55);
            dagger.MaxDamage = Utility.Random(55, 85);
            dagger.Attributes.LowerManaCost = 10;
            dagger.Attributes.NightSight = 1;
            dagger.Slayer = SlayerName.ElementalHealth;
            dagger.WeaponAttributes.HitMagicArrow = 30;
            dagger.SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);
            return dagger;
        }

        public EmeraldIsleChest(Serial serial) : base(serial)
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
