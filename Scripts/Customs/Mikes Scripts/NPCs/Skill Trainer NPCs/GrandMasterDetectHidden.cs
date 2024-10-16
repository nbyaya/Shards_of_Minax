using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterDetectHidden : BaseCreature
    {
        [Constructable]
        public GrandMasterDetectHidden() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Sherlock";
            this.Title = "the GrandMaster Detective";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // You can also set the NPC's clothing or equipment here
            this.AddItem(new SewingKit()); // You can create a custom SewingKit item if needed
            this.AddItem(new FancyShirt());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());
            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterDetectHidden(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Detecting Hidden skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for gold payment (adjust the cost as needed)
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming it costs 1000 gold for training
            {
                if (from.Skills.DetectHidden.Base < 120)
                {
                    from.Skills.DetectHidden.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Detecting Hidden skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great skill in Detecting Hidden.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my teaching services.");
                return false;
            }
        }
    }
}
