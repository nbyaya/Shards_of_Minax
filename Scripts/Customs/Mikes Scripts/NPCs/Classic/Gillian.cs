using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gillian")]
    public class Gillian : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Gillian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gillian";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 70;
            Int = 90;
            Hits = 50;

            // Appearance
            AddItem(new Skirt() { Hue = 1116 });
            AddItem(new FancyShirt() { Hue = 1114 });
            AddItem(new Boots() { Hue = 1157 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, traveler. I am Gillian, a purveyor of mystical knowledge and rare artifacts.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks to my knowledge of mystical remedies.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to uncover ancient relics and share my wisdom with those who seek it.");
            }
            else if (speech.Contains("artifacts") || speech.Contains("power"))
            {
                Say("Mystical artifacts often hold the key to unlocking one's true potential. Do you seek such power?");
            }
            else if (speech.Contains("wisdom") || speech.Contains("compassion"))
            {
                Say("Indeed, mystical power can be a double-edged sword. One must use it wisely and with compassion.");
            }

            base.OnSpeech(e);
        }

        public Gillian(Serial serial) : base(serial) { }

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
