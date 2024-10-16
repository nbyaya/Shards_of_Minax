using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterNecromancer : BaseCreature
    {
        [Constructable]
        public GrandMasterNecromancer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Morgana";
            this.Title = "the GrandMaster Necromancer";

            this.Body = 0x191; // This is the body value for a generic female NPC
            this.Hue = Utility.RandomSkinHue();

            // Set the NPC's clothing or equipment here:
            this.AddItem(new Robe(Utility.RandomNeutralHue())); // A robe of a neutral hue
            this.AddItem(new Sandals(Utility.RandomNeutralHue())); // Sandals of a neutral hue

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterNecromancer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Necromancy skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1500) // Costs 1500 gold for the training
            {
                if (from.Skills.Necromancy.Base < 120)
                {
                    from.Skills.Necromancy.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Necromancy skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Necromancy.");
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
