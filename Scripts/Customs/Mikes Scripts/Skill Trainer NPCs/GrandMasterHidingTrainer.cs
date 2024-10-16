using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterHidingTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterHidingTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Batman";
            this.Title = "the GrandMaster of Hiding";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue();

            // Equipment (customize as needed)
            this.AddItem(new Dagger());
            this.AddItem(new SewingKit()); // You can create a custom SewingKit item if needed
            this.AddItem(new FancyShirt());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());
            // Add other properties or equipment as needed
        }

        public GrandMasterHidingTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Hiding skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check: Ensure the player has enough gold for training
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Hiding.Base < 120) // Assuming the maximum skill level is 120
                {
                    from.Skills.Hiding.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Hiding skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered the art of hiding.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold as payment for my teachings.");
                return false;
            }
        }
    }
}
