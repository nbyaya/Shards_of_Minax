using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ThaliaTheDreamWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaliaTheDreamWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thalia the Dream Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1157)); // Dark blue hooded robe
        AddItem(new Sandals(1)); // Pure white sandals
        AddItem(new LongPants(2301)); // Midnight-blue pants
        AddItem(new Cloak(1153)); // Gilded cloak with golden trim
        AddItem(new LanternOfSouls()); // Unique lantern item to enhance the mysterious effect

        VirtualArmor = 15;
    }

    public ThaliaTheDreamWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Thalia, a weaver of dreams and a seeker of fairy secrets. Tell me, do you carry any fairy oil?");

        // Dialogue options
        greeting.AddOption("What do you need fairy oil for?", 
            p => true, 
            p =>
            {
                DialogueModule explanationModule = new DialogueModule("Fairy oil is an ingredient essential for my work with dreams. It is rare, but if you have it, I can offer you something in return—a gift of knowledge or light."
                    + " However, I must warn you, the journey I have taken to discover its use is a dark and painful one.");
                
                // Nested story about her past
                explanationModule.AddOption("You seem troubled. What happened to you?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule storyModule = new DialogueModule("Once, I was not merely a dream weaver. I was a soldier—a warrior who fought in wars that the history books choose to forget. The battles I fought were unlike anything this world has seen."
                            + " The enemy, you see, was not of flesh and bone. It was something else… something beyond the stars. They took my comrades, twisted their minds, broke their bodies. What returned were not the men and women I knew."
                            + " They spoke of strange visions, twisted dreams, and darkness that filled their hearts."
                            + " That is why I seek fairy oil—to push back against that darkness that still haunts me."
                            + " I may seem calm now, but the horrors remain, lurking in my dreams, and sometimes even in the light of day.");
                        
                        storyModule.AddOption("It sounds like you’ve been through a lot. How do you keep going?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule resilienceModule = new DialogueModule("I keep going because I must. There is no glory in survival, only necessity. I am haunted, traveler. Haunted by the screams of my friends, by the emptiness in their eyes."
                                    + " I cannot stop, for if I do, the darkness will consume me. I weave dreams because it is the only way I can take back control."
                                    + " Dreams can be shaped, molded, unlike the grim reality I faced. Through them, I hope to find a fragment of peace.");
                                
                                resilienceModule.AddOption("That is truly admirable. Can I help in any way?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Perhaps you can. The fairy oil you hold is a small fragment of magic that pushes back against the unknown."
                                            + " With it, I can shape the dreams into weapons against that which haunts us all."
                                            + " You might think it’s just an oil, but to me, it is the difference between fear and hope.");
                                        
                                        // Trade option after story
                                        helpModule.AddOption("I have fairy oil. What can you offer me?", 
                                            plaaa => HasFairyOil(plaaa) && CanTradeWithPlayer(plaaa), 
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });

                                        helpModule.AddOption("I do not have any right now.", 
                                            plaaa => !HasFairyOil(plaaa), 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Return to me when you possess the fairy oil.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        helpModule.AddOption("I traded recently; I'll come back later.", 
                                            plaaa => !CanTradeWithPlayer(plaaa), 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("The magic takes time to replenish. Return in a while.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, helpModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, resilienceModule));
                            });

                        pl.SendGump(new DialogueGump(pl, storyModule));
                    });

                explanationModule.AddOption("I do not have any right now.", 
                    pl => !HasFairyOil(pl), 
                    pl =>
                    {
                        pl.SendMessage("Return to me when you possess the fairy oil.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                explanationModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("The magic takes time to replenish. Return in a while.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, explanationModule));
            });

        greeting.AddOption("Farewell.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thalia nods gently, her eyes gleaming with mystery.");
            });

        return greeting;
    }

    private bool HasFairyOil(PlayerMobile player)
    {
        // Check the player's inventory for FairyOil
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FairyOil)) != null;
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
        // Remove the FairyOil and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fairyOil = player.Backpack.FindItemByType(typeof(FairyOil));
        if (fairyOil != null)
        {
            fairyOil.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for FleshLight and EssentialBooks
            rewardChoiceModule.AddOption("FleshLight", pl => true, pl => 
            {
                pl.AddToBackpack(new FleshLight());
                pl.SendMessage("You receive a FleshLight, shimmering with dream magic!");
            });

            rewardChoiceModule.AddOption("EssentialBooks", pl => true, pl =>
            {
                pl.AddToBackpack(new EssentialBooks());
                pl.SendMessage("You receive the EssentialBooks, filled with ancient wisdom!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have the fairy oil.");
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