using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pierre Furmont")]
    public class PierreFurmont : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PierreFurmont() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pierre Furmont";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new StrawHat() { Hue = Utility.RandomAnimalHue() }); // A fur hat to fit the theme
            AddItem(new StuddedChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new StuddedLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new StuddedGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Bandana() { Name = "Trader's Bandana", Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue(); // Skin hue for variety
            HairItemID = Utility.RandomList(0x204B, 0x204A); // Different hair styles
            HairHue = Utility.RandomHairHue(); // Random hair hue
            FacialHairItemID = Utility.RandomList(0x204E, 0x204F); // Different beard styles

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
                Say("Greetings! I am Pierre Furmont, a trader of rare and valuable items.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, ready to trade and share my wares.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to trade rare furs and precious items from the northern lands.");
            }
            else if (speech.Contains("trader"))
            {
                Say("Indeed, trading is my life. I've encountered many wonders and treasures in my travels.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, treasures! I have many rare items. But first, you must prove your knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge of the trade is important. Tell me, what do you know about the fur trade?");
            }
            else if (speech.Contains("fur trade"))
            {
                Say("The fur trade is essential for obtaining goods from the wild. It’s a practice as old as the land itself.");
            }
            else if (speech.Contains("land"))
            {
                Say("The northern lands are vast and filled with resources. They hold many secrets yet to be discovered.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets of the trade include finding rare items and knowing where to look. Have you heard of the Lost Trade Routes?");
            }
            else if (speech.Contains("lost trade routes"))
            {
                Say("Yes, the Lost Trade Routes are legendary paths where many treasures were once hidden. They are now largely forgotten.");
            }
            else if (speech.Contains("forgotten"))
            {
                Say("Forgotten paths can sometimes be rediscovered. Some say the key to finding them is knowing the history of the land.");
            }
            else if (speech.Contains("history"))
            {
                Say("The history of these lands is rich with tales of exploration and adventure. One must study it to understand the trade.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Adventure is what drives the explorer to seek out new lands and riches. Do you enjoy adventures yourself?");
            }
            else if (speech.Contains("explorer"))
            {
                Say("If you enjoy adventure, you may find the journey as rewarding as the destination. But now, prove your knowledge about fur trading.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Fur trading involves understanding the value of different furs and their demand in various regions. Can you name a few valuable furs?");
            }
            else if (speech.Contains("valuable furs"))
            {
                Say("Valuable furs include those from rare animals such as the Arctic Fox or the Siberian Tiger. Knowing their value is crucial in the trade.");
            }
            else if (speech.Contains("Fox") || speech.Contains("Arctic"))
            {
                Say("Yes, those furs are highly prized. If you continue to show your knowledge, you may just earn a special reward.");
            }
            else if (speech.Contains("prized"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return later for your prize.");
                }
                else
                {
                    Say("You've demonstrated your extensive knowledge and curiosity. For your efforts, I present to you this special chest.");
                    from.AddToBackpack(new FurTradersChest()); // Give the Fur Trader’s Chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("return later"))
            {
                Say("Indeed, sometimes patience is required. The reward will be yours in time if you show continued interest.");
            }

            base.OnSpeech(e);
        }

        public PierreFurmont(Serial serial) : base(serial) { }

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
