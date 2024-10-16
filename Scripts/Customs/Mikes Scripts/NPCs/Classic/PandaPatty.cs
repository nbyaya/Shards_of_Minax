using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Panda Patty")]
    public class PandaPatty : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PandaPatty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Panda Patty";
            Body = 0x191; // Human female body

            // Stats
            Str = 150;
            Dex = 70;
            Int = 110;
            Hits = 110;

            // Appearance
            AddItem(new BodySash() { Hue = 1173 });
            AddItem(new Kilt() { Hue = 1173 });
            AddItem(new Cloak() { Hue = 1173 });
            AddItem(new Sandals() { Hue = 1173 });
            AddItem(new QuarterStaff() { Name = "Panda Patty's Staff" });

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
                Say("I am Panda Patty, the animal tamer!");
            }
            else if (speech.Contains("health"))
            {
                Say("All my animals are in great health!");
            }
            else if (speech.Contains("job"))
            {
                Say("I train animals for a living.");
            }
            else if (speech.Contains("animals"))
            {
                Say("Compassion for animals is a virtue. Do you share this virtue?");
            }
            else if (speech.Contains("yes") && lastRewardTime.AddMinutes(10) <= DateTime.UtcNow)
            {
                Say("Wonderful! Here you go, a token of my appreciation. Remember, always be kind to every creature you meet.");
                from.AddToBackpack(new MaxxiaScroll()); // Replace with actual item
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
            else if (speech.Contains("yes"))
            {
                Say("I have no reward right now. Please return later.");
            }
            else if (speech.Contains("patty"))
            {
                Say("Panda Patty is what they call me, named after the very first creature I tamed. Have you ever encountered a panda before?");
            }
            else if (speech.Contains("griffin"))
            {
                Say("Yes, ensuring the health of my animals is my top priority. Just recently, I nursed a wounded griffin back to health. Have you ever seen a griffin?");
            }
            else if (speech.Contains("train"))
            {
                Say("Training animals is a skill passed down in my family for generations. My grandmother was especially renowned for taming wild dragons. Do you believe in dragons?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is the cornerstone of my work. For showing such understanding, I'd like to offer you a small reward. Would you accept it?");
            }

            base.OnSpeech(e);
        }

        public PandaPatty(Serial serial) : base(serial) { }

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
