using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Empress Victoria")]
    public class EmpressVictoria : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmpressVictoria() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Empress Victoria";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new FancyDress() { Hue = 2118 });
            AddItem(new Boots() { Hue = 2118 });
            AddItem(new GoldNecklace() { Name = "Victoria's Necklace" });
            AddItem(new Cloak() { Name = "Victoria's Cloak" });
            AddItem(new Mace() { Name = "Victoria's Fan" });
			
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
                Say("Greetings, traveler. I am Empress Victoria.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am the ruler of this land, overseeing its affairs and guiding its people.");
            }
            else if (speech.Contains("justice"))
            {
                Say("The virtue of Justice is of great importance to me. It ensures fairness and equity in our society. Do you value justice?");
            }
            else if (speech.Contains("yes"))
            {
                Say("That is a noble sentiment. Justice is the foundation of a harmonious society. Upholding it is our duty.");
            }

            base.OnSpeech(e);
        }

        public EmpressVictoria(Serial serial) : base(serial) { }

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
