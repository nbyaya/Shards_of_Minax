using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class RosewoodTheWanderer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RosewoodTheWanderer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rosewood the Wanderer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(90);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1653)); // Fancy shirt with a deep blue hue
        AddItem(new LongPants(1109)); // Pants with a dark hue
        AddItem(new Boots(1175)); // Boots with a dark brown hue
        AddItem(new FeatheredHat(1131)); // Feathered hat with a burgundy hue
        AddItem(new Lantern()); // Holds a lantern as if guiding travelers

        VirtualArmor = 12;
    }

    public RosewoodTheWanderer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! I am Rosewood, a wanderer with tales of distant lands. What brings you here? Perhaps a trade or a story?");

        // Start with dialogue about his travels
        greeting.AddOption("Tell me about your travels.", 
            p => true, 
            p =>
            {
                DialogueModule travelsModule = new DialogueModule("I've wandered far and wide. The shimmering dunes of Nujel'm, the icy peaks of Dagger Isle, and the deep, ancient forests of Yew. Each place has its own secrets and wonders. Would you like to hear about the Starless Oasis, the Whispering Pines, or the Frostfire Caverns?");
                
                // Nested options for each location
                travelsModule.AddOption("Tell me about the Starless Oasis.",
                    pl => true, 
                    pl =>
                    {
                        DialogueModule oasisModule = new DialogueModule("The Starless Oasis is a place of eerie stillness. No stars shine above it at night, and the air feels thick with old magic. They say the sands conceal treasures from an ancient civilization, but only those who can navigate by instinct alone will ever find them.");
                        oasisModule.AddOption("That sounds mysterious!", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule deepOasisModule = new DialogueModule("Indeed, many have tried to uncover its secrets, but few have returned. They say the spirits of those ancient people still roam the sands, guarding their secrets. Some nights, you can hear their whispers in the wind. I once met a man who claimed to have found a relic there—a golden scarab—but he vanished shortly after.");
                                deepOasisModule.AddOption("Did he vanish in the Oasis?", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule vanishModule = new DialogueModule("No, strangely enough, he vanished in his own home, miles away from the desert. His neighbors said they saw strange shadows moving through his windows the night he disappeared. It is said that the relic may have been cursed.");
                                        vanishModule.AddOption("Curses? Do you believe in them?", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                DialogueModule curseModule = new DialogueModule("I believe that the world holds many mysteries, and curses are just one of them. There are forces beyond our understanding, both good and ill. It is why I am careful about what I take from the lands I visit.");
                                                curseModule.AddOption("Wise words, Rosewood.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, curseModule));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, vanishModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, deepOasisModule));
                            });
                        pl.SendGump(new DialogueGump(pl, oasisModule));
                    });

                travelsModule.AddOption("Tell me about the Whispering Pines.",
                    pl => true, 
                    pl =>
                    {
                        DialogueModule pinesModule = new DialogueModule("The Whispering Pines are unlike any other forest. The trees seem to communicate, whispering secrets to those who dare to listen. Some say the whispers lead to hidden groves, filled with rare herbs and guarded by beings of light and shadow.");
                        pinesModule.AddOption("How intriguing!", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule pinesDeeperModule = new DialogueModule("Indeed, the whispers are not always friendly. They can guide you to great wonders, but they can also lead you astray. Many have become lost, wandering endlessly, drawn by the promises of the forest. There are those who say the forest is alive, that it feeds on those who lose their way.");
                                pinesDeeperModule.AddOption("Has anyone found these hidden groves?", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule groveModule = new DialogueModule("Yes, a few have. One such person was a healer named Elowen. She found a grove filled with herbs that could heal almost any ailment. She became famous for her remedies, but she also spoke of the price she had to pay—she claimed the forest took a part of her spirit in return.");
                                        groveModule.AddOption("What happened to Elowen?", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                DialogueModule elowenModule = new DialogueModule("Elowen eventually disappeared. Some say she returned to the forest, unable to resist its call. Others believe she was taken because she broke some pact with the forest spirits. Whatever the truth, her knowledge of herbs was unparalleled, and some of her scrolls can still be found today.");
                                                elowenModule.AddOption("I'd love to find one of her scrolls.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Perhaps you will, if fate wills it.");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, elowenModule));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, groveModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, pinesDeeperModule));
                            });
                        pl.SendGump(new DialogueGump(pl, pinesModule));
                    });

                travelsModule.AddOption("Tell me about the Frostfire Caverns.",
                    pl => true, 
                    pl =>
                    {
                        DialogueModule cavernsModule = new DialogueModule("The Frostfire Caverns are a paradox of elements. Ice and fire coexist, creating a dazzling spectacle of blue and orange light. Few have braved its depths, for the creatures within are unlike anything else—formed of both flame and frost, with tempers just as volatile.");
                        cavernsModule.AddOption("I'll remember that.", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule deeperCavernsModule = new DialogueModule("The creatures are not the only danger. The caverns themselves seem to shift and change, as if they are alive. I met a miner once who claimed that the walls whispered to him, offering secrets in exchange for warmth. He said the cold was unbearable, but he couldn't stop listening.");
                                deeperCavernsModule.AddOption("What secrets did he learn?", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule secretsModule = new DialogueModule("He spoke of a flame that never dies, hidden deep within the caverns—a fire that could warm the coldest of hearts and even bring life to the dead. But he warned that those who sought it often lost themselves in the process, consumed by their own desire.");
                                        secretsModule.AddOption("A dangerous pursuit, indeed.", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, secretsModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, deeperCavernsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, cavernsModule));
                    });

                p.SendGump(new DialogueGump(p, travelsModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you have something to trade?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Ah, yes indeed. I seek a YardCupid item for my collection. If you bring me one, I shall give you a ScentedCandle and a MaxxiaScroll in return. But I can only make such a trade once every 10 minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pla => CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a YardCupid for me?");
                        tradeModule.AddOption("Yes, I have a YardCupid.", 
                            plaa => HasYardCupid(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasYardCupid(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a YardCupid.");
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
                tradeIntroductionModule.AddOption("Perhaps another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Introduce Rosewood's garden and his obsession with the perfect flower
        greeting.AddOption("Tell me about your garden.",
            p => true,
            p =>
            {
                DialogueModule gardenModule = new DialogueModule("Ah, my garden is my pride and joy. A sanctuary amidst my wandering life. It is no ordinary garden, for it harbors plants of rare origin and potent properties. I am particularly obsessed with one goal—to cultivate a perfect flower, one that can grant immortality.");
                gardenModule.AddOption("Immortality? Is that even possible?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule immortalityModule = new DialogueModule("I believe it is. There are whispers of such a flower, hidden in the deepest corners of the world, and I have dedicated my life to nurturing one. It requires an extraordinary balance—a combination of patience, care, and a deep understanding of nature's mysteries.");
                        immortalityModule.AddOption("How do you care for such plants?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule careModule = new DialogueModule("Patience, above all else. Each plant has its own needs, its own rhythms. Some need shade, others sunlight. Some need whispered songs during moonlit nights, while others thrive under the warmth of a midday sun. I must be nurturing yet firm, for nature rewards only those who are willing to give themselves fully.");
                                careModule.AddOption("It sounds almost like raising a child.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule childModule = new DialogueModule("Indeed, it is much like raising a child. Plants, like people, respond to love and care. But unlike people, they do not question, they do not resist. They grow, they bloom, and in them, I find solace. I speak to them, I share my stories, and in return, they grow stronger.");
                                        childModule.AddOption("Do you think you'll ever succeed?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule succeedModule = new DialogueModule("I do not know. Perhaps I am chasing a dream that can never be fulfilled. But the pursuit gives me purpose. Even if I never achieve immortality, I will have spent my life surrounded by beauty and life, and that is enough for me.");
                                                succeedModule.AddOption("A beautiful pursuit indeed.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, succeedModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, childModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, careModule));
                            });
                        pl.SendGump(new DialogueGump(pl, immortalityModule));
                    });
                gardenModule.AddOption("What kind of plants do you grow?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule plantsModule = new DialogueModule("I grow all manner of plants—some that heal, some that harm, and some that are simply beautiful. There is the Moonshade Lily, which blooms only under the light of a blue moon. The Crimson Thorn, whose poison can paralyze, but also holds the key to certain antidotes. And then, there are the Dreamblooms—flowers that are said to induce visions when inhaled.");
                        plantsModule.AddOption("Tell me about the Moonshade Lily.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moonshadeModule = new DialogueModule("The Moonshade Lily is a delicate flower, its petals a deep indigo that seem to shimmer under moonlight. It is said to be a key ingredient in potions that enhance one's magical abilities. But it is fickle, and only those who have earned the forest's favor may find it in bloom.");
                                moonshadeModule.AddOption("How does one earn the forest's favor?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule favorModule = new DialogueModule("One must respect the balance of the forest. Take only what you need, give back whenever you can, and listen to the whispers of the trees. The forest can sense greed, and it does not forgive those who exploit its gifts.");
                                        favorModule.AddOption("I understand. Respect for nature is paramount.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, favorModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, moonshadeModule));
                            });
                        plantsModule.AddOption("Tell me about the Crimson Thorn.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule thornModule = new DialogueModule("The Crimson Thorn is a dangerous plant, its bright red hue a warning to those who would approach. The poison from its thorns can paralyze within moments, yet in careful doses, it is a powerful ingredient for antidotes. Many healers seek it, but few dare to harvest it themselves.");
                                thornModule.AddOption("How is it harvested?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule harvestModule = new DialogueModule("With great caution and respect. One must wear thick gloves and approach during the early dawn, when the plant is least active. The thorns seem to react to movement, almost as if they have a mind of their own. It is said that only those with a calm heart can succeed.");
                                        harvestModule.AddOption("A task for the brave, indeed.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, harvestModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, thornModule));
                            });
                        plantsModule.AddOption("Tell me about the Dreamblooms.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule dreambloomModule = new DialogueModule("Dreamblooms are ethereal flowers, their scent said to induce vivid dreams or visions. They are rare, and only grow in places where the veil between worlds is thin. Some believe they allow glimpses of the future, while others say they show only what lies within one's own heart.");
                                dreambloomModule.AddOption("Have you ever used a Dreambloom?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule usedDreambloomModule = new DialogueModule("I have, once. It was... an enlightening experience. I saw myself in my garden, surrounded by flowers that glowed with an inner light. It felt as if time had stopped, and I understood, if only for a moment, the delicate balance of life. It is why I continue my quest for the perfect flower.");
                                        usedDreambloomModule.AddOption("A powerful vision indeed.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, usedDreambloomModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, dreambloomModule));
                            });
                        pl.SendGump(new DialogueGump(pl, plantsModule));
                    });
                gardenModule.AddOption("Your passion is inspiring, Rosewood.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Rosewood smiles warmly, his eyes filled with a deep sense of purpose.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, gardenModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Rosewood nods thoughtfully and continues his wandering gaze.");
            });

        return greeting;
    }

    private bool HasYardCupid(PlayerMobile player)
    {
        // Check the player's inventory for YardCupid
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(YardCupid)) != null;
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
        // Remove the YardCupid and give the ScentedCandle and MaxxiaScroll, then set the cooldown timer
        Item yardCupid = player.Backpack.FindItemByType(typeof(YardCupid));
        if (yardCupid != null)
        {
            yardCupid.Delete();
            player.AddToBackpack(new ScentedCandle());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the YardCupid and receive a ScentedCandle and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a YardCupid.");
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