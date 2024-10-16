using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterBeggar : BaseCreature
    {
        [Constructable]
        public GrandMasterBeggar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Rags";
            this.Title = "the GrandMaster Beggar";

            this.Body = 0x190; // This is the body value for a generic male NPC
            this.Hue = Utility.RandomSkinHue();

            // Appropriate clothing/equipment for a beggar
            this.AddItem(new Shirt());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());
            this.AddItem(new Cap());

            // Other properties can be added if needed.
        }

        public GrandMasterBeggar(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Begging skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 500) // It costs 500 gold for the Begging training
            {
                if (from.Skills.Begging.Base < 120)
                {
                    from.Skills.Begging.Base += 0.1; // Increase by 0.1
                    from.SendMessage("Your Begging skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have mastered the art of Begging.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("Gold is what I require for my wisdom.");
                return false;
            }
        }
    }
}
