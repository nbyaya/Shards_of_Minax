using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Louis XIV")]
    public class LouisXIV : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LouisXIV() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Louis XIV";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 60;
            Int = 100;
            Hits = 85;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1202 }); // Fancy shirt with hue 1202
            AddItem(new LongPants() { Hue = 1141 }); // Long pants with hue 1141
            AddItem(new Boots() { Hue = 1913 }); // Boots with hue 1913

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Louis XIV, the Sun King of France!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in the best of health, as befits a king!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to rule France with absolute power!");
            }
            else if (speech.Contains("rule"))
            {
                Say("True power lies not in the crown, but in the hearts of the people. Do you understand, adventurer?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, my reign is a testament to the virtues of leadership and power!");
            }
            else if (speech.Contains("sun"))
            {
                Say("Ah, you've heard of my title, the Sun King. It signifies my radiant presence and the golden age of France under my rule.");
            }
            else if (speech.Contains("best"))
            {
                Say("Truly, I attribute my robust health to the daily exercises and the care taken by my royal physicians. They are the finest in all the land.");
            }
            else if (speech.Contains("absolute"))
            {
                Say("With absolute power comes great responsibility. I have advisors, but ultimately, the decisions rest with me. It's not always easy, but it's a duty I bear with pride.");
            }
            else if (speech.Contains("hearts"))
            {
                Say("Yes, winning the hearts of the people is of utmost importance. If they believe in their leader, they will follow him anywhere. For your wisdom in understanding this, I grant you a reward.");
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
                    Say("As a token of my appreciation, accept this small gift from the royal treasury. May it serve you well on your adventures.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as honor, loyalty, and courage are the pillars upon which I've built my reign. It's essential for a ruler to embody these values.");
            }

            base.OnSpeech(e);
        }

        public LouisXIV(Serial serial) : base(serial) { }

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
