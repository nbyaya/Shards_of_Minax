using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ValenTheTimekeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ValenTheTimekeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Valen the Timekeeper";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1109)); // Dark grey hooded cloak
        AddItem(new Sandals(1)); // Black sandals
        AddItem(new FancyShirt(1150)); // Deep blue shirt
        AddItem(new LongPants(1109)); // Dark grey pants
        AddItem(new AlchemistBookcase()); // A clockwork watch in his inventory, could be stolen

        VirtualArmor = 20;
    }

    public ValenTheTimekeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Valen, keeper of moments lost and found. Tell me, have you an interest in bending time itself?");

        // Dialogue options
        greeting.AddOption("Tell me more about bending time.", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("Ah, time is a mysterious force, always flowing, always changing. I seek artifacts that may help harness its flow. Perhaps you possess something that may be of use?");
                
                // Nested corrupt politician options
                storyModule.AddOption("Why do you want to control time?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule ambitionsModule = new DialogueModule("Why, control over time grants control over destiny itself! Imagine rewriting history, erasing mistakes, or altering fates. Such power is not to be taken lightly, but in the right hands... it can shape the world. I assure you, my intentions are noble—or at least, nobler than some others who might seek such power.");
                        
                        ambitionsModule.AddOption("Noble intentions? You sound rather ambitious.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule ambitionDetailsModule = new DialogueModule("Ambitious? Perhaps. But I prefer the term 'visionary.' The current leaders in this land are weak, afraid to make the hard choices. I am not. Sometimes, sacrifices must be made for the greater good. Besides, what is ambition but the courage to achieve greatness?");
                                
                                ambitionDetailsModule.AddOption("You seem manipulative. What are you really after?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule trueIntentionsModule = new DialogueModule("Manipulative? Such a harsh word. I prefer 'persuasive.' I know how to get what I need, and if a few promises need to be made or a few alliances need to be forged in shadowy corners, then so be it. Power is not given, it is taken. And I will take it, by any means necessary.");
                                        
                                        trueIntentionsModule.AddOption("What kind of alliances are you talking about?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule darkAlliancesModule = new DialogueModule("Ah, you are quite the inquisitive one. Let us just say that there are those who dwell in darkness—individuals with... unique talents. They may not be the kind of people you'd invite to a dinner party, but they understand the value of power and secrecy. We've come to certain agreements, and they help me, as long as I fulfill my end of the bargain.");
                                                
                                                darkAlliancesModule.AddOption("That sounds dangerous. Are you not afraid of betrayal?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule betrayalModule = new DialogueModule("Betrayal is always a possibility, but fear is not something that guides me. Those who serve me know the price of betrayal—an eternity lost in the echoes of time. Besides, when you hold the reins of time itself, there are very few threats that cannot be undone.");
                                                        
                                                        betrayalModule.AddOption("Isn't this all a bit deceitful?", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule deceitModule = new DialogueModule("Deceitful? Perhaps. But remember, the world is not kind to those who play by the rules. A wise ruler knows when to speak truth and when to veil it in shadow. I am merely doing what is necessary to secure the future I envision—one where I am not just a pawn, but a king.");
                                                                
                                                                deceitModule.AddOption("What future do you envision?", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule visionModule = new DialogueModule("A future where power is centralized, where the chaos of weak leadership is replaced by strength and order. Imagine a world where every action is calculated, every decision precisely chosen for the betterment of all, and where those with the will to lead are not constrained by petty moralities. That is the future I see.");
                                                                        
                                                                        visionModule.AddOption("It sounds like you crave ultimate power.", 
                                                                            plaaaaaaa => true, 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                DialogueModule ultimatePowerModule = new DialogueModule("Of course I do! Power is the key to shaping reality itself. Without power, one is at the mercy of others, tossed and turned by the whims of those who are willing to seize it. I refuse to be a victim of circumstance. I will become the master of fate—and if that means bending time to my will, then so be it.");
                                                                                
                                                                                ultimatePowerModule.AddOption("I see. Let’s talk about what you need again.", 
                                                                                    plaaaaaaaa => true, 
                                                                                    plaaaaaaaa =>
                                                                                    {
                                                                                        plaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaa, storyModule));
                                                                                    });
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, ultimatePowerModule));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, visionModule));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, deceitModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, betrayalModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, darkAlliancesModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, trueIntentionsModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, ambitionDetailsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, ambitionsModule));
                    });

                // Trade option after story
                storyModule.AddOption("What do you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a MagicOrb, I can offer you a choice of a unique AlchemistBookcase or a MultiPump, in exchange. The reward, however, can only be claimed once every ten minutes.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MagicOrb to trade?");
                                tradeModule.AddOption("Yes, I have a MagicOrb.", 
                                    plaa => HasMagicOrb(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasMagicOrb(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Return to me when you have a MagicOrb in your possession.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I've traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("The temporal winds haven't settled yet. You may only trade once every ten minutes.");
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
                p.SendMessage("Valen nods knowingly, as if he already sees your future departure.");
            });

        return greeting;
    }

    private bool HasMagicOrb(PlayerMobile player)
    {
        // Check the player's inventory for MagicOrb
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MagicOrb)) != null;
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
        // Remove the MagicOrb and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item magicOrb = player.Backpack.FindItemByType(typeof(MagicOrb));
        if (magicOrb != null)
        {
            magicOrb.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for AlchemistBookcase and MultiPump
            rewardChoiceModule.AddOption("AlchemistBookcase", pl => true, pl => 
            {
                pl.AddToBackpack(new AlchemistBookcase());
                pl.SendMessage("You receive an AlchemistBookcase!");
            });
            
            rewardChoiceModule.AddOption("MultiPump", pl => true, pl =>
            {
                pl.AddToBackpack(new MultiPump());
                pl.SendMessage("You receive a MultiPump!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a MagicOrb.");
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