using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TiberiusTheAntiquarian : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TiberiusTheAntiquarian() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tiberius the Antiquarian";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // A deep red fancy shirt
        AddItem(new LongPants(1109)); // Dark grey pants
        AddItem(new Boots(1175)); // Dark boots
        AddItem(new FeatheredHat(1153)); // A vibrant purple hat
        AddItem(new GoldBracelet()); // A gold bracelet as a trinket
        AddItem(new JesterSkull()); // The JesterSkull is also in his inventory, so it can be stolen

        VirtualArmor = 15;
    }

    public TiberiusTheAntiquarian(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Tiberius, collector of antiquities and relics of the past. Do you have something rare to trade?");

        // Dialogue about his interest in antiquities
        greeting.AddOption("What kind of relics do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I collect rare artifacts, ancient scrolls, and peculiar items from across the lands. Each object tells a story, and I am most intrigued by stories untold. Do you have something of value?");

                collectionModule.AddOption("What do you mean by 'something of value'?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule valueModule = new DialogueModule("Ah, an inquisitive mind! I love that. By 'something of value,' I mean an artifact that holds history, mystery, or magic—something like a GlassTable, which is an item I've long been searching for. You see, such items are more than just relics; they are a bridge to the past, a portal to forgotten melodies and untold tales.");

                        valueModule.AddOption("Why are you interested in a GlassTable?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule glassTableInterestModule = new DialogueModule("Ah, the GlassTable! It is said to resonate with a frequency that few can hear, but those who can... oh, they are transported to a different place. You know, back in my radio days, I used to talk about such frequencies—how music and certain objects could resonate with the very soul of the world. Imagine being able to tune into the echoes of the past! That's what the GlassTable represents to me—a chance to let the world hear what has been lost.");
                                glassTableInterestModule.AddOption("You were a radio host? Tell me more!",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule radioHostModule = new DialogueModule("Ah, yes! I was once a radio host, broadcasting underground music to survivors, those souls who needed something to hold onto in the chaos. My show was called 'Echoes of Serenity.' I believed—and still do—that music can heal the world. I would play everything from jazz to blues, even a bit of rock 'n' roll. Those were the days... the thrill of spinning records, the magic of sharing a melody with those who needed it most. Even now, I still believe in the power of music to bring people together.");
                                        radioHostModule.AddOption("That sounds fascinating! How do you broadcast now?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule broadcastModule = new DialogueModule("These days, it isn't easy. I operate from an undisclosed location, using whatever scraps of technology I can find. I set up a small transmitter in the basement of an old, crumbling library. It's a labor of love, really. Sometimes, I play records. Sometimes, I speak directly to the people, telling stories, sharing hope, and reminding everyone that we are not alone. The chaos may rage on, but as long as there is music, there is hope.");
                                                broadcastModule.AddOption("Do you think music can really make a difference?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule musicBeliefModule = new DialogueModule("Oh, absolutely! Music has a way of reaching places that words simply cannot. It can stir the heart, spark a memory, ignite a fire within the soul. I've seen it happen countless times. People come to me, broken and weary, and they leave with a spark in their eyes, humming a tune. The world may be chaotic, but as long as we can sing, dance, and remember who we are, we have a chance.");
                                                        musicBeliefModule.AddOption("I think I understand. It's a beautiful sentiment.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, musicBeliefModule));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, broadcastModule));
                                            });
                                        radioHostModule.AddOption("Music really is powerful. Thank you for sharing.",
                                            plaaaq => true,
                                            plaaaq =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, radioHostModule));
                                    });
                                glassTableInterestModule.AddOption("I don't have a GlassTable right now.",
                                    plaaw => !HasGlassTable(plaa),
                                    plaaw =>
                                    {
                                        plaa.SendMessage("Come back when you have a GlassTable, and we shall discuss further.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                glassTableInterestModule.AddOption("I recently traded; I'll come back later.",
                                    plaae => !CanTradeWithPlayer(plaa),
                                    plaae =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, glassTableInterestModule));
                            });

                        valueModule.AddOption("I don't have one right now.",
                            plaa => !HasGlassTable(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a GlassTable, and we shall discuss further.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        valueModule.AddOption("I recently traded; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, valueModule));
                    });

                collectionModule.AddOption("I'm not interested in trading right now.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Tiberius nods thoughtfully, his eyes lingering on your belongings. 'Remember, traveler, the past has much to teach us, and music... music can heal what words cannot.'");
            });

        return greeting;
    }

    private bool HasGlassTable(PlayerMobile player)
    {
        // Check the player's inventory for GlassTable
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GlassTable)) != null;
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
        // Remove the GlassTable and give the JesterSkull and MaxxiaScroll, then set the cooldown timer
        Item glassTable = player.Backpack.FindItemByType(typeof(GlassTable));
        if (glassTable != null)
        {
            glassTable.Delete();
            player.AddToBackpack(new JesterSkull());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the GlassTable and receive a JesterSkull and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a GlassTable.");
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