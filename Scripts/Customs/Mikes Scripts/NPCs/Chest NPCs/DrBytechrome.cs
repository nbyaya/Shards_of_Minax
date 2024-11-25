using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class DrBytechrome : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public DrBytechrome() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Bytechrome";
            Body = 0x190; // Male body
            Hue = Utility.RandomMetalHue(); // Metallic theme

            AddItem(new Robe(Utility.RandomMetalHue())); // Futuristic robe
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomMetalHue()));

            SetSkill(SkillName.EvalInt, 75.0, 100.0);
            SetSkill(SkillName.Inscribe, 75.0, 100.0);
            SetSkill(SkillName.ItemID, 75.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings! I am Dr. Bytechrome, the cybernetic scientist.");
            }
            else if (speech.Contains("job"))
            {
                Say("I specialize in the study of advanced technology and cybernetic enhancements.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am functioning optimally, thanks to my cybernetic enhancements.");
            }
            else if (speech.Contains("technology"))
            {
                Say("Technology is the key to unlocking the future. But it must be handled with care and wisdom.");
            }
            else if (speech.Contains("cybernetics"))
            {
                Say("Cybernetics represent the fusion of machine and biology, advancing the limits of both.");
            }
            else if (speech.Contains("fusion"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("You must demonstrate a true understanding of technology to earn a reward.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // For simplicity, this NPC always gives a reward
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                CyberneticCache chest = new CyberneticCache();
                from.AddToBackpack(chest);
                from.SendMessage("Congratulations! You have proven yourself worthy. Here is the Cybernetic Cache.");
                m_RewardGiven = true;
            }
        }

        public DrBytechrome(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
