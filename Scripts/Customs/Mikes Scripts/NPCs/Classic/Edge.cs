using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Edge")]
    public class Edge : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Edge() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Edge";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1153 });
            AddItem(new LeatherChest() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });
            AddItem(new LeatherCap() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new Katana() { Name = "Edge's Katana" });

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
                Say("I am Edge, the one and only. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care about my health? I'm not your physician.");
            }
            else if (speech.Contains("job"))
            {
                Say("Job? Hah! I'm a jack of all trades, master of none. Happy now?");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor? Spare me the heroics. What's your point?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, you're valiant, are you? How noble. Whatever.");
            }
            else if (speech.Contains("edge"))
            {
                Say("Why do you keep calling me by my name? Do I look familiar to you? Or are you just trying to get on my good side?");
            }
            else if (speech.Contains("physician"))
            {
                Say("I'm not your physician, but I once met a skilled healer in the East who taught me a thing or two about remedies. Perhaps that's of interest to you?");
            }
            else if (speech.Contains("trades"))
            {
                Say("Yes, I've tried my hand at many things. Blacksmithing, tailoring, even a bit of magic. But none held my interest for long. Why, you looking to learn a trade?");
            }
            else if (speech.Contains("familiar"))
            {
                Say("No, we've never met before. But if you've heard of me, it's probably because of my infamous escapades in the Forbidden Caves.");
            }
            else if (speech.Contains("remedies"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you're interested in remedies? Here's a little something for your troubles.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is a fickle thing. I've dabbled, but never mastered it. The arcane arts require a dedication I simply don't possess.");
            }
            else if (speech.Contains("caves"))
            {
                Say("Ah, the Forbidden Caves. A place of mystery and danger. Many adventurers have sought its treasures, but few have returned. Tread carefully if you ever decide to venture there.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("The arcane arts are not for the faint of heart. It's said that deep within the Mystic Woods, there's a hidden academy where mages train in secret. But, that's just a rumor.");
            }

            base.OnSpeech(e);
        }

        public Edge(Serial serial) : base(serial) { }

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
