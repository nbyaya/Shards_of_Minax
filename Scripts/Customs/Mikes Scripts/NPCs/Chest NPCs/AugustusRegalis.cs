using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Augustus Regalis")]
    public class AugustusRegalis : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public AugustusRegalis() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Augustus Regalis";
            Title = "the Emperor's Herald";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals());
            AddItem(new Cloak() { Hue = Utility.RandomBlueHue() });

            // Optional: You can add more items or customize the appearance as needed.
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Augustus Regalis, the Emperor's Herald.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, as befits my esteemed station.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to oversee the distribution of the Emperor's most prized treasures.");
            }
            else if (speech.Contains("emperor"))
            {
                Say("The Emperor's legacy is immortalized through these treasures. They hold the essence of his reign.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The Emperor's Legacy Chest you seek is a testament to his greatness. But to receive it, you must prove your worth.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Prove your worthiness through understanding the Emperor's virtues and the treasure will be yours.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as honor, valor, and wisdom are the pillars of the Emperor's reign. Speak of them to show your worth.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is the true measure of a person's character. Speak with honor, and you shall be rewarded.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the courage to face adversity with strength. Show your valor in words, and you will be rewarded.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through darkness. Share your wisdom, and you will be deemed worthy.");
            }
            else if (speech.Contains("reward"))
            {
                if (!m_RewardGiven)
                {
                    Say("You have proven yourself worthy. Accept this Emperor's Legacy Chest as a token of the Emperor's favor.");
                    from.AddToBackpack(new EmperorLegacyChest()); // Give the reward
                    m_RewardGiven = true;
                }
                else
                {
                    Say("I have already given you the Emperor's Legacy Chest. Return if you seek more knowledge or virtue.");
                }
            }
            else
            {
                Say("Speak to me of honor, valor, or wisdom to learn more about the Emperor's Legacy.");
            }

            base.OnSpeech(e);
        }

        public AugustusRegalis(Serial serial) : base(serial)
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
