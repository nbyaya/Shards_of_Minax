using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class GaiusAurus : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public GaiusAurus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gaius Aurus";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Roman Historian";

            AddItem(new Robe(Utility.RandomMetalHue())); // Roman-themed robe
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomMetalHue())); // Roman-style headgear

            // Stats
            SetSkill(SkillName.EvalInt, 75.0, 100.0);
            SetSkill(SkillName.Magery, 75.0, 100.0);
            SetSkill(SkillName.Parry, 75.0, 100.0);
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

                switch (speech)
                {
                    case "name":
                        from.SendMessage("Salutations! I am Gaius Aurus, a historian of ancient Rome.");
                        break;
                    case "job":
                        from.SendMessage("I delve into the annals of history and recount tales of Rome's grandeur.");
                        break;
                    case "health":
                        from.SendMessage("I am in robust health, fortified by the strength of ancient virtues.");
                        break;
                    case "history":
                        from.SendMessage("The chronicles of Rome are vast. They speak of valor, wisdom, and the rise and fall of emperors.");
                        break;
                    case "virtue":
                        from.SendMessage("Virtue was the backbone of Roman society. Wisdom, courage, and temperance were highly esteemed.");
                        break;
                    case "rome":
                        from.SendMessage("Rome, the Eternal City, is a treasure trove of stories and legends. If you seek knowledge, you've come to the right place.");
                        break;
                    case "treasure":
                        from.SendMessage("Ah, treasure! To uncover hidden riches, one must first earn the trust of those who guard it.");
                        break;
                    case "rewards":
                        if (CheckRewardConditions(from))
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("Prove your worthiness to receive the treasure I guard.");
                        }
                        break;
                    default:
                        base.OnSpeech(e);
                        break;
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Simple placeholder for reward conditions. Can be extended as needed.
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                CaesarChest chest = new CaesarChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have demonstrated your worthiness. Take this Caesar's Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public GaiusAurus(Serial serial) : base(serial)
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
