using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rockwell Starcaster")]
    public class RockwellStarcaster : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RockwellStarcaster() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rockwell Starcaster";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 90;
            Hits = 80;

            // Appearance
            AddItem(new LeatherCap() { Hue = Utility.RandomNondyedHue() });
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomNondyedHue() });
            AddItem(new Shoes() { Hue = Utility.RandomNondyedHue() });
            AddItem(new Lute() { Name = "Rockstar's Lute" });
            
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Rock star hair
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 1152; // Fun music-themed hue
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
                Say("Hey there! I'm Rockwell Starcaster, the ultimate rock legend.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as vibrant as a live concert! Rocking strong every day.");
            }
            else if (speech.Contains("job"))
            {
                Say("I travel the world, performing epic rock shows and spreading the music.");
            }
            else if (speech.Contains("music"))
            {
                Say("Music is the soul's way of speaking. It's what makes the world go 'round.");
            }
            else if (speech.Contains("concert"))
            {
                Say("Ah, concerts! The energy of a live crowd is what I live for. It’s where the magic happens.");
            }
            else if (speech.Contains("show"))
            {
                Say("Every show is a unique experience. The thrill of the stage never gets old.");
            }
            else if (speech.Contains("legend"))
            {
                Say("The title of 'legend' is earned through dedication and unforgettable performances.");
            }
            else if (speech.Contains("vault"))
            {
                Say("You know about the Vinyl Vault? It's the treasure trove of rock and roll! But only the worthy get to open it.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy, one must embrace the spirit of rock and roll. Prove your dedication to music.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is more than just showing up—it's living the music, feeling every beat, and rocking out with passion.");
            }
            else if (speech.Contains("passion"))
            {
                Say("Passion drives the heart of a performer. It fuels the stage and connects with every soul in the audience.");
            }
            else if (speech.Contains("soul"))
            {
                Say("The soul of rock and roll is about freedom and expression. It's where you find your true self.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom in music means breaking boundaries and letting the rhythm guide you. It’s what makes each performance unique.");
            }
            else if (speech.Contains("performance"))
            {
                Say("A true performance is a journey. It’s the energy, the connection, and the unforgettable moments shared with the crowd.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every rock star’s journey is filled with highs and lows, but it's the dedication that turns every show into a legendary performance.");
            }
            else if (speech.Contains("legendary"))
            {
                Say("Legendary status is reserved for those who truly leave their mark. If you seek to join the ranks of legends, show your commitment.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment is key to mastering any craft. In rock and roll, it’s about giving your all, both on and off the stage.");
            }
            else if (speech.Contains("mastering"))
            {
                Say("Mastering an instrument or a craft takes time, patience, and relentless effort. It’s the essence of a true rock star.");
            }
            else if (speech.Contains("instrument"))
            {
                Say("An instrument is more than just a tool—it's an extension of the performer. It helps convey the emotions and messages of the music.");
            }
            else if (speech.Contains("message"))
            {
                Say("Every great performance has a message. It’s what resonates with the audience and creates a memorable experience.");
            }
            else if (speech.Contains("resonates"))
            {
                Say("When music resonates, it touches the heart and soul of the listener. It's what makes each note and lyric meaningful.");
            }
            else if (speech.Contains("touches"))
            {
                Say("To touch someone's heart is to make a lasting impact. In music, it’s achieved through genuine emotion and powerful performances.");
            }
            else if (speech.Contains("impact"))
            {
                Say("A lasting impact is the mark of a true artist. It’s about leaving something memorable behind that continues to inspire others.");
            }
            else if (speech.Contains("inspire"))
            {
                Say("Inspiration is the driving force behind creativity. It’s what pushes artists to innovate and keep pushing boundaries.");
            }
            else if (speech.Contains("innovation"))
            {
                Say("Innovation in music leads to new sounds and genres. It’s about exploring new horizons and challenging the status quo.");
            }
            else if (speech.Contains("horizons"))
            {
                Say("Exploring new horizons is what keeps the music scene vibrant and ever-evolving. It’s about embracing change and growth.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth as an artist means continually evolving and pushing oneself to new heights. It’s an ongoing journey of discovery.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discoveries in music often lead to breakthroughs and fresh perspectives. It’s the essence of artistic evolution.");
            }
            else if (speech.Contains("evolution"))
            {
                Say("The evolution of music reflects the changing times and tastes. It’s a continuous cycle of adaptation and reinvention.");
            }
            else if (speech.Contains("reinvention"))
            {
                Say("Reinvention is about evolving while staying true to one’s core. It’s what keeps the music fresh and exciting.");
            }
            else if (speech.Contains("exciting"))
            {
                Say("Exciting performances captivate and energize the audience. It’s about delivering something memorable and exhilarating.");
            }
            else if (speech.Contains("memorable"))
            {
                Say("A memorable performance is one that leaves a lasting impression. It’s the result of passion, skill, and connection.");
            }
            else if (speech.Contains("connection"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(15);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You've already claimed your reward. Come back later for more rock and roll!");
                }
                else
                {
                    Say("You've proven your dedication to rock and roll through our dialogue puzzle. For your efforts, take this Vinyl Vault chest as a reward!");
                    from.AddToBackpack(new VinylVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RockwellStarcaster(Serial serial) : base(serial) { }

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
