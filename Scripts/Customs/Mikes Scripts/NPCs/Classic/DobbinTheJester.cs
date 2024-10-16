using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dobbin the Jester")]
    public class DobbinTheJester : BaseCreature
    {
        private DateTime lastJokeTime;

        [Constructable]
        public DobbinTheJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dobbin the Jester";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 75;
            Int = 75;
            Hits = 75;

            // Appearance
            AddItem(new ShortPants() { Hue = 1122 });
            AddItem(new FancyShirt() { Hue = 1123 });
            AddItem(new JesterHat() { Hue = 1121 });
            AddItem(new Boots() { Hue = 1124 });
            AddItem(new FeatheredHat() { Hue = 1120 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastJokeTime = DateTime.MinValue; // Initialize last joke time
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Dobbin, the court's jester!");
            }
            else if (speech.Contains("health"))
            {
                Say("Fit as a fiddle and ready for a riddle!");
            }
            else if (speech.Contains("job"))
            {
                Say("I jest and entertain the court!");
            }
            else if (speech.Contains("jokes"))
            {
                Say("Why did the knight refuse to fight the dragon? Because it was a 'drag' on his time! Haha! Want to hear another?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Of course! Why did the mage stay calm during the spell test? He knew how to keep his cool!");
            }
            else if (speech.Contains("no"))
            {
                Say("Then you're missing out on life's little jests!");
            }

            base.OnSpeech(e);
        }

        public DobbinTheJester(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastJokeTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastJokeTime = reader.ReadDateTime();
        }
    }
}
