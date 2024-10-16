using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterVeterinarian : BaseCreature
    {
        [Constructable]
        public GrandMasterVeterinarian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Ealiana";
            this.Title = "the GrandMaster Veterinarian";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            this.AddItem(new Bandage()); // Add bandages to the veterinarian's inventory
            this.AddItem(new FullApron());
            this.AddItem(new LongPants());
            this.AddItem(new Boots());

            // Add other properties or equipment as needed.
        }

        public GrandMasterVeterinarian(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Veterinary skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for gold payment (adjust the cost as needed)
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Veterinary.Base < 120)
                {
                    from.Skills.Veterinary.Base += 0.1; // Increase Veterinary skill by 0.1
                    from.SendMessage("Your Veterinary skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess advanced knowledge in Veterinary.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold coins for my teachings.");
                return false;
            }
        }
    }
}
