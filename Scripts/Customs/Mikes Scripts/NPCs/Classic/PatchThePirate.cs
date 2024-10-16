using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Patch the Pirate")]
    public class PatchThePirate : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PatchThePirate() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Patch the Pirate";
            Body = 0x190; // Human male body

            // Stats
            Str = 45;
            Dex = 40;
            Int = 25;
            Hits = 45;

            // Appearance
            AddItem(new LongPants() { Hue = 1904 });
            AddItem(new FancyShirt() { Hue = 1904 });
            AddItem(new Boots() { Hue = 1904 });
            AddItem(new TricorneHat() { Hue = 1904 });
            AddItem(new Cloak() { Name = "Patch's Cloak" }); // EyePatch

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
                Say("Ahoy there, I be Patch the Pirate, scourge of the seven seas!");
            }
            else if (speech.Contains("health"))
            {
                Say("I've taken a few scratches in me day, but I'm still sailin' strong!");
            }
            else if (speech.Contains("job"))
            {
                Say("I be a pirate through and through, always on the hunt for buried treasure!");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor be found in the heart of a pirate, for we face danger with a grin!");
            }
            else if (speech.Contains("yes"))
            {
                Say("Arrr! Would ye be brave enough to sail the high seas with me, matey?");
            }

            base.OnSpeech(e);
        }

        public PatchThePirate(Serial serial) : base(serial) { }

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
