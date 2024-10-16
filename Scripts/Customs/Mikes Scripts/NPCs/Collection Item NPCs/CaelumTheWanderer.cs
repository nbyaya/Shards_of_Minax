using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CaelumTheWanderer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CaelumTheWanderer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Caelum the Wanderer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 100;
        Karma = 100;

        // Outfit
        AddItem(new FancyShirt(1157)); // Fancy shirt with a blue hue
        AddItem(new LongPants(1109)); // Dark gray pants
        AddItem(new Boots(1175)); // Boots with a light brown hue
        AddItem(new Cloak(1953)); // A dark, mysterious cloak
        AddItem(new FeatheredHat(1150)); // A unique feathered hat

        VirtualArmor = 15;
    }

    public CaelumTheWanderer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Caelum, a wanderer of ancient places and collector of curious items. You seem like someone who appreciates the rare and wonderful. How can I assist you today?");

        // Dialogue options
        greeting.AddOption("Tell me about your travels.",
            p => true,
            p =>
            {
                DialogueModule travelsModule = new DialogueModule("I've been to the ruins of Antheria, the shimmering caves of Eredel, and beyond. Each place holds its mysteries, but it's the relics and forgotten treasures that captivate me the most. They tell stories untold.");
                travelsModule.AddOption("What kind of treasures?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule treasuresModule = new DialogueModule("Oh, many things! Ancient scrolls, gemstones imbued with magic, and even mundane items with hidden secrets. Each holds a piece of history, a fragment of the past waiting to be uncovered.");
                        treasuresModule.AddOption("Tell me more about these ancient scrolls.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule scrollsModule = new DialogueModule("The scrolls I have found speak of forgotten prophecies, warnings from times long past. One scroll, in particular, speaks of a cosmic entity, a being from beyond the stars, that was worshipped by ancient sailors. They believed it controlled the tides and brought with it knowledge... and madness.");
                                scrollsModule.AddOption("A cosmic entity? Tell me more.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule entityModule = new DialogueModule("Aye, a cosmic entity. They called it 'The Deep One.' It is said that those who gazed upon it were forever changed. I saw it once, far out at sea, during a storm that turned the sky to pitch. It whispered to me, promised secrets, but also horrors. Since that day, I've never been the same.");
                                        entityModule.AddOption("What did it whisper to you?",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                DialogueModule whisperModule = new DialogueModule("The whispers were not in any language I knew, yet I understood them. They spoke of the abyss, of endless depths where time has no meaning. They spoke of a coming storm, one that will carry more than just water. It will bring shadows, and those who are not prepared will be lost to the tides.");
                                                whisperModule.AddOption("Is this storm still coming?",
                                                    plaabcd => true,
                                                    plaabcd =>
                                                    {
                                                        DialogueModule stormModule = new DialogueModule("Aye, the storm is coming. The signs are there for those who can see them. The waves grow restless, and the winds carry a scent of something... unnatural. The Deep One is stirring, and with it comes the storm. I do not know when, but I know it is inevitable.");
                                                        stormModule.AddOption("How can we prepare?",
                                                            plaabcde => true,
                                                            plaabcde =>
                                                            {
                                                                DialogueModule prepareModule = new DialogueModule("To prepare, you must seek knowledge. The scrolls, the relics, the whispers of old sailors like me. Gather what you can, for knowledge is the only defense against the unknown. And remember, the Deep One cannot be fought with steel or magic—it can only be resisted with understanding and willpower.");
                                                                prepareModule.AddOption("Thank you for the warning.",
                                                                    plaabcdef => true,
                                                                    plaabcdef =>
                                                                    {
                                                                        plaabcdef.SendGump(new DialogueGump(plaabcdef, CreateGreetingModule(plaabcdef)));
                                                                    });
                                                                plaabcde.SendGump(new DialogueGump(plaabcde, prepareModule));
                                                            });
                                                        plaabcd.SendGump(new DialogueGump(plaabcd, stormModule));
                                                    });
                                                plaabc.SendGump(new DialogueGump(plaabc, whisperModule));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, entityModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, scrollsModule));
                            });
                        treasuresModule.AddOption("Fascinating! Thank you.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, treasuresModule));
                    });
                travelsModule.AddOption("Maybe some other time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, travelsModule));
            });

        // Trade option
        greeting.AddOption("Do you have something to trade?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in search of a PicnicDayBasket. If you happen to have one, I can offer you something quite rare in return: a Meteorite. Additionally, I shall give you a MaxxiaScroll as a token of my appreciation. But beware, I can only make this exchange once every ten minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a PicnicDayBasket for me?");
                        tradeModule.AddOption("Yes, I have a PicnicDayBasket.",
                            plaa => HasPicnicDayBasket(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasPicnicDayBasket(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a PicnicDayBasket.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pla.SendGump(new DialogueGump(pla, tradeModule));
                    });
                tradeIntroductionModule.AddOption("Perhaps another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Caelum nods and smiles knowingly.");
            });

        return greeting;
    }

    private bool HasPicnicDayBasket(PlayerMobile player)
    {
        // Check the player's inventory for PicnicDayBasket
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(PicnicDayBasket)) != null;
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
        // Remove the PicnicDayBasket and give the Meteorite and MaxxiaScroll, then set the cooldown timer
        Item picnicDayBasket = player.Backpack.FindItemByType(typeof(PicnicDayBasket));
        if (picnicDayBasket != null)
        {
            picnicDayBasket.Delete();
            player.AddToBackpack(new Meteorite());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the PicnicDayBasket and receive a Meteorite and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a PicnicDayBasket.");
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