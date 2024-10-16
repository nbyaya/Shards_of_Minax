using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Archibald Quill")]
    public class ProfessorArchibaldQuill : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorArchibaldQuill() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Archibald Quill";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 50;
            Int = 100;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new Sandals() { Hue = 1157 });
            AddItem(new Spellbook() { Name = "Professor's Compendium" });
            AddItem(new FeatheredHat() { Hue = 1157 });

            Hue = Utility.RandomSkinHue(); // Random skin hue
            HairItemID = Utility.RandomList(0x203B, 0x2041); // Random hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x204C; // A scholarly beard

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
                Say("Ah, greetings! I am Professor Archibald Quill, an esteemed scholar of the arcane and the ancient.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the pages of an ancient tome. Thank you for inquiring.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job involves studying ancient texts and unraveling the mysteries of the magical arts.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries of magic are vast. Have you heard of the Scholar's Enlightenment chest?");
            }
            else if (speech.Contains("chest"))
            {
                Say("Indeed, the Scholar's Enlightenment chest contains treasures of immense value. It is said to reward those who seek knowledge earnestly.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to all mysteries. Are you seeking wisdom?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is not merely the accumulation of facts, but the understanding of their deeper meanings.");
            }
            else if (speech.Contains("seeking"))
            {
                Say("You seem to be on a quest for knowledge. Prove your dedication, and you shall be rewarded.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is essential for enlightenment. What is it that you seek in this quest?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest for knowledge is a noble endeavor. It requires patience and persistence.");
            }
            else if (speech.Contains("patience"))
            {
                Say("Patience is a virtue in the pursuit of wisdom. Have you considered the role of perseverance?");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the steady persistence in a course of action. It often leads to great rewards.");
            }
            else if (speech.Contains("rewards"))
            {
                Say("The true reward is often the knowledge gained along the way. But sometimes, material rewards are also given.");
            }
            else if (speech.Contains("material"))
            {
                Say("Material rewards are tokens of recognition. They symbolize the effort and dedication put forth.");
            }
            else if (speech.Contains("effort"))
            {
                Say("Effort combined with knowledge and perseverance will lead to great achievements.");
            }
            else if (speech.Contains("achievements"))
            {
                Say("Achievements are milestones in our journey. Do you know the value of the journey itself?");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey of learning is filled with challenges and triumphs. It shapes who we become.");
            }
            else if (speech.Contains("shapes"))
            {
                Say("Indeed, the experiences we gather shape our understanding. Are you ready for the ultimate challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("The ultimate challenge requires a deep understanding and patience. Prove yourself worthy.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Your worthiness is acknowledged, but patience is also a virtue. Please return later.");
                }
                else
                {
                    Say("For your perseverance and dedication to the pursuit of knowledge, I present to you the Scholar's Enlightenment chest. May it aid you in your journey.");
                    from.AddToBackpack(new ScholarEnlightenmentChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey begins with a single step. Your dedication to this journey is commendable.");
            }

            base.OnSpeech(e);
        }

        public ProfessorArchibaldQuill(Serial serial) : base(serial) { }

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
