using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterMage : BaseCreature
    {
        [Constructable]
        public GrandMasterMage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Gandalf";
            this.Title = "the GrandMaster Mage";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue();

            // Add equipment for the mage
            this.AddItem(new Robe(Utility.RandomNeutralHue()));
            this.AddItem(new WizardsHat(Utility.RandomNeutralHue()));
            this.AddItem(new Sandals());

            // Add other properties as needed
        }

        public GrandMasterMage(Serial serial) : base(serial)
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
            if (dropped is Gold && dropped.Amount >= 1000)
            {
                if (from.Skills.Magery.Base < 120)
                {
                    from.Skills.Magery.Base += 0.1;
                    from.SendMessage("Your Magery skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Magery.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold for my magical teachings.");
                return false;
            }
        }
    }
}
