using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherd Tom")]
    public class ShepherdTom : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdTom() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherd Tom";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 50;
            Hits = 70;

            // Appearance
            AddItem(new PlainDress() { Hue = 1113 });
            AddItem(new Sandals() { Hue = 0 });
            AddItem(new ShepherdsCrook() { Name = "Shepherd Tom's Crook" });

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
                Say("Greetings, traveler! I am Shepherd Tom.");
            }
            else if (speech.Contains("health"))
            {
                Say("I tend to my flock, and they are healthy.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to care for these sheep and guide them to safety.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("True virtue lies in compassion for all creatures. Dost thou show compassion?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Compassion is a virtue that brings light to the world. May it guide thy actions, traveler.");
            }
            else if (speech.Contains("tom"))
            {
                Say("Shepherd Tom is what they call me, named after my father, a great shepherd himself.");
            }
            else if (speech.Contains("flock"))
            {
                Say("Yes, my flock is in good health, thanks to the nourishing meadows around these parts. Have you seen the meadows?");
            }
            else if (speech.Contains("care"))
            {
                Say("Caring for sheep isn't just a job, it's a calling. I've been doing it since I was a young lad. Ever heard the tales of the legendary sheep?");
            }
            else if (speech.Contains("father"))
            {
                Say("My father was a dedicated shepherd, always putting the needs of his flock first. He taught me the importance of dedication. Do you believe in dedication?");
            }
            else if (speech.Contains("meadows"))
            {
                Say("The meadows here are lush and green, providing the perfect food for the sheep. It's a blessing to have such land. Do you appreciate nature's blessings?");
            }
            else if (speech.Contains("legendary"))
            {
                Say("Ah, the tales of the legendary sheep speak of a special sheep with golden fleece. It's said that whoever finds it will be granted a reward. Are you seeking rewards?");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Rewards come in many forms, not just gold or treasures. Sometimes, the reward is the journey itself. But for you, kind traveler, I have a small token of appreciation for your interest. Here, take this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ShepherdTom(Serial serial) : base(serial) { }

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
