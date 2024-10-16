using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Spectral Sarah")]
    public class SpectralSarah : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SpectralSarah() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Spectral Sarah";
            Body = 0x191; // Human female body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 100;
            Hits = 90;

            // Appearance
            AddItem(new PlainDress() { Hue = 64 });
            AddItem(new ThighBoots() { Hue = 38 });
            AddItem(new BoneGloves() { Name = "Sarah's Spirit Siphons" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, traveler. I am Spectral Sarah, a student of the darkest arts.");
            }
            else if (speech.Contains("health"))
            {
                Say("My life force is sustained by the powers of the abyss. I am beyond health and death.");
            }
            else if (speech.Contains("job"))
            {
                Say("My purpose is to unravel the secrets of life and death, to command the legions of the undead.");
            }
            else if (speech.Contains("battles"))
            {
                Say("The eight virtues, you say? Ponder them, and you shall find your path.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you seek power, traveler? To what depths are you willing to descend to attain it?");
            }
            else if (speech.Contains("abyss"))
            {
                Say("The abyss is a realm of endless darkness and torment. It is where I draw my strength and where the spirits of the forgotten reside.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("There are many secrets of the netherworld that are yet to be discovered. Some say they hold the key to unimaginable power.");
            }
            else if (speech.Contains("undead"))
            {
                Say("The undead serve me loyally, but they are also a reminder of the thin line between life and death. They are bound to this world, unable to move on.");
            }
            else if (speech.Contains("legions"))
            {
                Say("My legions are vast, composed of spirits who have been denied their eternal rest. They are my eyes and ears in the realm of the living.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The eight virtues are the foundation of a just and honorable life. Among them, humility is the one most overlooked, yet it is the root of all virtues. Remember, the second syllable of its mantra is NAY.");
            }
            else if (speech.Contains("depths"))
            {
                Say("There are depths in both the abyss and in one's soul that many fear to tread. But with great risk comes great reward.");
            }
            else if (speech.Contains("life"))
            {
                Say("Life is a fleeting moment in the grand tapestry of existence. Cherish it, for once it's gone, it may never return.");
            }
            else if (speech.Contains("death"))
            {
                Say("Death is not the end, but a transition. It's the doorway to realms beyond our comprehension.");
            }
            else if (speech.Contains("arts"))
            {
                Say("The dark arts are not for the faint of heart. They require sacrifice, dedication, and an unwavering desire to bend the natural order.");
            }

            base.OnSpeech(e);
        }

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

        public SpectralSarah(Serial serial) : base(serial) { }
    }
}
