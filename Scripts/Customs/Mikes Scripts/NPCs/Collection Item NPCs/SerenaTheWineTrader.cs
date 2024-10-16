using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class SerenaTheWineTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SerenaTheWineTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Serena the Wine Trader";
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
        AddItem(new FancyShirt(1265)); // Deep burgundy shirt
        AddItem(new LongPants(1109)); // Black pants
        AddItem(new Sandals(1359)); // Crimson sandals
        AddItem(new FeatheredHat(1445)); // A dark purple feathered hat
        AddItem(new Cloak(1157)); // A wine-colored cloak

        VirtualArmor = 10;
    }

    public SerenaTheWineTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Serena, a collector of rare wines and fine antiquities. Do you happen to carry something of interest, perhaps a QuestWineRack? But before we talk trade, indulge me for a moment. Would you care to learn about the dance of shadows?");

        // Dialogue options
        greeting.AddOption("Tell me more about the dance of shadows.",
            p => true,
            p =>
            {
                DialogueModule danceExplanation = new DialogueModule("Ah, the dance... it is ethereal, haunting, graceful. I was once a performer, you know. The audiences would gaze upon me with wide, fearful eyes, for my movements were said to draw the gaze of beings from beyond our understanding.");
                danceExplanation.AddOption("What kind of beings?", pl => true, pl =>
                {
                    DialogueModule beingsModule = new DialogueModule("Otherworldly beings, spirits from places unseen. They watch, they wait, drawn to the rhythm and grace of my movements. Sometimes I wonder if they are benevolent or if they seek something more sinister.");
                    beingsModule.AddOption("Do they speak to you?", pl2 => true, pl2 =>
                    {
                        DialogueModule speakModule = new DialogueModule("Oh, they speak, though not with words. They whisper in the rustle of leaves, in the shiver of shadows cast by a flickering flame. Their voices are in the sound of distant chimes and in the crackling of ice beneath your feet.");
                        speakModule.AddOption("What do they tell you?", pl3 => true, pl3 =>
                        {
                            DialogueModule tellModule = new DialogueModule("They tell me of worlds unlike our own, of the dark sky where the stars are eyes, watching and waiting. They speak of endless dances, where performers lose themselves in the rhythm until they are no longer flesh, but shadow and memory.");
                            tellModule.AddOption("Does this frighten you?", pl4 => true, pl4 =>
                            {
                                DialogueModule fearModule = new DialogueModule("At first, I was afraid, but fear gives way to fascination, and fascination gives way to madness. I became entranced by my own shadow, spiraling deeper, believing my movements could summon them. Perhaps I was right. Perhaps they are here now, watching us.");
                                fearModule.AddOption("That sounds dangerous.", pl5 => true, pl5 =>
                                {
                                    DialogueModule dangerousModule = new DialogueModule("Dangerous, yes, but also beautiful. To dance on the edge of reason, to feel the presence of the unknown, it is like tasting a forbidden vintage—bitter yet exhilarating.");
                                    dangerousModule.AddOption("What happened to your performances?", pl6 => true, pl6 =>
                                    {
                                        DialogueModule performancesModule = new DialogueModule("The audiences dwindled, and those who remained spoke of feeling something in the air, a sense of unease. Some claimed to see shadows that did not belong. Eventually, no one came. I danced alone, and the unseen were my only audience.");
                                        performancesModule.AddOption("Do you still dance?", pl7 => true, pl7 =>
                                        {
                                            DialogueModule stillDanceModule = new DialogueModule("Oh, yes. I dance beneath the moonlight, when the air is thick with mist and the world feels fragile. My shadow moves with me, and sometimes it moves without me, slipping ahead, lingering behind. It has a will of its own.");
                                            stillDanceModule.AddOption("That sounds unsettling.", pl8 => true, pl8 =>
                                            {
                                                DialogueModule unsettlingModule = new DialogueModule("Unsettling, perhaps, but it is my truth. I cannot help but be drawn to it. To dance is to connect with something greater, something beyond the veil of our understanding. The beings that watch—they are part of me now.");
                                                unsettlingModule.AddOption("I think I understand now.", pl9 => true, pl9 =>
                                                {
                                                    DialogueModule understandModule = new DialogueModule("Do you? Perhaps you do. Perhaps you, too, feel the pull of the unknown. But enough of my ramblings. You came here for something, did you not? Let us discuss your QuestWineRack.");
                                                    AddTradeOptions(understandModule, player);
                                                    pl9.SendGump(new DialogueGump(pl9, understandModule));
                                                });
                                                pl8.SendGump(new DialogueGump(pl8, unsettlingModule));
                                            });
                                            pl7.SendGump(new DialogueGump(pl7, stillDanceModule));
                                        });
                                        pl6.SendGump(new DialogueGump(pl6, performancesModule));
                                    });
                                    pl5.SendGump(new DialogueGump(pl5, dangerousModule));
                                });
                                pl4.SendGump(new DialogueGump(pl4, fearModule));
                            });
                            pl3.SendGump(new DialogueGump(pl3, tellModule));
                        });
                        pl2.SendGump(new DialogueGump(pl2, speakModule));
                    });
                    pl.SendGump(new DialogueGump(pl, beingsModule));
                });
                p.SendGump(new DialogueGump(p, danceExplanation));
            });

        greeting.AddOption("I have a QuestWineRack.",
            p => HasQuestWineRack(p) && CanTradeWithPlayer(p),
            p =>
            {
                CompleteTrade(p);
            });

        greeting.AddOption("I don't have one right now.",
            p => !HasQuestWineRack(p),
            p =>
            {
                p.SendMessage("No worries, traveler. Come back if you find a QuestWineRack.");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        greeting.AddOption("I traded recently; I'll come back later.",
            p => !CanTradeWithPlayer(p),
            p =>
            {
                p.SendMessage("You can only trade once every 10 minutes. Please return later.");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Serena smiles warmly. \"May your journeys be filled with good wine and better company.\"");
            });

        return greeting;
    }

    private void AddTradeOptions(DialogueModule module, PlayerMobile player)
    {
        module.AddOption("I do have a QuestWineRack.",
            pl => HasQuestWineRack(pl) && CanTradeWithPlayer(pl),
            pl =>
            {
                CompleteTrade(pl);
            });

        module.AddOption("I don't have one right now.",
            pl => !HasQuestWineRack(pl),
            pl =>
            {
                pl.SendMessage("No worries, traveler. Come back if you find a QuestWineRack.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });

        module.AddOption("I traded recently; I'll come back later.",
            pl => !CanTradeWithPlayer(pl),
            pl =>
            {
                pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });
    }

    private bool HasQuestWineRack(PlayerMobile player)
    {
        // Check the player's inventory for QuestWineRack
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(QuestWineRack)) != null;
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
        // Remove the QuestWineRack and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item questWineRack = player.Backpack.FindItemByType(typeof(QuestWineRack));
        if (questWineRack != null)
        {
            questWineRack.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("As a token of appreciation, you may choose between a FancyCrystalSkull or a StoneHead. Which do you prefer?");

            // Add options for FancyCrystalSkull and StoneHead
            rewardChoiceModule.AddOption("FancyCrystalSkull", pl => true, pl =>
            {
                pl.AddToBackpack(new FancyCrystalSkull());
                pl.SendMessage("You receive a FancyCrystalSkull!");
            });

            rewardChoiceModule.AddOption("StoneHead", pl => true, pl =>
            {
                pl.AddToBackpack(new StoneHead());
                pl.SendMessage("You receive a StoneHead!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a QuestWineRack.");
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