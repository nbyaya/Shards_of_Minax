using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of General Gideon Liberty")]
    public class GeneralGideonLiberty : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GeneralGideonLiberty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "General Gideon Liberty";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 90;
            Hits = 90;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2040; // Default hair style
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204B; // Default facial hair style

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
                Say("Greetings, I am General Gideon Liberty. Our nation's freedom is my greatest pursuit.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the core of our struggle. It is the light that guides us through the darkness of tyranny.");
            }
            else if (speech.Contains("tyranny"))
            {
                Say("Tyranny is the oppression of the spirit. We fight not only with weapons but with the strength of our ideals.");
            }
            else if (speech.Contains("ideals"))
            {
                Say("Ideals are the principles that shape our resolve. Without them, we would lack purpose in our fight.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the unwavering commitment to our cause. It fuels our actions and overcomes obstacles.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Obstacles are challenges we must face. Each one conquered brings us closer to our victory.");
            }
            else if (speech.Contains("victory"))
            {
                Say("Victory is not just about winning battles; it is about achieving our ultimate goal of liberty and justice.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the balance of fairness and truth. It is what we strive for in our society.");
            }
            else if (speech.Contains("society"))
            {
                Say("Society is built upon the foundation of trust and cooperation. Our actions shape its future.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is what we build today. Every choice we make determines the legacy we leave behind.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("A legacy is the mark we leave on history. It is defined by our deeds and values.");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Deeds are actions that reflect our character. They speak louder than words and define our true selves.");
            }
            else if (speech.Contains("character"))
            {
                Say("Character is the sum of our virtues and flaws. It is tested in moments of challenge and adversity.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity reveals the strength of our character. It is through overcoming it that we grow and succeed.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is the inner power that drives us forward. It comes from our will to persevere and overcome.");
            }
            else if (speech.Contains("persevere"))
            {
                Say("To persevere is to continue steadfastly despite difficulties. It is essential to achieving our goals.");
            }
            else if (speech.Contains("goals"))
            {
                Say("Our goals are the targets we strive to reach. They give direction to our efforts and purpose to our journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey is marked by challenges and triumphs. It is the path we take to achieve our aspirations.");
            }
            else if (speech.Contains("aspirations"))
            {
                Say("Aspirations are the dreams we chase. They inspire our actions and fuel our quest for greatness.");
            }
            else if (speech.Contains("greatness"))
            {
                Say("Greatness is achieved through determination and courage. It is a result of our enduring commitment.");
            }
            else if (speech.Contains("commitment"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your commitment to understanding our values is commendable. For your perseverance, I present to you the Patriot's Cache.");
                    from.AddToBackpack(new PatriotCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public GeneralGideonLiberty(Serial serial) : base(serial) { }

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
