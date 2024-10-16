using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class DariusTheApothecary : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public DariusTheApothecary() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sister Darius";
        Body = 0x190; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(90);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(853)); // Robe with a dark blue hue
        AddItem(new Boots(1109)); // Boots with a deep brown hue
        AddItem(new HoodedShroudOfShadows()); // Hood for a reserved, monastic appearance
        AddItem(new GoldEarrings()); // Earrings for a distinctive look

        VirtualArmor = 15;
    }

    public DariusTheApothecary(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, dear traveler. I am Sister Darius, a humble servant devoted to the care of those in need. Tell me, how can I assist you today?");

        // Introduce her dedication and backstory
        greeting.AddOption("You seem pious. What drives your mission?",
            p => true,
            p =>
            {
                DialogueModule missionModule = new DialogueModule("The Lord calls me to serve, to bring healing and solace to the afflicted. I travel from village to village, offering what comfort I can to those struck by plague and famine. I pray constantly for the strength to continue this work, and for divine guidance to lead me where I am most needed.");
                missionModule.AddOption("How do you bring healing to those in need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule healingModule = new DialogueModule("My healing arts are simple but earnest. I use herbs, poultices, and the power of prayer to alleviate suffering. The body and spirit are deeply intertwined, and I believe that compassion, more than anything, is the greatest medicine.");
                        healingModule.AddOption("Tell me about your herbs.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule herbModule = new DialogueModule("There are many herbs I use—lavender for calming the mind, chamomile for easing pain, and yarrow to staunch bleeding. Yet, even the most potent herbs require faith to be truly effective.");
                                herbModule.AddOption("Do you gather these herbs yourself?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule gatheringModule = new DialogueModule("Yes, I often journey into the wilderness to gather what I need. It is during these times that I feel closest to the divine, as I am surrounded by the Creator's works. There is a quiet peace in knowing that everything we need has already been provided.");
                                        gatheringModule.AddOption("That sounds beautiful.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Sister Darius smiles softly, her eyes full of gentle warmth.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        gatheringModule.AddOption("It must be difficult to gather so much.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule difficultyModule = new DialogueModule("Indeed, it can be. There are times when I am weary, or when the weather and wild creatures make the journey perilous. But I believe the trials I face are meant to strengthen my resolve, to test my faith and my dedication.");
                                                difficultyModule.AddOption("Your dedication is admirable.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Sister Darius bows her head humbly.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, difficultyModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, gatheringModule));
                                    });
                                herbModule.AddOption("Thank you for sharing.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, herbModule));
                            });
                        healingModule.AddOption("Can prayer really heal?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule prayerModule = new DialogueModule("Prayer is not merely words. It is an act of faith, of belief that something greater than ourselves watches over us. I have seen the power of prayer bring comfort to the dying, hope to the despairing, and even strength to the weak. Healing is more than curing the body; it is about mending the soul.");
                                prayerModule.AddOption("I think I understand.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Sister Darius nods solemnly, her eyes filled with compassion.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                prayerModule.AddOption("I am not sure I believe that.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule doubtModule = new DialogueModule("It is alright to doubt. Faith is not something that comes easily to everyone, and questioning is part of the journey. Know that I do not judge you for it, and should you ever wish to speak more on this, I am here.");
                                        doubtModule.AddOption("Thank you, Sister.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Sister Darius gives a kind smile.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, doubtModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, prayerModule));
                            });
                        healingModule.AddOption("Thank you for your work.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, healingModule));
                    });
                missionModule.AddOption("Why do you help those in need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule compassionModule = new DialogueModule("I believe that it is our sacred duty to care for others, especially those who cannot care for themselves. The Lord teaches us to love our neighbors, to show compassion and mercy, even when it is difficult. The world is harsh enough—if I can bring a little light, a little warmth, then I am fulfilling my purpose.");
                        compassionModule.AddOption("That is very noble of you.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Sister Darius smiles gently, her eyes glistening with sincerity.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        compassionModule.AddOption("Doesn't it get overwhelming?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule overwhelmingModule = new DialogueModule("There are times when I feel overwhelmed, yes. Times when the suffering I witness is almost too much to bear. But it is in these moments that I turn to prayer, to seek the strength that I lack. And always, always, I find that strength, not within myself, but in the divine, and in the kindness of those around me.");
                                overwhelmingModule.AddOption("How can I help?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("You can help by being kind to others, by offering what you can, even if it is just a kind word or a listening ear. Every small act of compassion has the power to change someone's life. And if you are willing, I could use help gathering supplies for my travels.");
                                        helpModule.AddOption("What supplies do you need?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule suppliesModule = new DialogueModule("I am always in need of herbs, food, and clean water. Additionally, if you happen to find any Bottled Plague, I could use it for studying ways to better combat its effects. In return, I can offer a Butter Churn and a Maxxia Scroll, though I must limit trades to once every ten minutes.");
                                                suppliesModule.AddOption("I have a Bottled Plague.",
                                                    plaaaa => HasBottledPlague(plaaaa) && CanTradeWithPlayer(plaaaa),
                                                    plaaaa =>
                                                    {
                                                        CompleteTrade(plaaaa);
                                                    });
                                                suppliesModule.AddOption("I don't have one right now.",
                                                    plaaaa => !HasBottledPlague(plaaaa),
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Come back when you have a Bottled Plague.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                suppliesModule.AddOption("I traded recently; I'll come back later.",
                                                    plaaaa => !CanTradeWithPlayer(plaaaa),
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, suppliesModule));
                                            });
                                        helpModule.AddOption("I'll do my best.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Sister Darius nods, her expression filled with gratitude.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, helpModule));
                                    });
                                overwhelmingModule.AddOption("Thank you for your strength.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, overwhelmingModule));
                            });
                        pl.SendGump(new DialogueGump(pl, compassionModule));
                    });
                missionModule.AddOption("Thank you for sharing your story.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, missionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Sister Darius bows her head respectfully, offering a quiet prayer for your safety.");
            });

        return greeting;
    }

    private bool HasBottledPlague(PlayerMobile player)
    {
        // Check the player's inventory for BottledPlague
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BottledPlague)) != null;
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
        // Remove the BottledPlague and give the ButterChurn and MaxxiaScroll, then set the cooldown timer
        Item bottledPlague = player.Backpack.FindItemByType(typeof(BottledPlague));
        if (bottledPlague != null)
        {
            bottledPlague.Delete();
            player.AddToBackpack(new ButterChurn());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Bottled Plague and receive a Butter Churn and a Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Bottled Plague.");
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