using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cyber Engineer Ella")]
    public class CyberEngineerElla : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CyberEngineerElla() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cyber Engineer Ella";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 110;
            Int = 120;
            Hits = 60;

            // Appearance
            AddItem(new LongPants() { Hue = 2050 });
            AddItem(new Shirt() { Hue = 2050 });
            AddItem(new LeatherGloves() { Hue = 2050 });
            AddItem(new ThighBoots() { Hue = 2050 });
            AddItem(new Spellbook() { Name = "Ella's HoloPad" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("I am Cyber Engineer Ella, master of the digital realm!");
            }
            else if (speech.Contains("health"))
            {
                Say("My digital health is optimal!");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a cyber engineer, guardian of the digital frontier!");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom lies in the pursuit of knowledge! Are you a seeker of wisdom?");
            }
            else if (speech.Contains("yes") && lastRewardTime == DateTime.MinValue) // Add reward logic for `yes`
            {
                Say("Wisdom is the key to unlocking the mysteries of the digital realm!");
            }
            else if (speech.Contains("realm"))
            {
                Say("The digital realm is vast and complex, full of codes and algorithms. Have you ever ventured into it?");
            }
            else if (speech.Contains("digital"))
            {
                Say("It's always a pleasure to meet a fellow explorer of the digital landscape. Always remember, every byte holds a story!");
            }
            else if (speech.Contains("codes"))
            {
                Say("Codes are the backbone of the digital world, making sense of the chaos. If you're ever in need of deciphering one, just ask!");
            }
            else if (speech.Contains("ask"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Here, take this decoding ring. It might help you on your digital adventures. Consider it a gift from one tech enthusiast to another.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with appropriate reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("algorithm"))
            {
                Say("Algorithms are the guiding paths in our world, leading to solutions. Have you ever solved a complex algorithm?");
            }
            else if (speech.Contains("complex"))
            {
                Say("Impressive! Algorithms can be challenging, but they reveal the true beauty of computational logic.");
            }
            else if (speech.Contains("guardian"))
            {
                Say("As a guardian, I protect the data and ensure the smooth operation of the realm. Do you value protection?");
            }
            else if (speech.Contains("protection"))
            {
                Say("Protection in the digital frontier is crucial. Always be wary of the threats and stay vigilant!");
            }

            base.OnSpeech(e);
        }

        public CyberEngineerElla(Serial serial) : base(serial) { }

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
