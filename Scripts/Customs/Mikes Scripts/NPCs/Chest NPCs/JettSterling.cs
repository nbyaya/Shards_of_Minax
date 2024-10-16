using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jett Sterling")]
    public class JettSterling : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JettSterling() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jett Sterling";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new LeatherChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new Bandana() { Hue = Utility.RandomBrightHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new HeavyCrossbow() { Name = "Jett's Crossbow" });

            Hue = Race.RandomSkinHue(); // Facial features
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Hey there! I'm Jett Sterling, once a stuntman, now a keeper of treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("I've got a few scratches here and there, but still going strong!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To keep this chest safe and sound. It's packed with relics from my wild days.");
            }
            else if (speech.Contains("relics"))
            {
                Say("I've got some pretty unique stuff in here. A bit of everything from my glory days.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, the chest! It's a real gem. But only those who prove their worth get to take a peek inside.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, show me your curiosity. Ask me about my past adventures.");
            }
            else if (speech.Contains("adventures"))
            {
                Say("My adventures were wild, full of thrills and spills. Ask me more if you want to hear the tales.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Ah, you want the full story, huh? Well, it all started with a daring escape.");
            }
            else if (speech.Contains("escape"))
            {
                Say("Yes, escaping from a tight spot was always part of the thrill. Speaking of which, the thrill is what makes the stories memorable.");
            }
            else if (speech.Contains("thrill"))
            {
                Say("The thrill of the chase, the excitement of the unknown. Ask me about a memorable experience.");
            }
            else if (speech.Contains("experience"))
            {
                Say("One experience that stands out was a high-speed chase through the city streets. That was a wild ride.");
            }
            else if (speech.Contains("ride"))
            {
                Say("A wild ride indeed! I had to use every trick in the book to keep ahead. It’s these stories that make the chest’s treasures so special.");
            }
            else if (speech.Contains("special"))
            {
                Say("What makes these treasures special? They're relics of a time when life was lived in the fast lane. Ask me about a particular treasure.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Each treasure has its own story. For instance, the 'Greaser's Comb' is a token from a memorable heist.");
            }
            else if (speech.Contains("heist"))
            {
                Say("The heist was one of the biggest challenges. We had to outwit our rivals and get away without a scratch.");
            }
            else if (speech.Contains("rivals"))
            {
                Say("Dealing with rivals was always part of the game. It taught me a lot about strategy and resourcefulness.");
            }
            else if (speech.Contains("strategy"))
            {
                Say("Good strategy is key to success. And speaking of success, you've done well to follow the clues.");
            }
            else if (speech.Contains("success"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've already given you something. Come back later for more!");
                }
                else
                {
                    Say("You've demonstrated a keen mind and persistence. For your efforts, take this Greaser's Goldmine chest as your reward!");
                    from.AddToBackpack(new GreasersGoldmineChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public JettSterling(Serial serial) : base(serial) { }

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
