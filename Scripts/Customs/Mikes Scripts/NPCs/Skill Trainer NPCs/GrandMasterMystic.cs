using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterMystic : BaseCreature
    {
        [Constructable]
        public GrandMasterMystic() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Alister"; // Change the name to suit your NPC
            this.Title = "the GrandMaster Mystic";

            this.Body = 0x190; // This is the body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // This gives the NPC a random skin color

            // You can also set the NPC's clothing or equipment here, for example:
            this.AddItem(new GnarledStaff()); // Mystic staff, or any appropriate item
            this.AddItem(new Robe());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterMystic(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Mysticism skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // This is just a basic check; you might want more conditions or a different approach
            if (dropped is Gold && dropped.Amount >= 1000) // Let's assume it costs 1000 gold for the training
            {
                if (from.Skills.Mysticism.Base < 120)
                {
                    from.Skills.Mysticism.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Mysticism skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Mysticism.");
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
