using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria Shadowsong")]
    public class LyriaShadowsong : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyriaShadowsong() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria Shadowsong";
            Body = 0x191; // Female body

            // Stats
            Str = 80;
            Dex = 120;
            Int = 70;
            Hits = 65;

            // Appearance
            AddItem(new NinjaTabi() { Hue = 1900 });
            AddItem(new LongPants() { Hue = 1900 });
            AddItem(new FancyShirt() { Hue = 1900 });
            AddItem(new LeatherGorget() { Hue = 1900 });
            AddItem(new Kama() { Name = "Lyria's Dagger" });

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
                Say("I am Lyria Shadowsong, the outcast.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is a constant reminder of the darkness that surrounds me.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I dwell in the shadows, shunned by my kin and society.");
            }
            else if (speech.Contains("darkness"))
            {
                Say("Do you seek power or revenge, stranger?");
            }
            else if (speech.Contains("power"))
            {
                Say("So, you also seek to embrace the shadows. Be warned, their power comes at a cost.");
            }
            else if (speech.Contains("outcast"))
            {
                Say("I was once a part of the prestigious Shadowsong clan. But my thirst for knowledge beyond our ways branded me an outcast.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows are more than just a hiding place for me. They are allies, sources of strength, and occasionally, tormentors.");
            }
            else if (speech.Contains("revenge"))
            {
                Say("Revenge is a powerful motivator. But against whom do you seek it?");
            }
            else if (speech.Contains("clan"))
            {
                Say("The Shadowsong clan was known for its mastery over the elements and its close bond with nature. I dared to defy their strict traditions.");
            }
            else if (speech.Contains("forbidden"))
            {
                Say("The forbidden magics I delved into were ancient and powerful. Few dared to even speak of them, let alone practice. But if you are truly interested, I might teach you... for a price.");
            }
            else if (speech.Contains("tormentors"))
            {
                Say("The shadows can be unpredictable. While they can grant immense power, they can also betray and torment the user. Tread with caution.");
            }
            else if (speech.Contains("against"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("My vendetta is against those in the clan who shunned me. But if your enemy aligns with mine, perhaps we can assist each other. Take this as a token of potential partnership.");
                    from.AddToBackpack(new MusicianshipAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LyriaShadowsong(Serial serial) : base(serial) { }

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
