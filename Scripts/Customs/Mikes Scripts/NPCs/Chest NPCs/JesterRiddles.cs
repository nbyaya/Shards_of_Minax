using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Riddles")]
    public class JesterRiddles : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterRiddles() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Riddles";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new JesterHat() { Hue = Utility.RandomNondyedHue() });
            AddItem(new JesterSuit() { Hue = Utility.RandomBrightHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomBrightHue() });
            AddItem(new Shoes() { Hue = Utility.RandomBrightHue() });
            AddItem(new Cloak() { Hue = Utility.RandomBrightHue() });

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
                Say("Ah, greetings! I am Jester Riddles, the master of merriment and mischief!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Why, to bring laughter and joy to all who seek a smile!");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, I'm as spry as a dancing sprite, thank you for asking!");
            }
            else if (speech.Contains("jester"))
            {
                Say("A jester I am, and jesting is my game! From jest to jest, I weave my fame.");
            }
            else if (speech.Contains("joking"))
            {
                Say("Joking is the heart of my work. A jest for a jest, a smile for a smile!");
            }
            else if (speech.Contains("laughing"))
            {
                Say("Laughing is the melody of joy. It dances in the air and lifts the spirit!");
            }
            else if (speech.Contains("merriment"))
            {
                Say("Merriment is the essence of celebration. It fills the heart with boundless joy!");
            }
            else if (speech.Contains("celebration"))
            {
                Say("Celebration is the grand display of cheer and happiness. It marks moments of joy and triumph!");
            }
            else if (speech.Contains("cheer"))
            {
                Say("Cheer is a gift that keeps on giving. It uplifts the spirit and warms the heart.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure, you say? A prize for those who unravel the jest and jest again!");
            }
            else if (speech.Contains("reward"))
            {
                Say("Prove your wit and humor, and a delightful reward you shall receive.");
            }
            else if (speech.Contains("wit"))
            {
                Say("Wit is the spark that keeps the laughter flowing. Share a jest, and I shall share a jest with you.");
            }
            else if (speech.Contains("jest"))
            {
                Say("A jest for your wit, and wit for a jest. A playful exchange, indeed!");
            }
            else if (speech.Contains("humor"))
            {
                Say("Humor is the art of making light of life's trials. A well-timed jest can brighten the darkest day!");
            }
            else if (speech.Contains("jokes"))
            {
                Say("Jokes are like little bursts of joy, each one a tiny treasure of laughter!");
            }
            else if (speech.Contains("fools"))
            {
                Say("Fools we may be in jest, but even a fool can bring joy to the heart!");
            }
            else if (speech.Contains("jester's jamboree"))
            {
                Say("The Jester's Jamboree Chest holds the essence of merriment. Seek the treasure within!");
            }
            else if (speech.Contains("chest"))
            {
                Say("The chest is filled with laughter and delight. But only those who play along can unlock its secrets!");
            }
            else if (speech.Contains("unlock"))
            {
                Say("Unlocking the chest requires both wit and humor. Prove your playful spirit!");
            }
            else if (speech.Contains("playful"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The laughter has yet to fade, and I have no reward for you at this time. Return when the jest is fresh!");
                }
                else
                {
                    Say("Your wit and humor are as bright as a jester's cap! For your playful spirit, accept this Jester's Jamboree Chest!");
                    from.AddToBackpack(new JestersJamboreeChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("playful spirit"))
            {
                Say("A playful spirit is the heart of a true jester. It dances with joy and laughter!");
            }

            base.OnSpeech(e);
        }

        public JesterRiddles(Serial serial) : base(serial) { }

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
