using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class NylaraTheFrostfisher : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NylaraTheFrostfisher() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Nylara the Frostfisher";
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
        AddItem(new Robe(1152)); // Icy blue robe
        AddItem(new ThighBoots(1175)); // Frosted leather boots
        AddItem(new FishingPole()); // Her fishing pole, always in hand
        AddItem(new FeatheredHat(1150)); // A light blue feathered hat

        VirtualArmor = 10;
    }

    public NylaraTheFrostfisher(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Nylara, a fisher of the frozen lakes. These cold waters hide wonders, but tell me, do you seek something unique amidst the frost?");

        // Dialogue options
        greeting.AddOption("What wonders do you speak of?", 
            p => true, 
            p =>
            {
                DialogueModule wondersModule = new DialogueModule("The lake's treasures are not just fish. I've found ancient trinkets and relics hidden below the ice. If you happen to carry a HolidayPillow, I might be persuaded to exchange something special with you.");
                
                wondersModule.AddOption("Tell me more about these trinkets.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule trinketsDetailModule = new DialogueModule("These trinkets are fragments of stories long lost. Sometimes, it's a warrior's token, sometimes a scholar's notes. I once found an old pendant, covered in blood... it reminded me of a different time, a time when my hands were stained with something other than the scales of fish.");

                        trinketsDetailModule.AddOption("That sounds grim, what happened?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule traumaDetailModule = new DialogueModule("I was not always a simple fisher. There was a time when I fought in the underground pits. Brutal matches... battles where we fought not for honor, but survival. I was drawn to it, haunted by something I could never escape, and perhaps I sought redemption through pain, through blood.");

                                traumaDetailModule.AddOption("Why did you fight?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule fightReasonModule = new DialogueModule("I lost someone. Someone who mattered more to me than anything else. The pain of losing them became unbearable, and I thought I could bury it beneath violence. Every opponent I faced was a ghost of my past, and each strike was meant to silence the agony inside me.");

                                        fightReasonModule.AddOption("Did it help?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule redemptionModule = new DialogueModule("No. The faces of those I fought, their screams, they still haunt me. The violence never silenced the pain, it only added more ghosts to my mind. Now, I fish these frozen lakes to find peace, but peace is elusive. Every ripple in the water reminds me of the ripples of blood I caused.");

                                                redemptionModule.AddOption("Do you still fight?",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        DialogueModule fightAgainModule = new DialogueModule("Sometimes, I am tempted. The rage still lives within me, clawing to get out. I fight it every day, and instead, I try to put my energy into helping others. Perhaps, by giving what I find in these lakes, I can make some small difference, even if I can never forgive myself.");

                                                        fightAgainModule.AddOption("I have a HolidayPillow, can we trade?",
                                                            plaabcd => CanTradeWithPlayer(plaabcd),
                                                            plaabcd =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Do you have a HolidayPillow for me?");
                                                                tradeModule.AddOption("Yes, I have a HolidayPillow.",
                                                                    plaaq => HasHolidayPillow(plaa) && CanTradeWithPlayer(plaa),
                                                                    plaaq =>
                                                                    {
                                                                        CompleteTrade(plaa);
                                                                    });
                                                                tradeModule.AddOption("No, I don't have one right now.",
                                                                    plaaw => !HasHolidayPillow(plaa),
                                                                    plaaw =>
                                                                    {
                                                                        plaa.SendMessage("Come back when you have a HolidayPillow.");
                                                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                    });
                                                                tradeModule.AddOption("I traded recently; I'll come back later.",
                                                                    plaae => !CanTradeWithPlayer(plaa),
                                                                    plaae =>
                                                                    {
                                                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                    });
                                                                plaabcd.SendGump(new DialogueGump(plaabcd, tradeModule));
                                                            });

                                                        fightAgainModule.AddOption("Goodbye.",
                                                            plaabcd => true,
                                                            plaabcd =>
                                                            {
                                                                plaabcd.SendMessage("Nylara nods solemnly, her eyes heavy with unspoken sorrow.");
                                                            });

                                                        plaabc.SendGump(new DialogueGump(plaabc, fightAgainModule));
                                                    });

                                                plaab.SendGump(new DialogueGump(plaab, redemptionModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, fightReasonModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, traumaDetailModule));
                            });

                        pl.SendGump(new DialogueGump(pl, trinketsDetailModule));
                    });

                // Trade option after story
                wondersModule.AddOption("Do you wish to trade for a HolidayPillow?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a HolidayPillow, I will offer you a choice between an ExoticFish or a LargeWeatheredBook. You can only trade once every 10 minutes, mind you.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a HolidayPillow for me?");
                                tradeModule.AddOption("Yes, I have a HolidayPillow.", 
                                    plaa => HasHolidayPillow(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasHolidayPillow(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a HolidayPillow.");
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

                p.SendGump(new DialogueGump(p, wondersModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Nylara smiles and nods, her eyes twinkling like the ice.");
            });

        return greeting;
    }

    private bool HasHolidayPillow(PlayerMobile player)
    {
        // Check the player's inventory for HolidayPillow
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HolidayPillow)) != null;
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
        // Remove the HolidayPillow and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item holidayPillow = player.Backpack.FindItemByType(typeof(HolidayPillow));
        if (holidayPillow != null)
        {
            holidayPillow.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ExoticFish and LargeWeatheredBook
            rewardChoiceModule.AddOption("ExoticFish", pl => true, pl =>
            {
                pl.AddToBackpack(new ExoticFish());
                pl.SendMessage("You receive an ExoticFish!");
            });

            rewardChoiceModule.AddOption("LargeWeatheredBook", pl => true, pl =>
            {
                pl.AddToBackpack(new LargeWeatheredBook());
                pl.SendMessage("You receive a LargeWeatheredBook!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a HolidayPillow.");
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