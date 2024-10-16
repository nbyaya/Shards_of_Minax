using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BakerBob : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BakerBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Baker Bob";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(45);
        SetInt(60);
        SetHits(65);

        // Appearance
        AddItem(new ShortPants(1153));
        AddItem(new Doublet(443));
        AddItem(new ThighBoots(1904));
        AddItem(new LeatherGloves() { Name = "Bob's Baking Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("Ye dare disturb Baker Bob while he's toilin' over these infernal ovens? What do ye want, traveler?");

        greeting.AddOption("Tell me about the annual Cake Master of Britannia competition.",
            player => true,
            player =>
            {
                DialogueModule cakeCompetitionModule = new DialogueModule("Ah, the annual Cake Master of Britannia competition! It's the grandest event in all the land for us bakers. The finest cooks gather to showcase their best cakes, and only one earns the coveted title.");
                cakeCompetitionModule.AddOption("How do you compete in the competition?",
                    p => true,
                    p =>
                    {
                        DialogueModule competeModule = new DialogueModule("To compete, ye must present a cake that embodies creativity, taste, and skill. Judges are strict; they look for the perfect blend of flavors, decoration, and a story behind the cake.");
                        competeModule.AddOption("What's your secret for competing?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule secretModule = new DialogueModule("Ah, ye want to know Baker Bob's secret, eh? It's all about the ovenstone, lad. The ancient relic keeps the temperature perfect, and I've a special ingredient I use: Stardust Sugar. It's rare, but it makes the cakes shine as if blessed by the stars.");
                                secretModule.AddOption("Where can I find Stardust Sugar?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule sugarModule = new DialogueModule("Stardust Sugar is a tricky thing to find. Ye must venture to the Enchanted Grove during a full moon. It's guarded by the Forest Sprites, but if ye show them kindness, they might let ye harvest some.");
                                        sugarModule.AddOption("How do I show kindness to the Sprites?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule kindnessModule = new DialogueModule("The Sprites love music and laughter. If ye bring a lute and play a joyful tune, they might dance and allow ye to collect some Stardust Sugar. Beware though, they dislike sour notes!");
                                                kindnessModule.AddOption("I'll give it a try.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Ye feel a sense of determination to find the Stardust Sugar.");
                                                    });
                                                kindnessModule.AddOption("That sounds too risky.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, kindnessModule));
                                            });
                                        sugarModule.AddOption("I might search for it later.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, sugarModule));
                                    });
                                secretModule.AddOption("That sounds amazing, but I have more questions.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, cakeCompetitionModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, secretModule));
                            });
                        competeModule.AddOption("Who are the judges?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule judgesModule = new DialogueModule("The judges are the most renowned bakers and food critics in Britannia. Lady Amelia, the Royal Chef, is known for her sharp tongue and high standards. Then there's Sir Cedric, a former Cake Master himself, and Fiona, a mysterious alchemist with a talent for flavors.");
                                judgesModule.AddOption("Lady Amelia sounds strict.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule ameliaModule = new DialogueModule("Aye, Lady Amelia has no patience for half-baked attempts. She expects perfection, from the texture to the smallest decoration. Many a baker has crumbled under her scrutiny.");
                                        ameliaModule.AddOption("I think I could impress her.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Ye feel a surge of confidence to face Lady Amelia's high standards.");
                                            });
                                        ameliaModule.AddOption("She sounds terrifying.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, ameliaModule));
                                    });
                                judgesModule.AddOption("Tell me more about Sir Cedric.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule cedricModule = new DialogueModule("Sir Cedric is fair but tough. He appreciates traditional techniques and values a baker who respects the art's history. If ye can bake a classic cake perfectly, ye might win his favor.");
                                        cedricModule.AddOption("I'll try to impress him with a classic recipe.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Ye decide to practice the classic recipes to impress Sir Cedric.");
                                            });
                                        cedricModule.AddOption("I'm not one for tradition.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, cedricModule));
                                    });
                                judgesModule.AddOption("Who is Fiona, the alchemist?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule fionaModule = new DialogueModule("Fiona is a mysterious one. She uses her knowledge of alchemy to judge flavor combinations. She loves surprises, especially when ingredients are used in unexpected but delightful ways.");
                                        fionaModule.AddOption("I like experimenting too.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Ye feel inspired to experiment with new flavors to impress Fiona.");
                                            });
                                        fionaModule.AddOption("Alchemy and baking? That's strange.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, fionaModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, judgesModule));
                            });
                        competeModule.AddOption("I think I'll leave the competition to the experts.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, competeModule));
                    });
                cakeCompetitionModule.AddOption("Have you ever won the competition?",
                    p => true,
                    p =>
                    {
                        DialogueModule winModule = new DialogueModule("Once, many years ago, I took the title of Cake Master of Britannia. It was with a cake inspired by my mother's favorite flowers—lavender and roses. The judges said it was like tasting a memory.");
                        winModule.AddOption("That sounds beautiful.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Ye feel a deep respect for Baker Bob's passion.");
                            });
                        winModule.AddOption("Why haven't you won again?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule lossModule = new DialogueModule("The competition has only gotten tougher, and new talents emerge every year. I've tried, but sometimes the younger bakers bring something fresh that even I can't match. But I keep tryin'—not for the title, but for the love of bakin'.");
                                lossModule.AddOption("That's admirable, Baker Bob.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, lossModule));
                            });
                        p.SendGump(new DialogueGump(p, winModule));
                    });
                cakeCompetitionModule.AddOption("Sounds like quite the event.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, cakeCompetitionModule));
            });

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("Me job? Me slavin' away in this inferno to bake them ungrateful townies their daily bread!");
                jobModule.AddOption("It must be tough work.",
                    p => true,
                    p =>
                    {
                        DialogueModule toughWorkModule = new DialogueModule("Aye, it's tough work, but there's nothin' quite like the smell of freshly baked bread to keep me goin'.");
                        toughWorkModule.AddOption("The smell of bread, you say?", 
                            pl => true, 
                            pl =>
                            {
                                DialogueModule breadSmellModule = new DialogueModule("Ah, the aroma of freshly baked bread! It's a scent that can lure even the hungriest of adventurers from the furthest lands. Speaking of which, ever tried my special cinnamon loaf?");
                                breadSmellModule.AddOption("Tell me about your cinnamon loaf.",
                                    pla => true,
                                    pla =>
                                    {
                                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                                        {
                                            pla.SendMessage("I have no reward right now. Please return later.");
                                        }
                                        else
                                        {
                                            pla.SendMessage("My special cinnamon loaf is a secret recipe, passed down in my family. Here, have a taste as a reward!");
                                            pla.AddToBackpack(new MaxxiaScroll());
                                            lastRewardTime = DateTime.UtcNow;
                                        }
                                    });
                                breadSmellModule.AddOption("Maybe another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, breadSmellModule));
                            });
                        toughWorkModule.AddOption("Sounds like hard work.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, toughWorkModule));
                    });
                jobModule.AddOption("Why do you do it?",
                    p => true,
                    p =>
                    {
                        DialogueModule whyModule = new DialogueModule("Those townies might complain, but they'd be lost without my bakes. It's not just about bread; it's about keeping the spirit of the town alive.");
                        whyModule.AddOption("That's quite noble.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, whyModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What are those burns on your hands?",
            player => true,
            player =>
            {
                DialogueModule burnsModule = new DialogueModule("These burns ain't just for show, they're badges of honor! For every burn, there's a memory of a perfectly baked loaf or a satisfied customer.");
                burnsModule.AddOption("A badge of honor indeed!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, burnsModule));
            });

        greeting.AddOption("Can I see your ovenstone?",
            player => true,
            player =>
            {
                DialogueModule ovenstoneModule = new DialogueModule("The ovenstone is an ancient relic passed down through generations. It's said to have magical properties, ensuring every bake is perfect. Here, take a look.");
                ovenstoneModule.AddOption("Thank you, Baker Bob.",
                    p => true,
                    p =>
                    {
                        p.AddToBackpack(new CookingAugmentCrystal());
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, ovenstoneModule));
            });

        greeting.AddOption("Good luck with your baking.",
            player => true,
            player =>
            {
                player.SendMessage("Baker Bob grumbles something about 'ungrateful townies' as he turns back to his ovens.");
            });

        return greeting;
    }

    public BakerBob(Serial serial) : base(serial) { }

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