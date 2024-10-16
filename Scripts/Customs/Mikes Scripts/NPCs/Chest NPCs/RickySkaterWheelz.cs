using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ricky 'Skater' Wheelz")]
    public class RickySkaterWheelz : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RickySkaterWheelz() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ricky 'Skater' Wheelz";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 60;
            Hits = 60;

            // Appearance
            AddItem(new Bandana() { Name = "Rad Bandana", Hue = Utility.RandomBrightHue() });
            AddItem(new StuddedChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new StuddedLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomBrightHue() });

            // Speech Hue
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
                Say("Hey dude, I'm Ricky 'Skater' Wheelz. Welcome to the rad side!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm totally tubular! Just hanging loose and enjoying the ride.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To skate, chill, and spread the rad vibes. It's all about the good times!");
            }
            else if (speech.Contains("skate"))
            {
                Say("Skating is life! It's about freedom, fun, and feeling the wind in your hair. You know, the best part about skating is the thrill of the ride!");
            }
            else if (speech.Contains("thrill"))
            {
                Say("The thrill comes from pushing yourself and feeling that rush of adrenaline. It's all about living in the moment and enjoying every second!");
            }
            else if (speech.Contains("adrenaline"))
            {
                Say("Adrenaline makes you feel alive, like you're on top of the world. It's a great feeling, kind of like finding hidden treasure!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! You've got the right idea. There’s a special reward for those who really understand what it means to be rad.");
            }
            else if (speech.Contains("reward"))
            {
                Say("For a true skater like yourself, I’ve got something really special. But first, tell me about your favorite skate spot.");
            }
            else if (speech.Contains("skate spot"))
            {
                Say("Nice choice! Every skater has their favorite spot where they feel most alive. Mine is the old park down by the river.");
            }
            else if (speech.Contains("park"))
            {
                Say("The park is where the magic happens. It’s where I learned some of my best tricks. Speaking of tricks, do you have any of your own?");
            }
            else if (speech.Contains("tricks"))
            {
                Say("Tricks are what make skating so awesome. They’re a way to express yourself. For showing off your trick knowledge, I’ve got something special for you.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is power, especially in the world of skating. The more you know, the better you become. And speaking of better, how about a little challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("If you're up for a challenge, show me how well you know the skating scene. I’ve got a special reward for those who really impress me.");
            }
            else if (speech.Contains("impress"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I’ve already given out my reward recently. Come back in a bit for another chance!");
                }
                else
                {
                    Say("You’ve truly impressed me with your skater knowledge and attitude. Here’s the Rad Rider's Stash I promised. Keep riding and stay rad!");
                    from.AddToBackpack(new RadRidersStash()); // Give the Rad Rider's Stash as the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RickySkaterWheelz(Serial serial) : base(serial) { }

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
