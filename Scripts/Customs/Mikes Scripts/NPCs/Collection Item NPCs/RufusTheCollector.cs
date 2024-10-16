using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class RufusTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RufusTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rufus the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(80);

        SetHits(100);
        SetMana(100);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // Bright blue fancy shirt
        AddItem(new LongPants(1109)); // Dark green pants
        AddItem(new Sandals(2301)); // Orange sandals
        AddItem(new WideBrimHat(1175)); // Yellow wide brim hat
        AddItem(new Backpack()); // Just for aesthetics

        VirtualArmor = 12;
    }

    public RufusTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Rufus, a collector of rare and peculiar trinkets. Do you, by chance, have anything unusual you'd like to trade?");
        
        // Dialogue options
        greeting.AddOption("What kind of trinkets are you interested in?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I'm particularly interested in a SkullRing. If you have one, I can offer you a choice between an EasterDayEgg or a stack of skulls, the SkullsStack. Plus, I always reward with a MaxxiaScroll for those willing to trade!");
                
                tradeIntroductionModule.AddOption("Tell me more about these items.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule itemDetailsModule = new DialogueModule("Ah, the EasterDayEgg is a mysterious object said to bring luck during springtime. The SkullsStack, however, has a much darker purpose, believed to be linked to rituals of old...");
                        itemDetailsModule.AddOption("What kind of rituals?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule ritualDetailsModule = new DialogueModule("Legends speak of an ancient coven that used the SkullsStack to commune with spirits beyond. Some say the spirits were benevolent, others say they were vengeful... who can really say?");
                                ritualDetailsModule.AddOption("Sounds dangerous. Why would anyone want to use it?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Danger and curiosity go hand in hand, traveler. Sometimes, the need for answers outweighs the fear of consequences.");
                                        dangerModule.AddOption("You must be quite curious yourself.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule curiousModule = new DialogueModule("Indeed, I am. That is why I collect these trinkets, to piece together forgotten stories and uncover what others fear to explore. Perhaps, one day, I will understand them all.");
                                                plaaa.SendGump(new DialogueGump(plaaa, curiousModule));
                                            });
                                        dangerModule.AddOption("I think I'll pass on the danger.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Wise choice. Some knowledge is best left untouched.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, dangerModule));
                                    });
                                ritualDetailsModule.AddOption("I think I will steer clear of such things.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Not everyone is meant to walk that path, traveler. Perhaps you are wiser than most.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, ritualDetailsModule));
                            });
                        itemDetailsModule.AddOption("I think I understand now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, itemDetailsModule));
                    });
                
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a SkullRing for me?");
                        tradeModule.AddOption("Yes, I have a SkullRing.", 
                            plaa => HasSkullRing(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasSkullRing(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a SkullRing.");
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
                
                tradeIntroductionModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Who else have you traded with?", 
            p => true, 
            p =>
            {
                DialogueModule pastTradesModule = new DialogueModule("Ah, I have met many adventurers in my time. Some bold, some cautious, all curious in their own way. There was one, a young herbalist, who traded a vial of dark liquid... she was strange, yet wise.");
                pastTradesModule.AddOption("A herbalist? Tell me more.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule herbalistModule = new DialogueModule("Her name was known only as 'The Herbalist'. She spoke of rare plants, and the potions she brewed were said to heal, but also to curse. She was elusive, appearing only to those in desperate need.");
                        herbalistModule.AddOption("Did you trade with her?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeWithHerbalistModule = new DialogueModule("Indeed, I did. The vial she gave me seemed to whisper when touched, and its contents were as black as midnight. I have not dared to open it yet.");
                                tradeWithHerbalistModule.AddOption("Why haven't you opened it?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule vialModule = new DialogueModule("Some things are better left untouched, traveler. The unknown holds power, and I am not yet prepared to face what may lie within.");
                                        vialModule.AddOption("Perhaps it is wise to leave it be.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Caution is often the best path. Not all mysteries are meant to be solved.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        vialModule.AddOption("I could open it for you.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("No, no... if it must be opened, it will be on my own terms. But your courage is admirable, traveler.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, vialModule));
                                    });
                                tradeWithHerbalistModule.AddOption("Perhaps it is best left alone.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Indeed, only a fool rushes into the unknown without thought.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeWithHerbalistModule));
                            });
                        herbalistModule.AddOption("She sounds dangerous.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Dangerous, yes, but also wise beyond measure. Those who seek her out often have no other choice.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, herbalistModule));
                    });
                pastTradesModule.AddOption("Interesting. Maybe I'll meet her one day.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("Perhaps you will, if fate deems it so.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, pastTradesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Rufus nods and returns to examining his collection.");
            });

        return greeting;
    }

    private bool HasSkullRing(PlayerMobile player)
    {
        // Check the player's inventory for SkullRing
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SkullRing)) != null;
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
        // Remove the SkullRing and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item skullRing = player.Backpack.FindItemByType(typeof(SkullRing));
        if (skullRing != null)
        {
            skullRing.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for EasterDayEgg and SkullsStack
            rewardChoiceModule.AddOption("EasterDayEgg", pl => true, pl => 
            {
                pl.AddToBackpack(new EasterDayEgg());
                pl.SendMessage("You receive an EasterDayEgg!");
            });
            
            rewardChoiceModule.AddOption("SkullsStack", pl => true, pl =>
            {
                pl.AddToBackpack(new SkullsStack());
                pl.SendMessage("You receive a SkullsStack!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SkullRing.");
        }
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