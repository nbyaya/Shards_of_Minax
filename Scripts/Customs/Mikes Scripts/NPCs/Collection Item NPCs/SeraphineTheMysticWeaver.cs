using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SeraphineTheMysticWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SeraphineTheMysticWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Seraphine the Mystic Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(90);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1150)); // A mystical dark shroud
        AddItem(new Sandals(1153)); // Enchanted blue sandals
        AddItem(new Robe(2053)); // Flowing azure robe
        AddItem(new GoldBracelet()); // A bracelet with faintly glowing runes
        AddItem(new QuarterStaff()); // Decorative staff she carries

        VirtualArmor = 15;
    }

    public SeraphineTheMysticWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Seraphine, a weaver of mystical threads. Do you seek the wisdom of the loom, or perhaps a trade of enchantments?");

        // Dialogue options
        greeting.AddOption("Tell me more about your weaving.",
            p => true,
            p =>
            {
                DialogueModule weavingModule = new DialogueModule("My loom is no ordinary one; it weaves the strands of magic itself. I craft tapestries that can alter fate, if only I have the right materials. Are you perhaps in possession of a WaterWell item? I could offer something of value in exchange.");

                // Additional nested dialogue options for weavingModule
                weavingModule.AddOption("Why do you live out here, away from others?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("I once belonged to a noble house, a family of wealth and influence. But they disowned me, cast me aside like a piece of trash when I delved into arts they deemed improper. I chose the path of mystical weaving, and for that, I paid dearly.");

                        backstoryModule.AddOption("Why did they disown you?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule disownModule = new DialogueModule("Prideful fools, they were. My family believed only in wealth and power derived from mundane means. When I began to weave spells into fabric, creating garments that could alter destiny itself, they grew afraid. They accused me of witchcraft, of delving into forces best left untouched. They feared what they did not understand.");

                                disownModule.AddOption("Do you regret your choices?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? Perhaps at times, I feel the sting of loss. The warmth of a family, the luxury of noble life. But I have gained something far greater: power beyond their comprehension. I have learned secrets that only the most dedicated may glimpse. The bitter taste of exile is a small price to pay for the ability to weave fate itself.");

                                        regretModule.AddOption("Do you plan to take revenge on your family?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule revengeModule = new DialogueModule("Revenge? Ah, yes, it has crossed my mind many times. To see their proud faces crumble, to unravel the threads of their precious influence. Yet, revenge is a dangerous thread to weave, one that can easily ensnare the weaver. I bide my time, waiting for the right moment, if it ever comes.");

                                                revengeModule.AddOption("Maybe there's another path, one of reconciliation?",
                                                    plaac => true,
                                                    plaac =>
                                                    {
                                                        DialogueModule reconciliationModule = new DialogueModule("Reconciliation? You are either naive or kind-hearted, traveler. They would never accept me back, not after what I have become. And I... I am no longer the person they once knew. Perhaps I have grown too proud, too bitter to ever forgive. Still, it is a thought I entertain on lonely nights.");

                                                        reconciliationModule.AddOption("I believe people can change.",
                                                            plaad => true,
                                                            plaad =>
                                                            {
                                                                plaad.SendMessage("Seraphine gazes at you, her eyes softening for a brief moment before the cold mask returns.");
                                                                plaad.SendGump(new DialogueGump(plaad, CreateGreetingModule(plaad)));
                                                            });
                                                        reconciliationModule.AddOption("I understand, sometimes wounds run too deep.",
                                                            plaad => true,
                                                            plaad =>
                                                            {
                                                                plaad.SendMessage("Seraphine nods solemnly, her expression turning distant.");
                                                                plaad.SendGump(new DialogueGump(plaad, CreateGreetingModule(plaad)));
                                                            });
                                                    });

                                                revengeModule.AddOption("Revenge can be sweet, but it often costs more than it gives.",
                                                    plaac => true,
                                                    plaac =>
                                                    {
                                                        plaac.SendMessage("Seraphine's lips curl into a faint smile. 'Wise words, traveler. Perhaps there is more to you than meets the eye.'");
                                                        plaac.SendGump(new DialogueGump(plaac, CreateGreetingModule(plaac)));
                                                    });
                                            });
                                        
                                        regretModule.AddOption("No need to dwell on the past, tell me more about your craft.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, weavingModule));
                                            });
                                    });

                                disownModule.AddOption("They were wrong to fear you. Knowledge should be pursued.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Seraphine smiles, a glimmer of warmth in her otherwise cold demeanor. 'You understand, then. Knowledge is the truest power.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                            });

                        backstoryModule.AddOption("That sounds difficult. How have you managed on your own?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule surviveModule = new DialogueModule("It has not been easy, but I have grown accustomed to solitude. The wilderness is harsh, but it is also freeing. I am no longer bound by the expectations of society. I am free to weave my magic, to experiment and grow in ways they could never imagine.");

                                surviveModule.AddOption("You must be quite powerful now.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Seraphine's eyes gleam with pride. 'Indeed. My power has grown, and I intend for it to grow further still.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });

                                surviveModule.AddOption("It sounds lonely.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Seraphine sighs, her eyes briefly softening. 'Loneliness is the price of freedom, traveler. It is a burden I have learned to bear.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                // Trade option after story
                weavingModule.AddOption("What do you offer in exchange?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a WaterWell, I can offer you either a RibbonAward or a RookStone, along with a MaxxiaScroll as a token of appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a WaterWell for me?");
                                tradeModule.AddOption("Yes, I have a WaterWell.",
                                    plaa => HasWaterWell(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasWaterWell(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a WaterWell.");
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

                p.SendGump(new DialogueGump(p, weavingModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Seraphine nods, her eyes reflecting a glimmer of hidden knowledge.");
            });

        return greeting;
    }

    private bool HasWaterWell(PlayerMobile player)
    {
        // Check the player's inventory for WaterWell
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WaterWell)) != null;
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
        // Remove the WaterWell and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item waterWell = player.Backpack.FindItemByType(typeof(WaterWell));
        if (waterWell != null)
        {
            waterWell.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for RibbonAward and RookStone
            rewardChoiceModule.AddOption("RibbonAward", pl => true, pl =>
            {
                pl.AddToBackpack(new RibbonAward());
                pl.SendMessage("You receive a RibbonAward!");
            });

            rewardChoiceModule.AddOption("RookStone", pl => true, pl =>
            {
                pl.AddToBackpack(new RookStone());
                pl.SendMessage("You receive a RookStone!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a WaterWell.");
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