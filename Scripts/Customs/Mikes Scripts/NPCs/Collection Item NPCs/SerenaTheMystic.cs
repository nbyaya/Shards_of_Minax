using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SerenaTheMystic : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SerenaTheMystic() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Serena the Mystic";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1153)); // Deep blue robe
        AddItem(new Sandals(1)); // Black sandals
        AddItem(new Cloak(1150)); // Mystic dark cloak
        AddItem(new GnarledStaff()); // Staff
        AddItem(new WizardsHat(1153)); // Deep blue wizard's hat

        VirtualArmor = 12;
    }

    public SerenaTheMystic(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Serena, a seeker of hidden knowledge and a guardian of mystical artifacts. Might I interest you in a unique exchange?");
        
        // Dialogue options
        greeting.AddOption("What kind of exchange are you offering?", 
            p => true, 
            p =>
            {
                DialogueModule tradeModule = new DialogueModule("I am in search of a Bottled Plague. Should you possess one, I can offer you a choice of reward: a Nixie Statue or Grandma's Special Rolls. Additionally, I will always bestow upon you a Maxxia Scroll for your troubles.");
                
                tradeModule.AddOption("I have a Bottled Plague to trade.", 
                    pl => HasBottledPlague(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                tradeModule.AddOption("I don't have a Bottled Plague.", 
                    pl => !HasBottledPlague(pl), 
                    pl =>
                    {
                        pl.SendMessage("Return when you have a Bottled Plague to trade, and I shall reward you.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                tradeModule.AddOption("I recently traded; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You must wait a while before trading again. Please return in 10 minutes.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                tradeModule.AddOption("Who are you really, Serena?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule whoAreYouModule = new DialogueModule("I was once an apprentice to a Broken Clockmaker, a man obsessed with time itself. He sought to control it, to manipulate the ebb and flow of reality. He was ingenious, but his obsession cost him dearly.");
                        
                        whoAreYouModule.AddOption("What happened to the Clockmaker?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule clockmakerModule = new DialogueModule("His family was lost to the merciless sands of time, and he turned his sorrow into a fixation on crafting clocks. Each clock was more intricate than the last, until he began to craft ones that twisted time itself. He created a clock that could reverse a minute, then an hour, but it was never enough.");

                                clockmakerModule.AddOption("Did he succeed in controlling time?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule successModule = new DialogueModule("Success? Perhaps. But it was a hollow one. He managed to stop time for a moment, to trap himself in a bubble where nothing aged, nothing changed. In that moment, he realized his folly: Time wasn't the enemy. It was his refusal to let go of what was lost. He vanished, leaving behind only his clocks.");

                                        successModule.AddOption("Do you have one of these clocks?", 
                                            plaab => true, 
                                            plaab =>
                                            {
                                                DialogueModule clockModule = new DialogueModule("I do indeed, but it is not something I part with lightly. The clock holds power, but also deep melancholy. It is a reminder of what can happen when we obsess over what cannot be changed.");

                                                clockModule.AddOption("Could I see the clock?", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Serena opens her cloak slightly, revealing a small clock hanging by a chain. Its hands move erratically, as if struggling against some unseen force.");
                                                    });

                                                clockModule.AddOption("Thank you for sharing, Serena.", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Serena nods, her eyes carrying the weight of a thousand ticking clocks.");
                                                    });

                                                plaab.SendGump(new DialogueGump(plaab, clockModule));
                                            });

                                        successModule.AddOption("What happened to the clocks he made?", 
                                            plaab => true, 
                                            plaab =>
                                            {
                                                DialogueModule clocksModule = new DialogueModule("Some were scattered across the lands, others lost to the depths of time itself. A few remain, each with its own power. They say one can speed up time, another can freeze it, and yet another can make you relive a moment forever. Beware if you find one, traveler, for they carry the burden of his obsession.");

                                                clocksModule.AddOption("I will be careful.", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Serena smiles sadly. 'I hope so, for your sake.'");
                                                    });

                                                clocksModule.AddOption("I want to find one.", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Serena shakes her head. 'Some things are better left undisturbed, but if you must... the journey will be perilous.'");
                                                    });

                                                plaab.SendGump(new DialogueGump(plaab, clocksModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, successModule));
                                    });

                                clockmakerModule.AddOption("That's a tragic tale.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Serena sighs, her eyes distant. 'It is indeed. A tale of genius lost to grief.'");
                                    });

                                pla.SendGump(new DialogueGump(pla, clockmakerModule));
                            });

                        pl.SendGump(new DialogueGump(pl, whoAreYouModule));
                    });

                p.SendGump(new DialogueGump(p, tradeModule));
            });

        greeting.AddOption("Farewell.", 
            p => true, 
            p =>
            {
                p.SendMessage("Serena nods knowingly, her eyes glinting with mysterious knowledge.");
            });

        return greeting;
    }

    private bool HasBottledPlague(PlayerMobile player)
    {
        // Check the player's inventory for BottledPlague
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BottledPlague)) != null;
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
        // Remove the BottledPlague and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item bottledPlague = player.Backpack.FindItemByType(typeof(BottledPlague));
        if (bottledPlague != null)
        {
            bottledPlague.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for Nixie Statue and Grandma's Special Rolls
            rewardChoiceModule.AddOption("Nixie Statue", pl => true, pl => 
            {
                pl.AddToBackpack(new NixieStatue());
                pl.SendMessage("You receive a Nixie Statue!");
            });
            
            rewardChoiceModule.AddOption("Grandma's Special Rolls", pl => true, pl =>
            {
                pl.AddToBackpack(new GrandmasSpecialRolls());
                pl.SendMessage("You receive Grandma's Special Rolls!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a Bottled Plague.");
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