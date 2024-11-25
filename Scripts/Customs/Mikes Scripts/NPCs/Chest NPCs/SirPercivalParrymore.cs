using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Percival Parrymore")]
    public class SirPercivalParrymore : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirPercivalParrymore() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Percival Parrymore";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            // Randomize hair
            HairItemID = Utility.RandomList(0x203B, 0x2040, 0x203C);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x2041);

            // Speech Hue
            SpeechHue = 1153; // A distinctive hue for speech

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
                Say("Greetings, noble traveler! I am Sir Percival Parrymore, defender of the realm. My task is to teach the art of parrying.");
            }
            else if (speech.Contains("parrying"))
            {
                Say("Ah, parrying! A vital skill in combat. To master it, one must understand the importance of defense. Do you seek mastery?");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery is achieved through dedication and practice. To gain true mastery, one must excel in both technique and resilience.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is the cornerstone of skill. It requires perseverance and hard work. Have you shown this dedication in your endeavors?");
            }
            else if (speech.Contains("skill"))
            {
                Say("Skill is not just about the ability to parry but to anticipate and counter your opponent’s moves. Show me your resolve in mastering these skills.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the strength to continue despite challenges. If your resolve is strong, you are closer to achieving your goal. Do you feel prepared?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Being prepared means understanding both your strengths and weaknesses. In combat, this knowledge will guide your actions. Are you ready to prove your worth?");
            }
            else if (speech.Contains("worth"))
            {
                Say("Your worth is determined by your actions and commitment. Show me your worth, and I will reward you for your efforts.");
            }
            else if (speech.Contains("determined"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Please return later.");
                }
                else
                {
                    Say("For your dedication, skill, and resolve, I present you with this special chest, a token of your parry mastery.");
                    from.AddToBackpack(new ParryBonusChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("combat"))
            {
                Say("In combat, your technique is as important as your mindset. True mastery involves both skill and strategy.");
            }
            else if (speech.Contains("technique"))
            {
                Say("Technique involves the precise application of skill. It is through technique that one can turn the tide of battle. Have you practiced diligently?");
            }
            else if (speech.Contains("practice"))
            {
                Say("Practice is essential for improvement. It is through continuous effort that one refines their abilities. How have you been practicing your skills?");
            }
            else if (speech.Contains("effort"))
            {
                Say("Effort is the driving force behind progress. Without effort, mastery remains a distant dream. Have you put in the necessary effort?");
            }
            else if (speech.Contains("dream"))
            {
                Say("A dream of mastery is the first step towards achieving it. Transforming that dream into reality requires hard work and dedication.");
            }
            else if (speech.Contains("transformation"))
            {
                Say("Transformation is the result of persistent effort. To transform your skills into mastery, you must be resolute and consistent.");
            }

            base.OnSpeech(e);
        }

        public SirPercivalParrymore(Serial serial) : base(serial) { }

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
