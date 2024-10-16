using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Disco Dave")]
    public class DiscoDave : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DiscoDave() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Disco Dave";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomBrightHue() });
            AddItem(new Shoes() { Hue = Utility.RandomBrightHue() });
            AddItem(new Bandana() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldBracelet() { Hue = Utility.RandomBrightHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomBrightHue() });

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
                Say("Hey there! I'm Disco Dave, the coolest cat in town! I'm all about the freshest beats and grooviest moves.");
            }
            else if (speech.Contains("disco"))
            {
                Say("That's right, disco is my thing! The dance floor is where I shine the most. Got any moves to show?");
            }
            else if (speech.Contains("dance"))
            {
                Say("Dancing is the rhythm of life! I always say, 'Put on your dancing shoes and let the music take control!'");
            }
            else if (speech.Contains("shoes"))
            {
                Say("Ah, the shoes! They're essential for any dance party. The right pair can make or break your moves.");
            }
            else if (speech.Contains("moves"))
            {
                Say("Dance moves are the language of the soul. Show me your best move, and maybe I'll show you a secret or two!");
            }
            else if (speech.Contains("secret"))
            {
                Say("Secrets, eh? Every great dancer has a few tricks up their sleeve. But to uncover mine, you'll need to keep the rhythm going.");
            }
            else if (speech.Contains("tricks"))
            {
                Say("Tricks are what make dance extraordinary. You need to master the basics first. Are you ready for a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Here’s the challenge: Show me how you keep the rhythm alive! The better you dance, the better the reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("The reward is worth the effort. If you keep impressing me with your knowledge and enthusiasm, I might just have something special for you.");
            }
            else if (speech.Contains("enthusiasm"))
            {
                Say("Enthusiasm is key in the world of dance and music. If you have it, the sky's the limit. But first, tell me more about your favorite tunes.");
            }
            else if (speech.Contains("tunes"))
            {
                Say("Tunes make the world go round! Whether it's rock, pop, or disco, music connects us all. What's your favorite genre?");
            }
            else if (speech.Contains("genre"))
            {
                Say("Genres define our music taste. Mine is definitely disco. It's all about the beat and the groove!");
            }
            else if (speech.Contains("beat"))
            {
                Say("The beat is the heartbeat of the music. It guides every step and every move on the dance floor.");
            }
            else if (speech.Contains("heart"))
            {
                Say("The heart is where the passion lies. If your heart is in the dance, your moves will shine.");
            }
            else if (speech.Contains("passion"))
            {
                Say("Passion is what drives us to excel. With passion, every dance is a story, and every beat is a step towards greatness.");
            }
            else if (speech.Contains("greatness"))
            {
                Say("Greatness is achieved by those who strive and never give up. Show me your dedication, and the Rad Boombox Trove could be yours!");
            }
            else if (speech.Contains("dedication"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Come back later, and maybe I'll have a special surprise for you.");
                }
                else
                {
                    Say("Your dedication to the rhythm and passion is commendable! Here’s the Rad Boombox Trove as a reward for your efforts!");
                    from.AddToBackpack(new RadBoomboxTrove()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DiscoDave(Serial serial) : base(serial) { }

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
