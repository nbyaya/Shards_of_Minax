using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FeyrinTheArtisan : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FeyrinTheArtisan() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Feyrin the Artisan";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(1153)); // Fancy Shirt with blue hue
        AddItem(new LongPants(1109)); // Long Pants with dark hue
        AddItem(new Boots(1175)); // Light-colored boots
        AddItem(new Cloak(1157)); // Deep green cloak
        AddItem(new HalfApron(1161)); // Artisan apron
        AddItem(new SmithHammer()); // Smithing hammer to emphasize her craft

        VirtualArmor = 20;
    }

    public FeyrinTheArtisan(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule(player);
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule(PlayerMobile player)
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Feyrin, an artisan devoted to crafting unique tools and collecting rare curiosities. But that's not all there is to me. My past is one of fire, fists, and fierce battles. Do you dare to delve deeper?");

        // Dialogue about her past
        greeting.AddOption("You seem to have a strong spirit. What's your story?",
            p => true,
            p =>
            {
                DialogueModule pastModule = new DialogueModule("Ah, my story is not one I share lightly, but you've asked, and perhaps it will serve as a warning. I was once a champion in the underground fighting rings. Fierce and undefeated. They called me 'The Iron Fist of the Pit.' I fought because I had to, but in that life, you either grow callous or crumble.");

                // Deeper dive into her past
                pastModule.AddOption("How did you become a fighter?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule originModule = new DialogueModule("I had no choice. My family was in debt, and the underground rings were the only way to earn quick coin. I was young, naive, and full of rage. At first, I fought just to survive, but soon I became addicted to the cheers, the thrill, and the fear in my opponent's eyes. I became something monstrous, something I am not proud of.");

                        originModule.AddOption("Do you regret it?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule regretModule = new DialogueModule("Regret? Every scar on my body is a reminder of what I did, of the pain I caused, not just to others, but to myself. But I am not one to wallow in regret. I seek redemption now, to use what I know to help others. To train them to defend themselves, not to destroy. I will never be free of my past, but I can choose what I do with my future.");

                                regretModule.AddOption("How are you seeking redemption?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule redemptionModule = new DialogueModule("I train others. Those who need strength, who need to protect themselves, I guide them. I teach them the art of combat, but I also teach them restraint. Strength without wisdom is destruction. Every student I train is a step towards balance, a step away from the darkness I once embraced.");

                                        redemptionModule.AddOption("Could you train me?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule trainModule = new DialogueModule("Training is not for the faint of heart. It takes discipline, courage, and the willingness to face your own demons. I can show you the path, but you must walk it yourself. If you are truly committed, perhaps we can arrange something.");

                                                trainModule.AddOption("I'm ready for the challenge.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Feyrin nods approvingly. 'Very well. Meet me at dawn near the old oak tree. We will begin then.'");
                                                    });

                                                trainModule.AddOption("I need more time to prepare.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Feyrin gives you a knowing smile. 'Take your time. True strength comes not from haste, but from patience.'");
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, trainModule));
                                            });

                                        redemptionModule.AddOption("Thank you for sharing your story.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, redemptionModule));
                                    });

                                regretModule.AddOption("It must have been hard to change.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule changeModule = new DialogueModule("Change is the hardest battle I've fought. The arena was brutal, but predictable. Changing who I was, confronting my own reflection, that was the real fight. But it was worth it. Every day, I work to be better than I was the day before.");

                                        changeModule.AddOption("I admire your strength.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Feyrin gives you a fierce smile, her eyes glinting. 'Strength isn't just about muscle. It's about facing yourself, unflinching.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, changeModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, regretModule));
                            });

                        originModule.AddOption("That sounds like a tough life.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Feyrin nods solemnly. 'It was. But it made me who I am. I can't change the past, but I can forge a new future.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, originModule));
                    });

                pastModule.AddOption("Do you still fight?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule fightModule = new DialogueModule("I fight, but not like I used to. Now, my battles are within. To control my temper, to guide others rather than dominate them. The scars I bear are reminders of those battles, both won and lost. But I will not let the rage define me anymore.");

                        fightModule.AddOption("That's inspiring. Thank you for sharing.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        fightModule.AddOption("Can you show me some of your moves?",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Feyrin raises an eyebrow, then gives a slight grin. 'Maybe one day. For now, focus on your own strength. Master the basics before asking for the advanced.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, fightModule));
                    });

                p.SendGump(new DialogueGump(p, pastModule));
            });

        // Dialogue about her craft
        greeting.AddOption("What kind of tools do you craft?",
            p => true,
            p =>
            {
                DialogueModule toolsModule = new DialogueModule("I craft tools that embody the spirit of craftsmanship. Every piece is made with care, from the simplest hammer to the most intricate of contraptions. Crafting, like combat, requires precision, dedication, and passion. Perhaps you have a rare item to trade?");

                toolsModule.AddOption("Do you have something for trade?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Indeed, I am looking for a RibbonAward. If you have one, I can offer you a well-crafted ToolBox and a special MaxxiaScroll as a token of my appreciation.");

                        tradeModule.AddOption("I have a RibbonAward to trade.",
                            pla => HasRibbonAward(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });

                        tradeModule.AddOption("I don't have a RibbonAward right now.",
                            pla => !HasRibbonAward(pla),
                            pla =>
                            {
                                pla.SendMessage("Come back when you have a RibbonAward. I shall be here, continuing my craft.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });

                toolsModule.AddOption("Perhaps another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, toolsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Feyrin nods and returns to her work, the sound of hammering resuming softly.");
            });

        return greeting;
    }

    private bool HasRibbonAward(PlayerMobile player)
    {
        // Check the player's inventory for RibbonAward
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RibbonAward)) != null;
    }

    private bool CanTradeWithPlayer(PlayerMobile player)
    {
        // Check if the player can trade, based on the 10-minute cooldown
        if (LastTradeTime.TryGetValue(player, out DateTime lastTrade))
        {
            return (DateTime.UtcNow - lastTrade).TotalMinutes >= 10;
        }
        return true;
    }

    private void CompleteTrade(PlayerMobile player)
    {
        // Remove the RibbonAward and give the ToolBox and MaxxiaScroll, then set the cooldown timer
        Item ribbonAward = player.Backpack.FindItemByType(typeof(RibbonAward));
        if (ribbonAward != null)
        {
            ribbonAward.Delete();
            player.AddToBackpack(new ToolBox());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the RibbonAward and receive a ToolBox and a MaxxiaScroll in return. Feyrin smiles warmly at you.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a RibbonAward.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
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