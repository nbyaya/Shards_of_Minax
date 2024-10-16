using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Nikolai Ivanovich")]
    public class NikolaiIvanovich : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NikolaiIvanovich() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nikolai Ivanovich";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = 1157 }); // Soviet themed color
            AddItem(new FancyShirt() { Hue = 1157 });
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Shoes() { Hue = 1157 });
            AddItem(new FeatheredHat() { Hue = 1157 });
			
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair item
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2046; // Random facial hair item

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
                Say("Greetings, I am Nikolai Ivanovich, guardian of the Soviet relics. Do you seek to understand the history of these artifacts?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Yes, these artifacts are a link to the past. To truly appreciate them, you must first understand their origins.");
            }
            else if (speech.Contains("origins"))
            {
                Say("The origins of these relics are rooted in the Soviet Union's rich and complex history. What else would you like to know?");
            }
            else if (speech.Contains("history"))
            {
                Say("History is a treasure trove of lessons. To unlock its secrets, one must delve deeply into the past. Are you prepared for such a journey?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are hidden within the folds of time. To uncover them, you must prove your curiosity and resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the key to perseverance. Those who are steadfast will find what they seek. Have you considered what you are truly searching for?");
            }
            else if (speech.Contains("searching"))
            {
                Say("Searching for knowledge is noble. But true understanding requires more than mere questing—it demands introspection.");
            }
            else if (speech.Contains("introspection"))
            {
                Say("Introspection allows one to see beyond the surface. It can reveal truths hidden from ordinary sight.");
            }
            else if (speech.Contains("truths"))
            {
                Say("Truths about the past can illuminate the present. Are you prepared to confront the reality of what you discover?");
            }
            else if (speech.Contains("reality"))
            {
                Say("Reality is shaped by perception. What you find depends on your perspective and willingness to embrace it.");
            }
            else if (speech.Contains("perspective"))
            {
                Say("Perspective can alter one's understanding. It is crucial to approach knowledge with an open mind.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is both a gift and a burden. It bestows wisdom but also responsibility.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("With great knowledge comes great responsibility. To handle it wisely, one must be cautious and thoughtful.");
            }
            else if (speech.Contains("thoughtful"))
            {
                Say("Thoughtfulness in one's approach can lead to deeper insights. Have you reflected on your journey so far?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey has its trials. The path to uncovering the relics' secrets is no different.");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials test one's resolve and worthiness. Overcoming them demonstrates your dedication.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to a cause often leads to great rewards. Your efforts thus far have shown great promise.");
            }
            else if (speech.Contains("rewards"))
            {
                Say("The ultimate reward is a testament to your perseverance. For proving your worth, you are deserving of the relics.");
            }
            else if (speech.Contains("relics"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this time. Please return later.");
                }
                else
                {
                    Say("Congratulations on your journey. For your dedication, I present to you the USSR Relics Chest, filled with treasures of the past.");
                    from.AddToBackpack(new USSRRelicsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public NikolaiIvanovich(Serial serial) : base(serial) { }

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
