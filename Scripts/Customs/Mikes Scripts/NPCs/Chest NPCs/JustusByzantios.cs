using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class JustusByzantios : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public JustusByzantios() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Justus Byzantios";
            Title = "the Byzantine Historian";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Appearance
            AddItem(new Robe(Utility.RandomMetalHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomMetalHue()));

            SetSkill(SkillName.Anatomy, 60.0, 90.0);
            SetSkill(SkillName.Tactics, 60.0, 90.0);
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
                    from.SendMessage("I am Justus Byzantios, a historian from the Byzantine era.");
                }
                else if (speech.Contains("job"))
                {
                    from.SendMessage("I am here to share the tales of the ancient Byzantine Empire and to guard its secrets.");
                }
                else if (speech.Contains("health"))
                {
                    from.SendMessage("My health is as robust as the Byzantine fortifications.");
                }
                else if (speech.Contains("byzantium"))
                {
                    from.SendMessage("Ah, Byzantium! The grandeur of the Eastern Roman Empire. Have you come to learn its secrets?");
                }
                else if (speech.Contains("secrets"))
                {
                    from.SendMessage("The secrets of Byzantium are many. If you truly wish to uncover them, you must prove your worth.");
                }
                else if (speech.Contains("prove"))
                {
                    from.SendMessage("Prove yourself by learning about the Byzantine culture and history. Only then will you earn a reward.");
                }
                else if (speech.Contains("reward"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        from.SendMessage("You must first demonstrate your knowledge of the Byzantine Empire.");
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
            // Here you can define specific conditions for giving the reward
            return true; // Assuming conditions are met
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                EmperorJustinianCache chest = new EmperorJustinianCache();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Take this Emperor Justinian's Cache as your reward.");
                m_RewardGiven = true;
            }
        }

        public JustusByzantios(Serial serial) : base(serial) { }

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
