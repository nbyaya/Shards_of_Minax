using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyra the Elf")]
    public class LyraTheElf : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyraTheElf() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyra the Elf";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 100;
            Int = 60;
            Hits = 50;

            // Appearance
            AddItem(new LeatherSkirt() { Hue = 1195 });
            AddItem(new LeatherBustierArms() { Hue = 1195 });
            AddItem(new LeatherCap() { Hue = 1195 });
            AddItem(new Boots() { Hue = 1195 });
            AddItem(new BambooFlute() { Name = "Lyra's Flute" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
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
                Say("I am Lyra the Elf, a bard with a song in my heart.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to spread tales and melodies throughout the land.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Life's grandest battles are often fought with words and melodies. Do you appreciate the power of art?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then you understand the true magic of the world, where every word and note can shape destiny.");
            }
            else if (speech.Contains("elf"))
            {
                Say("Ah, elves! We have an affinity with nature and magic. Our age-old tales are sung from the ancient woods to the shimmering shores.");
            }
            else if (speech.Contains("good"))
            {
                Say("Yes, nature's melody keeps my spirit alive and well. The harmony of the forest and the song of the wind refresh my soul.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Every tale I sing has its roots in truth. Some are of heroes long past, while others are of the mysterious enchantments that fill our world.");
            }
            else if (speech.Contains("art"))
            {
                Say("Art is the mirror of our soul, reflecting both our deepest fears and highest hopes. I once met a painter who could capture the essence of a moment in a single stroke.");
            }
            else if (speech.Contains("mirror"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Indeed, the mirror holds many secrets. Here, I have a special looking glass for you, a gift for those who truly understand the essence of art.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LyraTheElf(Serial serial) : base(serial) { }

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
