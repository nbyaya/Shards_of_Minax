using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Electra Sparkle")]
    public class ElectraSparkle : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ElectraSparkle() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Electra Sparkle";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 85;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomBlueHue() });
            AddItem(new LongPants() { Hue = Utility.RandomPinkHue() });
            AddItem(new Shoes() { Hue = Utility.RandomBrightHue() });
            AddItem(new BodySash() { Hue = Utility.RandomGreenHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomOrangeHue() });

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
                Say("Hello, dazzling traveler! I am Electra Sparkle, the keeper of all things vibrant and electrifying!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am positively radiant and bursting with energy, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to illuminate the world with vibrant colors and guide those who seek the most dazzling treasures.");
            }
            else if (speech.Contains("vibrant"))
            {
                Say("Ah, the word 'vibrant' truly captures the essence of my existence. I am here to share the brilliance of the Neon Nights!");
            }
            else if (speech.Contains("neon nights"))
            {
                Say("The Neon Nights Chest is a beacon of excitement and wonder! To earn it, you'll need to show your enthusiasm and curiosity.");
            }
            else if (speech.Contains("enthusiasm"))
            {
                Say("Enthusiasm is the key to unlocking the most radiant rewards. Let your excitement shine brightly!");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is a wonderful trait. It leads to discovery and adventure. Are you ready to explore the wonders I have to offer?");
            }
            else if (speech.Contains("explore"))
            {
                Say("To explore is to discover the hidden gems of the world. But first, tell me, do you believe in magic?");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is the essence of the extraordinary. It can turn the ordinary into something truly spectacular! Do you have a favorite magical color?");
            }
            else if (speech.Contains("color"))
            {
                Say("Colors reflect the spectrum of our world. Each one has its own unique energy. If you could be any color, what would you choose?");
            }
            else if (speech.Contains("choose"))
            {
                Say("Choosing a color is like choosing a path in life. It reflects who you are and what you aspire to be. Do you feel ready to make a choice?");
            }
            else if (speech.Contains("ready"))
            {
                Say("Being ready is the first step towards achieving your goals. Let’s see how well you know the world of wonders. Have you ever heard of the Light of Lumina?");
            }
            else if (speech.Contains("lumina"))
            {
                Say("The Light of Lumina is said to shine brightly in the darkest of places. It's a symbol of hope and brilliance. Can you feel the light within you?");
            }
            else if (speech.Contains("light"))
            {
                Say("Light guides us through darkness, both literally and metaphorically. It reveals what’s hidden and brightens our path. Are you following your own light?");
            }
            else if (speech.Contains("path"))
            {
                Say("Every path has its own journey. Sometimes the road is clear, and sometimes it is shrouded in mystery. Are you prepared to navigate the unknown?");
            }
            else if (speech.Contains("unknown"))
            {
                Say("The unknown holds both challenges and opportunities. Embrace it with courage and curiosity. Have you ever faced a challenge with a brave heart?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are the tests of our strength and determination. Conquering them brings great rewards. Are you ready to take on one final challenge?");
            }
            else if (speech.Contains("final"))
            {
                Say("The final challenge will test your commitment and understanding. Show me your determination, and the Neon Nights Chest shall be yours.");
            }
            else if (speech.Contains("determination"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The reward is not ready for you at the moment. Please come back later.");
                }
                else
                {
                    Say("Your determination has shone brightly through the challenge! As a reward, please accept the Neon Nights Chest, a treasure filled with radiant surprises!");
                    from.AddToBackpack(new NeonNightsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Your journey through the world of neon and light is just beginning. Keep exploring and discovering, for the world is full of wonders!");
            }

            base.OnSpeech(e);
        }

        public ElectraSparkle(Serial serial) : base(serial) { }

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
