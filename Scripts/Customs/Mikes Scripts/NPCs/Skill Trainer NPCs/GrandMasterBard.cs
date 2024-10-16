using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterBard : BaseCreature
    {
        [Constructable]
        public GrandMasterBard() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Orpheus";
            this.Title = "the GrandMaster Bard";

            this.Body = 0x190;
            this.Hue = Utility.RandomSkinHue();

            // Add Bard-specific equipment here, like a musical instrument and clothing
            this.AddItem(new LapHarp());
            this.AddItem(new Cloak());
            this.AddItem(new Sandals());

            // Add other properties as needed
        }

        public GrandMasterBard(Serial serial) : base(serial)
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

        // Handle the interaction logic for training Discordance skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold && dropped.Amount >= 1000) // Assuming it costs 1000 gold for training
            {
                if (from.Skills.Discordance.Base < 120)
                {
                    from.Skills.Discordance.Base += 0.1; // Increase Discordance skill by 0.1, adjust as needed
                    from.SendMessage("Your Discordance skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have already mastered Discordance.");
                    return false;
                }
            }
            else
            {
                from.SendMessage("I require gold coins for my teachings.");
                return false;
            }
        }
    }
}
