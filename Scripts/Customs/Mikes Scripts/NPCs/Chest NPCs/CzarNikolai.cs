using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Czar Nikolai")]
    public class CzarNikolai : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CzarNikolai() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Czar Nikolai";
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
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            
            // Adding a robe for thematic consistency
            AddItem(new Robe() { Name = "Imperial Robe", Hue = 1157 });

            Hue = Race.RandomSkinHue(); // Skin hue
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, I am Czar Nikolai, protector of the Tsar's legacy.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as strong as the walls of the Kremlin.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the secrets of the Tsar's legacy and offer guidance to the worthy.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The Tsar's Legacy is not just a chest; it represents the opulence and grandeur of a bygone era.");
            }
            else if (speech.Contains("opulence"))
            {
                Say("Ah, opulence! It was a time of great wealth and splendor. To understand it is to grasp the essence of the Tsar's legacy.");
            }
            else if (speech.Contains("wealth"))
            {
                Say("Indeed, wealth was abundant. But true value lies in the wisdom and patience one demonstrates. Seek those virtues.");
            }
            else if (speech.Contains("value"))
            {
                Say("Value is not merely in gold or artifacts, but in the character and resolve of those who seek them. Do you have such resolve?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the cornerstone of greatness. Show me your resolve and you may be rewarded with a treasure worthy of your effort.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! Many seek it, but only the truly dedicated will find the reward. Are you prepared for a test of your dedication?");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication requires patience and wisdom. Speak to me of these virtues and I will guide you further.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue that must be nurtured. A test of patience will reveal your true worth. Are you ready for such a test?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through the darkness of ignorance. Speak wisely, and your path will be illuminated.");
            }
            else if (speech.Contains("test"))
            {
                Say("A test awaits those who are dedicated and wise. Prepare yourself, for the final trial will reveal if you are worthy of the Tsar's Legacy.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your patience and wisdom have proven you worthy. Accept this Tsar's Legacy Chest as a reward for your efforts.");
                    from.AddToBackpack(new TsarsLegacyChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public CzarNikolai(Serial serial) : base(serial) { }

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
