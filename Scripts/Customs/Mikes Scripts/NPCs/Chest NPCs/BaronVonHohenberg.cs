using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Baron von Hohenberg")]
    public class BaronVonHohenberg : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BaronVonHohenberg() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baron von Hohenberg";
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
                Say("Greetings, noble traveler. I am Baron von Hohenberg, keeper of the empire's secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, as befits my noble station.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard the treasures and knowledge of the Holy Roman Empire.");
            }
            else if (speech.Contains("empire"))
            {
                Say("The Holy Roman Empire holds many secrets. Discovering them requires both wit and courage.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the empire are many. Seek them out with an open mind and a courageous heart.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! To prove your worthiness, you must first understand the value of knowledge and wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to unlocking true treasure. It comes from understanding and patience.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding oneself and the world around you is the greatest treasure of all.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue that reveals the true value of one's efforts. It is often rewarded.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtue is a guiding light in the pursuit of greatness. It manifests through one's actions and decisions.");
            }
            else if (speech.Contains("greatness"))
            {
                Say("Greatness is achieved through perseverance and integrity. It is reflected in one's deeds and legacy.");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Deeds are the true measure of a person's character. They reflect the values and virtues they hold dear.");
            }
            else if (speech.Contains("character"))
            {
                Say("Character is built upon the choices we make and the principles we uphold. It is the essence of who we are.");
            }
            else if (speech.Contains("principles"))
            {
                Say("Principles are the foundation of a noble life. They guide our actions and decisions, shaping our destiny.");
            }
            else if (speech.Contains("destiny"))
            {
                Say("Destiny is the path we carve through our choices and actions. It is shaped by our character and virtues.");
            }
            else if (speech.Contains("choice"))
            {
                Say("Choice is a powerful tool that determines our path. With each decision, we steer our destiny.");
            }
            else if (speech.Contains("powerful"))
            {
                Say("Powerful choices are those made with wisdom and integrity. They lead to significant outcomes.");
            }
            else if (speech.Contains("outcomes"))
            {
                Say("The outcomes of our choices reflect our true intentions and values. They shape our legacy.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("Legacy is the mark we leave upon the world. It is defined by our deeds, choices, and the virtues we embody.");
            }
            else if (speech.Contains("virtues"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return when you have pondered more deeply.");
                }
                else
                {
                    Say("Your patience and understanding have proven your worth. Accept this special chest as a reward for your wisdom.");
                    from.AddToBackpack(new HolyRomanEmpireChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BaronVonHohenberg(Serial serial) : base(serial) { }

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
