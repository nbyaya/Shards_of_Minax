using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ElowenTheEnigmatic : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ElowenTheEnigmatic() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elowen the Enigmatic";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1109)); // A mysterious dark shroud
        AddItem(new Sandals(1150)); // Crimson sandals
        AddItem(new Skirt(1153)); // Deep purple skirt
        AddItem(new WitchesCauldron()); // A beaded necklace for an enigmatic look

        VirtualArmor = 15;
    }

    public ElowenTheEnigmatic(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Elowen, keeper of forgotten relics and tales untold. Are you seeking an exchange of mysterious nature?");

        // Dialogue options
        greeting.AddOption("Tell me more about yourself.",
            p => true,
            p =>
            {
                DialogueModule aboutModule = new DialogueModule("I wander the lands, collecting stories and items that speak to the heart. My favorite treasures are those given with affection. Do you have such an item?");

                aboutModule.AddOption("What kind of item do you seek?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a ValentineTeddybear, I would be willing to offer you something of equal charm. You may choose between a WitchesCauldron or a FireRelic.");

                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a ValentineTeddybear for me?");
                                tradeModule.AddOption("Yes, I have a ValentineTeddybear.",
                                    plaa => HasValentineTeddybear(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasValentineTeddybear(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a ValentineTeddybear.");
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

                aboutModule.AddOption("Why do you collect such items?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule motivationModule = new DialogueModule("Ah, it is a rather profitable hobby, you see. People cherish sentimental items, and where there is sentiment, there is value. Not everyone understands the true worth, but I do.");

                        motivationModule.AddOption("Are you interested in anything else?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moreInterestModule = new DialogueModule("Well, well, you seem curious. I sometimes deal in... shall we say, special requests. Certain individuals are willing to pay handsomely for the right relic. A word of advice: not all who seek my services are of noble intent.");

                                moreInterestModule.AddOption("What do you mean by 'special requests'?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule specialRequestsModule = new DialogueModule("Dark relics, powerful charms, things that some would say should remain buried. Those cultists, for instance, they are always eager for artifacts with dark potential. If you ever find something peculiar, bring it to me, and we may both profit.");

                                        specialRequestsModule.AddOption("Cultists? Are you involved with them?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule cultistInvolvementModule = new DialogueModule("Involved is a strong word, traveler. I simply provide... services. They have their needs, and I have mine. As long as they pay and keep their rituals out of my establishment, I am content.");

                                                cultistInvolvementModule.AddOption("Sounds dangerous. Aren't you afraid?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule dangerModule = new DialogueModule("Fear, my friend, is a luxury for those who have nothing to gain. I am careful, and charm can be more valuable than a sword. Besides, as long as guests pay their dues, they are safe... mostly.");

                                                        dangerModule.AddOption("Mostly? What happens to those who don't?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule fateModule = new DialogueModule("Let's just say, those who fail to pay may find themselves taking a rather long trip to the cellar. There are those who appreciate a willing guest, but they rarely ask too many questions if one arrives unwilling.");

                                                                fateModule.AddOption("That's horrible!",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Elowen gives a sly smile, her eyes revealing no remorse.");
                                                                    });
                                                                fateModule.AddOption("I see...", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Elowen nods knowingly, as if expecting that very response.");
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, fateModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, dangerModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, cultistInvolvementModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, specialRequestsModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, moreInterestModule));
                            });
                        pl.SendGump(new DialogueGump(pl, motivationModule));
                    });

                p.SendGump(new DialogueGump(p, aboutModule));
            });

        greeting.AddOption("I need a room for the night.",
            p => true,
            p =>
            {
                DialogueModule roomModule = new DialogueModule("Ah, looking for a place to rest your weary head, are you? The rates are... reasonable. But I must warn you, not all who stay here find it quite as comfortable as they expect.");

                roomModule.AddOption("What do you mean by that?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule warningModule = new DialogueModule("Oh, nothing you need to worry about. Just be sure to lock your door. There have been... strange occurrences, you see. Guests disappearing, whispers in the dark. But surely that won't happen to you, as long as you pay well.");

                        warningModule.AddOption("I'm not sure I want to stay here anymore.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Elowen chuckles softly, her smile never reaching her eyes. 'Oh, I'm sure you'll be just fine... if you change your mind, I'll be here.'");
                            });

                        warningModule.AddOption("I'll take the room anyway.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Elowen hands you a rusty key, her fingers lingering a moment too long. 'Room three, up the stairs. Sweet dreams, traveler.'");
                            });
                        pl.SendGump(new DialogueGump(pl, warningModule));
                    });
                p.SendGump(new DialogueGump(p, roomModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Elowen nods knowingly, her eyes twinkling with secrets untold.");
            });

        return greeting;
    }

    private bool HasValentineTeddybear(PlayerMobile player)
    {
        // Check the player's inventory for ValentineTeddybear
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ValentineTeddybear)) != null;
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
        // Remove the ValentineTeddybear and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item valentineTeddybear = player.Backpack.FindItemByType(typeof(ValentineTeddybear));
        if (valentineTeddybear != null)
        {
            valentineTeddybear.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for WitchesCauldron and FireRelic
            rewardChoiceModule.AddOption("WitchesCauldron", pl => true, pl =>
            {
                pl.AddToBackpack(new WitchesCauldron());
                pl.SendMessage("You receive a WitchesCauldron!");
            });

            rewardChoiceModule.AddOption("FireRelic", pl => true, pl =>
            {
                pl.AddToBackpack(new FireRelic());
                pl.SendMessage("You receive a FireRelic!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a ValentineTeddybear.");
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