using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zarth the Star Wanderer")]
    public class ZarthTheStarWanderer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZarthTheStarWanderer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zarth the Star Wanderer";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 150;
            Hits = 90;

            // Appearance
            AddItem(new Sandals() { Hue = 1173 });
            AddItem(new Cloak() { Hue = 1172 });
            AddItem(new QuarterStaff() { Name = "Zarth's Starseeker" });

            Hue = 1170; // Body hue
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Zarth the Star Wanderer, an explorer of cosmic mysteries!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect harmony with the cosmos.");
            }
            else if (speech.Contains("job"))
            {
                Say("My occupation is to seek the secrets of the universe.");
            }
            else if (speech.Contains("virtues") && speech.Contains("spirituality"))
            {
                Say("I often ponder the virtue of Spirituality. Is thy spirit in harmony with the cosmos?");
            }
            else if (speech.Contains("cosmos") && speech.Contains("universe") && speech.Contains("yes"))
            {
                Say("Ah, the mysteries of the cosmos await those who seek inner harmony. Do you seek to understand the universe?");
            }
            else if (speech.Contains("zarth"))
            {
                Say("Ah, you have heard of my travels! I've been to distant galaxies and seen wonders beyond imagination.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the balance of the stars, the planets, and oneself. It is a delicate dance that keeps the universe in motion.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are everywhere, hidden in the twinkling of stars, in ancient manuscripts, and even in the whispers of the wind. Do you wish to learn one?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("A harmonious spirit is one that listens, learns, and grows. The cosmos rewards those whose spirits are in tune.");
            }
            else if (speech.Contains("rewards"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The cosmos has gifted me with knowledge and small trinkets from my travels. For those in tune with the universe, I am inclined to share. Here, take this as a symbol of our conversation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("yes"))
            {
                Say("Good. The path to understanding the universe requires dedication and an open mind. Let your journey start with the stars.");
            }
            else if (speech.Contains("galaxies"))
            {
                Say("There are countless galaxies, each with its own secrets and wonders. I once visited a galaxy where stars sang melodies of ancient times.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Maintaining balance is crucial. Too much of one thing can offset the natural order. Do you strive for balance in your life?");
            }
            else if (speech.Contains("manuscripts"))
            {
                Say("I've come across ancient manuscripts that tell tales of civilizations that once communicated with the stars. Fascinating, isn't it?");
            }

            base.OnSpeech(e);
        }

        public ZarthTheStarWanderer(Serial serial) : base(serial) { }

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
