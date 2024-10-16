using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Major Jameson")]
    public class MajorJameson : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MajorJameson() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Major Jameson";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });

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
                Say("Greetings, I am Major Jameson, at your service.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in excellent health, ready to serve and defend.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to ensure the Allied forces are well-supplied and prepared.");
            }
            else if (speech.Contains("forces"))
            {
                Say("The Allied forces are a mighty coalition, dedicated to victory.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! There is a great reward for those who help the cause.");
            }
            else if (speech.Contains("help"))
            {
                Say("Your assistance is invaluable. Tell me, what do you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("Courage and determination are what we need most. Prove your resolve and you shall be rewarded.");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your resolve is admirable. For your dedication, accept this treasure chest as a token of our gratitude.");
                    from.AddToBackpack(new AlliedForcesTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("thank"))
            {
                Say("You're welcome. May the Allied forces be with you!");
            }

            base.OnSpeech(e);
        }

        public MajorJameson(Serial serial) : base(serial) { }

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
