using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SyraTheFletcher : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SyraTheFletcher() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Syra the Fletcher";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new LeatherChest(1109)); // Dark leather armor
        AddItem(new LeatherLegs(1109));  // Matching leather pants
        AddItem(new LeatherArms(1109));
        AddItem(new Boots(1175)); // Light brown boots
        AddItem(new FeatheredHat(1)); // A feathered hat for a touch of flair
        AddItem(new Bow()); // Carrying a bow as a fletcher would
        AddItem(new FletchingTalisman()); // Adding fletching tools to her inventory

        VirtualArmor = 20;
    }

    public SyraTheFletcher(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a fellow traveler! I am Syra, leader of a band of scavengers, and a master fletcher. We fight to survive against raiders, mutants, and anyone else who threatens our camp. What brings you to me today?");

        // Dialogue about her work
        greeting.AddOption("Tell me about your craft.",
            p => true,
            p =>
            {
                DialogueModule craftModule = new DialogueModule("Fletching is an art, you see. Each arrow, each bow, must be crafted with care and precision. The right wood, the sharpest flint, and a steady hand make all the difference between hitting your target and missing completely. But in our world, it's more than just craft—it's survival.");
                craftModule.AddOption("What materials do you use?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule materialsModule = new DialogueModule("I use yew wood for the bows, as it is both flexible and strong. For arrows, I prefer goose feathers for fletching, as they provide the truest flight. Of course, a good flint or even obsidian for the arrowhead can make a real impact, quite literally! The choice of material often means the difference between life and death.");
                        materialsModule.AddOption("Do you have any stories from scavenging for these materials?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule storyModule = new DialogueModule("Ah, yes, many stories. Once, we ventured into the ruins of an old town seeking yew wood. We were ambushed by raiders, but my people and I fought them off. It wasn't easy—my bowstring snapped in the middle of the fight, and I had to fend them off with a dagger until I could restring it. My people look to me for strength, and I can't afford to fail them.");
                                storyModule.AddOption("That sounds intense. How do you keep your people motivated?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule motivationModule = new DialogueModule("I remind them that we fight not just for survival, but for each other. The world is harsh, but we have each other, and that makes us strong. We may not have much, but every arrow I craft, every bow I string, I do it to protect them. It's my way of showing that I will always have their backs, and that keeps them going.");
                                        motivationModule.AddOption("You're truly an inspiration. Thank you for sharing.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, motivationModule));
                                    });
                                storyModule.AddOption("That's quite the story. Thanks for sharing.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, storyModule));
                            });
                        materialsModule.AddOption("That sounds fascinating, thank you.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, materialsModule));
                    });
                craftModule.AddOption("Interesting, but I have other questions.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, craftModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you need any help with materials?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Actually, yes. I have been searching for a ClutteredDesk item for my workshop. In return, I can offer you a FletchingTalisman and a MaxxiaScroll. Are you interested in making a trade? Just be aware, raiders have made it dangerous to move around with such items.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a ClutteredDesk for me?");
                        tradeModule.AddOption("Yes, I have a ClutteredDesk.",
                            plaa => HasClutteredDesk(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasClutteredDesk(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a ClutteredDesk.");
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
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Introduce her backstory
        greeting.AddOption("Tell me about your group of scavengers.",
            p => true,
            p =>
            {
                DialogueModule scavengerModule = new DialogueModule("We call ourselves the Ashen Wolves. We're a tight-knit group, and we look out for each other. Raiders, mutants, and even wild beasts try to make life hard for us, but we endure. We scavenge for supplies, defend our camp, and make sure we all have enough to eat. I've taken it upon myself to teach my people how to fight and how to craft what we need.");
                scavengerModule.AddOption("How did you become their leader?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule leaderModule = new DialogueModule("It wasn't a choice I made, really. It was more of a necessity. The old leader was killed in a raid, and someone had to step up. I couldn't let my people fall apart, so I did what needed to be done. I taught myself to fight, and I taught them too. Now, we're stronger than we ever were before, and we won't be easy prey again.");
                        leaderModule.AddOption("That's admirable. What challenges do you face now?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule challengeModule = new DialogueModule("The biggest challenge is keeping everyone safe and fed. Raiders are always a threat, but lately, we've also had to deal with mutants encroaching on our territory. And then there's the scarcity of resources—food, medicine, supplies—every day is a struggle. But we face it together, and as long as I draw breath, I'll protect my people.");
                                challengeModule.AddOption("Thank you for sharing. You truly are a protector.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, challengeModule));
                            });
                        leaderModule.AddOption("You're an inspiration, Syra.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, leaderModule));
                    });
                scavengerModule.AddOption("It must be hard to keep everyone safe.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hardModule = new DialogueModule("It is. But these people are my family now. We share everything—our successes, our losses. I can be tough when I need to be, but I never forget that we are all in this together. I push them because I love them, and I want to see them survive.");
                        hardModule.AddOption("I respect that. Thank you for everything you do.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, hardModule));
                    });
                p.SendGump(new DialogueGump(p, scavengerModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Syra nods, her expression softening for a moment. 'Stay safe out there,' she says as she turns back to her work.");
            });

        return greeting;
    }

    private bool HasClutteredDesk(PlayerMobile player)
    {
        // Check the player's inventory for ClutteredDesk
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ClutteredDesk)) != null;
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
        // Remove the ClutteredDesk and give the FletchingTalisman and MaxxiaScroll, then set the cooldown timer
        Item clutteredDesk = player.Backpack.FindItemByType(typeof(ClutteredDesk));
        if (clutteredDesk != null)
        {
            clutteredDesk.Delete();
            player.AddToBackpack(new FletchingTalisman());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ClutteredDesk and receive a FletchingTalisman and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a ClutteredDesk.");
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