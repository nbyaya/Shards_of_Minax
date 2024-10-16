using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class MasterForensicsExpert : BaseCreature
    {
        [Constructable]
        public MasterForensicsExpert() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Dexter";
            this.Title = "the Master Forensics Expert";

            this.Body = 0x190; // This is the body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // This gives the NPC a random skin color

            // Set the NPC's clothing or equipment here:
            this.AddItem(new FancyShirt());
            this.AddItem(new LongPants());
            this.AddItem(new Boots());
            this.AddItem(new WideBrimHat());
            this.AddItem(new Cloak());

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public MasterForensicsExpert(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Forensic Evaluation skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1500) // Assuming it costs 1500 gold for the training
            {
                if (from.Skills.Forensics.Base < 120)
                {
                    from.Skills.Forensics.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Forensic Evaluation skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Forensic Evaluation.");
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
