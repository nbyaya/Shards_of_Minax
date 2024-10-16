using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marina the Mapper")]
    public class MarinaTheMapper : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarinaTheMapper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marina the Mapper";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 50;
            Int = 120;
            Hits = 70;

            // Appearance
            AddItem(new Kilt() { Hue = 45 });
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new Shoes() { Hue = 1175 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Marina the Mapper, keeper of maps and knowledge.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, as always. The world is my map, and I must stay fit to explore it.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to chart the lands and discover their hidden treasures. I'm a cartographer by trade.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("The virtue of Justice, a cornerstone of our world, guides my work. Without fairness, maps lose their meaning.");
            }
            else if (speech.Contains("reflect"))
            {
                Say("Justice reminds us that our actions affect others. How do you ensure your actions reflect this virtue, traveler?");
            }
            else if (speech.Contains("marina"))
            {
                Say("Ah, the name Marina has been passed down through my family for generations. Each of us has been dedicated to the craft of mapping.");
            }
            else if (speech.Contains("world"))
            {
                Say("The world is vast and full of secrets. By maintaining my health, I hope to uncover as many of these secrets as I can in my lifetime.");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("Being a cartographer is not just about drawing maps. It's about understanding the land, its history, and the stories it holds.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is one of the eight virtues that shape our world. Each virtue is a beacon, guiding us through life's challenges.");
            }
            else if (speech.Contains("generations"))
            {
                Say("My ancestors were explorers, always venturing into the unknown. Their legacy is the vast collection of maps I now safeguard.");
            }
            else if (speech.Contains("secrets"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("There are places in this world untouched by man, where treasures and mysteries await. If you're keen, I might share a secret location with you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("stories"))
            {
                Say("Every map has a story behind it – tales of adventure, peril, and discovery. If you ever wish to hear one, simply ask.");
            }
            else if (speech.Contains("eight"))
            {
                Say("The eight virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each one essential for a balanced life.");
            }

            base.OnSpeech(e);
        }

        public MarinaTheMapper(Serial serial) : base(serial) { }

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
