using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cowgirl Annie")]
    public class CowgirlAnnie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CowgirlAnnie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cowgirl Annie";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 100;
            Int = 60;
            Hits = 75;

            // Appearance
            AddItem(new LeatherSkirt() { Hue = 1190 });
            AddItem(new FemaleLeatherChest() { Hue = 1190 });
            AddItem(new WideBrimHat() { Hue = 1188 });
            AddItem(new Boots() { Hue = 1155 });
            AddItem(new FireballWand() { Name = "Annie's Rifle" });

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
                Say("Howdy there, partner! Name's Cowgirl Annie.");
            }
            else if (speech.Contains("health"))
            {
                Say("Well, I reckon I'm feelin' as fit as a fiddle!");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm a cowgirl, partner! I wrangle cattle and tend to this here ranch.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Well, partner, true courage ain't just about guns and boots. It's about doin' right by folks. Do ya reckon y'all got courage in your heart?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Well, partner, it's mighty fine to hear that! Remember, in tough times, stand tall and do what's right!");
            }
            else if (speech.Contains("annie"))
            {
                Say("Y'know, my ma named me after her granny, a pioneer who crossed the great plains with just a horse and a dream.");
            }
            else if (speech.Contains("fit"))
            {
                Say("Thank ya kindly for askin'! I try to keep myself in tip-top shape. Out here in the ranch, you gotta be fit to keep up with the cattle and the chores.");
            }
            else if (speech.Contains("cowgirl"))
            {
                Say("It's a tough job, but someone's gotta do it. The ranch has been in my family for generations. My pa used to tell me tales of the old days when he was a young cowboy.");
            }
            else if (speech.Contains("pioneer"))
            {
                Say("That's right! My great-grandma was a true trailblazer. She faced many hardships, but her determination saw her through. She even found a hidden treasure once, but never told anyone where it was.");
            }
            else if (speech.Contains("chores"))
            {
                Say("Chores are never-ending 'round here. From mending fences to feeding the livestock. But it's all worth it when I see the sunset over the plains. If you ever want to lend a hand, I might have a little reward for ya.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Ah, pa's tales were the best! Stories of outlaws, gold rushes, and fierce storms. He always said every tale had a lesson, and it's up to us to find it.");
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
                    Say("You've got a kind heart, partner! Help me round up them stray cattle and I'll give you something special for your efforts.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("lesson"))
            {
                Say("One lesson I've always remembered is to never judge a book by its cover. Out here, even the toughest cowboys have a soft side, and the quietest critters can be the most fierce.");
            }

            base.OnSpeech(e);
        }

        public CowgirlAnnie(Serial serial) : base(serial) { }

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
