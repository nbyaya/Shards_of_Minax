using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MarekTheTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MarekTheTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marek the Trader";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(80);

        SetHits(100);
        SetMana(100);
        SetStam(70);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(1750)); // A stylish blue shirt
        AddItem(new LongPants(1175));  // Dark brown pants
        AddItem(new Sandals(1109));    // Simple sandals
        AddItem(new Cloak(1125));      // Green cloak for uniqueness

        VirtualArmor = 15;
    }

    public MarekTheTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I'm Marek, a trader of rare goods. If you have something interesting, I might be willing to make a trade.");

        greeting.AddOption("What kind of trade are you interested in?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I'm currently looking for a Satchel. If you have one, I can offer you a delicious Hotdog in return, along with a special MaxxiaScroll as a token of appreciation. But remember, I can only do this once every 10 minutes.");
                
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a Satchel for me?");
                        tradeModule.AddOption("Yes, I have a Satchel.",
                            plaa => HasSatchel(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasSatchel(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a Satchel.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                tradeIntroductionModule.AddOption("Perhaps another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Why do the townsfolk speak so highly of you?",
            p => true,
            p =>
            {
                DialogueModule praiseModule = new DialogueModule("Ah, the good people of the town do love my hotdogs! They say they're the best in the region, and who am I to argue? But, heh, they have no idea how I source my meats. Let's just say, I go to great lengths to ensure every bite is... unforgettable.");
                
                praiseModule.AddOption("What do you mean by 'great lengths'?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule secretiveModule = new DialogueModule("Well now, that's a trade secret, isn't it? Some things are better left unknown, don't you think? But I can tell you this much - I don't let anything go to waste, and I have my own ways of acquiring the finest ingredients. The less you know, the better for your appetite.");
                        
                        secretiveModule.AddOption("That sounds rather ruthless.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule ruthlessModule = new DialogueModule("Ruthless? Hah! Maybe. But in this world, you have to be a bit ruthless to survive. People want the best, and I'm here to provide it. If that means getting my hands dirty, so be it. At the end of the day, everyone leaves with a full belly and a smile, and that's what matters, isn't it?");
                                
                                ruthlessModule.AddOption("I suppose... but what about those who disappear?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule disappearModule = new DialogueModule("Disappear? Oh, you've been listening to those old wives' tales, haven't you? People disappear all the time. Bandits, monsters, accidents... But if you're implying I'm involved, well, I'd say that's quite the accusation. I'd be careful with such words if I were you.");
                                        
                                        disappearModule.AddOption("I didn't mean to offend.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marek narrows his eyes for a moment, then smiles jovially.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        disappearModule.AddOption("I think I'll be going now.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marek's smile fades slightly as you make your exit.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, disappearModule));
                                    });
                                ruthlessModule.AddOption("I guess it's none of my business.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Marek chuckles, his eyes glinting with amusement.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, ruthlessModule));
                            });
                        
                        secretiveModule.AddOption("I respect your secrets.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Marek grins widely. 'Good, good. Curiosity can be dangerous, you know.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, secretiveModule));
                    });
                
                praiseModule.AddOption("You must be quite popular then!",
                    pl => true,
                    pl =>
                    {
                        DialogueModule popularModule = new DialogueModule("Oh, absolutely! People can't get enough of my goods. There's always a line at my stall during festivals. And I must admit, I do love the attention. Life is too short to be anything but jovial, wouldn't you agree?");
                        
                        popularModule.AddOption("I can see why. You seem like quite the character.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Marek laughs heartily, clearly pleased. 'Ah, flattery will get you everywhere, my friend!'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        popularModule.AddOption("I suppose so. Farewell for now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Marek waves cheerfully as you take your leave.");
                            });
                        pl.SendGump(new DialogueGump(pl, popularModule));
                    });
                
                p.SendGump(new DialogueGump(p, praiseModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Marek nods politely as you take your leave.");
            });

        return greeting;
    }

    private bool HasSatchel(PlayerMobile player)
    {
        // Check the player's inventory for Satchel
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(Satchel)) != null;
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
        // Remove the Satchel and give the Hotdog and MaxxiaScroll, then set the cooldown timer
        Item satchel = player.Backpack.FindItemByType(typeof(Satchel));
        if (satchel != null)
        {
            satchel.Delete();
            player.AddToBackpack(new Hotdogs());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Satchel and receive a Hotdog and a MaxxiaScroll in return. Delicious!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Satchel.");
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