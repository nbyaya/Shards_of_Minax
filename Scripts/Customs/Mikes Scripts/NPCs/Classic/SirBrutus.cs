using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Brutus")]
    public class SirBrutus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirBrutus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Brutus";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 60;
            Int = 20;
            Hits = 100;

            // Appearance
            AddItem(new PlateChest() { Hue = 1175 });
            AddItem(new PlateLegs() { Hue = 1175 });
            AddItem(new PlateHelm() { Hue = 1175 });
            AddItem(new PlateGloves() { Hue = 1175 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new WarAxe() { Name = "Sir Brutus’s Axe" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Sir Brutus, the silent shadow in the night.");
            }
            else if (speech.Contains("health"))
            {
                Say("My wounds are mere scratches, nothing more.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an assassin, a master of shadows and deception.");
            }
            else if (speech.Contains("conflict ideals"))
            {
                Say("Justice and compassion, conflicting ideals in my line of work. Can one truly be just while taking lives?");
            }
            else if (speech.Contains("honor shadows"))
            {
                Say("What say you, traveler? Can one find honor in shadows, or does honor require the light?");
            }
            else if (speech.Contains("shadow"))
            {
                Say("Ah, the silent shadow. It's a moniker I've earned from my ability to move unnoticed. Many have tried to trace my steps, but darkness is my ally.");
            }
            else if (speech.Contains("scratches"))
            {
                Say("Every scar on my body tells a tale. Some of victory, others of lessons learned. If you're keen on stories, just ask.");
            }
            else if (speech.Contains("assassin"))
            {
                Say("Being an assassin isn't just about the kill. It's about strategy, patience, and understanding one's prey. But not all assignments are the same.");
            }
            else if (speech.Contains("assignments"))
            {
                Say("There are some I take for gold, others for personal reasons. There was one assignment, the silent melody, that changed everything for me.");
            }
            else if (speech.Contains("silent"))
            {
                Say("It's not a person but a rare and mystical tune. The one who plays it can control minds. I was hired to retrieve its notes. If you help me find the remaining parts, I may reward you.");
            }
            else if (speech.Contains("reward"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, always the curious type, aren't you? Very well, help me and you shall receive something invaluable. Something many in my profession seek but seldom find.");
                    from.AddToBackpack(new FencingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is subjective. To some, it's vengeance. To others, it's retribution. In my world, it's about maintaining balance.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Even in my line of work, compassion is not entirely lost. It's a fleeting emotion, but it has saved lives, including my own. Here take this.");
                from.AddToBackpack(new FencingAugmentCrystal()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a personal code. Light or shadow, it's our actions that define its true meaning.");
            }

            base.OnSpeech(e);
        }

        public SirBrutus(Serial serial) : base(serial) { }

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
