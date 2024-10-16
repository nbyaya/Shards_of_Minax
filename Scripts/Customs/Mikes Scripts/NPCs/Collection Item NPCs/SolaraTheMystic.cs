using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SolaraTheMystic : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SolaraTheMystic() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Solara the Mystic";
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
        AddItem(new Robe(1153)); // Mystic blue robe
        AddItem(new Sandals(1150)); // Matching sandals
        AddItem(new WizardsHat(1153)); // Mystic blue wizard's hat
        AddItem(new GoldBracelet()); // A golden bracelet for some flair
        AddItem(new ShepherdsCrook()); // A staff to complete the mystical look

        VirtualArmor = 15;
    }

    public SolaraTheMystic(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Solara, a mystic who seeks knowledge of the arcane. Do you carry any artifacts of interest?");

        // Dialogue options
        greeting.AddOption("What kind of artifacts do you seek?",
            p => true,
            p =>
            {
                DialogueModule artifactModule = new DialogueModule("I seek the FancyMirror, an item of beauty and mystery. If you have one, I can offer you a choice: either an ExoticPlum or a pair of Shears in exchange, as well as a special scroll of knowledge.");

                artifactModule.AddOption("I have a FancyMirror. Can we trade?",
                    pl => HasFancyMirror(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                artifactModule.AddOption("I don't have one right now.",
                    pl => !HasFancyMirror(pl),
                    pl =>
                    {
                        pl.SendMessage("Return when you have the FancyMirror, and we shall trade.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                artifactModule.AddOption("I have traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You must wait 10 minutes between trades. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                // Nested options to create a mysterious atmosphere
                artifactModule.AddOption("Who are you really, Solara?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule mysteryModule = new DialogueModule("Ah, the question that lingers in the mind of many. I am a traveler of realms, a seeker of forgotten truths. Some call me a wanderer, others a harbinger. The FancyMirror is but a fragment of a tale long forgotten, a key to the past, perhaps... or to the future.");

                        mysteryModule.AddOption("What do you mean, a key to the past?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule pastModule = new DialogueModule("The FancyMirror once belonged to the Forgotten Court, a place lost in the mists of time. It is said that those who look into it can glimpse the faces of gods long fallen, and hear the whispers of their secrets. Such power is dangerous, and best left to those who understand its weight.");

                                pastModule.AddOption("Tell me more about the Forgotten Court.",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule courtModule = new DialogueModule("The Forgotten Court was a place of opulence and dread, ruled by beings who were not quite gods, but neither were they mortal. They dined on ambition and drank the tears of the hopeful. Their games were cruel, and their laughter echoing in the darkness could chill even the bravest soul.");

                                        courtModule.AddOption("Why would anyone want such an artifact?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule intentModule = new DialogueModule("Ambition blinds many. Some seek power, others knowledge, and others yet—escape. The FancyMirror is a beacon to those whose hearts are heavy with desire. Beware, traveler, for it may reveal more than you wish to know.");

                                                intentModule.AddOption("I see. Perhaps it is best left alone.",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        plllll.SendMessage("Solara nods, her eyes gleaming with a knowing sadness. 'Wisdom is the better part of valor, or so they say.'");
                                                    });
                                                pllll.SendGump(new DialogueGump(pllll, intentModule));
                                            });
                                        plll.SendGump(new DialogueGump(plll, courtModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, pastModule));
                            });

                        mysteryModule.AddOption("What do you mean, a key to the future?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule futureModule = new DialogueModule("The future is a delicate thing, ever-shifting and intangible. Those who peer into the FancyMirror might catch a glimpse of what lies ahead, but at a cost. Such knowledge can alter one's fate, unraveling possibilities like threads from a tapestry.");

                                futureModule.AddOption("Have you looked into the FancyMirror?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule lookedModule = new DialogueModule("I have... and I saw what I was meant to see. A shadow, a promise, and a warning. It led me here, to this very place, where I wait for those who are bold—or foolish—enough to seek their own destinies.");

                                        lookedModule.AddOption("A shadow, a promise, and a warning?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule shadowModule = new DialogueModule("The shadow is of what follows—of the past that cannot be outrun. The promise is one of power, but only for those willing to pay the price. And the warning... well, some things are best left unsaid. Are you willing to tread such a path, traveler?");

                                                shadowModule.AddOption("I think not. Some mysteries are too dangerous.",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        plllll.SendMessage("Solara smiles, a hint of sadness in her eyes. 'Wise words. Perhaps there is hope for you yet.'");
                                                    });
                                                shadowModule.AddOption("I am willing. Tell me more.",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        DialogueModule dangerModule = new DialogueModule("Then know this: power is never without its price. The Forgotten Court's laughter still echoes in the dark corners of this world, and those who seek their favor may find themselves bound by chains unseen.");
                                                        dangerModule.AddOption("I understand. I will tread carefully.",
                                                            pllllll => true,
                                                            pllllll =>
                                                            {
                                                                pllllll.SendMessage("Solara nods, her expression unreadable. 'May your journey bring you what you seek, and may you never forget the weight of your choices.'");
                                                            });
                                                        plllll.SendGump(new DialogueGump(plllll, dangerModule));
                                                    });
                                                pllll.SendGump(new DialogueGump(pllll, shadowModule));
                                            });
                                        plll.SendGump(new DialogueGump(plll, lookedModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, futureModule));
                            });

                        pl.SendGump(new DialogueGump(pl, mysteryModule));
                    });

                p.SendGump(new DialogueGump(p, artifactModule));
            });

        greeting.AddOption("Farewell.",
            p => true,
            p =>
            {
                p.SendMessage("Solara nods gently, her eyes gleaming with arcane wisdom.");
            });

        return greeting;
    }

    private bool HasFancyMirror(PlayerMobile player)
    {
        // Check the player's inventory for FancyMirror
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FancyMirror)) != null;
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
        // Remove the FancyMirror and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fancyMirror = player.Backpack.FindItemByType(typeof(FancyMirror));
        if (fancyMirror != null)
        {
            fancyMirror.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ExoticPlum and Shears
            rewardChoiceModule.AddOption("ExoticPlum", pl => true, pl =>
            {
                pl.AddToBackpack(new ExoticPlum());
                pl.SendMessage("You receive an ExoticPlum!");
            });

            rewardChoiceModule.AddOption("Shears", pl => true, pl =>
            {
                pl.AddToBackpack(new Shears());
                pl.SendMessage("You receive a pair of Shears!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a FancyMirror.");
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