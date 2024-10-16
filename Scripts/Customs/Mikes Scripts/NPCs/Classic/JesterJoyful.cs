using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Joyful")]
    public class JesterJoyful : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterJoyful() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Joyful";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 80;
            Int = 85;
            Hits = 85;

            // Appearance
            AddItem(new JesterHat() { Hue = 35 });
            AddItem(new JesterSuit() { Hue = 45 });
            AddItem(new Sandals() { Hue = 1109 });

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
                Say("I am Jester Joyful, the keeper of mirth and wisdom.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am the embodiment of mirth and folly.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to bring laughter to the world.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("But remember, laughter is a virtue too.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The eight virtues guide us, each balancing the other.");
            }
            else if (speech.Contains("mirth"))
            {
                Say("Ah, mirth! It's the joy I spread with every jest and joke. Without mirth, the world would be a gloomy place indeed.");
            }
            else if (speech.Contains("folly"))
            {
                Say("Folly, you say? It is the mistakes we make that remind us of our humanity. Through folly, we learn, grow, and, most importantly, laugh at ourselves.");
            }
            else if (speech.Contains("laughter"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, laughter! It is the best medicine, they say. If you can make someone laugh, you've made their day. And for your efforts, here's a little reward from me.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("guide"))
            {
                Say("The virtues do guide us, just like a compass in a vast sea. But it is up to each of us to choose our path and decide how we let these virtues shape our destiny.");
            }
            else if (speech.Contains("balancing"))
            {
                Say("Balancing is key. Just as a tightrope walker must maintain balance, so must we balance the virtues in our lives to walk the path of righteousness.");
            }

            base.OnSpeech(e);
        }

        public JesterJoyful(Serial serial) : base(serial) { }

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
