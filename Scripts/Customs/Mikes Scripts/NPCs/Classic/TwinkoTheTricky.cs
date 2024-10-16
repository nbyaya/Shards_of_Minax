using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Twinko the Tricky")]
    public class TwinkoTheTricky : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TwinkoTheTricky() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Twinko the Tricky";
            Body = 0x190; // Human male body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Stats
            Str = 85;
            Dex = 70;
            Int = 90;
            Hits = 88;

            // Appearance
            AddItem(new JesterHat() { Hue = 1157 });
            AddItem(new JesterSuit() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1167 });

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
                Say("Greetings, traveler! I am Twinko the Tricky, the jester of these lands!");
            }
            else if (speech.Contains("health"))
            {
                Say("As for my health, well, I'm as fit as a fiddle, always ready for a jest!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? Why, it's to bring laughter and joy to all who cross my path!");
            }
            else if (speech.Contains("virtues"))
            {
                Say("But enough about me! Let's talk about virtues, those tricky little things! Which virtue do you find most intriguing?");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Ah, the virtue of honesty! It's a tricky one, isn't it? How do you think honesty and humor go hand in hand?");
            }
            else if (speech.Contains("twinko"))
            {
                Say("Twinko? That's a name I earned long ago for my quick wit and nimble fingers. They say I once tricked a dragon into laughing! Do you believe in dragons?");
            }
            else if (speech.Contains("fiddle"))
            {
                Say("Ah, 'fit as a fiddle'! It's a saying from my jester training days. It means being in perfect health. Have you ever tried playing a fiddle?");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Laughter and joy, the two emotions I cherish the most. But there's one joke that no one's been able to solve. If you manage to, I might give you a little reward. Would you like to hear it?");
            }
            else if (speech.Contains("tricky"))
            {
                Say("Indeed, virtues can be tricky. They guide us, yet challenge us. Have you ever faced a challenge where your virtues were tested?");
            }
            else if (speech.Contains("humor"))
            {
                Say("Humor and honesty often dance together. A true jest strikes at truth, and honest laughter is the most genuine. Ever thought of becoming a jester yourself?");
            }
            else if (speech.Contains("dragons"))
            {
                Say("Dragons, magnificent and fierce creatures! I once made a dragon chuckle by playing a tune on my flute. Speaking of which, have you ever heard the melody of the dragon's flute?");
            }
            else if (speech.Contains("joke"))
            {
                Say("Alright, here's the joke: 'Why did the scarecrow win an award? Because he was outstanding in his field!' If you ever find the answer to the deeper meaning behind it, come tell me, and your reward awaits!");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Every challenge is an opportunity to grow, and in my line of work, to make others smile. Have you ever turned a difficult situation into a jest?");
            }
            else if (speech.Contains("jester"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Becoming a jester is not just about jokes, but observing the world with a keen eye. If you ever decide to walk this path, I've got a special jester's hat for you as a reward.");
                    from.AddToBackpack(new JesterHatOfCommand()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public TwinkoTheTricky(Serial serial) : base(serial) { }

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
