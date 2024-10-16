using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rebel Scout Arya")]
    public class RebelScoutArya : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RebelScoutArya() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rebel Scout Arya";
            Body = 0x191; // Human female body

            // Stats
            Str = 130;
            Dex = 120;
            Int = 50;
            Hits = 90;

            // Appearance
            AddItem(new ShortPants() { Hue = 5005 });
            AddItem(new Shirt() { Hue = 5005 });
            AddItem(new Cap() { Hue = 5005 });
            AddItem(new Boots() { Hue = 5005 });
            AddItem(new Crossbow() { Name = "Arya's Quickbow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize the reward timer
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Rebel Scout Arya, and you're wasting my time.");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care? My health is my own business.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To watch from the shadows while others bask in glory.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think battles make you noble? Hah! What do you know of true valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, so you claim to be valiant? Prove it then, with more than just words.");
            }
            else if (speech.Contains("arya"))
            {
                Say("You've heard of me, have you? Not many know the stories of Rebel Scout Arya, and fewer still live to tell them.");
            }
            else if (speech.Contains("business"))
            {
                Say("My health may be my business, but I've endured more than most. The scars I bear are not just physical.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows are my allies, they've kept me safe in many a mission. But they've also taught me that darkness can be a refuge.");
            }
            else if (speech.Contains("stories"))
            {
                Say("Ah, the tales I could tell. Of battles won, allies lost, and the price of freedom. But such tales come at a price. Prove your worth, and perhaps I'll share one with you.");
            }
            else if (speech.Contains("scars"))
            {
                Say("Each scar tells a tale of survival and perseverance. If you've time and a keen ear, I could share the story behind one of them.");
            }
            else if (speech.Contains("mission"))
            {
                Say("My missions are for the rebel cause, to free the oppressed and fight injustice. If you truly wish to help, complete a task for me, and I'll reward you for your efforts.");
            }
            else if (speech.Contains("price"))
            {
                Say("Everything has its price. For some, it's gold. For others, it's loyalty. For me? Well, that's a secret. But assist me in my endeavors, and I might just reveal it.");
            }
            else if (speech.Contains("tale"))
            {
                Say("One of my scars came from a fierce battle against the empire's elite guards. It was a night of rain, and the ground was slick with mud. But we emerged victorious, though not without cost.");
            }
            else if (speech.Contains("task"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("There's a location not far from here where we've hidden supplies. Retrieve them for me, and I'll give you a reward fitting for your efforts.");
                    from.AddToBackpack(new MaxxiaScroll()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("secret"))
            {
                Say("Some secrets are best kept hidden, but for those who prove themselves, I might just let one slip. Accomplish a task for the rebels, and I'll share something few know.");
            }

            base.OnSpeech(e);
        }

        public RebelScoutArya(Serial serial) : base(serial) { }

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
