using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Charming Charles")]
    public class CharmingCharles : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CharmingCharles() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Charming Charles";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 70;
            Int = 50;
            Hits = 75;

            // Appearance
            AddItem(new Tunic() { Hue = 1157 });
            AddItem(new Kilt() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 38 });
            AddItem(new GoldRing() { Name = "Charles's Ring" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Charming Charles, a courtesan.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, my dear.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession is the art of seduction and companionship.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("True charm lies not in beauty alone, but in kindness and compassion. Art thou compassionate?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your words resonate with wisdom. Remember, kindness begets kindness.");
            }
            else if (speech.Contains("charming"))
            {
                Say("Ah, my reputation precedes me! I was named so by the people of the town, for my ability to win hearts with just a conversation.");
            }
            else if (speech.Contains("perfect"))
            {
                Say("Yes, keeping oneself in good spirits and surrounding oneself with positive energies aids in perfect health. Do you believe in positive energies?");
            }
            else if (speech.Contains("seduction"))
            {
                Say("It's not just about physical allure. It's about understanding, listening, and connecting with someone's soul. Have you ever felt a deep connection with someone?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is a virtue that everyone should strive to possess. It's what separates us from the beasts. Have you encountered beasts in your travels?");
            }
            else if (speech.Contains("connection"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("A deep connection with someone can be the most rewarding experience in life. As a token of appreciation for our heartfelt chat, here's a small gift for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("positive"))
            {
                Say("Ah, the energies that surround us play a crucial role in our well-being. I have charms that help channel these energies. Would you like to see them?");
            }
            else if (speech.Contains("town"))
            {
                Say("The town has been my home for many years. The people here are kind-hearted and they've always welcomed travelers with open arms. Have you visited the local inn yet?");
            }
            else if (speech.Contains("beasts"))
            {
                Say("Ah, beasts can be as varied as people. Some are kind, while others are malicious. But understanding them is key to coexisting. Have you faced any challenges with them?");
            }

            base.OnSpeech(e);
        }

        public CharmingCharles(Serial serial) : base(serial) { }

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
