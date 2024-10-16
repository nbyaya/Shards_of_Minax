using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterMaceFighter : BaseCreature
    {
        [Constructable]
        public GrandMasterMaceFighter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Gruffudd";
            this.Title = "the GrandMaster Mace Fighter";

            this.Body = 0x190; // This is the body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // This gives the NPC a random skin color

            // Equip Gruffudd with appropriate mace-fighting gear
            this.AddItem(new WarMace());     // Adds a war mace to his equipment
            this.AddItem(new PlateChest());  // Adds a plate chest armor for protection
            this.AddItem(new PlateGloves()); // Adds plate gloves for protection
            this.AddItem(new PlateLegs());   // Adds plate leggings for protection
            this.AddItem(new Boots());       // Adds boots

            // You can continue to customize Gruffudd's appearance and equipment as needed
        }

        public GrandMasterMaceFighter(Serial serial) : base(serial)
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

        // Handle the interaction logic for training the Mace Fighting skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Costs 1000 gold for the training
            {
                if (from.Skills.Macing.Base < 120) 
                {
                    from.Skills.Macing.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Mace Fighting skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Mace Fighting.");
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
