using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class Edge : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Edge() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Edge";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(80);
        SetInt(80);

        SetHits(80);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1153 });
        AddItem(new LeatherChest() { Hue = 1153 });
        AddItem(new LeatherGloves() { Hue = 1153 });
        AddItem(new LeatherCap() { Hue = 1153 });
        AddItem(new Boots() { Hue = 1153 });
        AddItem(new Katana() { Name = "Edge's Katana" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public Edge(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Edge, the one and only. What do you want?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Edge, a jack of all trades, master of none. Some know me from my infamous escapades in the Forbidden Caves. What else do you wish to know?");
                identityModule.AddOption("Tell me about the Forbidden Caves.",
                    p => true,
                    p =>
                    {
                        DialogueModule cavesModule = new DialogueModule("Ah, the Forbidden Caves. A place of mystery and danger. Many adventurers have sought its treasures, but few have returned. Tread carefully if you ever decide to venture there.");
                        cavesModule.AddOption("Sounds dangerous. I might check it out.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, cavesModule));
                    });
                identityModule.AddOption("What trades have you mastered?",
                    p => true,
                    p =>
                    {
                        DialogueModule tradesModule = new DialogueModule("I've tried my hand at many things. Blacksmithing, tailoring, even a bit of magic. But none held my interest for long. Are you looking to learn a trade?");
                        tradesModule.AddOption("Tell me about magic.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule magicModule = new DialogueModule("Magic is a fickle thing. I've dabbled, but never mastered it. The arcane arts require a dedication I simply don't possess. However, I've heard there's a hidden academy in the Mystic Woods where mages train in secret. Maybe that's of interest to you.");
                                magicModule.AddOption("Interesting, I might explore that.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, magicModule));
                            });
                        tradesModule.AddOption("Tell me about your life as a king.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule kingModule = new DialogueModule("Balancing the responsibilities of being a king with my desire to train in ninjutsu has been the greatest challenge of my life. As a king, I must ensure the prosperity of my people, oversee the court, and settle disputes—all while finding the time to hone my skills in the shadow arts.");
                                kingModule.AddOption("How do you manage to find time to train?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule trainingModule = new DialogueModule("It's not easy. I often train in the dead of night, when my people are asleep. The moonlight provides enough illumination for my exercises, and the solitude helps me focus. But it comes at the cost of sleep and rest. My duties by day are relentless, and by night I push my body to its limits.");
                                        trainingModule.AddOption("Isn't that exhausting?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule exhaustingModule = new DialogueModule("Yes, it is. There are times when I feel as though I might collapse under the weight of it all. But the discipline of ninjutsu teaches me endurance and resilience. The will to protect my kingdom and improve myself drives me forward, even when exhaustion takes hold.");
                                                exhaustingModule.AddOption("What keeps you motivated?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule motivationModule = new DialogueModule("My motivation comes from the love I have for my people and the belief that a leader should not only command but also lead by example. I wish to be a symbol of strength, someone who embodies the ideals of both a warrior and a ruler. I train to become stronger, not just for myself, but for everyone who looks up to me.");
                                                        motivationModule.AddOption("Do your people understand your struggle?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule understandingModule = new DialogueModule("Some do, but many don't. To most, I am simply their king—a distant figure who issues decrees and attends public events. Only a few trusted advisors and friends know the full extent of my struggle. I suppose that is part of leadership: to bear the burdens that others cannot see and to stay steadfast, even in solitude.");
                                                                understandingModule.AddOption("That sounds lonely.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule lonelyModule = new DialogueModule("It can be, yes. There are moments when I wish I could share my journey with someone who truly understands. But I have made peace with it. The path of a leader is often lonely, but it is also deeply rewarding. Knowing that my efforts make a difference for my people gives me strength.");
                                                                        lonelyModule.AddOption("Do you ever wish for a different life?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule differentLifeModule = new DialogueModule("Sometimes, I dream of a simpler life—one without the weight of a crown or the demands of the court. Perhaps I could have been a simple warrior, living in the mountains and perfecting my ninjutsu in peace. But those are just dreams. In reality, I cannot turn away from my responsibilities. My people need me, and I have accepted my role.");
                                                                                differentLifeModule.AddOption("That's admirable.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, differentLifeModule));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, lonelyModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, understandingModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, motivationModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, exhaustingModule));
                                            });
                                        trainingModule.AddOption("What kind of ninjutsu do you practice?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule ninjutsuModule = new DialogueModule("My ninjutsu training involves mastering stealth, agility, and swift combat techniques. I practice blending into the shadows, moving silently, and striking with precision. The art of ninjutsu is about control—not just of the body, but also the mind. It is as much about avoiding conflict as it is about fighting.");
                                                ninjutsuModule.AddOption("Can you teach me some of these skills?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule teachModule = new DialogueModule("I can teach you the basics, but true mastery requires years of dedication and practice. If you wish to learn, you must be prepared to embrace the discipline it demands. Ninjutsu is not just a skill—it is a way of life.");
                                                        teachModule.AddOption("I'm ready to learn.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Edge begins to teach you the basics of ninjutsu, emphasizing stealth and agility.");
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        teachModule.AddOption("Maybe another time.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, teachModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, ninjutsuModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, trainingModule));
                                    });
                                kingModule.AddOption("Does your court support your training?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule courtModule = new DialogueModule("My court has mixed opinions about my training. Some see it as a frivolous pursuit, something unbefitting of a king. Others, however, understand its value—how it keeps me sharp, prepared for threats that might arise. I rely on the support of those closest to me, even when the rest may doubt.");
                                        courtModule.AddOption("It must be hard to deal with their doubts.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule doubtModule = new DialogueModule("It is. But I have learned that leadership often means making decisions that others may not fully understand. I cannot let their doubts deter me from what I know is right. My training is essential—not just for me, but for the safety of my kingdom.");
                                                doubtModule.AddOption("I admire your resolve.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, doubtModule));
                                            });
                                        courtModule.AddOption("I see. Thank you for sharing.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, courtModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, kingModule));
                            });
                        tradesModule.AddOption("Perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, tradesModule));
                    });
                identityModule.AddOption("Never mind.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("Why do you care about my health? I'm not your physician. Though, I once met a skilled healer in the East who taught me a thing or two about remedies.");
                healthModule.AddOption("Can you share a remedy?",
                    p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    p =>
                    {
                        p.SendMessage("Edge gives you a small scroll with healing knowledge.");
                        p.AddToBackpack(new MaxxiaScroll());
                        lastRewardTime = DateTime.UtcNow;
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                healthModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Edge grunts dismissively as you leave.");
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