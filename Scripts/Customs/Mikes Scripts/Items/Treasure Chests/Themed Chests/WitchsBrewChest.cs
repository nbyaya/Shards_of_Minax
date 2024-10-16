using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class WitchsBrewChest : WoodenChest
    {
        [Constructable]
        public WitchsBrewChest()
        {
            Name = "Witch's Brew Chest";
            Hue = 1175;

            // Add items to the chest
            AddItem(CreateNamedItem<Emerald>("Cauldron's Heart"), 0.05);
            AddItem(CreateSimpleNote(), 0.18);
            AddItem(CreateNamedItem<TreasureLevel2>("Enchanted Herb Bundle"), 0.17);
            AddItem(CreateColoredItem<GoldEarrings>("Batwing Earring", 1109), 0.15);
            AddItem(new Gold(Utility.Random(1, 3000)), 0.20);
            AddItem(CreateNamedItem<Apple>("Cursed Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("Witch's Special Brew"), 0.16);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Nightshade", 1168), 0.19);
            AddItem(CreateRandomPotion(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Seer's Spyglass"), 0.12);
            AddItem(CreateRandomNecromancyReagent(), 0.13);
            AddItem(CreateRandomWand(), 0.18);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateChainArms(), 0.20);
            AddItem(CreateWarMace(), 0.20);
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
            note.NoteString = "Brew with caution, for magic is unpredictable.";
            note.TitleString = "Witch's Recipe";
            return note;
        }

        private Item CreateRandomPotion()
        {
            // Assuming RandomPotion class exists, replace with actual logic if different
            Item potion = Loot.RandomPotion();
            potion.Name = "Mystical Elixir";
            return potion;
        }

        private Item CreateRandomNecromancyReagent()
        {
            // Assuming RandomNecromancyReagent class exists, replace with actual logic if different
            Item reagent = Loot.RandomNecromancyReagent();
            reagent.Name = "Witch's Secret Herb";
            return reagent;
        }

        private Item CreateRandomWand()
        {
            // Assuming RandomWand class exists, replace with actual logic if different
            Item wand = Loot.RandomWand();
            wand.Name = "Wand of Dark Magic";
            return wand;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Molten Cloak";
            cloak.Hue = Utility.RandomMinMax(200, 800);
            cloak.ClothingAttributes.DurabilityBonus = 5;
            cloak.Attributes.RegenHits = 2;
            cloak.SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
            return cloak;
        }

        private Item CreateChainArms()
        {
            PlateArms chainArms = new PlateArms();
            chainArms.Name = "Gloomfang Chain";
            chainArms.Hue = Utility.RandomMinMax(250, 750);
            chainArms.BaseArmorRating = Utility.Random(30, 75);
            chainArms.AbsorptionAttributes.EaterPoison = 35;
            chainArms.Attributes.BonusHits = -30;
            chainArms.PoisonBonus = 25;
            chainArms.SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
            chainArms.ColdBonus = 10;
            chainArms.EnergyBonus = 10;
            chainArms.FireBonus = 10;
            return chainArms;
        }

        private Item CreateWarMace()
        {
            WarMace mace = new WarMace();
            mace.Name = "The Undead Crown";
            mace.Hue = Utility.RandomMinMax(650, 800);
            mace.MinDamage = Utility.Random(20, 50);
            mace.MaxDamage = Utility.Random(50, 80);
            mace.Attributes.BonusInt = 15;
            mace.WeaponAttributes.HitLeechHits = 25;
            mace.SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            return mace;
        }

        public WitchsBrewChest(Serial serial) : base(serial)
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
