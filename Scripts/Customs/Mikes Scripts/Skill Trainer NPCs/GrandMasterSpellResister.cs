using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterSpellResister : BaseCreature
    {
        [Constructable]
        public GrandMasterSpellResister() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Mordenkanin";
            this.Title = "the GrandMaster of Spell Resistance";

			this.Body = 0x190; // This is the body value for a generic male NPC
			this.Hue = Utility.RandomSkinHue(); // This gives the NPC a random skin color

			// Setting Merlin's attire, you can adjust it as you see fit
			this.AddItem(new Robe(Utility.RandomBlueHue())); // A blue robe 
			this.AddItem(new Sandals());
			this.AddItem(new WizardsHat(Utility.RandomBlueHue())); // A blue wizard's hat 

            // Add other properties as needed, like appearance, equipment, etc.
        }

        public GrandMasterSpellResister(Serial serial) : base(serial)
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

        // Handle the interaction logic for training the Resisting Spells skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1200) // Assuming it costs 1200 gold for the training
            {
                if (from.Skills.MagicResist.Base < 120)
                {
                    from.Skills.MagicResist.Base += 0.1; // Increase by 0.1, adjust as needed
                    from.SendMessage("Your Resisting Spells skill has increased!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You already possess great knowledge in Resisting Spells.");
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
