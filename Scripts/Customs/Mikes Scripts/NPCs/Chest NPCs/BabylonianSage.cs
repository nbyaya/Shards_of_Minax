using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class BabylonianSage : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public BabylonianSage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Babylonian Sage";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Sage";

            // Appearance
            AddItem(new Robe(Utility.RandomBrightHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomNeutralHue()));

            SetSkill(SkillName.EvalInt, 75.0, 100.0);
            SetSkill(SkillName.Inscribe, 75.0, 100.0);
            SetSkill(SkillName.ItemID, 75.0, 100.0);
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
                        from.SendMessage("I am the Babylonian Sage, keeper of ancient secrets.");
                        break;
                    case "job":
                        from.SendMessage("My role is to safeguard the knowledge of Babylon and share it with worthy seekers.");
                        break;
                    case "health":
                        from.SendMessage("I am in good health, thanks to the wisdom of the ancients.");
                        break;
                    case "secrets":
                        from.SendMessage("Babylon is filled with hidden treasures and knowledge. Seek and you shall find.");
                        break;
                    case "treasures":
                        from.SendMessage("The treasures of Babylon are not just material wealth, but also the wisdom and knowledge we carry.");
                        break;
                    case "wisdom":
                        from.SendMessage("Wisdom is the greatest treasure of all. It guides us through life and illuminates the path forward.");
                        break;
                    case "reward":
                        if (CheckRewardConditions(from))
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("You must prove your worth before receiving the treasure.");
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
            // Placeholder for reward conditions
            return !m_RewardGiven;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                BabylonianChest chest = new BabylonianChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Take this Babylonian Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public BabylonianSage(Serial serial) : base(serial)
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
