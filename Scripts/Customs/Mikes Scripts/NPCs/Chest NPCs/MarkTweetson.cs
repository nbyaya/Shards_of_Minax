using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mark Tweetson")]
    public class MarkTweetson : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarkTweetson() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mark Tweetson";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new FeatheredHat() { Hue = 1150 });
            AddItem(new BodySash() { Hue = 1150 });
            AddItem(new Spellbook() { Name = "Mark's Notepad" });
            
            Hue = Race.RandomSkinHue(); // Random skin color
            HairItemID = Race.RandomHair(this); // Random hair style
            HairHue = Race.RandomHairHue(); // Random hair color

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
                Say("Hello! I'm Mark Tweetson, your social media guru. I'm here to discuss trends and influence.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? It's all about spreading the latest news and trends. What would you like to know about my job?");
            }
            else if (speech.Contains("trends"))
            {
                Say("Trends are constantly changing. It's vital to stay updated. Interested in how trends affect our lives?");
            }
            else if (speech.Contains("influence"))
            {
                Say("Influence is about making an impact. A good influencer shapes trends. Care to learn more about influence?");
            }
            else if (speech.Contains("impact"))
            {
                Say("Making an impact requires dedication and creativity. What else would you like to discuss about making an impact?");
            }
            else if (speech.Contains("creativity"))
            {
                Say("Creativity drives the most successful social media campaigns. Want tips on how to harness creativity?");
            }
            else if (speech.Contains("tips"))
            {
                Say("One tip is to stay authentic and engaged with your audience. Have you ever tried engaging with your audience?");
            }
            else if (speech.Contains("engaging"))
            {
                Say("Engagement is key to building a loyal following. It requires genuine interaction. Need more advice on engaging with people?");
            }
            else if (speech.Contains("advice"))
            {
                Say("Always listen to feedback and adapt accordingly. If you're interested, I have a special reward for those who truly engage.");
            }
            else if (speech.Contains("feedback"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I'm out of rewards for now. Please come back later.");
                }
                else
                {
                    Say("For your curiosity and engagement, I present to you the Social Media Maven's Chest! It contains many exciting items.");
                    from.AddToBackpack(new SocialMediaMavensChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MarkTweetson(Serial serial) : base(serial) { }

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
