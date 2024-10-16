using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class OrlinTheEccentricBotanist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public OrlinTheEccentricBotanist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Orlin the Eccentric Botanist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(90);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1281)); // A green robe
        AddItem(new Boots(1321)); // Light brown boots
        AddItem(new WizardsHat(1444)); // A leafy green hat
        AddItem(new QuarterStaff()); // A wooden staff, adding to the botanist theme
        AddItem(new Backpack()); // Orlin's herb collection

        VirtualArmor = 15;
    }

    public OrlinTheEccentricBotanist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Orlin, an eccentric botanist, they say. Are you here to trade knowledge or perhaps something more... tangible?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your studies.", 
            p => true, 
            p =>
            {
                DialogueModule studiesModule = new DialogueModule("My passion lies in the peculiar flora of this world. There are plants with strange properties... mysterious, almost mystical. Lately, I have been fascinated by a certain MysteryOrb. Would you happen to have one?");
                
                // Additional nested dialogue options
                studiesModule.AddOption("Why are you fascinated by the MysteryOrb?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule fascinationModule = new DialogueModule("The MysteryOrb... it reflects my deepest thoughts. It is said to contain echoes of the lost, whispers of forgotten lives, and sorrows untold. It is both a blessing and a curse, reminding me of the darkness within our world.");
                        
                        fascinationModule.AddOption("You seem troubled. Is everything alright?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule troubledModule = new DialogueModule("Ah, perceptive one, you see beyond the surface. I have seen much tragedy, and it has left its mark. My writings are full of melancholic verses, reflections of the harshness of life and the sadness I carry within. The MysteryOrb speaks to that sadness.");
                                
                                troubledModule.AddOption("Tell me about the tragedies you've faced.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule tragedyModule = new DialogueModule("Once, I had a family... a loving wife and a child. But fate is cruel. My son fell ill, and despite my knowledge of plants, I could not save him. My wife, grief-stricken, left in search of something—perhaps solace, perhaps oblivion. I have not seen her since.");
                                        
                                        tragedyModule.AddOption("I'm sorry for your loss. Does your work help you cope?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule copingModule = new DialogueModule("Thank you for your kind words. My work is my way of coping, yes. The plants I nurture, the herbs I study—they are the only company I have now. They listen without judgment, and in them, I find echoes of life that still endure.");
                                                
                                                copingModule.AddOption("Is that why you help travelers like me?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Indeed. In helping others, I find a small measure of peace. Perhaps by sharing my knowledge or these peculiar trinkets, I can make some amends for my failures. If you have a MysteryOrb, I would gladly trade with you. It is a small way for me to feel connected again.");
                                                        
                                                        helpModule.AddOption("Do you need the MysteryOrb?", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a MysteryOrb, I could offer you something special. You may choose between a BlueberryPie or BioSamples, and I will also reward you with a MaxxiaScroll.");
                                                                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                                    plaaaaaa => CanTradeWithPlayer(plaaaaaa), 
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a MysteryOrb for me?");
                                                                        tradeModule.AddOption("Yes, I have a MysteryOrb.", 
                                                                            plaaaaaaa => HasMysteryOrb(plaaaaaaa) && CanTradeWithPlayer(plaaaaaaa), 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                CompleteTrade(plaaaaaaa);
                                                                            });
                                                                        tradeModule.AddOption("No, I don't have one right now.", 
                                                                            plaaaaaaa => !HasMysteryOrb(plaaaaaaa), 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("Come back when you have a MysteryOrb.");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                            plaaaaaaa => !CanTradeWithPlayer(plaaaaaaa), 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, tradeModule));
                                                                    });
                                                                tradeIntroductionModule.AddOption("Maybe another time.", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, tradeIntroductionModule));
                                                            });

                                                        helpModule.AddOption("Goodbye.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Orlin nods knowingly and turns back to his botanical musings.");
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, copingModule));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, tragedyModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, troubledModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, fascinationModule));
                    });

                // Trade option after story
                studiesModule.AddOption("Do you need the MysteryOrb?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a MysteryOrb, I could offer you something special. You may choose between a BlueberryPie or BioSamples, and I will also reward you with a MaxxiaScroll.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MysteryOrb for me?");
                                tradeModule.AddOption("Yes, I have a MysteryOrb.", 
                                    plaa => HasMysteryOrb(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasMysteryOrb(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a MysteryOrb.");
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

                p.SendGump(new DialogueGump(p, studiesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Orlin nods knowingly and turns back to his botanical musings.");
            });

        return greeting;
    }

    private bool HasMysteryOrb(PlayerMobile player)
    {
        // Check the player's inventory for MysteryOrb
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MysteryOrb)) != null;
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
        // Remove the MysteryOrb and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item mysteryOrb = player.Backpack.FindItemByType(typeof(MysteryOrb));
        if (mysteryOrb != null)
        {
            mysteryOrb.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for BlueberryPie and BioSamples
            rewardChoiceModule.AddOption("BlueberryPie", pl => true, pl => 
            {
                pl.AddToBackpack(new BlueberryPie());
                pl.SendMessage("You receive a BlueberryPie!");
            });
            
            rewardChoiceModule.AddOption("BioSamples", pl => true, pl =>
            {
                pl.AddToBackpack(new BioSamples());
                pl.SendMessage("You receive BioSamples!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a MysteryOrb.");
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