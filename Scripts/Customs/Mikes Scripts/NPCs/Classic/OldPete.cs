using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Old Pete")]
    public class OldPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OldPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Old Pete";
            Body = 0x190; // Human male body

            // Stats
            Str = 40;
            Dex = 30;
            Int = 30;
            Hits = 40;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new SkullCap() { Hue = 1153 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Can't you see I'm busy here, laddie?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? You think a beggar like me worries about health?");
            }
            else if (speech.Contains("job"))
            {
                Say("Job? You're lookin' at it, lad. Beggin' is my profession!");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor, huh? What's that got to do with a beggar's life?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Answer me this: do you know the taste of hunger, lad?");
            }
            else if (speech.Contains("old"))
            {
                Say("Aye, that's me. Been on these streets longer than most can remember. I've seen things, lad. Things that'd make your skin crawl.");
            }
            else if (speech.Contains("beggar"))
            {
                Say("It ain't a proud profession, but it's the one I know. Folks pass me by, but some still show a hint of kindness.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Aye, every once in a while, a good soul stops and lends me a hand. It's them small gestures that give me hope, and speaking of gestures, I remember something about a mantra.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Heard some monks whispering once. Said the third syllable of the mantra of Compassion is MUH. Don't know much else about it, but it stuck with me.");
            }
            else if (speech.Contains("streets"))
            {
                Say("These streets have history. From joyous parades to dark shadows lurking in the alleys. But they've always been my home.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Oh, the tales I could tell. Seen shady dealings and cloaked figures passing secrets. But a beggar like me? I keep my head down and stay out of it.");
            }
            else if (speech.Contains("hunger"))
            {
                Say("It gnaws at you. Day in, day out. But hunger also teaches you about the world, about who truly cares.");
            }
            else if (speech.Contains("cares"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("A few kind souls, here and there. But mostly, folks are too busy with their own lives. Can't blame them, though. Here take this it should help.");
                    from.AddToBackpack(new AnimalTamingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public OldPete(Serial serial) : base(serial) { }

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
