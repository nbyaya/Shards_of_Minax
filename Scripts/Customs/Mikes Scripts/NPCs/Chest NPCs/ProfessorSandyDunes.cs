using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Sandy Dunes")]
    public class ProfessorSandyDunes : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorSandyDunes() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Sandy Dunes";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FeatheredHat() { Hue = 1142 });
            AddItem(new StuddedChest() { Hue = 1142 });
            AddItem(new StuddedLegs() { Hue = 1142 });
            AddItem(new StuddedGloves() { Hue = 1142 });
            AddItem(new Sandals() { Hue = 1142 });
            AddItem(new WoodenShield() { Hue = 1142 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, I am Professor Sandy Dunes, an explorer of ancient desert treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("The desert is a harsh place, but I am in good shape. The quest for treasure keeps me active.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to uncover the secrets of the desert and its hidden treasures. It's a challenging but rewarding pursuit.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! You've got the spirit of an explorer. But finding it requires patience and wit.");
            }
            else if (speech.Contains("wit"))
            {
                Say("Wit and cleverness are key to unlocking the desert's secrets. If you show me some, I might have a special reward for you.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The desert hides many secrets beneath its sands. The true challenge is to decipher these mysteries.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries are like puzzles waiting to be solved. In the desert, each mystery leads to greater discoveries.");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("Discoveries are the essence of exploration. With each find, you get closer to uncovering the ultimate prize.");
            }
            else if (speech.Contains("prize"))
            {
                Say("The ultimate prize in the desert is a legendary treasure. Few have seen it, and even fewer have claimed it.");
            }
            else if (speech.Contains("legendary"))
            {
                Say("Legendary treasures are said to have magical properties. They are often hidden in the most unexpected places.");
            }
            else if (speech.Contains("magical"))
            {
                Say("Magical items often hold great power and mystery. They are highly sought after by adventurers and scholars alike.");
            }
            else if (speech.Contains("power"))
            {
                Say("The power of magical items can change the course of events. Those who seek them must be prepared for great challenges.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges are what make the pursuit of treasure thrilling. Each challenge brings you closer to the reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("A reward awaits those who prove their worth through knowledge and perseverance. For you, it could be the Sandstorm Chest.");
            }
            else if (speech.Contains("challenges") && speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've no reward to give at the moment. Please come back later.");
                }
                else
                {
                    Say("You've demonstrated great curiosity and perseverance. As a reward, take this Sandstorm Chest with you!");
                    from.AddToBackpack(new SandstormChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ProfessorSandyDunes(Serial serial) : base(serial) { }

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
