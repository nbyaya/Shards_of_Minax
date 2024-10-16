using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Melody Jazzington")]
    public class MelodyJazzington : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MelodyJazzington() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Melody Jazzington";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 90;
            Int = 75;
            Hits = 60;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Shoes() { Hue = Utility.RandomMetalHue() });
            AddItem(new JesterHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new Lute() { Name = "Melody's Lute" });
            AddItem(new Cloak() { Name = "Swinging Cloak", Hue = Utility.RandomBrightHue() });

            // Facial features
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
                Say("Ah, greetings! I am Melody Jazzington, at your service. Do you know about rhythm?");
            }
            else if (speech.Contains("rhythm"))
            {
                Say("Rhythm is the heartbeat of music. It guides the dance. Speaking of which, have you heard of swing?");
            }
            else if (speech.Contains("swing"))
            {
                Say("Swing is a lively style of jazz. It moves with a smooth, syncopated beat. Are you familiar with melodies?");
            }
            else if (speech.Contains("melody"))
            {
                Say("A melody is a sequence of notes that is pleasant to hear. It's essential in creating music. Do you enjoy dancing?");
            }
            else if (speech.Contains("dancing"))
            {
                Say("Dancing to good music can lift the spirit. Have you experienced the joy of rhythm?");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is the essence of life. It can be found in music and dance. Speaking of which, do you like surprises?");
            }
            else if (speech.Contains("surprises"))
            {
                Say("Surprises add excitement to life. I have a special reward for those who appreciate music. Are you ready for a gift?");
            }
            else if (speech.Contains("gift"))
            {
                Say("A gift for those who seek the joy of music. However, there's a small challenge first. Have you heard about the swing music era?");
            }
            else if (speech.Contains("swing music era"))
            {
                Say("The swing music era was a golden age of jazz. It brought people together through music. Would you like to know more about the musicians?");
            }
            else if (speech.Contains("musicians"))
            {
                Say("Musicians of the swing era were true artists. They created timeless classics. To unlock your reward, show me your appreciation for melodies and rhythm.");
            }
            else if (speech.Contains("appreciation"))
            {
                Say("Appreciation for music means understanding its power to bring joy. For your enthusiasm, here's a little something. Enjoy!");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've already given out my reward for now. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new SwingTimeChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Let's talk about something related to music. Try asking me about rhythm, melodies, or the joy of dancing.");
            }

            base.OnSpeech(e);
        }

        public MelodyJazzington(Serial serial) : base(serial) { }

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
