using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Miss Pythagoras")]
    public class MissPythagoras : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MissPythagoras() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Miss Pythagoras";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 70;
            Int = 105;
            Hits = 65;

            // Appearance
            AddItem(new Kilt() { Hue = 1154 });
            AddItem(new Tunic() { Hue = 1154 });
            AddItem(new Shoes() { Hue = 1154 });
            AddItem(new Spellbook() { Name = "Pythagoras Theorems" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Miss Pythagoras, a philosopher of the land.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is but a fleeting measure of my existence, as is yours.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation is that of contemplation and the pursuit of knowledge.");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("humility"))
                {
                    Say("The eight virtues are the pillars upon which a virtuous life is built: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
                }
                else
                {
                    Say("Do you ponder the virtues, traveler? For they are the essence of our existence.");
                }
            }
            else if (speech.Contains("pythagoras"))
            {
                Say("I am named after the great Pythagoras, known for his theorem. Have you studied geometry, traveler?");
            }
            else if (speech.Contains("fleeting"))
            {
                Say("Indeed, life is transient. It reminds me of the sands in an hourglass, ever flowing and inevitable.");
            }
            else if (speech.Contains("contemplation"))
            {
                Say("To contemplate is to dive deep into the abyss of thought, searching for answers to the universe's most perplexing questions. Have you ever wondered about the stars above?");
            }
            else if (speech.Contains("hourglass"))
            {
                Say("An hourglass marks the passage of time, its grains representing the fleeting moments of our lives. But even as it runs out, it can be turned and begun anew. Life, too, offers new beginnings. Do you believe in second chances?");
            }
            else if (speech.Contains("stars"))
            {
                Say("The stars are not just points of light in the night sky. They are guides, stories, and a reflection of our dreams. Some even say there's a constellation that guides one's fate. Do you know your constellation?");
            }
            else if (speech.Contains("honor"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Honor is not merely a virtue but a way of life. For one who truly embodies honor, the path they walk shines brightly. If you can show me an act of honor, I might have a reward for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("geometry"))
            {
                Say("Geometry, the study of shapes and sizes, is foundational to understanding the world around us. The ancient architects used its principles to craft marvels. Have you visited any ancient ruins?");
            }

            base.OnSpeech(e);
        }

        public MissPythagoras(Serial serial) : base(serial) { }

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
