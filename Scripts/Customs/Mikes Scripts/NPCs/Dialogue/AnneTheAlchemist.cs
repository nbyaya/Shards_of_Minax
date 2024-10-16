using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

public class AnneTheAlchemist : BaseCreature
{
    [Constructable]
    public AnneTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Anne the Alchemist";
        Body = 401; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 10;

        Frozen = true; // Prevent NPC from moving
        CantWalk = true;
    }

    public AnneTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Oh! Greetings, traveler. I didn't hear you approach amidst my concoctions. I'm Anne, an apprentice alchemist on a quest to master the art of healing. How may I assist you?");
        
        greeting.AddOption("Tell me about your quest.",
            player => true,
            player =>
            {
                DialogueModule questModule = new DialogueModule("It's my father, you see. He's fallen ill with a mysterious ailment that none of the local healers can cure. I've dedicated myself to learning everything about alchemy in hopes of finding a remedy. It's daunting, but I won't give up.");
                questModule.AddOption("Perhaps I can help you find a cure.",
                    p => true,
                    p =>
                    {
                        DialogueModule helpModule = new DialogueModule("You would aid me? That's incredibly kind! I've been searching for rare ingredients that are said to have potent healing properties. If you could gather them, we might be able to create a cure together.");
                        helpModule.AddOption("What ingredients do you need?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule ingredientsModule = new DialogueModule("I require Moonlit Petals from the Whispering Forest and a vial of Crystal Dew collected at dawn. They're rare and not easy to obtain, but I believe they're essential.");
                                ingredientsModule.AddOption("I'll return with the ingredients.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You set off to gather the rare ingredients.");
                                    });
                                ingredientsModule.AddOption("That sounds too dangerous for me.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, ingredientsModule));
                            });
                        helpModule.AddOption("I have other matters to attend to.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, helpModule));
                    });
                questModule.AddOption("I wish you luck on your journey.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, questModule));
            });

        greeting.AddOption("Can you teach me about alchemy?",
            player => player.Skills.Alchemy.Base >= 20.0,
            player =>
            {
                DialogueModule teachModule = new DialogueModule("I'd be happy to share what I've learned! Alchemy is both art and science, blending ingredients to unlock their hidden potentials. What would you like to know?");
                teachModule.AddOption("How do I create potions?",
                    p => true,
                    p =>
                    {
                        DialogueModule potionModule = new DialogueModule("Creating potions requires the right ingredients and a steady hand. Each ingredient has unique properties. Combining them at the correct ratios can produce powerful effects. Always be cautious, though—a wrong mixture can have unintended consequences.");
                        potionModule.AddOption("Thank you for the advice.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, potionModule));
                    });
                teachModule.AddOption("What are the rarest ingredients?",
                    p => true,
                    p =>
                    {
                        DialogueModule rareModule = new DialogueModule("Some of the rarest ingredients include Dragon's Breath, Essence of the Phoenix, and Stardust Crystals. They're incredibly hard to find and often guarded by formidable creatures or hidden in remote locations.");
                        rareModule.AddOption("Sounds challenging to obtain.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, rareModule));
                    });
                teachModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, teachModule));
            });

        greeting.AddOption("Why do you doubt your abilities?",
            player => true,
            player =>
            {
                DialogueModule doubtModule = new DialogueModule("It's just... sometimes the path ahead seems overwhelming. The more I learn, the more I realize how little I know. But thinking of my father gives me strength to continue. I have to succeed, for his sake.");
                doubtModule.AddOption("Stay strong; your determination is admirable.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                doubtModule.AddOption("Perhaps you should seek help from others.",
                    p => true,
                    p =>
                    {
                        DialogueModule helpModule = new DialogueModule("You're right. Maybe collaborating with other alchemists could accelerate the search for a cure. Thank you for your insight.");
                        helpModule.AddOption("Glad I could help.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, helpModule));
                    });
                player.SendGump(new DialogueGump(player, doubtModule));
            });

        greeting.AddOption("Do you sell alchemical supplies?",
            player => true,
            player =>
            {
                DialogueModule shopModule = new DialogueModule("I do have some spare supplies I've gathered. Feel free to browse my collection. Perhaps they'll aid you in your own adventures.");
                shopModule.AddOption("Let me see what you have.",
                    p => true,
                    p =>
                    {
                        // Open the player's buy/sell gump or shop interface
                        p.SendMessage("Anne shows you her collection of alchemical supplies.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                shopModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, shopModule));
            });

        greeting.AddOption("Good luck on your journey.",
            player => true,
            player =>
            {
                player.SendMessage("Anne smiles warmly at you.");
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
