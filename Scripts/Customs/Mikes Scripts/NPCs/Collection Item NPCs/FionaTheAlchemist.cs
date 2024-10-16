using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FionaTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FionaTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Fiona the Alchemist";
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

        // Outfit - unique alchemist attire
        AddItem(new Robe(1953)); // Deep green robe
        AddItem(new Sandals(1175)); // Yellow sandals
        AddItem(new Bandana(1175)); // Yellow bandana
        AddItem(new HalfApron(1150)); // Dark blue apron
        AddItem(new GoldEarrings()); // Gold earrings

        VirtualArmor = 15;
    }

    public FionaTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Fiona, a humble alchemist on a quest to perfect my concoctions. Tell me, do you dabble in the arcane science of alchemy?");
        
        // Dialogue options
        greeting.AddOption("What kind of alchemy are you working on?", 
            p => true, 
            p =>
            {
                DialogueModule alchemyModule = new DialogueModule("Ah, I mix rare ingredients to create elixirs and potions of wonder! But one substance remains elusive - Wood Alcohol. Would you happen to have some?");
                
                // Add deeper nested dialogues with more details
                alchemyModule.AddOption("Why is Wood Alcohol so important?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule importanceModule = new DialogueModule("Wood Alcohol is a key ingredient in stabilizing my most powerful potions. Without it, the elixirs lose their potency and, in some cases, become dangerously volatile. I am tasked with making sure the mixtures are safe for those who dare to use them.");
                        
                        importanceModule.AddOption("What are these potions for?", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule potionsPurposeModule = new DialogueModule("The potions I craft can be used for many purposes: some for healing, some for enhancing strength, and others for diving deep into the mysteries of magic itself. However, some potions are... different. They must never fall into the wrong hands. The responsibility weighs heavily on me.");
                                
                                potionsPurposeModule.AddOption("Why do you seem worried?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule worryModule = new DialogueModule("Ah, you have a keen eye, traveler. I am worried because not everyone who seeks my potions does so for good reasons. There are dark forces out there, ones who would use my creations to harm others. I have seen the consequences of alchemical misuse, and it haunts me.");
                                        
                                        worryModule.AddOption("Tell me more about these dark forces.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                DialogueModule darkForcesModule = new DialogueModule("They call themselves the Shadow Conclave. They operate in secrecy, recruiting those who are disillusioned or desperate. They have approached me before, offering wealth and power in exchange for my work. I refused, of course, but their presence lingers, like a shadow that won't fade.");
                                                
                                                darkForcesModule.AddOption("That sounds dangerous. How do you protect yourself?", 
                                                    plllll => true, 
                                                    plllll =>
                                                    {
                                                        DialogueModule protectionModule = new DialogueModule("I rely on my knowledge of alchemy, as well as allies who share my beliefs. I also trust in the goodness of travelers like you. Many have helped me before, and I am hoping that you too can lend a hand, should the need arise.");
                                                        
                                                        protectionModule.AddOption("How can I help you?", 
                                                            pllllll => true, 
                                                            pllllll =>
                                                            {
                                                                DialogueModule helpModule = new DialogueModule("If you encounter any information about the Shadow Conclave, or if you find any rare alchemical ingredients, please bring them to me. Together, we may prevent the dark from growing stronger.");
                                                                
                                                                helpModule.AddOption("I will keep an eye out.", 
                                                                    plllllll => true, 
                                                                    plllllll =>
                                                                    {
                                                                        plllllll.SendMessage("Fiona smiles warmly. 'Thank you, traveler. I knew I could count on you.'");
                                                                    });
                                                                
                                                                pllllll.SendGump(new DialogueGump(pllllll, helpModule));
                                                            });
                                                        
                                                        plllll.SendGump(new DialogueGump(plllll, protectionModule));
                                                    });
                                                
                                                pllll.SendGump(new DialogueGump(pllll, darkForcesModule));
                                            });
                                        
                                        plll.SendGump(new DialogueGump(plll, worryModule));
                                    });
                                
                                pll.SendGump(new DialogueGump(pll, potionsPurposeModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, importanceModule));
                    });

                // Trade option after story
                alchemyModule.AddOption("Do you need Wood Alcohol?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you bring me Wood Alcohol, I can offer you either a Chocolate Fountain or a Power Gem as a reward. You may also take this Maxxia Scroll, a humble token of my appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have Wood Alcohol for me?");
                                tradeModule.AddOption("Yes, I have Wood Alcohol.", 
                                    plaa => HasWoodAlcohol(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have it right now.", 
                                    plaa => !HasWoodAlcohol(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("No worries, bring me some Wood Alcohol when you find it!");
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

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Fiona nods and smiles. 'May your path be filled with discovery.'");
            });

        return greeting;
    }

    private bool HasWoodAlcohol(PlayerMobile player)
    {
        // Check the player's inventory for Wood Alcohol
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WoodAlchohol)) != null;
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
        // Remove the Wood Alcohol and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item woodAlcohol = player.Backpack.FindItemByType(typeof(WoodAlchohol));
        if (woodAlcohol != null)
        {
            woodAlcohol.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for Chocolate Fountain and Power Gem
            rewardChoiceModule.AddOption("Chocolate Fountain", pl => true, pl => 
            {
                pl.AddToBackpack(new ChocolateFountain());
                pl.SendMessage("You receive a Chocolate Fountain!");
            });
            
            rewardChoiceModule.AddOption("Power Gem", pl => true, pl =>
            {
                pl.AddToBackpack(new PowerGem());
                pl.SendMessage("You receive a Power Gem!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have Wood Alcohol.");
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