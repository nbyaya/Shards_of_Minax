using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class DameGraceSteelheart : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public DameGraceSteelheart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dame Grace Steelheart";
            Title = "the Fencing Virtuoso";
            Body = 0x191; // Female body
            Hue = Utility.RandomSkinHue();
            
            // Add fencing-themed equipment
            AddItem(new ChainChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new ChainLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });
            AddItem(new Longsword() { Hue = Utility.RandomMetalHue(), Name = "Steelheart's Sword" });

            // Set skills related to fencing
            SetSkill(SkillName.Fencing, 100.0);
            SetSkill(SkillName.Parry, 100.0);
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
                    from.SendMessage("Greetings, I am Dame Grace Steelheart, a virtuoso of the fencing arts.");
                }
                else if (speech.Contains("job"))
                {
                    from.SendMessage("I dedicate my days to perfecting the art of fencing and sharing my knowledge.");
                }
                else if (speech.Contains("health"))
                {
                    from.SendMessage("I am in excellent health, fortified by daily training.");
                }
                else if (speech.Contains("virtuoso"))
                {
                    from.SendMessage("A virtuoso is someone who has achieved mastery through practice and dedication.");
                }
                else if (speech.Contains("practice"))
                {
                    from.SendMessage("Practice is the path to mastery. Without it, even the most talented will struggle.");
                }
                else if (speech.Contains("mastery"))
                {
                    from.SendMessage("Mastery requires not just skill, but an understanding of the deeper principles of the art.");
                }
                else if (speech.Contains("principles"))
                {
                    from.SendMessage("The principles of fencing are grounded in balance, precision, and strategy.");
                }
                else if (speech.Contains("balance"))
                {
                    from.SendMessage("Balance is crucial in fencing. It allows for swift and precise movements.");
                }
                else if (speech.Contains("precision"))
                {
                    from.SendMessage("Precision is key to landing successful strikes and defending against attacks.");
                }
                else if (speech.Contains("strikes"))
                {
                    from.SendMessage("A well-placed strike can decide the outcome of a duel. It's both an art and a science.");
                }
                else if (speech.Contains("duel"))
                {
                    from.SendMessage("A duel is a test of skill and strategy, where preparation and execution are vital.");
                }
                else if (speech.Contains("preparation"))
                {
                    from.SendMessage("Preparation is essential for success. It involves mental readiness and physical conditioning.");
                }
                else if (speech.Contains("success"))
                {
                    from.SendMessage("Success in fencing is achieved through a combination of talent, practice, and perseverance.");
                }
                else if (speech.Contains("talent"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        from.SendMessage("You must first demonstrate your commitment to the art of fencing to earn my reward.");
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
            return true; // Assuming the player has met the conditions
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                WearableFencingChest chest = new WearableFencingChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Accept this Fencing Champion's Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public DameGraceSteelheart(Serial serial) : base(serial)
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
