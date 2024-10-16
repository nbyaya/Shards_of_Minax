using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Plumberston")]
    public class ProfessorPlumberston : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorPlumberston() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Plumberston";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Professor's Guide" });

            Hue = Utility.RandomSkinHue(); // Skin tone
            HairItemID = 0x203B; // Bald head
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
                Say("Greetings! I am Professor Plumberston, keeper of mystical treasures. Ask me about my job, and I may reveal more.");
            }
            else if (speech.Contains("job"))
            {
                Say("I dedicate my days to uncovering hidden relics and treasures of great power. To find treasures, one must understand the essence of relics.");
            }
            else if (speech.Contains("relics"))
            {
                Say("Ah, relics! They hold the secrets of ancient times. Only by proving curiosity can one uncover their mysteries.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is the key to understanding. If you wish to know more, inquire about treasures and prove your worthiness.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures are like hidden secrets, often requiring patience to uncover. To earn a reward, you must show dedication.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication will reveal great rewards. Ask me about patience to proceed further in your quest.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue in the pursuit of knowledge. Your patience will be rewarded if you inquire about worthiness.");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("To prove your worthiness, demonstrate your understanding of the treasures. Ask about the key to unlocking them.");
            }
            else if (speech.Contains("key"))
            {
                Say("The key to unlocking treasures is curiosity combined with dedication. If you have shown these traits, you are close to the reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("You are almost there! To claim your reward, you must answer questions about relics and treasures.");
            }
            else if (speech.Contains("questions"))
            {
                Say("Answer these questions: What is the essence of relics? What makes treasures valuable? Show me your understanding.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Your understanding is commendable. For your curiosity and patience, accept this special treasure box.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait before claiming another reward.");
                }
                else
                {
                    from.AddToBackpack(new MarioTreasureBox()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I only respond to questions about my name, job, relics, treasures, dedication, patience, worthiness, key, reward, and understanding.");
            }

            base.OnSpeech(e);
        }

        public ProfessorPlumberston(Serial serial) : base(serial) { }

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
