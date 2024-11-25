using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Goldsworth")]
    public class SirGoldsworth : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirGoldsworth() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Goldsworth";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldBracelet() { Hue = Utility.RandomMetalHue() });
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new Shoes() { Hue = Utility.RandomMetalHue() });
			
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2045);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2046, 0x2048);
            FacialHairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 1152; // Slightly regal

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
                Say("Greetings, traveler! I am Sir Goldsworth, the noble seeker of ancient treasures.");
            }
            else if (speech.Contains("sir"))
            {
                Say("Indeed, Sir Goldsworth is my title. But tell me, what do you know of ancient treasures?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, treasures! The very heart of my quest. Have you ever heard of the legendary King Kamehameha?");
            }
            else if (speech.Contains("kamehameha"))
            {
                Say("Yes, King Kamehameha, a figure of great legend. His treasure is said to be the greatest of them all. Do you know what makes it so special?");
            }
            else if (speech.Contains("special"))
            {
                Say("The treasure is said to be imbued with the essence of kings and queens. What do you think such a treasure might contain?");
            }
            else if (speech.Contains("contain"))
            {
                Say("Legends say it contains relics of immense power and beauty. But what of the guardians that protect it?");
            }
            else if (speech.Contains("guardians"))
            {
                Say("Ah, the guardians! They are said to be fierce and vigilant. Only those truly worthy may approach. What do you think is required to prove one's worth?");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove one's worth, you must demonstrate knowledge and valor. Can you tell me what you might offer to such a cause?");
            }
            else if (speech.Contains("offer"))
            {
                Say("An offer of courage and wisdom is always valuable. But what if I were to tell you that knowledge alone is not enough? What more might be required?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Yes, knowledge is crucial. Yet, without the right spirit, even the greatest knowledge can be futile. What does the spirit of a true seeker entail?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of a true seeker is one of unwavering determination and humility. Do you possess these qualities?");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination is indeed a key trait. However, to be truly worthy, one must also understand the essence of humility. Do you grasp this concept?");
            }
            else if (speech.Contains("humility"))
            {
                Say("Ah, humility! It is the acceptance of one's place in the grand scheme of things. It shows respect for the knowledge and the treasures one seeks. Do you seek such treasures for noble purposes?");
            }
            else if (speech.Contains("noble"))
            {
                Say("Noble purposes indeed! If you truly seek these treasures with respect and valor, then you are nearing the final challenge. Are you ready to prove your worth?");
            }
            else if (speech.Contains("ready"))
            {
                Say("Very well. To prove your readiness, answer me this: What is the essence of the greatest treasure known to man?");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of the greatest treasure is the culmination of all virtues—wisdom, courage, and humility. If you possess these qualities, then you are worthy of the final reward.");
            }
            else if (speech.Contains("wisdom"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Return later when the time is right.");
                }
                else
                {
                    Say("You have proven yourself worthy! Accept this chest of King Kamehameha’s treasure as a reward for your knowledge and virtue.");
                    from.AddToBackpack(new KingKamehamehaTreasure()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirGoldsworth(Serial serial) : base(serial) { }

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
