using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald Taffy")]
    public class SirReginaldTaffy : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public SirReginaldTaffy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Taffy";
            Body = 0x190; // Male body
            Hue = Utility.RandomPinkHue(); // Carnival theme hue
            Title = "the Candy Connoisseur";

            AddItem(new FancyShirt(Utility.RandomPinkHue()));
            AddItem(new ShortPants());
            AddItem(new Shoes(Utility.RandomPinkHue()));
            AddItem(new TricorneHat(Utility.RandomPinkHue()));
            AddItem(new QuarterStaff()); // Custom item if needed or use existing

            SetSkill(SkillName.Cooking, 75.0, 100.0);
            SetSkill(SkillName.TasteID, 75.0, 100.0);
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
                    Say("Greetings, traveler! I am Sir Reginald Taffy, the connoisseur of all things sweet and delightful.");
                }
                else if (speech.Contains("job"))
                {
                    Say("My job is to ensure every candy in the carnival is of the finest quality and to share the joy of sweets with all who visit.");
                }
                else if (speech.Contains("health"))
                {
                    Say("I am in splendid health, brimming with the joy of carnival cheer.");
                }
                else if (speech.Contains("candy"))
                {
                    Say("Ah, candy! The very essence of happiness. Have you tried the carnival's famous cotton candy?");
                }
                else if (speech.Contains("cheer"))
                {
                    Say("Carnival cheer is infectious! It’s the spirit of joy and celebration that fills the air with magic.");
                }
                else if (speech.Contains("infectious"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("You must prove your knowledge of carnival delights before receiving a reward.");
                    }
                }
                else
                {
                    Say("I see you are interested in the carnival! Ask me about candy, cheer, or perhaps even the secrets of our sweets.");
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Simple condition to ensure the reward is given only once
            return !m_RewardGiven;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                CandyCarnivalCoffer coffer = new CandyCarnivalCoffer();
                from.AddToBackpack(coffer);
                from.SendMessage("You have shown true understanding of the carnival's joys. Here is your very own Candy Carnival Coffer!");
                m_RewardGiven = true;
            }
        }

        public SirReginaldTaffy(Serial serial) : base(serial)
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
