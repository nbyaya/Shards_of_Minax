using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class CalderTheGlazier : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CalderTheGlazier() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Calder the Glazier";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // Fancy shirt with a blue hue
        AddItem(new LongPants(1109)); // Dark pants
        AddItem(new Boots(1175)); // Light grey boots
        AddItem(new HalfApron(1161)); // Apron to give an artisan look
        AddItem(new StainedWindow()); // Represents his craft

        VirtualArmor = 15;
    }

    public CalderTheGlazier(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there, traveler. I am Calder, a master of stained glass and light. Have you ever seen the sun's rays through a StainedWindow? Magnificent. Are you here to see my work or perhaps to trade?");

        // Start with dialogue about his craft
        greeting.AddOption("Tell me about your craft.",
            p => true,
            p =>
            {
                DialogueModule craftModule = new DialogueModule("I shape glass with fire and color it with rare minerals. Each piece tells a story, capturing light in a dance of hues. My favorite works are windows for temples and noble homes. There's magic in bringing beauty to light, don't you think?");
                craftModule.AddOption("It sounds beautiful indeed!", 
                    pla => true,
                    pla =>
                    {
                        DialogueModule beautyModule = new DialogueModule("Beauty, yes... but also patience. It takes countless hours of silent work to create something that seems so fleeting when touched by light. There is nothing quite like the satisfaction of patience rewarded. Most people cannot comprehend the dedication it takes.");
                        beautyModule.AddOption("You seem like someone who knows a lot about patience.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule patienceModule = new DialogueModule("Patience is a virtue born of solitude. I have roamed the wasteland for many years, tracking game and raiders alike. Out there, time moves differently. Silence stretches endlessly, and patience becomes as natural as breathing.");
                                patienceModule.AddOption("That sounds like a lonely existence."
                                    , plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule lonelyModule = new DialogueModule("Lonely? Perhaps. But I prefer solitude. People bring noise, distractions, and often trouble. Out there, in the quiet of the wasteland, there is clarity. You hear your own thoughts, unclouded by the worries of others. And yet... I will assist those in genuine need, if I must. Some burdens are worth bearing, even for a lone hunter.");
                                        lonelyModule.AddOption("You're a hunter? What kind of game do you track?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule hunterModule = new DialogueModule("Game... and raiders alike. In the wasteland, there is no distinction sometimes. Raiders prey on the weak, like beasts do, and so I hunt them. Fearless, they call me, but it is not bravery, just necessity. When you have faced a pack of raiders, you learn not to fear the lesser dangers of the land.");
                                                hunterModule.AddOption("Fearless... You must have seen many dangers.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule dangerModule = new DialogueModule("Danger is part of survival. You learn to read the land, track the movements of enemies, and anticipate the unexpected. The wasteland tests you, but it also reveals your strength. There were times I came close to death, facing the unknown, but fear is a wasted emotion. Focus, patience, and precision are what keep you alive.");
                                                        dangerModule.AddOption("I admire your resolve. Could you teach me something about tracking?",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                DialogueModule trackingModule = new DialogueModule("Tracking is an art of observation. Watch for the signs others miss. A broken branch, disturbed earth, the faintest footprint. Patience, again, is key. You wait and you watch until the story of the land unfolds before you. It is not easy, but I could show you, should we ever find ourselves in the wilderness together.");
                                                                trackingModule.AddOption("I'd be honored to learn from you someday.",
                                                                    plaaaaaaa => true,
                                                                    plaaaaaaa =>
                                                                    {
                                                                        plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                    });
                                                                trackingModule.AddOption("Perhaps another time. Thank you for sharing.",
                                                                    plaaaaaaa => true,
                                                                    plaaaaaaa =>
                                                                    {
                                                                        plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                    });
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, trackingModule));
                                                            });
                                                        dangerModule.AddOption("I'm not sure I could face such dangers.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Survival is not about bravery. It is about persistence. Remember that, should you ever find yourself in peril.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, dangerModule));
                                                    });
                                                hunterModule.AddOption("I hope I never run into those raiders.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("If you do, remember to stay quiet, move carefully, and never let fear guide you. Fear is their weapon as much as it is yours.");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, hunterModule));
                                            });
                                        lonelyModule.AddOption("I understand. Solitude has its own strength.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, lonelyModule));
                                    });
                                patienceModule.AddOption("I can see why patience is important in your line of work.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Indeed. Without patience, even the finest craftsman or the keenest hunter will fail.");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, patienceModule));
                            });
                        beautyModule.AddOption("Not everyone has the patience for such work.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("No, they do not. But that is why I do what I do, and they do what they do.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pla.SendGump(new DialogueGump(pla, beautyModule));
                    });
                craftModule.AddOption("Fascinating, but let's move on.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, craftModule));
            });

        // Introduce the trade option
        greeting.AddOption("Are you looking for anything in particular?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Ah, yes. I'm in need of a DrapedBlanket. Its fabric makes for a perfect canvas when I sketch out new designs. If you bring me one, I'll trade you a StainedWindow, and of course, I'll throw in a MaxxiaScroll for your trouble. But remember, I can only make this trade once every 10 minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a DrapedBlanket for me?");
                        tradeModule.AddOption("Yes, I have a DrapedBlanket.",
                            plaa => HasDrapedBlanket(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasDrapedBlanket(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a DrapedBlanket.");
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
                tradeIntroductionModule.AddOption("Maybe another time.",
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
                p.SendMessage("Calder nods, his eyes glinting with the colors of his craft. 'Stay safe out there, traveler. The wasteland is unforgiving.'");
            });

        return greeting;
    }

    private bool HasDrapedBlanket(PlayerMobile player)
    {
        // Check the player's inventory for DrapedBlanket
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DrapedBlanket)) != null;
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
        // Remove the DrapedBlanket and give the StainedWindow and MaxxiaScroll, then set the cooldown timer
        Item drapedBlanket = player.Backpack.FindItemByType(typeof(DrapedBlanket));
        if (drapedBlanket != null)
        {
            drapedBlanket.Delete();
            player.AddToBackpack(new StainedWindow());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the DrapedBlanket and receive a StainedWindow and a MaxxiaScroll in return. Calder thanks you warmly.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a DrapedBlanket.");
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