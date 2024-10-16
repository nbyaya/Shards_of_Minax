using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterTracker : BaseCreature
    {
        [Constructable]
        public GrandMasterTracker() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Bear Grylls";
            this.Title = "the GrandMaster Tracker";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            this.AddItem(new Bow());
            this.AddItem(new LeatherChest());
            this.AddItem(new LeatherGloves());
            this.AddItem(new LeatherLegs());
            this.AddItem(new LeatherGorget());
            this.AddItem(new Boots());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterTracker(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        // Handle the interaction logic for training Tracking skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Adjust the gold cost and skill level requirements as needed
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Tracking.Base < 120)
                {
                    from.Skills.Tracking.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Tracking skill has improved under Bear Grylls' guidance!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Tracking.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("Bear Grylls requires gold for his training.");
                return false;
            }
        }
    }
}
