using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Fionn O'Conor")]
    public class FionnOConor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FionnOConor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Fionn O'Conor";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 90;
            Hits = 100;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new GnarledStaff() { Hue = Utility.RandomMetalHue(), Name = "Fionn's Staff of Wisdom" });

            // Randomize appearance
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, I am Fionn O'Conor, keeper of ancient treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as a Celtic oak, strong and enduring.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the secrets of the ancient treasures and impart wisdom to the worthy.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the treasure of the soul. Seek it, and you will find the path to greatness.");
            }
            else if (speech.Contains("path"))
            {
                Say("The path to greatness is paved with knowledge and courage. One must seek the truth with an open heart.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is the light that guides us through the darkness. It is revealed through perseverance and sincerity.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is key to overcoming challenges. It transforms obstacles into stepping stones on the road to success.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Obstacles are but tests of one's resolve. Face them with courage, and they will yield to your strength.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the flame that burns within, driving us to face the unknown. It is the foundation of every great deed.");
            }
            else if (speech.Contains("deed"))
            {
                Say("Great deeds are the result of unwavering determination and the will to act despite fear. What deed do you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to embark on a journey of discovery. Every quest begins with a single step into the unknown.");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest is a journey undertaken to achieve a goal. Your quest may lead you to many discoveries and challenges.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery is the reward for those who dare to explore beyond the horizon. What have you uncovered on your journey?");
            }
            else if (speech.Contains("horizon"))
            {
                Say("The horizon represents the limit of one's vision. To see beyond it requires courage and determination.");
            }
            else if (speech.Contains("courage"))
            {
                Say("You have already demonstrated courage in seeking the truth. Your resolve is commendable.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the strength to continue despite adversity. Your resolve has led you to this point.");
            }
            else if (speech.Contains("point"))
            {
                Say("At this point, you have proven your worth. The treasure you seek is within your grasp.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! It lies within the chest I guard. Prove your worthiness to claim it.");
            }
            else if (speech.Contains("worthiness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Return after some time.");
                }
                else
                {
                    Say("Your worthiness has been proven. Take this chest as a token of your courage and wisdom.");
                    from.AddToBackpack(new SpecialWoodenChestOisin()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public FionnOConor(Serial serial) : base(serial) { }

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
