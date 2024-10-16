using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gadgeteer Gentry")]
    public class GadgeteerGentry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GadgeteerGentry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gadgeteer Gentry";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomMetalHue() }); // Tech-inspired robe
            AddItem(new FeatheredHat() { Hue = Utility.RandomMetalHue() }); // A quirky hat for added character
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Gadgeteer's Compendium" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203C, 0x2041, 0x204B); // Varied hair styles
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

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Gadgeteer Gentry, the master of technological wonders! Are you interested in gadgets?");
            }
            else if (speech.Contains("gadgets"))
            {
                Say("Ah, gadgets! They are marvelous creations. Have you ever seen a remarkable artifact?");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Artifacts are treasures of the past, full of hidden knowledge. Do you know the value of an artifact?");
            }
            else if (speech.Contains("value"))
            {
                Say("The value of an artifact lies in its history and functionality. Are you intrigued by challenges?");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges keep the mind sharp! Can you solve a puzzle to unlock more secrets?");
            }
            else if (speech.Contains("puzzle"))
            {
                Say("Puzzles are the essence of discovery. What about solving a puzzle for a unique reward?");
            }
            else if (speech.Contains("reward"))
            {
                Say("A reward for solving a puzzle is a great incentive. Do you have a knack for uncovering mysteries?");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries drive curiosity. What if I told you there’s a special cache awaiting the curious?");
            }
            else if (speech.Contains("cache"))
            {
                Say("Indeed, a cache of wonders! To earn it, you must prove your puzzle-solving prowess. Ready for the challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Congratulations on solving the puzzle! Here is the Technophile's Cache, a true treasure for those who seek knowledge.");
                    from.AddToBackpack(new TechnophilesCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power, and in the right hands, it can change the world. Seek out more if you're curious.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power can be harnessed for great deeds. Are you ready to apply what you've learned?");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Deeds speak louder than words. Show me your skills, and you might earn more than just a reward.");
            }
            else if (speech.Contains("skills"))
            {
                Say("Skills are honed through practice and challenge. Let’s see if you’re up to the task.");
            }

            base.OnSpeech(e);
        }

        public GadgeteerGentry(Serial serial) : base(serial) { }

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
