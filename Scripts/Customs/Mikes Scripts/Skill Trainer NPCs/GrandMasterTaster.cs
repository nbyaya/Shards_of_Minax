using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterTaster : BaseCreature
    {
        [Constructable]
        public GrandMasterTaster() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Gordon Ramsay";
            this.Title = "the GrandMaster Taster";

			this.Body = 0x190; // This is the body value for a generic male NPC
			this.Hue = Utility.RandomSkinHue();

			// Setting the NPC's clothing or equipment:
			this.AddItem(new FullApron());
			this.AddItem(new FancyShirt());
			this.AddItem(new LongPants());
			this.AddItem(new Boots());
			this.AddItem(new Cleaver());  // Chef's cleaver

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterTaster(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Taste Identification skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.TasteID.Base < 120)
                {
                    from.Skills.TasteID.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Taste Identification skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Taste Identification.");
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
