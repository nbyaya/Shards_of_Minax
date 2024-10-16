using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class BoyBandBox : WoodenChest
    {
        [Constructable]
        public BoyBandBox()
        {
            Name = "Boy Band Box";
            Hue = Utility.Random(1, 2700);

            // Add items to the chest
            AddItem(new MaxxiaScroll(), 0.05);
            AddItem(CreateNamedItem<Sapphire>("Blue for Backstreet"), 0.25);
            AddItem(CreateNamedItem<MaxxiaScroll>("NSYNC Golden Ticket"), 0.27);
            AddItem(CreateNamedItem<TreasureLevel2>("Fan Club Collection"), 0.24);
            AddItem(CreateNamedItem<SilverNecklace>("98 Degrees Charm"), 0.38);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 2900)), 0.14);
            AddItem(CreateRandomInstrument(), 0.17);
            AddItem(CreateArmor(), 0.20);
            AddItem(CreateCap(), 0.20);
            AddItem(CreateLeatherChest(), 0.20);
            AddItem(CreatePitchfork(), 0.20);
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

        private Item CreateSimpleNote()
        {
            SimpleNote note = new SimpleNote();
            note.NoteString = "Bye Bye Bye!";
            note.TitleString = "Boy Band Fan Notes";
            return note;
        }

        private Item CreateRandomInstrument()
        {
            Drums instrument = new Drums();
            instrument.Name = "Pop Star Mic";
            return instrument;
        }

        private Item CreateArmor()
        {
            BaseArmor armor = Utility.RandomList<BaseArmor>(new LeatherChest());
            armor.Name = "Pop Poster Armor";
            armor.Hue = Utility.RandomMinMax(300, 700);
            armor.BaseArmorRating = Utility.Random(40, 75);
            return armor;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Trucker's Iconic Cap";
            cap.Hue = Utility.RandomMinMax(300, 1300);
            cap.Attributes.BonusInt = 10;
            cap.SkillBonuses.SetValues(0, SkillName.Focus, 15.0);
            cap.SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
            return cap;
        }

        private Item CreateLeatherChest()
        {
            LeatherChest chest = new LeatherChest();
            chest.Name = "AVALANCHE Defender";
            chest.Hue = Utility.RandomMinMax(300, 700);
            chest.BaseArmorRating = Utility.Random(40, 75);
            chest.AbsorptionAttributes.EaterFire = 20;
            chest.ArmorAttributes.LowerStatReq = 20;
            chest.Attributes.BonusHits = 30;
            chest.Attributes.BonusStr = 20;
            chest.SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            chest.FireBonus = 20;
            chest.PhysicalBonus = 15;
            chest.EnergyBonus = 10;
            chest.PoisonBonus = 10;
            chest.ColdBonus = 5;
            return chest;
        }

        private Item CreatePitchfork()
        {
            Pitchfork pitchfork = new Pitchfork();
            pitchfork.Name = "Neptune's Trident";
            pitchfork.Hue = Utility.RandomMinMax(250, 450);
            pitchfork.MinDamage = Utility.Random(25, 75);
            pitchfork.MaxDamage = Utility.Random(75, 115);
            pitchfork.Attributes.BonusMana = 20;
            pitchfork.Attributes.LowerManaCost = 10;
            pitchfork.Slayer = SlayerName.WaterDissipation;
            pitchfork.Slayer2 = SlayerName.Vacuum;
            pitchfork.WeaponAttributes.HitColdArea = 30;
            pitchfork.WeaponAttributes.HitEnergyArea = 20;
            pitchfork.SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
            return pitchfork;
        }

        public BoyBandBox(Serial serial) : base(serial)
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
