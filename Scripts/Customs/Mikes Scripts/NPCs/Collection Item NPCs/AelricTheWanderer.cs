using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AelricTheWanderer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AelricTheWanderer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aelric the Wanderer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1109)); // Dark hooded cloak
        AddItem(new Sandals(1175)); // Light brown sandals
        AddItem(new FancyShirt(2413)); // Deep blue shirt
        AddItem(new LongPants(1109)); // Black pants
        AddItem(new StaffOfPower()); // A mystical staff for decoration

        VirtualArmor = 20;
    }

    public AelricTheWanderer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Aelric, a wanderer seeking the mysteries of the world. Perhaps you can help me, and in return, I have something for you.");

        // Dialogue options
        greeting.AddOption("What mysteries do you seek?", 
            p => true, 
            p =>
            {
                DialogueModule mysteryModule = new DialogueModule("I seek the rare artifact known as the WaterFountain. If you possess one, I would be willing to trade you something of equal value.");

                // Nested dialogues
                mysteryModule.AddOption("Why do you seek the WaterFountain?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule hauntedStoryModule = new DialogueModule("The WaterFountain is a key to uncovering the truth behind a battle that haunts me still. The battle of Dreadhollow... it was a slaughter. I fought bravely, but no amount of courage could save my comrades.");

                        hauntedStoryModule.AddOption("What happened at Dreadhollow?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule battleModule = new DialogueModule("We were ambushed. The fog rolled in thicker than any I'd seen. It swallowed our senses, and out of that fog came creatures—monsters whose eyes glowed with a cruel hunger. They tore through our lines. I watched as friends, men and women I'd trained beside, fell. I survived, but not without cost.");

                                battleModule.AddOption("How did you survive?", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("I fought until my strength failed me. I remember falling, and then... darkness. When I awoke, I was alone, surrounded by the fallen. It was as if the fog had taken pity on me. But I know it wasn't mercy that saved me. Something—or someone—allowed me to live, and I need to know why.");

                                        survivalModule.AddOption("That sounds terrible. How have you managed since then?", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                DialogueModule copingModule = new DialogueModule("I wander because staying still brings the memories back too strongly. The screams, the faces... they haunt me. But I can't let them go until I understand why I was spared. The WaterFountain may hold answers; it was rumored to be an artifact of great power, connected to the mysteries of the fog that engulfed us.");

                                                copingModule.AddOption("I'll help you find answers.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Aelric's eyes soften, and he nods.");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateTradeModule(plaaaaa)));
                                                    });

                                                copingModule.AddOption("I can't help you right now.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Aelric nods solemnly. 'Perhaps another time, then.'");
                                                    });

                                                plaaaa.SendGump(new DialogueGump(plaaaa, copingModule));
                                            });

                                        plaaa.SendGump(new DialogueGump(plaaa, survivalModule));
                                    });

                                plaa.SendGump(new DialogueGump(plaa, battleModule));
                            });

                        pl.SendGump(new DialogueGump(pl, hauntedStoryModule));
                    });

                // Trade option after story
                mysteryModule.AddOption("Do you wish to make a trade?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a WaterFountain, I can offer you a choice between a FountainWall or a HildebrandtFlag. You may also receive a MaxxiaScroll as a token of my appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a WaterFountain for me?");
                                tradeModule.AddOption("Yes, I have a WaterFountain.", 
                                    plaa => HasWaterFountain(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasWaterFountain(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a WaterFountain.");
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

                p.SendGump(new DialogueGump(p, mysteryModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Aelric nods and turns away, lost in thought.");
            });

        return greeting;
    }

    private DialogueModule CreateTradeModule(PlayerMobile player)
    {
        DialogueModule tradeModule = new DialogueModule("If you are ready, I can offer you a choice of reward for the WaterFountain.");
        tradeModule.AddOption("Yes, let's proceed.", 
            pl => HasWaterFountain(pl) && CanTradeWithPlayer(pl), 
            pl => CompleteTrade(pl));
        tradeModule.AddOption("Not right now.", 
            pl => true, 
            pl => pl.SendMessage("Very well, come back when you are ready."));
        return tradeModule;
    }

    private bool HasWaterFountain(PlayerMobile player)
    {
        // Check the player's inventory for WaterFountain
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WaterFountain)) != null;
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
        // Remove the WaterFountain and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item waterFountain = player.Backpack.FindItemByType(typeof(WaterFountain));
        if (waterFountain != null)
        {
            waterFountain.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for FountainWall and HildebrandtFlag
            rewardChoiceModule.AddOption("FountainWall", pl => true, pl =>
            {
                pl.AddToBackpack(new FountainWall());
                pl.SendMessage("You receive a FountainWall!");
            });

            rewardChoiceModule.AddOption("HildebrandtFlag", pl => true, pl =>
            {
                pl.AddToBackpack(new HildebrandtFlag());
                pl.SendMessage("You receive a HildebrandtFlag!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a WaterFountain.");
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