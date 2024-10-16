using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Swashbuckling Sally")]
    public class SwashbucklingSally : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SwashbucklingSally() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Swashbuckling Sally";
            Body = 0x191; // Human female body

            // Stats
            Str = 130;
            Dex = 65;
            Int = 25;
            Hits = 90;

            // Appearance
            AddItem(new TricorneHat() { Hue = 1175 });
            AddItem(new FancyShirt() { Hue = 1910 });
            AddItem(new ShortPants() { Hue = 2120 });
            AddItem(new Boots() { Hue = 1172 });
            AddItem(new Longsword() { Name = "Sally's Rapier" });

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
                Say("Arr matey! I be Swashbuckling Sally, the fiercest pirate on these seas!");
            }
            else if (speech.Contains("health"))
            {
                Say("I've weathered many a storm, and I be hearty as a kraken!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, ye see, is to plunder the high seas and search for buried treasures!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("But remember, the true treasure be not just gold and jewels, but the thrill of adventure and the camaraderie of a loyal crew!");
            }
            else if (speech.Contains("heart"))
            {
                Say("Arr matey, do ye have the heart of a true buccaneer?");
            }
            else if (speech.Contains("sally"))
            {
                Say("Aye, many tales have been spun about me exploits, but only the bravest dare ask for the true tales. Do ye wish to hear them?");
            }
            else if (speech.Contains("kraken"))
            {
                Say("Aye, the kraken be a mighty beast of the deep, and I've faced it more times than I can count! Have ye ever seen such a creature, or do you just believe the myths?");
            }
            else if (speech.Contains("plunder"))
            {
                Say("Plunderin' be a dangerous job, but the rewards be worth the risks. I've got me hands on some treasures that many believe to be mere legends. Have you heard of the Lost Sapphire of the Seas?");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Adventure is the lifeblood of any true pirate. Each new horizon brings new challenges and new chances for glory. I've had me fair share of adventures. Ever heard of the Ghost Ship of Tortuga?");
            }
            else if (speech.Contains("buccaneer"))
            {
                Say("To be a buccaneer is to embrace the unknown and challenge the odds. If ye prove yourself worthy, I might even share a bit of me own bounty with ye as a reward.");
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
                    Say("For proving your mettle, here's something special for ye. May it serve ye well on your own adventures!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SwashbucklingSally(Serial serial) : base(serial) { }

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
