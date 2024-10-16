using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class TiberiusTheTraveler : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TiberiusTheTraveler() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tiberius the Traveler";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(100);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1150)); // Blue fancy shirt
        AddItem(new LongPants(1109)); // Dark grey pants
        AddItem(new Boots(1175)); // Light brown boots
        AddItem(new FeatheredHat(1153)); // Green feathered hat
        AddItem(new Cloak(1173)); // Light blue cloak
        AddItem(new Lantern()); // A lantern to signify he's a traveler

        VirtualArmor = 15;
    }

    public TiberiusTheTraveler(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Tiberius, a wanderer of distant lands. I collect stories, relics, and rare items. I used to be a writer once, you know, but now I find solace in poetry amidst the ruins of the world. Do you happen to carry a KnightStone?");

        // Introduce dialogue options about his travels and past
        greeting.AddOption("Where have you traveled, Tiberius?",
            p => true,
            p =>
            {
                DialogueModule travelsModule = new DialogueModule("I've journeyed through the Frozen Wastes, across the Sands of Khaldun, and sailed the Seas of Nystul. Each place holds wonders, secrets, and treasures that few ever see. One must be prepared for anything when venturing into the unknown. But it wasn't always about treasures; sometimes, I found inspiration for my poetry in those distant lands.");
                travelsModule.AddOption("What kind of treasures?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule treasuresModule = new DialogueModule("Artifacts like the KnightStone, rare gems, and enchanted relics. The KnightStone is especially fascinating; it seems to hold the memories of the past, perhaps even the stories I never got to write. Speaking of which, if you have a KnightStone, I might have something to trade.");
                        treasuresModule.AddOption("What do you offer in return for a KnightStone?",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("If you have a KnightStone, I can offer you these FancyCopperWings. I will also give you a MaxxiaScroll as a token of my appreciation. However, I can only do this once every 10 minutes.");
                                tradeModule.AddOption("Yes, I have a KnightStone.",
                                    plaa => HasKnightStone(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasKnightStone(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a KnightStone, traveler.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll come back later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("I can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeModule));
                            });
                        treasuresModule.AddOption("Tell me more about your writing.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule writingModule = new DialogueModule("Ah, my writing... I used to document the stories of the wasteland through verse. The ruins speak to me; they tell tales of a past long forgotten, of sorrow and hope. I turned those stories into poems, hoping to inspire those who hear them. Would you care to hear one of my verses?");
                                writingModule.AddOption("Yes, I would love to hear a poem.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule poemModule = new DialogueModule("'Beneath the sky of ashen gray,\nWhere shadows of the past still play,\nThe ruins whisper secrets old,\nOf knights and kings, of tales untold.\nYet in their silence, beauty blooms,\nA hope that rises from the tombs.'\n\nI hope it brings you some comfort, traveler. The world may be harsh, but there is still beauty to be found, even in sorrow.");
                                        poemModule.AddOption("That was beautiful, thank you.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Tiberius smiles sadly, his eyes distant. 'Thank you for listening. It means more than you know.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        poemModule.AddOption("It's quite melancholic. Why do you write about sadness?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule melancholyModule = new DialogueModule("I suppose I write about sadness because it is something we all share. The ruins, the wastelands—they are filled with memories of what once was. Yet, through writing, I find a way to honor those memories and turn grief into something meaningful. Perhaps, through sorrow, we can also find strength.");
                                                melancholyModule.AddOption("I understand. There's beauty in that.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Tiberius nods, his gaze softening. 'Exactly. There is beauty in understanding and in accepting what we cannot change.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                melancholyModule.AddOption("It's too heavy for me.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Tiberius sighs gently. 'I understand. Not everyone wishes to dwell in such thoughts, and that's alright too.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, melancholyModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, poemModule));
                                    });
                                writingModule.AddOption("Perhaps another time.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, writingModule));
                            });
                        pl.SendGump(new DialogueGump(pl, treasuresModule));
                    });
                travelsModule.AddOption("Sounds interesting. Perhaps another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, travelsModule));
            });

        // Add additional conversation options related to Tiberius' artistic nature
        greeting.AddOption("You seem like someone with a deep soul. Why do you travel?",
            p => true,
            p =>
            {
                DialogueModule travelPurposeModule = new DialogueModule("Ah, a good question. I suppose I travel to find meaning, to find beauty in a world that often feels lost. Once, I wrote stories sitting by a warm hearth, but those days are gone. Now, I find my inspiration in the broken, the forgotten, and the overlooked. It's my way of understanding life, and maybe helping others see it too.");
                travelPurposeModule.AddOption("Do you ever get lonely?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule lonelinessModule = new DialogueModule("Often. The road is long, and companionship is rare. But loneliness, too, has its lessons. It teaches you to find peace within yourself, to cherish the fleeting moments of connection. And sometimes, the quiet gives me space to create—to write, to dream. But yes, it can be hard.");
                        lonelinessModule.AddOption("That sounds difficult.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Tiberius nods, a melancholic smile on his face. 'It is. But difficulties shape us, don't they? They make the moments of joy all the more precious.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        lonelinessModule.AddOption("I think I understand. Solitude has its own kind of beauty.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Tiberius' eyes brighten slightly. 'Exactly. There is beauty in solitude, in the way it allows you to truly see the world and yourself.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, lonelinessModule));
                    });
                travelPurposeModule.AddOption("What inspires you to keep going?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule inspirationModule = new DialogueModule("Hope, perhaps. The belief that there is still beauty in this world, even amidst the ruins. Every time I see a flower bloom in a desolate place, or hear laughter in a town rebuilt from ashes, I am reminded that there is still something worth fighting for, something worth documenting. I want to leave behind a record of that beauty, so others may find hope too.");
                        inspirationModule.AddOption("That's truly inspiring.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Tiberius smiles, a glimmer of warmth in his eyes. 'Thank you, traveler. It is people like you who make the journey worthwhile.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        inspirationModule.AddOption("I hope you find what you're looking for.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Tiberius bows his head slightly. 'Thank you. I hope so too. And I hope your journey brings you peace and wonder.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, inspirationModule));
                    });
                p.SendGump(new DialogueGump(p, travelPurposeModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Tiberius.",
            p => true,
            p =>
            {
                p.SendMessage("Tiberius nods and continues his journey, lantern in hand, his eyes lost in distant thoughts.");
            });

        return greeting;
    }

    private bool HasKnightStone(PlayerMobile player)
    {
        // Check the player's inventory for KnightStone
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(KnightStone)) != null;
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
        // Remove the KnightStone and give the FancyCopperWings, then set the cooldown timer
        Item knightStone = player.Backpack.FindItemByType(typeof(KnightStone));
        if (knightStone != null)
        {
            knightStone.Delete();
            player.AddToBackpack(new FancyCopperWings());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the KnightStone and receive FancyCopperWings and a MaxxiaScroll in return. Safe travels!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a KnightStone.");
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