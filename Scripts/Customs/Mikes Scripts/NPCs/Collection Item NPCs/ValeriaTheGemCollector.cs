using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ValeriaTheGemCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ValeriaTheGemCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Valeria the Gem Collector";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // A deep blue robe
        AddItem(new Sandals(1359)); // Purple sandals
        AddItem(new FeatheredHat(1157)); // A vibrant blue feathered hat
        AddItem(new GoldBracelet()); // A golden bracelet to show her love for gems
        AddItem(new HalfApron(2213)); // A gray apron for carrying small tools

        VirtualArmor = 15;
    }

    public ValeriaTheGemCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Valeria, a collector of rare gems and enchanted artifacts. Have you, by chance, stumbled upon anything peculiar during your adventures?");

        // Dialogue options
        greeting.AddOption("Tell me about your collection.",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I collect enchanted gems and rare artifacts from across the lands. These treasures hold stories, and I ensure they are used to benefit those who appreciate magic. I am particularly interested in something called an Enchanted Annealer.");

                // Nested dialogue
                collectionModule.AddOption("Why are you interested in these artifacts?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule interestModule = new DialogueModule("You see, I wasn't always a mere collector. I used to be a detective, one who chased down mysteries most folk wouldn't dare speak of. The Enchanted Annealer is one of those mysteries—a key to unlocking secrets that still haunt my nights.");

                        interestModule.AddOption("A detective? Tell me more.",
                            pll => true,
                            pll =>
                            {
                                DialogueModule detectiveStoryModule = new DialogueModule("Yes, a detective. I once worked cases that spanned strange lands and stranger people. My most harrowing case involved a series of bizarre murders, all linked to an ancient cult. They called themselves the Whispering Veil. Their practices... let's just say they had a penchant for the macabre.");

                                detectiveStoryModule.AddOption("What happened to the Whispering Veil?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule veilModule = new DialogueModule("I unraveled part of their network, but their influence ran deep. Too deep. I took down some of their leaders, but I was never able to eradicate them entirely. They knew things—secrets that defied logic and left me wondering if the world I knew was just a facade.");

                                        veilModule.AddOption("Do you think they are still out there?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule paranoiaModule = new DialogueModule("Yes. I know they are still out there. I can feel their eyes on me sometimes, lurking in the shadows. That's why I collect these artifacts. Each piece I find adds another layer of protection—a safeguard against those who would drag me back into that darkness.");

                                                paranoiaModule.AddOption("That sounds terrifying. How do you cope?",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        DialogueModule copingModule = new DialogueModule("Coping? Ha. I don't cope. I endure. The nights are long, and sleep is fleeting. The memories of those cases, the things I saw, they never truly leave. But I have learned to live with them, and I focus on what I can control—like these trades. Perhaps, with enough of these enchanted artifacts, I can finally find some peace.");
                                                        plllll.SendGump(new DialogueGump(plllll, copingModule));
                                                    });

                                                pllll.SendGump(new DialogueGump(pllll, paranoiaModule));
                                            });

                                        plll.SendGump(new DialogueGump(plll, veilModule));
                                    });

                                detectiveStoryModule.AddOption("That sounds dangerous. How did you survive?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("Barely. I survived through sheer shrewdness and a knack for reading people. Knowing when to push and when to disappear. The cult had eyes everywhere, and I had to become someone else entirely just to escape their grasp. I traded the badge for these robes and hoped they'd forget me.");
                                        plll.SendGump(new DialogueGump(plll, survivalModule));
                                    });

                                pll.SendGump(new DialogueGump(pll, detectiveStoryModule));
                            });

                        pl.SendGump(new DialogueGump(pl, interestModule));
                    });

                // Trade option after the story
                collectionModule.AddOption("I have an Enchanted Annealer. Can we make a trade?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Ah, the Enchanted Annealer! If you have one, I would gladly trade you for it. You can choose between a Glass of Bubbly or a Bird Statue, and I will always give you a Maxxia Scroll as well.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an Enchanted Annealer for me?");
                                tradeModule.AddOption("Yes, I have an Enchanted Annealer.",
                                    plaa => HasEnchantedAnnealer(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasEnchantedAnnealer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an Enchanted Annealer.");
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

                p.SendGump(new DialogueGump(p, collectionModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Valeria smiles and nods, her eyes still sparkling with curiosity, but a shadow of something else lingering beneath the surface.");
            });

        return greeting;
    }

    private bool HasEnchantedAnnealer(PlayerMobile player)
    {
        // Check the player's inventory for EnchantedAnnealer
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(EnchantedAnnealer)) != null;
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
        // Remove the EnchantedAnnealer and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item enchantedAnnealer = player.Backpack.FindItemByType(typeof(EnchantedAnnealer));
        if (enchantedAnnealer != null)
        {
            enchantedAnnealer.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for GlassOfBubbly and BirdStatue
            rewardChoiceModule.AddOption("Glass of Bubbly", pl => true, pl =>
            {
                pl.AddToBackpack(new GlassOfBubbly());
                pl.SendMessage("You receive a Glass of Bubbly!");
            });

            rewardChoiceModule.AddOption("Bird Statue", pl => true, pl =>
            {
                pl.AddToBackpack(new BirdStatue());
                pl.SendMessage("You receive a Bird Statue!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an Enchanted Annealer.");
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