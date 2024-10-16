using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ocelot Oliver")]
    public class OcelotOliver : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OcelotOliver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ocelot Oliver";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 90;
            Hits = 100;

            // Appearance
            AddItem(new FurCape() { Hue = 1173 });
            AddItem(new FurBoots() { Hue = 1173 });
            AddItem(new FurSarong() { Hue = 1173 });
            AddItem(new Dagger() { Name = "Ocelot Oliver's Dagger" });

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
                Say("I am Ocelot Oliver, the animal tamer!");
            }
            else if (speech.Contains("health"))
            {
                Say("All my animals are in good health!");
            }
            else if (speech.Contains("job"))
            {
                Say("I train and care for a variety of animals!");
            }
            else if (speech.Contains("animals"))
            {
                Say("The bond between human and animal is a true virtue! Are you compassionate?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then treat all creatures with kindness!");
            }
            else if (speech.Contains("ocelot"))
            {
                Say("Ah, not many recognize my unique name! My parents named me after the wild ocelot, a creature known for its stealth and beauty.");
            }
            else if (speech.Contains("feelings"))
            {
                Say("Every animal has its own way of expressing emotions. For instance, a wagging tail might signify happiness in dogs but agitation in cats.");
            }
            else if (speech.Contains("train"))
            {
                Say("Training isn't just about commands; it's about building trust. I have a special whistle that I use to communicate with them.");
            }
            else if (speech.Contains("parents"))
            {
                Say("My parents were adventurers who traveled the lands. They shared tales of mythical creatures and taught me to respect all living beings.");
            }
            else if (speech.Contains("whistle"))
            {
                Say("This whistle was passed down through my family for generations. It has a magical tune that only animals can hear. If you're ever in need, use this whistle, and nature might come to your aid.");
            }
            else if (speech.Contains("magical"))
            {
                // Reward mechanism with 10-minute cooldown
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Magic isn't just spells and potions. It's in the bond we share with animals and nature. In fact, I have a small magical token I can share with you as a reward for your interest.");
                    from.AddToBackpack(new MaxxiaScroll()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("reward"))
            {
                Say("Here you go, a charm imbued with nature's essence. It might bring you luck on your journeys.");
                from.AddToBackpack(new MaxxiaScroll()); // Another reward item
            }

            base.OnSpeech(e);
        }

        public OcelotOliver(Serial serial) : base(serial) { }

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
