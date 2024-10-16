using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

public class MarlaTheViper : BaseCreature
{
    [Constructable]
    public MarlaTheViper() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marla 'The Viper'";
        Body = 401; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(100);
        SetInt(70);

        SetHits(120);
        SetMana(50);
        SetStam(80);

        Fame = 0;
        Karma = -1000;

        VirtualArmor = 20;

        Frozen = true; // Prevent NPC from moving
        CantWalk = true;
    }

    public MarlaTheViper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Well, well, if it isn't a brave soul wandering into my territory. I'm Marla, but you may know me better as 'The Viper.' What brings you to my domain? Do you seek power, or are you here to negotiate?");

        greeting.AddOption("I'm here to confront you about your actions.",
            player => true,
            player =>
            {
                DialogueModule confrontModule = new DialogueModule("Confront me? Bold choice. But remember, many have tried and failed. What do you think you can do to stop me? You see, power respects power, and I hold the cards here.");
                confrontModule.AddOption("I won't let you continue your reign of terror.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You stand your ground, ready for a confrontation.");
                        // Initiate combat or further dialogue based on player choices.
                    });
                confrontModule.AddOption("What if we could work together instead?",
                    p => true,
                    p =>
                    {
                        DialogueModule allianceModule = new DialogueModule("Work with me? Intriguing proposal. But what could you possibly offer that I don’t already have? I’m listening, but tread carefully.");
                        allianceModule.AddOption("I can help you expand your influence.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        allianceModule.AddOption("I know about a rival that could threaten you.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        allianceModule.AddOption("Never mind. I'll find another way.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, allianceModule));
                    });
                confrontModule.AddOption("You're a monster, and you'll pay for your crimes!",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Marla laughs coldly, “Crimes? Or merely survival? In this world, one must do what they must to thrive. Perhaps it is you who needs to reconsider your stance.”");
                        // Potential conflict or combat initiation
                    });
                player.SendGump(new DialogueGump(player, confrontModule));
            });

        greeting.AddOption("What is the Shadow Syndicate really after?",
            player => true,
            player =>
            {
                DialogueModule syndicateModule = new DialogueModule("Ah, the curious mind! The Syndicate is about power and influence, my friend. We take what others have wasted or ignored. It’s a game of chess, and I intend to be the queen on the board.");
                syndicateModule.AddOption("Is that all? I expected more ambition.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule ambitionModule = new DialogueModule("Ambition? Oh, I have plans far beyond this city. But why share them with someone like you? Perhaps I could use your skills. Are you interested in climbing the ranks?");
                        ambitionModule.AddOption("Count me in!",
                            p => true,
                            p => 
                            {
                                p.SendMessage("You express your willingness to join her cause.");
                                // Potential recruitment or quest initiation
                            });
                        ambitionModule.AddOption("I'm not interested in your schemes.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, ambitionModule));
                    });
                syndicateModule.AddOption("How do you handle your rivals?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule rivalsModule = new DialogueModule("Rivals are dealt with swiftly. Fear is a tool, and I wield it like a dagger. I’d be happy to demonstrate on one of my enemies, but I could use a capable ally. Are you interested?");
                        rivalsModule.AddOption("Absolutely, let's take them down!",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You express eagerness to confront her enemies.");
                                // Quest or combat initiation
                            });
                        rivalsModule.AddOption("No, I want no part of this.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, rivalsModule));
                    });
                player.SendGump(new DialogueGump(player, syndicateModule));
            });

        greeting.AddOption("You seem to care for your own. Why not use that for good?",
            player => true,
            player =>
            {
                DialogueModule careModule = new DialogueModule("Care? A fool's game! My loyalty lies with those who are useful to me. But perhaps you see a glimmer of potential. If you can prove your worth, I might just consider your ideals.");
                careModule.AddOption("I will prove my worth to you.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You express determination to gain her favor.");
                        // Initiate quest or test of loyalty
                    });
                careModule.AddOption("I’ll find a way to stop you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, careModule));
            });

        greeting.AddOption("What are your thoughts on the people of East Britain?",
            player => true,
            player =>
            {
                DialogueModule thoughtsModule = new DialogueModule("The people? They’re a mixed bag. Most are weak, clinging to their pitiful lives. But some have potential—those who dare to challenge me. It’s a dance of survival out here. I respect strength, even in my enemies.");
                thoughtsModule.AddOption("Strength? You mean ruthlessness.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule ruthlessnessModule = new DialogueModule("Ruthlessness is a necessary trait in this city. Compassion can only take you so far when faced with the reality of the streets. You either adapt or perish. Which path will you choose?");
                        ruthlessnessModule.AddOption("I choose to protect the weak.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You firmly declare your intention to protect those in need.");
                                // Potentially set up a conflict
                            });
                        ruthlessnessModule.AddOption("I understand your perspective.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You acknowledge her point of view, perhaps signaling an understanding.");
                                // This could lead to deeper negotiations
                            });
                        player.SendGump(new DialogueGump(player, ruthlessnessModule));
                    });
                thoughtsModule.AddOption("What if I can help them fight back?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule helpModule = new DialogueModule("Help them? Interesting proposition. But would they accept aid from someone like you? There’s power in unity, but trust is a rare commodity in East Britain.");
                        helpModule.AddOption("I’ll earn their trust through my actions.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You confidently state your intent to earn their respect.");
                                // Quest initiation to help the community
                            });
                        helpModule.AddOption("Perhaps I should just take you down.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You threaten her, which could lead to a confrontation.");
                                // Potential combat scenario
                            });
                        player.SendGump(new DialogueGump(player, helpModule));
                    });
                player.SendGump(new DialogueGump(player, thoughtsModule));
            });

        greeting.AddOption("What do you desire most in this world?",
            player => true,
            player =>
            {
                DialogueModule desireModule = new DialogueModule("Desire? Power, of course. Control over my territory and respect from those who dwell within it. I want to carve my name into the annals of history—forever remembered as a queen of shadows. But you, what do you seek?");
                desireModule.AddOption("I seek justice for the oppressed.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule justiceModule = new DialogueModule("Justice? An admirable but naive quest. Justice is often a facade; power is the true currency in this world. I could teach you a different way, should you wish to learn.");
                        justiceModule.AddOption("I'm willing to learn.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You express a willingness to learn from her.");
                                // Possible quest or training opportunity
                            });
                        justiceModule.AddOption("I will find my own path.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, justiceModule));
                    });
                desireModule.AddOption("I desire power as well.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule sharedAmbitionModule = new DialogueModule("Power, then. You might just have a place in my Syndicate. Together, we could rule these streets, instilling fear and respect in our enemies. Are you ready to step into the shadows?");
                        sharedAmbitionModule.AddOption("I am ready to embrace the shadows.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You express your readiness to join her cause.");
                                // Potential recruitment quest
                            });
                        sharedAmbitionModule.AddOption("No, I seek a different path.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, sharedAmbitionModule));
                    });
                player.SendGump(new DialogueGump(player, desireModule));
            });

        greeting.AddOption("Goodbye, Marla.",
            player => true,
            player =>
            {
                player.SendMessage("Marla smirks, her eyes glinting. “Remember, the shadows are always watching. Choose your alliances wisely.”");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
