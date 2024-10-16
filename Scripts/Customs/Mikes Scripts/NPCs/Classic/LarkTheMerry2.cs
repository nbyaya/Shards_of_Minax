using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lark the Merry")]
    public class LarkTheMerry2 : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LarkTheMerry2() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lark the Merry";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 70; // Adjusted for balance

            // Appearance
            AddItem(new JesterSuit() { Hue = 1153 }); // Jester Suit
            AddItem(new JesterHat() { Hue = 1153 });  // Jester Hat
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Lark the Merry, the kingdom's jester!");
            }
            else if (speech.Contains("health"))
            {
                Say("As fit as a fiddle!");
            }
            else if (speech.Contains("job"))
            {
                Say("I jest and entertain, and sometimes share riddles!");
            }
            else if (speech.Contains("riddles"))
            {
                Say("Ah, a riddle for thee! What has keys but opens no doors?");
            }
            else if (speech.Contains("piano"))
            {
                Say("Indeed, a piano it is! Well guessed!");
            }
            else if (speech.Contains("piano") || speech.Contains("riddles"))
            {
                Say("Nay, try again!");
            }

            base.OnSpeech(e);
        }


        public LarkTheMerry2(Serial serial) : base(serial) { }

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
