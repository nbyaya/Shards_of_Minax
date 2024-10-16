using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThaddiusTheMinstrel : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaddiusTheMinstrel() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thaddius the Wandering Minstrel";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(90);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(2653)); // Fancy shirt with a blue hue
        AddItem(new LongPants(2125)); // Bright red pants
        AddItem(new Sandals(1175)); // Light yellow sandals
        AddItem(new FeatheredHat(144)); // A feathered hat for a bardic look
        AddItem(new Lute()); // Carrying a lute as a prop

        VirtualArmor = 15;
    }

    public ThaddiusTheMinstrel(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Thaddius, a minstrel of many talents and tales. Do you perhaps have an ear for a melody or a desire for an exchange?");

        // Initial options for dialogue
        greeting.AddOption("Tell me a story of your travels.",
            p => true,
            p =>
            {
                DialogueModule storyModule = new DialogueModule("Ah, there are so many tales! Let me tell you of the time I played for a dragon and lived to tell the tale. It was deep in the caverns of the Wyrmwood, where I was cornered by a great beast. I played my lute, and miraculously, it spared me... though I did lose a few strings in the process.");
                storyModule.AddOption("That sounds incredible!", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                storyModule.AddOption("Sounds dangerous, I'll pass on dragon tales.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                // Nested story options
                storyModule.AddOption("What did the dragon look like?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule dragonDescriptionModule = new DialogueModule("The dragon's scales shimmered like molten silver under the dim light of the cavern. Its eyes, glowing like twin suns, seemed to pierce through my soul. It was magnificent, yet utterly terrifying.");
                        dragonDescriptionModule.AddOption("Did you ever meet the dragon again?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule secondEncounterModule = new DialogueModule("Indeed, I did! Years later, I was traveling through the mountainous regions near the Wyrmwood. I heard a familiar rumble in the distance, and there it was again. This time, it simply nodded at me and flew away. I suppose it remembered the song.");
                                secondEncounterModule.AddOption("A dragon with a memory for music! Amazing!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                secondEncounterModule.AddOption("How did you feel at that moment?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule feelingsModule = new DialogueModule("I felt a mix of awe and fear. But above all, I felt a strange connection. It was as if, for a moment, two beings from entirely different worlds understood each other.");
                                        feelingsModule.AddOption("That must have been unforgettable.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, feelingsModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, secondEncounterModule));
                            });
                        
                        dragonDescriptionModule.AddOption("Did it threaten you again?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule threatModule = new DialogueModule("No, strangely enough. It seemed almost amused by my presence. Perhaps it enjoyed the music more than I knew.");
                                threatModule.AddOption("That's remarkable.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, threatModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, dragonDescriptionModule));
                    });
                
                p.SendGump(new DialogueGump(p, storyModule));
            });

        // Trade option
        greeting.AddOption("Do you have anything to trade?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroModule = new DialogueModule("Ah, indeed I do! I am searching for an OrnateHarp. In return, I can offer you a rare MedusaHead and a MaxxiaScroll. However, my generosity is limited, and I can only make such an exchange once every 10 minutes.");
                tradeIntroModule.AddOption("I have an OrnateHarp to trade.",
                    pl => HasOrnateHarp(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                tradeIntroModule.AddOption("I don't have an OrnateHarp right now.",
                    pl => !HasOrnateHarp(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have an OrnateHarp, my friend.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                tradeIntroModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                tradeIntroModule.AddOption("Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroModule));
            });

        // Enigmatic Librarian - New Character Intertwined in the Dialogue
        greeting.AddOption("I heard about an enigmatic librarian in town. Do you know anything about her?",
            p => true,
            p =>
            {
                DialogueModule librarianIntroModule = new DialogueModule("Ah, you must be talking about Selene, the keeper of the arcane tomes. She resides in the depths of the old library, guarding the secrets of this town's dark past. Selene is not someone you approach lightly, traveler. She knows more than she lets on, and those secrets are often better left untouched.");
                librarianIntroModule.AddOption("Why does she guard these secrets?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule secretGuardingModule = new DialogueModule("The town has a history that most would rather forget. Dark pacts, forbidden rituals, and the remnants of a cult that nearly brought the town to ruin. Selene took it upon herself to protect those secrets, ensuring that no one makes the same mistakes again.");
                        secretGuardingModule.AddOption("What kind of rituals?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule ritualModule = new DialogueModule("Whispers speak of rituals to summon beings from beyond our realm. Spirits that can twist fate, alter reality, or even grant immortality. But such power comes at a terrible price, and the town nearly tore itself apart because of it.");
                                ritualModule.AddOption("That sounds terrifying."
                                    , plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                ritualModule.AddOption("Did Selene witness any of this?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule witnessModule = new DialogueModule("Some say she did, that she was but a child when the rituals were performed. Others claim she was the one who put an end to it, at great personal cost. No one truly knows, and Selene isn't one to share her past.");
                                        witnessModule.AddOption("A true guardian, then.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, witnessModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, ritualModule));
                            });
                        secretGuardingModule.AddOption("Where can I find her?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule locationModule = new DialogueModule("She can be found in the old library at the edge of town. But be warned, she doesn’t take kindly to intrusions, and she has her ways of keeping unwanted guests at bay.");
                                locationModule.AddOption("I’ll keep that in mind.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, locationModule));
                            });
                        pl.SendGump(new DialogueGump(pl, secretGuardingModule));
                    });
                librarianIntroModule.AddOption("Why would anyone want to know these secrets?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule curiosityModule = new DialogueModule("Power, my friend. Some people are drawn to it like moths to a flame. They believe they can control it, bend it to their will. But such power often consumes those who seek it.");
                        curiosityModule.AddOption("A dangerous path indeed.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, curiosityModule));
                    });
                p.SendGump(new DialogueGump(p, librarianIntroModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Thaddius nods and strums his lute softly as you depart.");
            });

        return greeting;
    }

    private bool HasOrnateHarp(PlayerMobile player)
    {
        // Check the player's inventory for OrnateHarp
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(OrnateHarp)) != null;
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
        // Remove the OrnateHarp and give the MedusaHead and MaxxiaScroll, then set the cooldown timer
        Item ornateHarp = player.Backpack.FindItemByType(typeof(OrnateHarp));
        if (ornateHarp != null)
        {
            ornateHarp.Delete();
            player.AddToBackpack(new MedusaHead());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the OrnateHarp and receive a MedusaHead and a MaxxiaScroll in return. Thank you for this fine trade!");
        }
        else
        {
            player.SendMessage("It seems you no longer have an OrnateHarp.");
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