using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Harmonious Harry")]
    public class HarmoniousHarry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HarmoniousHarry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Harmonious Harry";
            Body = 0x190; // Human male body

            // Stats
            Str = 115;
            Dex = 70;
            Int = 85;
            Hits = 90;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1107 }); // FancyShirt with hue 1107
            AddItem(new LongPants() { Hue = 1912 }); // LongPants with hue 1912
            AddItem(new Boots() { Hue = 38 }); // Boots with hue 38
            AddItem(new LeatherGloves() { Name = "Harry's Harmony Gloves" });

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
                Say("I am Harmonious Harry, the bard with a tune as sour as life itself!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? As miserable as my melodies, I assure you!");
            }
            else if (speech.Contains("job"))
            {
                Say("Job? My 'job' is to serenade this wretched world with songs of despair.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valiant, you say? In this world of endless sorrow? I have my doubts. Prove your valor, if you dare.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! Words are wind, but I'll take your answer. Remember, in this world, even the sweetest songs carry bitterness within.");
            }
            else if (speech.Contains("bard"))
            {
                Say("Once, long ago, I played cheerful tunes, but the world's darkness changed me, and now my music reflects its true nature.");
            }
            else if (speech.Contains("miserable"))
            {
                Say("The gloom of this land weighs heavily upon me, yet it fuels the passion in my mournful songs.");
            }
            else if (speech.Contains("songs"))
            {
                Say("Each of my songs carries a tale, stories of lost hopes and broken dreams. Would you like to hear one?");
            }
            else if (speech.Contains("cheerful"))
            {
                Say("Ah, those were the days, when the world was bright and laughter echoed in every corner. But those days are long gone.");
            }
            else if (speech.Contains("gloom"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Gloom, you see, is a potent ingredient for art. Without it, my songs would lack depth. Here, take this token as a reminder of the duality of existence.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("tale"))
            {
                Say("I know of many tales, but there's one about a lost city that's my favorite. A city swallowed by shadows, never to be seen again.");
            }
            else if (speech.Contains("laughter"))
            {
                Say("Ah, laughter. A distant memory now. But even in its absence, it's proof that joy once existed in this world.");
            }
            else if (speech.Contains("art"))
            {
                Say("Art, in its truest form, is born from emotion, be it joy or sorrow. I channel my emotions, no matter how bleak, into my melodies.");
            }
            else if (speech.Contains("city"))
            {
                Say("The city I speak of was a beacon of hope, a jewel in the night. But, like all things, it faded into the annals of history.");
            }

            base.OnSpeech(e);
        }

        public HarmoniousHarry(Serial serial) : base(serial) { }

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
