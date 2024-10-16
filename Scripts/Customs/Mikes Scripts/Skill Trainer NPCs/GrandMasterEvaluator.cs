using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterEvaluator : BaseCreature
    {
        [Constructable]
        public GrandMasterEvaluator() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Einstein";
            this.Title = "the GrandMaster Evaluator";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            this.AddItem(new GnarledStaff());

            // Add other properties as needed.
        }

        public GrandMasterEvaluator(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Evaluating Intelligence skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills[SkillName.EvalInt].Base < 120)
                {
                    from.Skills[SkillName.EvalInt].Base += 0.1;
                    from.SendMessage("Your Evaluating Intelligence skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Evaluating Intelligence.");
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
