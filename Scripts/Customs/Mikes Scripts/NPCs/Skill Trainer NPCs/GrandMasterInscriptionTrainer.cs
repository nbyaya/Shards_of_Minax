using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterInscriptionTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterInscriptionTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Ahmes";
            this.Title = "the GrandMaster Scribe";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            this.AddItem(new ScribesPen()); // You can replace this with an appropriate item for your NPC

            // Add other properties or equipment as needed.
        }

        public GrandMasterInscriptionTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Inscription skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for gold payment (you can adjust the cost)
            if (dropped is Gold && dropped.Amount >= 1000) // Assume 1000 gold for training
            {
                if (from.Skills.Inscribe.Base < 120) // Check Inscription skill level
                {
                    from.Skills.Inscribe.Base += 0.1; // Increase skill (adjust as needed)
                    from.SendMessage("Your Inscription skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You've already mastered Inscription.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold as payment for my training.");
                return false;
            }
        }
    }
}
