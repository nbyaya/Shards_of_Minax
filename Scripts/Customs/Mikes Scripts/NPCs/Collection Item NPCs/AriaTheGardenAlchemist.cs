using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AriaTheGardenAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AriaTheGardenAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aria the Garden Alchemist";
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
        AddItem(new Robe(1446)); // A green alchemist's robe
        AddItem(new Sandals(1153)); // Light green sandals
        AddItem(new WideBrimHat(1446)); // A matching green hat
        AddItem(new GoldBracelet()); // A golden bracelet
        AddItem(new SalvageMachine()); // Symbolic alchemist tool (could be stolen)

        VirtualArmor = 12;
    }

    public AriaTheGardenAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Aria, a humble alchemist tending to the garden of nature. Do you seek a potion or perhaps some wisdom?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your alchemy.", 
            p => true, 
            p =>
            {
                DialogueModule alchemyModule = new DialogueModule("The essence of alchemy is transformation. I blend nature's gifts to create potions, elixirs, and curious curios. Yet, I need rare ingredients for my work.");
                
                // Alchemy deeper dialogue options
                alchemyModule.AddOption("What do you mean by transformation?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule transformationModule = new DialogueModule("Transformation is the heart of alchemy. It is about seeing potential where others see nothing but ordinary herbs and elements. I take these mundane objects and give them a purpose.");
                        transformationModule.AddOption("That sounds fascinating. Can you teach me more?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule teachingModule = new DialogueModule("I can teach you, but only if you truly understand the nature of balance. The transformation comes at a cost, and every action has a consequence. Are you prepared to bear that responsibility?");
                                teachingModule.AddOption("Yes, I am prepared.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendMessage("Very well, I shall begin your training when you bring me a special item - a symbol of your commitment. Bring me a MoonlitPetal, and we shall begin.");
                                    });
                                teachingModule.AddOption("No, I need more time to think.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendMessage("Take your time. True alchemy is not rushed.");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, teachingModule));
                            });
                        transformationModule.AddOption("Perhaps another time.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, transformationModule));
                    });
                
                alchemyModule.AddOption("Do you need any rare ingredients?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in search of a rare item - a LayingChicken. If you have one, I can offer you either an EssenceOfToad or a SalvageMachine in exchange, along with something extra for your trouble.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a LayingChicken for me?");
                                tradeModule.AddOption("Yes, I have a LayingChicken.", 
                                    plaa => HasLayingChicken(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasLayingChicken(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a LayingChicken.");
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

                p.SendGump(new DialogueGump(p, alchemyModule));
            });

        greeting.AddOption("Why do you look so lonely, Aria?", 
            p => true, 
            p =>
            {
                DialogueModule lonelinessModule = new DialogueModule("Lonely? Perhaps you see more than others do, traveler. My life here, amidst herbs and flowers, is solitary, yes, but not without meaning. Still, there is a weight on my heart that few understand.");
                lonelinessModule.AddOption("Tell me more about this weight.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule weightModule = new DialogueModule("I was marked by a family curse, a burden passed down for generations. It grants me certain abilities, but it also isolates me from those around me. The townsfolk fear what they do not understand.");
                        weightModule.AddOption("What kind of abilities?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule abilitiesModule = new DialogueModule("The curse grants me a perception beyond the ordinary. I can sense the hidden essence of things - life, death, potential. But with this sight comes fear. People believe I am dangerous, even though I have only ever tried to help.");
                                abilitiesModule.AddOption("That sounds like a gift, not a curse.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendMessage("You are kind to say so. I wish more people saw it as you do. Perhaps one day, the fear will fade.");
                                    });
                                abilitiesModule.AddOption("I understand why people might be afraid.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendMessage("Fear is powerful, and sometimes it blinds people to the truth. I do not blame them, but it does make life difficult.");
                                    });
                                abilitiesModule.AddOption("Can I help you in any way?", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Perhaps you can. There is a flower called the SolaceBloom that grows deep in the forest. It is said to ease the burden of loneliness. If you could bring it to me, I would be most grateful.");
                                        helpModule.AddOption("I will find the SolaceBloom for you.", 
                                            plaac => true, 
                                            plaac =>
                                            {
                                                plaac.SendMessage("Thank you, traveler. You have no idea what this means to me.");
                                            });
                                        helpModule.AddOption("I'm not sure I can take on such a task.", 
                                            plaac => true, 
                                            plaac =>
                                            {
                                                plaac.SendMessage("I understand. It is not an easy thing to ask. If you change your mind, I will be here.");
                                                plaac.SendGump(new DialogueGump(plaac, CreateGreetingModule(plaac)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, helpModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, abilitiesModule));
                            });
                        weightModule.AddOption("I hope you find peace, Aria.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendMessage("Thank you, traveler. Your words bring me some comfort.");
                            });
                        pl.SendGump(new DialogueGump(pl, weightModule));
                    });
                lonelinessModule.AddOption("I'm sorry, I didn't mean to pry.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("No harm done. Few take the time to notice, and for that, I thank you.");
                    });
                p.SendGump(new DialogueGump(p, lonelinessModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Aria nods thoughtfully and turns back to her herbs.");
            });

        return greeting;
    }

    private bool HasLayingChicken(PlayerMobile player)
    {
        // Check the player's inventory for LayingChicken
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LayingChicken)) != null;
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
        // Remove the LayingChicken and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item layingChicken = player.Backpack.FindItemByType(typeof(LayingChicken));
        if (layingChicken != null)
        {
            layingChicken.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for EssenceOfToad and SalvageMachine
            rewardChoiceModule.AddOption("EssenceOfToad", pl => true, pl => 
            {
                pl.AddToBackpack(new EssenceOfToad());
                pl.SendMessage("You receive an EssenceOfToad!");
            });
            
            rewardChoiceModule.AddOption("SalvageMachine", pl => true, pl =>
            {
                pl.AddToBackpack(new SalvageMachine());
                pl.SendMessage("You receive a SalvageMachine!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a LayingChicken.");
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