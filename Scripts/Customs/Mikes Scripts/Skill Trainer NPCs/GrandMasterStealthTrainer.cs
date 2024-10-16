using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterStealthTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterStealthTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Rambo";
            this.Title = "the GrandMaster Stealth Trainer";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equip the NPC with appropriate gear
            this.AddItem(new Dagger()); // You can change this to the weapon of your choice
            this.AddItem(new LeatherGloves()); // You can change this to appropriate gloves
            this.AddItem(new LeatherChest()); // You can change this to appropriate chest armor
            this.AddItem(new LeatherLegs()); // You can change this to appropriate leg armor
            this.AddItem(new LeatherCap()); // You can change this to appropriate headgear

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterStealthTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Stealth skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check: Assume it costs 1000 gold for the training
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Stealth.Base < 120)
                {
                    from.Skills.Stealth.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Stealth skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess masterful stealth skills.");
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
