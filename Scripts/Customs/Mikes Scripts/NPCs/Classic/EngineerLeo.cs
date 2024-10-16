using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Engineer Leo")]
    public class EngineerLeo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EngineerLeo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Engineer Leo";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 58;
            Int = 100;
            Hits = 68;

            // Appearance
            AddItem(new LongPants() { Hue = 1108 });
            AddItem(new BodySash() { Hue = 1132 });
            AddItem(new Boots() { Hue = 1105 });
            AddItem(new Cap() { Hue = 1902 });

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
                Say("I am Engineer Leo, the brilliant scientist!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is inconsequential. What matters is the pursuit of knowledge!");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' as you put it is to unravel the mysteries of the universe. But what would you know about that?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power. Are you powerful?");
            }
            else if (speech.Contains("power"))
            {
                Say("Power is nothing without knowledge. Do you seek knowledge?");
            }
            else if (speech.Contains("brilliant"))
            {
                Say("I've dedicated my life to the world of science and discovery. Few have the intellect to understand the depth of my work.");
            }
            else if (speech.Contains("inconsequential"))
            {
                Say("While my personal well-being is of little concern, I must ensure I'm fit enough to continue my experiments. They are of paramount importance.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The universe holds countless enigmas. Currently, I am working on a project that might revolutionize the way we understand magic and science.");
            }
            else if (speech.Contains("powerful"))
            {
                Say("True strength lies in understanding and wisdom. If you can aid me in my research, perhaps I can share some of my findings with you.");
            }
            else if (speech.Contains("seek"))
            {
                Say("Those who genuinely seek knowledge will always find a way. Prove your dedication by bringing me a rare component for my experiments, and I shall reward you.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Very well. Bring me a shard of moonstone from the Eastern Caves, and I shall provide a reward worthy of your dedication.");
            }
            else if (speech.Contains("aid"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new JesterHatOfCommand()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public EngineerLeo(Serial serial) : base(serial) { }

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
