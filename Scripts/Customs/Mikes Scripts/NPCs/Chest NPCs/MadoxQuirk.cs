using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Madox Quirk")]
    public class MadoxQuirk : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool spokeAboutChest;
        private bool spokeAboutStory;
        private bool spokeAboutReward;

        [Constructable]
        public MadoxQuirk() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Madox Quirk";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LongPants() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });
            AddItem(new TricorneHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new BodySash() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Madox's Compendium" });

            // Hair
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Facial hair
            FacialHairItemID = Race.RandomFacialHair(this);

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
            spokeAboutChest = false;
            spokeAboutStory = false;
            spokeAboutReward = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Madox Quirk, the purveyor of peculiar treasures!");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, I'm in fine health! Just a tad quirky, as always.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I curate and collect the most curious and modish items you can find!");
            }
            else if (speech.Contains("chest") && !spokeAboutChest)
            {
                Say("Ah, the Mod Madness Trunk! A chest filled with delightful and dazzling items, worthy of any eccentric collector.");
                spokeAboutChest = true;
            }
            else if (speech.Contains("story") && spokeAboutChest && !spokeAboutStory)
            {
                Say("Ah, the stories behind the items in the chest are as colorful as the items themselves. Each piece holds a tale of its own.");
                spokeAboutStory = true;
            }
            else if (speech.Contains("reward") && spokeAboutStory && !spokeAboutReward)
            {
                Say("You've been quite inquisitive! To truly understand the reward, one must first appreciate the stories and the chest itself.");
                spokeAboutReward = true;
            }
            else if (speech.Contains("chest") && spokeAboutStory)
            {
                Say("Indeed, the chest is a treasure trove of wonders. But remember, it's not just about what you find inside, but the journey you take to get there.");
            }
            else if (speech.Contains("story") && spokeAboutChest)
            {
                Say("Each item in the chest has a story that reflects its essence. Discovering these stories adds to the magic of the chest.");
            }
            else if (speech.Contains("reward") && spokeAboutChest)
            {
                Say("To earn the reward, one must unravel the tales and understand the significance of each item. You've done well so far.");
            }
            else if (speech.Contains("reward") && spokeAboutStory)
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You've proven yourself to be a true seeker of wonders! Here is a Mod Madness Trunk, packed with marvelous and modish treasures. Enjoy!");
                    from.AddToBackpack(new ModMadnessTrunk()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tales") && spokeAboutStory)
            {
                Say("The tales of the items are as varied as their forms. Some are whimsical, others profound. Each adds to the lore of the Mod Madness Trunk.");
            }
            else if (speech.Contains("journey") && spokeAboutChest)
            {
                Say("Indeed, the journey is as important as the destination. Every step you take reveals more about the mysteries contained within the chest.");
            }
            else if (speech.Contains("essence") && spokeAboutStory)
            {
                Say("The essence of each item is reflected in its story. Understanding this essence will unlock the full magic of the Mod Madness Trunk.");
            }

            base.OnSpeech(e);
        }

        public MadoxQuirk(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(spokeAboutChest);
            writer.Write(spokeAboutStory);
            writer.Write(spokeAboutReward);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            spokeAboutChest = reader.ReadBool();
            spokeAboutStory = reader.ReadBool();
            spokeAboutReward = reader.ReadBool();
        }
    }
}
