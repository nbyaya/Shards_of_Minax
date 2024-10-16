using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class RanzorTheAlchemicalTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RanzorTheAlchemicalTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ranzor the Alchemical Tinkerer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1359)); // A vibrant green robe
        AddItem(new Sandals(1175)); // Bright yellow sandals
        AddItem(new WizardsHat(1153)); // A mystical blue wizard hat
        AddItem(new LeatherGloves()); // Simple leather gloves
        AddItem(new Lantern() { Movable = false }); // A lantern, not meant to be removed

        VirtualArmor = 15;
    }

    public RanzorTheAlchemicalTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Ranzor, an alchemist of peculiar interests. Tell me, do you dabble in the arcane or the scientific?");

        // Dialogue options
        greeting.AddOption("Tell me more about your experiments.",
            p => true,
            p =>
            {
                DialogueModule experimentsModule = new DialogueModule("I concoct elixirs, distill essences, and tinker with the very fabric of reality! My journeys have taken me to dark and distant lands, where strange energies flow and eldritch artifacts lie buried. But alas, I am in constant need of rare ingredients. Perhaps you have something I need?");

                // Nested dialogue about travels
                experimentsModule.AddOption("What kind of places have you traveled to?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule travelsModule = new DialogueModule("Ah, the places I have seen... I have wandered through the Abyssal Wastes, where shadows have a mind of their own, and the Whispering Mire, where the fog speaks secrets best left forgotten. I have seen grand cities swallowed by the earth and endless deserts that drink up the sun. Each journey brought me closer to understanding the thin veil between what we call reality and the unknown.");

                        travelsModule.AddOption("That sounds dangerous. Why do you keep traveling?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule motivationModule = new DialogueModule("Dangerous? Oh, very much so. But danger is part of the thrill, is it not? The unknown calls to me, like a moth to a flame. There is a reckless part of me, I suppose, that cannot help but seek what others fear. But perhaps it is also because I know that in those dark places lie answers—answers to questions that haunt even the bravest of souls.");

                                motivationModule.AddOption("What kind of questions?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule questionsModule = new DialogueModule("Questions of existence, dear traveler. Questions of life, death, and the spaces in between. Have you ever wondered why the stars seem to flicker when you gaze at them too long? Or why some dreams leave a mark on your soul long after you have woken? I seek to uncover the hidden truths that bind our world together, even if those truths may shatter my own understanding.");

                                        questionsModule.AddOption("You must have found some interesting artifacts in your travels.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule artifactsModule = new DialogueModule("Indeed, I have brought back many relics from my journeys. Some of them resonate with an energy that is not of this world. I have a shard of the Nexus Crystal, for example—a fragment of a stone that pulses with an eerie light, as though it were alive. And then there is the Singing Mask, a faceplate that hums a tune that no one else seems to hear. Each artifact carries a story, and each story carries a hint of the beyond.");

                                                artifactsModule.AddOption("Can I see any of these artifacts?",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        DialogueModule showArtifactsModule = new DialogueModule("Ah, but these artifacts are not merely for show, my friend. They are dangerous in untrained hands. But if you were to help me gather some components, perhaps I could let you glimpse one of them. You see, alchemy is not just about knowledge—it is about trust, about forging a bond with the unknown.");

                                                        showArtifactsModule.AddOption("What components do you need?",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                DialogueModule componentsModule = new DialogueModule("Right now, I am in need of a GalvanizedTub. Should you have one, I could reward you handsomely. You may even choose between a FixedScales or a NexusShard as a token of my appreciation.");

                                                                componentsModule.AddOption("I'd like to make the trade.",
                                                                    plaaaaa => CanTradeWithPlayer(plaaaaa),
                                                                    plaaaaa =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a GalvanizedTub for me?");
                                                                        tradeModule.AddOption("Yes, I have a GalvanizedTub.",
                                                                            plaaaaaa => HasGalvanizedTub(plaaaaaa) && CanTradeWithPlayer(plaaaaaa),
                                                                            plaaaaaa =>
                                                                            {
                                                                                CompleteTrade(plaaaaaa);
                                                                            });
                                                                        tradeModule.AddOption("No, I don't have one right now.",
                                                                            plaaaaaa => !HasGalvanizedTub(plaaaaaa),
                                                                            plaaaaaa =>
                                                                            {
                                                                                plaaaaaa.SendMessage("Come back when you have a GalvanizedTub.");
                                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                            });
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                                                            plaaaaaa => !CanTradeWithPlayer(plaaaaaa),
                                                                            plaaaaaa =>
                                                                            {
                                                                                plaaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                            });
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, tradeModule));
                                                                    });
                                                                componentsModule.AddOption("Maybe another time.",
                                                                    plaaaaa => true,
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, componentsModule));
                                                            });
                                                        plaabc.SendGump(new DialogueGump(plaabc, showArtifactsModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, artifactsModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, questionsModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, motivationModule));
                            });
                        pl.SendGump(new DialogueGump(pl, travelsModule));
                    });

                // Trade option after story
                experimentsModule.AddOption("Do you need any specific ingredients?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! I am currently seeking a GalvanizedTub. Should you have one, I would be willing to offer you a choice between a FixedScales or a NexusShard.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a GalvanizedTub for me?");
                                tradeModule.AddOption("Yes, I have a GalvanizedTub.",
                                    plaa => HasGalvanizedTub(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasGalvanizedTub(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a GalvanizedTub.");
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

                p.SendGump(new DialogueGump(p, experimentsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Ranzor nods, his eyes glinting with curiosity.");
            });

        return greeting;
    }

    private bool HasGalvanizedTub(PlayerMobile player)
    {
        // Check the player's inventory for GalvanizedTub
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GalvanizedTub)) != null;
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
        // Remove the GalvanizedTub and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item galvanizedTub = player.Backpack.FindItemByType(typeof(GalvanizedTub));
        if (galvanizedTub != null)
        {
            galvanizedTub.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for FixedScales and NexusShard
            rewardChoiceModule.AddOption("FixedScales", pl => true, pl =>
            {
                pl.AddToBackpack(new FixedScales());
                pl.SendMessage("You receive FixedScales!");
            });

            rewardChoiceModule.AddOption("NexusShard", pl => true, pl =>
            {
                pl.AddToBackpack(new NexusShard());
                pl.SendMessage("You receive a NexusShard!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a GalvanizedTub.");
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