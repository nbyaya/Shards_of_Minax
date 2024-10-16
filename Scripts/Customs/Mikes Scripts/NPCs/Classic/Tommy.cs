using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tommy")]
    public class Tommy : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Tommy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tommy";
            Body = 0x190; // Human male body

            // Stats
            Str = 60;
            Dex = 40;
            Int = 40;
            Hits = 40;

            // Appearance
            AddItem(new LongPants() { Hue = 1152 });
            AddItem(new FancyShirt() { Hue = 1152 });
            AddItem(new Boots() { Hue = 1152 });
            AddItem(new ShepherdsCrook() { Name = "Tommy's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Tommy the Shepherd.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good health, tending to my flock.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to care for these sheep, guiding them through their days.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("The virtue of compassion guides my every step. How do you view compassion?");
            }
            else if (speech.Contains("show compassion"))
            {
                Say("Compassion is the light that warms the coldest heart. How can one show compassion in their daily life?");
            }
            else if (speech.Contains("shepherd"))
            {
                Say("Aye, being a shepherd is a noble calling. I have learned much from the sheep, especially patience.");
            }
            else if (speech.Contains("flock"))
            {
                Say("My flock is my family. I have raised many from lambs, and watched them grow. It's a fulfilling life, seeing them thrive.");
            }
            else if (speech.Contains("guide"))
            {
                Say("It's not just about leading them to fresh pastures, but also about protecting them from dangers like wolves. I have many tales to tell from my years in this job.");
            }
            else if (speech.Contains("view"))
            {
                Say("Our views shape our actions. By seeing compassion as a fundamental part of life, we can act in ways that support and nurture those around us.");
            }
            else if (speech.Contains("daily"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In daily life, one can show compassion by helping those in need, listening without judgment, and being kind even when it's difficult. For your thoughtful reflection, take this token of appreciation from me.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Tommy(Serial serial) : base(serial) { }

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
