using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterMeditationTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterMeditationTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Buddha";
            this.Title = "the GrandMaster Meditation Trainer";

            this.Body = 0x190; // Body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            this.AddItem(new Robe()); // You can change this to appropriate clothing
            this.AddItem(new Sandals()); // You can change this to appropriate footwear

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterMeditationTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Meditation skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check, you might want more conditions or a different approach
            if (dropped is Gold && dropped.Amount >= 1000) // Assume it costs 1000 gold for the training
            {
                if (from.Skills.Meditation.Base < 120)
                {
                    from.Skills.Meditation.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Meditation skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered Meditation.");
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
