using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class ProfessorDroidicus : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public ProfessorDroidicus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Droidicus";
            Body = 0x190; // Male body
            Title = "the Droid Engineer";

            // Equipment
            AddItem(new Robe(Utility.RandomMetalHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomMetalHue()));
            AddItem(new PlateGorget());

            // Skills
            SetSkill(SkillName.Tinkering, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 70.0, 90.0);
            SetSkill(SkillName.Inscribe, 70.0, 90.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Professor Droidicus, the renowned Droid Engineer.");
            }
            else if (speech.Contains("job"))
            {
                Say("My primary job is to repair and maintain droids. They are crucial for the advancement of technology.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, thanks to my advanced maintenance protocols.");
            }
            else if (speech.Contains("droids"))
            {
                Say("Droids are fascinating machines. They assist with many tasks and can be quite versatile.");
            }
            else if (speech.Contains("technology"))
            {
                Say("Technology is ever-evolving. Understanding its intricacies can unlock great potential.");
            }
            else if (speech.Contains("repair"))
            {
                Say("To repair a droid, one must have patience and the right tools. I have both.");
            }
            else if (speech.Contains("workshop"))
            {
                Say("The Droid Workshop is where I create and repair droids. It is filled with various parts and schematics.");
            }
            else if (speech.Contains("reward"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("Prove your worth, and I shall reward you with a special item.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return true; // You can add specific conditions if needed
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                DroidWorkshopChest chest = new DroidWorkshopChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Take this Droid Workshop Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public ProfessorDroidicus(Serial serial) : base(serial) { }

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
