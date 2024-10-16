using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class DinerWaitress : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public DinerWaitress() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Misty Shakes";
            Title = "the Waitress";

            Body = 0x191; // Female body
            Hue = Utility.RandomSkinHue();

            AddItem(new Skirt(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new HalfApron(Utility.RandomNeutralHue()));
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            SetSkill(SkillName.Cooking, 75.0, 100.0);
            SetSkill(SkillName.TasteID, 75.0, 100.0);
        }


        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                switch (e.Speech.ToLower())
                {
                    case "name":
                        from.SendMessage("My name is Misty Shakes, and I run this fine diner!");
                        break;
                    case "job":
                        from.SendMessage("I serve the best milkshakes and burgers in town. What can I get for you?");
                        break;
                    case "health":
                        from.SendMessage("I'm feeling great, thank you for asking. Business has been booming!");
                        break;
                    case "diner":
                        from.SendMessage("We have the best dishes! From milkshakes to tip jars, everything is delicious.");
                        break;
                    case "milkshake":
                        from.SendMessage("Our vintage milkshake is a classic! It’s made with the finest ingredients.");
                        break;
                    case "burger":
                        from.SendMessage("Our burgers are legendary. Don’t forget to try our special sauce!");
                        break;
                    case "reward":
                        if (CheckRewardConditions(from))
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("You need to prove yourself worthy before I can give you a reward.");
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
            // Implement any conditions needed for giving the reward
            // For simplicity, we'll assume the condition is always true here
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                DinerDelightChest chest = new DinerDelightChest();
                from.AddToBackpack(chest);
                from.SendMessage("You've earned it! Here's the Diner Delight Chest as a reward.");
                m_RewardGiven = true;
            }
        }

        public DinerWaitress(Serial serial) : base(serial)
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
