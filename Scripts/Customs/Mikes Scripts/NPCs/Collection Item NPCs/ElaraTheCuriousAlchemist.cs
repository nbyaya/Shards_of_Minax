using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ElaraTheCuriousAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ElaraTheCuriousAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elara the Curious Alchemist";
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
        AddItem(new Robe(1153)); // A vibrant blue robe
        AddItem(new Sandals(141)); // Dark gray sandals
        AddItem(new GoldBracelet()); // A golden bracelet
        AddItem(new WizardsHat(1153)); // Matching blue wizard's hat

        VirtualArmor = 15;
    }

    public ElaraTheCuriousAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Elara, an alchemist always seeking rare ingredients. Might you have something intriguing for me today?");

        // Dialogue options
        greeting.AddOption("What kind of ingredients do you seek?",
            p => true,
            p =>
            {
                DialogueModule requestModule = new DialogueModule("I am in need of a RareOil, a most exquisite and elusive ingredient. Should you bring it to me, I will reward you handsomely. You may choose between a ValentineTeddybear or a CarvingMachine.");
                
                requestModule.AddOption("Why do you need RareOil?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule storyModule = new DialogueModule("RareOil is a key component in my research to create an elixir that may help my kind, the mutants. You see, we are often seen as different, even dangerous, but I believe we can find a place in this world with the right understanding and tools.");
                        
                        storyModule.AddOption("Mutants? Tell me more about your kind.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule mutantModule = new DialogueModule("Yes, I am a mutant. Born with abilities and traits that many fear, but also with potential that few understand. We face distrust, even hatred, but I fight to prove that we deserve a place in this world. It is why I am so committed to my work - I hope to show people that mutants are not to be feared but embraced.");

                                mutantModule.AddOption("That sounds very noble. How can I help?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Your willingness to help is heartening. Bringing me RareOil is a great start. It will aid me in my research to craft an elixir that may mitigate the harmful effects of our mutations and help us live peacefully amongst others.");
                                        helpModule.AddOption("I'll find the RareOil for you.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Thank you, traveler. Return to me when you have the RareOil, and I will reward you.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        helpModule.AddOption("I'm not sure if I can help right now.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("I understand. The world is not easy for those willing to stand with mutants. If you change your mind, I will be here.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, helpModule));
                                    });

                                mutantModule.AddOption("Why do people fear mutants so much?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule fearModule = new DialogueModule("Fear often stems from misunderstanding. Many see our powers as dangerous or unpredictable. Some mutants, in their desperation, have resorted to violence, which only fuels the prejudice against us. I strive to change that perception by showing the good we can bring.");
                                        fearModule.AddOption("I see. I hope your work succeeds.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Thank you for your kind words. Every bit of support makes a difference.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, fearModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, mutantModule));
                            });

                        storyModule.AddOption("Can I learn more about alchemy and its uses?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule alchemyModule = new DialogueModule("Alchemy is an ancient practice, combining the natural elements to create potions, elixirs, and sometimes even miracles. For me, alchemy is a way to understand the world and to use that knowledge to help others, especially my kind.");
                                alchemyModule.AddOption("It sounds fascinating. I would love to learn more in the future.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("You are welcome anytime, traveler. Perhaps one day, you will become an alchemist too.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, alchemyModule));
                            });

                        pl.SendGump(new DialogueGump(pl, storyModule));
                    });

                // Trade option after request
                requestModule.AddOption("I have a RareOil. Can we trade?",
                    pl => HasRareOil(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                requestModule.AddOption("I don't have the RareOil right now.",
                    pl => !HasRareOil(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a RareOil. I'll be waiting!");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                requestModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, requestModule));
            });

        greeting.AddOption("Why do you call yourself an alchemist?",
            p => true,
            p =>
            {
                DialogueModule alchemistStoryModule = new DialogueModule("I call myself an alchemist because I transform not only elements, but also hearts and minds. I aim to change perceptions about mutants. Alchemy is about transformation, and I embrace that fully - transforming fear into understanding, and ignorance into compassion.");
                alchemistStoryModule.AddOption("That is a wonderful philosophy.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Thank you, traveler. It is a long and challenging journey, but one worth taking.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, alchemistStoryModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Elara nods and waves you off with a smile.");
            });

        return greeting;
    }

    private bool HasRareOil(PlayerMobile player)
    {
        // Check the player's inventory for RareOil
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RareOil)) != null;
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
        // Remove the RareOil and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item rareOil = player.Backpack.FindItemByType(typeof(RareOil));
        if (rareOil != null)
        {
            rareOil.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ValentineTeddybear and CarvingMachine
            rewardChoiceModule.AddOption("ValentineTeddybear", pl => true, pl =>
            {
                pl.AddToBackpack(new ValentineTeddybear());
                pl.SendMessage("You receive a ValentineTeddybear!");
            });

            rewardChoiceModule.AddOption("CarvingMachine", pl => true, pl =>
            {
                pl.AddToBackpack(new CarvingMachine());
                pl.SendMessage("You receive a CarvingMachine!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a RareOil.");
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