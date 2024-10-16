using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Jibe")]
    public class JesterJibe : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterJibe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Jibe";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 75;
            Int = 90;
            Hits = 90;

            // Appearance
            AddItem(new JesterHat() { Hue = 2128 });
            AddItem(new JesterSuit() { Hue = 2128 });
            AddItem(new ThighBoots() { Hue = 1104 });

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
                Say("Greetings, traveler! I am Jester Jibe, the eternal jester of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am but a humble jester, always jesting and dancing, never a care in the world.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? It's to bring laughter to the world and remind folks of the virtues that bind us.");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("question"))
                {
                    Say("Indeed, the virtues are a path to enlightenment. Let me ask you, which virtue do you think best exemplifies a jester like me?");
                }
                else if (speech.Contains("humility"))
                {
                    Say("Humility, indeed! For a jester must always remember that even the greatest jest is but a fleeting moment, and humility keeps us grounded.");
                }
                else
                {
                    Say("The virtues guide not just me but everyone in the kingdom. There's an old shrine dedicated to the virtues, hidden deep within the Forest of Echoes. Seek it out, and you might find a reward for your dedication.");
                }
            }
            else if (speech.Contains("jibe"))
            {
                Say("Jester Jibe is not my birth name, you know. I adopted it when I became a jester, to better fit the role. Before that, people knew me as a simple minstrel, wandering the lands with my lute.");
            }
            else if (speech.Contains("jester"))
            {
                Say("Jest and dance may keep my spirit alive, but I do have a secret potion that keeps me on my toes. It's made from rare herbs found only in the Whispering Woods.");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Apart from jesting, I also perform in the King's court on special occasions. It's a place of grandeur, but also of intrigues and secrets. One must always be careful there.");
            }
            else if (speech.Contains("question"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, it's always a delight to meet someone who ponders the deeper questions of life. For your curiosity, allow me to gift you something. It may come in handy on your journey.");
                    from.AddToBackpack(new LuckAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is not just for jesters but for all. There's an old saying in our land: 'Pride is the mask of one's own faults.' Remember this, and you'll go far.");
            }

            base.OnSpeech(e);
        }

        public JesterJibe(Serial serial) : base(serial) { }

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
