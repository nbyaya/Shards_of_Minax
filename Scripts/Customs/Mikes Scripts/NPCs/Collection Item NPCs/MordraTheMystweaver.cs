using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MordraTheMystweaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MordraTheMystweaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mordra the Mystweaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(90);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new Robe(1109)); // Dark blue robe
        AddItem(new Sandals(1175)); // Light green sandals
        AddItem(new WizardsHat(1109)); // Dark blue wizard's hat
        AddItem(new GnarledStaff()); // A staff to complete her look
        AddItem(new EnchantedRose()); // Unique jewelry item

        VirtualArmor = 15;
    }

    public MordraTheMystweaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler seeking secrets of the arcane, are you? I am Mordra, a weaver of mystic knowledge. Do you come to barter, or merely to chat?");

        // Start with dialogue about her background
        greeting.AddOption("Who are you, Mordra?", 
            p => true, 
            p =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a Mystweaver, an alchemist of rare expertise. I travel the realms seeking powerful relics and brewing elixirs from the arcane plants I discover. The world hides many secrets, and I intend to unravel them all. But my story is not without its dark chapters.");
                aboutModule.AddOption("Tell me about your past.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("Ah, my past... not many care to ask. I was once a mere mortal like you, but I survived the apocalypse by embracing what most feared: nature itself. The world was reduced to ash, but the flora—those brave, unyielding plants—taught me their resilience. I became something... different, a mutant some say. Reclusive, yes, but wiser for it.");
                        pastModule.AddOption("A mutant? What do you mean?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule mutantModule = new DialogueModule("Indeed, a mutant. My body, my very essence, was altered by the strange energies that lingered after the great cataclysm. My connection to the earth deepened; I can hear the whispers of the forest, sense the anger of the sea, and commune with creatures others would fear. It has made me both powerful and feared—people see my eccentricity as a threat, but I have embraced what I have become.");
                                mutantModule.AddOption("How did you survive the apocalypse?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("Survival was never guaranteed. While others ran, I stayed and listened. The plants taught me secrets of regeneration, of blending into nature, of creating remedies that stave off hunger and sickness. Many thought me mad for trusting in what they deemed mere weeds, but those weeds kept me alive when others perished.");
                                        survivalModule.AddOption("What kind of remedies?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule remedyModule = new DialogueModule("Remedies? Ah, a question from one who knows value! There are potions that heal, potions that make one invisible to the eyes of foes, and potions that can make the ground beneath one's feet bloom with life. There is also the SkullBottle elixir—dangerous, yes, but also incredibly potent. It requires rare herbs and a touch of the arcane, something only I can muster.");
                                                remedyModule.AddOption("Can you teach me to make one?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule teachModule = new DialogueModule("Teach you? Perhaps... But not without a price. The SkullBottle elixir is no mere potion. It requires dedication, knowledge of rare herbs, and an understanding of the balance between life and death. Gather what I need, and perhaps I'll consider it. But heed my warning: such knowledge changes a person.");
                                                        teachModule.AddOption("What would I need to gather?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule gatherModule = new DialogueModule("You must collect the following: a Moonshadow Petal, which only blooms under a lunar eclipse; the heartwood of an Elder Ent, which is a rare and dangerous task; and finally, the Tears of a Banshee. Only when you have gathered all these will I teach you the secrets of the SkullBottle elixir.");
                                                                gatherModule.AddOption("This sounds dangerous...",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule dangerModule = new DialogueModule("Of course it is dangerous. Power never comes without risk. The path of an alchemist, especially one who meddles with the arcane and the mystical, is fraught with peril. But those who succeed find themselves capable of wonders that others can only dream of.");
                                                                        dangerModule.AddOption("I will think about it.",
                                                                            plaaaaaaa => true,
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("Mordra nods, her eyes narrowing as though she can already see your resolve wavering or growing stronger.");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, dangerModule));
                                                                    });
                                                                gatherModule.AddOption("I will return when I have what you need.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Mordra nods, her expression unreadable. 'Then may the fates watch over you, traveler.'");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, gatherModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, teachModule));
                                                    });
                                                remedyModule.AddOption("Sounds too risky for me.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Mordra gives a knowing smile. 'Not all are meant to walk the path I tread. But perhaps another day, you will find the courage.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, remedyModule));
                                            });
                                        survivalModule.AddOption("I admire your resilience.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mordra looks at you thoughtfully. 'It was not mere resilience, but adaptation. Those who cannot adapt are lost to the past.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, survivalModule));
                                    });
                                mutantModule.AddOption("Your powers sound amazing, but also lonely.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule lonelyModule = new DialogueModule("Lonely? Yes, perhaps. The world fears what it does not understand. I am reclusive, by choice and necessity. People do not trust mutants, especially those who speak to plants or draw power from them. But solitude brings clarity, and in that clarity, I have found peace.");
                                        lonelyModule.AddOption("I can understand that.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mordra smiles faintly, a flicker of something like sadness in her eyes. 'Few do. But perhaps you are different.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        lonelyModule.AddOption("It sounds difficult.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mordra nods solemnly. 'It is, but all paths worth treading are difficult. It is in hardship that we find our true selves.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, lonelyModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, mutantModule));
                            });
                        pastModule.AddOption("How did you learn alchemy?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule alchemyModule = new DialogueModule("Alchemy? Ah, it is the union of science and magic, of nature and willpower. I learned from ancient tomes, from listening to the whispers of plants, and from trial and error. There is wisdom in failure, you know. I have failed many times, but each failure brought new insight.");
                                alchemyModule.AddOption("What kind of tomes?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule tomesModule = new DialogueModule("Old tomes, scorched by the fires of the apocalypse, filled with secrets of the old world. I have found books on herbalism, ancient rituals, forgotten potions... Some were written by hands long since turned to dust. They speak of a time when alchemy was both feared and revered.");
                                        tomesModule.AddOption("Can I see one of these tomes?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mordra shakes her head. 'No, they are far too fragile, and their knowledge too dangerous for the uninitiated. But perhaps one day, when you are ready...'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        tomesModule.AddOption("I understand. Ancient knowledge must be protected.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Mordra nods approvingly. 'Indeed. Not all are so wise. Many would destroy or misuse such power.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tomesModule));
                                    });
                                alchemyModule.AddOption("What is the most powerful potion you know?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule powerfulPotionModule = new DialogueModule("The most powerful? There are potions that can extend life, potions that can make one immune to pain, and those that allow one to see the unseen. But the most dangerous is the Elixir of Transmutation. It changes the very fabric of a person's being, but it comes with a heavy cost. Many who drank it were never seen again.");
                                        powerfulPotionModule.AddOption("What happened to those who drank it?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule fateModule = new DialogueModule("No one knows for sure. Some say they transcended, becoming beings of pure energy. Others believe they were torn apart, unable to handle the transformation. It is a gamble—power, or oblivion. A choice few are brave enough to make.");
                                                fateModule.AddOption("Would you ever drink it?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Mordra pauses, her eyes distant. 'Perhaps... If I had nothing left to lose. But for now, my path lies here, in this world.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                fateModule.AddOption("That sounds terrifying.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Mordra nods gravely. 'Power is always terrifying, for it demands sacrifice.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, fateModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, powerfulPotionModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, alchemyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });
                aboutModule.AddOption("Perhaps another time.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, aboutModule));
            });

        // Trade dialogue with additional options and detail
        greeting.AddOption("Do you have anything to trade?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I have wares for those who understand their value. The SkullBottle is something I seek. In return, I can offer you an EnchantedRose, an item of beauty and mystery. I also have a MaxxiaScroll for those who trade, a token of my appreciation.");
                tradeIntroductionModule.AddOption("I have a SkullBottle to trade.",
                    pla => HasSkullBottle(pla) && CanTradeWithPlayer(pla),
                    pla =>
                    {
                        CompleteTrade(pla);
                    });
                tradeIntroductionModule.AddOption("I do not have a SkullBottle right now.", 
                    pla => !HasSkullBottle(pla), 
                    pla =>
                    {
                        pla.SendMessage("Return when you possess a SkullBottle. Only then can we speak of trade.");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                tradeIntroductionModule.AddOption("I recently traded, I'll return later.",
                    pla => !CanTradeWithPlayer(pla),
                    pla =>
                    {
                        pla.SendMessage("The tides of magic must be respected. You can only trade once every 10 minutes. Return when the time is right.");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                tradeIntroductionModule.AddOption("Tell me more about this EnchantedRose.",
                    pla => true,
                    pla =>
                    {
                        DialogueModule roseModule = new DialogueModule("The EnchantedRose is no ordinary flower. It was grown from seeds imbued with the essence of a dryad. It never wilts, and it is said to bring peace to those who hold it, easing pain and granting restful dreams. However, its power is fleeting, for it must be nurtured with magic to maintain its bloom.");
                        roseModule.AddOption("How do you nurture it?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule nurtureModule = new DialogueModule("To nurture the EnchantedRose, you must bathe it in moonlight and whisper words of an ancient incantation—one that speaks to the heart of nature itself. It is a delicate process, but those who succeed will find the rose to be a lifelong companion.");
                                nurtureModule.AddOption("It sounds beautiful.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Mordra smiles softly. 'Beauty is often found in the most delicate of things.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                nurtureModule.AddOption("I do not have the patience for that.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Mordra frowns slightly. 'Patience is a virtue, traveler. But perhaps you will learn one day.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, nurtureModule));
                            });
                        roseModule.AddOption("I would like to trade for it.",
                            plaa => HasSkullBottle(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        pla.SendGump(new DialogueGump(pla, roseModule));
                    });
                tradeIntroductionModule.AddOption("Perhaps another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Mordra.", 
            p => true, 
            p =>
            {
                p.SendMessage("Mordra nods subtly, her eyes glinting with hidden knowledge.");
            });

        return greeting;
    }

    private bool HasSkullBottle(PlayerMobile player)
    {
        // Check the player's inventory for SkullBottle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SkullBottle)) != null;
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
        // Remove the SkullBottle and give the EnchantedRose and MaxxiaScroll, then set the cooldown timer
        Item skullBottle = player.Backpack.FindItemByType(typeof(SkullBottle));
        if (skullBottle != null)
        {
            skullBottle.Delete();
            player.AddToBackpack(new EnchantedRose());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the SkullBottle and receive an EnchantedRose and a MaxxiaScroll in return. Mordra nods approvingly.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a SkullBottle.");
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