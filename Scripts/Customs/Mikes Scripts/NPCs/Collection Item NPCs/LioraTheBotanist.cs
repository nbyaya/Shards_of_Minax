using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class LioraTheBotanist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LioraTheBotanist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Liora the Botanist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(100);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1161)); // Robe with a greenish hue
        AddItem(new Sandals(1175)); // Sandals with a light green hue
        AddItem(new FlowerGarland()); // A floral headpiece
        AddItem(new PineResin()); // Her reward item is also in her invantory, so it can be stolen

        VirtualArmor = 10;
    }

    public LioraTheBotanist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings! I sense you are drawn to nature as I am. I'm Liora, a botanist devoted to the preservation of rare flora. Do you seek knowledge?");

        // Start with dialogue about her work
        greeting.AddOption("Tell me about the plants you study.", 
            p => true, 
            p =>
            {
                DialogueModule plantsModule = new DialogueModule("Ah, the plants of the realm! Each one holds a secret waiting to be uncovered. Shall I tell you about the Moonflower, Crimson Briar, Crystal Bloom, Dragonroot, or Sunpetal?");
                
                // Nested options for each plant
                plantsModule.AddOption("Tell me about the Moonflower.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule moonflowerModule = new DialogueModule("The Moonflower is a rare nocturnal plant. It only blooms under the full moon's light. Its petals are said to possess restorative properties, used in healing elixirs. However, gathering it is dangerous, as it attracts shadowy creatures that guard it fiercely.");
                        moonflowerModule.AddOption("How is it used in healing?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule healingModule = new DialogueModule("The petals, when mixed with silverroot, create a potent salve that heals even the most grievous wounds. It’s said to speed up regeneration and can cure certain curses, but the mixture is difficult to perfect.");
                                healingModule.AddOption("Fascinating! Thank you.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, healingModule));
                            });
                        moonflowerModule.AddOption("Sounds too dangerous for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, moonflowerModule));
                    });

                plantsModule.AddOption("Tell me about the Crimson Briar.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule briarModule = new DialogueModule("Crimson Briar is a deadly vine that grows in deep, shadowed forests. Its thorns are poisonous, but it produces a bright red berry that, when properly distilled, can serve as an antidote to its own poison. Few dare to collect it, for the vine has a mind of its own, ensnaring those who approach.");
                        briarModule.AddOption("How is the antidote prepared?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule antidoteModule = new DialogueModule("You must extract the juice from the berries and mix it with ashroot and dew collected at dawn. If done correctly, the antidote can neutralize almost any poison.");
                                antidoteModule.AddOption("I'll be careful. Thank you.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, antidoteModule));
                            });
                        briarModule.AddOption("That sounds too risky for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, briarModule));
                    });

                // After they’ve learned about the plants, introduce the trade option
                plantsModule.AddOption("Is there anything you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of a MiniCherryTree for some of my research. In return, I can offer you PineResin. But I have a limited supply");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MiniCherryTree for me?");
                                tradeModule.AddOption("Yes, I have a MiniCherryTree.", 
                                    plaa => HasMiniCherryTree(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasMiniCherryTree(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a MiniCherryTree.");
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

                p.SendGump(new DialogueGump(p, plantsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Liora nods gracefully.");
            });

        return greeting;
    }

    private bool HasMiniCherryTree(PlayerMobile player)
    {
        // Check the player's inventory for MiniCherryTree
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MiniCherryTree)) != null;
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
        // Remove the MiniCherryTree and give the PineResin, then set the cooldown timer
        Item miniCherryTree = player.Backpack.FindItemByType(typeof(MiniCherryTree));
        if (miniCherryTree != null)
        {
            miniCherryTree.Delete();
            player.AddToBackpack(new PineResin());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MiniCherryTree and receive PineResin in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a MiniCherryTree.");
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
