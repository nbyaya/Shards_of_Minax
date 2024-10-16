using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sultry Sally")]
    public class SultrySally : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SultrySally() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sultry Sally";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 50;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 1150 });
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 2126 });
            AddItem(new GoldEarrings() { Name = "Sally's Earrings" });

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
                Say("Greetings, darling. I am Sultry Sally.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Why, my dear, it's positively radiant.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession? I provide companionship, darling, for those in search of warmth and intimacy.");
            }
            else if (speech.Contains("valor"))
            {
                Say("But remember, darling, true valor lies not only in the heart of a warrior but also in the tenderness of a lover. Do you agree?");
            }
            else if (speech.Contains("yes") && (speech.Contains("valor") || speech.Contains("love")))
            {
                Say("Ah, a wise answer, my sweet. Just remember that love can be the most powerful force of all.");
            }
            else if (speech.Contains("companionship"))
            {
                Say("Companionship is an art, my dear. It's about understanding, listening, and being there for someone when they need it. Some come for tales, others for comfort.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Over the years, I've heard many stories. From lost treasures to hidden caves, the world is full of secrets. One tale that intrigued me the most is that of the Moonlit Grotto.");
            }
            else if (speech.Contains("grotto"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the mysterious cavern where legends say the water glows under the moonlight. Many adventurers seek it, but few find it. If you ever wish to explore it, take this token. It might help you on your way.");
                    from.AddToBackpack(new BraceletSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("secrets"))
            {
                Say("Darling, the world holds many mysteries. Some are meant to be discovered, while others remain hidden. But the best secret? That's the power of love and connection.");
            }
            else if (speech.Contains("love"))
            {
                Say("Love is a force that moves us, binds us, and sometimes breaks us. But in its purest form, it gives us strength. Have you ever been in love, darling?");
            }
            else if (speech.Contains("no"))
            {
                Say("Do not worry, my dear. Love finds us in the most unexpected places and moments. Always keep your heart open.");
            }

            base.OnSpeech(e);
        }

        public SultrySally(Serial serial) : base(serial) { }

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
