using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Guinevere")]
    public class LadyGuinevere : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyGuinevere() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Guinevere";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new ChainChest() { Hue = 1908 });
            AddItem(new ChainLegs() { Hue = 1908 });
            AddItem(new ChainCoif() { Hue = 1908 });
            AddItem(new PlateGloves() { Hue = 1908 });
            AddItem(new Boots() { Hue = 1908 });
            AddItem(new Broadsword() { Name = "Lady Guinevere's Sword" });

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
                Say("Greetings, traveler. I am Lady Guinevere.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to uphold the virtue of Justice in these lands.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Justice is the cornerstone of virtue, wouldn't you agree?");
            }
            else if (speech.Contains("agree"))
            {
                Say("Indeed, it is the guiding light that leads us to a better world.");
            }
            else if (speech.Contains("guinevere"))
            {
                Say("I hail from the ancient lineage of Avalon, and my name carries the weight of my ancestors' legacy.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, the pristine waters and pure air of this realm keep me rejuvenated. But more than that, it's the love and respect of its people that nourish my spirit.");
            }
            else if (speech.Contains("duty"))
            {
                Say("My duty extends beyond mere title. I ensure that every judgment passed in these lands adheres to the highest standards of fairness and righteousness. It's not an easy task, but it's one I undertake with pride.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is more than just a concept; it's a way of life. For those who truly understand its depth, I sometimes bestow a token of appreciation. Would you like to be tested on your understanding of Justice?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Very well. Answer this: In a village, two men committed a crime. One did it out of greed, the other out of desperation to feed his starving family. Should their punishments be the same or different?");
            }
            else if (speech.Contains("different"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Wise answer. While both committed a wrongdoing, their intentions and circumstances were different. Recognizing this nuance is crucial for true Justice. For your wisdom, I bestow upon you a reward. May it serve you well in your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LadyGuinevere(Serial serial) : base(serial) { }

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
