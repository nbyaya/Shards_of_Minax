using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterFisherman : BaseCreature
    {
        [Constructable]
        public GrandMasterFisherman() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Isaac Walton";
            this.Title = "the GrandMaster Fisherman";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            // Equipment for the Fisherman
            this.AddItem(new FishingPole());
            this.AddItem(new FloppyHat());
            this.AddItem(new Doublet(0x4A5));
            this.AddItem(new LongPants());
            this.AddItem(new Sandals());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterFisherman(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Fishing skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // This is just a basic check, you might want more conditions or a different approach
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Fishing.Base < 120)
                {
                    from.Skills.Fishing.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Fishing skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Fishing.");
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
