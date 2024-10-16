using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZoranaFearbinder : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZoranaFearbinder() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zorana Fearbinder";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit - A mix of fearsome and mysterious appearance
        AddItem(new Robe(2345)); // Robe with a dark, sinister look
        AddItem(new Boots(1175)); // Dark boots
        AddItem(new TribalMask()); // A mysterious raven mask
        AddItem(new HumanCarvingKit()); // A unique belt to add a bit of a horror aesthetic

        VirtualArmor = 20;
    }

    public ZoranaFearbinder(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, traveler, you've stumbled upon Zorana Fearbinder. Do you feel the shadows gathering? I have tales that could chill even the bravest hearts. Or perhaps you have something of interest for me?");

        // Start with dialogue about her background and work
        greeting.AddOption("Who are you, Zorana?", 
            p => true, 
            p =>
            {
                DialogueModule whoModule = new DialogueModule("I am Zorana, a seeker of the obscure and the dark. My fascination lies in the things that go bump in the night: the whispers of the undead, the secrets of cursed relics, and the forgotten tales of ancient horrors. But alas, I am also a woman betrayed, and my journey is one of sorrow and vengeance.");

                whoModule.AddOption("Why do you seek vengeance?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule vengeanceModule = new DialogueModule("I was left at the altar by my beloved, a man I thought would stand by me until the end. On that day, everything I knew crumbled. He vanished, leaving me with naught but unanswered questions and an endless, aching emptiness. I have wandered these streets ever since, searching for him, hoping for closure, yet knowing my true desire is revenge against those who tore us apart.");

                        vengeanceModule.AddOption("Who betrayed you, Zorana?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule betrayalModule = new DialogueModule("It wasn't just my beloved who betrayed me. His family—those wretched souls—convinced him to leave me. They thought me unworthy, a woman too fascinated by the shadows, too devoted to mysteries they could not comprehend. They feared my devotion and my understanding of the darkness, and so they tore us apart. Now, I harbor a desire to see them pay for their deeds.");
                                betrayalModule.AddOption("Do you still love him?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule loveModule = new DialogueModule("Love... It is a strange thing, isn't it? Even after all he did, a part of me longs for the man he once was. I see his face in every corner of this city, hear his laughter in every whisper of the wind. Yet, love has turned to something twisted—an obsession, a need for closure that only vengeance can bring.");
                                        loveModule.AddOption("What would you do if you found him?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule foundHimModule = new DialogueModule("If I found him... I do not know. Part of me wishes to embrace him, to forgive him, to remember what we had. But the other part of me, the part that has wandered alone, that has been consumed by the shadows... that part wishes to see him suffer, to understand the pain he left me with. Perhaps I am beyond redemption, just as he is.");
                                                foundHimModule.AddOption("I hope you find peace, Zorana.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Zorana nods, her eyes glistening with unshed tears, a melancholy smile forming on her lips.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                foundHimModule.AddOption("Perhaps vengeance is not the answer.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule peaceModule = new DialogueModule("Perhaps... but what else is left for a woman like me? My heart is scarred, and my soul is restless. Vengeance is the only path that seems clear, even if it leads to my own destruction. Still, I will consider your words, traveler.");
                                                        peaceModule.AddOption("I wish you well, Zorana.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Zorana nods, her gaze distant as if looking into a future she cannot yet see.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, peaceModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, foundHimModule));
                                            });
                                        loveModule.AddOption("He does not deserve your love.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule angerModule = new DialogueModule("You are right. He does not deserve my love, nor my tears. But the heart is a foolish thing, clinging to the past even when it knows it should let go. My vengeance is not just for what he did to me, but for the foolishness he has left in my heart.");
                                                angerModule.AddOption("May your vengeance bring you peace.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Zorana smiles darkly, a flicker of determination crossing her features.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, angerModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, loveModule));
                                    });
                                betrayalModule.AddOption("I hope you find what you're looking for.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Zorana gives a wistful smile, her eyes clouded with memories.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, betrayalModule));
                            });
                        vengeanceModule.AddOption("Perhaps vengeance isn't the answer.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule doubtModule = new DialogueModule("Perhaps... Perhaps I am chasing a shadow that will never bring me peace. But what else do I have? I have given everything to my search. If not vengeance, then what purpose is left for me?");
                                doubtModule.AddOption("You could find a new path, something beyond this.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule newPathModule = new DialogueModule("A new path... It is hard to imagine after all these years. But maybe... maybe there is something beyond the darkness. If only I could see it. Thank you, traveler. Your words have given me something to think about.");
                                        newPathModule.AddOption("I hope you find your way, Zorana.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Zorana nods, her eyes softening, a hint of hope appearing where only darkness lay before.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, newPathModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, doubtModule));
                            });
                        pl.SendGump(new DialogueGump(pl, vengeanceModule));
                    });
                whoModule.AddOption("Tell me about the HorrorPumpkin.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule pumpkinModule = new DialogueModule("The HorrorPumpkin is a relic of the old autumn festivals, haunted by lost souls who never found their way home. Its origins are shrouded in mystery, but it is said that those who hold it are drawn into the realm of nightmares. Yet, for those brave enough to wield it, there is power to be found.");
                        pumpkinModule.AddOption("What kind of power?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule powerModule = new DialogueModule("It is the power to carve away the darkness—literally and metaphorically. The HumanCarvingKit I offer can shape the flesh of monsters, binding their essence and using it for your own gain. It is not a tool for the faint of heart, but for those willing to embrace the shadow.");
                                powerModule.AddOption("I'd like to make the trade.",
                                    plaa => HasHorrorPumpkin(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                powerModule.AddOption("I don't have a HorrorPumpkin right now.",
                                    plaa => !HasHorrorPumpkin(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you've gathered a HorrorPumpkin.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                powerModule.AddOption("I've traded recently; I'll come back later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Patience, traveler. You may only trade once every 10 minutes. Return when the time is right.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, powerModule));
                            });
                        pl.SendGump(new DialogueGump(pl, pumpkinModule));
                    });
                whoModule.AddOption("Goodbye.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("Zorana nods, her eyes glinting behind the raven mask.");
                    });
                p.SendGump(new DialogueGump(p, whoModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zorana gives a knowing smile, her presence lingering in the shadows.");
            });

        return greeting;
    }

    private bool HasHorrorPumpkin(PlayerMobile player)
    {
        // Check the player's inventory for HorrorPumpkin
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HorrorPumpkin)) != null;
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
        // Remove the HorrorPumpkin and give the HumanCarvingKit and MaxxiaScroll, then set the cooldown timer
        Item horrorPumpkin = player.Backpack.FindItemByType(typeof(HorrorPumpkin));
        if (horrorPumpkin != null)
        {
            horrorPumpkin.Delete();
            player.AddToBackpack(new HumanCarvingKit());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the HorrorPumpkin and receive a HumanCarvingKit and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a HorrorPumpkin.");
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