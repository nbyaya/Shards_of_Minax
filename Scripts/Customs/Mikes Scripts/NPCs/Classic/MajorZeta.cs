using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Major Zeta")]
    public class MajorZeta : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MajorZeta() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Major Zeta";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 50;
            Int = 70;
            Hits = 60;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1908 });
            AddItem(new PlateChest() { Hue = 1908 });
            AddItem(new PlateHelm() { Hue = 1908 });
            AddItem(new PlateGloves() { Hue = 1908 });
            AddItem(new Boots() { Hue = 1908 });
            AddItem(new FireballWand() { Name = "Major Zeta's Rifle" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, I am Major Zeta, a scientist in these parts.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession is that of a scientist. I spend my days conducting experiments and unraveling the mysteries of the world.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("The pursuit of knowledge is a virtue in itself. Do you value knowledge as well?");
            }
            else if (speech.Contains("yes") && lastRewardTime < DateTime.UtcNow.AddMinutes(-10))
            {
                Say("Knowledge is indeed a treasure. It is through the pursuit of knowledge that we unlock the secrets of the world and better ourselves. Always seek to learn and grow.");
            }
            else if (speech.Contains("zeta"))
            {
                Say("Ah, you've heard of me before? I've been working in this region for quite some time, always in search of the next big discovery.");
            }
            else if (speech.Contains("perfect"))
            {
                Say("Yes, it's crucial for me to be in good health. My experiments often require utmost concentration and physical stamina. Have you ever been part of a scientific experiment?");
            }
            else if (speech.Contains("scientist"))
            {
                Say("Being a scientist isn't just about the experiments. It's about the insatiable curiosity to understand the universe. I'm currently working on a project related to ancient artifacts. Are you familiar with them?");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discoveries can change the world. Recently, I found a rare mineral with unique properties. It's still under study, but I believe it has potential. Would you be interested in such findings?");
            }
            else if (speech.Contains("experiment"))
            {
                Say("Experiments are the backbone of scientific progress. Some are simple, while others are complex and dangerous. I once had an experiment that almost went awry, but the results were astonishing. Curious about the results?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts are remnants of the past, each holding a story. I've collected many over the years, and some have powers that are not yet understood. If you assist me in my research, I might reward you with one such artifact.");
            }
            else if (speech.Contains("mineral"))
            {
                Say("This mineral is unlike any I've seen before. Its luminescence and energy properties suggest it might have extraterrestrial origins. Have you ever encountered anything like it?");
            }
            else if (speech.Contains("results"))
            {
                Say("The results of that particular experiment led to the creation of a potion with rejuvenating properties. I've kept it a secret until now, but for someone as curious as you, I might share a sample as a reward.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Rewards are not just materialistic. The greatest reward is the knowledge and experience gained. However, for your efforts and interest, I shall grant you a special token of appreciation.");
                    from.AddToBackpack(new ParryingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("extraterrestrial"))
            {
                Say("Ah, you're intrigued by the unknown as well! The universe is vast, and I believe there are many secrets yet to be unveiled. Studying extraterrestrial phenomena is just one of my many passions.");
            }

            base.OnSpeech(e);
        }

        public MajorZeta(Serial serial) : base(serial) { }

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
