using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grand Duke Vladimir Alexandrovich")]
    public class GrandDukeVladimir : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrandDukeVladimir() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grand Duke Vladimir Alexandrovich";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1155 });
            AddItem(new PlateLegs() { Hue = 1155 });
            AddItem(new PlateArms() { Hue = 1155 });
            AddItem(new PlateHelm() { Hue = 1155 });
            AddItem(new MetalShield() { Hue = 1155 });
            AddItem(new FancyShirt() { Hue = 1155 });

            Hue = Race.RandomSkinHue(); // Skin color
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, I am Grand Duke Vladimir Alexandrovich, guardian of the royal treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the fortress walls, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to oversee the royal treasures and ensure they are safeguarded.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasures of the Tsar! Many seek them, but only those who prove their worth shall be rewarded.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must engage in a dialogue and show your knowledge of our virtues.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtue is the essence of nobility. Display your understanding and you may be rewarded.");
            }
            else if (speech.Contains("nobility"))
            {
                Say("Nobility is not just about rank but the character one upholds. Show me your knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Your knowledge of virtue and nobility is impressive. What else do you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to explore beyond the surface. Do you understand the depth of our customs?");
            }
            else if (speech.Contains("customs"))
            {
                Say("Our customs are a reflection of our values. Have you pondered their significance?");
            }
            else if (speech.Contains("ponder"))
            {
                Say("Pondering reveals deeper truths. It shows you value wisdom. Are you ready for a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("A challenge awaits those who seek to understand. Do you have the courage to face it?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is essential in every endeavor. Show me your resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the backbone of strength. If you have shown true resolve, I will reward you.");
            }
            else if (speech.Contains("backbone"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your understanding of virtue, nobility, and resolve is commendable. As a reward, take this royal chest!");
                    from.AddToBackpack(new TsarsRoyalChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public GrandDukeVladimir(Serial serial) : base(serial) { }

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
