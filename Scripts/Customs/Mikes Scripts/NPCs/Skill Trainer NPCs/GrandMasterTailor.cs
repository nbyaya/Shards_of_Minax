using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterTailor : BaseCreature
    {
        [Constructable]
        public GrandMasterTailor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Giorgio Armani"; // Famous tailor's name
            this.Title = "the GrandMaster Tailor";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue();

            // Equip the tailor with appropriate items
            this.AddItem(new SewingKit()); // You can create a custom SewingKit item if needed
            this.AddItem(new FancyShirt());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterTailor(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Tailoring skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check, you might want more conditions or a different approach
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Tailoring.Base < 120)
                {
                    from.Skills.Tailoring.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Tailoring skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Tailoring.");
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
