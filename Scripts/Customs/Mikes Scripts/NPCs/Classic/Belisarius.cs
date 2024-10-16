using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Belisarius")]
    public class Belisarius : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Belisarius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Belisarius";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 100;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1153 });
            AddItem(new PlateChest() { Hue = 1153 });
            AddItem(new PlateGloves() { Hue = 1153 });
            AddItem(new PlateHelm() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new Halberd() { Name = "Belisarius's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Belisarius of Byzantium, a wanderer of time and space.");
            }
            else if (speech.Contains("health"))
            {
                Say("My existence is but a fleeting moment in the river of time.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a seeker of ancient mysteries and keeper of forgotten knowledge.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Do you dare to grasp the threads of fate and unravel the tapestry of destiny?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("If you seek knowledge, you must answer this riddle: What is the beginning of eternity, the end of time and space, the beginning of every end, and the end of every race?");
            }
            else if (speech.Contains("byzantium"))
            {
                Say("Ah, Byzantium, the shining jewel of the east. Its wonders and marvels have been lost to the ravages of time, but its spirit remains in me.");
            }
            else if (speech.Contains("moment"))
            {
                Say("Each moment is precious, a glimpse of the infinite possibilities. But even in this short time, I have witnessed empires rise and fall.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("Ancient mysteries are the echoes of the past, holding secrets that can change the course of fate. To understand them, one must look beyond the visible horizon.");
            }
            else if (speech.Contains("fate"))
            {
                Say("Fate is the weaver of destinies, a force that binds us all. But with knowledge, one can influence the threads of fate.");
            }
            else if (speech.Contains("answer"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the answer is 'E'. You have a sharp mind. For your wisdom, I grant you this gift.");
                    from.AddToBackpack(new VelocityDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("wonders"))
            {
                Say("The wonders of Byzantium were not just its grand architecture or vast libraries, but the people and their indomitable spirit.");
            }
            else if (speech.Contains("empires"))
            {
                Say("Empires like Rome, Persia, and even the great Byzantine. Their stories, their legacies are etched in the annals of time.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like buried treasures, waiting for the right seeker to unearth and understand their true meaning.");
            }

            base.OnSpeech(e);
        }

        public Belisarius(Serial serial) : base(serial) { }

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
