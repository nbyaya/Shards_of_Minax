using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class ElderBirch : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public ElderBirch() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Elder Birch";
            Body = 0x190; // Male body
            Title = "the Elder";

            // Appearance
            AddItem(new Robe(Utility.RandomGreenHue()));
            AddItem(new Sandals(Utility.RandomGreenHue()));
            AddItem(new GnarledStaff(Utility.RandomGreenHue()));

            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.ItemID, 80.0, 100.0);
            SetSkill(SkillName.AnimalLore, 80.0, 100.0);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    from.SendMessage("I am Elder Birch, keeper of the ancient grove.");
                }
                else if (speech.Contains("job"))
                {
                    from.SendMessage("I oversee the spiritual well-being of our lands and preserve its secrets.");
                }
                else if (speech.Contains("health"))
                {
                    from.SendMessage("I am well, sustained by the vitality of the forest.");
                }
                else if (speech.Contains("forest"))
                {
                    from.SendMessage("The forest is a place of great power and wisdom, where the spirits dwell.");
                }
                else if (speech.Contains("spirits"))
                {
                    from.SendMessage("Spirits of the land guide and protect us. Their wisdom is ancient and profound.");
                }
                else if (speech.Contains("profound"))
                {
                    from.SendMessage("Profound knowledge is hidden within the forest, awaiting those who seek it.");
                }
                else if (speech.Contains("knowledge"))
                {
                    from.SendMessage("Knowledge of the forest is essential to understanding its mysteries.");
                }
                else if (speech.Contains("mysteries"))
                {
                    from.SendMessage("Mysteries of the forest are revealed to those who respect its balance.");
                }
                else if (speech.Contains("balance"))
                {
                    from.SendMessage("Balance is key to harmony. Disrupt it, and the forest will respond.");
                }
                else if (speech.Contains("harmony"))
                {
                    from.SendMessage("Harmony within the forest ensures its survival and prosperity.");
                }
                else if (speech.Contains("prosperity"))
                {
                    from.SendMessage("Prosperity comes from understanding and nurturing the land.");
                }
                else if (speech.Contains("nurture"))
                {
                    from.SendMessage("To nurture the land is to care for its needs and honor its gifts.");
                }
                else if (speech.Contains("gifts"))
                {
                    from.SendMessage("The gifts of the land are many. They are revealed to those who prove their respect.");
                }
                else if (speech.Contains("reward"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        from.SendMessage("You must show true respect for the land before receiving its gifts.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                FirstNationsHeritageChest chest = new FirstNationsHeritageChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have shown great respect. Accept this First Nations' Heritage Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public ElderBirch(Serial serial) : base(serial) { }

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
