using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Timewell")]
    public class ProfessorTimewell : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorTimewell() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Timewell";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Professor Timewell's Chronicles" });

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
                Say("Greetings! I am Professor Timewell, chronicler of the Millennium Era. Do you have an interest in history?");
            }
            else if (speech.Contains("history"))
            {
                Say("Ah, history! A fascinating subject. My studies focus on the turn of the millennium. Have you heard of the Millennium Time Capsule?");
            }
            else if (speech.Contains("time capsule"))
            {
                Say("Yes, the Millennium Time Capsule! It contains various artifacts from the year 2000. Would you like to learn more about these artifacts?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts from that time are truly unique. Each item tells a story of the past. Are you curious about any specific type of artifact?");
            }
            else if (speech.Contains("specific"))
            {
                Say("Hmm, a specific artifact? There were many, such as rare collectibles, limited-edition items, and more. Interested in hearing about some examples?");
            }
            else if (speech.Contains("examples"))
            {
                Say("Certainly! For instance, there was the 'Y2K Amulet' and the 'iPod of Tunes.' Fascinating items, aren’t they? Do you wish to explore more about these?");
            }
            else if (speech.Contains("y2k amulet"))
            {
                Say("The 'Y2K Amulet' was a symbol of the new millennium, believed to bring good luck. It’s one of the many treasures in the Millennium Time Capsule. Want to know about another item?");
            }
            else if (speech.Contains("ipod of tunes"))
            {
                Say("The 'iPod of Tunes' represents the height of portable music technology from the year 2000. A cherished item indeed! Would you like to learn about how to obtain these treasures?");
            }
            else if (speech.Contains("obtain"))
            {
                Say("To obtain these treasures, you must demonstrate your knowledge of the era. Do you think you have the knowledge required to uncover these artifacts?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Excellent! Knowledge is key. Let’s see if you know about the turn of the millennium. Do you know any significant events from that time?");
            }
            else if (speech.Contains("events"))
            {
                Say("Ah, the events! The millennium saw numerous changes, such as the rise of the internet and major technological advancements. What else do you know about the era?");
            }
            else if (speech.Contains("technological advancements"))
            {
                Say("Technological advancements included the launch of new gadgets, software updates, and the excitement over the new millennium. Are you familiar with any specific technological milestones?");
            }
            else if (speech.Contains("milestones"))
            {
                Say("One notable milestone was the widespread adoption of the internet. Many believed it would change the world. Interested in discovering more about the treasures related to this period?");
            }
            else if (speech.Contains("discover"))
            {
                Say("To discover these treasures, you need to prove your dedication to learning about the millennium. Do you wish to embark on this quest for knowledge?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest for knowledge indeed! If you’ve learned enough and can demonstrate your understanding, a reward awaits. Are you ready to claim your prize?");
            }
            else if (speech.Contains("prize"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Congratulations on your quest! Your dedication has earned you the Millennium Time Capsule. May it bring you joy and a glimpse into the past.");
                    from.AddToBackpack(new MillenniumTimeCapsule()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ProfessorTimewell(Serial serial) : base(serial) { }

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
