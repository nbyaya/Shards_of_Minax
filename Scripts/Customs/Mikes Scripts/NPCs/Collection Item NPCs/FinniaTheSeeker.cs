using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class FinniaTheSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FinniaTheSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Finnia the Seeker";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(75);
        SetInt(110);

        SetHits(90);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Cloak(1157)); // A deep blue cloak
        AddItem(new Boots(1109)); // Dark leather boots
        AddItem(new Kilt(1173)); // A unique purple kilt
        AddItem(new Bandana(1150)); // A light blue bandana
        AddItem(new Lantern()); // Holds a lantern, suggesting she travels a lot

        VirtualArmor = 15;
    }

    public FinniaTheSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Finnia, a seeker of rare and mysterious artifacts. I sense a kindred spirit in you. Are you interested in a little exchange?");

        // Dialogue about her travels and the RookStone
        greeting.AddOption("What kind of artifacts do you seek?",
            p => true,
            p =>
            {
                DialogueModule artifactModule = new DialogueModule("I search for relics that hold forgotten power, like the elusive RookStone. It is said to resonate with the memories of ancient heroes. Have you come across such an item?");
                artifactModule.AddOption("I have a RookStone.",
                    pl => HasRookStone(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                artifactModule.AddOption("No, I haven't found one yet.",
                    pl => !HasRookStone(pl),
                    pl =>
                    {
                        pl.SendMessage("No worries, perhaps one day you will.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                artifactModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                // More detailed dialogue paths
                artifactModule.AddOption("Why do you seek these relics?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule whySeekModule = new DialogueModule("Once, I was a beacon of hope. I led a congregation who looked to me for guidance. But that was before the world fell into shadow, before I lost everything. Now, I seek these relics as a way to understand the mysteries I once preached. Perhaps, they will help me regain my faith, or at least give meaning to my wanderings.");
                        whySeekModule.AddOption("That sounds tragic. How did you lose your faith?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule lostFaithModule = new DialogueModule("When the wastelands came, I watched as those who trusted me, those who depended on my words, were lost. My prayers went unanswered, my hope shattered. Since then, I have roamed, seeking some sign that the gods still listen, that there is still a light in the darkness.");
                                lostFaithModule.AddOption("Have you found any signs?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule foundSignsModule = new DialogueModule("I have found small glimmers, moments of grace in an otherwise bleak world. Sometimes, it is the kindness of a stranger, a flower blooming amidst the decay, or a child’s laughter. They are fleeting, but they remind me that perhaps, all is not lost.");
                                        foundSignsModule.AddOption("That's beautiful. You still offer hope to others, despite everything.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule hopeModule = new DialogueModule("Hope is all I have left to give. Even if my own faith wavers, I cannot stand by and watch others fall into despair. If my words, my actions, can bring even a little comfort, then perhaps my journey still has purpose.");
                                                hopeModule.AddOption("You're stronger than you think, Finnia.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Finnia gives a faint smile, her eyes betraying a mix of gratitude and sadness. 'Thank you, traveler. Perhaps strength comes from those around us.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, hopeModule));
                                            });
                                        foundSignsModule.AddOption("It's hard to hold onto hope in times like these.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Finnia nods slowly. 'I know. Every day is a struggle, but it is in that struggle that we find our true selves.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, foundSignsModule));
                                    });
                                lostFaithModule.AddOption("I hope you find what you're looking for.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Finnia bows her head slightly. 'Thank you, traveler. May your own journey be filled with purpose.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, lostFaithModule));
                            });
                        whySeekModule.AddOption("That must be difficult. How do you keep going?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule keepGoingModule = new DialogueModule("I keep going because I must. There are others out there who still need help, who need someone to believe in them, even if I no longer believe in myself. Compassion, even in the darkest times, is the only thing that keeps me moving.");
                                keepGoingModule.AddOption("Compassion is a strength, not a weakness.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Finnia smiles faintly. 'Perhaps you are right. It is a strength we often forget, but it is what binds us together.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                keepGoingModule.AddOption("You’re not alone, Finnia.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Finnia’s eyes well up slightly, but she quickly blinks it away. 'Thank you, traveler. Your words mean more than you know.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, keepGoingModule));
                            });
                        pl.SendGump(new DialogueGump(pl, whySeekModule));
                    });
                p.SendGump(new DialogueGump(p, artifactModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Finnia nods, her eyes glinting with curiosity and a hint of sadness.");
            });

        return greeting;
    }

    private bool HasRookStone(PlayerMobile player)
    {
        // Check the player's inventory for RookStone
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RookStone)) != null;
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
        // Remove the RookStone and give the MemorialCopper and MaxxiaScroll, then set the cooldown timer
        Item rookStone = player.Backpack.FindItemByType(typeof(RookStone));
        if (rookStone != null)
        {
            rookStone.Delete();
            player.AddToBackpack(new MemorialCopper());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the RookStone and receive a Memorial Copper and a Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a RookStone.");
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