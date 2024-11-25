using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alberto Da Vinci")]
    public class AlbertoDaVinci : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AlbertoDaVinci() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alberto Da Vinci";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomBlueHue() });
            AddItem(new LongPants() { Hue = Utility.RandomGreenHue() });
            AddItem(new Shoes() { Hue = Utility.RandomNeutralHue() });
            AddItem(new StrawHat() { Name = "Renaissance Cap", Hue = Utility.RandomNondyedHue() });
            AddItem(new Cloak() { Hue = Utility.RandomBrightHue() });

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
                Say("Ah, I am Alberto Da Vinci, a humble admirer of art and collector of rare treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as vibrant as the colors on a masterful canvas, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I dedicate my days to curating and preserving the finest of Renaissance artifacts.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, you seek treasure! I have a collection that would make even the Medici envious.");
            }
            else if (speech.Contains("collectors"))
            {
                Say("Collectors like myself treasure history and art. If you're intrigued by the past, you're in the right place.");
            }
            else if (speech.Contains("art"))
            {
                Say("Art is a reflection of the soul's deepest desires. It captures moments and emotions like no other medium.");
            }
            else if (speech.Contains("masterpiece"))
            {
                Say("A masterpiece is the culmination of vision, skill, and passion. It is both a creation and a story.");
            }
            else if (speech.Contains("medici"))
            {
                Say("The Medici family were great patrons of the arts. Their support helped to fuel the Renaissance.");
            }
            else if (speech.Contains("patrons"))
            {
                Say("For your interest in the arts and treasures of the Renaissance, accept this special chest as a token of appreciation.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    from.AddToBackpack(new RenaissanceCollectorsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("treasures"))
            {
                Say("The treasures I hold are not just material, but a testament to the brilliance of past eras.");
            }
            else if (speech.Contains("brilliance"))
            {
                Say("Brilliance shines through in every artistic endeavor, from painting to sculpture, embodying the spirit of the time.");
            }
            else if (speech.Contains("sculpture"))
            {
                Say("Sculpture captures the essence of its subject in three dimensions, giving form to the intangible.");
            }
            else if (speech.Contains("dimensions"))
            {
                Say("Dimensions in art refer to the depth and perspective, adding a sense of reality and immersion to the work.");
            }
            else if (speech.Contains("perspective"))
            {
                Say("Perspective in art creates the illusion of depth, guiding the viewer's eye and enhancing the narrative.");
            }
            else if (speech.Contains("narrative"))
            {
                Say("Every artwork tells a story, whether through its subjects, style, or the emotions it evokes.");
            }
            else if (speech.Contains("emotions"))
            {
                Say("Emotions in art connect deeply with the viewer, revealing the artist's innermost thoughts and feelings.");
            }
            else if (speech.Contains("thoughts"))
            {
                Say("The thoughts of an artist are often reflected in their work, capturing their vision and insight.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight into art can reveal hidden meanings and themes, enriching our understanding of its significance.");
            }
            else if (speech.Contains("significance"))
            {
                Say("The significance of a piece lies in its ability to resonate with and inspire those who experience it.");
            }

            base.OnSpeech(e);
        }

        public AlbertoDaVinci(Serial serial) : base(serial) { }

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
