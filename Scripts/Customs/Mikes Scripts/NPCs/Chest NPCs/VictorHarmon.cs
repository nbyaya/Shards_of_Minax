using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Victor Harmon")]
    public class VictorHarmon : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VictorHarmon() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Victor Harmon";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1157 });
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Shoes() { Hue = 1157 });
            AddItem(new FeatheredHat() { Hue = Utility.RandomBrightHue() });
            AddItem(new Lute() { Name = "Victor's Lute" });

            // Hair and facial hair
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x2040); // Different hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0; // No facial hair for this NPC

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
                Say("Greetings, traveler. I am Victor Harmon, the keeper of melodies.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as vibrant as a symphony, my health is splendid.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to preserve the timeless music and vintage treasures of yesteryears.");
            }
            else if (speech.Contains("melodies"))
            {
                Say("Melodies are the language of the soul. They tell stories beyond words. Have you heard of the 'jewels'?");
            }
            else if (speech.Contains("jewels"))
            {
                Say("The Jukebox Jewels hold the essence of nostalgia and music. They are not easily given away. Do you know the value of 'stories'?");
            }
            else if (speech.Contains("stories"))
            {
                Say("Stories are woven into every tune and trinket. They wait to be uncovered by those who seek. But to uncover them, you must appreciate 'music'.");
            }
            else if (speech.Contains("music"))
            {
                Say("Music is a treasure of its own, don't you think? It has the power to move hearts and inspire minds. Speaking of inspiration, what do you think of 'treasures'?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, treasures! They are the memories of the past, each one with its own tale. If you wish to earn one, you must show your understanding of 'appreciation'.");
            }
            else if (speech.Contains("appreciation"))
            {
                Say("Appreciation is a profound acknowledgment of value and significance. Do you understand the true essence of 'rewards'?");
            }
            else if (speech.Contains("rewards"))
            {
                Say("Rewards are not just tokens but symbols of recognition. To receive a true reward, one must have shown genuine 'dedication'.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication involves commitment and perseverance. If you've demonstrated dedication, you might be worthy of a special reward. But first, tell me about 'nostalgia'.");
            }
            else if (speech.Contains("nostalgia"))
            {
                Say("Nostalgia is the longing for the past, often wrapped in 'melancholy'. If you understand this, you are close to the final step. Speak to me of 'melancholy'.");
            }
            else if (speech.Contains("melancholy"))
            {
                Say("Melancholy is a deep, reflective sadness that often accompanies nostalgia. You have journeyed far in understanding the essence of the past. For your thoughtful inquiry, accept this Jukebox Jewels chest.");
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new JukeboxJewels()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("I have no treasure to give right now. Please return later.");
                }
            }
            else
            {
                Say("I'm afraid I don't understand that. Perhaps try speaking about something else related to music or treasures?");
            }

            base.OnSpeech(e);
        }

        public VictorHarmon(Serial serial) : base(serial) { }

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
