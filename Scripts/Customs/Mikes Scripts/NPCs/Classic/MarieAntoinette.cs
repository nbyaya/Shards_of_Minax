using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marie Antoinette")]
    public class MarieAntoinette : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarieAntoinette() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marie Antoinette";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 95;
            Hits = 75;

            // Appearance
            AddItem(new FancyDress() { Hue = 1157 }); // Fancy dress with hue 1157
            AddItem(new Sandals() { Hue = 1905 }); // Sandals with hue 1905

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
                Say("I am Marie Antoinette from the land of France!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in a constant struggle to maintain my integrity.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to inspire compassion in those who cross my path.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Justice is a mirror reflecting one's own actions. Do you seek justice?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your answer reveals much about your character.");
            }
            else if (speech.Contains("france"))
            {
                Say("Ah, France! A land filled with elegance and decadence, but also home to great upheavals. Have you heard about the Revolution?");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Maintaining one's integrity is a challenging task, especially when faced with difficult decisions. Have you faced dilemmas in your journey?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the act of truly understanding and empathizing with another's pain or distress. Have you shown kindness to others?");
            }
            else if (speech.Contains("revolution"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The Revolution was a time of great change and strife in France. Many were sacrificed in the name of equality. Some even say I made a comment about cake, but that's a tale for another day. Here, take this for showing interest in history.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("dilemmas"))
            {
                Say("Every journey is filled with choices, each leading to a different path. Reflecting upon them is the essence of growth. Ever faced a choice that changed your path?");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Acts of kindness, no matter how small, can change someone's world. It's the foundation of true nobility. Have you performed any noble acts?");
            }
            else if (speech.Contains("cake"))
            {
                Say("Ah, the infamous quote that was never truly mine. 'Let them eat cake' was attributed to me, but historians doubt its authenticity. Yet, it reminds us of the weight words carry, doesn't it?");
            }
            else if (speech.Contains("choice"))
            {
                Say("Choices shape our destiny. Some lead to happiness, while others to despair. It's the lessons we take from them that matter. Have you learned something valuable recently?");
            }
            else if (speech.Contains("nobility"))
            {
                Say("True nobility doesn't come from birthright but from one's actions and intentions. To be noble is to serve others selflessly. Do you believe in service?");
            }

            base.OnSpeech(e);
        }

        public MarieAntoinette(Serial serial) : base(serial) { }

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
