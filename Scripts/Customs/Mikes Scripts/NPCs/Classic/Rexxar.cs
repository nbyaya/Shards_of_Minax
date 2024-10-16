using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rexxar")]
    public class Rexxar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Rexxar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rexxar";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 120;
            Int = 40;
            Hits = 80;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1909 });
            AddItem(new PlateChest() { Hue = 1909 });
            AddItem(new PlateHelm() { Hue = 1909 });
            AddItem(new PlateGloves() { Hue = 1909 });
            AddItem(new Boots() { Hue = 1909 });
            AddItem(new Halberd() { Name = "Rexxar's Plasma Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string speech = e.Speech.ToLower();

            if (!from.InRange(this, 3))
                return;

            if (speech.Contains("name"))
            {
                Say("I am Rexxar, the Bionic Soldier!");
            }
            else if (speech.Contains("health"))
            {
                Say("My bionic enhancements keep me in peak health.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am programmed for combat and protection.");
            }
            else if (speech.Contains("courageous"))
            {
                Say("True valor is in the strength of one's convictions. Are you courageous?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Courage is a valuable trait. Keep your convictions strong, and you shall never falter.");
            }
            else if (speech.Contains("bionic"))
            {
                Say("My bionic enhancements are courtesy of the grand inventor, Dr. Eldric. They amplify my strength, speed, and cognitive functions.");
            }
            else if (speech.Contains("eldric"))
            {
                Say("Ah, Dr. Eldric! He's the genius behind many mechanical wonders in this world. He's based in the Eldric Lab to the east.");
            }
            else if (speech.Contains("lab"))
            {
                Say("The Eldric Lab is a place of great innovation. However, recently it's been silent. No one knows what's going on inside. Maybe you could investigate?");
            }
            else if (speech.Contains("investigate"))
            {
                Say("If you dare to delve into the mysteries of Eldric Lab, be prepared. And if you find any valuable information, report back to me. I may have something for you as a reward.");
            }
            else if (speech.Contains("report"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, brave adventurer! What news do you bring from the lab? ... Intriguing! For your efforts, take this as a token of my gratitude.");
                    from.AddToBackpack(new PoisonResistAugmentCrystal()); // Replace with appropriate item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("combat"))
            {
                Say("Combat is a dance of tactics, strategy, and skill. Even with my enhancements, I constantly upgrade my combat protocols. Can you match my prowess?");
            }
            else if (speech.Contains("protection"))
            {
                Say("I have sworn to protect those in need. It's a duty I take seriously. If ever you find yourself in peril, just call for me.");
            }

            base.OnSpeech(e);
        }

        public Rexxar(Serial serial) : base(serial) { }

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
