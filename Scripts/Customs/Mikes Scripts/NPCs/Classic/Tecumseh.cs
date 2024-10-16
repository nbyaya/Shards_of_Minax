using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tecumseh")]
    public class Tecumseh : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Tecumseh() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tecumseh";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 110;
            Int = 70;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1109 });
            AddItem(new LeatherChest() { Hue = 1109 });
            AddItem(new LeatherGloves() { Hue = 1109 });
            AddItem(new Bandana() { Hue = 1112 });
            AddItem(new Bardiche() { Name = "Tecumseh's Axe" });

            Hue = Race.RandomSkinHue();
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
                Say("Thou standest before Tecumseh, a watcher of the land.");
            }
            else if (speech.Contains("health"))
            {
                Say("The health of the land is fragile, like the spirit of man.");
            }
            else if (speech.Contains("job"))
            {
                Say("I walk the path of the guardian, a sentinel of these lands.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Seeker of wisdom, answer me this: What lies beneath the surface?");
            }
            else if (speech.Contains("beneath the surface"))
            {
                Say("The answers you seek are hidden, like the roots of a mighty oak.");
            }
            else if (speech.Contains("watcher"))
            {
                Say("For generations, my ancestors have been watchers, keeping an eye on the balance of nature.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of man is like a flickering flame, easily extinguished by the winds of temptation and turmoil.");
            }
            else if (speech.Contains("sentinel"))
            {
                Say("As a sentinel, I ensure that no harm comes to the sacred groves and the creatures that reside within.");
            }
            else if (speech.Contains("roots"))
            {
                Say("The roots not only anchor the tree but they draw sustenance, much like how our past experiences shape us.");
            }
            else if (speech.Contains("ancestors"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("They passed down stories and lessons, ensuring that the wisdom of old is not forgotten. For your keen interest, accept this small reward from me.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("temptation"))
            {
                Say("Temptation has led many astray, causing them to forget their true purpose in this realm.");
            }
            else if (speech.Contains("groves"))
            {
                Say("These groves are ancient and hold the spirits of creatures long gone. They are places of peace and reflection.");
            }
            else if (speech.Contains("past"))
            {
                Say("Our past experiences, both joyful and painful, weave together to form the tapestry of our souls.");
            }

            base.OnSpeech(e);
        }

        public Tecumseh(Serial serial) : base(serial) { }

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
