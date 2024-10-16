using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZoraTheSeaTeller : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZoraTheSeaTeller() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zora the Sea Teller";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2125)); // Deep blue robe, representing the ocean
        AddItem(new Sandals(2413)); // Sandy-colored sandals
        AddItem(new WizardsHat(1321)); // A hat with a nautical touch
        AddItem(new HildebrandtShield()); // A necklace adorned with seashells

        VirtualArmor = 15;
    }

    public ZoraTheSeaTeller(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Zora, a teller of oceanic tales and keeper of the secrets of the deep. Do you, by chance, bring with you something exotic from the sea?");

        // Dialogue options
        greeting.AddOption("Tell me more about the sea and its secrets.",
            p => true,
            p =>
            {
                DialogueModule seaStoriesModule = new DialogueModule("The sea is a mysterious force, full of wonders and dangers. I've spent my life traveling its vast expanse, collecting tales and treasures. But even I can't do it alone - I need the help of brave souls like you.");

                // Adding more nested and detailed dialogue
                seaStoriesModule.AddOption("What kind of treasures have you found?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule treasuresModule = new DialogueModule("Ah, the treasures of the sea... Some are pearls shimmering with moonlight, others are relics from ships long forgotten. But not all treasures are gold or jewels. Some are the echoes of the past, the whispers of spirits lost to the tides.");

                        treasuresModule.AddOption("Spirits? What do you mean?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule spiritsModule = new DialogueModule("Yes, spirits. The sea is haunted, you see. The souls of sailors who met their end in the cold embrace of the waves linger, their stories untold. Sometimes, I can hear them... especially when I paint. Their voices guide my brush, but they also haunt my dreams.");

                                spiritsModule.AddOption("That sounds disturbing. Are you alright?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule disturbedArtistModule = new DialogueModule("Alright? No, I am not alright. The visions... they are maddening. Each stroke of my brush reveals another nightmare, another tale of despair. But I cannot stop. I must paint, I must capture their sorrow before it consumes me entirely.");

                                        disturbedArtistModule.AddOption("Why do you keep painting if it brings you pain?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule passionAndMadnessModule = new DialogueModule("Because it is my passion! My curse! The sea chose me, and I cannot refuse her call. The madness, the visions, they are all part of me now. Without them, I am nothing. I paint not to remember, but because I cannot forget.");

                                                passionAndMadnessModule.AddOption("Is there anything I can do to help you?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Help me? Perhaps... If you bring me an ExoticFish, it may calm the spirits for a time. The sea speaks through its creatures, and perhaps with such an offering, I could find some peace, even if just for a moment.");

                                                        helpModule.AddOption("I will find an ExoticFish for you.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Thank you, traveler. I hope the sea guides your journey.");
                                                            });

                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, passionAndMadnessModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, disturbedArtistModule));
                                    });

                                spiritsModule.AddOption("That sounds fascinating. Can you show me one of your paintings?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule paintingModule = new DialogueModule("I wish I could, but the paintings are not for mortal eyes. They are cursed, each one a glimpse into the madness that lurks beneath the waves. To see them is to invite the sea's despair into your heart.");

                                        paintingModule.AddOption("I understand. Perhaps it's better that way.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Yes, it is better. Some things are not meant to be seen.");
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, paintingModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, spiritsModule));
                            });

                        treasuresModule.AddOption("Can you tell me about one of the ships you found?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule shipModule = new DialogueModule("Ah, the Sea Maiden. She was a grand vessel, her sails as white as foam. But she met her fate on the rocks, pulled under by a storm that seemed almost alive. I still remember the screams of her crew, the way the waves swallowed their hope.");

                                shipModule.AddOption("That sounds tragic. Did anyone survive?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule survivorsModule = new DialogueModule("No, none survived. The sea claimed them all. But their spirits linger, and sometimes, on quiet nights, I can hear their songs. They sing of lost loves, of dreams unfulfilled. It is beautiful, and yet... it is so very sad.");

                                        survivorsModule.AddOption("The sea is both beautiful and terrifying.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Indeed, traveler. The sea is a paradox - a cruel mistress and a gentle mother. We who live by her side must accept both faces.");
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, survivorsModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, shipModule));
                            });

                        pl.SendGump(new DialogueGump(pl, treasuresModule));
                    });

                seaStoriesModule.AddOption("Do you need any materials?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have an ExoticFish, I would gladly trade you something special. You may choose between a HildebrandtShield or a GarbageBag, and I'll always include a MaxxiaScroll as a token of gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an ExoticFish for me?");
                                tradeModule.AddOption("Yes, I have an ExoticFish.",
                                    plaa => HasExoticFish(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasExoticFish(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an ExoticFish.");
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
                        tradeIntroductionModule.AddOption("Maybe another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, seaStoriesModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Zora nods knowingly, her eyes glinting like the waves.");
            });

        return greeting;
    }

    private bool HasExoticFish(PlayerMobile player)
    {
        // Check the player's inventory for ExoticFish
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExoticFish)) != null;
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
        // Remove the ExoticFish and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item exoticFish = player.Backpack.FindItemByType(typeof(ExoticFish));
        if (exoticFish != null)
        {
            exoticFish.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for HildebrandtShield and GarbageBag
            rewardChoiceModule.AddOption("HildebrandtShield", pl => true, pl =>
            {
                pl.AddToBackpack(new HildebrandtShield());
                pl.SendMessage("You receive a HildebrandtShield!");
            });

            rewardChoiceModule.AddOption("GarbageBag", pl => true, pl =>
            {
                pl.AddToBackpack(new GarbageBag());
                pl.SendMessage("You receive a GarbageBag!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an ExoticFish.");
        }
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