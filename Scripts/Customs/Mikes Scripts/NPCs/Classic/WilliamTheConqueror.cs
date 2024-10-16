using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of William the Conqueror")]
    public class WilliamTheConqueror : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public WilliamTheConqueror() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "William the Conqueror";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 70;
            Hits = 100;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1910 });
            AddItem(new ChainChest() { Hue = 1910 });
            AddItem(new ChainCoif() { Hue = 1910 });
            AddItem(new PlateGloves() { Hue = 1910 });
            AddItem(new Boots() { Hue = 1910 });
            AddItem(new VikingSword() { Name = "William's Sword" });

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
                Say("I am William the Conqueror, the one they fear and despise.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Bah! As if it matters to someone like me.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? To conquer and rule with an iron fist, of course.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Do you think you have what it takes to challenge my might?");
            }
            else if (speech.Contains("yes") && speech.Contains("challenge"))
            {
                Say("Oh, you dare to challenge me? How amusing! Prove your worth, or crawl away like the rest.");
            }
            else if (speech.Contains("fear"))
            {
                Say("They fear me because of the battles I've won and the lands I've taken. My reputation precedes me.");
            }
            else if (speech.Contains("matters"))
            {
                Say("To most, health might be a concern. But in the face of power and ambition, such trivial matters fade.");
            }
            else if (speech.Contains("conquer"))
            {
                Say("Conquering is not just about taking lands, it's about imposing one's will and establishing dominance.");
            }
            else if (speech.Contains("battles"))
            {
                Say("I've fought countless battles, each one a testament to my strength and strategy. Many have tried to oppose me, all have failed.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power is not given, it is taken. And once you have it, many will try to take it from you. Guard it well.");
            }
            else if (speech.Contains("dominance"))
            {
                Say("Dominance is not just about ruling, but about earning respect and instilling loyalty among your subjects.");
            }
            else if (speech.Contains("strategy"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Share your insights on strategy, and if they impress me, you might earn a token of my appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("guard"))
            {
                Say("Guarding one's power requires both physical might and mental fortitude. It's a never-ending vigilance against those who seek to undermine you.");
            }
            else if (speech.Contains("loyalty"))
            {
                Say("Loyalty is hard to come by. Those who show true loyalty are worth their weight in gold.");
            }
            else if (speech.Contains("insights"))
            {
                Say("Share your insights on strategy, and if they impress me, you might earn a token of my appreciation.");
            }

            base.OnSpeech(e);
        }

        public WilliamTheConqueror(Serial serial) : base(serial) { }

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
