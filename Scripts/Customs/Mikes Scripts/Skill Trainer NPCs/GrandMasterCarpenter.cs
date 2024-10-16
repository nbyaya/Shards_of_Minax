using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterCarpenter : BaseCreature
    {
        [Constructable]
        public GrandMasterCarpenter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Jesus";
            this.Title = "the GrandMaster Carpenter";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            this.AddItem(new Saw()); // You can change this to a carpentry tool item
            this.AddItem(new FullApron());
            this.AddItem(new LongPants());
            this.AddItem(new Boots());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterCarpenter(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Carpentry skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // This is just a basic check, you might want more conditions or a different approach
            if (dropped is Gold && dropped.Amount >= 1000) // Assume it costs 1000 gold for the training
            {
                if (from.Skills.Carpentry.Base < 120)
                {
                    from.Skills.Carpentry.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Carpentry skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Carpentry.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my teachings.");
                return false;
            }
        }
    }
}
