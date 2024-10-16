using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class PharaohRamenhotep : BaseCreature
    {
        private DateTime m_LastRewardTime;

        [Constructable]
        public PharaohRamenhotep() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pharaoh Ramenhotep";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Wise";

            // Equip the Pharaoh
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals());
            AddItem(new PlateHelm() { Hue = 1150 }); // Optional: Adding a crown for the Pharaoh's look
            AddItem(new BlackStaff() { Hue = 1150 }); // Optional: A scepter as a regal item

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Hiding, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);

            // Initialize the lastRewardTime to a past time
            m_LastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Pharaoh Ramenhotep, ruler of the sands and keeper of ancient secrets.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the sacred treasures of Egypt and ensure that the ancient wisdom is preserved.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, blessed by the gods themselves.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I guard are powerful and must be handled with great respect. Only those who prove their worth shall uncover them.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must first understand the significance of the treasures of Egypt.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Egyptian treasures are legendary. They hold magic and history that have lasted millennia.");
            }
            else if (speech.Contains("prove"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - m_LastRewardTime < cooldown)
                {
                    Say("You must wait a little longer before you can receive another reward.");
                }
                else
                {
                    Say("You have shown patience and wisdom. As a reward for your persistence, I present you with an Egyptian Chest!");
                    from.AddToBackpack(new EgyptianChest()); // Give the reward
                    m_LastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("ancient"))
            {
                Say("The ancient lands are filled with knowledge and mystery. Seek and you shall find many wonders.");
            }
            else if (speech.Contains("egypt"))
            {
                Say("Egypt is a land of great history and power. Its treasures are as vast as its sands.");
            }

            base.OnSpeech(e);
        }

        public PharaohRamenhotep(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_LastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_LastRewardTime = reader.ReadDateTime();
        }
    }
}
