using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Niko")]
    public class Niko : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Niko() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Niko";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 65;
            Int = 45;
            Hits = 85;

            // Appearance
            AddItem(new ShortPants() { Hue = 1230 });
            AddItem(new Shirt() { Hue = 1230 });
            AddItem(new Boots() { Hue = 1230 });
            AddItem(new Pickaxe() { Name = "Niko's Pickaxe" });

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
                Say("Greetings, exile.");
            }
            else if (speech.Contains("health"))
            {
                Say("I've seen worse days.");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm a Delve explorer, hunting for ancient treasures in the darkness.");
            }
            else if (speech.Contains("battles"))
            {
                Say("The darkness is unrelenting. Have you faced it?");
            }
            else if (speech.Contains("yes") && speech.Contains("battles"))
            {
                Say("In the abyss, hesitation can lead to doom. Will you embrace the darkness?");
            }
            else if (speech.Contains("niko"))
            {
                Say("My name is Niko. Once a dweller from the north, now I've dedicated my life to delving.");
            }
            else if (speech.Contains("worse"))
            {
                Say("The darkness of the abyss takes a toll on one's health, but the rewards can sometimes make it worth the risks.");
            }
            else if (speech.Contains("delve"))
            {
                Say("The mysteries of the abyss are many. Most are deadly, but a few, a very few, hide treasures beyond imagining.");
            }
            else if (speech.Contains("north"))
            {
                Say("Ah, the north. It was a land of ice and snow. I left behind family and friends in search of greater truths in the abyss.");
            }
            else if (speech.Contains("rewards"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Some rewards are treasures of gold and jewels. Others are ancient artifacts with powers unknown. Once, I found this...");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("mysteries"))
            {
                Say("There are whispers of ancient civilizations, lost rituals, and forbidden magic deep within the abyss.");
            }
            else if (speech.Contains("family"))
            {
                Say("I often wonder how they fare, and if they would even recognize the man I've become after all these years in the abyss.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("The artifacts are a testament to the greatness of the civilizations that once thrived here. Many are cursed, while others can grant unimaginable power.");
            }
            else if (speech.Contains("rituals"))
            {
                Say("Legends say some rituals can summon entities from the void or even grant eternal life. But all at a cost.");
            }
            else if (speech.Contains("years"))
            {
                Say("The years have been harsh, but they've also taught me much. Experience is the greatest reward one can earn in the abyss.");
            }
            else if (speech.Contains("cursed"))
            {
                Say("I've seen brave souls fall victim to the allure of cursed artifacts. Greed can blind one to the dangers of the abyss.");
            }
            else if (speech.Contains("legends"))
            {
                Say("I've heard tales of a legendary city deep within the abyss, untouched by time and full of untold riches.");
            }
            else if (speech.Contains("experience"))
            {
                Say("With each delve, I've learned to respect the abyss and the dangers it holds. Those who rush in unprepared seldom return.");
            }
            else if (speech.Contains("greed"))
            {
                Say("Greed can be a powerful motivator, but in the abyss, it's often a deadly one. Too many have lost their way chasing shadows of wealth.");
            }
            else if (speech.Contains("city"))
            {
                Say("Some say the city is a mirage, while others believe it's the final resting place of an ancient king. Only the bravest or most foolish seek it.");
            }
            else if (speech.Contains("respect"))
            {
                Say("The abyss demands respect. Those who understand this live to tell their tales. Those who don't, well... their tales are told by others.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows can be deceiving. In the abyss, they might hide treasures, but more often, they hide dangers that can end an explorer's journey prematurely.");
            }
            else if (speech.Contains("king"))
            {
                Say("Legends of the king tell of a wise and just ruler, whose riches were unmatched. But like all in the abyss, his fate remains a mystery.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Over campfires, many share their tales of the abyss. Some are of triumph, others of loss. Each tale is a lesson for those willing to listen.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey into the abyss is a test of one's will and determination. Many start, few finish, but all are changed by the experience.");
            }

            base.OnSpeech(e);
        }

        public Niko(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
