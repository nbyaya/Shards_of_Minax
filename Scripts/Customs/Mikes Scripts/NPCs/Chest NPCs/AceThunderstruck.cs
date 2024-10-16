using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ace Thunderstruck")]
    public class AceThunderstruck : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AceThunderstruck() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ace Thunderstruck";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 75;

            // Appearance
            AddItem(new LeatherCap() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new StuddedGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Longsword() { Hue = Utility.RandomMetalHue(), Name = "Thunderblade" });

            // Hair and Facial Hair
            HairItemID = Utility.RandomList(0x203B, 0x2040, 0x203C); // Various hairstyles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x2042, 0x2045); // Various facial hair styles
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
                Say("Hey there! I'm Ace Thunderstruck, rock legend and keeper of secrets.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("I've got wild tales and treasures, but you'll need to prove you're a true fan.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Show me you know the rhythm of rock and roll, and I'll reward you.");
            }
            else if (speech.Contains("rhythm"))
            {
                Say("The rhythm is the heartbeat of rock. Can you keep up?");
            }
            else if (speech.Contains("keep up"))
            {
                Say("If you can keep up with the beat, you'll understand the essence of rock.");
            }
            else if (speech.Contains("beat"))
            {
                Say("The beat of rock is relentless and powerful. It's all about energy.");
            }
            else if (speech.Contains("energy"))
            {
                Say("Energy drives the rock spirit. What fuels your energy?");
            }
            else if (speech.Contains("fuel"))
            {
                Say("Passion fuels energy. Are you passionate about rock?");
            }
            else if (speech.Contains("passionate"))
            {
                Say("Passion is the soul of rock. Show me your passion!");
            }
            else if (speech.Contains("soul"))
            {
                Say("The soul of rock is in its heart and its fans. Do you have the soul of a rockstar?");
            }
            else if (speech.Contains("rockstar"))
            {
                Say("A true rockstar embodies the spirit of rock. If you have it, you'll be rewarded.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The rock spirit is unyielding. Show me you've got it!");
            }
            else if (speech.Contains("show"))
            {
                Say("Show me you’ve got the spirit of a rockstar, and you’ll earn a special reward.");
            }
            else if (speech.Contains("rocker"))
            {
                Say("Ah, a true rocker! For your enthusiasm, here's a special Rocker’s Vault.");
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new RockersVault()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("Come back later for your reward. Rock on!");
                }
            }

            base.OnSpeech(e);
        }

        public AceThunderstruck(Serial serial) : base(serial) { }

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
