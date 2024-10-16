using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Stabby Steve")]
    public class StabbySteve : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public StabbySteve() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Stabby Steve";
            Body = 0x190; // Human male body

            // Stats
            Str = 160;
            Dex = 70;
            Int = 20;
            Hits = 115;

            // Appearance
            AddItem(new BoneHelm() { Hue = 1172 });
            AddItem(new LongPants() { Hue = 1154 });
            AddItem(new Tunic() { Hue = 1120 });
            AddItem(new Boots() { Hue = 1908 });
            AddItem(new LeatherGloves() { Hue = 0, Name = "Steve's Slicing Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Who's askin'?");
            }
            else if (speech.Contains("health"))
            {
                Say("Why ya carin' 'bout my health, huh?");
            }
            else if (speech.Contains("job"))
            {
                Say("Murderin' is my trade, if ya gotta know.");
            }
            else if (speech.Contains("story"))
            {
                Say("Think you're any better, do ya? What's yer story?");
            }
            else if (speech.Contains("smart"))
            {
                Say("So, you think you're smart, huh? Tell me, why shouldn't I stab you right here?");
            }
            else if (speech.Contains("steve"))
            {
                Say("They call me Stabby Steve, but names don't mean much in my line of work.");
            }
            else if (speech.Contains("care"))
            {
                Say("I've had a few scrapes and bruises, but ain't nothin' that's kept me down. Got a scar on my back that tells a tale, if ya care to hear it.");
            }
            else if (speech.Contains("murder"))
            {
                Say("Aye, murderin' pays the bills, but there's more to it. There's an art to it, a passion. Ever heard of the Silent Dagger?");
            }
            else if (speech.Contains("line"))
            {
                Say("Most folks would call it dirty work, but to me, it's just business. You ever been on the wrong side of a deal?");
            }
            else if (speech.Contains("scar"))
            {
                Say("It was from a brawl in a dingy tavern. Got ambushed, but managed to fend them off. I'll give ya a reward if you find out who set me up that night.");
            }
            else if (speech.Contains("dagger"))
            {
                Say("The Silent Dagger is a group of elite assassins, and I used to be one of 'em. Left for reasons of my own. But that's another story for another day.");
            }
            else if (speech.Contains("deal"))
            {
                Say("Been double-crossed more times than I care to admit. But I always get my revenge. You ever been betrayed?");
            }
            else if (speech.Contains("ambush"))
            {
                Say("That night, I was outnumbered, but I had the element of surprise. Whoever set me up didn't expect me to fight back. I've got a hunch, but I need more evidence.");
            }
            else if (speech.Contains("elite"))
            {
                Say("Being elite means you're the best of the best. But it also means you've got a target on your back. Everyone wants to take down the top dog.");
            }
            else if (speech.Contains("betrayed"))
            {
                Say("Betrayal is a bitter pill. It's taught me not to trust easily. But sometimes, it leads you to unexpected allies.");
            }
            else if (speech.Contains("evidence"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("If you bring me solid proof of who set me up, there might be a reward in it for you. But be careful, they're dangerous.");
                    from.AddToBackpack(new Gold(100)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("top"))
            {
                Say("When you're at the top, the only way is down. But I ain't planning on falling anytime soon.");
            }
            else if (speech.Contains("allies"))
            {
                Say("Allies are hard to come by in my world. But once you've got someone's back, and they've got yours, it's a bond that's hard to break.");
            }
            else if (speech.Contains("proof"))
            {
                Say("Solid proof ain't easy to come by. But if you manage to get it, it'll be worth your while.");
            }
            else if (speech.Contains("falling"))
            {
                Say("I've had my share of close calls, but I always land on my feet. You ever been in a tight spot?");
            }
            else if (speech.Contains("bond"))
            {
                Say("Bonds are forged in the heat of battle, in moments of trust and vulnerability. They're not easily broken.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Everything has its price, including loyalty. But some things, like trust, are worth more than gold.");
            }
            else if (speech.Contains("spot"))
            {
                Say("Been cornered a few times, but I always find a way out. It's all about thinking on your feet.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is earned, not given. But once you have it, it's a powerful tool.");
            }
            else if (speech.Contains("gold"))
            {
                Say("Gold can buy you many things, but it can't buy respect. Respect is earned through deeds.");
            }
            else if (speech.Contains("cornered"))
            {
                Say("When you're cornered, you see what you're truly made of. Fight or flight, what's your choice?");
            }
            else if (speech.Contains("tool"))
            {
                Say("Every tool has its purpose, just like every person. It's all about how you use it. Take this.");
                from.AddToBackpack(new MaxxiaScroll()); // Example reward item
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect is something you can't take, it's given. And once you have it, it changes everything.");
            }
            else if (speech.Contains("choice"))
            {
                Say("Choices define us, shape our paths. Every choice has a consequence, whether good or bad.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Everyone has a purpose, a role to play. Some find it early, some search their whole lives.");
            }
            else if (speech.Contains("changes"))
            {
                Say("Life is full of changes, twists, and turns. You've got to adapt if you want to survive.");
            }
            else if (speech.Contains("consequence"))
            {
                Say("Consequences follow actions, like shadows. Some are predictable, others catch you off guard.");
            }
            else if (speech.Contains("search"))
            {
                Say("Searching can be tiring, but when you find what you're looking for, it's all worth it.");
            }

            base.OnSpeech(e);
        }

        public StabbySteve(Serial serial) : base(serial) { }

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
