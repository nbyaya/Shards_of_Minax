using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MavinaTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MavinaTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mavina the Alchemist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(90);
        SetMana(180);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // Robe with a dark blue hue
        AddItem(new Sandals(1175)); // Sandals with a light blue hue
        AddItem(new WizardsHat(1753)); // Matching hat
        AddItem(new SilverMirror()); // Flask item representing her trade

        VirtualArmor = 12;
    }

    public MavinaTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Mavina, an alchemist and baker of sorts. I blend magic into everything I create, even my pastries. Tell me, do you dabble in the culinary arts or seek a taste of something... different?");

        // Dialogue about her alchemical pursuits
        greeting.AddOption("Tell me about your alchemical pursuits.",
            p => true,
            p =>
            {
                DialogueModule alchemyModule = new DialogueModule("Alchemy is a delicate balance of science and magic. I work with rare herbs and ingredients, brewing potions to heal, enhance, and sometimes even transform. But, my favorite pursuit is infusing these powers into something delectable.");
                alchemyModule.AddOption("Infusing powers into pastries?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule infusionModule = new DialogueModule("Yes, pastries! There is nothing more enticing than a simple treat that hides a dark secret. I mix in whispers of the night, the tears of will-o'-wisps, and occasionally, a wish or two. But not all wishes come true without a price.");
                        infusionModule.AddOption("That sounds dangerous. What kind of price?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule priceModule = new DialogueModule("The price is subtle, barely noticeable at first. Those who partake in my delicacies often find themselves coming back for more, craving them. They forget that magic always demands something in return. Their energy, their will, or sometimes... their very soul.");
                                priceModule.AddOption("How can you do this to people?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule moralityModule = new DialogueModule("Oh, dear. It's not as though I force them! People have always wished for shortcuts, easy happiness, and power without effort. I simply offer a way to satisfy their desires. If they choose to indulge, well, that is their decision, is it not?");
                                        moralityModule.AddOption("I suppose that's true...", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        moralityModule.AddOption("That's still wrong. Goodbye.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mavina shrugs, her eyes glinting with a mysterious light.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, moralityModule));
                                    });
                                priceModule.AddOption("That's fascinating. Tell me more.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule moreInfoModule = new DialogueModule("The magic I infuse is drawn from many sources, and each pastry holds a different kind of allure. Some grant fleeting glimpses of a brighter future, others a surge of strength, and some even a chance to glimpse into someone else's dreams. But there are always side effects...");
                                        moreInfoModule.AddOption("Side effects? Like what?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule sideEffectsModule = new DialogueModule("Some people report strange dreams, others lose their sense of time, and a few even say they hear whispers urging them to return. Each creation is unique, and the effects depend on the person's own spirit. It is a risk, but for some, the rewards outweigh the dangers.");
                                                sideEffectsModule.AddOption("Sounds risky. I'd rather not.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                sideEffectsModule.AddOption("I think I'd like to try one...", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Mavina smiles knowingly. 'One day, perhaps. But remember, once you start, it's hard to stop.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, sideEffectsModule));
                                            });
                                        moreInfoModule.AddOption("I think I've heard enough for now.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, moreInfoModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, priceModule));
                            });
                        infusionModule.AddOption("What kind of wishes can your pastries grant?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule wishesModule = new DialogueModule("Ah, wishes are tricky things. Some wish for love, others for wealth, some for strength or beauty. My pastries can amplify desires, sometimes making them a reality. But magic twisted by desperation rarely unfolds as intended. Often, those who wish become trapped in their own greed.");
                                wishesModule.AddOption("What do you mean by 'trapped'?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule trappedModule = new DialogueModule("Their hearts become consumed by their desires. They become unable to live without the feeling of power, the fleeting fulfillment of their wish. They come to me, again and again, each time offering more of themselves until there is nothing left. But to them, it feels like bliss—until it doesn't.");
                                        trappedModule.AddOption("That's terrifying. I don't want anything to do with this.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mavina gives a slight nod. 'As you wish, dear traveler. Some paths are not meant for everyone.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        trappedModule.AddOption("Some risks are worth taking...", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mavina's eyes sparkle. 'Perhaps they are. Perhaps not. Only time will tell.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, trappedModule));
                                    });
                                wishesModule.AddOption("That's too much for me to handle.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, wishesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, infusionModule));
                    });
                alchemyModule.AddOption("I see. Fascinating work!", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, alchemyModule));
            });

        // Trade Option
        greeting.AddOption("Do you need anything from me?",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateTradeModule(p)));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Mavina nods and returns to her work, her eyes glinting with curiosity.");
            });

        return greeting;
    }

    private DialogueModule CreateTradeModule(PlayerMobile player)
    {
        DialogueModule tradeModule = new DialogueModule("Actually, I am in need of a CookingTalisman for my latest experiment. If you have one, I could offer you a SilverMirror and always a MaxxiaScroll in return.");

        tradeModule.AddOption("I have a CookingTalisman for you.",
            p => HasCookingTalisman(p) && CanTradeWithPlayer(p),
            p =>
            {
                CompleteTrade(p);
            });

        tradeModule.AddOption("I don't have one right now.",
            p => !HasCookingTalisman(p),
            p =>
            {
                p.SendMessage("Mavina smiles kindly. 'No worries, perhaps another time.'");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        tradeModule.AddOption("I traded recently; I'll come back later.",
            p => !CanTradeWithPlayer(p),
            p =>
            {
                p.SendMessage("You can only trade once every 10 minutes. Please return later.");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        return tradeModule;
    }

    private bool HasCookingTalisman(PlayerMobile player)
    {
        // Check the player's inventory for CookingTalisman
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CookingTalisman)) != null;
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
        // Remove the CookingTalisman and give the SilverMirror and MaxxiaScroll, then set the cooldown timer
        Item cookingTalisman = player.Backpack.FindItemByType(typeof(CookingTalisman));
        if (cookingTalisman != null)
        {
            cookingTalisman.Delete();
            player.AddToBackpack(new SilverMirror());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the CookingTalisman and receive a SilverMirror and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a CookingTalisman.");
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