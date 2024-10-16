using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class GhastlyGregor : BaseCreature
{
    [Constructable]
    public GhastlyGregor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ghastly Gregor";
        Body = 0x190; // Human male body

        // Stats
        SetStr(140);
        SetDex(60);
        SetInt(100);
        SetHits(110);

        // Appearance
        AddItem(new Robe() { Hue = 1154 });
        AddItem(new Boots() { Hue = 1175 });
        AddItem(new BoneGloves() { Name = "Gregor's Grasping Gloves" });

        // Customizing appearance
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

        SpeechHue = 0; // Default speech hue
    }

    public GhastlyGregor(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("So, you dare disturb Ghastly Gregor? State your purpose, or suffer the consequences.");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Ghastly Gregor, the necromancer. Through forbidden arts, I have transcended mortality. What pitiful questions do you have for me?");
                identityModule.AddOption("Why are you called 'Ghastly'?",
                    p => true,
                    p =>
                    {
                        DialogueModule nameModule = new DialogueModule("Long ago, I was merely Gregor. But through my mastery over the forbidden arts, I became Ghastly Gregor. My name itself carries the weight of my power.");
                        nameModule.AddOption("That's impressive.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, nameModule));
                    });
                identityModule.AddOption("What do you do here?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My 'job' is to embrace the shadows and command the undead. I revel in the art of death.");
                        jobModule.AddOption("Why pursue the dark arts?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule reasonModule = new DialogueModule("The dark arts provide power that no mortal could fathom. I traded my mortality for knowledge and command over the undead. Power is all that matters.");
                                reasonModule.AddOption("Interesting. Tell me more.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule graveyardModule = CreateGraveyardModule();
                                        pla.SendGump(new DialogueGump(pla, graveyardModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, reasonModule));
                            });
                        jobModule.AddOption("I see. Goodbye.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                identityModule.AddOption("I have no more questions.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Tell me about your studies.",
            player => true,
            player =>
            {
                DialogueModule studiesModule = new DialogueModule("I have spent countless hours in the catacombs, deciphering old tomes and consulting spirits. It is there that I learned the true nature of life and death.");
                studiesModule.AddOption("What did you learn?",
                    p => true,
                    p =>
                    {
                        DialogueModule learnModule = new DialogueModule("The shadows are not merely the absence of light. They are alive, sentient, and ever-present. By embracing them, I learned to manipulate the very fabric of reality.");
                        learnModule.AddOption("How do you manipulate the shadows?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule manipulateModule = new DialogueModule("To manipulate is to control. And to control the undead, one must first understand them. Only then can their true power be harnessed.");
                                manipulateModule.AddOption("Where can I learn more about the undead?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule graveyardModule = CreateGraveyardModule();
                                        pla.SendGump(new DialogueGump(pla, graveyardModule));
                                    });
                                manipulateModule.AddOption("That sounds challenging.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, manipulateModule));
                            });
                        learnModule.AddOption("I have other questions.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, learnModule));
                    });
                studiesModule.AddOption("I see. Farewell.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, studiesModule));
            });

        greeting.AddOption("Do you have a test for me?",
            player => true,
            player =>
            {
                DialogueModule testModule = new DialogueModule("I see potential in you. Answer me this riddle, and I may deem you worthy of a reward. 'I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.' What am I?");
                testModule.AddOption("Is it an echo?",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Ah, you've deciphered it! An echo is the answer. Very well, take this token of my appreciation. May it serve you in the dark times ahead.");
                        p.AddToBackpack(new BushidoAugmentCrystal());
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                testModule.AddOption("I'm not sure.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Perhaps you are not ready yet. Come back when you have the answer.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, testModule));
            });

        greeting.AddOption("I have no more questions.",
            player => true,
            player =>
            {
                player.SendMessage("Ghastly Gregor nods, his eyes narrowing as he watches you carefully.");
            });

        return greeting;
    }

    private DialogueModule CreateGraveyardModule()
    {
        DialogueModule graveyardModule = new DialogueModule("Ah, the graveyards of Britannia... places rich with the essence of the departed, perfect for gathering the components I need for my necromantic pursuits. Are you interested in learning more about these grim locations?");

        graveyardModule.AddOption("Tell me about the graveyards.",
            player => true,
            player =>
            {
                DialogueModule detailsModule = new DialogueModule("There are many graveyards across Britannia, but only a few are truly worthy of a necromancer's attention. Each has its own unique qualities and spirits that linger.");
                detailsModule.AddOption("Which is the most potent graveyard?",
                    p => true,
                    p =>
                    {
                        DialogueModule yewModule = new DialogueModule("The Yew Graveyard is perhaps the most potent of all. It is steeped in ancient sorrow, and the spirits there are restless. The earth itself is darkened by the sheer volume of magic flowing through it.");
                        yewModule.AddOption("What makes it special?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule specialModule = new DialogueModule("Yew Graveyard's potency comes from the generations of powerful individuals laid to rest there. Their lingering emotions and untapped potential make it ideal for gathering spiritual residue and binding it into usable form.");
                                specialModule.AddOption("Can I harvest there?",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You could, but beware. Spirits of Yew are known to be jealous of the living. They do not take kindly to those who disturb their rest.");
                                        pla.SendGump(new DialogueGump(pla, CreateGraveyardModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, specialModule));
                            });
                        yewModule.AddOption("Are there other graveyards worth mentioning?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule otherModule = new DialogueModule("Yes, there are others. The Graveyard of Cove, for example, is small but powerful due to its proximity to cursed lands. The spirits there are bound by regret, making their energies highly useful for necromantic potions.");
                                otherModule.AddOption("What about the Britain Graveyard?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule britainModule = new DialogueModule("The Britain Graveyard is more commonly visited, but do not be deceived. While its spirits are weaker individually, their sheer numbers make it valuable. You can often find minor reagents littering the grounds.");
                                        britainModule.AddOption("Interesting. Any others?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule othersModule = new DialogueModule("The Nujel'm Crypts are also worth mentioning, though technically not a graveyard. They house noble spirits, and their energies are infused with pride and desperation, making them potent for those who know how to harness it.");
                                                othersModule.AddOption("How do I harness this energy?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule harnessModule = new DialogueModule("To harness energy from a crypt or graveyard, you must commune with the spirits. This requires a ritual, which is both taxing and dangerous. You must be ready to sacrifice part of your own vitality.");
                                                        harnessModule.AddOption("I understand. Thank you.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGraveyardModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, harnessModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, othersModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, britainModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, otherModule));
                            });
                        p.SendGump(new DialogueGump(p, yewModule));
                    });
                detailsModule.AddOption("Where should I start?",
                    p => true,
                    p =>
                    {
                        DialogueModule startModule = new DialogueModule("I would suggest starting at the Britain Graveyard. It is the least dangerous, but still offers valuable materials for a fledgling necromancer. Gather bone shards, spectral residue, and pay attention to the lingering whispers.");
                        startModule.AddOption("Thank you for the advice.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGraveyardModule()));
                            });
                        p.SendGump(new DialogueGump(p, startModule));
                    });
                player.SendGump(new DialogueGump(player, detailsModule));
            });

        graveyardModule.AddOption("I am not interested in graveyards.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return graveyardModule;
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