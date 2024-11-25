using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class MaximusPravda : BaseCreature
    {
        private DateTime m_LastRewardTime;

        [Constructable]
        public MaximusPravda() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Maximus Pravda";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Revolutionary";

            // Equip the NPC
            AddItem(new PlateChest() { Hue = Utility.RandomRedHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomRedHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomRedHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomRedHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomRedHue() });
            AddItem(new Sandals());

            // Set Skills
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Parry, 70.0, 90.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Maximus Pravda, champion of the people.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, fortified by the strength of the cause.");
            }
            else if (speech.Contains("job"))
            {
                Say("I lead the charge for justice and equality. My task is to ensure the well-being of all comrades.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The revolution is a grand struggle for the betterment of society. Only through unity can we achieve our goals.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Unity is the cornerstone of our strength. Together, we are unstoppable.");
            }
            else if (speech.Contains("cause"))
            {
                Say("Our cause is just and noble. It is the driving force behind all our actions.");
            }
            else if (speech.Contains("maximus"))
            {
                Say("Maximus Pravda, at your service. My name is known throughout the land for leadership in the fight for justice.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the light that guides us through the darkness. It ensures fairness and equality for all.");
            }
            else if (speech.Contains("fairness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - m_LastRewardTime < cooldown)
                {
                    Say("The rewards must be earned. Return in due time, comrade.");
                }
                else
                {
                    GiveReward(from);
                }
            }

            base.OnSpeech(e);
        }

        private void GiveReward(Mobile from)
        {
            ComradesCache chest = new ComradesCache();
            from.AddToBackpack(chest);
            Say("You have shown great understanding and commitment. Here is the Comrade's Cache as a token of appreciation.");
            m_LastRewardTime = DateTime.UtcNow;
        }

        public MaximusPravda(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
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
