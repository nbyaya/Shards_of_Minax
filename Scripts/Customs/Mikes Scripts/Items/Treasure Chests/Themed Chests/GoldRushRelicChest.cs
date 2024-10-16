using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GoldRushRelicChest : WoodenChest
    {
        [Constructable]
        public GoldRushRelicChest()
        {
            Name = "Gold Rush Relic";
            Hue = Utility.Random(1, 1750);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.06);
            AddItem(CreateNamedItem<Sapphire>("Dawson's Discovery"), 0.27);
            AddItem(CreateGoldItem("Gold Nugget"), 0.30);
            AddItem(CreateNamedItem<TreasureLevel4>("Prospector's Prize"), 0.20);
            AddItem(CreateNamedItem<GoldEarrings>("Stampeders' Stake"), 0.35);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 5500)), 0.12);
            AddItem(CreateLootGem(), 1.0);
            AddItem(CreateWideBrimHat(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateLongsword(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateGoldItem(string name)
        {
            Gold gold = new Gold();
            gold.Name = name;
            return gold;
        }

        private Item CreateNamedItem<T>(string name) where T : Item, new()
        {
            T item = new T();
            item.Name = name;
            return item;
        }

        private Item CreateLootGem()
        {
            // Assuming RandomGem is a custom item or subclass of Gem.
            // Implement it according to your specific details.
            Ruby gem = new Ruby();
            gem.Name = "Gem of the Klondike";
            return gem;
        }

        private Item CreateWideBrimHat()
        {
            WideBrimHat hat = new WideBrimHat();
            hat.Name = "Cartographer's Hat";
            hat.Hue = Utility.RandomMinMax(600, 1400);
            hat.Attributes.CastRecovery = 2;
            hat.SkillBonuses.SetValues(0, SkillName.Cartography, 25.0);
            return hat;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Guardian Angel Arms";
            arms.Hue = Utility.RandomMinMax(300, 700);
            arms.BaseArmorRating = Utility.Random(35, 75);
            arms.Attributes.DefendChance = 20;
            arms.Attributes.EnhancePotions = 15;
            arms.ColdBonus = 25;
            arms.EnergyBonus = 20;
            arms.FireBonus = 25;
            arms.PhysicalBonus = 10;
            arms.PoisonBonus = 20;
            return arms;
        }

        private Item CreateLongsword()
        {
            Longsword longsword = new Longsword();
            longsword.Name = "Joan's Divine Longsword";
            longsword.Hue = Utility.RandomMinMax(50, 250);
            longsword.MinDamage = Utility.Random(20, 60);
            longsword.MaxDamage = Utility.Random(60, 100);
            longsword.Attributes.SpellChanneling = 1;
            longsword.Attributes.BonusHits = 15;
            longsword.Slayer = SlayerName.DaemonDismissal;
            longsword.WeaponAttributes.HitHarm = 30;
            longsword.SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
            return longsword;
        }

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "To the Yukon, where dreams glitter.";
            note.TitleString = "Miner's Memoir";
            return note;
        }

        public GoldRushRelicChest(Serial serial) : base(serial)
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
