using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterPoisoner : BaseCreature
    {
        [Constructable]
        public GrandMasterPoisoner() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Julius Poisonblade";
            this.Title = "the GrandMaster Poisoner";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            this.AddItem(new Dagger()); // You can adjust the equipment as needed
            this.AddItem(new LeatherChest()); // Leather armor, for example

            // Add other properties, appearance, and equipment as needed
        }

        public GrandMasterPoisoner(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Poisoning skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Assume training costs 1000 gold
            {
                if (from.Skills.Poisoning.Base < 120)
                {
                    from.Skills.Poisoning.Base += 0.1; // Increase Poisoning skill by 0.1, adjust as needed
                    from.SendMessage("Your Poisoning skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You are already a master of Poisoning.");
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
