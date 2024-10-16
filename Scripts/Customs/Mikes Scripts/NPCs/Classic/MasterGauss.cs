using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Master Gauss")]
    public class MasterGauss : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MasterGauss() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Master Gauss";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 105;
            Hits = 70;

            // Appearance
            AddItem(new ShortPants() { Hue = 1156 });
            AddItem(new FancyShirt() { Hue = 1156 });
            AddItem(new Shoes() { Hue = 1156 });
            AddItem(new Spellbook() { Name = "Gaussian Distributions" });

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
                Say("Greetings, seeker of wisdom. I am Master Gauss, a humble philosopher.");
            }
            else if (speech.Contains("health"))
            {
                Say("My physical health is of little consequence, for I seek to nourish the mind and spirit.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation, if it can be called such, is to ponder the mysteries of existence and the virtues that shape our world.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Do you seek wisdom, young one? Can you fathom the depths of the virtues that guide us: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility?");
            }
            else if (speech.Contains("journey"))
            {
                Say("What say you, seeker? Are you prepared to embark on a journey of self-discovery and enlightenment?");
            }
            else if (speech.Contains("gauss"))
            {
                Say("I hail from a long lineage of philosophers, each named 'Gauss'. Our duty has always been to seek out and preserve knowledge.");
            }
            else if (speech.Contains("mind"))
            {
                Say("The mind, when cultivated, can be the most resilient of all, outlasting the physical constraints of the body. It is through the mind that I connect to the spiritual realm and draw wisdom.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries of life are vast and intertwined. One particular riddle I've been contemplating is the essence of the mantra of Sacrifice. I've recently discovered that its third syllable is 'FOD'.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Sacrifice is the act of giving up something valued for the sake of something else deemed to be of greater importance or worth. It requires selflessness and understanding. Reflect upon this virtue and you may uncover more than you realize.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("The path of self-discovery is not a straightforward one. It is filled with trials, reflections, and moments of insight. But remember, the journey itself can be as enlightening as the destination.");
            }
            else if (speech.Contains("lineage"))
            {
                Say("Our family's texts span generations, filled with thoughts, debates, and philosophies. They serve as a beacon, guiding those who seek understanding.");
            }
            else if (speech.Contains("spiritual"))
            {
                Say("The spiritual realm is a vast expanse where time, space, and thought converge. It is there that I converse with the great minds of the past, seeking answers to the questions of the present.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Mantras are sacred utterances believed to possess spiritual efficacy. They're often repeated, serving as a focus for meditation and reflection. The mantra of Sacrifice, in particular, holds great power and significance.");
            }
            else if (speech.Contains("selflessness"))
            {
                Say("To be selfless is to put the needs of others before your own, understanding that the whole is greater than the sum of its parts. It is a virtue that few truly master but all should strive for.");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials are the challenges we face in life, both external and internal. They shape our character, testing our resolve and our understanding of the virtues.");
            }

            base.OnSpeech(e);
        }

        public MasterGauss(Serial serial) : base(serial) { }

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
