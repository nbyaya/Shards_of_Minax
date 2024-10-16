using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterThief : BaseCreature
    {
        [Constructable]
        public GrandMasterThief() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Arsène";
            this.Title = "the GrandMaster Thief";

            this.Body = 0x190; // Body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equipment for the thief
            this.AddItem(new Lockpick());
            this.AddItem(new Cloak());
            this.AddItem(new Bandana());

            // Add other properties or equipment as needed
        }

        public GrandMasterThief(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Stealing skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming it costs 1000 gold for the training
            {
                if (from.Skills.Stealing.Base < 120)
                {
                    from.Skills.Stealing.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Stealing skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess exceptional skill in Stealing.");
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
