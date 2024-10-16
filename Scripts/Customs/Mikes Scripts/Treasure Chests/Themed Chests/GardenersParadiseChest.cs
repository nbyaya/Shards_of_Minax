using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GardenersParadiseChest : WoodenChest
    {
        [Constructable]
        public GardenersParadiseChest()
        {
            Name = "Gardener's Paradise";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Emerald>("Emerald of the Verdant Grove"), 0.24);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Nectar of Nature", 1492), 0.17);
            AddItem(CreateNamedItem<TreasureLevel4>("Floral Fortune"), 0.23);
            AddItem(CreateNamedItem<Diamond>("Diamond Dewdrop"), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 1.0);
            AddItem(CreateShoes(), 0.20);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreateKatana(), 0.20);
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
            note.NoteString = "The world flourishes where nature is cherished.";
            note.TitleString = "Gardener's Memoir";
            return note;
        }

        private Item CreateShoes()
        {
            Shoes shoes = new Shoes();
            shoes.Name = "Baker's Soft Shoes";
            shoes.Hue = Utility.RandomMinMax(200, 700);
            shoes.Attributes.BonusDex = 8;
            shoes.Attributes.LowerManaCost = 5;
            shoes.SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            return shoes;
        }

        private Item CreateTunic()
        {
            StuddedChest tunic = new StuddedChest();
            tunic.Name = "Jester's Playful Tunic";
            tunic.Hue = Utility.RandomMinMax(500, 800);
            tunic.BaseArmorRating = Utility.RandomMinMax(20, 60);
            tunic.AbsorptionAttributes.EaterKinetic = 20;
            tunic.ArmorAttributes.SelfRepair = 5;
            tunic.Attributes.BonusStam = 40;
            tunic.Attributes.RegenStam = 5;
            tunic.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
            tunic.ColdBonus = 10;
            tunic.EnergyBonus = 10;
            tunic.FireBonus = 10;
            tunic.PhysicalBonus = 10;
            tunic.PoisonBonus = 10;
            return tunic;
        }

        private Item CreateKatana()
        {
            Katana katana = new Katana();
            katana.Name = "Frostflame Katana";
            katana.Hue = Utility.RandomMinMax(300, 500);
            katana.MinDamage = Utility.RandomMinMax(20, 60);
            katana.MaxDamage = Utility.RandomMinMax(60, 90);
            katana.Attributes.BonusStr = 10;
            katana.Attributes.RegenStam = 3;
            katana.Slayer = SlayerName.FlameDousing;
            katana.Slayer2 = SlayerName.WaterDissipation;
            katana.WeaponAttributes.HitFireArea = 30;
            katana.WeaponAttributes.HitColdArea = 30;
            katana.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            katana.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
            return katana;
        }

        public GardenersParadiseChest(Serial serial) : base(serial)
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
