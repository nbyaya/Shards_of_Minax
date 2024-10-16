using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterChivalryTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterChivalryTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Sir Lancelot";
            this.Title = "the GrandMaster Chivalry Trainer";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Add chivalry-related equipment or appearance items here if needed

            // Make sure to include these lines to ensure proper functionality
            this.AddItem(new Longsword());
            this.AddItem(new PlateChest());
            this.AddItem(new PlateLegs());
            this.AddItem(new PlateGorget());
            this.AddItem(new PlateArms());
            this.AddItem(new PlateGloves());
            this.AddItem(new PlateHelm());

            // Add other properties, equipment, or appearance items as necessary
        }

        public GrandMasterChivalryTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Chivalry skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for payment and skill level
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming training costs 1000 gold
            {
                if (from.Skills.Chivalry.Base < 120)
                {
                    from.Skills.Chivalry.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Chivalry skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered the ways of Chivalry.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold as payment for my Chivalry training.");
                return false;
            }
        }
    }
}
