using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Quincy Goldsmith")]
    public class QuincyGoldsmith : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public QuincyGoldsmith() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Quincy Goldsmith";
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
            AddItem(new LeatherCap() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Pickaxe()); // Treasure hunting theme

            Hue = Race.RandomSkinHue();
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
                Say("Ah, greetings! I am Quincy Goldsmith, a treasure hunter of great renown. Do you seek treasure?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasure is what I live for. The thrill of the hunt and the joy of discovery are my true rewards. What kind of treasure are you interested in?");
            }
            else if (speech.Contains("kind"))
            {
                Say("There are many types of treasure, from gold and jewels to ancient relics and artifacts. Which piques your interest?");
            }
            else if (speech.Contains("gold"))
            {
                Say("Gold is the most sought-after treasure. It has driven many to great deeds. But true treasure often lies hidden in the form of relics. Have you heard of relics?");
            }
            else if (speech.Contains("relics"))
            {
                Say("Ah, relics! They are remnants of ages past, holding secrets and stories. If you're interested in relics, you might want to learn about their origins. Do you know where they come from?");
            }
            else if (speech.Contains("origins"))
            {
                Say("Relics often come from ancient civilizations or long-forgotten empires. Their origins can be as mysterious as the relics themselves. But finding them is part of the adventure. Are you ready for such a quest?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest for relics is not for the faint of heart. It requires courage and curiosity. If you're up for it, you should prepare well. Do you have a map or any equipment for such a journey?");
            }
            else if (speech.Contains("map"))
            {
                Say("A map is essential for any treasure hunt. It can guide you to hidden places and forgotten lands. If you have a map, make sure it's detailed. Do you need any advice on using it?");
            }
            else if (speech.Contains("advice"))
            {
                Say("When using a map, always look for landmarks and clues. They can lead you to the right path. And remember, not all paths are straightforward. Sometimes, the journey itself reveals the treasure. Are you ready to start your adventure?");
            }
            else if (speech.Contains("adventure"))
            {
                Say("An adventure is the heart of discovery. It requires bravery, patience, and a sharp mind. If you're truly prepared, then I have a special reward for you. Would you like to know more about it?");
            }
            else if (speech.Contains("discovery"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You've shown true curiosity and spirit! For your efforts, I present to you the Gold Rush Relic Chest, a treasure of my latest findings.");
                    from.AddToBackpack(new GoldRushRelicChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("equipment"))
            {
                Say("Good equipment can make all the difference. A sturdy pickaxe, a reliable map, and a keen eye for detail are crucial. If you're missing any, I can offer some suggestions. Do you need any specific gear?");
            }
            else if (speech.Contains("gear"))
            {
                Say("Proper gear includes tools for exploration, protection from dangers, and resources to help you on your journey. If you're lacking in any area, it's best to acquire the necessary items before setting out. Ready to gear up?");
            }
            else if (speech.Contains("set out"))
            {
                Say("Setting out on a quest requires preparation and resolve. Ensure you have everything you need and are mentally prepared. The journey is as important as the destination. Are you ready to begin?");
            }
            else if (speech.Contains("begin"))
            {
                Say("Wonderful! Embark on your journey with a brave heart and a sharp mind. Remember, the real treasure is often the experiences and knowledge you gain along the way. Safe travels, and may fortune favor you!");
            }

            base.OnSpeech(e);
        }

        public QuincyGoldsmith(Serial serial) : base(serial) { }

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
