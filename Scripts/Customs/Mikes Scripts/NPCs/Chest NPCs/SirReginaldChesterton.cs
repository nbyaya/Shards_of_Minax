using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald Chesterton")]
    public class SirReginaldChesterton : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginaldChesterton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Chesterton";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 60;
            Int = 85;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Hue = 2157 });
            AddItem(new PlateLegs() { Hue = 1357 });
            AddItem(new PlateArms() { Hue = 1197 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1757 });
            AddItem(new FancyShirt() { Hue = 1157 });
            
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2040); // Random hair item ID
            HairHue = Utility.RandomHairHue();
            
            // Speech Hue
            SpeechHue = 1157; // Light blue

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
                Say("Greetings, noble traveler. I am Sir Reginald Chesterton, guardian of the King's treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, as befits a knight of the realm.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to safeguard the treasures of the King's Chest and ensure they fall into worthy hands.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, the treasures! They are mighty and grand, hidden within the King's Chest.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be deemed worthy, one must prove their knowledge and perseverance. Speak to me with wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from seeking knowledge and understanding. Show me your wisdom, and you may earn a grand reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Rewards are given to those who demonstrate true worthiness and insight.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight is the ability to see beyond the surface. It requires patience and understanding.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue that many lack. It is essential in seeking out hidden truths.");
            }
            else if (speech.Contains("truths"))
            {
                Say("The truth is often concealed and requires effort to uncover. Seek diligently, and you shall find.");
            }
            else if (speech.Contains("find"))
            {
                Say("To find something of great value, one must be prepared for challenges and tests.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges test our resolve and reveal our true character. Embrace them with courage.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to face adversity and overcome fear. It is essential for any quest.");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest is an adventure undertaken to achieve a noble goal. It often involves trials and rewards.");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials are the trials of one's skill and determination. They are the path to earning true rewards.");
            }
            else if (speech.Contains("earn"))
            {
                Say("To earn a reward, one must demonstrate true worth through actions and choices.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Our actions define who we are and determine the outcomes of our endeavors.");
            }
            else if (speech.Contains("outcomes"))
            {
                Say("The outcomes of our endeavors can vary, but they are shaped by our choices and efforts.");
            }
            else if (speech.Contains("choices"))
            {
                Say("Choices guide our path and influence our destiny. Make wise choices, and you shall be rewarded.");
            }
            else if (speech.Contains("destiny"))
            {
                Say("Destiny is the culmination of our choices and actions. Embrace it with honor and integrity.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a fundamental virtue that guides our actions and decisions. It is the foundation of true worth.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the quality of being honest and having strong moral principles. It is essential for earning trust.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is earned through consistent and honorable actions. It is the basis for strong relationships.");
            }
            else if (speech.Contains("relationships"))
            {
                Say("Strong relationships are built on mutual respect and trust. They enhance our journey through life.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Our journey through life is shaped by our experiences, choices, and the people we meet.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience is the accumulation of knowledge gained through our endeavors and challenges.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is a powerful tool that can guide us in our quest for truth and understanding.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding comes from seeking knowledge and applying it with wisdom and discernment.");
            }
            else if (speech.Contains("discernment"))
            {
                Say("Discernment is the ability to judge well and make wise decisions. It is crucial for navigating complex situations.");
            }
            else if (speech.Contains("situations"))
            {
                Say("Every situation presents its own set of challenges and opportunities. Approach them with a clear mind.");
            }
            else if (speech.Contains("mind"))
            {
                Say("A clear mind is essential for making informed decisions and achieving success.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is the result of perseverance, skill, and the right choices. It is the reward for a well-fought journey.");
            }
            else if (speech.Contains("perseverance"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(15);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your journey has shown great wisdom and insight. As a reward for your perseverance, accept this King's Chest.");
                    from.AddToBackpack(new KingsBest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirReginaldChesterton(Serial serial) : base(serial) { }

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
