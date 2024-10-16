using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class FrostwyndTheArcaneSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FrostwyndTheArcaneSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Frostwynd, the Arcane Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1153)); // Dark blue hooded robe
        AddItem(new Boots(1109)); // Midnight blue boots
        AddItem(new BodySash(1153)); // Dark blue sash
        AddItem(new GnarledStaff()); // Gnarled staff to emphasize his arcane nature

        VirtualArmor = 15;
    }

    public FrostwyndTheArcaneSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, traveler! I am Frostwynd, seeker of rare arcane relics. Do you possess the FluxCompound, the item of legend?");
        
        // Dialogue options
        greeting.AddOption("What do you want with the FluxCompound?", 
            p => true, 
            p =>
            {
                DialogueModule explanationModule = new DialogueModule("The FluxCompound is a rare material, brimming with the energy I require for my experiments. If you have it, I can offer you something in return.");
                
                explanationModule.AddOption("I have a FluxCompound. What can you offer in exchange?", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Wonderful! I can offer you one of two items: an ArtisanHolidayTree or a TinyWizard. Additionally, you will receive a MaxxiaScroll.");
                        tradeModule.AddOption("I'd like to trade for the ArtisanHolidayTree.",
                            plaa => HasFluxCompound(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa, new ArtisanHolidayTree());
                            });
                        tradeModule.AddOption("I'd like to trade for the TinyWizard.",
                            plaa => HasFluxCompound(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa, new TinyWizard());
                            });
                        tradeModule.AddOption("I need more time to decide.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                
                explanationModule.AddOption("Tell me more about yourself.",
                    pla => true,
                    pla =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("I am Frostwynd, once a humble mapmaker. I charted lands that no one had ever seen, places connected to dark realms where strange forces dwell. My maps are filled with symbols and warnings, but none believed me. They called me paranoid, said my mind had slipped into obsession. Yet, here I am, still uncovering secrets and seeking lost relics to confirm my discoveries.");
                        
                        backstoryModule.AddOption("Why were your maps filled with warnings?", 
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule warningModule = new DialogueModule("My warnings were for those who would heed them. There are places in this world that were never meant for mortal eyes, paths that lead into shadow, into madness. I mapped these places because I had to know. I had to understand. And now, I see that I was right. The dark realms, they are real, and they seep into our world in ways few understand.");
                                
                                warningModule.AddOption("Tell me about the dark realms.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule darkRealmsModule = new DialogueModule("The dark realms are twisted reflections of our own lands, filled with terrors unseen. They shift, change, and defy reason. I have charted them obsessively, even when it cost me my sanity. My maps are littered with erratic symbols—warnings to those brave or foolish enough to tread those paths. Some say I am paranoid, but I know the truth. These realms are connected, and they are watching us.");
                                        
                                        darkRealmsModule.AddOption("What do these symbols mean?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule symbolsModule = new DialogueModule("The symbols are my own invention—a language I devised to convey dangers where words fall short. A spiral means a place where time is not constant. A jagged line indicates a boundary that should never be crossed. And a black star... well, that marks a place of ultimate peril, where the boundary between worlds is thin, and the creatures of the dark can reach through.");
                                                
                                                symbolsModule.AddOption("Have you ever encountered these creatures?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule creaturesModule = new DialogueModule("Yes, I have seen them. Shadows with form, eyes that stare back from the darkness, whispers that promise knowledge but only deliver madness. They do not belong here, yet they find a way through. I have spent my life learning how to avoid them, how to ward them off. That is why I need items like the FluxCompound—to keep the darkness at bay, to continue my work.");
                                                        
                                                        creaturesModule.AddOption("What do you do with the FluxCompound?",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                DialogueModule fluxModule = new DialogueModule("The FluxCompound is a key—a material that holds the power to stabilize the thin boundaries between our world and the dark realms. I use it in my experiments, creating wards and devices to keep the dark from seeping through. It is dangerous work, but necessary. Without it, the dark realms would consume us all.");
                                                                
                                                                fluxModule.AddOption("I have a FluxCompound. What can you offer in exchange?", 
                                                                    pl => CanTradeWithPlayer(pl), 
                                                                    pl =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Wonderful! I can offer you one of two items: an ArtisanHolidayTree or a TinyWizard. Additionally, you will receive a MaxxiaScroll.");
                                                                        tradeModule.AddOption("I'd like to trade for the ArtisanHolidayTree.",
                                                                            plaaq => HasFluxCompound(plaaq) && CanTradeWithPlayer(plaaq),
                                                                            plaaq =>
                                                                            {
                                                                                CompleteTrade(plaa, new ArtisanHolidayTree());
                                                                            });
                                                                        tradeModule.AddOption("I'd like to trade for the TinyWizard.",
                                                                            plaaq => HasFluxCompound(plaaq) && CanTradeWithPlayer(plaaq),
                                                                            plaaq =>
                                                                            {
                                                                                CompleteTrade(plaa, new TinyWizard());
                                                                            });
                                                                        tradeModule.AddOption("I need more time to decide.",
                                                                            plaaq => true,
                                                                            plaaq =>
                                                                            {
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        
                                                                        pl.SendGump(new DialogueGump(pl, tradeModule));
                                                                    });
                                                                
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, fluxModule));
                                                            });
                                                        
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, creaturesModule));
                                                    });
                                                
                                                plaaaa.SendGump(new DialogueGump(plaaaa, symbolsModule));
                                            });
                                        
                                        plaaa.SendGump(new DialogueGump(plaaa, darkRealmsModule));
                                    });
                                
                                plaa.SendGump(new DialogueGump(plaa, warningModule));
                            });
                        
                        pla.SendGump(new DialogueGump(pla, backstoryModule));
                    });
                
                explanationModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendMessage("Frostwynd nods, understanding your hesitation.");
                    });
                
                p.SendGump(new DialogueGump(p, explanationModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Frostwynd inclines his head, a hint of mystery in his eyes.");
            });

        return greeting;
    }

    private bool HasFluxCompound(PlayerMobile player)
    {
        // Check the player's inventory for FluxCompound
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FluxCompound)) != null;
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

    private void CompleteTrade(PlayerMobile player, Item rewardItem)
    {
        // Remove the FluxCompound and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fluxCompound = player.Backpack.FindItemByType(typeof(FluxCompound));
        if (fluxCompound != null)
        {
            fluxCompound.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll
            player.AddToBackpack(rewardItem); // Give the chosen reward

            player.SendMessage($"You receive a {rewardItem.GetType().Name} and a MaxxiaScroll!");

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a FluxCompound.");
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