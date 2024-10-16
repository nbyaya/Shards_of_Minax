using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Cedric Bladeheart")]
    public class SirCedricBladeheart : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirCedricBladeheart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Cedric Bladeheart";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });

            Hue = Race.RandomSkinHue(); // Beard and facial hair
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
                Say("Greetings, I am Sir Cedric Bladeheart, a knight devoted to the art of swordsmanship.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, forged by the discipline of countless battles. My strength is my blade.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not just physical, but also the resolve of one's spirit. It is what drives us to excel in our pursuits.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the steadfastness that guides us through trials. It is essential for mastering any art, including swordsmanship.");
            }
            else if (speech.Contains("swordsmanship"))
            {
                Say("Swordsmanship is an art that requires dedication, skill, and a deep respect for the weapon. Only through practice can one achieve mastery.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery of the sword is not only about technique but also about understanding the philosophy behind it.");
            }
            else if (speech.Contains("philosophy"))
            {
                Say("The philosophy of the sword is about balance, precision, and the harmony between the blade and the wielder.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony in swordsmanship means achieving perfect synchronization between mind and body, reflecting in every strike and parry.");
            }
            else if (speech.Contains("strike"))
            {
                Say("A strike should be decisive and controlled, embodying both the power and the precision of the swordsman.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power in swordsmanship is not brute force but the efficient application of skill and technique.");
            }
            else if (speech.Contains("technique"))
            {
                Say("Technique is the foundation of swordsmanship, enabling the swordsman to execute complex maneuvers with grace and effectiveness.");
            }
            else if (speech.Contains("grace"))
            {
                Say("Grace in combat is the ability to move fluidly and elegantly, demonstrating mastery over one's movements and the weapon.");
            }
            else if (speech.Contains("elegance"))
            {
                Say("Elegance in swordsmanship is the seamless integration of form and function, making each movement appear effortless.");
            }
            else if (speech.Contains("effortless"))
            {
                Say("Achieving an effortless execution in combat comes from years of practice and understanding of the art's subtleties.");
            }
            else if (speech.Contains("subtleties"))
            {
                Say("The subtleties of swordsmanship are the nuances that differentiate a skilled swordsman from a mere fighter.");
            }
            else if (speech.Contains("fighter"))
            {
                Say("A fighter may rely on brute strength and aggression, but a true swordsman combines these traits with finesse and control.");
            }
            else if (speech.Contains("control"))
            {
                Say("Control is the ability to harness one's skill and strength, ensuring that every action is deliberate and purposeful.");
            }
            else if (speech.Contains("purposeful"))
            {
                Say("Every strike, parry, and movement must be purposeful to achieve the desired effect in combat.");
            }
            else if (speech.Contains("combat"))
            {
                Say("Combat is the ultimate test of a swordsman's skill and understanding, where all aspects of swordsmanship are put to the test.");
            }
            else if (speech.Contains("test"))
            {
                Say("The test of skill is not merely in victory but in the journey of improvement and the pursuit of excellence.");
            }
            else if (speech.Contains("excellence"))
            {
                Say("Excellence in swordsmanship is the culmination of practice, understanding, and the continuous pursuit of perfection.");
            }
            else if (speech.Contains("perfection"))
            {
                Say("Perfection is an ideal to strive for, but true mastery comes from understanding that the journey is as important as the destination.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey of a swordsman is filled with challenges and growth, each step leading closer to the ideal of mastery.");
            }
            else if (speech.Contains("ideal"))
            {
                Say("The ideal of a swordsman is not a static goal but a dynamic pursuit of growth and improvement.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth in swordsmanship is both physical and mental, reflecting the continuous development of skill and understanding.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your dedication to understanding the art of swordsmanship, accept this Chest of the Swordmaster as a token of appreciation.");
                    from.AddToBackpack(new SwordsmanshipBonusChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirCedricBladeheart(Serial serial) : base(serial) { }

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
