using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Shen Long")]
    public class DrShenLong : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrShenLong() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Shen Long";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new SkullCap() { Hue = 1153 });
            AddItem(new FancyShirt() { Hue = 1153 });

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
                Say("Greetings, traveler. I am Dr. Shen Long, a humble scholar of the Tang Dynasty.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as a Tang Dynasty oak.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard the wisdom and treasures of the Tang era.");
            }
            else if (speech.Contains("tang"))
            {
                Say("Ah, the Tang Dynasty! A golden age of culture and prosperity. Do you seek knowledge of this grand period?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge of the Tang Dynasty is vast and profound. The secrets lie in its wisdom and artifacts.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom of the Tang is hidden in many forms. One must be curious and determined to uncover its secrets.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the Tang Dynasty are guarded well. Only those who are worthy and persistent shall find them.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy of the Tang secrets, one must show dedication and understanding of the era's legacy.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is key to uncovering the treasures of the Tang Dynasty. Have you explored its artifacts?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts from the Tang era are not just treasures, but pieces of history. They hold great value and knowledge.");
            }
            else if (speech.Contains("value"))
            {
                Say("The value of Tang artifacts lies in their historical significance and the stories they tell. Have you heard of the great treasures?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Indeed, the Tang Dynasty was known for its great treasures. These were not merely material wealth but symbols of its grandeur.");
            }
            else if (speech.Contains("grandeur"))
            {
                Say("The grandeur of the Tang Dynasty is reflected in its achievements and its lasting impact on history. Do you feel prepared for a reward?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("If you are truly prepared and have learned much, you are worthy of the reward. But first, understand the essence of Tang.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of the Tang Dynasty is found in its culture and achievements. If you grasp this, you will be ready for the reward.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Please return later.");
                }
                else
                {
                    Say("For your dedication to uncovering the legacy of the Tang Dynasty and understanding its essence, accept this special chest as your reward.");
                    from.AddToBackpack(new TangDynastyChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DrShenLong(Serial serial) : base(serial) { }

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
