using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of a shadowy figure")]
    public class ShadowyAssassin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShadowyAssassin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shadowy Assassin";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 75;
            Int = 70;
            Hits = 60;

            // Appearance
            AddItem(new LeatherChest() { Hue = 0x455 }); // Dark armor
            AddItem(new LeatherLegs() { Hue = 0x455 });
            AddItem(new LeatherGloves() { Hue = 0x455 });
            AddItem(new Boots() { Hue = 0x455 });
            AddItem(new Bandana() { Hue = 0x455 });

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
                Say("I am known only as the Shadowy Assassin. I am a master of stealth and secrecy.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as elusive as my presence. I remain hidden from prying eyes.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to guard secrets and ensure that they remain buried in shadows.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are power, and power is hidden away from those who do not understand it.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power lies in the shadows, where only the brave dare to tread.");
            }
            else if (speech.Contains("brave"))
            {
                Say("Bravery is not the absence of fear, but the will to overcome it. If you are brave, you may find a reward.");
            }
            else if (speech.Contains("absence"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Return later to prove your worth.");
                }
                else
                {
                    Say("You have proven your bravery and understanding. For your efforts, accept this coffer of secrets.");
                    from.AddToBackpack(new AssassinsCoffer()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ShadowyAssassin(Serial serial) : base(serial) { }

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
