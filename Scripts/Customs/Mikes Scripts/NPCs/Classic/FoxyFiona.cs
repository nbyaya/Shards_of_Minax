using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Foxy Fiona")]
    public class FoxyFiona : BaseCreature
    {
        [Constructable]
        public FoxyFiona() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Foxy Fiona";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 90;

            // Appearance
            AddItem(new LeatherSkirt() { Hue = 2118 });
            AddItem(new FemaleLeatherChest() { Hue = 2118 });
            AddItem(new Boots() { Hue = 2118 });
            AddItem(new Bow() { Name = "Foxy Fiona's Bow" });

            // Equip items
            EquipItem(new LeatherSkirt() { Hue = 2118 });
            EquipItem(new LeatherBustierArms() { Hue = 2118 });
            EquipItem(new Boots() { Hue = 2118 });
            EquipItem(new Bow() { Name = "Foxy Fiona's Bow" });

            // Direction and other settings
            Direction = Direction.West;

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
                Say("I am Foxy Fiona, the animal tamer!");
            }
            else if (speech.Contains("health"))
            {
                Say("All my animals are in good health!");
            }
            else if (speech.Contains("job"))
            {
                Say("I train and care for animals!");
            }
            else if (speech.Contains("animals"))
            {
                Say("Compassion towards animals is a virtue. Do you have compassion?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Animals bring joy and teach us kindness.");
            }
            else if (speech.Contains("fiona"))
            {
                Say("Oh, you've heard of me? I've been in this town for years, taming and caring for creatures of all kinds. Even the elusive Moon Cat.");
            }
            else if (speech.Contains("elixir"))
            {
                Say("My animals are not just in good health; they are thriving. Especially since I found the Elixir of Vitality.");
            }
            else if (speech.Contains("train"))
            {
                Say("Training animals requires patience and understanding. Especially with the more mystical ones like the Whispering Phoenix.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("True compassion can lead to understanding the mantras of virtues. Have you heard of the mantra of Honesty?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Ah, the mantra of Honesty. Its power lies in understanding its syllables. The second syllable is LOR. Remember this; it might guide your path");
            }
            else if (speech.Contains("moon"))
            {
                Say("The Moon Cat is a rare creature, known to be a harbinger of lunar events. I managed to form a bond with one some years ago.");
            }
            else if (speech.Contains("phoenix"))
            {
                Say("It's a legendary bird that communicates through soft whispers. Those whispers can reveal truths to those who listen closely.");
            }

            base.OnSpeech(e);
        }

        public FoxyFiona(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
