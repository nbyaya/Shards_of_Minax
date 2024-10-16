using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BrutalBen : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BrutalBen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Brutal Ben";
        Body = 0x190; // Human male body

        // Stats
        SetStr(170);
        SetDex(55);
        SetInt(18);

        SetHits(120);

        // Appearance
        AddItem(new ChainChest() { Hue = 1102 });
        AddItem(new ChainLegs() { Hue = 1102 });
        AddItem(new PlateHelm() { Hue = 1102 });
        AddItem(new LeatherGloves() { Name = "Ben's Bashing Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BrutalBen(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Brutal Ben, they call me. What's it to you?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I'm in the business of death, friend. Not that you'd understand. But, if you want to know more, you'd better be ready for the truth.");
                aboutModule.AddOption("What do you mean by 'business of death'?",
                    p => true,
                    p =>
                    {
                        DialogueModule deathModule = new DialogueModule("Death isn't just my business, it's an art. And the blade? It's my brush. Ever heard of the Silent Dagger?");
                        deathModule.AddOption("Tell me about the Silent Dagger.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule daggerModule = new DialogueModule("The Silent Dagger, the guild of assassins. Some say they're just a myth, but those in the know, fear them. I might or might not be connected, but ask too much and you might find out the hard way.");
                                daggerModule.AddOption("How many people have you killed?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule killCountModule = new DialogueModule("Ah, the question of numbers. You really want to know? Fine. I've lost count, but it's well over a hundred. Each face is etched in my mind—some deserved it, some... well, let's just say it's the way of the world.");
                                        killCountModule.AddOption("Did you ever regret any of them?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule regretModule = new DialogueModule("Regret? Aye, some of them haunt me. The eyes of those who didn't deserve it—they stare back at me in my dreams. But I can't change the past, and regrets don't help you survive in this world.");
                                                regretModule.AddOption("Who were they?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule whoModule = new DialogueModule("There was a young lad, barely a man. He was just in the wrong place at the wrong time. And a healer—she tried to protect her village. I did what I had to do, but those memories never leave.");
                                                        whoModule.AddOption("That sounds rough.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, whoModule));
                                                    });
                                                regretModule.AddOption("I suppose that's the life you chose.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, regretModule));
                                            });
                                        killCountModule.AddOption("You seem proud of it.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule prideModule = new DialogueModule("Proud? No. But I am alive, and in this world, that's something. Every kill was a step forward, a way to keep breathing for one more day. It's not pride—it’s survival.");
                                                prideModule.AddOption("I see. It's all about survival.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                prideModule.AddOption("You don't have to live like this forever.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule changeModule = new DialogueModule("Maybe not. But changing who I am isn't easy. I've lived by the blade for so long, it's all I know. Perhaps one day, I'll find a way out—if I live that long.");
                                                        changeModule.AddOption("I hope you do.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        changeModule.AddOption("It's never too late to try.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, changeModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, prideModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, killCountModule));
                                    });
                                daggerModule.AddOption("I think I'll stop asking questions.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, daggerModule));
                            });
                        deathModule.AddOption("Maybe this isn't for me.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, deathModule));
                    });
                aboutModule.AddOption("I don't care about your story.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("You mentioned you've been through worse.",
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
                    DialogueModule worseModule = new DialogueModule("I've been beaten, broken, and left for dead more times than you can count. Yet here I stand. Even when I was down, I never lost hope. The amulet my mother gave me has always brought me luck. Say, you seem like someone who appreciates trinkets. Here, have this.");
                    worseModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Assuming MaxxiaScroll is a valid item
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, worseModule));
                }
            });

        greeting.AddOption("What keeps you going?",
            player => true,
            player =>
            {
                DialogueModule determinationModule = new DialogueModule("The more I face, the stronger I become. It's about survival. You ever been to the Dead Man's Alley?");
                determinationModule.AddOption("Tell me about Dead Man's Alley.",
                    p => true,
                    p =>
                    {
                        DialogueModule alleyModule = new DialogueModule("It's a dark place, where many have met their end. It's not for the faint of heart. You looking to prove something?");
                        alleyModule.AddOption("Maybe one day.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, alleyModule));
                    });
                determinationModule.AddOption("Not interested.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, determinationModule));
            });

        greeting.AddOption("Goodbye, Ben.",
            player => true,
            player =>
            {
                player.SendMessage("Brutal Ben grunts and turns away.");
            });

        return greeting;
    }

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