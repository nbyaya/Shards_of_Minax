using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cinemar Foley")]
    public class CinemarFoley : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CinemarFoley() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cinemar Foley";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Cap() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Spellbook() { Name = "Foley's Script" });
			
			Hue = Race.RandomSkinHue(); // Skin tone
			HairItemID = Race.RandomHair(this); // Random hair
			HairHue = Race.RandomHairHue(); // Random hair color

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
                Say("Hello there! I am Cinemar Foley, the sound maestro of this adventure.");
            }
            else if (speech.Contains("health"))
            {
                Say("I’m in perfect health, ready to roll the reel at any moment.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to ensure every sound effect in your quest is perfectly synchronized with the adventure.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Ah, adventure! It’s all about capturing the essence of excitement and wonder.");
            }
            else if (speech.Contains("sound"))
            {
                Say("Sound is the heartbeat of any grand adventure. Every creak, roar, and whisper adds to the story.");
            }
            else if (speech.Contains("reel"))
            {
                Say("The reel holds many secrets of our adventures. It’s filled with moments waiting to be discovered.");
            }
            else if (speech.Contains("secret"))
            {
                Say("Secrets are best revealed to those who truly seek them. Are you ready to uncover more?");
            }
            else if (speech.Contains("seek"))
            {
                Say("Seeking is the path to understanding. Prove your dedication and I shall reward you.");
            }
            else if (speech.Contains("dedication"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Please return later; I have no reward for you at the moment.");
                }
                else
                {
                    Say("Your dedication to the adventure has not gone unnoticed. Here is the VHS Adventure Cache, filled with treasures of our journey!");
                    from.AddToBackpack(new VHSAdventureCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures are not just items but memories captured from our adventures.");
            }
            else if (speech.Contains("memories"))
            {
                Say("Memories of great adventures are captured in every sound and scene. Do you have a favorite memory?");
            }
            else if (speech.Contains("favorite"))
            {
                Say("A favorite memory is one that stands out among the rest. What about the adventures we’ve had here?");
            }
            else if (speech.Contains("adventures"))
            {
                Say("Adventures are defined by the stories and experiences they bring. Each one is unique in its own way.");
            }
            else if (speech.Contains("stories"))
            {
                Say("Stories are the heart of every adventure. They are what we remember and retell to others.");
            }
            else if (speech.Contains("heart"))
            {
                Say("The heart of an adventure is where the true essence lies. What do you think it is for you?");
            }
            else if (speech.Contains("essence"))
            {
                Say("Essence is the fundamental nature of something. For adventures, it’s the journey and the discoveries along the way.");
            }
            else if (speech.Contains("journey"))
            {
                Say("A journey is a path of exploration and growth. Every step brings new experiences and challenges.");
            }
            else if (speech.Contains("exploration"))
            {
                Say("Exploration is about discovering the unknown. It’s an integral part of any grand adventure.");
            }
            else if (speech.Contains("unknown"))
            {
                Say("The unknown holds many possibilities. It’s where we find the most unexpected treasures.");
            }
            else if (speech.Contains("possibilities"))
            {
                Say("Possibilities are endless when we venture into the unknown. What possibilities do you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("Seeking new possibilities often leads to the greatest rewards. Your journey is just beginning.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey is a story waiting to be told. Embrace each step and you’ll uncover the greatest treasures.");
            }

            base.OnSpeech(e);
        }

        public CinemarFoley(Serial serial) : base(serial) { }

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
