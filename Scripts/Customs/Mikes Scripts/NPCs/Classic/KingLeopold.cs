using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of King Leopold")]
    public class KingLeopold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KingLeopold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "King Leopold";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 60;
            Int = 90;
            Hits = 95;

            // Appearance
            AddItem(new RingmailLegs() { Hue = 2118 });
            AddItem(new RingmailChest() { Hue = 2118 });
            AddItem(new Helmet() { Hue = 2118 });
            AddItem(new RingmailGloves() { Hue = 2118 });
            AddItem(new Boots() { Hue = 2118 });
            AddItem(new WarMace() { Name = "Congo Pacifier" });

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
                Say("I am King Leopold, ruler of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, as a king should be.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to maintain order and justice in these lands.");
            }
            else if (speech.Contains("qualities leader"))
            {
                Say("But tell me, adventurer, what qualities do you believe make a true leader?");
            }
            else if (speech.Contains("hmm") || speech.Contains("yes") || speech.Contains("true"))
            {
                Say("Your response intrigues me. Leadership indeed requires such qualities.");
            }
            else if (speech.Contains("lands"))
            {
                Say("These lands have a rich history, full of valor, honor, and sometimes, treachery. Have you ever heard about the Mantra of Honor?");
            }
            else if (speech.Contains("perfect"))
            {
                Say("Yes, I feel quite fortunate. However, maintaining one's health requires diligence and a touch of wisdom. Do you seek wisdom in your travels?");
            }
            else if (speech.Contains("order"))
            {
                Say("Order is not just about rules and decrees, but also about understanding and upholding the virtues of our land. One such virtue is Honor. Its mantra is a well-guarded secret, but I can share with you that the second syllable is KIR.");
            }
            else if (speech.Contains("qualities"))
            {
                Say("Some believe courage, compassion, and wisdom are essential. Others think of sacrifice and humility. But above all, I value honor and truth. What is your most valued virtue?");
            }
            else if (speech.Contains("mantra"))
            {
                Say("The Mantra of Honor is a chant of great significance. Those who understand it, uphold the virtue of Honor in its true essence. As I mentioned, the second syllable is KIR. Seek the rest, and you shall be closer to mastering Honor.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is not just the accumulation of knowledge, but the application of it. In our lands, the wisest seek to understand the mantras of the virtues. They are keys to inner power.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtues are the pillars upon which our society stands. Each virtue has a mantra, a chant that embodies its essence. The seekers of truth and honor travel the lands to uncover them.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is not the absence of fear, but the strength to face it. Many battles have been won not by the mightiest, but by the bravest. Do you consider yourself brave?");
            }
            else if (speech.Contains("brave"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new Gold(1000)); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public KingLeopold(Serial serial) : base(serial) { }

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
