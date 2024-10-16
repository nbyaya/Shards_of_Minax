using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Carmen the Crimson")]
    public class CarmenTheCrimson : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CarmenTheCrimson() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Carmen the Crimson";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 50;
            Hits = 75;

            // Appearance
            AddItem(new FancyDress() { Hue = 1156 }); // Clothing item with hue 1156
            AddItem(new GoldNecklace()); // Example item
            AddItem(new Boots() { Hue = 1174 }); // Boots with hue 1174
            AddItem(new Dagger() { Name = "Carmen's Blade" });

            // Hair and facial features
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = 0x2041; // Specific facial hair item ID
            FacialHairHue = 38;


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
                Say("I'm Carmen the Crimson, the sharpest blade in this wretched town.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Who cares about that? You look fine to me.");
            }
            else if (speech.Contains("job"))
            {
                Say("Job, you say? I relieve folks of their burdens, for a fee.");
            }
            else if (speech.Contains("guts"))
            {
                Say("True valor is a myth in this town, my friend. Tell me, do you have the guts to survive here?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Guts alone won't get you far. You need cunning, like me. Got it?");
            }
            else if (speech.Contains("burdens"))
            {
                Say("Yes, heavy pockets weigh one down. I help lighten the load by taking... 'donations'.");
            }
            else if (speech.Contains("donations"))
            {
                Say("Gold, gems, trinkets... people give willingly when they see my blade. Curious about my collection?");
            }
            else if (speech.Contains("collection"))
            {
                Say("Over the years, I've amassed quite a hoard. Say, I could use someone like you to fetch a particular item. Interested?");
            }
            else if (speech.Contains("interested"))
            {
                Say("There's a pendant, once owned by a powerful mage. It's rumored to be in the catacombs. Retrieve it, and I might reward you handsomely.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Ah, always eager for a prize? Very well. Bring me the pendant and not only will I give you gold, but also a trinket from my own stash.");
            }
            else if (speech.Contains("blade"))
            {
                Say("Not just any blade, a blade bathed in the blood of a dragon. It gives me an edge in my line of work. Ever seen dragon's blood?");
            }
            else if (speech.Contains("blood"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("It's a rare sight. The crimson hue, the fiery essence. But I know where one can find some. Perhaps that's a tale for another time. Here, take this.");
                    from.AddToBackpack(new ArcheryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public CarmenTheCrimson(Serial serial) : base(serial) { }

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
