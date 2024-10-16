using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class ThalionSilverleaf : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ThalionSilverleaf() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Thalion Silverleaf";
            Body = 0x191; // Human male body
            Hue = Utility.RandomSkinHue();
            Title = "the Elven Guardian";

            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new ElvenBoots() { Hue = Utility.RandomMetalHue() });

            // Stats
            Str = 90;
            Dex = 85;
            Int = 100;
            Hits = 70;

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
                Say("Greetings, I am Thalion Silverleaf, keeper of elven treasures.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the ancient treasures of the elven realms, ensuring only the worthy may lay claim to them.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as the ancient oaks. Thank you for your concern.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Our treasures are vast and filled with wonders. Seek them wisely.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy, one must prove their worth through wisdom and valor.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to understanding the true value of treasures. Seek it with an open heart.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is shown not in the battles fought, but in the courage to face them.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Prove your worth by seeking the wisdom of the ancients and the valor of your heart.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait a while before you can receive another reward.");
                }
                else
                {
                    Say("You have shown the wisdom and valor worthy of elven treasures. Accept this Elven Treasury Chest as your reward.");
                    from.AddToBackpack(new ElvenTreasuryChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public ThalionSilverleaf(Serial serial) : base(serial) { }

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
