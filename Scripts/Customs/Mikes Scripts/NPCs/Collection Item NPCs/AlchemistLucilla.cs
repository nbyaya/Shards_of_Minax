using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AlchemistLucilla : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AlchemistLucilla() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lucilla the Alchemist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 100;

        // Outfit
        AddItem(new Robe(1266)); // A dark purple robe
        AddItem(new Sandals(1150)); // Light purple sandals
        AddItem(new WizardsHat(1266)); // Matching dark purple wizard hat
        AddItem(new GoldBracelet()); // A shiny bracelet to show alchemical wealth

        VirtualArmor = 15;
    }

    public AlchemistLucilla(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, traveler! You have the look of someone with an interest in the mysteries of alchemy. I am Lucilla, master of rare elixirs and distillations. Do you seek an exchange of arcane curiosities?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your work.", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("I spend my days brewing potions, refining essences, and creating magical artifacts. However, the more I learn, the more I uncover dark secrets from ancient times. The rituals, the sacrifices... oh, but forgive me. You see, my work has led me to some very unsettling discoveries.");
                
                // Add deeper layers to the story
                storyModule.AddOption("What kind of discoveries?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule discoveryModule = new DialogueModule("The ancient texts I found speak of horrific rituals, dark gods, and unspeakable pacts made by those desperate for power. The more I read, the more I became convinced that these dark entities still watch us, waiting for an opportunity to return.");
                        
                        discoveryModule.AddOption("That sounds dangerous. Why continue?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule obsessionModule = new DialogueModule("You don't understand! I have to know. I must understand what they knew, or else we are all in peril. Every night, I feel their eyes on me, hear their whispers in the dark. The paranoia drives me, pushes me to uncover more, to stay one step ahead of whatever is coming.");
                                
                                obsessionModule.AddOption("You seem obsessed. Is it worth the risk?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule riskModule = new DialogueModule("Worth the risk? Perhaps not. But I have no choice. I can't turn back now. The more I learn, the more I realize how close we are to catastrophe. These dark gods, these horrors... they are real, and they are waiting for a moment of weakness.");
                                        
                                        riskModule.AddOption("Is there anything I can do to help?", 
                                            plaab => true, 
                                            plaab =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("Yes, actually. I need rare components to complete my research. DistilledEssence, for instance, is crucial. If you bring me one, I can reward you with a rare artifact. Perhaps, together, we can prepare for what lies ahead.");
                                                
                                                helpModule.AddOption("What kind of artifact?", 
                                                    plaaa => true, 
                                                    plaaa =>
                                                    {
                                                        DialogueModule artifactModule = new DialogueModule("I have a few artifacts of note. You may choose between a WigStand or a BrassFountain. Each is imbued with protective charms, meant to guard against the dark influences I fear may soon overwhelm us.");
                                                        
                                                        artifactModule.AddOption("I'd like to make the trade.", 
                                                            plaaaa => CanTradeWithPlayer(plaaaa), 
                                                            plaaaa =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Do you have a DistilledEssence for me?");
                                                                tradeModule.AddOption("Yes, I have a DistilledEssence.", 
                                                                    plaaaaa => HasDistilledEssence(plaaaaa) && CanTradeWithPlayer(plaaaaa), 
                                                                    plaaaaa =>
                                                                    {
                                                                        CompleteTrade(plaaaaa);
                                                                    });
                                                                tradeModule.AddOption("No, I don't have one right now.", 
                                                                    plaaaaa => !HasDistilledEssence(plaaaaa), 
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendMessage("Come back when you have a DistilledEssence.");
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                                    });
                                                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                    plaaaaa => !CanTradeWithPlayer(plaaaaa), 
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, tradeModule));
                                                            });
                                                        artifactModule.AddOption("Maybe another time.", 
                                                            plaaaa => true, 
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, artifactModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, helpModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, riskModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, obsessionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, discoveryModule));
                    });

                // Trade option after story
                storyModule.AddOption("Do you need any rare components?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a DistilledEssence, I can offer you a choice between a WigStand or a BrassFountain, along with a small token of my gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a DistilledEssence for me?");
                                tradeModule.AddOption("Yes, I have a DistilledEssence.", 
                                    plaa => HasDistilledEssence(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasDistilledEssence(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a DistilledEssence.");
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

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Lucilla nods knowingly, her eyes twinkling with curiosity.");
            });

        return greeting;
    }

    private bool HasDistilledEssence(PlayerMobile player)
    {
        // Check the player's inventory for DistilledEssence
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DistilledEssence)) != null;
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
        // Remove the DistilledEssence and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item distilledEssence = player.Backpack.FindItemByType(typeof(DistilledEssence));
        if (distilledEssence != null)
        {
            distilledEssence.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for WigStand and BrassFountain
            rewardChoiceModule.AddOption("WigStand", pl => true, pl => 
            {
                pl.AddToBackpack(new WigStand());
                pl.SendMessage("You receive a WigStand!");
            });
            
            rewardChoiceModule.AddOption("BrassFountain", pl => true, pl =>
            {
                pl.AddToBackpack(new BrassFountain());
                pl.SendMessage("You receive a BrassFountain!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a DistilledEssence.");
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