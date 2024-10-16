using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lark the Merry")]
    public class LarkTheMerry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LarkTheMerry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lark the Merry";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 90;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new JesterSuit() { Hue = 1153 }); // Jester suit with hue 1153
            AddItem(new JesterHat() { Hue = 1153 }); // Jester hat with hue 1153
            
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
                Say("Ho ho! Art thou in need of some merriment?");
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
            else if (speech.Contains("lark"))
            {
                Say("Ah, Lark is but a name. The true essence of merriness lies within! Do you believe in the power of laughter?");
            }
            else if (speech.Contains("fiddle"))
            {
                Say("Ah, the fiddle! A wonderful instrument that brings much joy. Have you ever heard a fiddle's melody?");
            }
            else if (speech.Contains("jest"))
            {
                Say("Jest is an art form, much like painting or singing. It's not merely about making others laugh, but about understanding the human soul. Have you ever tried jesting?");
            }
            else if (speech.Contains("doors"))
            {
                Say("Doors can be both literal and metaphorical. While some doors may remain closed, others open to vast opportunities. Ever stumbled upon a mysterious door in your travels?");
            }
            else if (speech.Contains("guessed"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your wit is commendable! For your keen intellect, I bestow upon you a small reward. May it serve you well on your journey.");
                    from.AddToBackpack(new Gold(1000)); // Give 100 gold as a reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("laughter"))
            {
                Say("Ah, the sweet sound of laughter, a balm for weary souls. There's a tale I once heard about a village where laughter was the most treasured possession. Would you like to hear it?");
            }
            else if (speech.Contains("melody"))
            {
                Say("A melody can capture emotions and memories, taking one on a journey through time. I once composed a tune for the king's coronation. Would you like a rendition?");
            }
            else if (speech.Contains("soul"))
            {
                Say("The soul, ethereal and profound, is the essence of our being. There's an old shrine dedicated to the guardians of souls. Have you ever visited such a place?");
            }
            else if (speech.Contains("health"))
            {
                Say("As fit as a fiddle!");
            }

            base.OnSpeech(e);
        }

        public LarkTheMerry(Serial serial) : base(serial) { }

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
