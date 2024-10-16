using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterTinker : BaseCreature
    {
        [Constructable]
        public GrandMasterTinker() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Leonardo";
            this.Title = "the GrandMaster Tinker";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            // Equip the tinker's tools
            this.AddItem(new TinkerTools());

            // You can also add other equipment if needed
            this.AddItem(new Robe());
            this.AddItem(new Sandals());

            // Add other properties as needed
        }

        public GrandMasterTinker(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Tinkering skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check for payment, assuming it costs 1000 gold for training
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Tinkering.Base < 120)
                {
                    from.Skills.Tinkering.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Tinkering skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You've already reached a high level of expertise in Tinkering.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my tutelage.");
                return false;
            }
        }
    }
}
