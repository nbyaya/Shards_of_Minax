using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marcus Aurelius Maximus")]
    public class MarcusAureliusMaximus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarcusAureliusMaximus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marcus Aurelius Maximus";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            AddItem(new Cloak() { Hue = 0x0 }); // Roman-inspired cloak

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Short hairstyles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x204B, 0x204C); // Beards

            // Speech Hue
            SpeechHue = 0;

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
                Say("Salve, I am Marcus Aurelius Maximus, guardian of ancient treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, as is fitting for a guardian of relics.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard the treasures of Constantine and ensure they remain hidden from unworthy hands.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, the treasures you seek are indeed magnificent. But to obtain them, one must first prove their worth.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must first understand the legacy of Constantine. Ask me more about it.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The legacy of Constantine is vast and storied. Speak of his empire or his deeds, and you will gain further insight.");
            }
            else if (speech.Contains("empire"))
            {
                Say("Constantine's empire was known for its grandeur and its impact on history. Reflect on its significance to understand more.");
            }
            else if (speech.Contains("history"))
            {
                Say("History holds the key to understanding the present. Speak of great historical events to continue.");
            }
            else if (speech.Contains("events"))
            {
                Say("Great events shape the course of history. Reflect on notable events and their impact on the world.");
            }
            else if (speech.Contains("impact"))
            {
                Say("The impact of historical events can be seen in many ways. Speak of notable figures or outcomes to proceed.");
            }
            else if (speech.Contains("figures"))
            {
                Say("Notable figures like Constantine had a lasting impact. Their stories are woven into the fabric of history.");
            }
            else if (speech.Contains("stories"))
            {
                Say("Stories of ancient heroes and rulers can reveal much. Discuss these stories to gain more understanding.");
            }
            else if (speech.Contains("heroes"))
            {
                Say("Heroes of the past have left us with great tales. Speak of such heroes to continue your quest.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Tales of valor and bravery are inspiring. Reflect on such tales to gain further insight.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the essence of heroism. Speak of valor and bravery to show your understanding.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is essential in the face of challenges. Demonstrate your own bravery in the quest for knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power. Those who seek it earn their rewards. Are you prepared for the final challenge?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("To be prepared is to be wise. If you have followed my guidance, you are now ready for the final reward.");
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
                    Say("Your dedication and understanding have earned you the greatest of treasures. Accept this chest as a token of my gratitude.");
                    from.AddToBackpack(new SpecialWoodenChestConstantine()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MarcusAureliusMaximus(Serial serial) : base(serial) { }

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
