using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyric Lila")]
    public class LyricLila : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyricLila() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyric Lila";
            Body = 0x191; // Human female body

            // Stats
            Str = 118;
            Dex = 68;
            Int = 84;
            Hits = 87;

            // Appearance
            AddItem(new FancyDress() { Hue = 54 }); // Clothing item with hue 54
            AddItem(new Shoes() { Hue = 66 }); // Shoes with hue 66
            AddItem(new LeatherGloves() { Name = "Lila's Lyric Gloves" });

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
                Say("Greetings, traveler! I am Lyric Lila, the bard.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good spirits, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession is to weave tales and sing songs that touch the soul. I am a bard.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Ah, the eight virtues, they are the very essence of a noble heart. Which virtue doth thou seek insight into?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty, the foundation of trust. To be honest is to be true to oneself and others.");
            }
            else if (speech.Contains("lyric"))
            {
                Say("Yes, that's my name. A family title that has been passed down through generations, a symbol of our love for music and words.");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Being a bard isn't just about music. It's about capturing the essence of stories, emotions, and passing them on. It's a legacy of our ancestors.");
            }
            else if (speech.Contains("virtues") && speech.Contains("walk"))
            {
                Say("The virtues guide us in our journey through life. They are essential for every true hero. To know them is to walk a righteous path.");
            }
            else if (speech.Contains("music"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Music is the universal language. It transcends borders and speaks to the soul. For your genuine interest, let me give you this special lute as a gift.");
                    from.AddToBackpack(new SpellweavingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tales"))
            {
                Say("Tales are a bridge to our past. They teach us lessons and remind us of values long forgotten. A wise traveler always seeks stories as much as treasure.");
            }
            else if (speech.Contains("ancestors"))
            {
                Say("My ancestors were renowned bards, traveling the world and gathering tales. Their songs are still remembered and cherished.");
            }
            else if (speech.Contains("hero"))
            {
                Say("Heroes aren't just those who wield swords. Some sing, write, and inspire. It's the spirit that counts.");
            }
            else if (speech.Contains("lute"))
            {
                Say("Ah, you've noticed my gift. This lute is imbued with magical properties. When played with true intent, it can produce sounds that can heal wounds. Here, it's yours.");
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new SpellweavingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LyricLila(Serial serial) : base(serial) { }

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
