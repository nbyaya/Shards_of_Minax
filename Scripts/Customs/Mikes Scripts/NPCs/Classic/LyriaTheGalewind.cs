using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria the Galewind")]
    public class LyriaTheGalewind : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyriaTheGalewind() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria the Galewind";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 140;
            Int = 70;
            Hits = 85;

            // Appearance
            AddItem(new ElvenBoots() { Hue = 1182 });
            AddItem(new ElvenShirt() { Hue = 1181 });
            AddItem(new Bow() { Name = "Lyria's Windbow" });

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
                Say("I am Lyria the Galewind, born of the winds and skies.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am untouched by the ailments of mortals.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to soar the skies and keep watch over the realms.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Like the winds, valor is ever-changing, yet constant.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you understand the ever-shifting nature of valor?");
            }
            else if (speech.Contains("winds"))
            {
                Say("The winds whisper secrets to those who listen. One of them spoke of a mantra where its second syllable is VEX.");
            }
            else if (speech.Contains("ailments"))
            {
                Say("Many mortals seek cures for their ailments in the ancient texts and arcane rituals.");
            }
            else if (speech.Contains("realms"))
            {
                Say("Each realm holds its own challenges and treasures. I've seen the birth and demise of many kingdoms from above.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is like the northern star, guiding the lost and brave. In my flights, I've witnessed countless acts of valor.");
            }
            else if (speech.Contains("vex"))
            {
                Say("VEX is said to be the second syllable of the mantra of Justice. It's a word of power and importance.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("These texts speak of prophecies and legends, including the tales of the Eight Virtues.");
            }
            else if (speech.Contains("demise"))
            {
                Say("Kingdoms rise and fall, but the virtues remain timeless, serving as a beacon for those in darkness.");
            }
            else if (speech.Contains("guide"))
            {
                Say("As the northern star guides sailors, I guide those lost in the vast expanse of the sky.");
            }
            else if (speech.Contains("power"))
            {
                Say("Many seek to wield words of power, but true strength lies in understanding their essence.");
            }
            else if (speech.Contains("prophecies"))
            {
                Say("Prophecies are a double-edged sword. They can guide or mislead based on one's interpretation.");
            }

            base.OnSpeech(e);
        }

        public LyriaTheGalewind(Serial serial) : base(serial) { }

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
