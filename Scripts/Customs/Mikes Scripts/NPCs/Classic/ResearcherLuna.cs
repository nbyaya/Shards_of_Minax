using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Researcher Luna")]
    public class ResearcherLuna : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ResearcherLuna() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Researcher Luna";
            Body = 0x191; // Human female body

            // Stats
            Str = 78;
            Dex = 62;
            Int = 105;
            Hits = 63;

            // Appearance
            AddItem(new ShortPants() { Hue = 1154 });
            AddItem(new Tunic() { Hue = 1107 });
            AddItem(new Sandals() { Hue = 1152 });
            AddItem(new Bonnet() { Hue = 1122 });

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
                Say("Greetings, traveler. I am Researcher Luna, a scientist of Britannia.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is excellent, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a scientist, dedicated to unraveling the mysteries of our world.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("The pursuit of knowledge is a noble endeavor, one that embodies the virtue of spirituality. Do you seek wisdom, traveler?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Indeed, wisdom is a virtue to be cherished. Tell me, traveler, what knowledge do you seek on your journey?");
            }
            else if (speech.Contains("britannia"))
            {
                Say("Britannia is a land rich with history and mysteries. My ancestors have lived here for generations.");
            }
            else if (speech.Contains("excellent"))
            {
                Say("It is essential to maintain one's health when delving into deep research. One must be both physically and mentally prepared.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Currently, I am researching the ancient runes scattered throughout Britannia. They seem to hold secrets from a bygone era.");
            }
            else if (speech.Contains("spirituality"))
            {
                Say("Spirituality is not just about seeking divine connections. It is about understanding oneself and one's purpose in this vast universe.");
            }
            else if (speech.Contains("generations"))
            {
                Say("For generations, my family has been dedicated to science and knowledge. My grandfather even made a significant discovery that shaped our understanding of magic.");
            }
            else if (speech.Contains("physically"))
            {
                Say("Ensuring physical fitness allows me to travel to remote locations, unearthing hidden truths long forgotten.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets within these runes have eluded many, but I believe the key lies in understanding their origins. If you help me decipher one, I'll reward you for your efforts.");
            }
            else if (speech.Contains("universe"))
            {
                Say("Our universe is vast and complex. Some believe in destiny, while others believe in forging their own path. Which do you believe in, traveler?");
            }
            else if (speech.Contains("discovery"))
            {
                Say("His discovery was a rare tome that described how energies could be channeled to perform wonders. It's a treasured family heirloom now.");
            }
            else if (speech.Contains("truths"))
            {
                Say("Hidden truths often lie in places least expected. I've found artifacts in simple caves and atop mountains alike.");
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
                    Say("Your aid would be invaluable. As a token of appreciation, I'll share a rare gem from my collection.");
                    from.AddToBackpack(new HidingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ResearcherLuna(Serial serial) : base(serial) { }

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
