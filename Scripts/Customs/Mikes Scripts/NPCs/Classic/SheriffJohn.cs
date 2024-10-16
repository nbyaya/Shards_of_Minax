using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sheriff John")]
    public class SheriffJohn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SheriffJohn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sheriff John";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 90;
            Int = 65;
            Hits = 85;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1160 });
            AddItem(new LeatherChest() { Hue = 1160 });
            AddItem(new WideBrimHat() { Hue = 1158 });
            AddItem(new Boots() { Hue = 1140 });
            AddItem(new BodySash() { Name = "Town's Badge" });
            AddItem(new FireballWand() { Name = "John's Revolver" });

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
                Say("What do you want, stranger?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Ain't none of your business, but I'm survivin'.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm the law around these parts, and I ain't too thrilled about it.");
            }
            else if (speech.Contains("battles"))
            {
                Say("You think you can do a better job than me?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Well, ain't you a cocky one. Let's see if you can handle a posse of troublemakers.");
            }
            else if (speech.Contains("john"))
            {
                Say("Sheriff John's the name. Not many people remember it, being focused more on the badge than the man wearing it.");
            }
            else if (speech.Contains("business"))
            {
                Say("You see, this job can wear you down. The stress, the sleepless nights, the battles... It ain't all sunshine and rainbows.");
            }
            else if (speech.Contains("law"))
            {
                Say("Been Sheriff for a long time. Seen this town go through its ups and downs. It's the outsiders, the outlaws, they're the real problem.");
            }
            else if (speech.Contains("badge"))
            {
                Say("This badge represents order in this chaotic land. But sometimes, it feels like it weighs a ton, holding me down. Still, it's a duty I take seriously. If you prove yourself, maybe there's a reward for folks like you.");
            }
            else if (speech.Contains("stress"))
            {
                Say("Ever since the gold rush, we've had bandits, thieves, and no-goods trying to take a piece. Keeping peace is a daily struggle.");
            }
            else if (speech.Contains("outlaws"))
            {
                Say("Too many of them skulking about, causing trouble. But there's one, goes by the name of Black Bart. He's the worst of them, a real menace. You heard of him?");
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
                    Say("For those who stand by the law and help bring justice, there's always something in the coffers. Help me catch an outlaw or two, and you'll see what I mean. A sample.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("gold"))
            {
                Say("When the gold started flowing, so did the greed. Folks from all over came seeking their fortune. Some found it, most didn't. But it's the trouble they brought that lingers.");
            }
            else if (speech.Contains("bart"))
            {
                Say("Aye, that Black Bart's a cunning one. Been eluding my grasp for years now. They say he hides out in the Dead Man's Canyon. If you ever cross paths, be wary.");
            }
            else if (speech.Contains("fortune"))
            {
                Say("Many have sought their fortune here, only to leave broken or worse. But there are tales of hidden treasures, waiting for the brave or the foolish.");
            }
            else if (speech.Contains("canyon"))
            {
                Say("A treacherous place, filled with traps and old legends. But if you do manage to find Black Bart there and bring him to justice, well... Let's just say there's a hefty bounty on his head.");
            }

            base.OnSpeech(e);
        }

        public SheriffJohn(Serial serial) : base(serial) { }

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
