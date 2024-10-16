using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Naela the Mistweaver")]
    public class NaelaTheMistweaver : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NaelaTheMistweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Naela the Mistweaver";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 80;
            Int = 110;
            Hits = 70;

            // Appearance
            AddItem(new Cloak() { Hue = 1181 }); // Add a cloak with the specified hue
            AddItem(new SilverRing() { Name = "Naela's Band" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
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
                Say("Greetings, traveler. I am Naela the Mistweaver, a water genasi of ancient lineage.");
            }
            else if (speech.Contains("health"))
            {
                Say("My essence is attuned to the flow of water, and my health is ever fluid like the tides.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a Mistweaver, one who harnesses the power of water to mend wounds and bring clarity to the mind.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("The secrets of water are deep and unfathomable. Do you seek enlightenment, traveler?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Wisdom is a river, and one must be willing to wade through its depths to find answers.");
            }

            base.OnSpeech(e);
        }

        public NaelaTheMistweaver(Serial serial) : base(serial) { }

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
