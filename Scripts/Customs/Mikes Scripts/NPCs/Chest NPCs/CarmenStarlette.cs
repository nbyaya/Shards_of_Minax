using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Carmen Starlette")]
    public class CarmenStarlette : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CarmenStarlette() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Carmen Starlette";
            Body = 0x190; // Human male body, change to female if preferred

            // Stats
            Str = 75;
            Dex = 70;
            Int = 90;
            Hits = 60;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new Kilt() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new FeatheredHat() { Hue = 1153 });
            AddItem(new BodySash() { Hue = 1153 });
            
            Hue = Utility.RandomBrightHue(); // Colorful appearance
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Hair styles
            HairHue = Utility.RandomHairHue();

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

            // Initial responses
            if (speech.Contains("name"))
            {
                Say("Hello there, I'm Carmen Starlette, the sensation of the stage! Ask me about my job or health.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to dazzle audiences with my music and charm. But tell me, what do you think about music?");
            }
            else if (speech.Contains("health"))
            {
                Say("I’m in fabulous health, darling! Just finished a whirlwind tour. How do you feel about the charm of a pop star?");
            }
            // Intermediate responses
            else if (speech.Contains("music"))
            {
                Say("Music is the heartbeat of my existence. It’s what keeps me vibrant and alive. Have you seen the audience I perform for?");
            }
            else if (speech.Contains("charm"))
            {
                Say("Charm is all about captivating the audience and making every moment magical. Ever wondered about the fame that comes with it?");
            }
            else if (speech.Contains("audience"))
            {
                Say("My audience is the reason I perform. They give me energy and inspiration. It’s all part of the grand stage performance.");
            }
            else if (speech.Contains("vibrant"))
            {
                Say("Vibrancy comes from living life to the fullest and expressing oneself freely. It all culminates in the ultimate glam of the stage.");
            }
            // Advanced responses
            else if (speech.Contains("fame"))
            {
                Say("Fame is a double-edged sword. It brings glory but also scrutiny. The stage is where I shine the brightest, though.");
            }
            else if (speech.Contains("stage"))
            {
                Say("The stage is my domain, where I captivate and entertain. It’s where all the magic happens. What do you think about a performance?");
            }
            else if (speech.Contains("performance"))
            {
                Say("A performance is a symphony of talent, preparation, and audience interaction. It’s what makes every show memorable.");
            }
            else if (speech.Contains("glam"))
            {
                Say("Glam is not just about appearance, it’s about attitude and presence. It’s the sparkle that makes everything shine.");
            }
            // Final reward
            else if (speech.Contains("sparkle"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Come back later for more glitter and glam!");
                }
                else
                {
                    Say("For your keen interest and exploration, I present to you the Pop Star's Trove!");
                    from.AddToBackpack(new PopStarsTrove()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Feel free to ask me about my name, job, or health, and I’ll share more about the life of a pop star!");
            }

            base.OnSpeech(e);
        }

        public CarmenStarlette(Serial serial) : base(serial) { }

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
