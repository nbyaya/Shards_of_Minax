using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marquis du Prisme")]
    public class MarquisDuPrisme : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarquisDuPrisme() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marquis du Prisme";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1157 });
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Boots() { Hue = 1157 });
            AddItem(new TricorneHat() { Hue = 1157 });
            AddItem(new GoldNecklace() { Hue = 1157 });

            Hue = Race.RandomSkinHue(); // Random skin hue
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, I am Marquis du Prisme, a humble guardian of secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the fortress of my estate.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to safeguard the treasures of the Revolution and ensure their secrets remain hidden.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, secrets! They are the lifeblood of our power. To unlock them, one must prove their worth.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The Revolution was a time of great upheaval and transformation. Such times are always marked by hidden treasures and untold riches.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures of the Revolution are not merely material. They are symbols of our triumph and resolve.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Your worth is demonstrated through your inquisitiveness. But to truly understand, you must explore deeper.");
            }
            else if (speech.Contains("deeper"))
            {
                Say("To go deeper, one must grasp the essence of our cause. The Revolution's heart is in its values and its people.");
            }
            else if (speech.Contains("values"))
            {
                Say("The values of the Revolution are equality, liberty, and fraternity. They guide every decision and every action.");
            }
            else if (speech.Contains("equality"))
            {
                Say("Equality was the cornerstone of the Revolution. It means that every person is of equal worth and should be treated as such.");
            }
            else if (speech.Contains("liberty"))
            {
                Say("Liberty was the cry of the Revolution. It represents freedom and the right to self-determination.");
            }
            else if (speech.Contains("fraternity"))
            {
                Say("Fraternity is about brotherhood and solidarity. It's the unity that binds people together in the struggle for a common cause.");
            }
            else if (speech.Contains("solidarity"))
            {
                Say("Solidarity means standing together against oppression. It's a powerful force that strengthens our collective resolve.");
            }
            else if (speech.Contains("oppression"))
            {
                Say("Oppression is what the Revolution fought against. It represents the unjust treatment of people and the denial of their rights.");
            }
            else if (speech.Contains("rights"))
            {
                Say("Rights are the fundamental freedoms and entitlements that every individual should have. They are central to our fight for justice.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the fair treatment of all people. It is the ultimate goal of our struggle, ensuring that everyone is treated with dignity and respect.");
            }
            else if (speech.Contains("dignity"))
            {
                Say("Dignity is about respecting oneself and others. It is a key element of a just society and a core value of the Revolution.");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect is the recognition of each person's worth and individuality. It is crucial for harmony and cooperation.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the peaceful coexistence of individuals with different views and backgrounds. It is essential for a functioning society.");
            }
            else if (speech.Contains("society"))
            {
                Say("Society is the collective group of people living together. It is shaped by the values and principles we uphold.");
            }
            else if (speech.Contains("principles"))
            {
                Say("Principles are the fundamental truths or propositions that guide our actions and beliefs. They are the foundation of our values.");
            }
            else if (speech.Contains("foundation"))
            {
                Say("The foundation of our Revolution is built on unwavering principles. To honor it, one must fully understand and embrace its essence.");
            }
            else if (speech.Contains("honor"))
            {
                Say("To honor something is to hold it in high regard and uphold its values. It is a mark of true commitment and respect.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment is the dedication to a cause or principle. It reflects one's resolve to see it through despite challenges.");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your resolve and understanding have been proven. For your dedication, I bestow upon you the legendary \"Coffre de la Révolution.\"");
                    from.AddToBackpack(new RevolutionChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MarquisDuPrisme(Serial serial) : base(serial) { }

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
