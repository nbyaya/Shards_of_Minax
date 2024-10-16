using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Miss Nixie")]
    public class MissNixie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MissNixie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Miss Nixie";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 70;
            Int = 90;
            Hits = 45;

            // Appearance
            AddItem(new LeatherArms() { Name = "Nixie's Leather Sleeves" });
            AddItem(new LeatherChest() { Name = "Nixie's Leather Bustier" });
            AddItem(new LeatherGloves() { Name = "Nixie's Leather Gloves" });
            AddItem(new LeatherLegs() { Name = "Nixie's Leather Skirt" });
            AddItem(new LeatherCap() { Name = "Nixie's Leather Cap" });
            AddItem(new Boots() { Name = "Nixie's Boots" });
            AddItem(new Cloak() { Name = "Nixie's Mortar and Pestle" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("I am Miss Nixie, the cunning thief!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in perfect health, always ready for a quick escape!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Well, let's just say I specialize in acquiring valuable items.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Thievery is an art, wouldn't you agree? Do you have what it takes to be a master thief?");
            }
            else if (speech.Contains("yes"))
            {
                Say("So, are you daring enough to join the ranks of thieves and outlaws?");
            }
            else if (speech.Contains("no"))
            {
                Say("Oh, a coward, are you? Well, not everyone has the cunning and audacity required for a life of thievery.");
            }
            else if (speech.Contains("nixie"))
            {
                Say("Ah, you've heard of me, haven't you? Stories of my escapades often reach the corners of the kingdom.");
            }
            else if (speech.Contains("escape"))
            {
                Say("It's not just about escaping, it's about knowing when to strike and when to vanish into the shadows. Have you ever hidden in the shadows?");
            }
            else if (speech.Contains("valuable"))
            {
                Say("You'd be surprised at the treasures that people just leave unattended. Ever wonder about the most precious item I've pilfered?");
            }
            else if (speech.Contains("escapades"))
            {
                Say("Once, I stole the crown jewels from the very castle itself! All while the king's guards were mere steps away.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows are a thief's best ally. With the right skills, one can blend in and become nearly invisible. Have you ever wanted to learn such skills?");
            }
            else if (speech.Contains("precious"))
            {
                Say("The most precious item I've ever taken? Ah, that would be the heart-shaped locket from the duchess. But not for its material value... Do you know the story behind it?");
            }
            else if (speech.Contains("skills"))
            {
                Say("If you're truly interested, I might be persuaded to teach you a trick or two. But it'll cost you. Are you willing to pay the price?");
            }
            else if (speech.Contains("price"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Very well. Give me a moment... Here, this trinket will aid you in your endeavors. Use it wisely and remember, discretion is key.");
                    from.AddToBackpack(new MaxxiaScroll()); // Assuming RobeOfTheJabba is a valid item in your server
                    lastRewardTime = DateTime.UtcNow;
                }
            }

            base.OnSpeech(e);
        }

        public MissNixie(Serial serial) : base(serial) { }

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
