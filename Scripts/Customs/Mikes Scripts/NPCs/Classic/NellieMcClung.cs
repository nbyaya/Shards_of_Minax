using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Nellie McClung")]
    public class NellieMcClung : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NellieMcClung() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nellie McClung";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 80;
            Int = 110;
            Hits = 70;

            // Appearance
            AddItem(new Kilt() { Hue = 1115 });
            AddItem(new FancyShirt() { Hue = 1120 });
            AddItem(new Boots() { Hue = 1155 });
            AddItem(new Bonnet() { Hue = 1132 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler! I am Nellie McClung, a proud Canadian.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm a writer and a suffragist. I fight battles for women's rights!");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is seen not in the force of arms, but in the force of will! Are you familiar with the virtue of Valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, Valor is the courage to stand up for what you believe in. Do you possess such courage?");
            }
            else if (speech.Contains("canadian"))
            {
                Say("Yes, I hail from the vast and beautiful lands of Canada. Have you ever visited our northern territories?");
            }
            else if (speech.Contains("good"))
            {
                Say("Being active in one's community and having a purpose keeps the spirit invigorated. How do you keep yourself well?");
            }
            else if (speech.Contains("writer"))
            {
                Say("I've penned numerous works in my time, advocating for the rights of women and the underrepresented. Have you read any of my writings?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is one of the virtues I hold close to my heart. Another virtue dear to me is Compassion. Do you believe in the power of Compassion?");
            }
            else if (speech.Contains("courage"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("It is commendable to have such courage! For your bravery, take this small token of appreciation from me.");
                    from.AddToBackpack(new WrestlingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("visited"))
            {
                Say("Canada is a country filled with breathtaking nature and warm-hearted folks. If you ever get a chance, you should experience its beauty firsthand.");
            }
            else if (speech.Contains("well"))
            {
                Say("A balanced life of physical activity, mental stimulation, and emotional fulfillment is essential. Seek these out, and your spirit will always be lifted.");
            }
            else if (speech.Contains("writings"))
            {
                Say("My writings mainly focus on pushing for equality, shedding light on the injustices faced by women. I always encourage readers to spread the word and educate others.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the ability to understand the suffering of others and wanting to alleviate it. It's a virtue that can change the world. Do you try to show compassion in your daily life?");
            }

            base.OnSpeech(e);
        }

        public NellieMcClung(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
