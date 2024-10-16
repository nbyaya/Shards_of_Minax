using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class VesperaTheStargazer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public VesperaTheStargazer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Vespera the Stargazer";
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
        AddItem(new Robe(1109)); // Midnight blue robe
        AddItem(new Sandals(1150)); // Silver sandals
        AddItem(new WizardsHat(1109)); // Midnight blue wizard's hat
        AddItem(new Lantern() { Movable = false, Light = LightType.Circle300 }); // Permanent lantern, representing her stargazing habit

        VirtualArmor = 10;
    }

    public VesperaTheStargazer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Vespera, an observer of the stars. Tell me, do you share my fascination for the mysteries of the night sky?");
        
        // Dialogue options
        greeting.AddOption("Tell me more about the stars.", 
            p => true, 
            p =>
            {
                DialogueModule starsModule = new DialogueModule("The night sky is full of wonders, each star a story waiting to be told. I use my telescope to gaze deeper into the cosmos. Say, do you happen to have an AmateurTelescope with you?");
                
                // Nested options to expand the story
                starsModule.AddOption("Why are you so interested in the stars?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule obsessionModule = new DialogueModule("Ah, the stars... they hold secrets that go beyond the simple beauty you see. They whisper to me, revealing forbidden knowledge, artifacts hidden away for centuries. I collect them, you see. Each one brings me closer to understanding what lies beyond.");

                        obsessionModule.AddOption("Artifacts? What kind of artifacts?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule artifactsModule = new DialogueModule("Artifacts of immense power, relics of forgotten rituals. They are whispered about only in the darkest of circles. Some say they drive the collector mad, but I know better. They grant power, and with power comes clarity. Wouldn't you want to know what the universe is hiding from us?");

                                artifactsModule.AddOption("That sounds dangerous. Are you not afraid?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Dangerous? Perhaps. But the rewards are worth it. The voices, they tell me things, secrets of those who once wielded this power. They warn me, guide me, and... sometimes, they show me things that no one else can see. I would rather embrace the danger than live in ignorance.");

                                        dangerModule.AddOption("What do these voices say?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule voicesModule = new DialogueModule("They speak of rituals, of the old ways that were once practiced in secret. They reveal to me the locations of powerful artifacts, hidden away so no one else may find them. At times, they scream, other times they whisper, but always, they guide me to more power.");

                                                voicesModule.AddOption("I think you should be careful with this power.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Vespera's eyes narrow, and she gives a small, secretive smile. 'Careful? Perhaps. But power is not meant to be handled with caution. It is meant to be wielded.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });

                                                voicesModule.AddOption("Can I help you find these artifacts?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Vespera tilts her head, studying you carefully. 'Perhaps you could... if you are truly willing to risk your sanity. The artifacts have a way of testing those who seek them.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, dangerModule));
                                    });

                                artifactsModule.AddOption("What do you plan to do with these artifacts?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule planModule = new DialogueModule("My plans? Power, knowledge, and perhaps a way to touch the stars themselves. To transcend this mortal coil and become something more. The artifacts are the key, each one a step closer to unlocking the truth.");

                                        planModule.AddOption("This sounds like madness.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Vespera's eyes blaze with an intense light. 'Madness? Perhaps. But the line between madness and genius is thin, and I will cross it if I must.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                    });

                                pla.SendGump(new DialogueGump(pla, artifactsModule));
                            });

                        pl.SendGump(new DialogueGump(pl, obsessionModule));
                    });
                
                // Trade option after story
                starsModule.AddOption("I have an AmateurTelescope. Can we trade?", 
                    pl => HasAmateurTelescope(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                starsModule.AddOption("I don't have a telescope right now.", 
                    pl => !HasAmateurTelescope(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have an AmateurTelescope. The stars will be waiting.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                starsModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, starsModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Vespera smiles and looks back at the stars, her eyes glinting with something unspoken.");
            });

        return greeting;
    }

    private bool HasAmateurTelescope(PlayerMobile player)
    {
        // Check the player's inventory for AmateurTelescope
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AmatureTelescope)) != null;
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
        // Remove the AmateurTelescope and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item amateurTelescope = player.Backpack.FindItemByType(typeof(AmatureTelescope));
        if (amateurTelescope != null)
        {
            amateurTelescope.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for AnniversaryPainting and MagicBookStand
            rewardChoiceModule.AddOption("AnniversaryPainting", pl => true, pl => 
            {
                pl.AddToBackpack(new AnniversaryPainting());
                pl.SendMessage("You receive an AnniversaryPainting!");
            });
            
            rewardChoiceModule.AddOption("MagicBookStand", pl => true, pl =>
            {
                pl.AddToBackpack(new MagicBookStand());
                pl.SendMessage("You receive a MagicBookStand!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an AmateurTelescope.");
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