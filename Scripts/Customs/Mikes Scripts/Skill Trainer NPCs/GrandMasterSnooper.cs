using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterSnooper : BaseCreature
    {
        [Constructable]
        public GrandMasterSnooper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Watson"; // Famous name for a snooper
            this.Title = "the GrandMaster Snooper";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equip the NPC with appropriate items (you can adjust these)
            this.AddItem(new LeatherGloves());
            this.AddItem(new Boots());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterSnooper(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Snooping skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for payment (you can adjust the cost)
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming it costs 1000 gold for training
            {
                if (from.Skills.Snooping.Base < 120)
                {
                    from.Skills.Snooping.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Snooping skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Snooping.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my secretive teachings.");
                return false;
            }
        }
    }
}
