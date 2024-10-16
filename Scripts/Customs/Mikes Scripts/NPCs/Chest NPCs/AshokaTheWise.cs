using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ashoka the Wise")]
    public class AshokaTheWise : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AshokaTheWise() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ashoka the Wise";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new SkullCap() { Hue = 1150 });
            AddItem(new QuarterStaff() { Hue = 1150 });

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

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Ashoka the Wise. I guard the secrets of ancient treasure.");
            }
            else if (speech.Contains("job"))
            {
                Say("My task is to protect and share wisdom about the legendary treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is well, as I live in harmony with the teachings of peace.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! To earn such a reward, you must prove your understanding of wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through darkness. What do you seek to know about it?");
            }
            else if (speech.Contains("ancient"))
            {
                Say("The ancient ways speak of a great treasure hidden from those who are unworthy.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, answer me this: What is the essence of peace?");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace is not merely the absence of conflict, but the presence of understanding and compassion.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding comes from within and from the harmonious balance with others.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the key to unlocking many of life's greatest treasures.");
            }
            else if (speech.Contains("unlock"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already received a reward. Please return later.");
                }
                else
                {
                    Say("You have shown great wisdom. Accept this treasure as a token of your understanding.");
                    from.AddToBackpack(new AshokasTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am not familiar with that. Seek wisdom and you may find what you seek.");
            }

            base.OnSpeech(e);
        }

        public AshokaTheWise(Serial serial) : base(serial) { }

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
