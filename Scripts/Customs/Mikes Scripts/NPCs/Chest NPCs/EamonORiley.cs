using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class EamonORiley : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public EamonORiley() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Eamon O'Riley";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Emerald Keeper";

            // Equip items to match the emerald and Celtic theme
            AddItem(new Tunic() { Hue = 1270 }); // Green Tunic
            AddItem(new ThighBoots() { Hue = 1270 }); // Green Thigh Boots
            AddItem(new Cap() { Hue = 1270 }); // Green Cap
            AddItem(new Cloak() { Hue = 1270 }); // Green Cloak
            AddItem(new Apple() { Name = "Shamrock Apple" }); // Unique item related to theme

            SetSkill(SkillName.Magery, 75.0, 100.0);
            SetSkill(SkillName.Carpentry, 75.0, 100.0);
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
                        from.SendMessage("Greetings, traveler! I am Eamon O'Riley, the keeper of emerald secrets.");
                        break;
                    case "job":
                        from.SendMessage("My job is to safeguard the treasures of the Emerald Isle and share its tales.");
                        break;
                    case "health":
                        from.SendMessage("I am in good health, thank you. The emerald glen keeps me well.");
                        break;
                    case "emerald":
                        from.SendMessage("Ah, you speak of the Emerald Isle. It is a land of legend and treasure. Have you proven your worth?");
                        break;
                    case "worth":
                        from.SendMessage("If you wish to prove your worth, tell me about the tales of the Emerald Isle.");
                        break;
                    case "tales":
                        from.SendMessage("The tales of Emerald Isle speak of ancient magic and hidden treasures. Do you wish to hear more?");
                        break;
                    case "treasures":
                        from.SendMessage("The greatest treasure is the Emerald Isle Chest. But only those who are worthy may claim it.");
                        break;
                    case "chest":
                        if (!m_RewardGiven)
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("You have already received your reward. Farewell!");
                        }
                        break;
                    default:
                        base.OnSpeech(e);
                        break;
                }
            }
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                EmeraldIsleChest chest = new EmeraldIsleChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy! Take this Emerald Isle Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public EamonORiley(Serial serial) : base(serial)
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
