using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterCamping : BaseCreature
    {
        [Constructable]
        public GrandMasterCamping() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Ray Mears"; // A famous name associated with Camping
            this.Title = "the GrandMaster of Camping";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equip the NPC with appropriate items, such as a Camping Gear item

            // Add other properties or equipment as needed
        }

        public GrandMasterCamping(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Camping skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // This is just a basic check; you can adjust the conditions as needed
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Camping.Base < 120)
                {
                    from.Skills.Camping.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Camping skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You are already a Camping expert.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold coins for my training.");
                return false;
            }
        }
    }
}
