using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marco the Navigator")]
    public class MarcoTheNavigator : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarcoTheNavigator() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marco the Navigator";
            Body = 0x190; // Human male body

            // Stats
            Str = 50;
            Dex = 50;
            Int = 70;
            Hits = 70;

            // Appearance
            AddItem(new LongPants() { Hue = 2126 });
            AddItem(new Doublet() { Hue = 1904 });
            AddItem(new ThighBoots() { Hue = 1904 });
            AddItem(new TricorneHat() { Hue = 2126 });
            AddItem(new Cloak() { Name = "Marco's Cloak" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler! I am Marco the Navigator, a cartographer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job as a cartographer is to map the lands and seas, guiding adventurers like yourself.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("The art of cartography is a blend of precision and creativity. It reflects the virtue of humility.");
            }
            else if (speech.Contains("ponder"))
            {
                Say("Do you ponder the virtues, traveler? Which one resonates most with your heart?");
            }
            else if (speech.Contains("marco"))
            {
                Say("Ah, you've heard of me! I've traveled far and wide, mapping the uncharted and learning tales of ancient places.");
            }
            else if (speech.Contains("thank"))
            {
                Say("Your kindness warms my heart. It's travelers like you that make my journeys worthwhile.");
            }
            else if (speech.Contains("map"))
            {
                Say("Yes, maps! They are essential for adventurers. I recently found an old map leading to a hidden treasure. Would you like to have it?");
            }
            else if (speech.Contains("uncharted"))
            {
                Say("The uncharted regions often hold the greatest mysteries. I've faced many dangers to plot them on a map, but the thrill of discovery is unmatched.");
            }
            else if (speech.Contains("journeys"))
            {
                Say("My journeys have taken me to the highest peaks and the deepest caverns. The world is vast, and there's so much more to explore.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("There are tales of cities submerged underwater and ancient civilizations lost in time. Solving these mysteries is a challenge, but the truth awaits the persistent.");
            }
            else if (speech.Contains("explore"))
            {
                Say("Exploration is not just about discovering new lands. It's also about understanding the cultures, languages, and histories of the places we visit.");
            }
            else if (speech.Contains("risk"))
            {
                Say("Risks are a part of every journey. But with preparation, knowledge, and a bit of luck, one can navigate through the most treacherous paths.");
            }
            else if (speech.Contains("civilizations"))
            {
                Say("Lost civilizations hold clues to our past. By studying their remains, we can piece together stories of their greatness and downfall.");
            }
            else if (speech.Contains("cultures"))
            {
                Say("Each culture has its unique traditions, arts, and values. It's a privilege to learn from them and share their stories with others.");
            }
            else if (speech.Contains("preparation"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Proper preparation involves gathering the right tools, learning about the region, and always having a backup plan. It has saved me more than once! Here take this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MarcoTheNavigator(Serial serial) : base(serial) { }

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
