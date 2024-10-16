using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Beastmaster Bella")]
    public class BeastmasterBella : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BeastmasterBella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Beastmaster Bella";
            Body = 0x191; // Human female body

            // Stats
            Str = 93;
            Dex = 70;
            Int = 85;
            Hits = 93;

            // Appearance
            AddItem(new ShortPants(1143));
            AddItem(new FancyShirt(38));
            AddItem(new Boots(1153));
            AddItem(new ShepherdsCrook { Name = "Bella's Beast Stick" });

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
                Say("Greetings, traveler! I am Beastmaster Bella.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an animal tamer. My job is to care for and train creatures of all kinds.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is found in the bond between a tamer and their loyal beasts. Do you seek valor, traveler?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If so, remember that strength alone does not make one valorous. It's the choices we make that define us.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Living close to nature and among animals keeps me feeling robust. The natural world has many secrets. Do you know about herbs?");
            }
            else if (speech.Contains("creatures"))
            {
                Say("I've trained creatures from the fiercest of dragons to the gentlest of rabbits. Each has a lesson to teach. Do you have a favorite beast?");
            }
            else if (speech.Contains("choices"))
            {
                Say("One of the most challenging choices I made was to release a rare beast back into the wild after taming it. It taught me much about freedom. Have you faced such dilemmas?");
            }
            else if (speech.Contains("traditions"))
            {
                Say("Traditions anchor us to our past and guide our future. My family has been taming beasts for generations. Ever heard of the Whispering Woods?");
            }
            else if (speech.Contains("herbs"))
            {
                Say("Herbs are wondrous plants. They can heal, harm, and even charm animals. I often use lavender to calm agitated creatures. Would you like some?");
            }
            else if (speech.Contains("favorite"))
            {
                Say("Everyone has a favorite, mine is the majestic Griffin. Their blend of lion and eagle is truly mesmerizing. Have you encountered one?");
            }
            else if (speech.Contains("dilemmas"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Dilemmas test our character. For making an effort to understand and pondering my challenges, here's a small reward for you. May it aid you on your journey!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("woods"))
            {
                Say("Ah, the Whispering Woods! A place where the trees seem to talk and animals roam freely. It's where I learned most of my skills. Have you been there?");
            }
            else if (speech.Contains("lavender"))
            {
                Say("Lavender is not just calming, but it's also a symbol of peace and purity. In times of doubt, its scent can guide the heart. Do you find solace in nature?");
            }
            else if (speech.Contains("bella"))
            {
                Say("Bella is a name passed down in my family. It means 'beautiful' in an ancient tongue. Have you heard of such traditions?");
            }

            base.OnSpeech(e);
        }

        public BeastmasterBella(Serial serial) : base(serial) { }

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
