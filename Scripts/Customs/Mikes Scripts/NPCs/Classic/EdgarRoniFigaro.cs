using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EdgarRoniFigaro : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EdgarRoniFigaro() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Edgar Roni Figaro";
        Body = 0x190; // Human male body

        // Stats
        SetStr(110);
        SetDex(90);
        SetInt(100);
        SetHits(85);

        // Appearance
        AddItem(new PlateLegs() { Hue = 1900 });
        AddItem(new PlateChest() { Hue = 1900 });
        AddItem(new PlateGloves() { Hue = 1900 });
        AddItem(new Cloak() { Hue = 1175 });
        AddItem(new BattleAxe() { Name = "Edgar's Axe" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize reward timer
    }

    public EdgarRoniFigaro(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings, my friend. I am Edgar Roni Figaro, a man of many talents. How can I entertain you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a master of disguise, a lover of the finer things, and a confidant of the wind. My talents range from playing the lute to crafting trinkets for those I care about.");
                aboutModule.AddOption("What is your job?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My job, you ask? Let's just say I'm a master of disguise, and I've had the pleasure of experiencing many adventures. It's not a job for the faint-hearted, but the thrill is unmatched.");
                        jobModule.AddOption("Interesting, tell me more.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule thrillModule = new DialogueModule("Disguises have allowed me to enter places and hear stories unknown to most. But the greatest part of my adventures is the people I meet along the way, each with their own secrets and tales.");
                                thrillModule.AddOption("Tell me about your inventions.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule inventionModule = new DialogueModule("Ah, my inventions! I've always had a penchant for tinkering. I've crafted many machines over the years, some quite extraordinary.");
                                        inventionModule.AddOption("What kind of machines have you invented?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule machinesModule = new DialogueModule("One of my more notable creations is the chainsaw. A powerful tool that can fell even the mightiest of trees. It took years of trial and error, but the result was worth it.");
                                                machinesModule.AddOption("The chainsaw? That sounds dangerous!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule chainsawModule = new DialogueModule("Indeed, it can be quite dangerous if used carelessly. However, in the right hands, it's a marvel of engineering—capable of great things. I designed it originally to help in clearing land quickly.");
                                                        chainsawModule.AddOption("Do you have any other inventions?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule burrowingCastleModule = new DialogueModule("Ah, yes! My pride and joy—the Burrowing Castle. Imagine a grand fortress that can tunnel underground and relocate itself at will. It was built to keep those inside safe from threats while remaining entirely hidden.");
                                                                burrowingCastleModule.AddOption("A castle that moves underground? Incredible! How does it work?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule workingCastleModule = new DialogueModule("The Burrowing Castle uses a series of rotating drill mechanisms combined with enchantments of displacement. It's both magic and technology working in perfect harmony. The drill cuts through the earth, while the enchantments stabilize the structure and maintain comfort for those inside.");
                                                                        workingCastleModule.AddOption("What was your inspiration for the Burrowing Castle?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule inspirationModule = new DialogueModule("I was inspired by nature itself—the way burrowing animals like moles create intricate networks underground. I thought, why shouldn't people have a similar means to protect themselves? Thus, the Burrowing Castle was born.");
                                                                                inspirationModule.AddOption("That is truly ingenious.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, inspirationModule));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, workingCastleModule));
                                                                    });
                                                                burrowingCastleModule.AddOption("What other uses does the Burrowing Castle have?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule usesModule = new DialogueModule("Besides evading threats, the Burrowing Castle has been used for secret meetings, protecting valuable treasures, and even for transporting entire communities safely across great distances. It's a versatile creation, limited only by imagination.");
                                                                        usesModule.AddOption("You must be quite proud of these inventions.",
                                                                            plg => true,
                                                                            plg =>
                                                                            {
                                                                                plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, usesModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, burrowingCastleModule));
                                                            });
                                                        chainsawModule.AddOption("Fascinating. What other machines have you created?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule otherMachinesModule = new DialogueModule("Oh, there have been many. I've dabbled in flying contraptions, automated carriages, and even a device that can mimic the songs of birds. Each project has taught me something new.");
                                                                otherMachinesModule.AddOption("A flying contraption? Tell me more!",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule flyingModule = new DialogueModule("Ah, yes, the Sky Sailer. It harnesses both wind currents and a levitation enchantment to glide gracefully through the air. It's not without its risks, but there's nothing quite like soaring above the clouds.");
                                                                        flyingModule.AddOption("You must be a true visionary.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, flyingModule));
                                                                    });
                                                                otherMachinesModule.AddOption("Maybe another time.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, otherMachinesModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, chainsawModule));
                                                    });
                                                machinesModule.AddOption("What else have you invented?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule otherInventionsModule = new DialogueModule("Besides the chainsaw and Burrowing Castle, I've designed various smaller gadgets—devices for navigation, stealth, and even tools for the arts. Creativity is boundless when you have the right materials and an open mind.");
                                                        otherInventionsModule.AddOption("You truly have a remarkable imagination.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, otherInventionsModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, machinesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, inventionModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, thrillModule));
                            });
                        jobModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                aboutModule.AddOption("What are your talents?",
                    p => true,
                    p =>
                    {
                        DialogueModule talentsModule = new DialogueModule("Ah, my talents? I've been known to play the lute, dance under the moonlight, and craft trinkets for those dear to me. Each day is a chance to embrace life with creativity and passion.");
                        talentsModule.AddOption("Tell me more about your crafting.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule craftingModule = new DialogueModule("Crafting is an art that requires both patience and inspiration. I craft trinkets, charms, and even some intricate devices that serve practical purposes. For instance, I've made a bracelet that glows softly in the presence of danger—quite handy, wouldn't you say?");
                                craftingModule.AddOption("That sounds useful. Do you sell these trinkets?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule sellModule = new DialogueModule("I do, occasionally. However, I prefer to craft for those who inspire me or whom I consider friends. Each piece is unique, crafted with care and meaning.");
                                        sellModule.AddOption("I would love to see your work someday.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, sellModule));
                                    });
                                craftingModule.AddOption("You have a true artist's spirit.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, craftingModule));
                            });
                        talentsModule.AddOption("You seem full of surprises.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, talentsModule));
                    });
                aboutModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Why do you speak of the wind?",
            player => true,
            player =>
            {
                DialogueModule windModule = new DialogueModule("The wind has always been my confidant, carrying my secrets far and wide. But not all secrets are for everyone—some must remain close to the heart.");
                windModule.AddOption("I understand, some secrets are precious.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, windModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later, my friend.");
                    noRewardModule.AddOption("I will check back another time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("You've shown interest in my tales, and for that, I have a small token of gratitude. Take this crystal—it may serve you well in your travels.");
                    rewardModule.AddOption("Thank you, Edgar.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new ProvocationAugmentCrystal());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye, Edgar.",
            player => true,
            player =>
            {
                player.SendMessage("Edgar bows gracefully and turns back to his thoughts.");
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