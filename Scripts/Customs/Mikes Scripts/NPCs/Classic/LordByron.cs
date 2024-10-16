using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lord Byron")]
    public class LordByron : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LordByron() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lord Byron";
            Body = 0x190; // Human male body

            // Stats
            Str = 40;
            Dex = 80;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new LongPants() { Hue = 1908 });
            AddItem(new FancyShirt() { Hue = 1908 });
            AddItem(new Boots() { Hue = 1908 });
            AddItem(new Cloak() { Hue = 1908 });
            AddItem(new FeatheredHat() { Hue = 1908 });
            AddItem(new GoldRing() { Name = "Lord Byron's Signet Ring" });

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
                Say("I am Lord Byron, a knight in service of this wretched kingdom.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as miserable as my station in life.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, if you can call it that, is to serve a kingdom that cares not for honor or valor.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you fancy yourself a knight, valiant one?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If you dare call yourself valiant, then heed my advice: never flee, unless the need is dire.");
            }
            else if (speech.Contains("knight"))
            {
                Say("I took an oath to protect this kingdom, but the taint of corruption has made my role a heavy burden to bear.");
            }
            else if (speech.Contains("miserable"))
            {
                Say("My spirits are weighed down by the injustices I see daily, but I soldier on, hoping for better days.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor used to be the lifeblood of knights, but now, many have turned their backs on it for power and greed.");
            }
            else if (speech.Contains("valiant"))
            {
                Say("True valor is not just in facing battles, but in standing up for what's right, even when the whole world turns against you.");
            }
            else if (speech.Contains("oath"))
            {
                Say("When I took my oath, it was a sacred bond, a promise to serve the people and the land, no matter the cost.");
            }
            else if (speech.Contains("injustices"))
            {
                Say("Every day, I witness acts of cruelty and betrayal, yet I must turn a blind eye due to my position. It's a torment I wish on no one.");
            }
            else if (speech.Contains("greed"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Greed has taken many of my fellow knights, seducing them with promises of power and wealth. But at what cost to their souls? For your deeds, I wish to reward you. Please, accept this token of appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("right"))
            {
                Say("Always remember, standing up for the right cause, even in the face of adversity, is the mark of true valor.");
            }
            else if (speech.Contains("cruelty"))
            {
                Say("I've seen acts of cruelty that would chill your bones. But I've also seen acts of kindness that restore my faith in humanity.");
            }

            base.OnSpeech(e);
        }

        public LordByron(Serial serial) : base(serial) { }

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
