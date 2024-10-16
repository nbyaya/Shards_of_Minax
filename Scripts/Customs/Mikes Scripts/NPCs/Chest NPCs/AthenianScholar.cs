using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class AthenianScholar : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public AthenianScholar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Athenian Scholar";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Scholar";

            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomBlueHue()));

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
                switch (e.Speech.ToLower())
                {
                    case "name":
                        from.SendMessage("My name is Pallas, the Athenian Scholar.");
                        break;
                    case "job":
                        from.SendMessage("I study ancient artifacts and share knowledge with those who seek it.");
                        break;
                    case "health":
                        from.SendMessage("I am in good health, thank you for asking.");
                        break;
                    case "artifacts":
                        from.SendMessage("The artifacts of Athens hold great power and wisdom. Seek them wisely.");
                        break;
                    case "wisdom":
                        from.SendMessage("To gain wisdom, one must first seek knowledge.");
                        break;
                    case "knowledge":
                        from.SendMessage("Knowledge is the key to unlocking the secrets of the past.");
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
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                AthenianTreasureChest chest = new AthenianTreasureChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Take this Athenian Treasure Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public AthenianScholar(Serial serial) : base(serial)
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
