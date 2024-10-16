using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Khaled Mirage")]
    public class KhaledMirage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KhaledMirage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Khaled Mirage";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new LeatherCap() { Hue = 1153 });
            AddItem(new Tunic() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new GoldNecklace() { Hue = 1153 });

            // Hair
            HairItemID = 0x203B; // Dark hair
            HairHue = 1153;

            // Facial Hair
            FacialHairItemID = 0x2044; // Short beard
            FacialHairHue = 1153;

            // Speech Hue
            SpeechHue = 1153;

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
                Say("Greetings, adventurer. I am Khaled Mirage, keeper of hidden wonders.");
            }
            else if (speech.Contains("health"))
            {
                Say("Ah, my health is as vibrant as a desert sunrise. Thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I wander the sands, guarding secrets and uncovering treasures lost to time.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like the desert sands—vast and shifting. What more do you wish to know?");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures can be found where you least expect them, like this very chest.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, the Mirage Chest! It holds many wonders. Solve the riddles to unlock its secrets.");
            }
            else if (speech.Contains("riddles"))
            {
                Say("To understand the riddles, one must first understand the desert. Speak to me again with more knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the true key to unlocking the mysteries of the Mirage Chest. Seek wisdom and it shall be revealed.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is gained through experience and learning. Tell me more about what you seek.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience is like the desert wind—ever-present and shaping the sands. What have you learned from your journey?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey is a tale of discovery. Share with me what you have uncovered.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery often leads to greater understanding. What has been your greatest find?");
            }
            else if (speech.Contains("find"))
            {
                Say("A great find often reveals deeper truths. Have you found any truths in your travels?");
            }
            else if (speech.Contains("truths"))
            {
                Say("Truths are like the stars—guiding and constant. What truths have guided you?");
            }
            else if (speech.Contains("stars"))
            {
                Say("The stars are ancient and wise. They have seen many journeys and secrets. Do you follow their guidance?");
            }
            else if (speech.Contains("guidance"))
            {
                Say("Guidance can lead to great rewards. Reflect on what you seek, and share it with me.");
            }
            else if (speech.Contains("rewards"))
            {
                Say("Rewards are given to those who prove their worth. Have you proven yourself worthy?");
            }
            else if (speech.Contains("worthy"))
            {
                Say("Worthy is the one who perseveres and learns. Prove your worth and you may receive a great reward.");
            }
            else if (speech.Contains("perseveres"))
            {
                Say("Perseverance is key to unlocking many doors. Tell me, what challenges have you overcome?");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges are the trials that shape us. Overcoming them reveals our true strength. What has your strength achieved?");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength comes from within and is tested by trials. Share with me how your strength has been tested.");
            }
            else if (speech.Contains("tested"))
            {
                Say("Testing one's abilities is a path to greatness. If you have endured such tests, you are closer to your goal.");
            }
            else if (speech.Contains("goal"))
            {
                Say("A goal is a destination we strive for. Tell me, what is your ultimate goal?");
            }
            else if (speech.Contains("ultimate"))
            {
                Say("The ultimate goal is a reflection of one's deepest desires. If you have reached it, a reward awaits.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Return once you have reached your ultimate goal.");
                }
                else
                {
                    Say("Congratulations on reaching your ultimate goal. As a token of your journey, accept this Mirage Chest.");
                    from.AddToBackpack(new MirageChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public KhaledMirage(Serial serial) : base(serial) { }

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
