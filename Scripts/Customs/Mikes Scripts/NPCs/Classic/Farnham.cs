using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Farnham")]
    public class Farnham : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Farnham() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Farnham";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 30;
            Int = 30;
            Hits = 40;

            // Appearance
            AddItem(new LongPants() { Hue = 1105 });
            AddItem(new FancyShirt() { Hue = 1102 });
            AddItem(new Boots() { Hue = 0 });
            AddItem(new Dagger() { Name = "Farnham's Drink" });

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
                Say("I'm Farnham, the drinker!");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, my head...");
            }
            else if (speech.Contains("job"))
            {
                Say("I used to be an adventurer like you... until I discovered the ale!");
            }
            else if (speech.Contains("ale"))
            {
                Say("Do you enjoy the taste of ale, friend?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ah, good choice! Ale makes everything better, don't you think?");
            }
            else if (speech.Contains("adventurer"))
            {
                Say("Ah, those were the days! Battling dragons, finding treasures, and meeting other brave souls. But ale... it's a different kind of adventure.");
            }
            else if (speech.Contains("dragon"))
            {
                Say("Have you faced a dragon? They're mighty beasts. Their roar alone could shake the ground. Lost a good friend to one. But at the tavern, tales of dragons are best shared over a drink!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("I once found a gleaming chalice in a dungeon. Thought it'd make me rich, but turned out it was just a really fancy cup for my ale!");
            }
            else if (speech.Contains("no"))
            {
                Say("To each their own. But remember, friend, sometimes it's the little joys that help us forget the burdens of the world.");
            }
            else if (speech.Contains("headache"))
            {
                Say("You wouldn't happen to know a good remedy for these constant headaches, would you?");
            }
            else if (speech.Contains("herbs"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, herbs! I've tried some. Mixed them with ale once. Didn't help the headache, but it sure made the ale interesting!");
                    from.AddToBackpack(new Gold(100)); // Give a reward (replace with appropriate item)
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tavern"))
            {
                Say("The tavern has its own tales. From lost loves to ghostly visitors. And of course, many a song about the wonders of ale!");
            }
            else if (speech.Contains("song"))
            {
                Say("There was this one time, a group of us sang till dawn. Can't remember the words, but the melody... it lingers.");
            }

            base.OnSpeech(e);
        }

        public Farnham(Serial serial) : base(serial) { }

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
