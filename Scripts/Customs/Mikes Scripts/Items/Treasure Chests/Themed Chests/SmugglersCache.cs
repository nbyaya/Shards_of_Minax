using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SmugglersCash : WoodenChest
    {
        [Constructable]
        public SmugglersCash()
        {
            Name = "Smuggler's Cache";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.1);
            AddItem(CreateNamedItem<Sapphire>("Smuggler's Blue Jewel"), 0.21);
            AddItem(CreateColoredItem<GreaterHealPotion>("Contraband Brew", 1491), 0.20);
            AddItem(CreateNamedItem<TreasureLevel4>("Illicit Goods"), 0.23);
            AddItem(CreateNamedItem<GoldEarrings>("Golden Compass Earring"), 0.19);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 7000)), 0.15);
            AddItem(CreateNamedItem<Spyglass>("Smuggler's Lookout Lens"), 0.12);
            AddItem(CreateRandomInstrument(), 0.12);
            AddItem(CreateRandomLoot(), 0.12);
            AddItem(CreateHat(), 0.20);
            AddItem(CreateLeatherCap(), 0.20);
            AddItem(CreateCleaver(), 0.20);
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
            note.NoteString = "If you're reading this, I'm either dead or richer than you.";
            note.TitleString = "Smuggler's Note";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            BaseInstrument instrument = Loot.RandomInstrument();
            instrument.Name = "Smuggler's Tune";
            return instrument;
        }

        private Item CreateRandomLoot()
        {
            Item loot = Loot.RandomArmorOrShieldOrWeaponOrJewelry();
            loot.Name = "Black Market Find";
            return loot;
        }

        private Item CreateHat()
        {
            WideBrimHat hat = new WideBrimHat();
            hat.Name = "Fisherman's Sun Hat";
            hat.Hue = Utility.RandomMinMax(300, 800);
            hat.ClothingAttributes.LowerStatReq = 2;
            hat.Attributes.BonusInt = 5;
            hat.Attributes.RegenStam = 2;
            hat.SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            return hat;
        }

        private Item CreateLeatherCap()
        {
            LeatherCap cap = new LeatherCap();
            cap.Name = "Falconer's Coif";
            cap.Hue = Utility.RandomMinMax(1, 1000);
            cap.BaseArmorRating = Utility.Random(18, 53);
            cap.AbsorptionAttributes.EaterCold = 25;
            cap.ArmorAttributes.SelfRepair = 5;
            cap.Attributes.BonusDex = 25;
            cap.Attributes.RegenHits = 5;
            cap.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            cap.SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
            cap.ColdBonus = 15;
            cap.EnergyBonus = 10;
            cap.FireBonus = 10;
            cap.PhysicalBonus = 10;
            cap.PoisonBonus = 10;
            return cap;
        }

        private Item CreateCleaver()
        {
            Cleaver cleaver = new Cleaver();
            cleaver.Name = "Kaom's Cleaver";
            cleaver.Hue = Utility.RandomMinMax(250, 450);
            cleaver.MinDamage = Utility.Random(30, 70);
            cleaver.MaxDamage = Utility.Random(70, 110);
            cleaver.Attributes.BonusStr = 25;
            cleaver.Attributes.RegenHits = 5;
            cleaver.Slayer = SlayerName.OrcSlaying;
            cleaver.WeaponAttributes.HitFireArea = 30;
            cleaver.WeaponAttributes.BloodDrinker = 10;
            cleaver.SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            return cleaver;
        }

        public SmugglersCash(Serial serial) : base(serial)
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
