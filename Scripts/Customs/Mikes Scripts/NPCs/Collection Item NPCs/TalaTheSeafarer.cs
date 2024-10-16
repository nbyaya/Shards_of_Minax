using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TalaTheSeafarer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TalaTheSeafarer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tala the Seafarer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(100);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new TricorneHat(1153)); // A sailor's hat with a blue hue
        AddItem(new FancyShirt(1152)); // A blue fancy shirt
        AddItem(new LongPants(1109)); // Sea-green pants
        AddItem(new Boots(1109)); // Matching sea-green boots
        AddItem(new Cutlass()); // Tala always carries her trusted cutlass

        VirtualArmor = 15;
    }

    public TalaTheSeafarer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy there, traveler! I'm Tala, a retired seafarer turned collector of rare treasures. Do you have an interest in tales of the sea or perhaps a trade?");

        // Start with dialogue about her seafaring days
        greeting.AddOption("Tell me about your seafaring days.", 
            p => true, 
            p =>
            {
                DialogueModule seaTaleModule = new DialogueModule("The sea is a vast and unforgiving mistress. I've sailed through storms that could tear the sky apart, and seen creatures that would make your hair stand on end. But I wouldn't trade those years for anything. Do you wish to hear about the Kraken's Lament, the Lost Isles, or perhaps my fall from grace?");

                // Nested options for each story
                seaTaleModule.AddOption("Tell me about the Kraken's Lament.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule krakenModule = new DialogueModule("The Kraken's Lament is a haunting ballad sung by sailors who survived its wrath. They say the Kraken appears when the sea itself cries, pulling entire ships to the depths. Few have lived to tell the tale, and those who did were never quite the same.");
                        krakenModule.AddOption("That sounds terrifying!", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        krakenModule.AddOption("How did you survive such a creature?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule survivalModule = new DialogueModule("Ah, now that's where my resourcefulness comes into play. The Kraken may be strong, but it's not very bright. A well-placed decoy and a quick escape saved my crew and me, though not without losing half the ship in the process.");
                                survivalModule.AddOption("You must be quite resourceful indeed.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, survivalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, krakenModule));
                    });

                seaTaleModule.AddOption("Tell me about the Lost Isles.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule lostIslesModule = new DialogueModule("The Lost Isles are said to appear only once every decade, hidden by thick mists. They are filled with treasures and dangers beyond imagination. Many have tried to reach them, but few have returned. I myself glimpsed their shores once, but the sea had other plans.");
                        lostIslesModule.AddOption("Perhaps I'll seek them out one day.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        lostIslesModule.AddOption("What kind of treasures are there?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule treasuresModule = new DialogueModule("Ah, the treasures of the Lost Isles... Golden idols, ancient artifacts, gems the size of your fist. But beware, for each treasure is fiercely guarded. Not just by traps, but by the spirits of those who failed before you. They say the islands themselves are alive, and they don’t take kindly to intruders.");
                                treasuresModule.AddOption("Sounds like a place for the brave and foolish.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, treasuresModule));
                            });
                        pl.SendGump(new DialogueGump(pl, lostIslesModule));
                    });

                seaTaleModule.AddOption("Tell me about your fall from grace.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule exileModule = new DialogueModule("Ah, my fall from grace... I was once part of the upper class, living a life of luxury. But my arrogance got the best of me. I crossed the wrong people, made enemies I shouldn’t have, and found myself exiled to the wasteland. Now, I use what charm I have left to survive, and perhaps one day, reclaim what was taken from me.");
                        exileModule.AddOption("You sound like someone who won't give up easily.", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule determinationModule = new DialogueModule("Indeed. I may have lost my status, but I have not lost my will. Every person I meet, every deal I make, brings me one step closer to my goal. And mark my words, I will rise again, by any means necessary.");
                                determinationModule.AddOption("I admire your determination.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, determinationModule));
                            });
                        exileModule.AddOption("What exactly did you do to be exiled?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule betrayalModule = new DialogueModule("Ah, the details... I betrayed someone very powerful. A nobleman who thought he could control me. I took what he valued most, and he made sure I paid the price. But even in exile, I consider it a victory. I may be out here, but he lives in fear that one day I'll return.");
                                betrayalModule.AddOption("Sounds like you still hold a grudge.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule grudgeModule = new DialogueModule("A grudge? No, it's much more than that. It's a promise. One day, I'll reclaim everything he took from me, and more. The wasteland has taught me resilience, and I've learned how to turn every setback into an opportunity.");
                                        grudgeModule.AddOption("I hope you get your chance.", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, grudgeModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, betrayalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, exileModule));
                    });

                p.SendGump(new DialogueGump(p, seaTaleModule));
            });

        // Trade Option
        greeting.AddOption("Do you need anything for your collection?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Aye, there is something I seek! If you have a WatermelonFruit, I could offer you a HeavyAnchor in exchange, along with a MaxxiaScroll. But remember, I can only make such a trade once every ten minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pla => CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a WatermelonFruit for me?");
                        tradeModule.AddOption("Yes, I have a WatermelonFruit.", 
                            plaa => HasWatermelonFruit(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasWatermelonFruit(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a WatermelonFruit.");
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
                p.SendMessage("Tala gives you a knowing nod, her eyes glinting with memories of the sea.");
            });

        return greeting;
    }

    private bool HasWatermelonFruit(PlayerMobile player)
    {
        // Check the player's inventory for WatermelonFruit
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WatermelonFruit)) != null;
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
        // Remove the WatermelonFruit and give the HeavyAnchor, then set the cooldown timer
        Item watermelonFruit = player.Backpack.FindItemByType(typeof(WatermelonFruit));
        if (watermelonFruit != null)
        {
            watermelonFruit.Delete();
            player.AddToBackpack(new HeavyAnchor());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the WatermelonFruit and receive a HeavyAnchor and a MaxxiaScroll in return. Fair winds to you, traveler!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a WatermelonFruit.");
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