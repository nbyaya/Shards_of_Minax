using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Wirt")]
    public class Wirt : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Wirt() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Wirt";
            Body = 0x190; // Human male body

            // Stats
            Str = 40;
            Dex = 60;
            Int = 50;
            Hits = 30;

            // Appearance
            AddItem(new ShortPants() { Hue = 1110 });
            AddItem(new Tunic() { Hue = 1125 });
            AddItem(new Boots() { Hue = 0 });
            AddItem(new Club() { Name = "Wirt's Peg Leg" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I'm Wirt, the one-legged wonder. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Who cares about that? I've got one leg!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Selling overpriced goods to naive adventurers, of course!");
            }
            else if (speech.Contains("one"))
            {
                Say("Think you're a hero, do you? Well, let me ask you this: Do you know what it's like to walk with only one leg?");
            }
            else if (speech.Contains("valiant") && lastRewardTime.AddMinutes(10) < DateTime.UtcNow)
            {
                Say("It's not about the limbs you have, but the heart within. Show me you're truly valiant, and I might have a reward for you.");
            }
            else if (speech.Contains("wert"))
            {
                Say("Ah, you've heard of me then? Not surprising. Many adventurers come to me for my unique... collection of items.");
            }
            else if (speech.Contains("leg"))
            {
                Say("Lost it in a dark cave when I was younger. That cave... it held treasures but at a terrible price.");
            }
            else if (speech.Contains("overpriced"))
            {
                Say("You'd be surprised how many rare artifacts I've gathered over the years. But remember, everything has its price.");
            }
            else if (speech.Contains("walk"))
            {
                Say("Walking might be a challenge, but it's given me a unique perspective on life. Makes you appreciate the little things more.");
            }
            else if (speech.Contains("limbs"))
            {
                Say("It's not about the limbs you have, but the heart within. Show me you're truly valiant, and I might have a reward for you.");
            }
            else if (speech.Contains("reward"))
            {
                if (lastRewardTime.AddMinutes(10) > DateTime.UtcNow)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, so you're interested? Very well. Complete a task for me, and I'll give you something special. But first, you must prove you're worthy. A sample for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Wirt(Serial serial) : base(serial) { }

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
