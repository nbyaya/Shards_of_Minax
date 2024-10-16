using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lucius Fortunatus")]
    public class LuciusFortunatus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LuciusFortunatus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lucius Fortunatus";
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
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedGorget() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });

            // Customize hair and beard
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Random hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2045, 0x2046); // Random beard
            FacialHairHue = Utility.RandomHairHue();

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
                Say("Salve, I am Lucius Fortunatus, at your service.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, thanks to the blessings of the gods.");
            }
            else if (speech.Contains("job"))
            {
                Say("I serve as a guardian of the treasures of Rome. My duties involve safeguarding these precious artifacts.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, treasure is what I guard. But to earn it, one must prove their worth.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, demonstrate your knowledge of the Roman virtues.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The Roman virtues of courage, justice, and wisdom are what we hold dear. They are the keys to true honor.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the heart of the Roman spirit. It drives us to face danger and stand tall. Without courage, the other virtues are meaningless.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the cornerstone of our society. It ensures that all are treated fairly and with honor. Yet, justice is nothing without wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom guides our decisions and actions. It is through wisdom that we avoid folly and seek true knowledge. Seek wisdom in your quests.");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest for knowledge or honor often requires deep reflection and understanding. Reflect on the virtues, and you will find what you seek.");
            }
            else if (speech.Contains("reflect"))
            {
                Say("Reflecting on the virtues requires introspection. The more you understand them, the more you align with their true essence.");
            }
            else if (speech.Contains("understand"))
            {
                Say("Understanding the virtues is a lifelong journey. Only those who truly grasp their meaning can earn the ultimate rewards.");
            }
            else if (speech.Contains("earn"))
            {
                Say("Earning the ultimate reward involves demonstrating both knowledge and integrity. Show that you have learned the lessons well.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is doing the right thing even when no one is watching. It is the final test of your journey towards the treasure.");
            }
            else if (speech.Contains("test"))
            {
                Say("The final test is not a trial of strength but a measure of your understanding and commitment to the virtues. Are you ready?");
            }
            else if (speech.Contains("ready"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received your reward. Return later for another chance.");
                }
                else
                {
                    Say("Your demonstration of Roman virtues and understanding has been impressive. For your dedication, accept this treasure chest as a reward.");
                    from.AddToBackpack(new RomanBritanniaChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LuciusFortunatus(Serial serial) : base(serial) { }

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
