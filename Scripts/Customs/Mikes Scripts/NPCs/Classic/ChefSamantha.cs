using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Chef Samantha")]
    public class ChefSamantha : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ChefSamantha() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Chef Samantha";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 50;
            Int = 70;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 443 });
            AddItem(new FancyShirt() { Hue = 490 });
            AddItem(new Boots() { Hue = 30 });
            AddItem(new LeatherGloves() { Name = "Samantha's Oven Mitts" });

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
                Say("Welcome, traveler! I am Chef Samantha, the finest cook in these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("Cooking is my passion, my livelihood. It's not just a job; it's an art.");
            }
            else if (speech.Contains("cooking"))
            {
                Say("True virtue lies in the art of cooking. Are you a culinary enthusiast?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Excellent! Tell me, what's your favorite dish to savor?");
            }
            else if (speech.Contains("chef"))
            {
                Say("Ah, you've heard of me! It's always a pleasure to meet someone who recognizes my culinary expertise.");
            }
            else if (speech.Contains("perfect"))
            {
                Say("It's all thanks to a balanced diet and the fresh ingredients I use in my cooking. Have you ever tried incorporating fresh herbs into your meals?");
            }
            else if (speech.Contains("art"))
            {
                Say("The art of cooking is not just about flavors, it's about creating an experience. Have you ever dined in a place that left an unforgettable memory?");
            }
            else if (speech.Contains("expertise"))
            {
                Say("I've spent years traveling the world, learning from various cultures. From the spiced dishes of the deserts to the cool flavors of the northern shores. Every region has its unique taste.");
            }
            else if (speech.Contains("herbs"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Herbs can truly elevate a dish. If you ever come across rare herbs, do bring them to me. I might reward you for your effort.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("banquet"))
            {
                Say("It was a night to remember. Royalty from various kingdoms gathered, and the pressure was immense. But the satisfaction of seeing them enjoy my creations was unparalleled.");
            }
            else if (speech.Contains("fusion"))
            {
                Say("Fusion cooking is about merging the best of both worlds. It's challenging but also immensely rewarding when done right. Have you tried any fusion dishes? Try This!");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
            }

            base.OnSpeech(e);
        }

        public ChefSamantha(Serial serial) : base(serial) { }

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
