using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zhuge Liang")]
    public class ZhugeLiang : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZhugeLiang() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zhuge Liang";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 130;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1117 });
            AddItem(new Sandals() { Hue = 1117 });
            AddItem(new Mace() { Name = "Zhuge's Fan" });

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
                Say("Greetings, traveler. I am Zhuge Liang.");
            }
            else if (speech.Contains("health"))
            {
                Say("My well-being is of little consequence.");
            }
            else if (speech.Contains("job"))
            {
                Say("My path is one of wisdom and strategy.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True power lies not in might, but in the cunning of the mind. Are you wise?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Wisdom is a treasure, indeed. May you find it on your journey.");
            }
            else if (speech.Contains("zhuge"))
            {
                Say("Ah, you recognize the name. In the ancient lands, I was a strategist, helping kingdoms rise and fall with mere words and plans.");
            }
            else if (speech.Contains("consequence"))
            {
                Say("The physical realm matters little when you are focused on a greater purpose. It is the mind and spirit that guide me now.");
            }
            else if (speech.Contains("strategy"))
            {
                Say("Indeed, strategy is an art. It's more than just plans; it's understanding people, predicting outcomes, and manipulating circumstances for a desired result.");
            }
            else if (speech.Contains("art"))
            {
                Say("Art is not just paintings and music. It's the beauty in thought, the elegance of a well-laid plan, and the grace of execution. Would you like to learn?");
            }
            else if (speech.Contains("learn"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, a keen mind! Very well, here is a scroll containing some of my strategies. May it serve you well on your adventures.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the actual item name for the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("purpose"))
            {
                Say("A man without purpose is like a ship without a compass. My purpose has always been to bring order and balance. Seek your own, and you might find greatness.");
            }

            base.OnSpeech(e);
        }

        public ZhugeLiang(Serial serial) : base(serial) { }

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
