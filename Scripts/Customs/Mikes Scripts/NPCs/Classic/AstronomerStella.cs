using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Astronomer Stella")]
    public class AstronomerStella : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AstronomerStella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Astronomer Stella";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 65;
            Int = 110;
            Hits = 60;

            // Appearance
            AddItem(new PlainDress(1125)); // Clothing item with hue 1125
            AddItem(new Shoes(1135)); // Shoes with hue 1135
            AddItem(new WideBrimHat(1106)); // Hat with hue 1106
            
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
                Say("Greetings, traveler. I am Astronomer Stella, a seeker of celestial truths.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, but my mind often soars among the stars.");
            }
            else if (speech.Contains("job"))
            {
                Say("My purpose is to study the cosmos and uncover its mysteries.");
            }
            else if (speech.Contains("cosmos"))
            {
                Say("Do you ever gaze at the night sky, adventurer? The cosmos holds secrets beyond our imagination.");
            }
            else if (speech.Contains("constellation"))
            {
                Say("Ah, a fellow stargazer! Tell me, what constellation do you find most captivating?");
            }
            else if (speech.Contains("celestial"))
            {
                Say("The celestial bodies guide my every move and decision. They have a language of their own, if you're willing to listen.");
            }
            else if (speech.Contains("stars"))
            {
                Say("The stars have been particularly bright recently, suggesting a significant event is on the horizon. Have you noticed any anomalies in the sky?");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("These mysteries often lead me to ancient prophecies. Just recently, I stumbled upon a prophecy that speaks of a hero who will bring balance to our world.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("One of the greatest secrets I've learned is the existence of a rare comet that appears once every century. It's believed that those who witness it are granted a special boon.");
            }
            else if (speech.Contains("stargazer"))
            {
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    Say("It's heartening to meet another stargazer. For your shared interest, take this telescope. It may help you see the stars more clearly.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("I have no reward right now. Please return later.");
                }
            }
            else if (speech.Contains("language"))
            {
                Say("The language of the stars is not written or spoken, but felt. It's a deep connection between the cosmos and one's soul, a bond that few truly understand.");
            }
            else if (speech.Contains("anomalies"))
            {
                Say("I've observed several anomalies recently, like shooting stars taking unpredictable paths or constellations slightly shifting. It's both intriguing and concerning.");
            }
            else if (speech.Contains("prophecy"))
            {
                Say("The prophecy speaks of dark times, but also of hope. It foretells the rise of a beacon of light amidst the darkness, a symbol of hope for all.");
            }
            else if (speech.Contains("comet"))
            {
                Say("This rare comet is named 'Lunar's Grace.' Legend says that those who make a wish upon seeing it will have their deepest desires granted.");
            }
            else if (speech.Contains("telescope"))
            {
                Say("With this telescope, you'll be able to see distant galaxies, nebulas, and even some planets on a clear night. Use it wisely and let the universe inspire you.");
            }

            base.OnSpeech(e);
        }

        public AstronomerStella(Serial serial) : base(serial) { }

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
