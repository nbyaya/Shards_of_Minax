using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alva")]
    public class Alva : BaseCreature
    {
        [Constructable]
        public Alva() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alva";
            Body = 0x191; // Human female body
            Hue = 0x2002; // Clothing Hue

            // Stats
            Str = 90;
            Dex = 100;
            Int = 70;
            Hits = 75;

            // Appearance
            AddItem(new FancyShirt(0x2002)); // Fancy Dress with hue 2002
            AddItem(new Boots(0x2002)); // Boots with hue 2002
            AddItem(new GnarledStaff { Name = "Alva's Staff" });

            SpeechHue = 0; // Default speech hue


        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            if (Insensitive.Contains(e.Speech, "name"))
            {
                Say("I am Alva, the explorer of forgotten worlds.");
            }
            else if (Insensitive.Contains(e.Speech, "health"))
            {
                Say("My body may be frail, but my knowledge is vast.");
            }
            else if (Insensitive.Contains(e.Speech, "job"))
            {
                Say("My 'job' is uncovering secrets and treasures hidden in the sands of time.");
            }
            else if (Insensitive.Contains(e.Speech, "battles"))
            {
                Say("But what is 'valiance' in the face of ancient mysteries?");
            }
            else if (Insensitive.Contains(e.Speech, "yes"))
            {
                Say("Do you think your 'valiance' will help you uncover the secrets of Wraeclast?");
            }
            else if (Insensitive.Contains(e.Speech, "explorer"))
            {
                Say("I've journeyed through the forgotten ruins of Sarn and even the perilous depths of Vaal. My expeditions have uncovered untold stories and artifacts.");
            }
            else if (Insensitive.Contains(e.Speech, "frail"))
            {
                Say("While my body may be weak, I've fortified my mind with the knowledge and experiences from my journeys. Do you know about the Elixir of Youth I once sought?");
            }
            else if (Insensitive.Contains(e.Speech, "treasures"))
            {
                Say("Ah, treasures. They can be both a blessing and a curse. Many are the relics I've found, but the Orb of Lost Echoes is the most enigmatic of all.");
            }
            else if (Insensitive.Contains(e.Speech, "sarn"))
            {
                Say("Ah, Sarn, the ancient city filled with memories of a bygone era. I once uncovered a map there that hinted at a secret chamber deep below the city.");
            }
            else if (Insensitive.Contains(e.Speech, "elixir"))
            {
                Say("The Elixir of Youth, a legendary potion said to grant eternal youth. I once believed it was real, and while I never found the actual elixir, the journey taught me more about life than any potion ever could. For your curiosity, take this.");
                from.AddToBackpack(new MaxxiaScroll()); // Assuming PowerSurge is a valid item
            }
            else if (Insensitive.Contains(e.Speech, "orb"))
            {
                Say("The Orb of Lost Echoes is said to contain the voices and memories of those long gone. Holding it, one can hear whispers from the past. But I've only ever caught glimpses of it in ancient texts.");
            }
            else if (Insensitive.Contains(e.Speech, "map"))
            {
                Say("The map was old, worn, and barely legible. But it spoke of a chamber where time stood still. I've been meaning to investigate, but dangers lurk in the shadows of Sarn.");
            }

            base.OnSpeech(e);
        }

        public Alva(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
