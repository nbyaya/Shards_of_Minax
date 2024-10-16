using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Harun al-Rashid")]
    public class HarunAlRashid : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HarunAlRashid() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Harun al-Rashid";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = 1152 }); // Robe with a matching hue
            AddItem(new Sandals() { Hue = 1152 }); // Sandals with the same hue
            AddItem(new GoldNecklace() { Name = "Necklace of the Abbasids" });

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
                Say("I am Harun al-Rashid, Caliph of the Abbasid Caliphate. What would you like to know?");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guide and protect the treasures of the Abbasid dynasty.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, though the weight of leadership can be heavy.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasures of the Abbasids are legendary. Do you seek such wealth?");
            }
            else if (speech.Contains("wealth"))
            {
                Say("Wealth is not just gold and jewels. It is the wisdom and knowledge we gather. But if you seek material wealth, you must first prove your worth.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove yourself, you must answer this question: What is the essence of the Abbasid’s glory?");
            }
            else if (speech.Contains("glory"))
            {
                Say("The essence of our glory lies in our knowledge and cultural achievements. But do you truly understand its value?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Correct. Knowledge is the key to our greatness. As a reward for your understanding, please accept this treasure chest.");
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new AbbasidsTreasureChest()); // Give the Abbasid’s Treasure Chest
                    lastRewardTime = DateTime.UtcNow;
                }
                else
                {
                    Say("You have already received your reward recently. Return later for more.");
                }
            }
            else
            {
                Say("I do not understand your words. Perhaps you should ask about something else.");
            }

            base.OnSpeech(e);
        }

        public HarunAlRashid(Serial serial) : base(serial) { }

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
