using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cyrus Scepterius")]
    public class CyrusScepterius : BaseCreature
    {
        private bool m_RewardGiven;
        private DateTime lastRewardTime;

        [Constructable]
        public CyrusScepterius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cyrus Scepterius";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Ancient Guardian";

            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });

            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 75.0, 100.0);
            SetSkill(SkillName.Parry, 75.0, 100.0);

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
                Say("Greetings, I am Cyrus Scepterius, the Guardian of the Ancient Treasure.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as eternal as the sands of time.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the ancient secrets and treasure of Cyrus the Great.");
            }
            else if (speech.Contains("cyrus"))
            {
                Say("Cyrus, the great king of Persia, left a legacy of wealth and wisdom. Seek his treasure with reverence.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The treasure of Cyrus is hidden and requires both wit and courage to obtain.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes from understanding the past and seeking the truth.");
            }
            else if (speech.Contains("seek"))
            {
                Say("If you seek the treasure, you must prove your worth by solving the riddles I present.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, answer these questions and show your knowledge.");
            }
            else if (speech.Contains("riddles"))
            {
                Say("Riddles are the keys to unlock the treasure. Answer them wisely.");
            }
            else if (speech.Contains("answer"))
            {
                Say("You have answered well. As a reward for your wisdom and perseverance, accept this chest of treasures.");
                if (!m_RewardGiven && (DateTime.UtcNow - lastRewardTime).TotalMinutes >= 10)
                {
                    from.AddToBackpack(new CyrusTreasure());
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    m_RewardGiven = true;
                }
                else
                {
                    Say("You have already received your reward. Return later for more.");
                }
            }

            base.OnSpeech(e);
        }

        public CyrusScepterius(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
