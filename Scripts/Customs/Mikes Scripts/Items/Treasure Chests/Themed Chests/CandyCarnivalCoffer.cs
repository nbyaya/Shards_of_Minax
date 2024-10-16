using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CandyCarnivalCoffer : WoodenChest
    {
        [Constructable]
        public CandyCarnivalCoffer()
        {
            Name = "Candy Carnival Coffer";
            Hue = 1185;

            // Add items to the chest
            AddItem(CreateNamedItem<Emerald>("Carnival Candy Jewel"), 0.05);
            AddItem(CreateSimpleNote(), 0.20);
            AddItem(CreateNamedItem<TreasureLevel4>("Carnival Candy Bag"), 0.17);
            AddItem(CreateNamedItem<GoldEarrings>("Rainbow Lollipop Earrings"), 0.15);
            AddItem(new Gold(Utility.Random(1, 4500)));
            AddItem(CreateNamedItem<Apple>("Candy Sprinkle Apple"), 0.18);
            AddItem(CreateNamedItem<GreaterHealPotion>("Fizzy Pop Drink"), 0.10);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Candy Clown", 1183), 0.12);
            AddItem(CreateNamedItem<Spyglass>("Carnival Gaze Spyglass"), 0.04);
            AddItem(CreateNamedItem<Drums>("Candy Beat Drum"), 0.12); // RandomInstrument
            AddItem(CreateNamedItem<FancyShirt>("Candy Striped Garb"), 0.14); // RandomClothing
            AddItem(CreateArmor(), 0.20); // Random Armor
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateStuddedLegs(), 0.20);
            AddItem(CreateKryss(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
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
            note.NoteString = "Every candy tells a tale of joy.";
            note.TitleString = "Carnival Candy Chronicle";
            return note;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "Candy Coated Armor";
            armor.Hue = Utility.RandomList(1, 1186);
            armor.BaseArmorRating = Utility.Random(20, 55);
            return armor;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Vest of the Vein Seeker";
            tunic.Hue = Utility.RandomMinMax(300, 1300);
            tunic.ClothingAttributes.DurabilityBonus = 3;
            tunic.Attributes.Luck = 20;
            tunic.SkillBonuses.SetValues(0, SkillName.Mining, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.ItemID, 10.0);
            return tunic;
        }

        private Item CreateStuddedLegs()
        {
            StuddedLegs legs = new StuddedLegs();
            legs.Name = "Witchwood Greaves";
            legs.Hue = Utility.RandomMinMax(1, 1000);
            legs.BaseArmorRating = Utility.Random(25, 70);
            legs.ArmorAttributes.LowerStatReq = 10;
            legs.Attributes.Luck = 30;
            legs.SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
            legs.SkillBonuses.SetValues(1, SkillName.Herding, 15.0);
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateKryss()
        {
            Kryss kryss = new Kryss();
            kryss.Name = "Touch of Anguish";
            kryss.Hue = Utility.RandomMinMax(250, 450);
            kryss.MinDamage = Utility.Random(15, 50);
            kryss.MaxDamage = Utility.Random(50, 85);
            kryss.Attributes.RegenMana = 5;
            kryss.Attributes.Luck = 120;
            kryss.Slayer = SlayerName.Ophidian;
            kryss.WeaponAttributes.HitColdArea = 35;
            kryss.WeaponAttributes.HitLeechMana = 20;
            kryss.SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            kryss.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return kryss;
        }

        public CandyCarnivalCoffer(Serial serial) : base(serial)
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
