using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FrostbiteTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FrostbiteTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Frostbite, the Wandering Trader";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(80);
        SetInt(110);

        SetHits(100);
        SetMana(150);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1153)); // A mysterious dark blue shroud
        AddItem(new Sandals(2125)); // Black sandals
        AddItem(new BodySash(1266)); // Dark gray sash
        AddItem(new Kilt(1109)); // Deep blue kilt
        AddItem(new GlassTable()); // A unique, lore-friendly pair of glasses

        VirtualArmor = 15;
    }

    public FrostbiteTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler seeking treasures in the bitter cold, are you? I am Frostbite, a trader of unique items. I collect rare trinkets in exchange for valuable goods.");

        // Dialogue options
        greeting.AddOption("Tell me about your travels.",
            p => true,
            p =>
            {
                DialogueModule storyModule = new DialogueModule("The frozen wastelands hold many secrets, and I have wandered far, trading my wares for rare and mystical items. I am always in search of something unique. But... there is more to my story, if you have the time to listen.");

                storyModule.AddOption("What haunts you, Frostbite?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hauntedModule = new DialogueModule("Haunts... yes. I have seen things no one should see. I once lived a peaceful life, but that was taken from me. My village, my family, all lost to the cold, unforgiving night. I survived, but at what cost? The guilt, the loneliness... they follow me like shadows that never fade.");

                        hauntedModule.AddOption("You must feel so alone...",
                            pla => true,
                            pla =>
                            {
                                DialogueModule aloneModule = new DialogueModule("Alone, yes. Even in the bustling cities, I feel detached. The faces of strangers blur together, and I wonder if I will ever belong anywhere again. But I keep moving, searching for something... perhaps redemption, or perhaps just a reason to keep going.");

                                aloneModule.AddOption("What keeps you going?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule reasonModule = new DialogueModule("Hope, perhaps. A flicker of it, faint and fragile. I believe that maybe, one day, I will find a way to atone for what happened. I tell myself that every PlatinumChip I trade, every trinket I pass on, brings me closer to some semblance of peace. But deep down, the shadows remain.");

                                        reasonModule.AddOption("Do you believe redemption is possible?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule redemptionModule = new DialogueModule("I want to believe it is. But there are days when doubt overwhelms me. The faces of those I lost appear in my dreams, and I hear their voices. They ask me why I lived, why I didn't save them. I have no answers. All I can do is keep wandering, keep trading, and hope that one day, I will find a way to silence those voices.");

                                                redemptionModule.AddOption("I'm sorry for your pain, Frostbite.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Frostbite looks down, his eyes distant. 'Thank you... It means more than you know.'");
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, redemptionModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, reasonModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, aloneModule));
                            });

                        hauntedModule.AddOption("Do you need anything in particular?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Indeed, traveler! If you have a PlatinumChip, I can offer you a choice: a TribalHelm or a GlassTable. Additionally, I'll throw in a MaxxiaScroll as a token of my appreciation. The items I trade are more than just objects; they are pieces of my journey, fragments of what I once was.");

                                tradeModule.AddOption("I'd like to make the trade.",
                                    plaa => CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        DialogueModule confirmTradeModule = new DialogueModule("Do you have a PlatinumChip with you?");
                                        confirmTradeModule.AddOption("Yes, here it is.",
                                            plaaa => HasPlatinumChip(plaaa) && CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });
                                        confirmTradeModule.AddOption("No, I don't have one.",
                                            plaaa => !HasPlatinumChip(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Return when you have a PlatinumChip to offer.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        confirmTradeModule.AddOption("I'll come back later.",
                                            plaaa => !CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, confirmTradeModule));
                                    });
                                tradeModule.AddOption("Not right now.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeModule));
                            });

                        hauntedModule.AddOption("Goodbye, Frostbite.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Frostbite nods, his eyes clouded with old memories. 'Safe travels, traveler... may you never face the cold I have.'");
                            });

                        pl.SendGump(new DialogueGump(pl, hauntedModule));
                    });

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Frostbite nods and shivers slightly in the cold wind.");
            });

        return greeting;
    }

    private bool HasPlatinumChip(PlayerMobile player)
    {
        // Check the player's inventory for PlatinumChip
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(PlatinumChip)) != null;
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
        // Remove the PlatinumChip and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item platinumChip = player.Backpack.FindItemByType(typeof(PlatinumChip));
        if (platinumChip != null)
        {
            platinumChip.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for TribalHelm and GlassTable
            rewardChoiceModule.AddOption("TribalHelm", pl => true, pl =>
            {
                pl.AddToBackpack(new TribalHelm());
                pl.SendMessage("You receive a TribalHelm!");
            });

            rewardChoiceModule.AddOption("GlassTable", pl => true, pl =>
            {
                pl.AddToBackpack(new GlassTable());
                pl.SendMessage("You receive a GlassTable!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a PlatinumChip.");
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