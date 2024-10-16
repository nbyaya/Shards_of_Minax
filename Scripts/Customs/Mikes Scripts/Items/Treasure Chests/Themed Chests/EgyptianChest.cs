using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EgyptianChest : WoodenChest
    {
        [Constructable]
        public EgyptianChest()
        {
            Name = "Egyptian Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<GoldBracelet>("Golden Ankh Bracelet"), 0.20);
            AddItem(CreateNamedItem<GreaterHealPotion>("Egyptian Wine"), 0.15);
            AddItem(CreateNamedItem<TreasureLevel3>("Egyptian Treasure"), 0.17);
            AddItem(CreateNamedItem<GoldNecklace>("Golden Scarab Necklace"), 0.20);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.15);
            AddItem(CreateColoredItem<Emerald>("Emerald of the Nile", 1775), 0.12);
            AddItem(CreateNamedItem<GreaterHealPotion>("Aged Egyptian Wine"), 0.08);
            AddItem(CreateNamedItem<Gold>("Egyptian Coin"), 0.16);
            AddItem(CreateColoredItem<Sandals>("Sandals of the Pharaoh", 2213), 0.19);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Eye of Horus Earring"), 0.17);
            AddItem(CreateMap(), 0.04);
            AddItem(CreateNamedItem<GreaterCurePotion>("Bottle of Mysterious Cure"), 0.20);
            AddItem(CreateBandana(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "The secrets of the pyramids are hidden within.";
            note.TitleString = "Egyptian Scroll";
            return note;
        }

        private Item CreateMap()
        {
            SimpleMap map = new SimpleMap();
            map.Name = "Map to Egyptian Tomb";
            map.Bounds = new Rectangle2D(2000, 2000, 400, 400);
            return map;
        }

        private Item CreateBandana()
        {
            Bandana bandana = new Bandana();
            bandana.Name = "Craftsman's Protective Gloves";
            bandana.Hue = Utility.RandomMinMax(720, 1670);
            bandana.ClothingAttributes.DurabilityBonus = 4;
            bandana.Attributes.RegenStam = 2;
            bandana.SkillBonuses.SetValues(0, SkillName.Carpentry, 15.0);
            bandana.SkillBonuses.SetValues(1, SkillName.ArmsLore, 10.0);
            return bandana;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Atziri's Step";
            legs.Hue = Utility.RandomMinMax(100, 300);
            legs.BaseArmorRating = Utility.RandomMinMax(20, 60);
            legs.Attributes.DefendChance = 20;
            legs.Attributes.BonusHits = 40;
            legs.Attributes.RegenHits = 5;
            legs.ColdBonus = 10;
            legs.EnergyBonus = 10;
            legs.FireBonus = 10;
            legs.PhysicalBonus = 10;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Wizardspike";
            dagger.Hue = Utility.RandomMinMax(600, 800);
            dagger.MinDamage = Utility.RandomMinMax(15, 50);
            dagger.MaxDamage = Utility.RandomMinMax(50, 85);
            dagger.Attributes.BonusMana = 50;
            dagger.Attributes.LowerManaCost = 10;
            dagger.Attributes.SpellDamage = 10;
            dagger.Slayer = SlayerName.ElementalBan;
            dagger.WeaponAttributes.HitColdArea = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            dagger.SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
            return dagger;
        }

        public EgyptianChest(Serial serial) : base(serial)
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
