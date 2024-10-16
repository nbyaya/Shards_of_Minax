using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Eleanor")]
    public class LadyEleanor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyEleanor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Eleanor";
            Body = 0x191; // Human female body

            // Stats
            Str = 150;
            Dex = 62;
            Int = 23;
            Hits = 105;

            // Appearance
            AddItem(new Kilt() { Hue = 1200 });
            AddItem(new ChainChest() { Hue = 1200 });
            AddItem(new PlateHelm() { Hue = 1200 });
            AddItem(new PlateGloves() { Hue = 1200 });
            AddItem(new Boots() { Hue = 1200 });
			
			Hue = Race.RandomSkinHue();
			HairItemID = Race.RandomHair(Female);
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
                Say("I am Eleanor of Aquitaine, once a queen, now a prisoner of this wretched realm.");
            }
            else if (speech.Contains("health"))
            {
                Say("Society has cast me aside, treating me like a relic of the past. My health? It withers just as my spirit does.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job'? It was once to rule a kingdom, to lead armies, to shape history. Now, it is to languish in this forsaken place.");
            }
            else if (speech.Contains("society"))
            {
                Say("Do you think yourself any different? Are you trapped by the whims of fate or society's chains?");
            }
            else if (speech.Contains("queen"))
            {
                Say("Once, I stood by kings and influenced the course of nations. A queen's crown is heavy, laden with responsibilities and expectations.");
            }
            else if (speech.Contains("withers"))
            {
                Say("Time has not been kind to me. The beauty and vitality of youth have faded, but my memories remain sharp as ever.");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("My kingdom was vast, with lands stretching far and wide. It was a realm of prosperity, culture, and power. But even great kingdoms fall, and fate can be cruel.");
            }
            else if (speech.Contains("chains"))
            {
                Say("Yes, chains, both seen and unseen, bind us all. Perhaps you could break these chains for me? Do this, and I shall grant you a reward from my past treasures.");
            }
            else if (speech.Contains("responsibilities"))
            {
                Say("As a queen, my duties were endless. From diplomatic negotiations to overseeing the well-being of my subjects, every decision carried weight.");
            }
            else if (speech.Contains("sharp"))
            {
                Say("The events of my life play in my mind like a vivid tapestry. Wars, alliances, betrayals... every moment is etched in my heart.");
            }
            else if (speech.Contains("cruel"))
            {
                Say("Fate's whims can change the course of lives and nations. I've seen empires rise and fall, all at the hands of destiny.");
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
                    Say("My treasures were many, and while most have been lost to time, a few precious items remain. Aid me, and one shall be yours. A sample for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LadyEleanor(Serial serial) : base(serial) { }

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
