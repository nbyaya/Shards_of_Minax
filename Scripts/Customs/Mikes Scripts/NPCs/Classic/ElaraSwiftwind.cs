using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Elara Swiftwind")]
    public class ElaraSwiftwind : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ElaraSwiftwind() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Elara Swiftwind";
            Body = 0x191; // Female human body

            // Stats
            Str = 100;
            Dex = 90;
            Int = 80;
            Hits = 100;

            // Appearance
            AddItem(new Kilt() { Hue = 1107 });
            AddItem(new BodySash() { Hue = 1123 });
            AddItem(new ElvenBoots() { Hue = 1152 });
            AddItem(new CompositeBow() { Name = "Elara's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, traveler. I am Elara Swiftwind, the archer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation is that of an archer. I excel in the art of the bow and arrow.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor in an archer lies not only in the precision of her shots but also in the wisdom of her choices. Are you wise?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Wisdom is a virtue that guides one's actions. Never underestimate the power of a well-placed arrow, and never let folly rule your choices.");
            }
            else if (speech.Contains("bow"))
            {
                Say("The bow is not just a weapon; it is an extension of oneself. It requires patience, understanding, and connection to truly master.");
            }
            else if (speech.Contains("arrow"))
            {
                Say("Each arrow carries a message. Sometimes that message is for one's enemy, but other times, it can be a whisper to the winds, revealing secrets of the world.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom is not just about knowing what's right but also acting upon it. It's a virtue I value deeply. Speaking of virtues, have you heard about the mantra of Compassion?");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Ah, virtues! They guide us, shape us, and remind us of our true path. Compassion is one such virtue. Its mantra is said to have a syllable that resonates deeply with me... 'MUH'.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is about understanding and empathy. It's about putting yourself in another's shoes and feeling their pain or joy. The world could use more of it.");
            }
            else if (speech.Contains("ponder"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new Gold(1000)); // Give a reward, example: 100 gold
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("destiny"))
            {
                Say("Destiny is like the wind, ever-shifting and unpredictable. Yet, with a keen sense of understanding and the right skills, one can navigate through its challenges.");
            }

            base.OnSpeech(e);
        }

        public ElaraSwiftwind(Serial serial) : base(serial) { }

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
