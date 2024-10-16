using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Wu Zetian")]
    public class WuZetian : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public WuZetian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Wu Zetian";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 90;
            Int = 90;
            Hits = 80;

            // Appearance
            AddItem(new FancyDress() { Hue = 1102 }); // Clothing item with hue 1102
            AddItem(new OrcMask() { Hue = 1102 }); // Mask with hue 1102
            AddItem(new Sandals() { Hue = 1102 }); // Sandals with hue 1102

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
                Say("I am Wu Zetian, Empress of China!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is none of your business!");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' is to sit here and entertain ignorant wanderers like you!");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Do you think you can handle the challenges of this world, or are you just another fool?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Well, let's see if you can prove me wrong. Are you up for the challenge?");
            }
            else if (speech.Contains("empress"))
            {
                Say("Ah, the title of Empress carries with it a long history of responsibility and power. It was not always easy to obtain or maintain.");
            }
            else if (speech.Contains("business"))
            {
                Say("While it is not your business to know of my health, know that I remain as strong as ever. Strength of both mind and will is necessary to lead.");
            }
            else if (speech.Contains("entertain"))
            {
                Say("Entertainment is but a small facet of my duties. Ruling a vast empire, ensuring its prosperity and security, those are my true tasks.");
            }
            else if (speech.Contains("fool"))
            {
                Say("Fools are plentiful, but those who show wisdom and courage are few. What do you believe sets you apart?");
            }
            else if (speech.Contains("history"))
            {
                Say("The history of my reign is filled with both intrigue and innovation. I've faced many adversities, but also celebrated countless victories.");
            }
            else if (speech.Contains("mind"))
            {
                Say("The mind is the most powerful weapon. It can conceive strategies, birth revolutions, and change the course of an entire nation.");
            }
            else if (speech.Contains("prosperity"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ensuring prosperity is a challenge. It requires balanced governance, thriving trade, and the happiness of the people. In fact, I have something that might help you on your journey. Here, take this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public WuZetian(Serial serial) : base(serial) { }

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
