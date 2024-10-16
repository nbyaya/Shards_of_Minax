using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Bloodbeard")]
    public class CaptainBloodbeard : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainBloodbeard() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Bloodbeard";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 70;
            Int = 30;
            Hits = 100;

            // Appearance
            AddItem(new ShortPants() { Hue = 38 });
            AddItem(new FancyShirt() { Hue = 2111 });
            AddItem(new Boots() { Hue = 1157 });
            AddItem(new ThinLongsword() { Name = "Bloodbeard's Saber" });
            AddItem(new TricorneHat() { Hue = 2112 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ye be standin' before Captain Bloodbeard, the most feared pirate on these seas!");
            }
            else if (speech.Contains("health"))
            {
                Say("I be as healthy as a shark in a frenzy!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job be plunderin' and seekin' treasure, of course!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Life be a series of battles, lad. Be ye ready to face the storms?");
            }
            else if (speech.Contains("law"))
            {
                Say("Arr, ye have a spine, I see. Tell me, what be yer thoughts on the scallywags who follow the law blindly?");
            }
            else if (speech.Contains("bloodbeard"))
            {
                Say("Bloodbeard be not just a name, it's a legacy. It be a tale of how I got me beard stained with the blood of a treacherous mutineer!");
            }
            else if (speech.Contains("shark"))
            {
                Say("Ah, the shark, a creature I respect greatly. They be the true rulers of the deep, always on the hunt, never restin'. Just like meself!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasure be not just about gold and jewels, lad. The greatest treasures be the tales we collect and the mates we share 'em with.");
            }
            else if (speech.Contains("storms"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Storms be nature's way of testin' us. But after each storm, there be calm and a rainbow. If ye can withstand the storm, ye might find yer own treasure at the end of that rainbow. Here, for yer bravery, take this as a token.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("scallywags"))
            {
                Say("Scallywags be everywhere, not just on the seas. Some wear the mask of law, others be pirates. It's the heart that truly defines a man, not the flag he sails under.");
            }

            base.OnSpeech(e);
        }

        public CaptainBloodbeard(Serial serial) : base(serial) { }

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
