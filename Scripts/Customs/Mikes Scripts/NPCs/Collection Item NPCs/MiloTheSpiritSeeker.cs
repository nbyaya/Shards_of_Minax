using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MiloTheSpiritSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MiloTheSpiritSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Milo the Spirit Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(110);

        SetHits(100);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows()); // Mysterious dark hooded robe
        AddItem(new Boots(1150)); // Dark boots
        AddItem(new BodySash(1153)); // Silver sash
        AddItem(new Lantern() { Movable = false }); // Always carrying a lantern

        VirtualArmor = 15;
    }

    public MiloTheSpiritSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Milo, a seeker of the restless spirits. The ethereal world has secrets that only a few dare to uncover. Do you have something... spooky for me?");

        // Start with dialogue about his work
        greeting.AddOption("What do you mean by 'restless spirits'?",
            p => true,
            p =>
            {
                DialogueModule spiritsModule = new DialogueModule("The restless spirits are those who have not found peace. They linger in the shadows, bound to objects or places. I seek to understand them, and sometimes... help them move on.");
                spiritsModule.AddOption("How can you help them?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule helpModule = new DialogueModule("I use items imbued with spiritual energy to communicate with them. One such item is the SpookyGhost. It holds the essence of a trapped spirit, and with it, I can guide them to the beyond.");
                        helpModule.AddOption("Interesting, tell me more.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule moreInfoModule = new DialogueModule("Ah, you seek more knowledge, do you? The spirits speak in riddles and whispers. Only those with a cunning mind can understand their needs. I have spent years honing my craft, not just with wisdom, but with an iron will. The spirits do not respect weakness.");
                                moreInfoModule.AddOption("Why do the spirits respect you?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule respectModule = new DialogueModule("They respect me because they fear me. I am not just a seeker of spirits; I am a leader, a commander. In my past life, I led a raider gang—a band of ruthless men and women who carved out a living in the wastelands. It was through fear, and the promise of power, that I commanded their loyalty. The spirits are no different. They need to be shown strength before they will listen.");
                                        respectModule.AddOption("You were a raider? Tell me more.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule raiderModule = new DialogueModule("Yes, I was the leader of the 'Ashen Vultures.' We took what we needed, we burned what we couldn't carry, and we crushed anyone who stood in our way. It was a brutal life, but it taught me valuable lessons—lessons in loyalty, fear, and charisma. I learned how to bend others to my will, whether they were living... or dead.");
                                                raiderModule.AddOption("How did you go from raiding to spirit seeking?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule transitionModule = new DialogueModule("After years of violence, I began to see the faces of those I had wronged. They haunted me, whispering in the dead of night. At first, I thought it was guilt. But then I realized—they were spirits, bound to me by the deeds I had done. I knew I could either let them destroy me, or I could learn to control them. I chose power. I chose to harness their energy, and in doing so, I became the Spirit Seeker.");
                                                        transitionModule.AddOption("That's a dark path you've walked.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                DialogueModule darkPathModule = new DialogueModule("Dark? Perhaps. But the world is not a kind place, traveler. Only those willing to embrace the shadows can hope to reshape it. My ultimate goal is to build an empire—not of the living, but of both the living and the dead. From the ashes of the old world, I will raise a new order, one where the restless spirits serve a greater purpose.");
                                                                darkPathModule.AddOption("An empire of the living and the dead?",
                                                                    plaaaaaaa => true,
                                                                    plaaaaaaa =>
                                                                    {
                                                                        DialogueModule empireModule = new DialogueModule("Yes, an empire unlike any other. The living are fragile, but the dead are eternal. Imagine a world where no one need fear death, because death itself is part of the empire. The spirits will be our allies, our soldiers, and our servants. I will be the one to bridge the gap between life and death, and through this, I will achieve greatness.");
                                                                        empireModule.AddOption("That sounds both terrifying and fascinating.",
                                                                            plaaaaaaaa => true,
                                                                            plaaaaaaaa =>
                                                                            {
                                                                                plaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaa, CreateGreetingModule(plaaaaaaaa)));
                                                                            });
                                                                        plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, empireModule));
                                                                    });
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, darkPathModule));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, transitionModule));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, raiderModule));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, respectModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, moreInfoModule));
                            });
                        pl.SendGump(new DialogueGump(pl, helpModule));
                    });
                spiritsModule.AddOption("That's a bit too eerie for me.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, spiritsModule));
            });

        // Trade option
        greeting.AddOption("Do you need anything from me?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am looking for a SpookyGhost. If you have one, I can offer you a FireRelic and a MaxxiaScroll in return. However, the spirits must rest, so I can only perform this exchange once every 10 minutes per person.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a SpookyGhost for me?");
                        tradeModule.AddOption("Yes, I have a SpookyGhost.",
                            plaa => HasSpookyGhost(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasSpookyGhost(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a SpookyGhost.");
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
                p.SendMessage("Milo nods solemnly, his eyes glinting with a mysterious light.");
            });

        return greeting;
    }

    private bool HasSpookyGhost(PlayerMobile player)
    {
        // Check the player's inventory for SpookyGhost
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SpookyGhost)) != null;
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
        // Remove the SpookyGhost and give the FireRelic and MaxxiaScroll, then set the cooldown timer
        Item spookyGhost = player.Backpack.FindItemByType(typeof(SpookyGhost));
        if (spookyGhost != null)
        {
            spookyGhost.Delete();
            player.AddToBackpack(new FireRelic());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the SpookyGhost and receive a FireRelic and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a SpookyGhost.");
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