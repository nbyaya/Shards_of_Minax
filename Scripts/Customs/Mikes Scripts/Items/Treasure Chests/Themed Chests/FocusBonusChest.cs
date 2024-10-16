using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class FocusBonusChest : WoodenChest
    {
        [Constructable]
        public FocusBonusChest()
        {
            Name = "Focus Bonus Chest";
            Hue = Utility.Random(1, 1788);

            // Add items to the chest
            AddItem(CreateNamedItem<Cap>("Focus Cap"), 0.20);
            AddItem(CreateNamedItem<Robe>("Focus Robe"), 0.25);
            AddItem(CreateNamedItem<GoldRing>("Focus Ring"), 0.15);
            AddItem(CreateNamedItem<Necklace>("Focus Necklace"), 0.10);
            AddItem(CreateSimpleNote(), 1.0);
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
            note.NoteString = "These items are imbued with the power of Focus. Wear them to enhance your concentration and magical abilities.";
            note.TitleString = "Focus Bonus Items";
            return note;
        }

        private Item CreateCap()
        {
            Cap cap = new Cap();
            cap.Name = "Focus Cap";
            cap.Attributes.BonusInt = 5;
            cap.Attributes.BonusMana = 10;
            return cap;
        }

        private Item CreateRobe()
        {
            Robe robe = new Robe();
            robe.Name = "Focus Robe";
            robe.Attributes.BonusInt = 10;
            robe.Attributes.RegenMana = 5;
            return robe;
        }

        private Item CreateGoldRing()
        {
            GoldRing ring = new GoldRing();
            ring.Name = "Focus Ring";
            ring.Attributes.BonusInt = 5;
            ring.Attributes.BonusMana = 5;
            return ring;
        }

        private Item CreateNecklace()
        {
            Necklace necklace = new Necklace();
            necklace.Name = "Focus Necklace";
            necklace.Attributes.BonusInt = 10;
            necklace.Attributes.RegenMana = 5;
            return necklace;
        }

        public FocusBonusChest(Serial serial) : base(serial)
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
