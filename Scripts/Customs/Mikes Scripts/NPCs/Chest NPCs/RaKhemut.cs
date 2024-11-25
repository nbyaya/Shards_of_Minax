using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ra-Khemut")]
    public class RaKhemut : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RaKhemut() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ra-Khemut";
            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();
            Title = "the Desert Guardian";

            // Appearance
            AddItem(new Robe(Utility.RandomMetalHue())); // Golden robes
            AddItem(new Sandals(Utility.RandomMetalHue())); // Golden sandals
            AddItem(new QuarterStaff(Utility.RandomMetalHue())); // Headpiece

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

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
                Say("I am Ra-Khemut, the guardian of the Desert Pharaoh's secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as eternal as the sands of time.");
            }
            else if (speech.Contains("job"))
            {
                Say("My task is to protect the treasures and secrets of the Pharaoh's resting place.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The Pharaoh's secrets are guarded by puzzles and trials. Only the worthy may gain access.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("The treasures of the Pharaoh are many, but only the most deserving shall claim them.");
            }
            else if (speech.Contains("pharaoh"))
            {
                Say("The Desert Pharaoh was a ruler of great might and wisdom. His resting place holds many wonders.");
            }
            else if (speech.Contains("resting"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must prove yourself worthy before receiving the Pharaoh's reward.");
                }
                else
                {
                    Say("For your bravery and cleverness, accept this Desert Pharaoh's Chest as your reward.");
                    from.AddToBackpack(new DesertPharaohChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RaKhemut(Serial serial) : base(serial) { }

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
