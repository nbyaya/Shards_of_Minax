using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Alexios")]
    public class EmperorAlexios : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorAlexios() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Alexios";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 80;
            Hits = 100;

            // Appearance
            AddItem(new PlateChest() { Hue = 2213 });
            AddItem(new PlateLegs() { Hue = 2213 });
            AddItem(new PlateHelm() { Hue = 2213 });
            AddItem(new PlateGloves() { Hue = 2213 });
            AddItem(new Boots() { Hue = 2213 });
            AddItem(new WarMace() { Name = "Emperor's Scepter" });

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
                Say("Greetings, traveler. I am Emperor Alexios, ruler of Byzantium.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, as befits a ruler.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to uphold the Byzantine Empire, a task of great importance.");
            }
            else if (speech.Contains("justice"))
            {
                Say("The virtue of justice guides my rule. Tell me, do you believe in the just rule of law?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your answer speaks volumes. In the realm of virtues, justice is the cornerstone of all. Strive to be just in all your actions, and you will find the path to righteousness.");
            }
            else if (speech.Contains("byzantium"))
            {
                Say("Byzantium is a city rich in history and culture. It is my honor and duty to protect and lead its people.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Indeed, robustness is essential not just for a ruler's physical well-being, but also for the mental fortitude required to lead an empire.");
            }
            else if (speech.Contains("empire"))
            {
                Say("The Byzantine Empire is a beacon of civilization, standing tall against barbaric forces. But to maintain its glory, the empire requires more than just strength; it requires spirituality.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Each virtue is a guiding light in the darkness. Speaking of virtues, have you delved into the study of mantras associated with them? For instance, the second syllable of the mantra of Spirituality is JIX.");
            }
            else if (speech.Contains("jix"))
            {
                Say("Ah, you show interest in the sacred syllables. The mantras are a means to connect with the virtues on a deeper level. Meditating on them can grant insight and strength.");
            }
            else if (speech.Contains("history"))
            {
                Say("Our history is a tapestry of valiant heroes, cunning strategists, and tales of resilience. It's our legacy that continues to inspire the current generation.");
            }
            else if (speech.Contains("fortitude"))
            {
                Say("Fortitude is the mental strength to face challenges head-on, without wavering. It's a trait admired by many but possessed by few.");
            }

            base.OnSpeech(e);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Add any special death effects here if needed
        }

        public EmperorAlexios(Serial serial) : base(serial) { }

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
