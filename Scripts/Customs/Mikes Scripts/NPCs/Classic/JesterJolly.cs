using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Jolly")]
    public class JesterJolly : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterJolly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Jolly";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new JesterHat() { Hue = 2210 });
            AddItem(new JesterSuit() { Hue = 1260 });
            AddItem(new Boots() { Hue = 1154 });

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
                Say("Greetings, traveler! I am Jester Jolly, the whimsical entertainer!");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, I'm always in high spirits, thank you for asking!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to bring laughter and joy to this world through jests and pranks!");
            }
            else if (speech.Contains("humor"))
            {
                Say("Laughter is a virtue, a key to the heart. Tell me, traveler, do you find humor in life's twists and turns?");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Ah, a keen sense of humor, I like that! In this world, where chaos and order dance, humor can be a beacon. Do you possess any other virtues?");
            }
            else if (speech.Contains("eight virtues"))
            {
                Say("Ah, the eight virtues! Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each a facet of a radiant gem. How do you see them intertwining in your own journey?");
            }
            else if (speech.Contains("jolly"))
            {
                Say("Ah, you recognize the name! Yes, I've been performing under that moniker for many a moon now. Have you heard any of the tales of my grand performances?");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Indeed! My spirits are often lifted by the crowd's applause. It's the energy from my audience that keeps me feeling alive and vibrant. Have you attended any jesters' festivals?");
            }
            else if (speech.Contains("pranks"))
            {
                Say("Ah, pranks! They're the spice of a jester's life. Some love them, some loathe them, but all remember them. Would you like to hear about my most memorable prank?");
            }
            else if (speech.Contains("twists"))
            {
                Say("Life is indeed full of unexpected moments. Sometimes it's the unexpected that brings the heartiest of laughs. Have you experienced any humorous surprises recently?");
            }
            else if (speech.Contains("beacon"))
            {
                Say("Indeed, humor shines bright like a beacon in the darkest of times. But humor isn't the only light. Have you ever used humor to guide others in their path?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every individual's journey is unique, yet all are touched by the virtues. Tell me, is there a particular virtue you hold dearest to your heart?");
            }
            else if (speech.Contains("dearest"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, so that virtue resonates with your soul! Here, for being so open about your values, accept this small token from me. It might come in handy on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public JesterJolly(Serial serial) : base(serial) { }

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
