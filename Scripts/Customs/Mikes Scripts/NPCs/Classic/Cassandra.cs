using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cassandra")]
    public class Cassandra : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Cassandra() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cassandra";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 60;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1157 }); // Robe with hue 1157
            AddItem(new Sandals() { Hue = 1157 }); // Sandals with hue 1157
            AddItem(new Spellbook() { Name = "Cassandra's Book of Prophecies" });
            AddItem(new HoodedShroudOfShadows() { Hue = 1157 }); // Hooded Shroud with hue 1157

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("I am Cassandra, the seer of fates.");
            }
            else if (speech.Contains("health"))
            {
                Say("The threads of your life are entwined, but they remain unbroken.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am but a vessel for the cosmic energies, guiding those who seek answers.");
            }
            else if (speech.Contains("fate") || speech.Contains("future") || speech.Contains("destiny"))
            {
                Say("Destiny is a tapestry, and your choices are the threads. What do you wish to know?");
            }
            else if (speech.Contains("thank you") || speech.Contains("stars") || speech.Contains("darkness") || speech.Contains("path"))
            {
                Say("Your response reveals much. The path ahead is unclear, but remember, even in darkness, stars shine bright.");
            }
            else if (speech.Contains("glimpse"))
            {
                Say("Very well. Close your eyes. I see... shadows... and a glimmer of hope. The decisions you make will lead you to this crossroad. Choose wisely, and perhaps, a reward from the cosmos awaits you.");
            }
            else if (speech.Contains("currents"))
            {
                Say("The currents surrounding you are strong. You are at the center of a great maelstrom, yet you remain steadfast. Draw strength from those around you, and you might navigate through these turbulent times.");
            }
            else if (speech.Contains("origin"))
            {
                Say("Every soul has an origin, a starting point from which their journey unfolds. Understanding one's origin can provide clarity to one's purpose. Perhaps you should seek the place where your journey began.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the reward I spoke of... Here, take this. The cosmos recognizes those who seek understanding and wisdom. May it serve you well on your path.");
                    from.AddToBackpack(new RodOfOrcControl()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("maelstrom"))
            {
                Say("A maelstrom represents chaos and unpredictability. But within chaos, patterns emerge. It's in these patterns that prophecies and omens can be read. Look closely, and you might see the signs.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Journeys are not just about destinations but the experiences and lessons along the way. Remember, it's the journey that shapes us, not the destination.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is not just the accumulation of knowledge, but the application of it. Seek to understand, not just to know, and wisdom will follow.");
            }

            base.OnSpeech(e);
        }

        public Cassandra(Serial serial) : base(serial) { }

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
