using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterRemoveTrapTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterRemoveTrapTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Houdini"; // Famous name for the NPC
            this.Title = "the GrandMaster Remove Trap Trainer";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Equipment
            this.AddItem(new SmithHammer());
            this.AddItem(new FullApron());
            this.AddItem(new LongPants());
            this.AddItem(new Boots());

            // Add other properties as needed
        }

        public GrandMasterRemoveTrapTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Remove Trap skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for gold payment
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming training costs 1000 gold
            {
                if (from.Skills.RemoveTrap.Base < 120)
                {
                    from.Skills.RemoveTrap.Base += 0.1; // Increase by 0.1 (adjust as needed)
                    from.SendMessage("Your Remove Trap skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered the art of Remove Trap.");
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
