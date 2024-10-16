using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class KaelTheHoarder : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public KaelTheHoarder() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Kael the Hoarder";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new LongPants(2053)); // Dark blue pants
        AddItem(new FancyShirt(1157)); // Deep red shirt
        AddItem(new Boots(1109)); // Black boots
        AddItem(new GoldBracelet()); // A golden bracelet
        AddItem(new Cloak(1102)); // A dark cloak

        VirtualArmor = 15;
    }

    public KaelTheHoarder(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Kael, a collector of rare and powerful items. I have an eye for anything ancient or mysterious. What brings you to my humble stash of treasures?");

        // Option to introduce the exchange trade
        greeting.AddOption("Do you collect anything in particular?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("Indeed, I have been searching for an item known as the Ancient Drum. If you have one, I could offer you something quite rare in return: an Essential Book. And of course, a MaxxiaScroll as a token of gratitude.");

                collectionModule.AddOption("I have an Ancient Drum. Let's trade.",
                    pl => HasAncientDrum(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                collectionModule.AddOption("I don't have the Ancient Drum right now.",
                    pl => !HasAncientDrum(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have the Ancient Drum.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                collectionModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                collectionModule.AddOption("Perhaps another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Additional creepy taxidermy backstory and dialogue
        greeting.AddOption("What else do you collect?",
            p => true,
            p =>
            {
                DialogueModule taxidermyModule = new DialogueModule("Ah, a curious soul, aren't you? I collect more than mere trinkets... I have a passion for preserving the forms of animals. Yes, taxidermy. But not just any taxidermy... I strive to capture their essence, their souls.");

                taxidermyModule.AddOption("Why taxidermy?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule whyTaxidermyModule = new DialogueModule("Why, you ask? The stillness of death reveals the secrets of the soul. I believe that by preserving their forms in grotesque poses, I can hold onto their spirits, their memories. The townsfolk may call it madness, but they do not understand the art of it.");

                        whyTaxidermyModule.AddOption("That's... unsettling.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule unsettlingModule = new DialogueModule("Unsettling? Perhaps. But there is a beauty in the darkness, a truth that others are too afraid to face. I am merely an observer, capturing what others wish to ignore.");
                                unsettlingModule.AddOption("I suppose everyone has their hobbies...",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Kael gives you a thin, eerie smile.");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, unsettlingModule));
                            });

                        whyTaxidermyModule.AddOption("I see. You're an artist, in a way.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule artistModule = new DialogueModule("Yes, precisely! An artist, capturing the fleeting essence of life. These creatures, now frozen in time, tell stories that would otherwise be forgotten. It is my way of giving them immortality.");
                                artistModule.AddOption("That's quite profound.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Kael seems genuinely pleased by your understanding.");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, artistModule));
                            });

                        pl.SendGump(new DialogueGump(pl, whyTaxidermyModule));
                    });

                taxidermyModule.AddOption("Can I see your work?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule seeWorkModule = new DialogueModule("Ah, my workshop... it is not for the faint of heart. Shadows linger there, and whispers of the past echo from the preserved forms. If you truly wish to see it, you must be prepared for what lurks in the dark corners.");
                        seeWorkModule.AddOption("I'm not afraid. Show me.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule showWorkshopModule = new DialogueModule("Very well, but heed my warning: the things you will see are not easily forgotten. I have creatures posed in grotesque forms, twisted in ways to reveal their inner nature. Many who enter my workshop do not leave the same.");
                                showWorkshopModule.AddOption("On second thought, maybe not.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Kael chuckles darkly. 'Wise choice, traveler.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                showWorkshopModule.AddOption("Let's go.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Kael gives you a long, unsettling look. 'Very well. Follow me...' (This is where a potential event or adventure hook could occur.)");
                                    });
                                pla.SendGump(new DialogueGump(pla, showWorkshopModule));
                            });
                        seeWorkModule.AddOption("Perhaps another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Kael nods, seemingly indifferent.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, seeWorkModule));
                    });

                taxidermyModule.AddOption("I think I'll pass.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Kael smirks. 'Perhaps you are wiser than the others.'");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, taxidermyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Kael nods and returns to organizing his trinkets, his eyes glancing at his preserved creatures.");
            });

        return greeting;
    }

    private bool HasAncientDrum(PlayerMobile player)
    {
        // Check the player's inventory for Ancient Drum
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AncientDrum)) != null;
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
        // Remove the Ancient Drum and give the Essential Book and Maxxia Scroll, then set the cooldown timer
        Item ancientDrum = player.Backpack.FindItemByType(typeof(AncientDrum));
        if (ancientDrum != null)
        {
            ancientDrum.Delete();
            player.AddToBackpack(new EssentialBooks());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Ancient Drum and receive an Essential Book and a Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the Ancient Drum.");
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