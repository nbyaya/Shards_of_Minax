using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Antoine Lumière")]
    public class AntoineLumiere : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AntoineLumiere() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Antoine Lumière";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomNeutralHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Sandals() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Spellbook() { Name = "Book of Enlightenment" });

            // Facial features
            Hue = Race.RandomSkinHue(); 
            HairItemID = Utility.RandomList(0x204F, 0x203B, 0x203C); // Various hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = -1; // Clean-shaven

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
                Say("Greetings, I am Antoine Lumière, seeker of wisdom and bearer of light.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as radiant as the enlightenment I seek.");
            }
            else if (speech.Contains("job"))
            {
                Say("I wander these lands to share knowledge and seek out the treasures of wisdom.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Enlightenment is the pursuit of knowledge and understanding. It illuminates the path ahead.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that guides us through the darkness of ignorance. It is found in the pursuit of truth.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasures of wisdom are the greatest rewards. Seek out the secrets I guard, and a special treasure awaits you.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("To uncover secrets, one must ask the right questions and ponder deeply.");
            }
            else if (speech.Contains("ponder"))
            {
                Say("Contemplate the nature of knowledge and the pursuit of truth. The answers you seek are revealed through reflection.");
            }
            else if (speech.Contains("reveal"))
            {
                Say("The path to revelation is one of patience and understanding. Seek the essence of knowledge.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of knowledge lies in the accumulation of wisdom and the ability to apply it.");
            }
            else if (speech.Contains("accumulation"))
            {
                Say("Accumulation of wisdom requires effort and time. Are you prepared for such a journey?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Preparation is key to unlocking deeper truths. Only those who are ready can truly understand.");
            }
            else if (speech.Contains("understand"))
            {
                Say("To understand is to grasp the deeper meanings and connections between things. It requires insight and contemplation.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight comes from experience and reflection. Seek knowledge earnestly, and it will reveal itself.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is a vast ocean. Dive deep and explore its depths to uncover hidden truths.");
            }
            else if (speech.Contains("ocean"))
            {
                Say("The ocean of knowledge is boundless. To navigate it, one must have a clear purpose and direction.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose guides us through the vastness of knowledge. With purpose, even the deepest mysteries become clearer.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries are the unknown aspects of knowledge. Solving them requires dedication and persistence.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is essential for uncovering truths. Only through dedicated effort can one achieve enlightenment.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("You have demonstrated a commitment to seeking truth. As a reward for your dedication, accept this chest, which holds the culmination of wisdom.");
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new SpecialWoodenChestFrench()); // Give the special chest as the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("Return later for your reward. The path to enlightenment requires patience.");
                }
            }

            base.OnSpeech(e);
        }

        public AntoineLumiere(Serial serial) : base(serial) { }

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
