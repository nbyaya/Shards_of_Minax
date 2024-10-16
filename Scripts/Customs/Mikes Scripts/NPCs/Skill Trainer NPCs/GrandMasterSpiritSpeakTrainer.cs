using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class GrandMasterSpiritSpeakTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterSpiritSpeakTrainer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Cole";
            this.Title = "the GrandMaster Spirit Speak Trainer";

            this.Body = 0x190; // Generic male NPC body
            this.Hue = Utility.RandomSkinHue(); // Random skin color

            // Add equipment or appearance customization here if needed
            this.AddItem(new SewingKit()); // You can create a custom SewingKit item if needed
            this.AddItem(new FancyShirt());
            this.AddItem(new ShortPants());
            this.AddItem(new Sandals());
            // Make sure to add a Spirit Speak book to the NPC's inventory so players can buy it

            // Set other properties as needed
        }

        public GrandMasterSpiritSpeakTrainer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        // Handle the interaction logic for training Spirit Speak skill
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Basic check: Ensure the player is dropping gold (modify the cost as needed)
            if (dropped is Gold && dropped.Amount >= 1000) // Training costs 1000 gold
            {
                if (from.Skills.SpiritSpeak.Base < 120) // Check if the player's skill is under 120
                {
                    from.Skills.SpiritSpeak.Base += 0.1; // Increase Spirit Speak skill by 0.1 (adjust as needed)
                    from.SendMessage("Your Spirit Speak skill has improved!");
                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("You have mastered the art of Spirit Speak.");
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
