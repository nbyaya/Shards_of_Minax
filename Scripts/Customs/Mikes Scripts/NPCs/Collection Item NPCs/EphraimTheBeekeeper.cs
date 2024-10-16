using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EphraimTheBeekeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EphraimTheBeekeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ephraim the Beekeeper";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(55);
        SetDex(50);
        SetInt(70);

        SetHits(100);
        SetMana(120);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // Yellow shirt to resemble bee color
        AddItem(new LongPants(1174)); // Dark pants
        AddItem(new Boots(1109)); // Sturdy boots
        AddItem(new StrawHat()); // Classic beekeeper hat
        AddItem(new LeatherGloves()); // Custom gloves item

        VirtualArmor = 12;
    }

    public EphraimTheBeekeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there! I'm Ephraim, caretaker of all things bee-related! You know, there's nothing quite like the buzz of a hive on a warm day. Are you interested in learning about bees, or perhaps... a little trade?");

        // Dialogue about bees
        greeting.AddOption("Tell me about bees, Ephraim.",
            p => true,
            p =>
            {
                DialogueModule beeModule = new DialogueModule("Bees are fascinating creatures! They are essential to our ecosystem, you know. The honey, the wax, the pollination... and their dances! Have you ever seen a bee dance? It tells the hive where the flowers are blooming.");
                beeModule.AddOption("Why are they so important?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule importanceModule = new DialogueModule("Bees help plants grow by pollinating them. Without bees, we wouldn't have many of the fruits and vegetables we love. Imagine a world without apples, cucumbers, or even almonds! It'd be a bleak place, wouldn't it?");
                        importanceModule.AddOption("I never knew that! Thanks, Ephraim.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, importanceModule));
                    });
                beeModule.AddOption("Tell me more about bee dances.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule danceModule = new DialogueModule("The bee dance is called the waggle dance. It's how bees communicate the distance and direction of flowers to their hive-mates. They use the angle of the dance relative to the sun to show direction. It's incredibly tactical and precise, almost military in nature!");
                        danceModule.AddOption("That's amazing! How did you learn all this?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule learnModule = new DialogueModule("Ah, well, I wasn't always a simple beekeeper. I used to be a lawman, back in the day. I learned a lot tracking down criminals, even learned to read their movements like a bee reads a dance. But those days are long behind me... or at least, mostly behind me.");
                                learnModule.AddOption("You were a lawman? Tell me more!",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule lawmanModule = new DialogueModule("Yes, I was. I was a marshal, keeping order in the wastelands. It was a tough life, filled with danger and deceit. There was one gang in particular, the Black Vipers, who made things personal. They cost me more than I care to admit, and I've been hunting them ever since. Even now, as a beekeeper, I keep my ear to the ground.");
                                        lawmanModule.AddOption("The Black Vipers? Who are they?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule vipersModule = new DialogueModule("The Black Vipers are a ruthless gang, notorious for their cold-blooded tactics. They operate in the shadows, always a step ahead, like a swarm of angry hornets. I've tracked them for years, and I've sworn to take them down, one way or another. That's why I value information, just like bees value their hive.");
                                                vipersModule.AddOption("Sounds dangerous. Why not give up the chase?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule tenaciousModule = new DialogueModule("Give up? Never. I'm tenacious, like a bee defending its hive. Once I set my mind on something, I see it through to the end. The Vipers took everything from me, and I won't rest until I see them brought to justice. That's why I live out here, tending to bees. They remind me of the order and discipline I need to stay focused.");
                                                        tenaciousModule.AddOption("I admire your determination, Ephraim.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Ephraim nods solemnly, his eyes showing a hint of gratitude.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        tenaciousModule.AddOption("I hope you find what you're looking for.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Ephraim gives a faint smile. 'So do I, traveler. So do I.'");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, tenaciousModule));
                                                    });
                                                vipersModule.AddOption("How can I help you in your quest?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Help? Well, there are always ways to help. If you come across any information about the Black Vipers, or if you find any of their insignias, bring them to me. I may be a beekeeper now, but I still have some fight left in me, and allies are always welcome.");
                                                        helpModule.AddOption("I'll keep an eye out for you, Ephraim.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Ephraim nods firmly. 'I appreciate that, traveler. Be careful out there.'");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        helpModule.AddOption("This sounds dangerous. I think I'll pass.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Ephraim gives a slight nod. 'I understand. Not everyone is cut out for this kind of work.'");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, helpModule));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, vipersModule));
                                            });
                                        lawmanModule.AddOption("That sounds like a tough life. I'm glad you found some peace with the bees.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Ephraim smiles faintly. 'The bees keep me grounded. They remind me of simpler times, and they help me focus on what's important.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, lawmanModule));
                                    });
                                learnModule.AddOption("I can see why you'd leave that behind. Must have been hard.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Ephraim nods. 'It was. But sometimes, the past doesn't let go so easily. You just learn to live with it.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, learnModule));
                            });
                        pl.SendGump(new DialogueGump(pl, danceModule));
                    });
                beeModule.AddOption("That's quite interesting. Thank you!",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, beeModule));
            });

        // Trade option
        greeting.AddOption("Do you need anything, Ephraim?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am always on the lookout for a BeeHive to support my work. I would be more than happy to offer you a RibEye in return, plus a little extra, of course. What do you say?");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a BeeHive for me?");
                        tradeModule.AddOption("Yes, I have a BeeHive.",
                            plaa => HasBeeHive(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasBeeHive(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a BeeHive.");
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
                tradeIntroductionModule.AddOption("Maybe some other time, Ephraim.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye option
        greeting.AddOption("Goodbye, Ephraim.",
            p => true,
            p =>
            {
                p.SendMessage("Ephraim waves at you with a friendly smile.");
            });

        return greeting;
    }

    private bool HasBeeHive(PlayerMobile player)
    {
        // Check the player's inventory for BeeHive
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BeeHive)) != null;
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
        // Remove the BeeHive and give the RibEye and MaxxiaScroll, then set the cooldown timer
        Item beeHive = player.Backpack.FindItemByType(typeof(BeeHive));
        if (beeHive != null)
        {
            beeHive.Delete();
            player.AddToBackpack(new RibEye());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the BeeHive and receive a RibEye and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a BeeHive.");
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