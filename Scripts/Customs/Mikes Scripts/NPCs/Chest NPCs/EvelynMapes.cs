using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Evelyn Mapes")]
    public class EvelynMapes : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EvelynMapes() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Evelyn Mapes";
            Body = 0x190; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new LeatherCap() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Spellbook() { Name = "Evelyn's Journal" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x204B); // Random hair
            HairHue = Utility.RandomHairHue();

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
                Say("Ah, I am Evelyn Mapes, explorer and cartographer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to chart the uncharted and find treasures lost to time.");
            }
            else if (speech.Contains("explorer"))
            {
                Say("Yes, I spend my days discovering new lands and mapping them for future generations.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! There are many hidden secrets in this world.");
            }
            else if (speech.Contains("map"))
            {
                Say("Maps are crucial for an explorer. They guide us to new adventures.");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Adventure is the heart of exploration. It drives us to discover the unknown.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the world are often hidden in plain sight. Seek and you shall find.");
            }
            else if (speech.Contains("legacy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have proven yourself to be a true seeker of adventure. For your efforts, accept this special chest as a reward.");
                    from.AddToBackpack(new SpecialWoodenChestExplorerLegacy()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to venture into the unknown with courage and curiosity. The true reward lies in the journey itself.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is essential for any explorer. It helps us face the dangers of the unknown and continue our quest.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity drives us to explore new frontiers and discover new things. It is the spark of adventure.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey is filled with both challenges and rewards. Embrace every moment of it.");
            }
            else if (speech.Contains("reward"))
            {
                Say("The greatest reward is the knowledge and experience gained along the way. But there are also tangible rewards for those who persist.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the treasure we uncover through our experiences. It lights the path ahead.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience teaches us more than any map or book ever could. It is the true guide in our adventures.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path of an explorer is rarely straight or easy, but it is always filled with wonder and discovery.");
            }
            else if (speech.Contains("wonder"))
            {
                Say("Wonder is what keeps us exploring. It is the sense of awe and excitement that drives us forward.");
            }

            base.OnSpeech(e);
        }

        public EvelynMapes(Serial serial) : base(serial) { }

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
