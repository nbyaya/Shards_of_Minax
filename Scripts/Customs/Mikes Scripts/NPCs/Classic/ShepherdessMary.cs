using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherdess Mary")]
    public class ShepherdessMary : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdessMary() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherdess Mary";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 65;
            Int = 55;
            Hits = 65;

            // Appearance
            AddItem(new Kilt() { Hue = 1115 }); // Skirt with specified hue
            AddItem(new Shirt() { Hue = 1114 }); // Shirt with specified hue
            AddItem(new Sandals() { Hue = 0 }); // Sandals with hue 0
            AddItem(new ShepherdsCrook() { Name = "Shepherdess Mary's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("Oh, it's you. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Healthy as a sheep, unlike you, I suppose.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I herd sheep. It's as dull as it sounds.");
            }
            else if (speech.Contains("sheep"))
            {
                Say("Do you even understand what real work is?");
            }
            else if (speech.Contains("yes") && speech.Contains("sheep"))
            {
                Say("Well, aren't you a quick learner. Do you have any real questions?");
            }
            else if (speech.Contains("mary"))
            {
                Say("They call me Mary, the shepherdess. Not much to it, really. Just Mary.");
            }
            else if (speech.Contains("herd"))
            {
                Say("Herding sheep might seem boring to the likes of you, but it requires a sharp eye and a sharper wit. Ever heard of the infamous black sheep?");
            }
            else if (speech.Contains("shepherdess"))
            {
                Say("Yes, a shepherdess. Most think it's a man's job, but I've proven them wrong time and time again. I've even outwitted bandits who tried to steal my flock.");
            }
            else if (speech.Contains("wilderness"))
            {
                Say("The wilderness is a harsh mistress. There are wolves, thieves, and worse. But I've befriended a few animals that help keep me and my sheep safe.");
            }
            else if (speech.Contains("black"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the legendary black sheep. Some say it brings fortune, others curse. I found one once, and for helping me, I'll share a piece of its wool with you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("wolves"))
            {
                Say("Wolves can be a menace, especially during the winter months. But there's one old wolf, Greymane, who has become an unexpected ally.");
            }
            else if (speech.Contains("faced"))
            {
                Say("It's a tough world out there. If you haven't faced a bandit yet, it's only a matter of time. Always stay prepared.");
            }
            else if (speech.Contains("greymane"))
            {
                Say("Greymane, the old wolf, once tried to attack my flock. But after I saved him from a trap, he's been nothing but loyal. Nature has its own ways.");
            }

            base.OnSpeech(e);
        }

        public ShepherdessMary(Serial serial) : base(serial) { }

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
