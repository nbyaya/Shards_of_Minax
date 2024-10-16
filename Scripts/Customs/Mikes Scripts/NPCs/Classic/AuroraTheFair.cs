using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Aurora the Fair")]
    public class AuroraTheFair : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AuroraTheFair() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Aurora the Fair";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 75;
            Int = 75;
            Hits = 75;

            // Appearance
            AddItem(new FancyDress(1153)); // Clothing item with hue 1153
            AddItem(new Sandals(1153)); // Sandals with hue 1153
            AddItem(new FeatheredHat(1153)); // Feathered hat with hue 1153
            AddItem(new HeavyCrossbow() { Name = "Aurora's Heavy Crossbow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Aurora the Fair, a humble archer.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is good, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect the virtue of Valor.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor is one of the eight virtues. It represents courage and determination in the face of adversity.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you have the courage to uphold the virtue of Valor?");
            }
            else if (speech.Contains("archer"))
            {
                Say("Yes, I've trained with the bow for many years. It takes patience and precision to master. Being an archer also taught me much about humility.");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is another of the eight virtues. While I protect the virtue of Valor, I have great respect for Humility. It is the opposite of pride and teaches us to be grounded. Did you know, the first syllable of the mantra of Humility is LUM?");
            }
            else if (speech.Contains("lum"))
            {
                Say("Ah, you've been paying attention. LUM is a powerful syllable, and meditating upon it can bring you closer to understanding true humility.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("There are eight virtues in total: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each has its own importance in shaping one's character.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity tests our limits and shows us our true self. It's through challenges that we grow and learn who we truly are.");
            }
            else if (speech.Contains("protect"))
            {
                Say("To protect the virtue of Valor means not only defending it in battles but also upholding its principles in everyday life. It means showing courage even when it's difficult.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage isn't the absence of fear, but the ability to act despite it. It's a quality I hold dear and strive to embody every day.");
            }

            base.OnSpeech(e);
        }

        public AuroraTheFair(Serial serial) : base(serial) { }

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
