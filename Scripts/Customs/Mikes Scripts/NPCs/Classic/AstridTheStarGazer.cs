using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Astrid the Star-Gazer")]
    public class AstridTheStarGazer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AstridTheStarGazer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Astrid the Star-Gazer";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 60;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new Kilt(2126));
            AddItem(new Surcoat(1904));
            AddItem(new Sandals(1904));
            AddItem(new Bandana(2126));
            AddItem(new Sextant { Name = "Astrid's Sextant" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Astrid the Star-Gazer, the finest cartographer in this wretched realm.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Who cares about my health when the world around me decays!");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job,\" if you can call it that, is to map this forsaken land for fools who know nothing of its beauty.");
            }
            else if (speech.Contains("stars"))
            {
                Say("Do you have any inkling of the vastness of this world? Have you ever truly gazed upon the stars?");
            }
            else if (speech.Contains("fool"))
            {
                Say("You dare to mock me? Perhaps you're not as much of a fool as the rest. Tell me, what do you see when you gaze upon the stars?");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("Mapping the world is no easy task. But by observing the stars, I've uncovered many hidden truths of our realm.");
            }
            else if (speech.Contains("decays"))
            {
                Say("The skies tell a tale of the world's fate. If only more people would look up and listen.");
            }
            else if (speech.Contains("forsaken"))
            {
                Say("This land may seem desolate to most, but if you look closer, you'll find remnants of its former glory.");
            }
            else if (speech.Contains("gazed"))
            {
                Say("Gazing upon the vastness of the cosmos, I've found ancient constellations that foretell of events to come.");
            }
            else if (speech.Contains("events"))
            {
                Say("There's one particular constellation that has caught my eye lately. It aligns once every millennium. If you help me observe it tonight, I might reward you for your assistance.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Ah, eager for treasure? Very well. Return to me after the stars have aligned, and I shall bestow upon you something special.");
            }
            else if (speech.Contains("special"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The reward I offer isn't materialistic, but knowledge. Knowledge that might guide you on your adventures and perhaps even save your life one day. But have this token.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public AstridTheStarGazer(Serial serial) : base(serial) { }

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
