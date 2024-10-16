using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Beggar Bob")]
    public class BeggarBob : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BeggarBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Beggar Bob";
            Body = 0x190; // Human male body

            // Stats
            Str = 40;
            Dex = 30;
            Int = 20;
            Hits = 45;

            // Appearance
            AddItem(new LongPants(902)); // Pants with hue 902
            AddItem(new Shirt(2500));    // Shirt with hue 2500
            AddItem(new Sandals(902));   // Sandals with hue 902

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

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
                Say("I am Beggar Bob, the humble beggar.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is frail, and I suffer in the streets.");
            }
            else if (speech.Contains("job"))
            {
                Say("Begging is my job, kind sir/madam.");
            }
            else if (speech.Contains("humility"))
            {
                Say("I may have nothing, but I strive for humility. Do you understand humility?");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("humility"))
                {
                    Say("Humility is the virtue of selflessness and modesty. It is a noble path to follow.");
                }
            }
            else if (speech.Contains("bob"))
            {
                Say("Bob's just a name I picked up. My real name's been lost to time, much like the town I come from.");
            }
            else if (speech.Contains("suffer"))
            {
                Say("The cold nights and relentless hunger takes its toll, but the kindness of strangers gives me hope.");
            }
            else if (speech.Contains("begging"))
            {
                Say("It's not a life I chose. Once, I was a merchant with riches and respect. But fate can be cruel.");
            }
            else if (speech.Contains("selflessness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Being selfless taught me to appreciate the small joys in life. Speaking of which, here's something for you. It's not much, but I hope it brings you a smile.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("town"))
            {
                Say("My hometown, Silverbrook, was a vibrant place. Alas, now it's just ruins and memories.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("A warm meal or a kind word, every bit of kindness has helped me survive another day. Here take this as a token of my kindness.");
                // Rewarding should be handled in a specific condition to avoid repeatable rewards.
            }
            else if (speech.Contains("merchant"))
            {
                Say("I traded in rare artifacts. Lost it all in a shipwreck off the Misty Isles.");
            }
            else if (speech.Contains("joys"))
            {
                Say("Even in this life, joys come in many forms - a sunny day, the song of a bird, or the laughter of children.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey has its challenges. But it's the lessons we learn and the friends we make that matter.");
            }

            base.OnSpeech(e);
        }

        public BeggarBob(Serial serial) : base(serial) { }

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
