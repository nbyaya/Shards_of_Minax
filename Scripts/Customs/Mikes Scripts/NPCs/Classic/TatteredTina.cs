using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tattered Tina")]
    public class TatteredTina : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TatteredTina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tattered Tina";
            Body = 0x191; // Human female body

            // Stats
            Str = 30;
            Dex = 31;
            Int = 22;
            Hits = 38;

            // Appearance
            // Removing default items and equipping specific items with hues
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Equipment
            AddItem(new Skirt() { Hue = 1150 });
            AddItem(new BodySash() { Hue = 48 });
            AddItem(new Boots() { Hue = 67 });

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
                Say("I am Tattered Tina, a wretch of this forsaken place.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Ha! I'm but a walking corpse.");
            }
            else if (speech.Contains("job"))
            {
                Say("Job? Ha! This is my station, begging for scraps.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think I chose this life willingly?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Well, enjoy your valor. I'll be here in the filth, forgotten and scorned.");
            }
            else if (speech.Contains("forsaken"))
            {
                Say("This land was not always forsaken. There was a time of prosperity and honor. Alas, all good things must come to an end.");
            }
            else if (speech.Contains("corpse"))
            {
                Say("Every day I am reminded of the mistakes I made, which led me to this cursed existence. A shadow of my former self.");
            }
            else if (speech.Contains("scraps"))
            {
                Say("Once, I dined in the grandest halls, wearing the finest silks. Now, I'm reduced to begging for scraps. Such is the cost of pride and folly.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Speaking of honor, have you been seeking the mantras? The third syllable of the mantra of Honor is RAH.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("In the shadows, I've learned much about the world and its hidden truths. Perhaps, one day, I'll share my knowledge.");
            }
            else if (speech.Contains("folly"))
            {
                Say("Folly has led many astray. I've seen heroes fall and villains rise, all due to the blinding effects of hubris.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("The world is not as it seems. Secrets are kept by those in power, and the common folk are often left in the dark.");
            }
            else if (speech.Contains("villains"))
            {
                Say("While many villains wear their malevolence openly, others cloak themselves in deceit. Be wary of those who seem too good to be true.");
            }

            base.OnSpeech(e);
        }

        public override void OnThink()
        {
            base.OnThink();

            // This method can be used to implement periodic behaviors or checks.
        }

        public TatteredTina(Serial serial) : base(serial) { }

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
