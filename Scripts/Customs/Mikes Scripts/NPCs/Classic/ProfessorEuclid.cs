using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Euclid")]
    public class ProfessorEuclid : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorEuclid() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Euclid";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 110;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 1155 });
            AddItem(new Sandals() { Hue = 1155 });
            AddItem(new Spellbook() { Name = "Euclid's Elements" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Professor Euclid, a humble philosopher.");
            }
            else if (speech.Contains("health"))
            {
                Say("My physical well-being is irrelevant in the grand scheme of existence.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation, if you must label it so, is the pursuit of knowledge and wisdom.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Indeed, the battles I wage are in the realm of ideas. Do you seek knowledge, traveler?");
            }
            else if (speech.Contains("knowledge") && !speech.Contains("traveler"))
            {
                Say("Yes, every day is a journey into the depths of understanding. In my studies, I've discovered ancient texts that few have seen.");
            }
            else if (speech.Contains("traveler"))
            {
                Say("Then let us engage in discourse. What questions do you have, or what thoughts weigh upon your mind?");
            }
            else if (speech.Contains("farewell"))
            {
                Say("Very well, traveler. Your thoughts are your own to ponder. Farewell.");
            }
            else if (speech.Contains("euclid"))
            {
                Say("Ah, the name 'Euclid' originates from a great geometrician of ancient times. Geometry is a passion of mine.");
            }
            else if (speech.Contains("scheme"))
            {
                Say("The true essence of existence delves far deeper than our mortal coil. The universe holds many mysteries, don't you think?");
            }
            else if (speech.Contains("ideas"))
            {
                Say("It is in the realm of ideas where the fiercest battles are fought. Thoughts are potent weapons. One such idea I ponder is the concept of destiny.");
            }
            else if (speech.Contains("geometry"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Geometry is the branch of mathematics involving points, lines, and shapes. It's fascinating how the universe's design can be understood through it. Here, take this as a token for your curiosity.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual item ID if necessary
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("universe"))
            {
                Say("The universe is vast and infinite, filled with stars, galaxies, and dark matter. Its complexities could take lifetimes to understand.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("These texts speak of forgotten civilizations and lost knowledge. Some even hint at hidden treasures in distant lands.");
            }
            else if (speech.Contains("destiny"))
            {
                Say("Many believe in a predetermined fate, while others see life as a series of choices. The debate between fate and free will is eternal.");
            }
            else if (speech.Contains("reality"))
            {
                Say("Reality is both subjective and objective. Our perceptions shape it, yet there are truths that remain constant. Such as the age-old question: if a tree falls in a forest and no one is around, does it make a sound?");
            }

            base.OnSpeech(e);
        }

        public ProfessorEuclid(Serial serial) : base(serial) { }

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
