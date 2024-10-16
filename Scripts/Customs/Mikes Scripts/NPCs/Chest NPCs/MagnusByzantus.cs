using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Magnus Byzantus")]
    public class MagnusByzantus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MagnusByzantus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Magnus Byzantus";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1157 }); // Byzantine-inspired robe
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new GoldRing() { Name = "Ring of the Emperor" });
            AddItem(new Spellbook() { Name = "Magnus's Manuscripts" });

            // Hair and Facial Hair
            HairItemID = 0x204F; // Long hair
            HairHue = 0x47E; // Gray
            FacialHairItemID = 0x203E; // Beard
            FacialHairHue = 0x47E; // Gray

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

            // Keyword chain
            if (speech.Contains("name"))
            {
                Say("Greetings, I am Magnus Byzantus, keeper of Byzantine treasures and lore. If you seek wisdom, you must start by understanding the Empire's past.");
            }
            else if (speech.Contains("empire"))
            {
                Say("The Byzantine Empire was a beacon of culture and wealth. Its treasures are legendary. To uncover them, one must understand the lore.");
            }
            else if (speech.Contains("lore"))
            {
                Say("Ah, the lore! The lore tells of great emperors and their legacies. Speak of the emperors, and you shall learn more.");
            }
            else if (speech.Contains("emperor"))
            {
                Say("The emperors, such as Justinian, shaped the course of history. Do you know of Justinian's deeds?");
            }
            else if (speech.Contains("justinian"))
            {
                Say("Justinian was a great emperor known for his codification of laws and the construction of the Hagia Sophia. He left behind many treasures.");
            }
            else if (speech.Contains("hagia sophia"))
            {
                Say("The Hagia Sophia was a masterpiece of Byzantine architecture. Its splendor is a testament to the Empire's grandeur.");
            }
            else if (speech.Contains("architecture"))
            {
                Say("Byzantine architecture is renowned for its domes and mosaics. To understand it, one must appreciate the craftsmanship.");
            }
            else if (speech.Contains("craftsmanship"))
            {
                Say("The craftsmanship of the Byzantines is reflected in their artifacts. Speak of artifacts to delve deeper.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts from the Byzantine era include coins, jewelry, and scrolls. These items hold great historical value.");
            }
            else if (speech.Contains("coins"))
            {
                Say("Byzantine coins, like the solidus and nomisma, were symbols of economic power. Do you seek knowledge about these coins?");
            }
            else if (speech.Contains("solidus"))
            {
                Say("The solidus was a gold coin introduced by Constantine the Great. It became the standard currency of the Byzantine Empire.");
            }
            else if (speech.Contains("nomisma"))
            {
                Say("The nomisma was another important coin used in Byzantium. Its value and design reflect the Empire's wealth.");
            }
            else if (speech.Contains("wealth"))
            {
                Say("The wealth of the Byzantine Empire was immense, reflected in its art, architecture, and treasures. Speak of treasures to proceed.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("The treasures of Byzantium are vast and varied. To claim one, you must show your dedication and understanding.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to knowledge and respect for history are key. Prove your dedication by demonstrating your knowledge of Byzantine lore.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Your knowledge of Byzantine history and culture is commendable. As a reward for your dedication, accept this chest of Justinian's treasures.");
                TimeSpan cooldown = TimeSpan.FromMinutes(15);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new JustinianTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("return"))
            {
                Say("Return when you are ready, and continue your quest for knowledge. The treasures of Byzantium await the worthy.");
            }

            base.OnSpeech(e);
        }

        public MagnusByzantus(Serial serial) : base(serial) { }

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
