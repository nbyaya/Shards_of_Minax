using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterFocusTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterFocusTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Zenobia";
            this.Title = "the GrandMaster Focus Trainer";

            this.Body = 0x190; // Body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            this.AddItem(new QuarterStaff()); // You can change the equipment as needed
            this.AddItem(new SewingKit()); // You can create a custom SewingKit item if needed
            this.AddItem(new FancyShirt());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());
            // Add other properties and equipment as needed.
        }

        public GrandMasterFocusTrainer(Serial serial) : base(serial)
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

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming it costs 1000 gold for training
            {
                if (from.Skills.Focus.Base < 120)
                {
                    from.Skills.Focus.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Focus skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered the art of Focus.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my training services.");
                return false;
            }
        }
    }
}
