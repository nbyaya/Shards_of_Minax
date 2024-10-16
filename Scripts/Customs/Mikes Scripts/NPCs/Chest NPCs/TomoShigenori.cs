using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tomo Shigenori")]
    public class TomoShigenori : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TomoShigenori() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tomo Shigenori";
            Body = 0x191; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new FancyDress() { Hue = 1157 }); // Elegant attire suitable for a samurai
            AddItem(new Sandals() { Hue = 1157 }); // Traditional footwear
            AddItem(new Katana() { Name = "Shigenori’s Blade", Hue = 1157 });

            // Facial features
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x204C; // Black hair
            HairHue = 0x0B2D; // Black hair color
            FacialHairItemID = 0x204D; // Beard

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
                Say("Greetings, I am Tomo Shigenori, once a proud samurai of the Minamoto clan.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, ready to share the tales of my adventures.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty was to guard the treasures of the clan and fight valiantly in battle.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, you seek treasure! It is said that treasure reveals itself to those who prove their worth.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must answer my queries wisely. Show me you have the heart of a true warrior.");
            }
            else if (speech.Contains("warrior"))
            {
                Say("A warrior's heart is steadfast and true. Show me you possess such qualities.");
            }
            else if (speech.Contains("quest"))
            {
                Say("To start, answer my questions about virtues and honor. Prove your knowledge and you shall be rewarded.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Virtues guide a warrior's path. True honor is reflected in one's actions and resolve.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues are the guiding principles of a noble life. They shape our actions and our destiny.");
            }
            else if (speech.Contains("destiny"))
            {
                Say("One's destiny is shaped by their actions and choices. What path do you seek?");
            }
            else if (speech.Contains("path"))
            {
                Say("The path of a warrior is one of courage and perseverance. Do you walk this path?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to face fear and adversity. How do you embody courage?");
            }
            else if (speech.Contains("fear"))
            {
                Say("Fear is natural, but it must be faced with resolve. What do you fear?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the determination to achieve one's goals despite challenges. Show me your resolve.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges test our resolve and character. What challenges have you overcome?");
            }
            else if (speech.Contains("overcome"))
            {
                Say("Overcoming challenges builds strength and wisdom. Reflect on your victories.");
            }
            else if (speech.Contains("victories"))
            {
                Say("Victories are celebrated, but true honor lies in the journey and growth.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey of a warrior is filled with trials and lessons. What have you learned on your journey?");
            }
            else if (speech.Contains("lessons"))
            {
                Say("Lessons from our experiences shape who we are. Share a lesson you have learned.");
            }
            else if (speech.Contains("share"))
            {
                Say("Sharing knowledge is a mark of wisdom. For your willingness to share, you have proven your worth.");
            }
            else if (speech.Contains("worth"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Return later, brave one. Your reward awaits.");
                }
                else
                {
                    Say("You have shown great wisdom and honor. Accept this chest of treasures as a symbol of your accomplishment.");
                    from.AddToBackpack(new SpecialWoodenChestTomoe()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("accomplishment"))
            {
                Say("An accomplishment is a testament to one's dedication and effort. May your path be honored.");
            }
            else if (speech.Contains("path"))
            {
                Say("May the path you choose be guided by wisdom and virtue.");
            }

            base.OnSpeech(e);
        }

        public TomoShigenori(Serial serial) : base(serial) { }

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
