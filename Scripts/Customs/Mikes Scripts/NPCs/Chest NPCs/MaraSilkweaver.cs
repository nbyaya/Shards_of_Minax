using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mara Silkweaver")]
    public class MaraSilkweaver : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MaraSilkweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mara Silkweaver";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 70;
            Int = 90;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomNeutralHue() });
            AddItem(new FancyShirt() { Hue = Utility.RandomNondyedHue() });
            AddItem(new BodySash() { Hue = Utility.RandomBrightHue() });
            AddItem(new FeatheredHat() { Hue = Utility.RandomBrightHue() });

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            if (!from.InRange(this, 3)) return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Mara Silkweaver, guardian of the Silk Road's treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, blessed by the wonders of the Silk Road.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect and share the rich treasures of the Silk Road with those who are worthy.");
            }
            else if (speech.Contains("silk road"))
            {
                Say("The Silk Road is a marvelous trade route, full of exotic goods and ancient secrets.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("The treasures I guard are of great value, collected from many distant lands along the Silk Road.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the Silk Road are not easily shared. Only those who prove their worth will learn them.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Proving your worth requires wisdom and patience. Show me your determination, and I shall reward you.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination is key to uncovering the Silk Road's hidden treasures. Are you prepared for a journey of discovery?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Being prepared means understanding the mysteries of the Silk Road. Have you pondered its many mysteries?");
            }
            else if (speech.Contains("ponder"))
            {
                Say("Pondering the mysteries of the Silk Road requires a thoughtful mind. Do you seek enlightenment on this journey?");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Enlightenment on the Silk Road comes from understanding its history and the lives of those who traveled it. Do you seek to learn more about its history?");
            }
            else if (speech.Contains("history"))
            {
                Say("The history of the Silk Road is rich with tales of adventure and discovery. To fully appreciate it, one must understand the diverse cultures it connected.");
            }
            else if (speech.Contains("cultures"))
            {
                Say("The Silk Road connected many cultures, each contributing to its legacy. Understanding these cultures enriches your journey. Are you ready to explore its legacy?");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The legacy of the Silk Road is a tapestry woven from the experiences of countless travelers. Are you ready to embrace its full story?");
            }
            else if (speech.Contains("story"))
            {
                Say("Every story along the Silk Road adds to its grandeur. By sharing these stories, we preserve the journey for future generations. Do you seek to become part of this story?");
            }
            else if (speech.Contains("part"))
            {
                Say("Becoming part of the Silk Road's story means embracing its treasures and its trials. If you truly seek to be a part of it, demonstrate your resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your resolve has impressed me. As a reward for your perseverance, accept this Silk Road Treasures Chest.");
                    from.AddToBackpack(new SilkRoadTreasuresChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MaraSilkweaver(Serial serial) : base(serial) { }

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
