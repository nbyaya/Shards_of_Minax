using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Mao Zedong")]
public class MaoZedong : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MaoZedong() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mao Zedong";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(95);
        SetHits(72);

        // Appearance
        AddItem(new LongPants() { Hue = 1155 });
        AddItem(new Doublet() { Hue = 1111 });
        AddItem(new Boots() { Hue = 1111 });
        AddItem(new ShepherdsCrook() { Name = "The Long March" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public MaoZedong(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Mao Zedong, the leader of the proletariat! How may I assist you, comrade?");

        greeting.AddOption("Tell me about your ideals.",
            player => true,
            player =>
            {
                DialogueModule idealsModule = new DialogueModule("The ideals of communism call for the equal distribution of wealth and the eradication of class distinctions. It's the beacon of hope for many.");
                idealsModule.AddOption("What do you mean by equality?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateEqualityModule()));
                    });
                idealsModule.AddOption("How can I support these ideals?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule supportModule = new DialogueModule("You can support these ideals by educating others, standing against oppression, and working to unite the working class.");
                        supportModule.AddOption("How do I educate others?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateEducateModule()));
                            });
                        supportModule.AddOption("What about activism?",
                            p => true,
                            p =>
                            {
                                DialogueModule activismModule = new DialogueModule("Activism is crucial! Join protests, write pamphlets, and use your voice to spread our message. Every action counts!");
                                activismModule.AddOption("I will get involved!",
                                    pq => true,
                                    pq =>
                                    {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, activismModule));
                            });
                        pl.SendGump(new DialogueGump(pl, supportModule));
                    });
                player.SendGump(new DialogueGump(player, idealsModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My job is to spread the ideals of communism and unite the working class!")));
            });

        greeting.AddOption("Tell me about the proletariat.",
            player => true,
            player =>
            {
                DialogueModule proletariatModule = new DialogueModule("The proletariat is the working class, those who toil for their bread. They are the backbone of our great revolution.");
                proletariatModule.AddOption("What are their struggles?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule strugglesModule = new DialogueModule("The struggles of the proletariat include exploitation by the bourgeoisie, lack of rights, and a voice in governance. We must fight for their dignity!");
                        strugglesModule.AddOption("How can we help them?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateHelpModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, strugglesModule));
                    });
                player.SendGump(new DialogueGump(player, proletariatModule));
            });

        greeting.AddOption("How can I help?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    lastRewardTime = DateTime.UtcNow;
                    player.AddToBackpack(new EarringSlotChangeDeed()); // Replace with the appropriate reward item
                    player.SendGump(new DialogueGump(player, new DialogueModule("This token is a symbol of our appreciation for your support. Take it as a reminder of the cause you're helping advance.")));
                }
                else
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
            });

        return greeting;
    }

    private DialogueModule CreateEqualityModule()
    {
        DialogueModule equalityModule = new DialogueModule("Equality means that every individual, regardless of their background, has the same rights and opportunities. It is what we strive for, day and night.");
        equalityModule.AddOption("What about inequality?",
            pl => true,
            pl =>
            {
                DialogueModule inequalityModule = new DialogueModule("Inequality arises from greed and exploitation. It breeds division and suffering. We must work together to dismantle these structures.");
                inequalityModule.AddOption("How can we dismantle inequality?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateDismantleInequalityModule()));
                    });
                pl.SendGump(new DialogueGump(pl, inequalityModule));
            });
        equalityModule.AddOption("Thank you for explaining.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return equalityModule;
    }

    private DialogueModule CreateEducateModule()
    {
        DialogueModule educateModule = new DialogueModule("Educating others involves sharing knowledge and creating awareness about our cause. You can start by discussing these topics with friends.");
        educateModule.AddOption("What resources can I use?",
            pl => true,
            pl =>
            {
                DialogueModule resourcesModule = new DialogueModule("Books, documentaries, and online articles are great resources. Consider forming study groups to discuss these materials!");
                resourcesModule.AddOption("I will gather materials!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, resourcesModule));
            });
        return educateModule;
    }

    private DialogueModule CreateHelpModule()
    {
        DialogueModule helpModule = new DialogueModule("You can help by volunteering for local organizations that support workers' rights or by participating in community outreach.");
        helpModule.AddOption("I will volunteer!",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return helpModule;
    }

    private DialogueModule CreateDismantleInequalityModule()
    {
        DialogueModule dismantleModule = new DialogueModule("To dismantle inequality, we must challenge unjust laws, support worker unions, and engage in peaceful protests. Every voice matters!");
        dismantleModule.AddOption("I will join the movement!",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return dismantleModule;
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
