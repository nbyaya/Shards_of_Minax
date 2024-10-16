using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Charting Charlene")]
    public class ChartingCharlene : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ChartingCharlene() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Charting Charlene";
            Body = 0x191; // Human female body

            // Stats
            Str = 105;
            Dex = 52;
            Int = 118;
            Hits = 72;

            // Appearance
            AddItem(new FancyDress() { Hue = 1124 }); // Clothing item with hue 1124
            AddItem(new Sandals() { Hue = 1165 }); // Sandals with hue 1165

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, traveler! I am Charting Charlene, the cartographer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to map the lands and chart the unexplored territories.");
            }
            else if (speech.Contains("valor"))
            {
                Say("True valor is not just about courage in battle but also about the courage to explore the unknown. Do you seek adventure?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Adventures await those who dare to step into the unknown. May your journeys be filled with discovery and valor!");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("Ah, you've heard of my profession! Cartography is a meticulous task, requiring patience and precision. I've traveled far and wide to create detailed maps.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, exploring and charting keeps me active and vibrant. The fresh air and the thrill of discovery does wonders for one's well-being. Have you ever tried exploring for your health?");
            }
            else if (speech.Contains("unexplored"))
            {
                Say("The unexplored territories are vast and full of mysteries. I've faced many challenges, but the rewards, both material and knowledge, are immense. Would you be interested in some of the artifacts I've found?");
            }
            else if (speech.Contains("maps"))
            {
                Say("My maps are known for their accuracy and detail. They've helped many adventurers and travelers. If you're ever in need of a map, just let me know. For someone as curious as you, I might even offer one as a reward.");
            }
            else if (speech.Contains("exploring"))
            {
                Say("Exploring is not just a profession for me, it's a passion. The thrill of setting foot on untouched lands, the mysteries waiting to be uncovered, it's an experience like no other. Have you encountered any mysteries on your journeys?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Over the years, I've collected various artifacts from different territories. Some are ancient relics, while others are curious oddities. If you prove your valor and help me in my quests, perhaps I could part with one.");
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
                    Say("Ah, you're interested in the reward! Very well, for someone as adventurous as you, I have just the thing. Here, take this. It might come in handy on your travels.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ChartingCharlene(Serial serial) : base(serial) { }

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
