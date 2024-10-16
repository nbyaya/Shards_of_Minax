using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Elbow Ellie")]
    public class ElbowEllie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ElbowEllie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Elbow Ellie";
            Body = 0x191; // Human female body

            // Stats
            Str = 95;
            Dex = 60;
            Int = 70;
            Hits = 95;

            // Appearance
            AddItem(new Kilt() { Hue = 64 });
            AddItem(new Doublet() { Hue = 38 });
            AddItem(new Boots() { Hue = 38 });
            AddItem(new PlateGloves() { Name = "Ellie's Elbow Pads" });

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
                Say("I am Elbow Ellie, the master of the wrestling ring!");
            }
            else if (speech.Contains("health"))
            {
                Say("Fit as a fiddle!");
            }
            else if (speech.Contains("job"))
            {
                Say("I grapple foes in the ring!");
            }
            else if (speech.Contains("ring"))
            {
                Say("Honor is not just in winning, but in how we face our battles. Do you fight with honor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("True honor lies not in the outcome but in the intent. Remember this.");
            }
            else if (speech.Contains("wrestling"))
            {
                Say("Ah, wrestling! It's not just about physical strength, but mental agility too. Ever thought about trying it out?");
            }
            else if (speech.Contains("training"))
            {
                Say("Every day, I train hard to stay at the top of my game. Dedication is key. Interested in some tips?");
            }
            else if (speech.Contains("tips"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Alright! A balanced diet, regular exercise, and a strong mindset. And if you ever face an opponent, always look for their weak spot. For your dedication, take this reward.");
                    from.AddToBackpack(new GlovesOfCommand()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("reward"))
            {
                Say("You have earned this for showing genuine interest in the art of wrestling. Use it wisely.");
                from.AddToBackpack(new GlovesOfCommand()); // Give the second reward
            }
            else if (speech.Contains("foes"))
            {
                Say("I've faced many in my time. Some for glory, some for honor, and others, just to test my limits. Every opponent teaches you something.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Ah, honor! It's the code I live by. It's more than just a word, it's a way of life.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Battles in the ring have taught me so much. Every bruise, every defeat, every victory. They all have a story.");
            }
            else if (speech.Contains("story"))
            {
                Say("Ah, stories! I have many. From my first match, to my fiercest rivalry. Wrestling has given me memories to cherish.");
            }
            else if (speech.Contains("rivalry"))
            {
                Say("Rivalries are what make the sport exciting. Pushing each other to the limits, challenging, and growing together.");
            }

            base.OnSpeech(e);
        }

        public ElbowEllie(Serial serial) : base(serial) { }

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
