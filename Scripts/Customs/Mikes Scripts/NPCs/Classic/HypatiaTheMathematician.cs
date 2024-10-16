using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hypatia the Mathematician")]
    public class HypatiaTheMathematician : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HypatiaTheMathematician() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hypatia the Mathematician";
            Body = 0x191; // Human female body
            
            // Stats
            Str = 72;
            Dex = 48;
            Int = 125;
            Hits = 61;

            // Appearance
            AddItem(new FancyDress() { Hue = 2220 }); // Dress with hue 2220
            AddItem(new Sandals() { Hue = 1178 }); // Sandals with hue 1178
            AddItem(new Spellbook() { Name = "Hypatia's Theorems" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true); // true for female
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
                Say("Greetings, traveler. I am Hypatia the Mathematician.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the principles of mathematics.");
            }
            else if (speech.Contains("job"))
            {
                Say("My vocation, dear traveler, is to explore the intricate web of numbers and patterns, a quest for truth akin to the virtues themselves.");
            }
            else if (speech.Contains("virtues humility"))
            {
                Say("Consider the virtue of Humility, for in the vast realm of numbers, we find that even the most learned are but humble students.");
            }
            else if (speech.Contains("relationship virtue mathematics"))
            {
                Say("Do you ponder the relationship between virtue and mathematics, traveler?");
            }
            else if (speech.Contains("hypatia"))
            {
                Say("I am named after the great scholar of Alexandria, a beacon of wisdom in times of darkness. Her legacy inspires me every day.");
            }
            else if (speech.Contains("principles"))
            {
                Say("The principles of mathematics are unyielding, ever true. They offer a stability in life that few other things can.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is an elusive quarry, yet the hunt for it is what gives meaning to my research. Sometimes, when a traveler shows keen interest, I offer a reward for solving one of my riddles. Are you interested?");
            }
            else if (speech.Contains("humility"))
            {
                Say("In humility, we find the acceptance of our limitations. Even in math, there are problems that remain unsolved, reminding us to be humble.");
            }
            else if (speech.Contains("ponder"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Pondering is the first step towards understanding. When we allow ourselves to wonder, we open doors to new knowledge. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("scholar"))
            {
                Say("Scholars of the past have left behind rich legacies of knowledge. By studying their work, we can progress further than they ever imagined.");
            }
            else if (speech.Contains("stability"))
            {
                Say("With stability comes clarity. In a world full of uncertainties, the constants of mathematics provide an anchor.");
            }
            else if (speech.Contains("wonder"))
            {
                Say("Wonder leads to curiosity, and curiosity is the mother of all discoveries. It's a virtue every mathematician should possess. Here, take this.");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            base.OnSpeech(e);
        }

        public HypatiaTheMathematician(Serial serial) : base(serial) { }

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
