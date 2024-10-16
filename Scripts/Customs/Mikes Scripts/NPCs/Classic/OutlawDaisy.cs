using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Outlaw Daisy")]
    public class OutlawDaisy : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OutlawDaisy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Outlaw Daisy";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 105;
            Int = 70;
            Hits = 70;

            // Appearance
            AddItem(new LeatherSkirt() { Hue = 1178 });
            AddItem(new FemaleLeatherChest() { Hue = 1178 });
            AddItem(new Bandana() { Hue = 1185 });
            AddItem(new Boots() { Hue = 1152 });
            AddItem(new Dagger() { Name = "Daisy's Dagger" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true); // true for female
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
                Say("Howdy, stranger! They call me Outlaw Daisy.");
            }
            else if (speech.Contains("health"))
            {
                Say("I reckon I'm as healthy as a cactus in the desert.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? Well, I'm a law-abiding outlaw.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Listen up, partner! True grit ain't about shootin' first; it's about outwittin' the varmints. You savvy?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Are you quick on the draw, or do you prefer a peaceful parley?");
            }
            else if (speech.Contains("words"))
            {
                Say("Well, partner, remember that it ain't always the quickest gun that wins the fight. Sometimes, words can be mightier than bullets.");
            }
            else if (speech.Contains("outlaw"))
            {
                Say("Ah, the tales they spin 'bout me are plenty. Some true, some mere whispers of the wind. If you're lookin' for adventure, maybe I can point you in the right direction.");
            }
            else if (speech.Contains("cactus"))
            {
                Say("A cactus stands tall, storing water and weathering the harshest conditions. Much like me, it's got its thorns, but it's also got its beauty. Ever seen a cactus flower bloom?");
            }
            else if (speech.Contains("law"))
            {
                Say("A paradox, ain't it? I may have broken a law or two in my time, but I've always had my reasons. There's a fine line between right and wrong in these parts, and sometimes the law ain't always just.");
            }
            else if (speech.Contains("tales"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The stories they tell about me? Some of 'em are true. Like the time I outsmarted the sheriff and his posse, using nothin' but my wits and a rusty harmonica. Now, that's a tale worth hearin'. As a token of appreciation for your curiosity, take this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("flower"))
            {
                Say("The desert flower is a symbol of hope and resilience. Blooms so rare and beautiful in such a harsh environment. It's a reminder that even in the toughest times, there's always a ray of beauty and hope.");
            }
            else if (speech.Contains("paradox"))
            {
                Say("Life out here is full of contradictions. Like a snake that heals with its venom, or a river that gives life but can also take it away. You've got to learn to navigate the complexities to survive.");
            }

            base.OnSpeech(e);
        }

        public OutlawDaisy(Serial serial) : base(serial) { }

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
