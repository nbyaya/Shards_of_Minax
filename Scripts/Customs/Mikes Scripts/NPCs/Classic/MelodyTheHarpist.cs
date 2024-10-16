using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Melody the Harpist")]
    public class MelodyTheHarpist : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MelodyTheHarpist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Melody the Harpist";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 80;
            Int = 80;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Name = "Melody's Shroud", Hue = 1156 }); // Example hue
            AddItem(new Spellbook() { Name = "Melody's Songs" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("I am Melody the Harpist, a bard by trade.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job as a bard is to weave tales and melodies to entertain and inspire.");
            }
            else if (speech.Contains("music"))
            {
                Say("Music can lift even the heaviest of hearts. Would you like to hear a melody?");
            }
            else if (speech.Contains("instrument"))
            {
                Say("Ah, a fellow music lover! What instrument do you play?");
            }
            else if (speech.Contains("melody"))
            {
                Say("I was named after the beautiful tunes that fill the air. My parents believed in destiny and felt that my name would guide my path.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, playing music keeps me vibrant and alive. Plus, the dances and laughter it brings warms my spirit.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Many of the stories I tell are passed down from generations, while some are inspired by the adventures I've had on the road.");
            }
            else if (speech.Contains("tune"))
            {
                Say("I recently composed a tune dedicated to the heroes of our land. It's my humble tribute to their bravery.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Please come back later for a reward.");
                }
                else
                {
                    Say("Music has a way of connecting souls. For your appreciation, I'd like to give you a small reward.");
                    from.AddToBackpack(new BanishingOrb()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("parents"))
            {
                Say("My parents were humble folk, living in the village. My father played the lute, and my mother had a voice that could soothe even the fiercest of beasts.");
            }
            else if (speech.Contains("dances"))
            {
                Say("Whenever there's a festival or celebration in town, I play for the dancers. The joy on their faces is priceless.");
            }
            else if (speech.Contains("adventures"))
            {
                Say("I've traveled to distant lands and faced many challenges. But it's always the power of music that brings me home safely.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("The heroes of our land have faced great evils and brought peace to many. Their tales inspire me to write songs of valor.");
            }

            base.OnSpeech(e);
        }

        public MelodyTheHarpist(Serial serial) : base(serial) { }

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
