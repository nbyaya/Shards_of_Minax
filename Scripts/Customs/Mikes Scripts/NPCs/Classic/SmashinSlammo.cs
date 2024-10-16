using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Smashin' Slammo")]
    public class SmashinSlammo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SmashinSlammo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Smashin' Slammo";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Hits = 100;

            // Appearance
            AddItem(new Cloak() { Hue = 1153 });
            AddItem(new ThighBoots() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });
            AddItem(new TribalMask() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1153 });

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
                Say("I'm Smashin' Slammo, the wrestling sensation!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in top shape, ready for action!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to entertain the crowd with my wrestling moves!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you have what it takes to step into the ring with me?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Aye, they used to call me 'Slammo the Sensation' back in the day. I was unstoppable in the ring!");
            }
            else if (speech.Contains("action"))
            {
                Say("Always ready for the next match! I've been training hard, working on a new signature move!");
            }
            else if (speech.Contains("moves"))
            {
                Say("Have you heard of my famous 'Slammo Slam'? No one gets up from that one!");
            }
            else if (speech.Contains("unstoppable"))
            {
                Say("Those were the days... I faced some of the greatest wrestlers ever. Ever heard of Raging Rick?");
            }
            else if (speech.Contains("signature"))
            {
                Say("If you're really interested, I can show you a trick or two. Might even reward you if you impress me!");
            }
            else if (speech.Contains("slammo"))
            {
                Say("Ah, the legendary finisher! Took me years to perfect. Want a demonstration?");
            }
            else if (speech.Contains("rick"))
            {
                Say("Ah, Raging Rick, my fiercest rival. We had some intense bouts back in our prime!");
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
                    Say("Alright, show me your best wrestling stance. Ah, not bad! Here, take this as a token of my appreciation!");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with the actual item type and name
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SmashinSlammo(Serial serial) : base(serial) { }

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
