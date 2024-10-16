using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CadeTheGravekeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CadeTheGravekeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cade the Gravekeeper";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(60);

        SetHits(100);
        SetMana(80);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows()); // A dark hooded robe
        AddItem(new Boots(1675)); // Dark boots
        AddItem(new Lantern()); // Carries a lantern, giving an eerie vibe
        AddItem(new SkullCap(1109)); // Dark gray skull cap

        VirtualArmor = 15;
    }

    public CadeTheGravekeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Cade, the keeper of the graves. The dead have secrets, and I am their guardian. Do you have something... peculiar for me?");

        // Start with dialogue about his work
        greeting.AddOption("What do you do here?",
            p => true,
            p =>
            {
                DialogueModule workModule = new DialogueModule("I tend to the graves, ensuring no restless spirits rise. The dead deserve peace, and it is my duty to guard their rest. But sometimes, artifacts from the grave have other uses...");
                workModule.AddOption("Artifacts? What do you mean?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule artifactModule = new DialogueModule("Indeed. There are certain items that only the dead possess. If you bring me a DeadBody, I can exchange it for something less morbid - perhaps a HeartPillow to remember the departed. Do you have one?");
                        artifactModule.AddOption("I have a DeadBody.",
                            pla => HasDeadBody(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });
                        artifactModule.AddOption("I don't have one right now.",
                            pla => !HasDeadBody(pla),
                            pla =>
                            {
                                pla.SendMessage("Come back when you have a DeadBody for me.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        artifactModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        // Adding more dialogue options with detailed, nested conversations
                        artifactModule.AddOption("Tell me more about these artifacts.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule artifactDetailsModule = new DialogueModule("The artifacts are remnants of lives once lived, memories trapped in mundane objects. They have power, if you know how to use them. Each one holds a story, a secret of the departed.");
                                artifactDetailsModule.AddOption("What kind of stories?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule storyModule = new DialogueModule("Some speak of unfulfilled dreams, others of betrayal, and a few of love lost to the passage of time. These stories cling to the items, whispering in the dark. Not all can hear them, but I can.");
                                        storyModule.AddOption("What have you heard lately?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule recentStoryModule = new DialogueModule("There was a barmaid, once. She worked in the local tavern, listening to the patrons' tales. They spoke of monsters lurking in the woods, of ghosts haunting the crossroads. She knew too much, and one night, she vanished without a trace.");
                                                recentStoryModule.AddOption("A barmaid? Tell me more about her.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        DialogueModule barmaidModule = new DialogueModule("Her name was Elara. She was jaded, witty, and had a sharp tongue that could cut through the thickest bravado. She hid her fear well, but I could see it - she had witnessed horrors that would drive most to madness.");
                                                        barmaidModule.AddOption("What horrors did she see?",
                                                            plaabcd => true,
                                                            plaabcd =>
                                                            {
                                                                DialogueModule horrorModule = new DialogueModule("She saw men transformed into beasts, shadows that moved of their own accord, and heard whispers that spoke of doom. She tried to warn the patrons, but they laughed it off, calling her paranoid. It was their mistake.");
                                                                horrorModule.AddOption("Did anyone believe her?",
                                                                    plaabcde => true,
                                                                    plaabcde =>
                                                                    {
                                                                        DialogueModule believerModule = new DialogueModule("One man believed her - a traveling bard. He took her warnings seriously and left town before the darkness came. He returned years later, older and wiser, and spoke of the dangers she had warned of. By then, it was too late for the others.");
                                                                        believerModule.AddOption("What happened to the town?",
                                                                            plaabcdef => true,
                                                                            plaabcdef =>
                                                                            {
                                                                                DialogueModule townFateModule = new DialogueModule("The town fell into ruin. The darkness she spoke of consumed it, leaving only empty shells of buildings and the echoes of those who once lived there. The bard sang of its fate, a haunting melody that still lingers in the air.");
                                                                                townFateModule.AddOption("That's a tragic tale. Thank you for sharing.",
                                                                                    plaabcdefg => true,
                                                                                    plaabcdefg =>
                                                                                    {
                                                                                        plaabcdefg.SendMessage("Cade nods, his eyes reflecting the sorrow of the tale. 'The dead have much to teach, if we are willing to listen.'");
                                                                                    });
                                                                                plaabcdef.SendGump(new DialogueGump(plaabcdef, townFateModule));
                                                                            });
                                                                        plaabcde.SendGump(new DialogueGump(plaabcde, believerModule));
                                                                    });
                                                                horrorModule.AddOption("I can see why she was scared.",
                                                                    plaabcde => true,
                                                                    plaabcde =>
                                                                    {
                                                                        plaabcde.SendMessage("Cade nods solemnly. 'Fear is often the most rational response to the unknown.'");
                                                                    });
                                                                plaabcd.SendGump(new DialogueGump(plaabcd, horrorModule));
                                                            });
                                                        barmaidModule.AddOption("She must have been strong to endure that.",
                                                            plaabcd => true,
                                                            plaabcd =>
                                                            {
                                                                plaabcd.SendMessage("Cade nods, a hint of respect in his voice. 'Strength comes in many forms. Hers was the ability to keep going, even when she knew what was coming.'");
                                                            });
                                                        plaabc.SendGump(new DialogueGump(plaabc, barmaidModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, recentStoryModule));
                                            });
                                        storyModule.AddOption("I don't think I want to hear any more.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Cade nods, his eyes dark. 'Sometimes ignorance is a blessing.'");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, storyModule));
                                    });
                                artifactDetailsModule.AddOption("I think I'll pass on the haunted items.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Cade smirks. 'Not everyone is ready to face the past.'");
                                    });
                                pla.SendGump(new DialogueGump(pla, artifactDetailsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, artifactModule));
                    });
                workModule.AddOption("That's a grim task. Farewell.",
                    pla => true,
                    pla =>
                    {
                        pla.SendMessage("Cade nods solemnly, understanding your reluctance.");
                    });
                p.SendGump(new DialogueGump(p, workModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Cade bows his head slightly, his eyes lingering on the graves.");
            });

        return greeting;
    }

    private bool HasDeadBody(PlayerMobile player)
    {
        // Check the player's inventory for DeadBody
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DeadBody)) != null;
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
        // Remove the DeadBody and give the HeartPillow, then set the cooldown timer
        Item deadBody = player.Backpack.FindItemByType(typeof(DeadBody));
        if (deadBody != null)
        {
            deadBody.Delete();
            player.AddToBackpack(new HeartPillow());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the DeadBody and receive a HeartPillow and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a DeadBody.");
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