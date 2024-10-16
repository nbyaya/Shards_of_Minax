using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class SilasTheWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SilasTheWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Silas the Weaver";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // A deep blue fancy shirt
        AddItem(new LongPants(2306)); // Dark green pants
        AddItem(new Sandals(1175)); // Light blue sandals
        AddItem(new FloppyHat(1165)); // A violet floppy hat
        AddItem(new HalfApron(2125)); // A golden-colored apron
        AddItem(new Backpack());

        VirtualArmor = 12;
    }

    public SilasTheWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Silas, a weaver of tales and tapestries. Do you seek a story, or perhaps something more tangible?");

        // Dialogue options
        greeting.AddOption("Tell me about your work.",
            p => true,
            p =>
            {
                DialogueModule storyModule = new DialogueModule("My tapestries tell the stories of this land: the battles, the love, the loss. They are woven with care, but I always require rare materials to create my finest pieces.");

                // Trade option after story
                storyModule.AddOption("Do you need any materials?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of something quite rare. If you have the HandOfFate, I would be willing to offer you a choice of my finest works: a NameTapestry or an OpalBranch.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have the HandOfFate for me?");
                                tradeModule.AddOption("Yes, I have the HandOfFate.",
                                    plaa => HasHandOfFate(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasHandOfFate(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have the HandOfFate.");
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

                storyModule.AddOption("What is your favorite tapestry?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule favoriteTapestryModule = new DialogueModule("Ah, my favorite? That would be 'The Midnight Feast'. It tells the story of a secret gathering beneath a full moon, where wishes are whispered into the night and dark forces conspire to grant them. Each stitch of the tapestry captures the desperation and hope of those who dare to dream. There is something mesmerizing about such moments, wouldn't you agree?");

                        favoriteTapestryModule.AddOption("Why is it called 'The Midnight Feast'?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule feastExplanationModule = new DialogueModule("They say, under the midnight sky, the hungry souls gather to feast on desires. The price of such desires is always steep. You see, the magic that weaves itself into these dreams is not of the light. Those who take part may find themselves eternally indebted to the darkness.");
                                feastExplanationModule.AddOption("That sounds quite dangerous...",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Dangerous, yes, but isn't that what makes it all the more enticing? The unknown, the risks, the thrill of seeing a wish granted. Many are drawn to it, though few are prepared for the consequences.");
                                        dangerModule.AddOption("I prefer to stay away from such risks.",
                                            plaac => true,
                                            plaac =>
                                            {
                                                plaac.SendMessage("A wise choice, traveler. The darkness is best observed from afar.");
                                            });
                                        dangerModule.AddOption("I think I understand the allure.",
                                            plaac => true,
                                            plaac =>
                                            {
                                                plaac.SendMessage("Ah, a kindred spirit. The world is full of mundane routines, but there is something exhilarating in knowing that there are still mysteries out there, waiting to be unraveled.");
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, dangerModule));
                                    });
                                feastExplanationModule.AddOption("I want to hear more about these wishes.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule wishesModule = new DialogueModule("The wishes granted under the midnight sky are powerful, but they carry a secret price. The magic used to weave these dreams is drawn from the darkness, and those who wish must relinquish something of value, something dear. Often, they do not realize the true cost until it is far too late.");
                                        wishesModule.AddOption("What do they lose?",
                                            plaac => true,
                                            plaac =>
                                            {
                                                plaac.SendMessage("Their freedom, their happiness, sometimes even their very soul. It's not a fair exchange, but such is the way of dark bargains.");
                                            });
                                        wishesModule.AddOption("I see... That's quite a high price.",
                                            plaac => true,
                                            plaac =>
                                            {
                                                plaac.SendMessage("Yes, and yet, people are willing to pay it. The promise of a wish fulfilled is a powerful lure.");
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, wishesModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, feastExplanationModule));
                            });

                        favoriteTapestryModule.AddOption("That's a beautiful story, thank you for sharing.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("You're very welcome, traveler. I hope it inspires you, in some way.");
                            });

                        pl.SendGump(new DialogueGump(pl, favoriteTapestryModule));
                    });

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Tell me more about yourself.",
            p => true,
            p =>
            {
                DialogueModule selfModule = new DialogueModule("I was once a baker, long ago. I crafted pastries that people said could grant wishes. Of course, nothing in this world is free. The secret ingredients I used had a power all their own, and those who tasted my pastries found themselves craving more and more, until they were willing to pay any price. I suppose you could say I became... obsessed with perfection, and with the magic I wove into my craft.");

                selfModule.AddOption("That sounds a little sinister...",
                    pl => true,
                    pl =>
                    {
                        DialogueModule sinisterModule = new DialogueModule("Sinister, perhaps. But is it not true that every craft has its secrets? The difference lies in how far one is willing to go to achieve perfection. I went further than most.");
                        sinisterModule.AddOption("And what happened to your bakery?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule bakeryModule = new DialogueModule("The bakery still stands, in a way. It is hidden, lost to most, but if you listen closely, you may hear whispers of its location. Some say those who find it can still taste the magic, though they are never quite the same after.");
                                bakeryModule.AddOption("I think I'd rather not seek it out.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("A wise decision. Not all places are meant to be found.");
                                    });
                                bakeryModule.AddOption("I am intrigued. Where can I find it?",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Perhaps, one day, the way will reveal itself to you. But remember, some doors are better left closed.");
                                    });
                                plaa.SendGump(new DialogueGump(plaa, bakeryModule));
                            });
                        sinisterModule.AddOption("I suppose every craft has its price.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Indeed. The question is, are you willing to pay it?");
                            });
                        pl.SendGump(new DialogueGump(pl, sinisterModule));
                    });

                selfModule.AddOption("It must have been difficult to leave that life behind.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Difficult, yes, but necessary. Sometimes, to weave a new story, one must unravel the old.");
                    });

                p.SendGump(new DialogueGump(p, selfModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Silas bows slightly, his hands brushing the edges of his apron.");
            });

        return greeting;
    }

    private bool HasHandOfFate(PlayerMobile player)
    {
        // Check the player's inventory for HandOfFate
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HandOfFate)) != null;
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
        // Remove the HandOfFate and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item handOfFate = player.Backpack.FindItemByType(typeof(HandOfFate));
        if (handOfFate != null)
        {
            handOfFate.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for NameTapestry and OpalBranch
            rewardChoiceModule.AddOption("NameTapestry", pl => true, pl =>
            {
                pl.AddToBackpack(new NameTapestry());
                pl.SendMessage("You receive a NameTapestry!");
            });

            rewardChoiceModule.AddOption("OpalBranch", pl => true, pl =>
            {
                pl.AddToBackpack(new OpalBranch());
                pl.SendMessage("You receive an OpalBranch!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have the HandOfFate.");
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