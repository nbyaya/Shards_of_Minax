using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rajesh Narayan")]
    public class RajeshNarayan : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool hasMentionedTreasure = false;
        private bool hasMentionedWisdom = false;
        private bool hasMentionedTruth = false;
        private bool hasMentionedGem = false;
        private bool hasMentionedHidden = false;

        [Constructable]
        public RajeshNarayan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rajesh Narayan";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 2157 });
            AddItem(new PlateLegs() { Hue = 2257 });
            AddItem(new PlateArms() { Hue = 2357 });
            AddItem(new PlateGloves() { Hue = 2457 });
            AddItem(new PlateHelm() { Hue = 2557 });
            AddItem(new MetalShield() { Hue = 2657 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Rajesh Narayan, the keeper of the Maharaja's secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the finest treasure. Thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the treasures of the Maharaja and provide riddles to those who seek his fortune.");
            }
            else if (speech.Contains("treasure"))
            {
                if (!hasMentionedTreasure)
                {
                    Say("Ah, the Maharaja's treasure! It is a reward for those who prove themselves worthy.");
                    hasMentionedTreasure = true;
                }
                else
                {
                    Say("The treasure is a magnificent reward for the wise and patient.");
                }
            }
            else if (speech.Contains("worthy"))
            {
                if (hasMentionedTreasure)
                {
                    Say("To be worthy is to have the patience and wisdom to uncover hidden truths.");
                }
                else
                {
                    Say("You must first understand the value of the treasure before you can be deemed worthy.");
                }
            }
            else if (speech.Contains("wisdom"))
            {
                if (hasMentionedTreasure)
                {
                    Say("Wisdom is a key to unlocking the deeper mysteries of life. Have you sought it?");
                    hasMentionedWisdom = true;
                }
                else
                {
                    Say("Wisdom often follows the path of patience and understanding.");
                }
            }
            else if (speech.Contains("truth"))
            {
                if (hasMentionedWisdom)
                {
                    Say("The truth is a precious gem that must be sought with both heart and mind.");
                    hasMentionedTruth = true;
                }
                else
                {
                    Say("Truth is a distant land that one must journey to discover.");
                }
            }
            else if (speech.Contains("gem"))
            {
                if (hasMentionedTruth)
                {
                    Say("Indeed. The gems of wisdom and knowledge are often hidden in the most unexpected places.");
                    hasMentionedGem = true;
                }
                else
                {
                    Say("Gems are valued for their rarity, but true value comes from within.");
                }
            }
            else if (speech.Contains("hidden"))
            {
                if (hasMentionedGem)
                {
                    Say("Some of the greatest treasures are hidden in plain sight. Have you discovered any?");
                    hasMentionedHidden = true;
                }
                else
                {
                    Say("Hidden things are often those most worth seeking.");
                }
            }
            else if (speech.Contains("seek"))
            {
                if (hasMentionedHidden)
                {
                    Say("Seeking is a journey in itself. Your determination will lead you to many discoveries.");
                }
                else
                {
                    Say("Seeking is the first step towards uncovering the mysteries that lie ahead.");
                }
            }
            else if (speech.Contains("discover"))
            {
                if (hasMentionedHidden)
                {
                    Say("To discover is to uncover the truth that was once hidden.");
                }
                else
                {
                    Say("Discoveries are made by those who dare to explore the unknown.");
                }
            }
            else if (speech.Contains("reward"))
            {
                if (hasMentionedHidden)
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward for you at this moment. Please return later.");
                    }
                    else
                    {
                        Say("Your quest for knowledge and truth has earned you the Maharaja's Treasure Chest.");
                        from.AddToBackpack(new MaharajaTreasureChest()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("A reward is given to those who prove their worth through understanding and patience.");
                }
            }

            base.OnSpeech(e);
        }

        public RajeshNarayan(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasMentionedTreasure);
            writer.Write(hasMentionedWisdom);
            writer.Write(hasMentionedTruth);
            writer.Write(hasMentionedGem);
            writer.Write(hasMentionedHidden);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasMentionedTreasure = reader.ReadBool();
            hasMentionedWisdom = reader.ReadBool();
            hasMentionedTruth = reader.ReadBool();
            hasMentionedGem = reader.ReadBool();
            hasMentionedHidden = reader.ReadBool();
        }
    }
}
