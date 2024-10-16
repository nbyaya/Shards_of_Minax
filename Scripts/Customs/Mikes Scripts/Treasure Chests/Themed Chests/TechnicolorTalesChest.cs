using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TechnicolorTalesChest : WoodenChest
    {
        [Constructable]
        public TechnicolorTalesChest()
        {
            Name = "Technicolor Tales";
            Hue = Utility.Random(1, 1850);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateColoredItem<Emerald>("Cinema Reel Gem", 2005), 0.28);
            AddItem(CreateColoredItem<GreaterHealPotion>("Popcorn Potion", 1750), 0.30);
            AddItem(CreateNamedItem<TreasureLevel4>("Golden Era Artifact"), 0.25);
            AddItem(CreateNamedItem<SilverNecklace>("Starlet's Silver Chain"), 0.36);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4700)), 0.14);
            AddItem(CreateRandomWand(), 0.19);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreatePants(), 0.20);
            AddItem(CreateLeatherLegs(), 0.20);
            AddItem(CreateSpear(), 0.20);
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
            note.NoteString = "Behind the scenes and on the screen.";
            note.TitleString = "Cinema Chronicles";
            return note;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Director's Baton";
            return wand;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new PlateChest(), new PlateArms(), new PlateLegs(), new PlateHelm());
            armor.Name = "TV Throwback Threads";
            armor.Hue = Utility.RandomList(1, 1850);
            return armor;
        }

        private Item CreatePants()
        {
            LongPants pants = new LongPants();
            pants.Name = "Groovy Bell-Bottom Pants";
            pants.Hue = Utility.RandomMinMax(250, 1100);
            pants.Attributes.BonusStam = 10;
            pants.Attributes.NightSight = 1;
            pants.SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            return pants;
        }

        private Item CreateLeatherLegs()
        {
            LeatherLegs legs = new LeatherLegs();
            legs.Name = "Courtier's Dashing Boots";
            legs.Hue = Utility.RandomMinMax(700, 950);
            legs.BaseArmorRating = Utility.Random(30, 58);
            legs.AbsorptionAttributes.EaterEnergy = 10;
            legs.ArmorAttributes.MageArmor = 1;
            legs.Attributes.RegenStam = 5;
            legs.Attributes.CastSpeed = 1;
            legs.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            legs.ColdBonus = 5;
            legs.EnergyBonus = 5;
            legs.FireBonus = 5;
            legs.PhysicalBonus = 5;
            legs.PoisonBonus = 10;
            return legs;
        }

        private Item CreateSpear()
        {
            Spear spear = new Spear();
            spear.Name = "Khufu's WarSpear";
            spear.Hue = Utility.RandomMinMax(100, 350);
            spear.MinDamage = Utility.Random(30, 80);
            spear.MaxDamage = Utility.Random(80, 120);
            spear.Attributes.BonusHits = 10;
            spear.Attributes.LowerRegCost = 5;
            spear.WeaponAttributes.HitLightning = 25;
            spear.WeaponAttributes.DurabilityBonus = 20;
            spear.SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
            return spear;
        }

        public TechnicolorTalesChest(Serial serial) : base(serial)
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
