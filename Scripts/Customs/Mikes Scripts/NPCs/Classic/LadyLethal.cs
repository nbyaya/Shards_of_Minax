using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Lethal")]
    public class LadyLethal : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyLethal() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Lethal";
            Body = 0x191; // Human female body

            // Stats
            Str = 110;
            Dex = 95;
            Int = 55;
            Hits = 75;

            // Appearance
            AddItem(new BoneLegs() { Hue = 2118 });
            AddItem(new BoneChest() { Hue = 2118 });
            AddItem(new BoneArms() { Hue = 2118 });
            AddItem(new BoneHelm() { Hue = 2118 });
            AddItem(new Boots() { Hue = 2118 });
            AddItem(new DoubleAxe() { Name = "Lady Lethal's Axe" });

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
                Say("I am Lady Lethal, the assassin extraordinaire!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is none of your business, but it's good enough for my line of work.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\" involves discreetly eliminating people who annoy me. You might call it an art form.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you have what it takes to thrive in this cutthroat world, or are you just another pawn?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, you think you're valiant? Tell me, what's the most devious thing you've ever done?");
            }
            else if (speech.Contains("assassin"))
            {
                Say("Yes, I've trained for years in the shadows, mastering the craft of taking lives. It's not just a job, but a passion. Do you despise me for it?");
            }
            else if (speech.Contains("art"))
            {
                Say("Indeed. The dance of death, the silent approach, the quick strike, and the unseen exit. It's all about elegance and precision. Ever considered joining the dark side?");
            }
            else if (speech.Contains("join"))
            {
                Say("The life of an assassin isn't for everyone. It's about loyalty to oneself, being cold-hearted, and having a knack for vanishing. If you prove your worth, there might be something in it for you.");
            }
            else if (speech.Contains("prove"))
            {
                Say("A test of your skills and resolve. Fetch me the medallion of Lord Valorian, and perhaps I'll consider you capable. Do this, and there's a reward awaiting.");
            }
            else if (speech.Contains("medallion"))
            {
                Say("A symbol of power and legacy, held by Lord Valorian. It won't be easy, but if you succeed, it'll prove your worth. Beware, though, for he is not defenseless.");
            }
            else if (speech.Contains("pawn"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Most people in this world are mere pawns, easily manipulated and disposed of. To be more, you must prove your cunning and strength. Have you ever manipulated someone to your advantage?");
                    from.AddToBackpack(new NinjitsuAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("no"))
            {
                Say("Ah, innocence or cowardice? Hard to tell. Either way, tread carefully around me.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Ah, eager for the prize? If you prove yourself and fetch the medallion, you'll receive something that many would kill for. But I won't reveal it just yet. Patience.");
            }

            base.OnSpeech(e);
        }

        public LadyLethal(Serial serial) : base(serial) { }

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
