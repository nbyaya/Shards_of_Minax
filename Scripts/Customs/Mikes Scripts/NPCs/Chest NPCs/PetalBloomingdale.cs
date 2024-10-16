using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class PetalBloomingdale : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public PetalBloomingdale() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Petal Bloomingdale";
            Body = 0x191; // Human female body
            Hue = Utility.RandomSkinHue();
            Title = "the Flower Child";

            // Equip Petal in hippie-themed attire
            AddItem(new Shirt(Utility.RandomPinkHue()));
            AddItem(new Kilt(Utility.RandomGreenHue()));
            AddItem(new Sandals(Utility.RandomNeutralHue()));
            AddItem(new FeatheredHat(Utility.RandomBrightHue()));

            // Add other visual elements to fit the theme
            AddItem(new Cloak(Utility.RandomPinkHue()) { Name = "Peace Flower" });
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Hey there! I'm Petal Bloomingdale, spreading peace and love.");
            }
            else if (speech.Contains("job"))
            {
                Say("I spend my days celebrating the beauty of nature and sharing positive vibes.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm as healthy as a spring flower in full bloom!");
            }
            else if (speech.Contains("nature"))
            {
                Say("Nature is our greatest gift. Embrace its beauty and find your inner peace.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace begins with a smile. Let's spread it far and wide.");
            }
            else if (speech.Contains("flower"))
            {
                Say("Flowers are a symbol of peace and love. They remind us of the simple joys of life.");
            }
            else if (speech.Contains("love"))
            {
                Say("Love is the essence of life. It's what makes our hearts bloom like flowers.");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is found in the little things, like a sunny day or a gentle breeze.");
            }
            else if (speech.Contains("bloom"))
            {
                Say("Just like flowers, we all have the potential to bloom and bring beauty into the world.");
            }
            else if (speech.Contains("beauty"))
            {
                Say("Beauty is all around us. Sometimes we just need to open our eyes and hearts to see it.");
            }
            else if (speech.Contains("eyes"))
            {
                Say("Our eyes are windows to our souls. They reflect what we truly feel inside.");
            }
            else if (speech.Contains("soul"))
            {
                Say("Our soul is our true self. It is where we find our deepest desires and passions.");
            }
            else if (speech.Contains("passion"))
            {
                Say("Passion fuels our dreams and drives us to pursue our deepest goals.");
            }
            else if (speech.Contains("dreams"))
            {
                Say("Dreams are the seeds of our future. Nurture them with hope and love.");
            }
            else if (speech.Contains("hope"))
            {
                Say("Hope is the light that guides us through dark times. Keep it close and let it shine.");
            }
            else if (speech.Contains("light"))
            {
                Say("Light dispels darkness and illuminates the path ahead. It represents truth and clarity.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Truth is the foundation of trust. It helps us connect with others on a deeper level.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is built over time and is essential for any meaningful relationship.");
            }
            else if (speech.Contains("relationship"))
            {
                Say("Relationships are the ties that bind us. They bring richness and depth to our lives.");
            }
            else if (speech.Contains("richness"))
            {
                Say("Richness is not just about material wealth but the depth of our experiences and connections.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience shapes who we are and helps us grow. Embrace every moment as a learning opportunity.");
            }
            else if (speech.Contains("growth"))
            {
                Say("Growth is a continual process. Embrace change and let it guide you to new heights.");
            }
            else if (speech.Contains("change"))
            {
                Say("Change is a natural part of life. It brings new opportunities and challenges.");
            }
            else if (speech.Contains("opportunities"))
            {
                Say("Opportunities are chances to explore new paths. Take them with open arms and an open heart.");
            }
            else if (speech.Contains("path"))
            {
                Say("Our path is shaped by our choices and actions. Walk it with purpose and joy.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose gives our lives direction and meaning. Find what drives you and pursue it passionately.");
            }
            else if (speech.Contains("direction"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("You need to show more love and harmony before receiving a reward.");
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
                FlowerPowerChest chest = new FlowerPowerChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have embraced the spirit of peace and love! Enjoy this Flower Power Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public PetalBloomingdale(Serial serial) : base(serial) { }

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
