using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Adria")]
    public class Adria : BaseCreature
    {
        [Constructable]
        public Adria() : base(AIType.AI_Healer, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Adria";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 50;
            Int = 140;
            Hits = 60;

            // Appearance
            AddItem(new Robe(1152));
            AddItem(new Sandals(0));
            AddItem(new Spellbook() { Name = "Adria's Grimoire" });

            // Speech hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            if (Insensitive.Contains(e.Speech, "name"))
            {
                Say("I am Adria, the Sorceress of Tristram. What do you want?");
            }
            else if (Insensitive.Contains(e.Speech, "health"))
            {
                Say("My health is of no concern to you. Focus on your own affairs.");
            }
            else if (Insensitive.Contains(e.Speech, "job"))
            {
                Say("My \"job,\" as you call it, is to harness the power of magic. Something you could never understand.");
            }
            else if (Insensitive.Contains(e.Speech, "magic") || Insensitive.Contains(e.Speech, "power"))
            {
                Say("Do you truly believe you have the wit to understand the forces I command? Are you even capable of comprehending true power?");
            }
            else if (Insensitive.Contains(e.Speech, "ambition") || Insensitive.Contains(e.Speech, "power"))
            {
                Say("Ha! You are amusingly confident for a mere mortal. Tell me, what would you do with true power if you had it?");
            }
            else if (Insensitive.Contains(e.Speech, "knowledge") || Insensitive.Contains(e.Speech, "prove"))
            {
                Say("Hmph. Perhaps you are not entirely without potential. But potential alone means nothing. Prove your worth to me, and I may share some of my knowledge.");
            }

            base.OnSpeech(e);
        }

        public Adria(Serial serial) : base(serial) { }

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
