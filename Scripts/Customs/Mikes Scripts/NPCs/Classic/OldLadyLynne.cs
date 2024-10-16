using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Old Lady Lynne")]
    public class OldLadyLynne : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OldLadyLynne() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Old Lady Lynne";
            Body = 0x191; // Human female body

            // Stats
            Str = 32;
            Dex = 28;
            Int = 25;
            Hits = 40;

            // Appearance
            AddItem(new Robe() { Hue = 11 }); // Robe with hue 11
            AddItem(new Sandals() { Hue = 1175 }); // Sandals with hue 1175

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Old Lady Lynne, a humble beggar.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Oh, it's frail, but what can an old beggar expect?");
            }
            else if (speech.Contains("job"))
            {
                Say("Begging is my only job, kind traveler.");
            }
            else if (speech.Contains("virtue") && speech.Contains("compassion"))
            {
                Say("Compassion is a virtue I hold dear. Do you have a coin to spare for a poor beggar?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, thank you, kind soul. May your compassion shine bright in your adventures.");
            }
            else if (speech.Contains("lynne"))
            {
                Say("Ah, my parents named me after the beautiful lake Lynne. It was said to have clear, shimmering waters.");
            }
            else if (speech.Contains("frail"))
            {
                Say("Yes, age has caught up with me. But it's not just age, it's also the memories of my past that weigh me down.");
            }
            else if (speech.Contains("begging"))
            {
                Say("It wasn't always this way. I was once a dancer, light on my feet and the belle of every ball.");
            }
            else if (speech.Contains("lake"))
            {
                Say("Lake Lynne was a serene place, surrounded by tall pines and singing birds. But it dried up many years ago, taking a piece of my heart with it.");
            }
            else if (speech.Contains("memories"))
            {
                Say("Ah, memories of lost love, failed endeavors, and the swift passage of time. But also memories of joy, laughter, and dancing.");
            }
            else if (speech.Contains("dancer"))
            {
                Say("Those were the days! Dancing under the moonlight, with music filling the air. Would you like to see a few steps? If so, maybe I could offer a little token in return for your kindness.");
            }
            else if (speech.Contains("dried"))
            {
                Say("The townspeople say it was due to a curse. But I believe it was a sign of nature telling us to cherish what we have. Sometimes, I dream of its return.");
            }
            else if (speech.Contains("love"))
            {
                Say("My heart still aches for the love I lost. He was a brave knight, off to battle, and never returned. His name was Sir Reginald.");
            }
            else if (speech.Contains("moonlight"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Here, let me show you a move. [She attempts a dance move but stumbles a bit]. Oh, it seems my legs aren't as nimble as they used to be. But for your patience, here's a little something.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the correct item to give as a reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public OldLadyLynne(Serial serial) : base(serial) { }

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
