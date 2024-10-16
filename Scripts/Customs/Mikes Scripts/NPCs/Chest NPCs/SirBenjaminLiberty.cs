using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Benjamin Liberty")]
    public class SirBenjaminLiberty : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirBenjaminLiberty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Benjamin Liberty";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomNeutralHue() });
            AddItem(new TricorneHat() { Name = "Patriot's Hat", Hue = 1157 });

            // Add a bit of flair with a cape or other accessories
            AddItem(new Cloak() { Hue = Utility.RandomRedHue() });

            // Appearance randomizations
            Hue = Utility.RandomSkinHue(); // Skin color
            HairItemID = Utility.RandomList(0x203B, 0x2049); // Different hair styles
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

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
                Say("Greetings, I am Sir Benjamin Liberty, protector of our cherished values.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in strong health, thanks to the freedoms we cherish and protect.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to uphold the values of liberty and to guide others in understanding them.");
            }
            else if (speech.Contains("values"))
            {
                Say("The values we hold dear are liberty, justice, and courage. Each is vital to our way of life.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice ensures that freedom is not taken for granted. It must be upheld by everyone.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to stand up for justice and liberty, even when faced with great challenges.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom allows us to express ourselves and pursue our ideals. It's a treasure that must be safeguarded.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, freedom is a treasure. And speaking of treasures, there is something special you should know about.");
            }
            else if (speech.Contains("special"))
            {
                Say("Yes, there is a chest of great significance. It embodies our ideals and holds many valuable items.");
            }
            else if (speech.Contains("embodies"))
            {
                Say("The chest embodies the spirit of our revolution. It symbolizes the sacrifices made for our freedom.");
            }
            else if (speech.Contains("sacrifices"))
            {
                Say("Many have sacrificed much for our freedoms. Their bravery is what allows us to enjoy the liberties we have.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is not the absence of fear but the willingness to face it for a greater cause. It is a virtue we celebrate.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtues guide us in our actions and decisions. They are the moral compass by which we navigate life's challenges.");
            }
            else if (speech.Contains("moral"))
            {
                Say("Morality is a framework for understanding what is right and wrong. It is essential for maintaining justice and harmony.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the balance we strive for in our society. It comes from respecting others and upholding our shared values.");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect for others is fundamental to a functioning society. It is the cornerstone of our interactions and relationships.");
            }
            else if (speech.Contains("interaction"))
            {
                Say("Every interaction we have is an opportunity to uphold our values and show respect for others. It's a continuous journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Our journey towards a just and free society is ongoing. Every step forward is a testament to our shared commitment.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Commitment to our ideals ensures that we honor the sacrifices made and continue to strive for a better future.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is shaped by our actions today. By embracing our values and working together, we can build a better world.");
            }
            else if (speech.Contains("world"))
            {
                Say("Yes, the world is shaped by our collective efforts. And to honor your understanding of our ideals, I have a reward for you.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your insightful understanding of our values and principles, accept this Revolutionary Chest. It is a symbol of our enduring freedom.");
                    from.AddToBackpack(new RevolutionaryChess()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirBenjaminLiberty(Serial serial) : base(serial) { }

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
