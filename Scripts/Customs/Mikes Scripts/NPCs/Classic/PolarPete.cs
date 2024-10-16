using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Polar Pete")]
    public class PolarPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PolarPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Polar Pete";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 90;
            Hits = 160;

            // Appearance
            AddItem(new FurCape() { Hue = 1173 });
            AddItem(new FurBoots() { Hue = 1173 });
            AddItem(new FurSarong() { Hue = 1173 });
            AddItem(new Mace() { Name = "Polar Pete's Mace" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, traveler! I am Polar Pete, the animal tamer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to tame and care for the creatures of this land. It's a fulfilling task.");
            }
            else if (speech.Contains("compassion") && !speech.Contains("yes"))
            {
                Say("True compassion is shown through our treatment of animals. Do you have compassion for all creatures?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Compassion for all creatures is a virtue to uphold.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("I often ponder on the virtue of humility, for it is in our humility that we truly connect with nature.");
            }
            else if (speech.Contains("pete"))
            {
                Say("Ah, the name Polar Pete was given to me because I've spent much of my life in the icy regions, studying and interacting with arctic animals.");
            }
            else if (speech.Contains("good"))
            {
                Say("While my health is well now, the cold can sometimes take a toll. But the animals and the beauty of the wilderness keep me invigorated.");
            }
            else if (speech.Contains("tame"))
            {
                Say("My job allows me to travel and encounter various creatures. Recently, I've been working with majestic wolves. They're truly fascinating.");
            }
            else if (speech.Contains("icy"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In the icy regions, survival is a daily challenge. But there's a unique beauty to it, like watching the northern lights. By the way, for sharing this moment with me, here's a small reward for your journey.");
                    from.AddToBackpack(new PhysicalResistAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("wilderness"))
            {
                Say("The wilderness is both harsh and beautiful. The rawness of nature teaches me humility and respect.");
            }
            else if (speech.Contains("wolves"))
            {
                Say("Wolves are pack animals. They rely on each other and have a deep sense of community. It's something we humans can learn from.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Kindness is a simple gesture, yet its impact is profound. A little act can change someone's day or even their life.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is about standing up for what's right, even when it's difficult. It's about making the hard choices and facing challenges head-on.");
            }

            base.OnSpeech(e);
        }

        public PolarPete(Serial serial) : base(serial) { }

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
