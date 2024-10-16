using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SwingTimeChest : WoodenChest
    {
        [Constructable]
        public SwingTimeChest()
        {
            Name = "Swing Time Chest";
            Hue = Utility.Random(1, 1942);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.07);
            AddItem(CreateColoredItem<Emerald>("Duke's Gem", 2334), 0.22);
            AddItem(CreateRandomInstrument(), 0.30);
            AddItem(CreateNamedItem<TreasureLevel2>("Record Album of Classics"), 0.24);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4500)), 0.15);
            AddItem(CreateNamedItem<GoldEarrings>("Swing Dancer's Earrings"), 0.20);
            AddItem(CreateRandomWand(), 0.30);
            AddItem(CreateCloak(), 0.20);
            AddItem(CreateWoodenKiteShield(), 0.20);
            AddItem(CreateWarAxe(), 0.20);
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
            note.NoteString = "It don't mean a thing if it ain't got that swing.";
            note.TitleString = "Swing Lyrics";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            BambooFlute instrument = new BambooFlute();
            instrument.Name = "Vintage Saxophone";
            return instrument;
        }

        private Item CreateRandomWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Music Conductor's Baton";
            return wand;
        }

        private Item CreateCloak()
        {
            Cloak cloak = new Cloak();
            cloak.Name = "Bandit's Hidden Cloak";
            cloak.Hue = Utility.RandomMinMax(1200, 2000);
            cloak.Attributes.LowerRegCost = 10;
            cloak.Attributes.NightSight = 1;
            cloak.SkillBonuses.SetValues(0, SkillName.Hiding, 25.0);
            cloak.SkillBonuses.SetValues(1, SkillName.Stealing, 20.0);
            return cloak;
        }

        private Item CreateWoodenKiteShield()
        {
            WoodenKiteShield shield = new WoodenKiteShield();
            shield.Name = "Tal Rasha's Relic";
            shield.Hue = Utility.RandomMinMax(100, 400);
            shield.BaseArmorRating = Utility.RandomMinMax(25, 60);
            shield.AbsorptionAttributes.EaterCold = 15;
            shield.Attributes.CastRecovery = 2;
            shield.SkillBonuses.SetValues(0, SkillName.Spellweaving, 15.0);
            shield.ColdBonus = 25;
            shield.EnergyBonus = 20;
            shield.FireBonus = 5;
            shield.PhysicalBonus = 15;
            shield.PoisonBonus = 5;
            return shield;
        }

        private Item CreateWarAxe()
        {
            WarAxe axe = new WarAxe();
            axe.Name = "Bismarckian WarAxe";
            axe.Hue = Utility.RandomMinMax(400, 600);
            axe.MinDamage = Utility.RandomMinMax(25, 85);
            axe.MaxDamage = Utility.RandomMinMax(85, 125);
            axe.Attributes.ReflectPhysical = 10;
            axe.Slayer = SlayerName.TrollSlaughter;
            axe.WeaponAttributes.HitCurse = 25;
            axe.SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
            return axe;
        }

        public SwingTimeChest(Serial serial) : base(serial)
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
