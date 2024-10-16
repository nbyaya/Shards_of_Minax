using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alien Ambassador Zark")]
    public class AlienAmbassadorZark : BaseCreature
    {
        [Constructable]
        public AlienAmbassadorZark() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alien Ambassador Zark";
            Body = 0x190; // Human male body; adjust as needed

            // Stats
            Str = 80;
            Dex = 90;
            Int = 130;
            Hits = 70;

            // Appearance
            AddItem(new Robe(2400)); // Robe with Hue 4400
            AddItem(new BlackStaff { Name = "Zark's Energy Staff" });

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            if (Insensitive.Contains(e.Speech, "name"))
            {
                Say("Greetings, I am Alien Ambassador Zark, representing the interstellar council.");
            }
            else if (Insensitive.Contains(e.Speech, "health"))
            {
                Say("I am in perfect health, as our technology can swiftly mend any ailment.");
            }
            else if (Insensitive.Contains(e.Speech, "job"))
            {
                Say("My role here is to foster diplomacy and understanding between our two worlds.");
            }
            else if (Insensitive.Contains(e.Speech, "virtues"))
            {
                Say("We, the Council, hold the virtue of Spirituality dear, seeking harmony among all beings.");
            }
            else if (Insensitive.Contains(e.Speech, "thoughts"))
            {
                Say("What are your thoughts on the virtue of Spirituality, and how it relates to your world?");
            }
            else if (Insensitive.Contains(e.Speech, "council"))
            {
                Say("The interstellar council comprises various species from distant galaxies, working together to ensure peace and prosperity in the cosmos.");
            }
            else if (Insensitive.Contains(e.Speech, "technology"))
            {
                Say("Our technology is not just for health. We've harnessed the power of stars, manipulated the fabric of space, and unlocked secrets of the universe. Would you like a demonstration?");
            }
            else if (Insensitive.Contains(e.Speech, "diplomacy"))
            {
                Say("Diplomacy is not just about communication, but understanding. We believe in learning from every civilization we encounter, combining knowledge for the betterment of all.");
            }
            else if (Insensitive.Contains(e.Speech, "harmony"))
            {
                Say("Harmony is achieved not by dominance, but by mutual respect. Every being, regardless of their origin, has value and can contribute to the larger tapestry of existence.");
            }
            else if (Insensitive.Contains(e.Speech, "world"))
            {
                Say("Your world is unique, filled with its own challenges and joys. It reminds us of a planet in the Andromeda sector. If you ever wish to visit, I can make arrangements.");
            }
            else if (Insensitive.Contains(e.Speech, "species"))
            {
                Say("There are countless species in the council, from the ethereal Aeloxians to the robust Yarnarians. We cherish the diversity, as it brings varied perspectives.");
            }
            else if (Insensitive.Contains(e.Speech, "demonstration"))
            {
                Say("Watch closely. [Ambassador Zark produces a small device, and with a press, a holographic universe unfolds before your eyes, showing the vastness of the cosmos]. This is but a fraction of what we have explored. As a token of our meeting, please take this device. May it inspire wonder in you.");
                from.AddToBackpack(new TailoringAugmentCrystal()); // Adjust item type as needed
            }
            else if (Insensitive.Contains(e.Speech, "civilization"))
            {
                Say("Each civilization we encounter holds a mirror to our own, reflecting both our strengths and areas for growth. By sharing and learning, we aim to uplift all.");
            }
            else if (Insensitive.Contains(e.Speech, "origin"))
            {
                Say("Regardless of one's origin, the universal language is kindness. We've found this truth resonates across galaxies and species.");
            }
            else if (Insensitive.Contains(e.Speech, "aeloxians"))
            {
                Say("The Aeloxians are beings of pure energy, not bound by physical form. Their wisdom has greatly influenced the council's philosophy.");
            }
            else if (Insensitive.Contains(e.Speech, "device"))
            {
                Say("The device you now hold is a portable universe map. It's a tool for exploration and understanding. Cherish it, and perhaps one day, you'll join our explorations.");
            }

            base.OnSpeech(e);
        }

        public AlienAmbassadorZark(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
