using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterParryTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterParryTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Li Mu Bai";
            this.Title = "the GrandMaster Parry Trainer";

			this.Body = 0x190; // Generic male NPC body value
			this.Hue = Utility.RandomSkinHue(); // Random skin color

			// Setting the NPC's clothing and equipment
			this.AddItem(new Longsword()); // He carries a sword as a parrying master
			this.AddItem(new WoodenShield()); // And a shield to showcase his expertise
			this.AddItem(new FancyShirt());
			this.AddItem(new LongPants());
			this.AddItem(new Boots());
        }

        public GrandMasterParryTrainer(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Parrying skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1500) // Assuming it costs 1500 gold for the training
            {
                if (from.Skills.Parry.Base < 130) // Let's allow it to go beyond 120 for grandmaster level
                {
                    from.Skills.Parry.Base += 0.1; // Increase by 0.1, you can adjust as needed
                    from.SendMessage("Your Parrying skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess grand mastery in Parrying.");
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
