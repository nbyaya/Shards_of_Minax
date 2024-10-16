using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rockin' Randy")]
    public class RockinRandy : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RockinRandy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rockin' Randy";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Bandana() { Hue = Utility.RandomBrightHue() });
            AddItem(new Shoes() { Hue = 1150 });
            AddItem(new LeatherCap() { Hue = Utility.RandomBrightHue() });
            AddItem(new Necklace() { Hue = Utility.RandomMetalHue() });

            // Hair and Facial Hair
            HairItemID = 0x203B; // Long hair
            HairHue = 1150;

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
                Say("Hey there, I'm Rockin' Randy, the king of the rock 'n' roll scene! Let's groove!");
            }
            else if (speech.Contains("groove"))
            {
                Say("Groovin' is what it's all about! It's the rhythm of the music and the energy of the crowd.");
            }
            else if (speech.Contains("rhythm"))
            {
                Say("Rhythm is the heartbeat of music. It's what gets your feet moving and your soul dancing.");
            }
            else if (speech.Contains("heartbeat"))
            {
                Say("The heartbeat of rock 'n' roll is its powerful drive and passion. Can you feel it?");
            }
            else if (speech.Contains("passion"))
            {
                Say("Passion fuels the greatest performances. It's the fire in your soul that makes music magical.");
            }
            else if (speech.Contains("fire"))
            {
                Say("The fire of rock 'n' roll burns bright and fierce. It's what keeps the music alive and electrifying.");
            }
            else if (speech.Contains("electrifying"))
            {
                Say("An electrifying performance is unforgettable. It lights up the stage and the hearts of the audience.");
            }
            else if (speech.Contains("stage"))
            {
                Say("The stage is where the magic happens. It's where artists pour their hearts into their music.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Music has a magical way of touching our hearts and bringing people together.");
            }
            else if (speech.Contains("together"))
            {
                Say("Together, we experience the joy of music. It unites us in a shared rhythm and melody.");
            }
            else if (speech.Contains("joy"))
            {
                Say("The joy of music is unparalleled. It lifts our spirits and fills us with happiness.");
            }
            else if (speech.Contains("happiness"))
            {
                Say("Happiness is a key part of the rock 'n' roll lifestyle. It's about enjoying every beat and moment.");
            }
            else if (speech.Contains("beat"))
            {
                Say("The beat of a song drives its energy. It's what keeps everyone in sync and moving.");
            }
            else if (speech.Contains("sync"))
            {
                Say("When everything is in sync, the music flows effortlessly. It's a beautiful harmony of sounds and emotions.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony in music is when different sounds come together to create something wonderful.");
            }
            else if (speech.Contains("wonderful"))
            {
                Say("A wonderful performance leaves a lasting impression. It's the culmination of talent, passion, and connection.");
            }
            else if (speech.Contains("talent"))
            {
                Say("Talent shines through in every performance. It's what sets exceptional musicians apart.");
            }
            else if (speech.Contains("exceptional"))
            {
                Say("Exceptional music makes you feel alive. It transcends ordinary experiences and reaches into your soul.");
            }
            else if (speech.Contains("soul"))
            {
                Say("The soul of rock 'n' roll is its ability to touch and transform people through its power and emotion.");
            }
            else if (speech.Contains("transform"))
            {
                Say("Music has the power to transform our lives. It can change our mood, our outlook, and even our destiny.");
            }
            else if (speech.Contains("destiny"))
            {
                Say("Our destiny is shaped by the experiences and choices we make. Music is a powerful influence on our journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey of music is an adventure through sounds and emotions. Embrace it fully and you'll find something special.");
            }
            else if (speech.Contains("special"))
            {
                Say("Speaking of special, here's a little surprise for you. You've shown a real appreciation for the essence of rock 'n' roll.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've got no more surprises right now. Come back later!");
                }
                else
                {
                    Say("For your enthusiasm and dedication to the spirit of rock 'n' roll, take this Rock 'n' Roll Vault as a reward!");
                    from.AddToBackpack(new RockNRollVault()); // Give the Rock 'n' Roll Vault
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("surprise"))
            {
                Say("Ah, a surprise! You've already shown you've got the spirit of rock 'n' roll. Keep it up!");
            }

            base.OnSpeech(e);
        }

        public RockinRandy(Serial serial) : base(serial) { }

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
