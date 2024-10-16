using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ThandorTheSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThandorTheSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thandor the Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 200;

        // Outfit
        AddItem(new FancyShirt(1150)); // Deep blue shirt
        AddItem(new LongPants(1109)); // Black pants
        AddItem(new Boots(1175)); // Dark boots
        AddItem(new Cloak(1153)); // Purple cloak for a mystical look
        AddItem(new QuarterStaff()); // Staff as a sign of his knowledge-seeking nature
		AddItem(new FieldPlow());

        VirtualArmor = 15;
    }

    public ThandorTheSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Thandor, a seeker of rare artifacts and ancient lore. Tell me, do you carry something extraordinary?");

        // Dialogue options about Thandor's background
        greeting.AddOption("Who are you, Thandor?", 
            p => true, 
            p =>
            {
                DialogueModule introModule = new DialogueModule("I am Thandor, once a scholar in the grand libraries of Trinsic. I now wander the lands in search of artifacts and stories lost to time. Each item tells a story, and I make it my mission to uncover them.");
                introModule.AddOption("Why did you leave the libraries?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Knowledge is not meant to be hoarded behind walls of stone. I seek to bring wisdom to the people, and learn from the world itself, rather than old, dusty tomes.");
                        reasonModule.AddOption("That sounds admirable.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        reasonModule.AddOption("That must have been a difficult choice.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule difficultyModule = new DialogueModule("Indeed, it was. But not as difficult as the choice to leave someone I loved behind.");
                                difficultyModule.AddOption("Who did you leave behind?",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule exWifeModule = new DialogueModule("My ex-wife, Elenora. She was the love of my life, but our paths diverged when I chose to leave the comfort of Trinsic and embark on this journey. She could not bear the uncertainty and danger that came with my desire to seek knowledge in the unknown.");
                                        exWifeModule.AddOption("Do you regret leaving her?",
                                            plaabb => true,
                                            plaabb =>
                                            {
                                                DialogueModule regretModule = new DialogueModule("Every day, I wonder what life would have been like if I had stayed. I remember her smile, her laughter... the way she would listen to my wild theories, even if she did not always understand. I often feel as though I have traded one form of treasure for another.");
                                                regretModule.AddOption("Do you think you could find love again?",
                                                    plaabba => true,
                                                    plaabba =>
                                                    {
                                                        DialogueModule loveAgainModule = new DialogueModule("Ah, love. It is much like the artifacts I seek. Rare, precious, and often hidden in the most unexpected places. I hope that perhaps one day, I might stumble upon it once more. But I must admit, I am afraid. It is easier to face the unknown dangers of ancient tombs than to open my heart again.");
                                                        loveAgainModule.AddOption("Why are you afraid?",
                                                            plaabbaa => true,
                                                            plaabbaa =>
                                                            {
                                                                DialogueModule fearModule = new DialogueModule("Because love, unlike artifacts, is not something you can simply dust off and restore. It requires trust, vulnerability, and a willingness to be hurt. After losing Elenora, I am not sure I have the strength to endure that kind of pain again.");
                                                                fearModule.AddOption("Perhaps love is worth the risk.",
                                                                    plaabbaaa => true,
                                                                    plaabbaaa =>
                                                                    {
                                                                        DialogueModule hopeModule = new DialogueModule("You may be right, traveler. Perhaps, like the artifacts I seek, the true value of love lies in the risk and effort required to obtain it. Maybe one day, I will find someone who can accept me for who I am, with all my flaws and all my dreams.");
                                                                        hopeModule.AddOption("I wish you luck, Thandor.",
                                                                            plaabbaaab => true,
                                                                            plaabbaaab =>
                                                                            {
                                                                                plaabbaaab.SendGump(new DialogueGump(plaabbaaab, CreateGreetingModule(plaabbaaab)));
                                                                            });
                                                                        plaabbaaa.SendGump(new DialogueGump(plaabbaaa, hopeModule));
                                                                    });
                                                                fearModule.AddOption("I understand your hesitation.",
                                                                    plaabbaab => true,
                                                                    plaabbaab =>
                                                                    {
                                                                        plaabbaab.SendMessage("Thandor nods solemnly, lost in thought.");
                                                                        plaabbaab.SendGump(new DialogueGump(plaabbaab, CreateGreetingModule(plaabbaab)));
                                                                    });
                                                                plaabbaa.SendGump(new DialogueGump(plaabbaa, fearModule));
                                                            });
                                                        loveAgainModule.AddOption("Perhaps the artifacts are enough.",
                                                            plaabbab => true,
                                                            plaabbab =>
                                                            {
                                                                DialogueModule artifactModule = new DialogueModule("Perhaps. The artifacts hold stories, but they cannot share warmth or companionship. Still, they are safe. They cannot break your heart.");
                                                                artifactModule.AddOption("Thank you for sharing, Thandor.",
                                                                    plaabbabb => true,
                                                                    plaabbabb =>
                                                                    {
                                                                        plaabbabb.SendGump(new DialogueGump(plaabbabb, CreateGreetingModule(plaabbabb)));
                                                                    });
                                                                plaabbab.SendGump(new DialogueGump(plaabbab, artifactModule));
                                                            });
                                                        plaabba.SendGump(new DialogueGump(plaabba, loveAgainModule));
                                                    });
                                                regretModule.AddOption("I am sure she understood your passion.",
                                                    plaabbc => true,
                                                    plaabbc =>
                                                    {
                                                        DialogueModule understandingModule = new DialogueModule("I like to think she did. She always said that my heart belonged to the mysteries of the world, and she was right. Still, I hope that wherever she is, she found happiness. That is all I could ever wish for her.");
                                                        understandingModule.AddOption("She must have loved you deeply.",
                                                            plaabbcc => true,
                                                            plaabbcc =>
                                                            {
                                                                plaabbcc.SendMessage("Thandor smiles softly, his eyes distant.");
                                                                plaabbcc.SendGump(new DialogueGump(plaabbcc, CreateGreetingModule(plaabbcc)));
                                                            });
                                                        plaabbc.SendGump(new DialogueGump(plaabbc, understandingModule));
                                                    });
                                                plaabb.SendGump(new DialogueGump(plaabb, regretModule));
                                            });
                                        exWifeModule.AddOption("That must have been difficult.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                plaabc.SendMessage("Thandor sighs, his expression heavy with emotion.");
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, exWifeModule));
                                    });
                                difficultyModule.AddOption("You must miss her a lot.",
                                    plaac => true,
                                    plaac =>
                                    {
                                        DialogueModule missModule = new DialogueModule("I do. Every artifact I find, every story I uncover, I wish I could share it with her. She had a way of making my discoveries feel even more meaningful. Without her, sometimes the treasures feel hollow.");
                                        missModule.AddOption("I'm sure she would be proud of you.",
                                            plaacc => true,
                                            plaacc =>
                                            {
                                                plaacc.SendMessage("Thandor smiles wistfully, his eyes glistening.");
                                                plaacc.SendGump(new DialogueGump(plaacc, CreateGreetingModule(plaacc)));
                                            });
                                        plaac.SendGump(new DialogueGump(plaac, missModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, difficultyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });
                introModule.AddOption("Fascinating, thank you.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, introModule));
            });

        // Introduce the trade dialogue
        greeting.AddOption("I have something you might be interested in.", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroModule = new DialogueModule("Ah, I am particularly seeking a FishingBear, a rare item indeed! If you have one, I can offer you a FieldPlow in return, along with a MaxxiaScroll as a token of my appreciation. However, I can only make such a trade once every ten minutes.");
                tradeIntroModule.AddOption("I'd like to make the trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a FishingBear for me?");
                        tradeModule.AddOption("Yes, here is a FishingBear.", 
                            plaa => HasFishingBear(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have it right now.", 
                            plaa => !HasFishingBear(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a FishingBear.");
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
                tradeIntroModule.AddOption("Maybe another time.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Thandor.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thandor nods and smiles thoughtfully.");
            });

        return greeting;
    }

    private bool HasFishingBear(PlayerMobile player)
    {
        // Check the player's inventory for FishingBear
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FishingBear)) != null;
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
        // Remove the FishingBear and give the FieldPlow and MaxxiaScroll, then set the cooldown timer
        Item fishingBear = player.Backpack.FindItemByType(typeof(FishingBear));
        if (fishingBear != null)
        {
            fishingBear.Delete();
            player.AddToBackpack(new FieldPlow());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the FishingBear and receive a FieldPlow and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a FishingBear.");
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