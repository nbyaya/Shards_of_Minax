using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RebelChest : WoodenChest
    {
        [Constructable]
        public RebelChest()
        {
            Name = "Rebel's Chest";
            Hue = Utility.Random(1157, 1158);

            // Add items to the chest
            AddItem(CreateColoredItem<Emerald>("Punk's Gem", 2223), 0.20);
            AddItem(CreateSimpleNote(), 0.16);
            AddItem(CreateNamedItem<TreasureLevel2>("Punk's Loot Cache"), 0.15);
            AddItem(CreateColoredItem<GoldEarrings>("Safety Pin Earring", 1170), 0.15);
            AddItem(new Gold(Utility.Random(1, 3000)), 0.18);
            AddItem(CreateNamedItem<Apple>("Anarchist Apple"), 0.10);
            AddItem(CreateNamedItem<GreaterHealPotion>("DIY Brew"), 0.12);
            AddItem(CreateColoredItem<ThighBoots>("Boots of the Street Rebel", 1120), 0.15);
            AddItem(CreateRandomInstrument(), 0.04);
            AddItem(CreateNamedItem<Spyglass>("Underground's Gaze"), 0.12);
            AddItem(CreateRandomWand(), 0.14);
            AddItem(CreateRandomClothing(), 0.20);
            AddItem(CreateSandals(), 0.20);
            AddItem(CreatePlateArms(), 0.20);
            AddItem(CreateDagger(), 0.20);
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
            note.NoteString = "Rebellion isn't just about noise; it's about a statement.";
            note.TitleString = "Punk Manifesto";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = Utility.RandomList<BaseInstrument>(new Drums());
            instrument.Name = "Smashed Guitar";
            return instrument;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Wand of Raw Power";
            return wand;
        }

        private Item CreateRandomClothing()
        {
            BaseClothing clothing = Utility.RandomList<BaseClothing>(new FancyShirt());
            clothing.Name = "Patched Jacket";
            return clothing;
        }

        private Item CreateSandals()
        {
            Sandals sandals = new Sandals();
            sandals.Name = "Ringmaster's Sandals";
            sandals.Hue = Utility.RandomList(400, 1400);
            sandals.Attributes.BonusStam = 10;
            sandals.Attributes.NightSight = 1;
            sandals.SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
            return sandals;
        }

        private Item CreatePlateArms()
        {
            PlateArms arms = new PlateArms();
            arms.Name = "Toxin Ward";
            arms.Hue = Utility.RandomList(100, 300);
            arms.BaseArmorRating = Utility.Random(35, 75);
            arms.AbsorptionAttributes.EaterPoison = 15;
            arms.ArmorAttributes.DurabilityBonus = 10;
            arms.Attributes.BonusStr = 20;
            arms.Attributes.AttackChance = 10;
            arms.SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            arms.ColdBonus = 5;
            arms.EnergyBonus = 10;
            arms.FireBonus = 10;
            arms.PhysicalBonus = 15;
            arms.PoisonBonus = 25;
            return arms;
        }

        private Item CreateDagger()
        {
            Dagger dagger = new Dagger();
            dagger.Name = "Mage Masher";
            dagger.Hue = Utility.RandomList(860, 880);
            dagger.MinDamage = Utility.Random(20, 40);
            dagger.MaxDamage = Utility.Random(60, 80);
            dagger.Attributes.LowerRegCost = 10;
            dagger.Attributes.AttackChance = 5;
            dagger.Slayer = SlayerName.ElementalBan;
            dagger.WeaponAttributes.HitManaDrain = 30;
            dagger.WeaponAttributes.HitDispel = 20;
            dagger.SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            dagger.SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);
            return dagger;
        }

        public RebelChest(Serial serial) : base(serial)
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
