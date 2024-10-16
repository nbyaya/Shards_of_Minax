using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class GarrickTheExplorer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public GarrickTheExplorer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Garrick the Explorer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(2150)); // Fancy shirt with a unique hue
        AddItem(new LongPants(1109)); // Dark brown pants
        AddItem(new Boots(1175)); // Light brown boots
        AddItem(new FeatheredHat(1121)); // Blue feathered hat
        AddItem(new LeatherGloves()); // Leather gloves for an adventurer look

        VirtualArmor = 15;
    }

    public GarrickTheExplorer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! I am Garrick, an explorer of the forgotten and the lost. You seem like someone who appreciates rare treasures. Are you interested in making a trade?");

        // Start with dialogue about his explorations
        greeting.AddOption("Tell me about your adventures.", 
            p => true, 
            p =>
            {
                DialogueModule adventuresModule = new DialogueModule("I've traveled far and wide, from the scorching deserts to the icy peaks. Each journey brings new tales and discoveries. But beware, not everyone is as they seem, and many have tried to steal my secrets! Do you wish to hear about the Sand Serpent, the Crystal Caverns, the Lost City of Axion, or perhaps something... more secretive?");

                // Nested options for each adventure
                adventuresModule.AddOption("Tell me about the Sand Serpent.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule serpentModule = new DialogueModule("The Sand Serpent is a beast that dwells beneath the desert sands, waiting for unsuspecting prey. Its scales are said to shimmer like gold, and its venom can either kill or cure, depending on how it's used. I once tried to extract its venom, but I was ambushed... those blasted thieves thought they could take my research!");
                        serpentModule.AddOption("How can its venom be used?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule venomModule = new DialogueModule("The venom, when mixed with desert rose petals, can create an elixir that grants resistance to poison. However, extracting it is no easy task. You must be careful... people are always watching, trying to steal what they do not understand! Did you know that I once had a partner who betrayed me over this very elixir?");
                                venomModule.AddOption("What happened with your partner?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule partnerModule = new DialogueModule("Ah, yes, my so-called partner, Silas. We worked together for years, creating devices and discovering secrets that could change the world. But he grew greedy, and one day he tried to sell our secrets to the highest bidder. I barely escaped with my notes, and now I trust no one... except perhaps you, traveler. You seem different.");
                                        partnerModule.AddOption("I appreciate your trust, Garrick.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, partnerModule));
                                    });
                                venomModule.AddOption("Fascinating! Thank you.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, venomModule));
                            });
                        serpentModule.AddOption("Sounds too dangerous for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, serpentModule));
                    });

                adventuresModule.AddOption("Tell me about the Crystal Caverns.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule cavernsModule = new DialogueModule("The Crystal Caverns are a labyrinth of glittering tunnels, filled with dangerous creatures and rare minerals. I discovered a unique crystal there, one that hums with a strange energy. I tried to bring it back, but I was followed... always followed. They want what I have, but they won't get it.");
                        cavernsModule.AddOption("Who was following you?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule followersModule = new DialogueModule("A shadowy group, they call themselves The Shroud. They believe my discoveries belong to them, that they have some claim over what I find. But I am too clever for them! I have hidden my best findings in places only I know. Sometimes, I wonder if they watch me even now... I must be careful.");
                                followersModule.AddOption("The Shroud? Tell me more about them.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule shroudModule = new DialogueModule("The Shroud are an organization of thieves and spies, obsessed with knowledge and control. They think they can use my inventions to gain power, but they lack the brilliance to understand my work! I've had to take precautions, traps, secret compartments... no one will find my secrets unless I want them to.");
                                        shroudModule.AddOption("You are truly brilliant, Garrick.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, shroudModule));
                                    });
                                followersModule.AddOption("I hope they never find you.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, followersModule));
                            });
                        cavernsModule.AddOption("Did you bring anything back from the caverns?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule crystalModule = new DialogueModule("I managed to bring back a small shard, just a fragment of the larger crystal. It pulses with energy, and I believe it could be used to power something... something grand. But I must keep it hidden until I can complete my work. Too many eyes are on me.");
                                crystalModule.AddOption("Could I help you with your work?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Perhaps... perhaps you could. But you must be trustworthy, and you must never speak of this to anyone. There are things I need, rare components that are difficult to find. If you could gather them, I could finally complete my masterpiece, a device that could change everything.");
                                        helpModule.AddOption("What components do you need?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Garrick hands you a list of rare components, scribbled hastily on parchment.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        helpModule.AddOption("I'm not sure I'm ready for that.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, helpModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, crystalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, cavernsModule));
                    });

                // After they’ve learned about his adventures, introduce the trade option
                adventuresModule.AddOption("Do you need anything for your travels?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of a LeatherStrapBelt for one of my expeditions. In return, I can offer you a TrexSkull and a MaxxiaScroll. But remember, I can only make this trade once every 10 minutes. Too many eyes are always watching, trying to take what is mine.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a LeatherStrapBelt for me?");
                                tradeModule.AddOption("Yes, I have a LeatherStrapBelt.", 
                                    plaa => HasLeatherStrapBelt(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasLeatherStrapBelt(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a LeatherStrapBelt.");
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
                        tradeIntroductionModule.AddOption("Perhaps another time.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, adventuresModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Garrick nods and tips his hat to you, a glint of paranoia still in his eyes.");
            });

        return greeting;
    }

    private bool HasLeatherStrapBelt(PlayerMobile player)
    {
        // Check the player's inventory for LeatherStrapBelt
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LeatherStrapBelt)) != null;
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
        // Remove the LeatherStrapBelt and give the TrexSkull and MaxxiaScroll, then set the cooldown timer
        Item leatherStrapBelt = player.Backpack.FindItemByType(typeof(LeatherStrapBelt));
        if (leatherStrapBelt != null)
        {
            leatherStrapBelt.Delete();
            player.AddToBackpack(new TrexSkull());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the LeatherStrapBelt and receive a TrexSkull and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a LeatherStrapBelt.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
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