using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Xeno")]
    public class DrXeno : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrXeno() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Xeno";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 50;
            Int = 150;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new Spellbook() { Name = "Dr. Xeno's Research Notes" });
			
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Random hair item IDs
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
                Say("Greetings, I am Dr. Xeno, an alien researcher.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I dedicate my life to studying alien artifacts and their mysteries.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Ah, the alien artifacts. They hold secrets that could change the world as we know it.");
            }
            else if (speech.Contains("research"))
            {
                Say("My research focuses on unlocking the secrets of these alien artifacts. It's a dangerous but fascinating endeavor.");
            }
            else if (speech.Contains("xeno"))
            {
                Say("Yes, that's me. A humble researcher driven by curiosity and a thirst for knowledge.");
            }
            else if (speech.Contains("alien"))
            {
                Say("The aliens were a highly advanced race. Their technology is far beyond anything we have today.");
            }
            else if (speech.Contains("technology"))
            {
                Say("Alien technology is both wondrous and perilous. It requires great care to study.");
            }
            else if (speech.Contains("technology"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Please come back later.");
                }
                else
                {
                    Say("For your curiosity and willingness to learn, I present you with this alien artifact chest.");
                    from.AddToBackpack(new AlienArtifactChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("danger"))
            {
                Say("The study of alien artifacts is not without its dangers. One must always be cautious.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is the key to discovery. It drives us to explore and understand the unknown.");
            }

            base.OnSpeech(e);
        }

        public DrXeno(Serial serial) : base(serial) { }

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
