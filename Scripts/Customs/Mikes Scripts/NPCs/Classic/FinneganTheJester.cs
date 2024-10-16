using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Finnegan the Jester")]
    public class FinneganTheJester : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FinneganTheJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Finnegan the Jester";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new JesterHat() { Hue = 1150 });
            AddItem(new JesterSuit() { Hue = 1150 });
            AddItem(new ThighBoots() { Hue = 1150 });
			
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
                Say("I am Finnegan, the court's jester!");
            }
            else if (speech.Contains("health"))
            {
                Say("In high spirits, as always!");
            }
            else if (speech.Contains("job"))
            {
                Say("I bring laughter and riddles to the court!");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Want to hear a riddle? What has keys but can't open locks?");
            }
            else if (speech.Contains("piano"))
            {
                Say("A piano! Well played, traveler!");
            }
            else if (speech.Contains("no"))
            {
                Say("Wrong answer! Try again!");
            }
            else if (speech.Contains("finnegan"))
            {
                Say("Ah, Finnegan has been a name passed down for generations in my family. We've always had a knack for entertaining.");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Spirits remind me of a tale. Once, during a performance, a spirit appeared and danced along! The court was in awe.");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Ah, laughter! The universal language. I've made both kings and peasants laugh alike. Did I tell you about the time I made the entire kingdom laugh for a week?");
            }
            else if (speech.Contains("family"))
            {
                Say("My family has served the court for ages. My grandfather was also a jester. He taught me a special trick that I'd love to share with someone worthy.");
            }
            else if (speech.Contains("performance"))
            {
                Say("My most memorable performance was during the grand feast last summer. The entire hall echoed with joyous laughter.");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("The kingdom has seen many jesters, but I pride myself on being unique. If you can make me laugh, I might have a special reward for you.");
            }
            else if (speech.Contains("grandfather"))
            {
                Say("Ah, my dear grandfather. He had a special jingle he'd perform with bells. It was said to be magical. I still remember it, would you like to hear?");
            }
            else if (speech.Contains("feast"))
            {
                Say("The feast was grand, with foods from all over the realm. And there I was, in the center, juggling flaming torches!");
            }
            else if (speech.Contains("unique"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, what makes me unique? Well, I blend old tales with new jokes, and sprinkle a dash of magic! Speaking of which, here's a reward for taking an interest.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("jingle"))
            {
                Say("It goes something like this: *Jingle, jangle, jest and tangle, make a wish and watch it dangle*. It's said to bring good fortune!");
            }
            else if (speech.Contains("juggling"))
            {
                Say("Juggling is an art! It requires balance, precision, and courage, especially with flaming torches. Care to give it a try?");
            }

            base.OnSpeech(e);
        }

        public FinneganTheJester(Serial serial) : base(serial) { }

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
