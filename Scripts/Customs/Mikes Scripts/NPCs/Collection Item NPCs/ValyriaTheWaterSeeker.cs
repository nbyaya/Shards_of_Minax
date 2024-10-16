using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ValyriaTheWaterSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ValyriaTheWaterSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Valyria the Water Seeker";
        Body = 0x191; // Human female body
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
        AddItem(new HoodedShroudOfShadows(1153)); // Dark blue shroud representing her affinity to water
        AddItem(new Sandals(136)); // Light blue sandals
        AddItem(new FancyShirt(1266)); // Azure blue fancy shirt
        AddItem(new Skirt(1287)); // Deep sea-green skirt
        AddItem(new GoldBracelet()); // Adds a touch of mystery

        VirtualArmor = 15;
    }

    public ValyriaTheWaterSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Valyria, seeker of the rarest waters. Do you perhaps carry the WaterRelic, an artifact I have been longing to study?");

        // Dialogue options
        greeting.AddOption("Tell me about the WaterRelic.",
            p => true,
            p =>
            {
                DialogueModule relicInfoModule = new DialogueModule("The WaterRelic is said to be blessed by the ancient spirits of the ocean. I believe that those who carry it have a special connection to the waters. If you happen to have one, I would gladly offer you something in return.");

                relicInfoModule.AddOption("I have the WaterRelic. What will you offer in exchange?",
                    pl => HasWaterRelic(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                relicInfoModule.AddOption("I do not have the WaterRelic.",
                    pl => !HasWaterRelic(pl),
                    pl =>
                    {
                        pl.SendMessage("It seems you do not have the WaterRelic. Come back if you find it.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                relicInfoModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                relicInfoModule.AddOption("Tell me more about yourself, Valyria.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule valyriaBackstory = new DialogueModule("Ah, well, if you insist. I wasn't always Valyria the Water Seeker. I used to have another name, another life. I was once known as Valyria the Radiant, a gambler who lived by the whims of chance. My old life was filled with games of risk, dice rolling, and betting my fortunes.");
                        valyriaBackstory.AddOption("A gambler? That sounds dangerous.",
                            pll => true,
                            pll =>
                            {
                                DialogueModule gamblingStory = new DialogueModule("Dangerous? Ha! Life is danger, my friend. I thrived on it. I gambled for everything - my food, my shelter, even my companions. There was something thrilling in the unknown, in having everything or nothing, all on the roll of a die.");

                                gamblingStory.AddOption("Did you ever lose something valuable?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule lossStory = new DialogueModule("Oh, many things, dear traveler. Once, I lost my entire fortune in a single night. Another time, I lost a companion - someone who was very dear to me. The stakes were always high, and with each win or loss, I learned more about who I was.");

                                        lossStory.AddOption("That sounds heartbreaking. Why keep gambling?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule heartbreakStory = new DialogueModule("Heartbreaking? Yes. But there was also beauty in the risk. You see, when you live on the edge, every moment matters. Every breath, every decision is amplified, vivid. That is why I kept gambling - not for the riches, but for the feeling of being truly alive.");

                                                heartbreakStory.AddOption("And what brought you to seek the WaterRelic?",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        DialogueModule relicTransition = new DialogueModule("Ah, now we come to it. After losing everything one final time, I found myself wandering. I was empty, both in pocket and spirit. It was then I stumbled upon a tale of the WaterRelic, an artifact that could quench even the deepest thirst, not just of the body, but of the soul. I knew that finding it would bring me the solace I never had.");

                                                        relicTransition.AddOption("Did you succeed?", 
                                                            pllllll => true, 
                                                            pllllll => 
                                                            {
                                                                DialogueModule successStory = new DialogueModule("Success is a relative term. I have found many waters, rare and mystical, each with its own story. But the true WaterRelic remains elusive. It’s out there somewhere, and I believe it will grant me the peace I’ve sought for so long. But for now, I must rely on the generosity of travelers like you to bring me closer to my goal.");

                                                                successStory.AddOption("I admire your perseverance. Let me see if I can help.",
                                                                    pmmmm => true,
                                                                    pmmmm =>
                                                                    {
                                                                        pmmmm.SendGump(new DialogueGump(pmmmm, CreateGreetingModule(pmmmm)));
                                                                    });

                                                                successStory.AddOption("Good luck on your quest, Valyria.",
                                                                    pmmmm => true,
                                                                    pmmmm =>
                                                                    {
                                                                        pmmmm.SendMessage("Valyria nods, a wistful look in her eyes. 'Thank you, traveler. I will continue my search.'");
                                                                    });

                                                                pllllll.SendGump(new DialogueGump(pllllll, successStory));
                                                            });

                                                        plllll.SendGump(new DialogueGump(plllll, relicTransition));
                                                    });

                                                pllll.SendGump(new DialogueGump(pllll, heartbreakStory));
                                            });

                                        plll.SendGump(new DialogueGump(plll, lossStory));
                                    });

                                pll.SendGump(new DialogueGump(pll, gamblingStory));
                            });

                        pl.SendGump(new DialogueGump(pl, valyriaBackstory));
                    });

                p.SendGump(new DialogueGump(p, relicInfoModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Valyria nods, her eyes glimmering like the ocean.");
            });

        return greeting;
    }

    private bool HasWaterRelic(PlayerMobile player)
    {
        // Check the player's inventory for WaterRelic
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WaterRelic)) != null;
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
        // Remove the WaterRelic and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item waterRelic = player.Backpack.FindItemByType(typeof(WaterRelic));
        if (waterRelic != null)
        {
            waterRelic.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Thank you for the WaterRelic. Please, choose your reward.");

            // Add options for DailyNewspaper and SkullIncense
            rewardChoiceModule.AddOption("DailyNewspaper", pl => true, pl =>
            {
                pl.AddToBackpack(new DailyNewspaper());
                pl.SendMessage("You receive a DailyNewspaper!");
            });

            rewardChoiceModule.AddOption("SkullIncense", pl => true, pl =>
            {
                pl.AddToBackpack(new SkullIncense());
                pl.SendMessage("You receive a SkullIncense!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have the WaterRelic.");
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