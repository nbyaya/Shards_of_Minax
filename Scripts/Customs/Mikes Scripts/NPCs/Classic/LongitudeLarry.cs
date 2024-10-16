using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Longitude Larry")]
    public class LongitudeLarry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LongitudeLarry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Longitude Larry";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 55;
            Int = 115;
            Hits = 75;

            // Appearance
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Tunic() { Hue = 1103 });
            AddItem(new Shoes() { Hue = 45 });

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
                Say("I am Longitude Larry, the world's most renowned cartographer. But what do you care?");
            }
            else if (speech.Contains("health"))
            {
                Say("As if my health concerns you! I have endured hardships you wouldn't understand.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm a cartographer, mapping this wretched world for fools like you who can't find their way!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Oh, you think you're valiant, do you? Let's see if you can even comprehend what true valor means.");
            }
            else if (speech.Contains("yes") && HasPreviousSpeech(30))
            {
                Say("Valor, you say? Well, prove it! When you're lost in the wilderness, will you flee like a coward or find your way like a true adventurer?");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("Ah, so you've heard of cartography! It's the art and science of drawing maps. But it's not just scribbles on parchment; it's about understanding the world! Have you ever used a map I've drawn?");
            }
            else if (speech.Contains("hardships"))
            {
                Say("Ha! I've traversed treacherous terrains, crossed mountains, and braved deserts. All to create the perfect map! You think it's easy? Have you faced any battles yourself?");
            }
            else if (speech.Contains("mapping"))
            {
                Say("Mapping isn't just about charting unknown lands. It's about giving adventurers a beacon of hope, a guide through darkness. But I doubt you understand the intricacies of my work. Ever tried your hand at mapmaking?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor isn't about showcasing strength. It's about pushing on even when the path is unclear. I've seen many who've claimed to be brave, but few who truly are. Show me an artifact of true bravery, and perhaps you'll earn a reward.");
            }
            else if (speech.Contains("adventurer"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the spirit of an adventurer! Always seeking, always exploring. If you're truly an adventurer, you'll appreciate the gift of a map. Here, take this. It might help you on your journey.");
                    from.AddToBackpack(new FocusAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        private bool HasPreviousSpeech(int entryNumber)
        {
            // This is a placeholder method for determining if a previous speech entry was triggered.
            // Implement logic based on your specific requirements or data storage.
            return true;
        }

        public LongitudeLarry(Serial serial) : base(serial) { }

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
