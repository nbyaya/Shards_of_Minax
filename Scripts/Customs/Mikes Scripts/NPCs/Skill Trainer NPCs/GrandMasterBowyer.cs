using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterBowyer : BaseCreature
    {
        [Constructable]
        public GrandMasterBowyer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Robin";
            this.Title = "the GrandMaster Bowyer";

			this.Body = 0x190; // This is the body value for a generic male NPC
			this.Hue = Utility.RandomSkinHue(); // This gives the NPC a random skin color

			// Set the NPC's clothing or equipment
			this.AddItem(new Bow());
			this.AddItem(new LeatherChest());
			this.AddItem(new LeatherLegs());
			this.AddItem(new Boots());
			this.AddItem(new FeatheredHat());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterBowyer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Fletching & Fletching skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Fletching.Base < 120)
                {
                    from.Skills.Fletching.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Bowcraft & Fletching skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Bowcraft & Fletching.");
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
