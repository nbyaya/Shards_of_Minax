using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SprocketTheTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SprocketTheTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sprocket the Tinkerer";
        Body = 0x190; // Human male body
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
        AddItem(new FancyShirt(2353)); // A fancy shirt with a bluish hue
        AddItem(new ShortPants(1125)); // Short pants with a reddish hue
        AddItem(new Sandals(1157)); // Sandals with a bright yellow hue
        AddItem(new ClockworkAssembly()); // Unique accessory item
        AddItem(new SpikedChair()); // Tool that can also be stolen

        VirtualArmor = 15;
    }

    public SprocketTheTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Sprocket, master of all things odd and mechanical. Do you fancy a trade, or perhaps a tale of my wondrous inventions?");

        // Dialogue options for backstory or trade
        greeting.AddOption("Tell me about your inventions.",
            p => true,
            p =>
            {
                DialogueModule inventionsModule = new DialogueModule("Oh, my inventions! I tinker with gears, cogs, and springs to create marvelous contraptions. Have you heard of the Wind-Up Rat, the Clockwork Bird, or my prized SpikedChair?");

                inventionsModule.AddOption("Tell me about the Wind-Up Rat.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule ratModule = new DialogueModule("The Wind-Up Rat is a clever little device that scurries around on its own, confusing foes and delighting children. It's powered by a tiny crystal core that must be wound every day.");
                        ratModule.AddOption("What inspired you to create it?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule inspirationModule = new DialogueModule("Ah, inspiration often strikes in strange ways. The Wind-Up Rat was inspired by a visit to the Gloomy Undertaker. He spoke to me of restless spirits that could only be appeased by strange, mechanical offerings. It got me thinking—why not create a little device to entertain them?");
                                inspirationModule.AddOption("The Gloomy Undertaker? Tell me more about him.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule undertakerModule = new DialogueModule("The Gloomy Undertaker is an odd fellow. Stoic, pragmatic, and utterly unsettling. He has a fascination with death, a chilling interest in the boundary between the living and the dead. He offers services that delve into the occult and supernatural—séances, embalming rituals, and the crafting of charms said to ward off spirits. He once told me that death is just another kind of journey, and that those who understand it hold power over both realms.");
                                        undertakerModule.AddOption("What kind of services does he offer?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule servicesModule = new DialogueModule("He offers things you wouldn't expect. For a price, he can commune with spirits, offering messages from beyond the grave. He also creates protective amulets, each one supposedly blessed—or cursed, depending on your perspective. And then there are his burial rites... I've heard tales of people who weren't quite dead being prepared for their final rest. He has a way of unsettling even the bravest of men.");
                                                servicesModule.AddOption("That sounds... unnerving. Why do people go to him?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule reasonsModule = new DialogueModule("People go to him out of desperation, mostly. They want answers, closure, or some form of control over death. There are those who wish to contact loved ones, others who seek vengeance or protection from restless spirits. His pragmatic nature means he never judges, merely provides what is asked—at a cost, of course. Death fascinates him, and I suspect he gets something out of every transaction, beyond mere gold.");
                                                        reasonsModule.AddOption("That's quite dark. Let's talk about something else.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, reasonsModule));
                                                    });
                                                servicesModule.AddOption("I think I need a break from this topic.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, servicesModule));
                                            });
                                        undertakerModule.AddOption("I think I'd rather not know more.",
                                            plaaaq => true,
                                            plaaaq =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, undertakerModule));
                                    });
                                inspirationModule.AddOption("That's an unusual source of inspiration.",
                                    plaaw => true,
                                    plaaw =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, inspirationModule));
                            });
                        ratModule.AddOption("Fascinating! Tell me more.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule moreModule = new DialogueModule("Well, one time, it got loose in the mayor's office... Let’s just say, it wasn't my fault that important documents were nibbled on!");
                                moreModule.AddOption("That's quite the story!", plaae => true, plaae => plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa))));
                                plaa.SendGump(new DialogueGump(plaa, moreModule));
                            });
                        ratModule.AddOption("Thank you, that's enough.", plaa => true, plaa => plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa))));
                        pl.SendGump(new DialogueGump(pl, ratModule));
                    });

                inventionsModule.AddOption("Tell me about the Clockwork Bird.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule birdModule = new DialogueModule("The Clockwork Bird is a mechanical marvel, capable of mimicking the songs of real birds. It's powered by a series of intricate gears, each tuned to create different pitches. It's not just a toy—it can also be used to deliver messages, as long as the distance isn't too far.");
                        birdModule.AddOption("How did you learn to make something like that?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule learningModule = new DialogueModule("Ah, that took years of practice and the help of some... unconventional mentors. One of them was the Gloomy Undertaker. He knows a great deal about the ethereal and the mechanical. He believes that by understanding death, one can understand the inner workings of all life, even the artificial kind. He gave me insights into the delicate balance of gears and the energy they can channel.");
                                learningModule.AddOption("The Undertaker again? He seems important to you.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule importanceModule = new DialogueModule("Indeed, he has been both a mentor and an enigma. His knowledge of the arcane and the mechanical has pushed my inventions to a new level. Though I must admit, sometimes his fascination with death leaves me uneasy. He speaks of spirits like old friends and claims that even the dead have stories left to tell.");
                                        importanceModule.AddOption("What kind of stories?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule storiesModule = new DialogueModule("The kind that keep you up at night. He once told me about a spirit that wouldn't rest until it saw justice for its untimely death. The Undertaker brokered a deal—an invention of mine was part of it, a device to amplify the spirit's whispers so that its murderer could hear them day and night until they confessed. Creepy, but effective.");
                                                storiesModule.AddOption("That's terrifying.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                storiesModule.AddOption("I'd rather not hear more about this.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, storiesModule));
                                            });
                                        importanceModule.AddOption("Let's talk about something less macabre.",
                                            plaad => true,
                                            plaad =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, importanceModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, learningModule));
                            });
                        birdModule.AddOption("That's incredible. Thank you.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, birdModule));
                    });

                inventionsModule.AddOption("Tell me about the SpikedChair.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule chairModule = new DialogueModule("Ah, the SpikedChair! Not for sitting, mind you, unless you enjoy a prickly experience. It's a deterrent for thieves, rigged to spring spikes if tampered with. Very effective! It was inspired by an idea the Gloomy Undertaker gave me—something about making the living feel the discomfort of the dead.");
                        chairModule.AddOption("Why would anyone want that?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule discomfortModule = new DialogueModule("It's about fear and deterrence. The Undertaker believes that fear can be used as a tool, just as much as a wrench or a cog. The discomfort of the living is nothing compared to the silence of the grave, he says. He once mentioned that the dead don't feel pain, only regret. The spikes are a reminder that we're still capable of feeling.");
                                discomfortModule.AddOption("That is unsettling. Let's change the subject.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, discomfortModule));
                            });
                        chairModule.AddOption("Interesting!", pla => true, pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla))));
                        pl.SendGump(new DialogueGump(pl, chairModule));
                    });

                inventionsModule.AddOption("Enough about inventions.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl))));
                p.SendGump(new DialogueGump(p, inventionsModule));
            });

        // Trade option
        greeting.AddOption("Do you need anything?",
            p => true,
            p =>
            {
                DialogueModule tradeModule = new DialogueModule("Ah, yes! I am searching for a BrokenBottle. In return, I can offer you a SpikedChair. But remember, I can only do this once every 10 minutes.");
                tradeModule.AddOption("I have a BrokenBottle.",
                    pl => HasBrokenBottle(pl) && CanTradeWithPlayer(pl),
                    pl => CompleteTrade(pl));
                tradeModule.AddOption("I don't have one right now.",
                    pl => !HasBrokenBottle(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a BrokenBottle.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                tradeModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, tradeModule));
            });

        // Goodbye option
        greeting.AddOption("Goodbye.",
            p => true,
            p => p.SendMessage("Sprocket waves energetically, his many trinkets jingling."));

        return greeting;
    }

    private bool HasBrokenBottle(PlayerMobile player)
    {
        // Check the player's inventory for BrokenBottle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BrokenBottle)) != null;
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
        // Remove the BrokenBottle and give the SpikedChair and MaxxiaScroll, then set the cooldown timer
        Item brokenBottle = player.Backpack.FindItemByType(typeof(BrokenBottle));
        if (brokenBottle != null)
        {
            brokenBottle.Delete();
            player.AddToBackpack(new SpikedChair());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the BrokenBottle and receive a SpikedChair and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a BrokenBottle.");
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