using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Qin Stone")]
    public class DrQinStone : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrQinStone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Qin Stone";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Dr. Qin Stone's Tome" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Random male hair
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
                Say("Greetings, I am Dr. Qin Stone, scholar of the ancient dynasties.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the Great Wall. Thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am dedicated to preserving the treasures and knowledge of the Qin Dynasty.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! The chest I guard holds secrets and relics of the Qin Dynasty. Seek further knowledge to understand its value.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the Qin Dynasty are vast. They include ancient wisdom, powerful artifacts, and the legacy of a unified empire.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts from the Qin Dynasty, like the one in the chest, hold immense historical and magical significance.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through the darkness of ignorance. It is the key to understanding our past and shaping our future.");
            }
            else if (speech.Contains("magical"))
            {
                Say("Magical artifacts are rare and hold great power. They are often protected by ancient spells and hidden in sacred places.");
            }
            else if (speech.Contains("spells"))
            {
                Say("Ancient spells are intricate and powerful. They were used to protect treasures and maintain the balance of nature.");
            }
            else if (speech.Contains("nature"))
            {
                Say("Nature was revered by the ancient Qin. They believed that harmony with nature was essential for prosperity and peace.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony with nature and the universe is a core principle of ancient wisdom. It teaches us balance and respect for all life.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Maintaining balance in life and nature is essential for stability and growth. It is a principle that guides many ancient teachings.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth, both personal and communal, is a reflection of the balance we maintain. It leads to prosperity and understanding.");
            }
            else if (speech.Contains("prosperity"))
            {
                Say("Prosperity is the result of wisdom, balance, and growth. It is the ultimate goal of the ancient teachings and the legacy of the Qin Dynasty.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The legacy of the Qin Dynasty is rich with achievements, including the construction of the Great Wall and the creation of a unified empire.");
            }
            else if (speech.Contains("great wall"))
            {
                Say("The Great Wall was built to protect the empire from invasions. It is a symbol of strength and perseverance.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the ability to keep moving forward despite challenges. It is a key virtue for achieving great things.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues of wisdom, courage, and perseverance are essential for understanding the Qin Dynasty's legacy.");
            }
            else if (speech.Contains("prove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received a reward. Please return later.");
                }
                else
                {
                    Say("Your understanding of the virtues and the legacy of the Qin Dynasty is impressive. As a reward, accept this chest of ancient treasures.");
                    from.AddToBackpack(new TreasureChestOfTheQinDynasty()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DrQinStone(Serial serial) : base(serial) { }

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
