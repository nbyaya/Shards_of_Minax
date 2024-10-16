using System;
using Server;
using Server.Items; // <-- This line is added to reference the Gold class

namespace Server.Mobiles
{
    public class GrandMasterAnatomy : BaseCreature
    {
        [Constructable]
        public GrandMasterAnatomy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Hippocrates"; // A famous healer's name
            this.Title = "the GrandMaster Healer";

            this.Body = 0x190; // This is the body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // This gives the NPC a random skin color

            // You can also set the NPC's clothing or equipment here, for example:
            this.AddItem(new Bandage(100)); // GrandMaster Healer carries bandages
            this.AddItem(new Robe(Utility.RandomNeutralHue())); // He wears a robe
            this.AddItem(new Sandals());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterAnatomy(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Anatomy skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // This is just a basic check, you might want more conditions or a different approach
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Anatomy.Base < 120)
                {
                    from.Skills.Anatomy.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Anatomy skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Anatomy.");
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
