using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class GreasersGoldmineChest : WoodenChest
    {
        [Constructable]
        public GreasersGoldmineChest()
        {
            Name = "Greaser's Goldmine";
            Hue = Utility.Random(1, 1930);

            // Add items to the chest
            AddItem(CreateInstrument(), 0.07);
            AddItem(CreateNamedItem<GoldEarrings>("Hot Rod Earrings"), 0.26);
            AddItem(CreateNamedItem<TreasureLevel4>("Motor Oil Can"), 0.30);
            AddItem(CreateSimpleNote(), 1.0);
            AddItem(new Gold(Utility.Random(1, 4800)), 0.14);
            AddItem(CreateWand(), 0.19);
            AddItem(CreateColoredItem<Shoes>("Greaser's Leather Boots", 1590), 0.28);
            AddItem(CreateFancyDress(), 0.20);
            AddItem(CreateGloves(), 0.20);
            AddItem(CreateHalberd(), 0.20);
        }

        private void AddItem(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
            {
                DropItem(item);
            }
        }

        private Item CreateInstrument()
        {
            // Example of a random instrument, you may need to adjust this according to actual instruments
            Lute instrument = new Lute();
            instrument.Name = "Greaser's Comb";
            return instrument;
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
            note.NoteString = "Born to ride!";
            note.TitleString = "Greaser's Journal";
            return note;
        }

        private Item CreateWand()
        {
            Shaft wand = new Shaft();
            wand.Name = "Greaser's Switchblade";
            return wand;
        }

        private Item CreateFancyDress()
        {
            PlainDress dress = new PlainDress();
            dress.Name = "Starlet's Fancy Dress";
            dress.Hue = Utility.RandomMinMax(500, 1000);
            dress.Attributes.BonusInt = 15;
            dress.Attributes.SpellDamage = 5;
            dress.SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            return dress;
        }

        private Item CreateGloves()
        {
            LeatherGloves gloves = new LeatherGloves();
            gloves.Name = "Wrestler's Gripping Gloves";
            gloves.Hue = Utility.RandomMinMax(10, 250);
            gloves.BaseArmorRating = Utility.Random(35, 55);
            gloves.ArmorAttributes.MageArmor = 1;
            gloves.Attributes.WeaponDamage = 10;
            gloves.Attributes.RegenHits = 5;
            gloves.SkillBonuses.SetValues(3, SkillName.Wrestling, 10.0);
            gloves.ColdBonus = 5;
            gloves.EnergyBonus = 5;
            gloves.FireBonus = 5;
            gloves.PhysicalBonus = 15;
            gloves.PoisonBonus = 5;
            return gloves;
        }

        private Item CreateHalberd()
        {
            Halberd halberd = new Halberd();
            halberd.Name = "Sky Piercer";
            halberd.Hue = Utility.RandomMinMax(350, 550);
            halberd.MinDamage = Utility.Random(40, 80);
            halberd.MaxDamage = Utility.Random(80, 130);
            halberd.Attributes.BonusDex = 10;
            halberd.Attributes.WeaponSpeed = 10;
            halberd.Slayer = SlayerName.TrollSlaughter;
            halberd.WeaponAttributes.HitLightning = 35;
            halberd.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            return halberd;
        }

        public GreasersGoldmineChest(Serial serial) : base(serial)
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
