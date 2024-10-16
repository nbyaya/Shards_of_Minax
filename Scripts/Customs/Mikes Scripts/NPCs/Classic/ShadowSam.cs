using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shadow Sam")]
    public class ShadowSam : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShadowSam() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shadow Sam";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 80;
            Int = 100;
            Hits = 90;

            // Appearance
            AddItem(new NinjaTabi() { Hue = 1175 });
            AddItem(new ShortPants() { Hue = 1109 });
            AddItem(new AssassinSpike());
            AddItem(new LeatherNinjaBelt() { Hue = 1109 });

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
                Say("I am Shadow Sam, a rogue in the shadows!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm always in the shadows, so my health is a secret.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Well, let's just say I procure things that others can't.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is a fluid concept, my friend. What's your take on it?");
            }
            else if (speech.Contains("trust"))
            {
                Say("Your answer tells me a lot about you. Be careful who you trust, my friend.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("The shadows hide many secrets, and sometimes, treasures for those who dare to look.");
            }
            else if (speech.Contains("secret"))
            {
                Say("Every rogue has their secrets, some of which can be lethal. I've encountered many dangers in my line of work.");
            }
            else if (speech.Contains("procure"))
            {
                Say("Procurement is an art, especially when the item in question is... unique. Ever heard of the Whispering Gem?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, curious about the secrets I've uncovered? Let's just say not everything is as it seems in this world.");
            }
            else if (speech.Contains("dangers"))
            {
                Say("One of the most dangerous missions I undertook was in the Caves of Despair. That place is not for the faint-hearted.");
            }
            else if (speech.Contains("gem"))
            {
                Say("Ah, you've heard of it? It's a rare artifact said to grant immense power to its possessor. I might have a lead on its whereabouts... If you help me, I might just share that info, and perhaps a little reward.");
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
                    Say("Ah, you've proven yourself brave. Here's a little something for your troubles.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("world"))
            {
                Say("This world is filled with both wonder and peril. It's the balance of these that makes a rogue's life thrilling.");
            }
            else if (speech.Contains("caves"))
            {
                Say("Those caves are rumored to be cursed. Many have gone in, few have returned. But those who do come back with tales of unimaginable riches.");
            }

            base.OnSpeech(e);
        }

        public ShadowSam(Serial serial) : base(serial) { }

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
