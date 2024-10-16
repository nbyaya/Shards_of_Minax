using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Locke Cole")]
    public class LockeCole : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LockeCole() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Locke Cole";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 130;
            Int = 70;
            Hits = 75;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1175 });
            AddItem(new LeatherChest() { Hue = 1175 });
            AddItem(new LeatherCap() { Hue = 1175 });
            AddItem(new Dagger() { Name = "Locke's Dagger" });

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
                Say("Greetings, stranger. I'm Locke Cole.");
            }
            else if (speech.Contains("health"))
            {
                Say("I've had my fair share of scrapes, but I'm still standing.");
            }
            else if (speech.Contains("job"))
            {
                Say("I make a living as a treasure hunter, searching for hidden relics.");
            }
            else if (speech.Contains("approach"))
            {
                Say("Life's full of choices, isn't it? What's your approach to challenges?");
            }
            else if (speech.Contains("daring"))
            {
                Say("Well, I respect that. It takes a certain daring to succeed in this world.");
            }
            else if (speech.Contains("scrapes"))
            {
                Say("Every scar tells a story, and trust me, I've got plenty. But I suppose that's the life of an adventurer.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("It's not just about the gold and jewels. It's the thrill of the hunt, and the legends behind each relic that drive me.");
            }
            else if (speech.Contains("rumors"))
            {
                Say("They say that the most valuable treasures are guarded by the most dangerous creatures. But, for those brave enough to face them, the rewards are worth it.");
            }
            else if (speech.Contains("scar"))
            {
                Say("This one here on my arm? Got that from a near encounter with a dragon. Not my finest moment, but I learned a valuable lesson.");
            }
            else if (speech.Contains("legends"))
            {
                Say("One legend speaks of an ancient city, hidden beneath the mountains, where time stands still. I aim to find it one day.");
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
                    Say("Speaking of rewards, for someone as inquisitive as you, here's a little something. Keep it safe, it might come in handy.");
                    from.AddToBackpack(new StealingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("dragon"))
            {
                Say("Dragons are powerful, majestic creatures. Not something you'd want to confront without proper preparation. But their treasures? Unparalleled.");
            }
            else if (speech.Contains("locke"))
            {
                Say("Ah, you've heard of me before? Rumors tend to spread, but not all of them are true.");
            }

            base.OnSpeech(e);
        }

        public LockeCole(Serial serial) : base(serial) { }

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
