using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Anarchist")]
    public class Anarchist : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Anarchist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "The Anarchist";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Bandana() { Hue = 1153 });
            AddItem(new LeatherChest() { Hue = 1153 });
            AddItem(new LeatherLegs() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new Dagger() { Hue = 1153 });
            AddItem(new SimpleNote() { Name = "Anarchist's Notes" });

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
                Say("I am known as the Anarchist, a rebel against the old order.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as vibrant as my spirit. The fight for freedom keeps me strong.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to sow discord among the tyrants and gather allies for our cause.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the ultimate goal. It is worth every sacrifice.");
            }
            else if (speech.Contains("rebel"))
            {
                Say("To rebel is to challenge the status quo. It requires courage and determination.");
            }
            else if (speech.Contains("discord"))
            {
                Say("Discord is the tool with which we shake the foundations of oppression.");
            }
            else if (speech.Contains("cause"))
            {
                Say("Our cause is just. We fight for a world where all are equal.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Every sacrifice made is a step closer to the dawn of a new era.");
            }
            else if (speech.Contains("anarchist"))
            {
                Say("You seek the Anarchist's Cache, do you? To earn it, you must prove your worth.");
            }
            else if (speech.Contains("prove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must return later to earn the reward.");
                }
                else
                {
                    Say("Your knowledge of our cause is commendable. For your efforts, take this Anarchist's Cache as your reward.");
                    from.AddToBackpack(new AnarchistsCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Anarchist(Serial serial) : base(serial) { }

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
