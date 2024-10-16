using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Zappa")]
    public class DrZappa : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrZappa() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Zappa";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 100;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Hue = 1181 });
            AddItem(new PlateHelm() { Hue = 1181 });
            AddItem(new LeatherGloves() { Hue = 1181 });
            AddItem(new Shoes() { Hue = 1181 });
            AddItem(new FireballWand() { Name = "Dr. Zappa's Pistol" });

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
                Say("Greetings, I am Dr. Zappa, the scientist!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in perfect health, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to unravel the mysteries of the universe through science and experimentation!");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Science is a pursuit of knowledge and understanding, just as valor is the pursuit of courage. Do you seek knowledge?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Knowledge is the foundation of progress. Keep seeking it, and you shall find answers!");
            }
            else if (speech.Contains("compassion") || speech.Contains("humility") || speech.Contains("enlightenment"))
            {
                Say("Remember, the pursuit of knowledge should always be tempered with compassion and humility. Only then can it lead to true enlightenment.");
            }
            else if (speech.Contains("scientist"))
            {
                Say("Ah, being a scientist is not just about test tubes and experiments. It's about the quest to understand the very fabric of reality!");
            }
            else if (speech.Contains("perfect"))
            {
                Say("Indeed, maintaining one's health is essential, especially when you're often surrounded by volatile experiments and curious concoctions.");
            }
            else if (speech.Contains("experimentation"))
            {
                Say("Experimentation is the bedrock of discovery. Through it, I've uncovered many a secret and perhaps, with the right assistance, I could share one with you.");
            }
            else if (speech.Contains("quest"))
            {
                Say("Every quest begins with a question, a curiosity to uncover the unknown. Have you ever embarked on such a journey?");
            }
            else if (speech.Contains("experiments"))
            {
                Say("Some of my experiments have led to groundbreaking revelations, while others... well, they've had more explosive results. Always be cautious in the lab!");
            }
            else if (speech.Contains("assistance"))
            {
                Say("Ah, so you're interested in assisting me? Very well, help me with a particular task, and I shall reward you for your efforts.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Journeys can be physical, venturing into the unknown lands, or mental, diving deep into one's own thoughts and reflections. Both are equally enlightening.");
            }
            else if (speech.Contains("cautious"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Being cautious is essential, especially when dealing with unknown substances. One wrong move and it could be catastrophic! But with risk comes reward. Here, take this.");
                    from.AddToBackpack(new MiningAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DrZappa(Serial serial) : base(serial) { }

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
