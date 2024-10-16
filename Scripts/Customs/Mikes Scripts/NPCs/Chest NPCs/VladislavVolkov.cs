using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Vladislav Volkov")]
    public class VladislavVolkov : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VladislavVolkov() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Vladislav Volkov";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 95;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = 1140 });
            AddItem(new PlateLegs() { Hue = 1140 });
            AddItem(new PlateArms() { Hue = 1140 });
            AddItem(new PlateGloves() { Hue = 1140 });
            AddItem(new PlateHelm() { Hue = 1140 });
            AddItem(new MetalShield() { Hue = 1140 });
			
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x2046, 0x2049); // Random hairstyles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x204B, 0x204C); // Random facial hair

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
                Say("Greetings, traveler. I am Vladislav Volkov, guardian of ancient Slavic secrets. Do you seek the wisdom of legends?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, the secrets of old! Many tales are hidden in these lands. Have you heard of the ancient artifacts?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Yes, artifacts of great power. These relics hold the essence of Slavic heroes. You must prove your worth to seek them.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must understand the essence of legends and heroes. Have you pondered their stories?");
            }
            else if (speech.Contains("stories"))
            {
                Say("Indeed. The tales of old are filled with lessons and wisdom. What is it you seek from these tales?");
            }
            else if (speech.Contains("lessons"))
            {
                Say("Lessons of bravery, wisdom, and strength. Each tale reveals a piece of the puzzle. Have you reflected on these virtues?");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues such as courage and honor are revered in Slavic tales. Do you value these virtues in your journey?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the foundation of every hero’s tale. It drives one to face the unknown. Have you faced any great challenges?");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges test our resolve and spirit. Reflecting on these trials, have you sought guidance from ancient heroes?");
            }
            else if (speech.Contains("guidance"))
            {
                Say("Guidance can be found in the stories of ancient heroes. They offer wisdom for those who seek it. Are you ready to receive their gift?");
            }
            else if (speech.Contains("gift"))
            {
                Say("The greatest gift is knowledge and a symbol of heroism. Only those who truly understand the legends can claim it. Do you wish to proceed?");
            }
            else if (speech.Contains("proceed"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your journey through the legends has shown your worth. As a reward for your dedication, accept this Slavic Legends Chest.");
                    from.AddToBackpack(new SlavicBrosChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("heroism"))
            {
                Say("Heroism is celebrated in every tale of old. It’s a mark of greatness. Have you embraced the spirit of the heroes?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of the heroes lives in the hearts of those who honor their stories. You seem to understand this well. Continue to cherish these values.");
            }
            else if (speech.Contains("cherish"))
            {
                Say("Cherishing the tales and values of old keeps their memory alive. It’s a noble path you walk. Do you have any further questions about the legends?");
            }
            else if (speech.Contains("questions"))
            {
                Say("Ask what you will, traveler. The stories of Slavic heroes are vast, and I am here to guide you through them.");
            }
            else if (speech.Contains("guidance"))
            {
                Say("Indeed, guidance is key in unraveling the mysteries of the past. Continue seeking knowledge, and you shall uncover many secrets.");
            }

            base.OnSpeech(e);
        }

        public VladislavVolkov(Serial serial) : base(serial) { }

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
