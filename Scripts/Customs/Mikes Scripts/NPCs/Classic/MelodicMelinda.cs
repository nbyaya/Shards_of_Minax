using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Melodic Melinda")]
    public class MelodicMelinda : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MelodicMelinda() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Melodic Melinda";
            Body = 0x191; // Human female body

            // Stats
            Str = 120;
            Dex = 65;
            Int = 80;
            Hits = 85;

            // Appearance
            AddItem(new FancyDress() { Hue = 72 });
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new LeatherGloves() { Name = "Melinda's Melody Gloves" });

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
                Say("Greetings, traveler! I am Melodic Melinda, the bard.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good health, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession is that of a bard. I weave tales and songs.");
            }
            else if (speech.Contains("spirituality"))
            {
                if (speech.Contains("moonstone"))
                {
                    Say("The Moonstone is said to hold immense power, especially when the two moons align. Legends say it's hidden in a secret shrine.");
                }
                else
                {
                    Say("Art and music are expressions of spirituality. Have you pondered the nature of spirituality?");
                }
            }
            else if (speech.Contains("honesty"))
            {
                if (speech.Contains("reward"))
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        Say("True honesty is a gift, and those who uphold it are rewarded. As a token of appreciation for seeking the virtues, here's a small reward for you.");
                        from.AddToBackpack(new MaxxiaScroll()); // Give the reward item
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("The eight virtues guide our lives. Among them, honesty is the foundation. Do you value honesty?");
                }
            }
            else if (speech.Contains("melodic"))
            {
                Say("Ah, Melodic Melinda is a name given to me because of the enchanting melodies I produce with my lute.");
            }
            else if (speech.Contains("good"))
            {
                Say("My spirits are lifted by the joy of music and the natural beauty around us. The forest, in particular, has healing properties.");
            }
            else if (speech.Contains("bard"))
            {
                Say("As a bard, I travel from town to town, sharing stories and songs. My favorite story is about the ancient druid circle.");
            }
            else if (speech.Contains("forest"))
            {
                Say("The forest is not just trees and animals; it's alive with magic and history. There's an old grove I often visit to compose my tunes.");
            }
            else if (speech.Contains("druid"))
            {
                Say("The ancient druids were guardians of nature. They held ceremonies at stone circles where they channeled the earth's energies.");
            }
            else if (speech.Contains("moonstone"))
            {
                Say("The Moonstone is said to hold immense power, especially when the two moons align. Legends say it's hidden in a secret shrine.");
            }

            base.OnSpeech(e);
        }

        public MelodicMelinda(Serial serial) : base(serial) { }

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
