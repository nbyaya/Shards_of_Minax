using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Eldric the Sage")]
    public class EldricTheSage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EldricTheSage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Eldric the Sage";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 50;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 2153 });
            AddItem(new Sandals() { Hue = 2653 });
            AddItem(new WizardsHat() { Hue = 1853 });
            AddItem(new Spellbook() { Name = "Eldric's Tome" });
            
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2041); // Random hair styles
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, traveler. I am Eldric the Sage, keeper of arcane knowledge.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, thank you for asking. The magic I study keeps me in good shape.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard ancient secrets and share wisdom with those worthy.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, seeking secrets, are you? Perhaps you can prove your worthiness.");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("Prove your worthiness by answering my questions. Only then shall you be rewarded.");
            }
            else if (speech.Contains("questions"))
            {
                Say("Very well. Answer these correctly, and you shall be rewarded with a chest of great value.");
            }
            else if (speech.Contains("chest"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait a while before you can receive another reward.");
                }
                else
                {
                    Say("Congratulations on proving your worthiness. Here is the Arcanum Chest, a treasure of great mystery and power.");
                    from.AddToBackpack(new ArcanumChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am here to share my wisdom. Ask me about my name, job, or health to begin.");
            }

            base.OnSpeech(e);
        }

        public EldricTheSage(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
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
