using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Jest")]
    public class JesterJest : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterJest() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Jest";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 90;
            Int = 85;
            Hits = 70;

            // Appearance
            AddItem(new JesterHat() { Hue = 88 }); // Jester Hat with hue 88
            AddItem(new JesterSuit() { Hue = 88 }); // Jester Suit with hue 88
            AddItem(new Sandals() { Hue = 1190 }); // Sandals with hue 1190

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

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
                Say("Greetings, traveler! I am Jester Jest, the merry jester of these lands!");
            }
            else if (speech.Contains("health"))
            {
                Say("Fear not, my health is as vibrant as a rainbow! But I jest, for I am but a humble jester.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? Why, it is to spread laughter and merriment throughout the realm!");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Ah, the virtues! They are like the colors of a rainbow, each one shining in its own unique way. Can you name one of them?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Very good! \"Honesty\" is one of the virtues. In this realm, as in life, honesty is a key to harmony. Do you seek more wisdom about the virtues?");
            }
            else if (speech.Contains("jest"))
            {
                Say("Ah, my moniker is a play on words, for jesting is my nature! Have you heard any of my famous jokes?");
            }
            else if (speech.Contains("rainbow"))
            {
                Say("Rainbows are magical, don't you think? They bring joy and wonder. Would you like to hear a tale about a rainbow I once encountered?");
            }
            else if (speech.Contains("laughter"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Laughter is the best medicine, they say. If you share a joke with me and make me laugh, I might have a special reward for you!");
                    // Optionally add a prompt for a joke if needed.
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("jokes"))
            {
                Say("Ah, one of my favorites goes: \"Why did the scarecrow win an award? Because he was outstanding in his field!\" Do you have any jokes to share?");
            }
            else if (speech.Contains("tale"))
            {
                Say("Once, I saw a rainbow so close, I could touch it! It led me to a mystical grove where fairies danced. Their leader, a sprite named Lila, gave me a charm that ensures my jokes never fall flat!");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("A promise is a promise! Here, take this. It's a trinket that has brought me luck in my travels. May it serve you well!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("scarecrow"))
            {
                Say("Ah, the scarecrow! An age-old symbol of harvest and protection. I've seen some in the farmlands to the west, guarding their crops from pesky crows. Perhaps they could use a jester to keep them company?");
            }
            else if (speech.Contains("lila"))
            {
                Say("Lila, the fairy leader, was a fascinating creature! She told me that laughter and joy have power in their realm. She said that one who spreads joy can even charm the heart of a fairy. Do you believe in the magic of joy?");
            }

            base.OnSpeech(e);
        }

        public JesterJest(Serial serial) : base(serial) { }

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
