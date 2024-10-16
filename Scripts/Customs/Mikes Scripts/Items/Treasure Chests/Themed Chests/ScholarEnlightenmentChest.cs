using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ScholarEnlightenmentChest : WoodenChest
    {
        [Constructable]
        public ScholarEnlightenmentChest()
        {
            Name = "Scholar's Enlightenment";
            Hue = Utility.Random(1, 1300);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateEmerald(), 0.25);
            AddItem(CreateNamedItem<Apple>("Peach of Immortality"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel3>("Scrolls of Wisdom"), 0.22);
            AddItem(CreateNamedItem<SilverNecklace>("Amulet of the Sages"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.18);
            AddItem(CreateRandomInstrument(), 0.20);
            AddItem(CreateScroll(), 0.20);
            AddItem(CreateTunic(), 0.2);
            AddItem(CreatePlateHelm(), 0.2);
            AddItem(CreateBow(), 0.2);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateEmerald()
        {
            Emerald emerald = new Emerald();
            emerald.Name = "Confucian Pearl";
            return emerald;
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
            note.NoteString = "The path to wisdom is paved with knowledge.";
            note.TitleString = "Scholar's Musings";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = Utility.RandomList<BaseInstrument>(new Lute(), new Drums(), new Harp());
            instrument.Name = "Ancient Guqin";
            return instrument;
        }

        private Item CreateScroll()
        {
            MaxxiaScroll scroll = new MaxxiaScroll();
            scroll.Name = "Daoist Scroll";
            return scroll;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Cartographer's Exploratory Tunic";
            tunic.Hue = Utility.RandomMinMax(300, 1300);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusDex = 10;
            tunic.Attributes.BonusInt = 5;
            tunic.SkillBonuses.SetValues(0, SkillName.Cartography, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
            return tunic;
        }

        private Item CreatePlateHelm()
        {
            PlateHelm helm = new PlateHelm();
            helm.Name = "Veil of Steel";
            helm.Hue = Utility.RandomMinMax(100, 400);
            helm.BaseArmorRating = Utility.RandomMinMax(50, 80);
            helm.AbsorptionAttributes.EaterFire = 20;
            helm.ArmorAttributes.DurabilityBonus = 20;
            helm.Attributes.BonusStr = 25;
            helm.Attributes.RegenMana = 5;
            helm.SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            helm.ColdBonus = 10;
            helm.EnergyBonus = 10;
            helm.FireBonus = 20;
            helm.PhysicalBonus = 15;
            helm.PoisonBonus = 10;
            return helm;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Norman Conqueror's Bow";
            bow.Hue = Utility.RandomMinMax(300, 500);
            bow.MinDamage = Utility.RandomMinMax(20, 60);
            bow.MaxDamage = Utility.RandomMinMax(60, 110);
            bow.Attributes.BonusDex = 10;
            bow.Attributes.WeaponSpeed = 5;
            bow.Slayer = SlayerName.DragonSlaying;
            bow.WeaponAttributes.HitFireball = 20;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
            return bow;
        }

        public ScholarEnlightenmentChest(Serial serial) : base(serial)
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
