using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FirstNationsHeritageChest : WoodenChest
    {
        [Constructable]
        public FirstNationsHeritageChest()
        {
            Name = "First Nations' Heritage";
            Hue = Utility.Random(1, 1600);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Emerald>("Totem of the Tribes"), 0.22);
            AddItem(CreateNamedItem<Apple>("Sacred Fruit of Life"), 0.28);
            AddItem(CreateNamedItem<TreasureLevel3>("Ancestral Artifact"), 0.24);
            AddItem(CreateNamedItem<GoldEarrings>("Earrings of the Elders"), 0.32);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.12);
            AddItem(CreateNamedItem<MandrakeRoot>("Mystical Herb"), 0.15);
            AddItem(CreateNamedItem<GreaterHealPotion>("Potion of the Spirit Guide"), 0.15);
            AddItem(CreateTunic(), 0.20);
            AddItem(CreatePlateChest(), 0.20);
            AddItem(CreateWarAxe(), 0.20);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Our lands, our stories.";
            note.TitleString = "Tribal Chronicle";
            return note;
        }

        private Item CreateTunic()
        {
            Tunic tunic = new Tunic();
            tunic.Name = "Beastmaster's Tunic";
            tunic.Hue = Utility.RandomMinMax(450, 1450);
            tunic.ClothingAttributes.SelfRepair = 3;
            tunic.Attributes.BonusDex = 10;
            tunic.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
            tunic.SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
            return tunic;
        }

        private Item CreatePlateChest()
        {
            PlateChest plateChest = new PlateChest();
            plateChest.Name = "Lionheart Plate";
            plateChest.Hue = Utility.RandomMinMax(400, 750);
            plateChest.BaseArmorRating = Utility.RandomMinMax(45, 85);
            plateChest.AbsorptionAttributes.EaterCold = 15;
            plateChest.ArmorAttributes.DurabilityBonus = 20;
            plateChest.Attributes.BonusStr = 25;
            plateChest.Attributes.BonusDex = 20;
            plateChest.Attributes.BonusInt = 15;
            plateChest.ColdBonus = 25;
            plateChest.EnergyBonus = 15;
            plateChest.FireBonus = 25;
            plateChest.PhysicalBonus = 20;
            plateChest.PoisonBonus = 20;
            return plateChest;
        }

        private Item CreateWarAxe()
        {
            WarAxe warAxe = new WarAxe();
            warAxe.Name = "Beowulf's WarAxe";
            warAxe.Hue = Utility.RandomMinMax(350, 550);
            warAxe.MinDamage = Utility.RandomMinMax(30, 70);
            warAxe.MaxDamage = Utility.RandomMinMax(70, 110);
            warAxe.Attributes.BonusHits = 20;
            warAxe.Attributes.ReflectPhysical = 10;
            warAxe.WeaponAttributes.HitLeechHits = 20;
            warAxe.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            return warAxe;
        }

        public FirstNationsHeritageChest(Serial serial) : base(serial)
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
