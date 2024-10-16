using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cowboy Bill")]
    public class CowboyBill : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CowboyBill() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cowboy Bill";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 95;
            Int = 55;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1175 });
            AddItem(new LeatherChest() { Hue = 1175 });
            AddItem(new WideBrimHat() { Hue = 1173 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new FireballWand() { Name = "Bill's Revolver" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            if (Insensitive.Contains(e.Speech, "name"))
            {
                Say("Howdy, partner! I'm Cowboy Bill, the quickest draw in these parts!");
            }
            else if (Insensitive.Contains(e.Speech, "health"))
            {
                Say("Well, I reckon I've seen better days, but I'm still kickin'!");
            }
            else if (Insensitive.Contains(e.Speech, "job"))
            {
                Say("My job? Well, I wrangle up them pesky varmints and keep this town safe!");
            }
            else if (Insensitive.Contains(e.Speech, "battles"))
            {
                Say("Listen up, partner! True grit ain't just about shootin'. It's about havin' the heart to face anythin' head-on. Are you tough as leather?");
            }
            else if (Insensitive.Contains(e.Speech, "yes") && e.Speech.Contains("tough"))
            {
                Say("Good answer, partner! Remember, when the goin' gets tough, you stand tall and never back down!");
            }
            else if (Insensitive.Contains(e.Speech, "cowboy"))
            {
                Say("You know, I wasn't always called Cowboy Bill. Used to go by a different name back when I was just a greenhorn.");
            }
            else if (Insensitive.Contains(e.Speech, "kickin"))
            {
                Say("Yep, still kickin' despite that one time a rattlesnake got the better of me. Lucky for me, Doc Thompson was around to help.");
            }
            else if (Insensitive.Contains(e.Speech, "varmints"))
            {
                Say("Them varmints can be real trouble, especially the coyotes that've been causin' a ruckus at the Johnson farm.");
            }
            else if (Insensitive.Contains(e.Speech, "greenhorn"))
            {
                Say("When I was a greenhorn, I learned the ropes from old Sheriff Tucker. He taught me everything I know about bein' a cowboy.");
            }
            else if (Insensitive.Contains(e.Speech, "rattlesnake"))
            {
                Say("That rattlesnake bite was somethin' fierce. But if you ever get bitten, there's an old remedy my grandma used to swear by. You interested?");
            }
            else if (Insensitive.Contains(e.Speech, "coyotes"))
            {
                Say("Those coyotes have been givin' the folks a hard time, stealin' chickens and whatnot. If you help round 'em up, there might be a reward in it for ya.");
            }
            else if (Insensitive.Contains(e.Speech, "sheriff"))
            {
                Say("Sheriff Tucker was a legend in this town. He wore the shiniest badge and had a heart of gold. Saved my hide more times than I can count.");
            }
            else if (Insensitive.Contains(e.Speech, "remedy"))
            {
                Say("My grandma's remedy was a mix of wild herbs and moonshine. Worked like a charm every time! If you ever need some, let me know.");
            }
            else if (Insensitive.Contains(e.Speech, "reward"))
            {
                if (DateTime.Now > lastRewardTime.AddMinutes(10))
                {
                    Say("As for that reward, I ain't rich, but I got a special trinket that's been in my family for generations. You help with them coyotes, it's yours.");
                    lastRewardTime = DateTime.Now;
                    from.AddToBackpack(new MaxxiaScroll()); // Reward item
                }
                else
                {
                    Say("You have to wait a bit before I can give you another reward.");
                }
            }
            else if (Insensitive.Contains(e.Speech, "badge"))
            {
                Say("This old badge ain't as shiny as Sheriff Tucker's, but it's got its own tales to tell. It's seen its fair share of duels and showdowns.");
            }
            else if (Insensitive.Contains(e.Speech, "moonshine"))
            {
                Say("That moonshine ain't just for remedies, you know. On a cold night, it warms you right up. Just don't have too much or you'll be seein' double!");
            }
            else if (Insensitive.Contains(e.Speech, "trinket"))
            {
                Say("This trinket is an old silver locket with a picture of my ma and pa inside. It's got sentimental value, but I reckon it'll serve you well.");
            }
            else if (Insensitive.Contains(e.Speech, "duels"))
            {
                Say("Ah, duels. I've had my fair share, but I always try to resolve things peacefully. A good cowboy knows when to draw and when to talk.");
            }
            else if (Insensitive.Contains(e.Speech, "cold"))
            {
                Say("On a cold night, there's nothin' better than sittin' by a campfire, strummin' a guitar, and sharin' stories with good folks.");
            }
            else if (Insensitive.Contains(e.Speech, "locket"))
            {
                Say("If you ever open that locket, you'll see a young version of my folks. They were pioneers, helped build this town from the ground up.");
            }
            else if (Insensitive.Contains(e.Speech, "peacefully"))
            {
                Say("Resolving things peacefully has saved me more trouble than I can count. It's all about understanding and listening to the other fella.");
            }
            else if (Insensitive.Contains(e.Speech, "campfire"))
            {
                Say("There's magic in a campfire, I tell ya. The way the flames dance and the embers glow, it's like they're telling their own stories.");
            }
            else if (Insensitive.Contains(e.Speech, "pioneers"))
            {
                Say("Pioneers like my folks had it tough. They faced many challenges, but their spirit and determination paved the way for our town's future.");
            }
            else if (Insensitive.Contains(e.Speech, "listening"))
            {
                Say("Listening is an art, my friend. It's how you learn about people, their hopes, dreams, and fears. Everyone's got a story to tell.");
            }
            else if (Insensitive.Contains(e.Speech, "embers"))
            {
                Say("Embers remind me of the past, of memories long gone but never forgotten. They're the last glow of a fire, holding onto its warmth.");
            }
            else if (Insensitive.Contains(e.Speech, "challenges"))
            {
                Say("Challenges are what make us stronger. They test our mettle, push us to our limits, and show us what we're truly made of.");
            }
            else if (Insensitive.Contains(e.Speech, "hopes"))
            {
                Say("We all have hopes and dreams. For me, it's to see this town thrive and be a place where folks can live in peace and harmony.");
            }
            else if (Insensitive.Contains(e.Speech, "memories"))
            {
                Say("Memories are like treasures, partner. Some are happy, some are sad, but they all shape who we are. Cherish them. Take this.");
                if (DateTime.Now > lastRewardTime.AddMinutes(10))
                {
                    lastRewardTime = DateTime.Now;
                    from.AddToBackpack(new MaxxiaScroll()); // Reward item
                }
            }

            base.OnSpeech(e);
        }

        public CowboyBill(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
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
