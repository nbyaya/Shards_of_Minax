using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CowgirlAnnie : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CowgirlAnnie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cowgirl Annie";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(100);
        SetInt(60);
        SetHits(75);
        SetMana(100);
        SetStam(100);

        Fame = 0;
        Karma = 0;
        VirtualArmor = 10;

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

    public CowgirlAnnie(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Howdy there, partner! Name's Cowgirl Annie. What brings you 'round these parts?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Well, I reckon I'm just a simple cowgirl, wrangling cattle and tending to the ranch. My ma named me after her granny, a pioneer who crossed the great plains with just a horse and a dream.");
                aboutModule.AddOption("That's impressive. Tell me more about the pioneer.",
                    p => true,
                    p =>
                    {
                        DialogueModule pioneerModule = new DialogueModule("That's right! My great-grandma was a true trailblazer. She faced many hardships, but her determination saw her through. She even found a hidden treasure once, but never told anyone where it was.");
                        pioneerModule.AddOption("Sounds like she was tough as nails.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, pioneerModule));
                    });
                aboutModule.AddOption("What do you do around here?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("I'm a cowgirl, partner! I wrangle cattle and tend to this here ranch. It's tough work, but it's been in my family for generations. I couldn't do it all without the help of my loyal dog, Rex.");
                        jobModule.AddOption("Tell me about Rex.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule rexModule = new DialogueModule("Rex is my loyal companion and the best cattle dog a cowgirl could ask for. He's been with me since he was just a pup, and he knows this ranch like the back of his paw. He's always by my side, keeping the cattle in line and making sure no predators get too close.");
                                rexModule.AddOption("Sounds like a great dog. How does he help?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Rex helps in all sorts of ways! He rounds up stray cattle, barks to alert me of any trouble, and even helps guide the younger calves back to their mothers. He's got a keen sense of danger too—once, he scared off a cougar that was sneaking up on the herd.");
                                        helpModule.AddOption("Wow, that must have been scary! Tell me more about that.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule cougarModule = new DialogueModule("It sure was! I remember it like it was yesterday. Rex and I were out in the north field when he suddenly started growling and barking like crazy. I turned around just in time to see a cougar creeping towards the herd. Rex didn't hesitate—he ran right at that cougar, barking his head off until it turned tail and ran. He's got more courage than most folks I know.");
                                                cougarModule.AddOption("Rex sounds incredible. Has he had other adventures?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule adventuresModule = new DialogueModule("Oh, plenty! Rex once helped me track down a lost calf during a fierce storm. The wind was howlin', and the rain was comin' down in sheets, but Rex never gave up. He led me straight to that calf, huddled under a tree, and we got him back to the barn safe and sound. He's got a nose for trouble, and a heart full of loyalty.");
                                                        adventuresModule.AddOption("What a brave dog. Do you reward him?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule rewardRexModule = new DialogueModule("Absolutely! Rex gets the best treats and the warmest spot by the fire. I make sure he's well taken care of—he deserves it for all the hard work he does. He's not just a working dog; he's family.");
                                                                rewardRexModule.AddOption("He sounds like he deserves it.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, rewardRexModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, adventuresModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, cougarModule));
                                            });
                                        helpModule.AddOption("That's amazing. You two make a great team.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, helpModule));
                                    });
                                rexModule.AddOption("I'd love to meet him sometime.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Cowgirl Annie smiles warmly. 'Rex is probably out in the fields right now, but if you stick around, I'm sure you'll see him!'");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                rexModule.AddOption("He must be a big help.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, rexModule));
                            });
                        jobModule.AddOption("Do you ever need help with chores?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("Well, I reckon I'm feelin' as fit as a fiddle! Out here in the ranch, you gotta be fit to keep up with the cattle and the chores.");
                healthModule.AddOption("Glad to hear it!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Do you have any lessons to share?",
            player => true,
            player =>
            {
                DialogueModule lessonModule = new DialogueModule("One lesson I've always remembered is to never judge a book by its cover. Out here, even the toughest cowboys have a soft side, and the quietest critters can be the fiercest.");
                lessonModule.AddOption("That's good advice.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, lessonModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("You've got a kind heart, partner! Help me round up them stray cattle and I'll give you something special for your efforts.");
                    rewardModule.AddOption("Thank you, I'll help.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendMessage("Cowgirl Annie gives you a reward for your help.");
                        });
                    rewardModule.AddOption("Maybe later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Cowgirl Annie tips her hat to you.");
            });

        return greeting;
    }

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