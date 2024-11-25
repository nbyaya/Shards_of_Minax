using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class SageWillow : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public SageWillow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sage Willow";
            Body = 0x190; // Male body
            Title = "the Sage";

            // Appearance
            AddItem(new Tunic(Utility.RandomNondyedHue()));
            AddItem(new Sandals(Utility.RandomNondyedHue()));
            AddItem(new Cap(Utility.RandomNondyedHue()));

            SetSkill(SkillName.AnimalTaming, 80.0, 100.0);
            SetSkill(SkillName.AnimalLore, 80.0, 100.0);
            SetSkill(SkillName.Veterinary, 80.0, 100.0);
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
                    from.SendMessage("I am Sage Willow, guardian of ancient wisdom.");
                }
                else if (speech.Contains("job"))
                {
                    from.SendMessage("My role is to safeguard the knowledge and treasures of our heritage.");
                }
                else if (speech.Contains("health"))
                {
                    from.SendMessage("I am in good health, thanks to the wisdom of the ancients.");
                }
                else if (speech.Contains("wisdom"))
                {
                    from.SendMessage("Wisdom is the key to understanding the world and its mysteries.");
                }
                else if (speech.Contains("treasures"))
                {
                    from.SendMessage("Our treasures are not just material but hold great historical value.");
                }
                else if (speech.Contains("heritage"))
                {
                    from.SendMessage("Our heritage is rich and filled with powerful artifacts. The true value lies in understanding their significance.");
                }
                else if (speech.Contains("artifact"))
                {
                    from.SendMessage("Artifacts tell stories of the past and hold the key to our future.");
                }
                else if (speech.Contains("stories"))
                {
                    from.SendMessage("The stories of old reveal the secrets of our ancestors. They are woven into our artifacts.");
                }
                else if (speech.Contains("secrets"))
                {
                    from.SendMessage("The secrets of the ancients are hidden within their artifacts. Discover them if you dare.");
                }
                else if (speech.Contains("discover"))
                {
                    from.SendMessage("To discover is to seek the unknown. Are you ready to unlock the mysteries?");
                }
                else if (speech.Contains("mysteries"))
                {
                    from.SendMessage("Mysteries are the heart of our lore. Solve them, and the rewards will follow.");
                }
                else if (speech.Contains("rewards"))
                {
                    from.SendMessage("The greatest reward comes from unraveling the mysteries of our heritage.");
                }
                else if (speech.Contains("unlock"))
                {
                    from.SendMessage("Unlocking the truth requires patience and wisdom.");
                }
                else if (speech.Contains("truth"))
                {
                    from.SendMessage("The truth is a reflection of our journey. Prove your worth and claim your prize.");
                }
                else if (speech.Contains("prize"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        from.SendMessage("You must prove your worthiness before receiving such a treasure.");
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
                from.SendMessage("You have proven yourself worthy. Take this First Nations' Heritage Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public SageWillow(Serial serial) : base(serial) { }

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
