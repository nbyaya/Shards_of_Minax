using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Puckery")]
    public class JesterPuckery : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterPuckery() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Puckery";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new JesterHat() { Hue = Utility.RandomBlueHue() });
            AddItem(new JesterSuit() { Hue = Utility.RandomBrightHue() });
            AddItem(new Shoes() { Hue = Utility.RandomBrightHue() });
            AddItem(new BodySash() { Hue = Utility.RandomBrightHue() });

            // Hair and facial hair
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = Utility.RandomNeutralHue();

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
                Say("Ah, you found me! I am Jester Puckery, the merriest of jesters.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as vibrant as a rainbow on a sunny day!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to spread laughter and joy, and perhaps a few surprises along the way.");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Laughter is a magical gift that brightens even the darkest day. Wouldn't you agree?");
            }
            else if (speech.Contains("magic"))
            {
                Say("The real magic lies in making others smile. But if you're looking for something special...");
            }
            else if (speech.Contains("special"))
            {
                Say("If you seek the playful spirit of the jester, you'll find it in the very chest I guard.");
            }
            else if (speech.Contains("chest"))
            {
                Say("The chest is a marvel of jest and whimsy. To unlock it, you must first understand the true meaning of merriment.");
            }
            else if (speech.Contains("merriment"))
            {
                Say("Merriment is not just about laughing; it's about bringing joy to others. Tell me, what brings you joy?");
            }
            else if (speech.Contains("joy"))
            {
                Say("Ah, joy! It's the heart of every jest. But joy is fleeting; one must find it in the little things.");
            }
            else if (speech.Contains("things"))
            {
                Say("Yes, the little things matter. In the grand scheme of life, it's the small joys that make it worthwhile.");
            }
            else if (speech.Contains("small"))
            {
                Say("Indeed! Small things can have great significance. For instance, have you ever thought about the magic of a single smile?");
            }
            else if (speech.Contains("smile"))
            {
                Say("A smile can turn the mundane into something marvelous. Keep smiling, traveler!");
            }
            else if (speech.Contains("mundane"))
            {
                Say("The mundane is often overlooked, but it's where the magic hides. Embrace it, and you'll find hidden wonders.");
            }
            else if (speech.Contains("wonders"))
            {
                Say("Wonders are everywhere, if you know where to look. Sometimes, it's the unexpected that brings the greatest joy.");
            }
            else if (speech.Contains("unexpected"))
            {
                Say("The unexpected can be quite delightful. It's the surprises that often leave the biggest smiles.");
            }
            else if (speech.Contains("surprises"))
            {
                Say("Ah, surprises! They are the essence of jest. Speaking of which, are you ready for your reward?");
            }
            else if (speech.Contains("jest"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I cannot reward you just yet. Return later and you might receive a merry prize.");
                }
                else
                {
                    Say("You've earned a token of jest and whimsy. Take this Jester's Giggling Chest as a reward!");
                    from.AddToBackpack(new JestersGigglingChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public JesterPuckery(Serial serial) : base(serial) { }

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
