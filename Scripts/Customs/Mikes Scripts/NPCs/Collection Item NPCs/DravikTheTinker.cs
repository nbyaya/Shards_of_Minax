using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class DravikTheTinker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public DravikTheTinker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dravik the Tinker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1650)); // Dark blue fancy shirt
        AddItem(new LongPants(1109)); // Grey pants
        AddItem(new Boots(1175)); // Light brown boots
        AddItem(new SmithHammer()); // A tinker-themed item for roleplaying
        AddItem(new HalfApron(1153)); // Blacksmith-style apron

        VirtualArmor = 15;
    }

    public DravikTheTinker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Dravik, a tinker and master craftsman. Are you interested in an exchange of rare materials?");

        // Start with dialogue about his work and interests
        greeting.AddOption("What kind of work do you do?",
            p => true,
            p =>
            {
                DialogueModule workModule = new DialogueModule("I am a tinker of some renown. I craft intricate mechanisms, tools, and trinkets. My creations range from practical gadgets to wondrous curiosities, some powered by mysterious energies.");
                workModule.AddOption("Tell me more about your creations.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule creationsModule = new DialogueModule("Ah, well, I have crafted items like the Clockwork Beetle, a mechanical pet, and even devices to aid miners and smiths in their tasks. The finest work, though, is always made with the rarest materials.");
                        creationsModule.AddOption("Sounds fascinating!", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        creationsModule.AddOption("Are your creations for sale?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule saleModule = new DialogueModule("Oh ho, interested in buying, are we? Some might say everything has a price, but my finest creations are not for coin alone. They require rare components, and perhaps a willingness to get your hands a bit dirty.");
                                saleModule.AddOption("What kind of rare components?",
                                    plb => true,
                                    plb =>
                                    {
                                        DialogueModule componentsModule = new DialogueModule("Some components are mundane enough: gears, levers, springs. But others... others require a touch of alchemy or a dip into the arcane. Sometimes I need materials that are difficult to acquire, or even a... special sort of meat. Not for the squeamish.");
                                        componentsModule.AddOption("Special sort of meat?",
                                            plc => true,
                                            plc =>
                                            {
                                                DialogueModule meatModule = new DialogueModule("Ah, yes. You look like the type who can keep a secret. The townsfolk adore my fine meats, succulent and tender. But few know the real story of my sourcing methods. Let's just say... the forest can be unforgiving to those who wander too far alone.");
                                                meatModule.AddOption("That's... unsettling. How do you sleep at night?",
                                                    pld => true,
                                                    pld =>
                                                    {
                                                        DialogueModule ruthlessModule = new DialogueModule("Hah! Sleep? Oh, I sleep like a babe, my friend. Life is ruthless, and you either learn to wield that ruthlessness, or you become prey to it. I simply play my part, and those that eat my delicious cuts? They never complain.");
                                                        ruthlessModule.AddOption("I suppose... it makes sense in a twisted way.",
                                                            ple => true,
                                                            ple =>
                                                            {
                                                                ple.SendGump(new DialogueGump(ple, CreateGreetingModule(ple)));
                                                            });
                                                        ruthlessModule.AddOption("You're a monster!",
                                                            ple => true,
                                                            ple =>
                                                            {
                                                                ple.SendMessage("Dravik laughs heartily, showing no sign of remorse.");
                                                                ple.SendGump(new DialogueGump(ple, CreateGreetingModule(ple)));
                                                            });
                                                        pld.SendGump(new DialogueGump(pld, ruthlessModule));
                                                    });
                                                meatModule.AddOption("I see. Let's talk about something else.",
                                                    pld => true,
                                                    pld =>
                                                    {
                                                        pld.SendGump(new DialogueGump(pld, CreateGreetingModule(pld)));
                                                    });
                                                plc.SendGump(new DialogueGump(plc, meatModule));
                                            });
                                        componentsModule.AddOption("Let's move on. This is getting strange.",
                                            plc => true,
                                            plc =>
                                            {
                                                plc.SendGump(new DialogueGump(plc, CreateGreetingModule(plc)));
                                            });
                                        plb.SendGump(new DialogueGump(plb, componentsModule));
                                    });
                                saleModule.AddOption("I'll keep my hands clean, thank you.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule(plb)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, saleModule));
                            });
                        pl.SendGump(new DialogueGump(pl, creationsModule));
                    });
                workModule.AddOption("That sounds impressive.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, workModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you need any special materials?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am currently in search of a FineGoldWire for one of my latest inventions. If you have one, I can offer you a PlatinumChip in exchange, along with a special MaxxiaScroll as a token of my gratitude.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a FineGoldWire for me?");
                        tradeModule.AddOption("Yes, I have a FineGoldWire.",
                            plaa => HasFineGoldWire(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasFineGoldWire(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a FineGoldWire.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                tradeIntroductionModule.AddOption("Maybe another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Deeper into his secrets
        greeting.AddOption("Why are the townsfolk so fond of you?",
            p => true,
            p =>
            {
                DialogueModule fondnessModule = new DialogueModule("Oh, the townsfolk do enjoy my... delicacies. I provide the finest cuts of meat, tender and flavorful, like nothing they've ever tasted. It's all about keeping the supply flowing and the quality... unmatched.");
                fondnessModule.AddOption("Where do you get your meat?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule sourcingModule = new DialogueModule("Ah, now, that's a bit of a secret. You see, I've learned to make use of every opportunity, whether it's a stray animal or... a wandering traveler. Nothing goes to waste, and the townsfolk? They simply can't get enough.");
                        sourcingModule.AddOption("That's horrifying!",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Dravik chuckles, his eyes glinting mischievously.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        sourcingModule.AddOption("So you're saying you've... used people?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule peopleModule = new DialogueModule("People, animals... in the end, it's all meat, isn't it? The weak fall, and the strong survive. I'm simply making sure nothing is wasted.");
                                peopleModule.AddOption("You're twisted. Goodbye.",
                                    plab => true,
                                    plab =>
                                    {
                                        plab.SendMessage("Dravik waves you off, unfazed by your judgment.");
                                    });
                                peopleModule.AddOption("You have a dark sense of practicality.",
                                    plab => true,
                                    plab =>
                                    {
                                        plab.SendMessage("Dravik smiles broadly, clearly pleased with your understanding.");
                                        plab.SendGump(new DialogueGump(plab, CreateGreetingModule(plab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, peopleModule));
                            });
                        pl.SendGump(new DialogueGump(pl, sourcingModule));
                    });
                fondnessModule.AddOption("I think I'd rather not know.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, fondnessModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Dravik nods and returns to his tinkering.");
            });

        return greeting;
    }

    private bool HasFineGoldWire(PlayerMobile player)
    {
        // Check the player's inventory for FineGoldWire
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FineGoldWire)) != null;
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
        // Remove the FineGoldWire and give the PlatinumChip and MaxxiaScroll, then set the cooldown timer
        Item fineGoldWire = player.Backpack.FindItemByType(typeof(FineGoldWire));
        if (fineGoldWire != null)
        {
            fineGoldWire.Delete();
            player.AddToBackpack(new PlatinumChip());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the FineGoldWire and receive a PlatinumChip and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a FineGoldWire.");
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