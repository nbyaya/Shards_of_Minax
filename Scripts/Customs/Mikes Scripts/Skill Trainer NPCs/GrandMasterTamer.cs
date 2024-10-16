using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterTamer : BaseCreature
    {
        [Constructable]
        public GrandMasterTamer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Hagrid"; // An appropriate famous name for a Tamer
            this.Title = "the GrandMaster Tamer";

            this.Body = 0x190; // Body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equip the Tamer with appropriate items (feel free to customize further)
            this.AddItem(new Pitchfork());
            this.AddItem(new LeatherChest()); // Replace with suitable Tamer clothing
            this.AddItem(new LeatherLegs()); // Replace with suitable Tamer clothing
            this.AddItem(new Sandals()); // Replace with suitable Tamer footwear

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterTamer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Animal Taming skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for payment (adjust as needed)
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.AnimalTaming.Base < 120)
                {
                    from.Skills.AnimalTaming.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Animal Taming skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered Animal Taming.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my expertise.");
                return false;
            }
        }
    }
}
