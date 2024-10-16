using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class YorickTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public YorickTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Yorick the Alchemist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1953)); // Robe with a dark purple hue
        AddItem(new Boots(1109)); // Boots with a black hue
        AddItem(new WizardsHat(1153)); // Wizard's hat with matching hue
        AddItem(new GoldBracelet()); // A gold bracelet for a bit of flair
        AddItem(new BlacksmithTalisman()); // A decorative alchemy flask as a unique item

        VirtualArmor = 15;
    }

    public YorickTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Yorick, a seeker of alchemical wonders and perhaps, something more. Do you fancy a bit of trade, some knowledge, or perhaps... an opportunity to change your fate?");

        // Start with dialogue about his background
        greeting.AddOption("Tell me about your alchemical studies.", 
            p => true, 
            p =>
            {
                DialogueModule studiesModule = new DialogueModule("Alchemy is an art, a science, and a mystery all in one. I have spent my life unraveling its secrets—from elixirs of life to potions of transmutation. But alchemy alone does not grant power, true power comes from influence, and influence from ambition.");
                
                // Nested options about his studies
                studiesModule.AddOption("What is the most powerful elixir you've created?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule elixirModule = new DialogueModule("The Elixir of Eternal Vigilance, capable of granting one the ability to stay awake and alert for days. It is potent, yet dangerous if overused. Only those with true resilience dare attempt it.");
                        elixirModule.AddOption("That sounds incredible!", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        elixirModule.AddOption("Sounds risky. I'll pass.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, elixirModule));
                    });

                studiesModule.AddOption("Can alchemy really turn lead into gold?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule goldModule = new DialogueModule("Ah, the fabled Philosopher's Stone! Many have tried, few have succeeded. True transmutation requires not just ingredients, but an understanding of the balance between all elements. And for those willing to take certain... risks, one might find it within reach.");
                        goldModule.AddOption("What kind of risks?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule risksModule = new DialogueModule("There are those who would lend their power for a price. Dark figures, peddlers of secrets, who require loyalty above all else. In politics, in alchemy, alliances are sometimes forged not in the light of day, but in shadows. Are you willing to take that step?");
                                risksModule.AddOption("Tell me more about these alliances.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        DialogueModule allianceModule = new DialogueModule("There are forces in this town, powerful people, who shape the tides of fate. I am but a rising star among them, but with every deal, every pact, my influence grows. If you are clever, if you can play the game, you might find yourself rising alongside me.");
                                        allianceModule.AddOption("Sounds dangerous, but intriguing.", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        allianceModule.AddOption("I think I'd rather keep my distance.", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, allianceModule));
                                    });
                                risksModule.AddOption("I'm not interested in dark dealings.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, risksModule));
                            });
                        goldModule.AddOption("I think I'll stick to simpler pursuits.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, goldModule));
                    });

                studiesModule.AddOption("Do you need anything for your experiments?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of a MiniKeg for my latest concoction. In return, I can offer you a BlacksmithTalisman and always a MaxxiaScroll as a token of my gratitude. But remember, I have limited resources, and can only trade once every 10 minutes. You see, these items are useful in more ways than one.");
                        tradeIntroductionModule.AddOption("Useful how?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule usefulModule = new DialogueModule("Let me be candid, friend. The BlacksmithTalisman has a certain value among those who wish to forge more than just metal. It represents influence, a sign of goodwill, and with enough goodwill, doors begin to open. The MaxxiaScroll, on the other hand, is a promise of knowledge—knowledge that some would kill to obtain. I reward those who help me, but I also grow stronger in the process.");
                                usefulModule.AddOption("So, you use these trades to gather influence?", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        DialogueModule influenceModule = new DialogueModule("Precisely! Influence is power, and power is what drives this world. Every trade I make, every pact I form, builds my network. People trust me, they owe me favors, and soon enough, they find themselves under my sway. And you, friend, could be a part of that network if you wish.");
                                        influenceModule.AddOption("Perhaps I'll consider it.", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        influenceModule.AddOption("No, thank you. I prefer to be independent.", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, influenceModule));
                                    });
                                usefulModule.AddOption("I'll take your word for it.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                pla.SendGump(new DialogueGump(pla, usefulModule));
                            });

                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MiniKeg for me?");
                                tradeModule.AddOption("Yes, I have a MiniKeg.", 
                                    plaa => HasMiniKeg(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasMiniKeg(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a MiniKeg.");
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
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, studiesModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Yorick nods knowingly, his eyes gleaming with alchemical curiosity and something darker lurking beneath.");
            });

        return greeting;
    }

    private bool HasMiniKeg(PlayerMobile player)
    {
        // Check the player's inventory for MiniKeg
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MiniKeg)) != null;
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
        // Remove the MiniKeg and give the BlacksmithTalisman and MaxxiaScroll, then set the cooldown timer
        Item miniKeg = player.Backpack.FindItemByType(typeof(MiniKeg));
        if (miniKeg != null)
        {
            miniKeg.Delete();
            player.AddToBackpack(new BlacksmithTalisman());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MiniKeg and receive a BlacksmithTalisman and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a MiniKeg.");
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