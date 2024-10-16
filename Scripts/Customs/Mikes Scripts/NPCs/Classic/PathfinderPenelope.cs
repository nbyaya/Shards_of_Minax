using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pathfinder Penelope")]
    public class PathfinderPenelope : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PathfinderPenelope() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pathfinder Penelope";
            Body = 0x191; // Human female body

            // Stats
            Str = 103;
            Dex = 54;
            Int = 119;
            Hits = 71;

            // Appearance
            AddItem(new FancyDress() { Hue = 44 }); // Clothing item with hue 44
            AddItem(new Boots() { Hue = 1122 }); // Boots with hue 1122

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
                Say("Greetings, traveler. I am Pathfinder Penelope, the cartographer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a cartographer, dedicated to mapping the lands and uncovering their secrets.");
            }
            else if (speech.Contains("maps") || speech.Contains("exploration"))
            {
                Say("Maps reveal the world's beauty, but also its mysteries. Do you value exploration?");
            }
            else if (speech.Contains("yes") || speech.Contains("exploration") || speech.Contains("enlightenment"))
            {
                Say("Exploration is the path to enlightenment. Carry on your journey with an open heart and a keen eye, traveler.");
            }
            else if (speech.Contains("pathfinder"))
            {
                Say("Ah, yes. The title 'Pathfinder' was given to me after I discovered a hidden path through the treacherous Blackwood Forest. It was a title of honor and respect.");
            }
            else if (speech.Contains("good"))
            {
                Say("My health has been steady, thanks to the various herbs I come across during my expeditions. Nature has a remedy for most ailments if you know where to look.");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("Being a cartographer is not just about drawing maps. It's about capturing the essence of the land, its dangers, and its treasures. I've come across many fascinating places in my travels.");
            }
            else if (speech.Contains("maps"))
            {
                Say("I've charted many regions, from the icy tundras of the North to the fiery volcanoes of the South. Each map tells a story of its own.");
            }
            else if (speech.Contains("exploration"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("There is no greater thrill than uncovering a place untouched by others. Exploration offers rewards, both tangible and intangible. Please return later for your reward.");
                }
                else
                {
                    Say("There is no greater thrill than uncovering a place untouched by others. Exploration offers rewards, both tangible and intangible. Speaking of which, here's a little something for your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("True enlightenment comes when one understands the world around them. The lands, the creatures, and the history. It's a continuous journey of learning.");
            }

            base.OnSpeech(e);
        }

        public PathfinderPenelope(Serial serial) : base(serial) { }

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
