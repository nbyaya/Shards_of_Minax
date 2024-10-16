using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class PennylessPete : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public PennylessPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Pennyless Pete";
        Body = 0x190; // Human male body

        // Stats
        SetStr(35);
        SetDex(35);
        SetInt(25);
        SetHits(42);

        // Appearance
        AddItem(new ShortPants() { Hue = 33 });
        AddItem(new Doublet() { Hue = 44 });
        AddItem(new Shoes() { Hue = 1109 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public PennylessPete(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ye lookin' at Pennyless Pete, the wretched beggar. I have a tale to tell about me troubles with shoes. What do ye want to know?");

        greeting.AddOption("What’s the problem with your shoes?",
            player => true,
            player =>
            {
                DialogueModule shoeModule = new DialogueModule("Ah, me poor feet. I can only afford these cardboard shoes. They ain't much, but they keep the cobbles from givin' me splinters. But oh, how I long for leather!");
                shoeModule.AddOption("Why don’t you buy leather shoes?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule whyNotLeatherModule = new DialogueModule("Leather shoes? Aye, they be the finest, but the cost! I’d need a small fortune to afford 'em. Me pockets are as empty as me stomach most days.");
                        whyNotLeatherModule.AddOption("What do they cost?",
                            p => true,
                            p =>
                            {
                                DialogueModule costModule = new DialogueModule("A decent pair o' leather shoes runs at least 20 gold coins. That’s enough to feed me for a week! I can’t afford to part with that much.");
                                costModule.AddOption("Have you tried to earn some gold?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule earnGoldModule = new DialogueModule("Tried me hand at many things, I have! But folks look right through me. A beggar's plight is seldom rewarded with coin.");
                                        earnGoldModule.AddOption("Maybe I can help you earn gold?",
                                            pw => true,
                                            pw =>
                                            {
                                                DialogueModule helpEarnModule = new DialogueModule("Aye, if ye could help me find odd jobs or even give a coin or two, I’d be ever so grateful! I'd save every last one for me shoes.");
                                                helpEarnModule.AddOption("What kind of odd jobs?",
                                                    ple => true,
                                                    ple =>
                                                    {
                                                        DialogueModule jobListModule = new DialogueModule("There be a few things about town. I could help clean stables or carry messages for folk. It's not glorious, but it's honest work.");
                                                        jobListModule.AddOption("I'll help you find a job.",
                                                            pr => true,
                                                            pr =>
                                                            {
                                                                p.SendMessage("You offer to help Pennyless Pete find work.");
                                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                            });
                                                        jobListModule.AddOption("Maybe another time.",
                                                            plt => true,
                                                            plt =>
                                                            {
                                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                            });
                                                        p.SendGump(new DialogueGump(p, jobListModule));
                                                    });
                                                helpEarnModule.AddOption("I can spare a few coins.",
                                                    ply => true,
                                                    ply =>
                                                    {
                                                        player.AddToBackpack(new Gold(1)); // Example donation
                                                        pl.SendMessage("You give a coin to Pennyless Pete.");
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                    });
                                                player.SendGump(new DialogueGump(player, helpEarnModule));
                                            });
                                        earnGoldModule.AddOption("That's unfortunate. What about trade?",
                                            pu => true,
                                            pu =>
                                            {
                                                DialogueModule tradeModule = new DialogueModule("Trade, ye say? I’d gladly barter, but me wares are few. I have naught but me charm and a good tale or two.");
                                                tradeModule.AddOption("Tell me one of your tales.",
                                                    pli => true,
                                                    pli =>
                                                    {
                                                        DialogueModule taleModule = new DialogueModule("Ah, gather ‘round! Once, I stumbled upon a lost treasure map in the gutter. It led to a hidden chest, but the goblins got to it first!");
                                                        taleModule.AddOption("What happened then?",
                                                            po => true,
                                                            po =>
                                                            {
                                                                DialogueModule taleContinueModule = new DialogueModule("I ran as fast as me legs would carry me! But I learned that treasure ain’t worth much if ye lose yer life over it.");
                                                                taleContinueModule.AddOption("A wise lesson indeed.",
                                                                    plp => true,
                                                                    plp => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                                                p.SendGump(new DialogueGump(p, taleContinueModule));
                                                            });
                                                        taleModule.AddOption("Sounds dangerous!",
                                                            pla => true,
                                                            pla => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                                        p.SendGump(new DialogueGump(p, taleModule));
                                                    });
                                                player.SendGump(new DialogueGump(player, tradeModule));
                                            });
                                        p.SendGump(new DialogueGump(p, earnGoldModule));
                                    });
                                player.SendGump(new DialogueGump(player, costModule));
                            });
                        player.SendGump(new DialogueGump(player, whyNotLeatherModule));
                    });

                shoeModule.AddOption("What’s so bad about cardboard shoes?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule cardboardModule = new DialogueModule("They keep out the rain, but they tear too easily. I’ve had to patch 'em up so many times, they’ve turned into a puzzle of sorts.");
                        cardboardModule.AddOption("How do you patch them?",
                            p => true,
                            p =>
                            {
                                DialogueModule patchingModule = new DialogueModule("I use whatever I can find. Old rags, bits of leather, even some leftover glue from a broken pot! Whatever it takes to keep me feet from touchin' the ground.");
                                patchingModule.AddOption("That's resourceful!",
                                    pls => true,
                                    pls => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                player.SendGump(new DialogueGump(player, patchingModule));
                            });
                        cardboardModule.AddOption("I can get you some proper shoes.",
                            pld => true,
                            pld =>
                            {
                                DialogueModule shoesHelpModule = new DialogueModule("Aye, if ye could spare the gold or fetch me a pair, I'd be eternally grateful! Even a second-hand pair would do!");
                                shoesHelpModule.AddOption("I’ll look for some shoes for you.",
                                    p => true,
                                    p =>
                                    {
                                        p.SendMessage("You promise to look for some shoes for Pennyless Pete.");
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                shoesHelpModule.AddOption("Sorry, I can’t help right now.",
                                    plf => true,
                                    plf => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                player.SendGump(new DialogueGump(player, shoesHelpModule));
                            });
                        player.SendGump(new DialogueGump(player, cardboardModule));
                    });

                player.SendGump(new DialogueGump(player, shoeModule));
            });

        greeting.AddOption("Do you need a reward?",
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
                    player.SendMessage("For your thoughtful inquiry, please accept this reward.");
                    player.AddToBackpack(new Gold(1)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("Goodbye, Pennyless Pete.",
            player => true,
            player =>
            {
                player.SendMessage("Farewell, and may fortune find ye!");
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
