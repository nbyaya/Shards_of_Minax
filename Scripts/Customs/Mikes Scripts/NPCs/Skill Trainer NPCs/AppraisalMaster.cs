using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class AppraisalMaster : BaseCreature
    {
        [Constructable]
        public AppraisalMaster() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Ezren Stoneforge";
            this.Title = "the Appraisal Master";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue();

            this.AddItem(new GnarledStaff()); // You can change this to an appropriate item
            this.AddItem(new Robe());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public AppraisalMaster(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Item Identification skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for the cost of training
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming it costs 1000 gold for training
            {
                if (from.Skills.ItemID.Base < 120)
                {
                    from.Skills.ItemID.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Item Identification skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Item Identification.");
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
