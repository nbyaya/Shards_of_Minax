using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald Cavalier")]
    public class SirReginaldCavalier : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginaldCavalier() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Cavalier";
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
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });
			
            Hue = Utility.RandomSkinHue(); // Beard and facial hair
            HairItemID = Utility.RandomList(0x2046, 0x2047, 0x2049); // Random hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x204B, 0x204C, 0x204D); // Random facial hair

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
                Say("Hail, brave traveler! I am Sir Reginald Cavalier, a knight of valor.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in splendid health, prepared for any challenge that lies ahead.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to protect the realm and ensure the safety of its citizens.");
            }
            else if (speech.Contains("duty"))
            {
                Say("Duty is the commitment to one’s responsibilities, no matter the personal cost.");
            }
            else if (speech.Contains("responsibilities"))
            {
                Say("Responsibilities often include safeguarding the weak and upholding justice.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is what ensures peace and order in our world, and it must be upheld at all costs.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace is a hard-won treasure. It is the ultimate goal of every knight's quest.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasures! There is one that I hold dear, but you must prove your worth to earn it.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, demonstrate your knowledge of valor and duty.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the courage in the face of danger, a trait that every true knight must possess.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to face fear and adversity. It is what drives a knight forward.");
            }
            else if (speech.Contains("fear"))
            {
                Say("Fear is a natural response, but a knight learns to face and overcome it with bravery.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is what makes a hero out of a mere mortal. It is an essential quality for anyone seeking greatness.");
            }
            else if (speech.Contains("greatness"))
            {
                Say("Greatness is achieved through deeds of honor and sacrifice. Only those who strive for it truly understand its value.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Sacrifice is giving up something valued for the sake of something more important. It is a core principle of knighthood.");
            }
            else if (speech.Contains("worth"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Return after some time.");
                }
                else
                {
                    Say("You have shown great understanding. For your insight into valor and duty, accept this chest as your reward.");
                    from.AddToBackpack(new HussarsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirReginaldCavalier(Serial serial) : base(serial) { }

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
