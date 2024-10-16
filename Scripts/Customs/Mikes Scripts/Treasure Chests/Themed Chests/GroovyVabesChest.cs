using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GroovyVabesChest : WoodenChest
    {
        [Constructable]
        public GroovyVabesChest()
        {
            Name = "Groovy Vibes";
            Hue = Utility.Random(1, 1900);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.08);
            AddItem(CreateColoredItem<Sapphire>("Disco Ball Gem", 2446), 0.25);
            AddItem(CreateColoredItem<GreaterHealPotion>("Boogie Brew", 1700), 0.27);
            AddItem(CreateNamedItem<TreasureLevel1>("Dance Floor Relic"), 0.24);
            AddItem(CreateNamedItem<SilverNecklace>("Disco Fever Pendant"), 0.40);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4000)), 0.15);
            AddItem(CreateInstrument("Groove Guitar"), 0.18);
            AddItem(CreateJewelry("Dancing Queen's Bracelet"), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreateLeatherArms(), 0.20);
            AddItem(CreateButcherKnife(), 0.20);
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
            note.NoteString = "Stayin' alive, stayin' alive.";
            note.TitleString = "Dance Floor Diary";
            return note;
        }

        private Item CreateInstrument(string name)
        {
            Lute instrument = new Lute();
            instrument.Name = name;
            return instrument;
        }

        private Item CreateJewelry(string name)
        {
            Diamond jewelry = new Diamond();
            jewelry.Name = name;
            jewelry.Hue = Utility.RandomList(1, 1700);
            return jewelry;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Beatnik's Beret";
            cap.Hue = Utility.RandomList(1, 50);
            cap.Attributes.BonusInt = 15;
            cap.Attributes.LowerManaCost = 5;
            cap.SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);
            cap.SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
            return cap;
        }

        private Item CreateLeatherArms()
        {
            LeatherArms arms = new LeatherArms();
            arms.Name = "Alchemist's Grounded Boots";
            arms.Hue = Utility.RandomMinMax(500, 750);
            arms.BaseArmorRating = Utility.RandomMinMax(20, 55);
            arms.AbsorptionAttributes.EaterEnergy = 10;
            arms.ArmorAttributes.ReactiveParalyze = 1;
            arms.Attributes.BonusStam = 10;
            arms.Attributes.RegenHits = 5;
            arms.SkillBonuses.SetValues(0, SkillName.Camping, 15.0);
            arms.ColdBonus = 5;
            arms.EnergyBonus = 10;
            arms.FireBonus = 15;
            arms.PhysicalBonus = 15;
            arms.PoisonBonus = 5;
            return arms;
        }

        private Item CreateButcherKnife()
        {
            ButcherKnife knife = new ButcherKnife();
            knife.Name = "Inuit Ulu of the North";
            knife.Hue = Utility.RandomMinMax(100, 300);
            knife.MinDamage = Utility.RandomMinMax(15, 50);
            knife.MaxDamage = Utility.RandomMinMax(50, 75);
            knife.Attributes.BonusInt = 5;
            knife.Attributes.LowerManaCost = 10;
            knife.WeaponAttributes.HitColdArea = 25;
            knife.WeaponAttributes.SelfRepair = 3;
            knife.SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);
            knife.SkillBonuses.SetValues(1, SkillName.Cooking, 10.0);
            return knife;
        }

        public GroovyVabesChest(Serial serial) : base(serial)
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
