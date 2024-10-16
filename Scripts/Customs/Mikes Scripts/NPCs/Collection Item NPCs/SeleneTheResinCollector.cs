using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SeleneTheResinCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SeleneTheResinCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Selene the Resin Collector";
        Body = 0x191; // Human female body
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
        AddItem(new Cloak(1150)); // Deep green cloak symbolizing her affinity with nature
        AddItem(new ThighBoots(1446)); // Brown leather boots
        AddItem(new LongPants(1109)); // Dark green pants
        AddItem(new FancyShirt(1301)); // Earthy-toned shirt
        AddItem(new Bandana(1153)); // Forest green bandana

        VirtualArmor = 15;
    }

    public SeleneTheResinCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Selene, a collector of rare resins and natural wonders. Have you come across any PineResin in your travels?");
        
        // Dialogue options
        greeting.AddOption("Who are you?", 
            p => true, 
            p =>
            {
                DialogueModule aboutSeleneModule = new DialogueModule("I am but a hermit, hiding away from the eyes of those who do not understand. Once, I was a scholar, a seeker of truth. But I learned something that drove me from civilization. The trees... the shadows... they hide more than you know.");
                
                aboutSeleneModule.AddOption("What did you discover?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule terribleTruthModule = new DialogueModule("The forest speaks, traveler. It whispers secrets, terrible secrets, about the world and what lies beyond. There are things that lurk in the shadows of these woods, things that do not belong to our reality. I could no longer stay among people, knowing what I know.");

                        terribleTruthModule.AddOption("What kind of things?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule lurkingShadowsModule = new DialogueModule("They are the shadows that stretch too far, the whispers that echo when there is no wind. They watch, they listen, and they wait. I have seen eyes in the dark, countless eyes, and I have heard them call my name. I fled, but the whispers followed me here.");
                                lurkingShadowsModule.AddOption("Are you in danger?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Perhaps. Perhaps we all are. The shadows take interest in those who know of them, and now you know too. Be wary, traveler. Do not stray from the path at night, and do not listen when they call your name.");
                                        plaa.SendGump(new DialogueGump(plaa, dangerModule));
                                    });
                                lurkingShadowsModule.AddOption("That sounds terrifying.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Selene nods solemnly, her eyes distant as if reliving the memories.");
                                    });
                                pla.SendGump(new DialogueGump(pla, lurkingShadowsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, terribleTruthModule));
                    });
                
                aboutSeleneModule.AddOption("Why did you choose the forest?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule forestModule = new DialogueModule("The forest is ancient, traveler. It is full of secrets, some dark, some wondrous. Here, among the trees, I can hear their whispers and learn to understand. They are old, wiser than any scholar, and they can teach us much if we are willing to listen.");
                        forestModule.AddOption("What have you learned from the forest?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule wisdomModule = new DialogueModule("I have learned that nature does not belong to us; we belong to it. The balance is delicate, and those who disturb it will suffer. I have also learned how to harvest the rare resins that the forest provides, such as PineResin, and use them for good.");
                                pla.SendGump(new DialogueGump(pla, wisdomModule));
                            });
                        pl.SendGump(new DialogueGump(pl, forestModule));
                    });
                
                p.SendGump(new DialogueGump(p, aboutSeleneModule));
            });
        
        greeting.AddOption("What do you do with PineResin?", 
            p => true, 
            p =>
            {
                DialogueModule resinUseModule = new DialogueModule("PineResin holds many secrets of the forest. It can be used in alchemical recipes, or to craft items of great resilience and flexibility. If you have some, I would be willing to trade.");
                
                // Trade option after story
                resinUseModule.AddOption("What kind of trade are you offering?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have PineResin, I can offer you a choice between a SpiderTree or a RopeSpindle. Additionally, you will receive a MaxxiaScroll as a token of my appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have PineResin for me?");
                                tradeModule.AddOption("Yes, I have PineResin.", 
                                    plaa => HasPineResin(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have any right now.", 
                                    plaa => !HasPineResin(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have PineResin to trade.");
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
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                resinUseModule.AddOption("Why do you collect resins?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule collectionReasonModule = new DialogueModule("The resins of the forest are powerful, but they must be gathered with care. They are gifts from nature, and can be used to protect us from the lurking dangers. They also help me remain connected to the forest, and remind me of the balance I must uphold.");
                        pl.SendGump(new DialogueGump(pl, collectionReasonModule));
                    });
                
                p.SendGump(new DialogueGump(p, resinUseModule));
            });

        greeting.AddOption("Why do you live alone here?",
            p => true,
            p =>
            {
                DialogueModule aloneModule = new DialogueModule("The world beyond the woods is full of dangers far worse than the shadows that lurk among the trees. People do not understand the threats we face, and they do not heed my warnings. I live alone because it is safer for everyone that way.");
                aloneModule.AddOption("What kind of threats?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule threatDetailsModule = new DialogueModule("The threats are many. Not only the shadows that haunt these woods but also the greed of men who would harm the forest for their own gain. They invite the darkness, unknowingly or otherwise. I choose to be here, to guard against them, and to learn.");
                        pl.SendGump(new DialogueGump(pl, threatDetailsModule));
                    });
                p.SendGump(new DialogueGump(p, aloneModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Selene nods and smiles knowingly.");
            });

        return greeting;
    }

    private bool HasPineResin(PlayerMobile player)
    {
        // Check the player's inventory for PineResin
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(PineResin)) != null;
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
        // Remove the PineResin and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item pineResin = player.Backpack.FindItemByType(typeof(PineResin));
        if (pineResin != null)
        {
            pineResin.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for SpiderTree and RopeSpindle
            rewardChoiceModule.AddOption("SpiderTree", pl => true, pl => 
            {
                pl.AddToBackpack(new SpiderTree());
                pl.SendMessage("You receive a SpiderTree!");
            });
            
            rewardChoiceModule.AddOption("RopeSpindle", pl => true, pl =>
            {
                pl.AddToBackpack(new RopeSpindle());
                pl.SendMessage("You receive a RopeSpindle!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have PineResin.");
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