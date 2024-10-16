using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Johann Schmidt")]
    public class JohannSchmidt : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public JohannSchmidt() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Johann Schmidt";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Festmaster";

            // Outfit for the Bavarian festival
            AddItem(new FancyShirt(Utility.RandomGreenHue()));
            AddItem(new Kilt() { Hue = Utility.RandomGreenHue() });
            AddItem(new ThighBoots() { Hue = Utility.RandomGreenHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomGreenHue() });
            AddItem(new Cloak() { Hue = Utility.RandomGreenHue() });

            // Skills
            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings! I am Johann Schmidt, the Festmaster of this grand celebration.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to ensure the festival runs smoothly and to provide joy to all who attend.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thanks to the festive spirit that surrounds me.");
            }
            else if (speech.Contains("festival"))
            {
                Say("The festival is a time for merrymaking, dancing, and enjoying good company. It is a cherished tradition.");
            }
            else if (speech.Contains("merrymaking"))
            {
                Say("Ah, merrymaking is the heart of our festival. It keeps the spirits high and the days bright.");
            }
            else if (speech.Contains("tradition"))
            {
                Say("Tradition is what binds us together. It reminds us of our heritage and brings joy to our hearts.");
            }
            else if (speech.Contains("reward"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("You must show true festival spirit to earn the reward.");
                }
            }
            else
            {
                base.OnSpeech(e);
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
                BavarianFestChest chest = new BavarianFestChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have shown the spirit of the festival! Take this Bavarian Fest Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public JohannSchmidt(Serial serial) : base(serial) { }

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
