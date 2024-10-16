using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sage Wu-Lin")]
    public class SageWuLin : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool hasHeardBalance;
        private bool hasHeardDao;
        private bool hasHeardWisdom;
        private bool hasHeardMystical;
        private bool hasHeardSecrets;

        [Constructable]
        public SageWuLin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sage Wu-Lin";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Sandals() { Hue = Utility.RandomNeutralHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomNeutralHue() });
            AddItem(new GnarledStaff() { Hue = Utility.RandomNeutralHue() });

            Hue = Race.RandomSkinHue(); // Random skin hue
            HairItemID = Race.RandomHair(this); // Random hair
            HairHue = Race.RandomHairHue(); // Random hair hue

            // Speech Hue
            SpeechHue = 1150; // Light blue

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
                Say("Greetings, I am Sage Wu-Lin, keeper of ancient secrets.");
                hasHeardBalance = false;
                hasHeardDao = false;
                hasHeardWisdom = false;
                hasHeardMystical = false;
                hasHeardSecrets = false;
            }
            else if (speech.Contains("health"))
            {
                Say("My health is in balance, as is my spirit.");
            }
            else if (speech.Contains("job"))
            {
                Say("My task is to guide those who seek enlightenment through the mysteries of the Dao.");
            }
            else if (speech.Contains("dao"))
            {
                if (hasHeardBalance)
                {
                    Say("The Dao represents the natural order of the universe. Balance and harmony are key.");
                    hasHeardDao = true;
                }
                else
                {
                    Say("To understand the Dao, you must first grasp the concept of balance.");
                }
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is crucial. It is the harmony between opposing forces that brings true understanding.");
                hasHeardBalance = true;
            }
            else if (speech.Contains("wisdom"))
            {
                if (hasHeardDao)
                {
                    Say("Wisdom is gained through knowledge and experience. Seek it in all aspects of life.");
                    hasHeardWisdom = true;
                }
                else
                {
                    Say("Wisdom cannot be understood without first knowing the Dao.");
                }
            }
            else if (speech.Contains("mystical"))
            {
                if (hasHeardWisdom)
                {
                    Say("The Mystical Dao is a symbol of harmony and balance. It holds treasures of wisdom and power.");
                    hasHeardMystical = true;
                }
                else
                {
                    Say("The mystical nature of the Dao cannot be perceived without wisdom.");
                }
            }
            else if (speech.Contains("secrets"))
            {
                if (hasHeardMystical)
                {
                    Say("The secrets of the Dao are revealed to those who show patience and respect.");
                    hasHeardSecrets = true;
                }
                else
                {
                    Say("The secrets will remain hidden until you understand the Mystical Dao.");
                }
            }
            else if (speech.Contains("reveal"))
            {
                if (hasHeardSecrets)
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward for you at this moment. Return later for further guidance.");
                    }
                    else
                    {
                        Say("You have shown great patience and understanding. Accept this Mystical Dao Chest as a token of your journey.");
                        from.AddToBackpack(new MysticalDaoChest()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
                else
                {
                    Say("The reward is not yet within your grasp. Continue to seek the secrets of the Dao.");
                }
            }
            else
            {
                if (hasHeardSecrets)
                {
                    Say("Continue to explore the mysteries of the Dao. Your journey is almost complete.");
                }
                else
                {
                    Say("Seek wisdom and understanding. The path to enlightenment requires patience.");
                }
            }

            base.OnSpeech(e);
        }

        public SageWuLin(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasHeardBalance);
            writer.Write(hasHeardDao);
            writer.Write(hasHeardWisdom);
            writer.Write(hasHeardMystical);
            writer.Write(hasHeardSecrets);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasHeardBalance = reader.ReadBool();
            hasHeardDao = reader.ReadBool();
            hasHeardWisdom = reader.ReadBool();
            hasHeardMystical = reader.ReadBool();
            hasHeardSecrets = reader.ReadBool();
        }
    }
}
