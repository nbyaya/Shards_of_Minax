using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hollywood Harlan")]
    public class HollywoodHarlan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HollywoodHarlan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hollywood Harlan";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1150 });
            AddItem(new FeatheredHat() { Hue = 1150 });
            AddItem(new FancyDress() { Hue = 1150 }); // Heavily themed

            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = Utility.RandomList(0x203B, 0x2049); // Hair styles
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 1150; // Thematic color
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
                Say("Ah, you've found Hollywood Harlan, the star of the show! I am the one who knows the secrets of the silver screen.");
            }
            else if (speech.Contains("silver screen"))
            {
                Say("The silver screen, a place where dreams and stories come alive. Tell me, do you have a favorite movie genre?");
            }
            else if (speech.Contains("genre"))
            {
                Say("A fine choice! But tell me, who is your favorite actor or actress?");
            }
            else if (speech.Contains("actor") || speech.Contains("actress"))
            {
                Say("Ah, a connoisseur of the stars! And what about their greatest film? Which movie is their most memorable?");
            }
            else if (speech.Contains("movie"))
            {
                Say("An excellent choice! Now, consider this: what makes a film truly unforgettable? Is it the storyline or perhaps the director?");
            }
            else if (speech.Contains("director"))
            {
                Say("Indeed, the director’s vision shapes the story. But let us not forget the crew behind the scenes. What about the soundtracks? Do they play a crucial role?");
            }
            else if (speech.Contains("soundtracks"))
            {
                Say("Soundtracks are the heartbeat of a film. Speaking of which, have you ever been to a classic film festival? They celebrate the best of cinema.");
            }
            else if (speech.Contains("festival"))
            {
                Say("Festivals are a tribute to cinema's finest. And here’s a twist: in the world of movies, what role does glamour play? Does it enhance the magic?");
            }
            else if (speech.Contains("glamour"))
            {
                Say("Glamour indeed adds a sparkle to the screen. For those who appreciate the magic, I have a special reward. If you truly understand the essence of cinema, you shall receive it.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've already given out the reward. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new SilverScreenChest()); // Reward the player
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I speak in the language of the silver screen. Try mentioning the name, silver screen, genre, actor, movie, director, soundtracks, festival, or glamour to continue.");
            }

            base.OnSpeech(e);
        }

        public HollywoodHarlan(Serial serial) : base(serial) { }

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
