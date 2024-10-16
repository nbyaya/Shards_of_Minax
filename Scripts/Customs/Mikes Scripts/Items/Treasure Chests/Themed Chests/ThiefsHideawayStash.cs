using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ThiefsHideawayStash : WoodenChest
    {
        [Constructable]
        public ThiefsHideawayStash()
        {
            Name = "Thief's Hideaway Stash";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Diamond>("Thief's Sparkling Prize"), 0.22);
            AddItem(CreateColoredItem<DeadlyPoisonPotion>("Nightshade Brew", 1489), 0.19);
            AddItem(CreateNamedItem<TreasureLevel2>("Hidden Rogue Booty"), 0.17);
            AddItem(CreateNamedItem<SilverNecklace>("Silver Lockpick Pendant"), 0.21);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5000)), 0.20);
            AddItem(CreateNamedItem<Dagger>("Shadow Dagger"), 0.13);
            AddItem(CreateRandomClothing(), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Mask Earring"), 0.16);
            AddItem(CreateNamedItem<Spyglass>("Night's Eye Spyglass"), 0.11);
            AddItem(CreateCloak(), 0.2);
            AddItem(CreateLeatherChest(), 0.2);
            AddItem(CreateDagger(), 0.2);
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
            note.NoteString = "Silence is golden, but so are the coins I pilfer.";
            note.TitleString = "Thief's Confession";
            return note;
        }

        private Item CreateRandomClothing()
        {
            BaseClothing clothing = Loot.RandomClothing();
            clothing.Name = "Rogue's Garb";
            return clothing;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Angler's Seabreeze Cloak";
            cloak.Hue = Utility.RandomMinMax(400, 900);
            cloak.ClothingAttributes.SelfRepair = 3;
            cloak.Attributes.BonusDex = 10;
            cloak.Attributes.LowerManaCost = 5;
            cloak.SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
            return cloak;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "Beast Whisperer's Robe";
            chest.Hue = Utility.RandomMinMax(1, 1000);
            chest.BaseArmorRating = Utility.RandomMinMax(20, 60);
            chest.AbsorptionAttributes.EaterKinetic = 25;
            chest.ArmorAttributes.LowerStatReq = 15;
            chest.Attributes.BonusInt = 35;
            chest.Attributes.RegenMana = 5;
            chest.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
            chest.SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
            chest.ColdBonus = 10;
            chest.EnergyBonus = 10;
            chest.FireBonus = 10;
            chest.PhysicalBonus = 10;
            chest.PoisonBonus = 10;
            return chest;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Wind Dancer's Dagger";
            dagger.Hue = Utility.RandomMinMax(450, 650);
            dagger.MinDamage = Utility.RandomMinMax(10, 40);
            dagger.MaxDamage = Utility.RandomMinMax(40, 70);
            dagger.Attributes.BonusDex = 10;
            dagger.Attributes.WeaponSpeed = 5;
            dagger.Slayer = SlayerName.SummerWind;
            dagger.WeaponAttributes.HitFatigue = 30;
            dagger.WeaponAttributes.BattleLust = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            dagger.SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
            return dagger;
        }

        public ThiefsHideawayStash(Serial serial) : base(serial)
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
