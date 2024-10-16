using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Michael Psellus")]
    public class MichaelPsellus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MichaelPsellus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Michael Psellus";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 60;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1157 }); // Robe with hue 1157
            AddItem(new Sandals() { Hue = 1157 }); // Sandals with hue 1157
            AddItem(new Spellbook() { Name = "Michael Psellus's Chronicle" });

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
                Say("Oh, you want to know who I am, do you?");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is none of your business, but don't worry, I'll live to suffer another day.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Ha! I'm a resident of this wretched place they call Byzantium, stuck in the muck of history.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True strength? You'll find none of that here, only the endless drudgery of existence. Are you brave enough to face it?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! I see you're as deluded as the rest. Bravery is a fool's errand. But if you insist, go on, be brave!");
            }
            else if (speech.Contains("byzantium"))
            {
                Say("Ah, Byzantium! Once a jewel of the world, now lost to time. But secrets of its past still lurk in shadows, waiting to be found.");
            }
            else if (speech.Contains("suffer"))
            {
                Say("Every day is a test, a challenge. But it's in suffering that we sometimes find purpose. I've come across ancient relics that carry tales of old. Perhaps you'd be interested?");
            }
            else if (speech.Contains("history"))
            {
                Say("Byzantium's history is a tapestry of tales, woven through time. Many seek its lost stories. Some even say there's a hidden chamber in the city, untouched for centuries.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows hide more than just the past. They often hold treasures. I've got a few I've found over the years. For someone as curious as you, I'll part with one.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose is a fickle thing. But these relics, they're remnants of a bygone era. If you prove yourself worthy, I might just entrust one to you.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Tales of bravery, of heroes long gone. If you ever manage to find that chamber, who knows what you might discover? I'll give you a hint, but only if you prove you can handle it.");
            }
            else if (speech.Contains("curious"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Curiosity has led many to their doom, but it's also the key to unraveling mysteries. Here, take this. It might aid you on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("relics"))
            {
                Say("Ah, eager for the relics, are you? Very well. Complete a task for me, and I might deem you worthy. Seek the Golden Chalice hidden in the city and bring it to me.");
            }
            else if (speech.Contains("discover"))
            {
                Say("Discovery is a path few tread. But for you, I'll share this: 'Where moonlight touches stone, the entrance is shown'. Seek wisely.");
            }

            base.OnSpeech(e);
        }

        public MichaelPsellus(Serial serial) : base(serial) { }

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
