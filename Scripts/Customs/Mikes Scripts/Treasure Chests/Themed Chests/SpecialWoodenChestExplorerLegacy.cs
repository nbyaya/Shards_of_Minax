using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialWoodenChestExplorerLegacy : WoodenChest
    {
        [Constructable]
        public SpecialWoodenChestExplorerLegacy()
        {
            Name = "Explorer's Legacy";
            Hue = Utility.Random(1, 1400);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateNamedItem<Sapphire>("Cabot's Compass"), 0.23);
            AddItem(CreateNamedItem<Spyglass>("Champlain's Vision"), 0.25);
            AddItem(CreateNamedItem<TreasureLevel2>("Map of New France"), 0.20);
            AddItem(CreateNamedItem<SilverNecklace>("Voyageur's Amulet"), 0.30);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4300)), 0.15);
            AddItem(CreateRandomInstrument(), 0.12);
            AddItem(CreateColoredItem<Boots>("Pathfinder's Boots", Utility.RandomList(1, 1400)), 0.08);
            AddItem(CreateMuffler(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateBow(), 0.20);
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
            note.NoteString = "Untouched lands stretched beyond our sight.";
            note.TitleString = "Explorer's Diary";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = new Lute();
            instrument.Name = "Fiddle of the Frontier";
            return instrument;
        }

        private Item CreateMuffler()
        {
            Cap muffler = new Cap();
            muffler.Name = "Nature's Muffler";
            muffler.Hue = Utility.RandomMinMax(900, 1900);
            muffler.ClothingAttributes.SelfRepair = 5;
            muffler.Attributes.BonusInt = 10;
            muffler.Attributes.NightSight = 1;
            muffler.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            muffler.SkillBonuses.SetValues(1, SkillName.TasteID, 20.0);
            return muffler;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Arkaine's Valor Arms";
            arms.Hue = Utility.RandomMinMax(400, 850);
            arms.BaseArmorRating = Utility.Random(45, 85);
            arms.Attributes.BonusStr = 20;
            arms.Attributes.BonusHits = 30;
            arms.Attributes.AttackChance = 15;
            arms.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            arms.SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
            arms.ColdBonus = 15;
            arms.EnergyBonus = 10;
            arms.FireBonus = 15;
            arms.PhysicalBonus = 20;
            arms.PoisonBonus = 10;
            return arms;
        }

        private Item CreateBow()
        {
            Bow bow = new Bow();
            bow.Name = "Archer's Yew Bow";
            bow.Hue = Utility.RandomMinMax(150, 300);
            bow.MinDamage = Utility.Random(35, 55);
            bow.MaxDamage = Utility.Random(55, 85);
            bow.Attributes.WeaponSpeed = 10;
            bow.Attributes.LowerRegCost = 15;
            bow.Slayer = SlayerName.OrcSlaying;
            bow.WeaponAttributes.HitMagicArrow = 25;
            bow.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
            return bow;
        }

        public SpecialWoodenChestExplorerLegacy(Serial serial) : base(serial)
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
