using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZephyrWindwhisperer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZephyrWindwhisperer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zephyr the Windwhisperer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // A light blue shirt
        AddItem(new LongPants(2406)); // Dark green pants
        AddItem(new Sandals(2211)); // Teal sandals
        AddItem(new FeatheredHat(2125)); // A dark purple feathered hat
        AddItem(new HalfApron(2603)); // A wind-colored apron

        VirtualArmor = 15;
    }

    public ZephyrWindwhisperer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Zephyr, the Windwhisperer. The winds carry tales, secrets, and sometimes... treasures. Are you interested in what I have to offer?");
        
        // Dialogue options
        greeting.AddOption("Tell me more about your treasures.", 
            p => true, 
            p =>
            {
                DialogueModule treasuresModule = new DialogueModule("The wind, it brings many strange things to my hands. I often find peculiar items that I am willing to trade for something equally unique. Do you happen to have a RibEye with you?");
                
                treasuresModule.AddOption("What else do the winds whisper to you, Zephyr?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule whispersModule = new DialogueModule("Ah, the winds... they speak to me of darkness, of ancient beings that linger just beyond our sight. They whisper to me tales of a world unseen, of monstrous forms that emerge when shadows lengthen. You see, I am more than just a trader... I am a vessel for the entities that dwell in the in-between.");
                        
                        whispersModule.AddOption("You speak of entities... what are they like?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule entitiesModule = new DialogueModule("They are marvelous, terrible beings! They dance in my visions, guiding my hand as I paint their forms. My art, you see, is but a reflection of the things they show me. Do not mistake them for mere musings of a delusional artist—no, they are real, and they breathe life into my creations. In the dark, they come to life, writhing and reaching for those who dare to gaze upon them.");
                                
                                entitiesModule.AddOption("Are your paintings dangerous?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule dangerousModule = new DialogueModule("Dangerous? Oh, yes, indeed! The macabre beauty of my art is a snare for the curious. Those who look too long might find themselves lost within the canvas, their minds consumed by the horrors lurking within. My downfall began when I tried to contain them, to capture the pure essence of what the wind whispered to me... but they cannot be contained.");
                                        
                                        dangerousModule.AddOption("What happened to you, Zephyr?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule downfallModule = new DialogueModule("My obsession was my undoing. I spent days, weeks, even months, perfecting the depictions of my visions. I refused to sleep, to eat, to heed the warnings of those around me. The entities grew stronger as I grew weaker. They whispered promises of eternal creativity, of visions beyond comprehension... and I believed them. I still do, in some ways. But now I am here, a mere shadow of what I once was, caught between the reality of this world and the horror of theirs.");
                                                
                                                downfallModule.AddOption("Can you still create these visions?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule createModule = new DialogueModule("Yes, yes! The winds have not abandoned me entirely. I still create, though now with a price. Each stroke of my brush brings them closer, each sculpture I craft invites them into this world. The treasures I offer are fragments of what I once was—tokens of my devotion to the macabre, and the price I paid for my art.");
                                                        
                                                        createModule.AddOption("I have a RibEye. Can we make a trade?", 
                                                            plaaaaa => HasRibEye(plaaaaa) && CanTradeWithPlayer(plaaaaa), 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Wonderful! I can offer you either a ButterChurn or a StainedWindow. Which would you prefer?");
                                                                
                                                                // Options for ButterChurn or StainedWindow
                                                                tradeModule.AddOption("I would like the ButterChurn.", 
                                                                    plaq => true, 
                                                                    plaq =>
                                                                    {
                                                                        CompleteTrade(pla, new ButterChurn());
                                                                        pla.SendMessage("You receive a ButterChurn!");
                                                                    });
                                                                
                                                                tradeModule.AddOption("I would like the StainedWindow.", 
                                                                    plaw => true, 
                                                                    plaw =>
                                                                    {
                                                                        CompleteTrade(pla, new StainedWindow());
                                                                        pla.SendMessage("You receive a StainedWindow!");
                                                                    });
                                                                
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, tradeModule));
                                                            });
                                                        
                                                        createModule.AddOption("Maybe another time, Zephyr.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("The winds will be here, whispering their tales. Come find me when you are ready.");
                                                            });
                                                        
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, createModule));
                                                    });
                                                
                                                downfallModule.AddOption("I must leave now, Zephyr.", 
                                                    plaaae => true, 
                                                    plaaae =>
                                                    {
                                                        plaaa.SendMessage("Zephyr stares off into the distance, his eyes glazed over as if he were seeing something beyond this world.");
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, downfallModule));
                                            });
                                        
                                        dangerousModule.AddOption("I should be going.", 
                                            plaar => true, 
                                            plaar =>
                                            {
                                                plaa.SendMessage("Zephyr waves absentmindedly, his attention already drifting back to the unseen entities around him.");
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, dangerousModule));
                                    });
                                
                                entitiesModule.AddOption("I think I have heard enough.", 
                                    plat => true, 
                                    plat =>
                                    {
                                        pla.SendMessage("Zephyr gives a knowing smile, as if understanding your reluctance to delve deeper.");
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, entitiesModule));
                            });
                        
                        whispersModule.AddOption("That sounds unsettling. I think I should go.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Zephyr nods, his eyes distant as he listens to the voices only he can hear.");
                            });
                        
                        pl.SendGump(new DialogueGump(pl, whispersModule));
                    });
                
                treasuresModule.AddOption("I have a RibEye. Can we make a trade?", 
                    pl => HasRibEye(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Wonderful! I can offer you either a ButterChurn or a StainedWindow. Which would you prefer?");
                        
                        // Options for ButterChurn or StainedWindow
                        tradeModule.AddOption("I would like the ButterChurn.", 
                            pla => true, 
                            pla =>
                            {
                                CompleteTrade(pla, new ButterChurn());
                                pla.SendMessage("You receive a ButterChurn!");
                            });
                        
                        tradeModule.AddOption("I would like the StainedWindow.", 
                            pla => true, 
                            pla =>
                            {
                                CompleteTrade(pla, new StainedWindow());
                                pla.SendMessage("You receive a StainedWindow!");
                            });
                        
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                
                treasuresModule.AddOption("I do not have a RibEye right now.", 
                    pl => !HasRibEye(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a RibEye.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                treasuresModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, treasuresModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zephyr nods and whispers something unintelligible to the wind.");
            });

        return greeting;
    }

    private bool HasRibEye(PlayerMobile player)
    {
        // Check the player's inventory for RibEye
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RibEye)) != null;
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

    private void CompleteTrade(PlayerMobile player, Item reward)
    {
        // Remove the RibEye and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item ribEye = player.Backpack.FindItemByType(typeof(RibEye));
        if (ribEye != null)
        {
            ribEye.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll
            player.AddToBackpack(reward); // Give the chosen reward item

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a RibEye.");
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