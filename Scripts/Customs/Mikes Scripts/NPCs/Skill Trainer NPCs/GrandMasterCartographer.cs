using System;
using Server;
using Server.Items; 

namespace Server.Mobiles
{
    public class GrandMasterCartographer : BaseCreature
    {
        [Constructable]
        public GrandMasterCartographer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Marco Polo";
            this.Title = "the GrandMaster Cartographer";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equipment for the Cartography trainer
            this.AddItem(new MapmakersPen());
            this.AddItem(new FullApron());
            this.AddItem(new LongPants());
            this.AddItem(new Boots());

            // Add other properties or equipment as needed.
        }

        public GrandMasterCartographer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Cartography skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check, you might want to refine this
            if (dropped is Gold && dropped.Amount >= 1000) // Cost of training: 1000 gold
            {
                if (from.Skills.Cartography.Base < 120)
                {
                    from.Skills.Cartography.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Cartography skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered Cartography.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold as payment for my expertise.");
                return false;
            }
        }
    }
}
