using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class CocoaDeMerlot : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CocoaDeMerlot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cocoa de Merlot";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Chocolatier";

            // Equip the NPC with a chocolate-themed appearance
            AddItem(new Robe(Utility.RandomPinkHue()));
            AddItem(new Sandals());
            AddItem(new FeatheredHat(Utility.RandomPinkHue()));

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, you have met Cocoa de Merlot, the finest chocolatier in these lands.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to create the most exquisite chocolates and sweets. Care to hear more?");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as sweet as ever, thank you for asking.");
            }
            else if (speech.Contains("chocolates"))
            {
                Say("The art of chocolate-making is as old as time itself. Each piece tells a story of its own.");
            }
            else if (speech.Contains("sweets"))
            {
                Say("Sweets have a magical way of making life just a little bit better.");
            }
            else if (speech.Contains("magical"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please come back later.");
                }
                else
                {
                    Say("For your sweet curiosity and delightful conversation, I present to you the Chocolatier's Treasure Chest.");
                    from.AddToBackpack(new ChocolatierTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am not sure how to respond to that. Perhaps try asking about chocolates or sweets?");
            }

            base.OnSpeech(e);
        }

        public CocoaDeMerlot(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
