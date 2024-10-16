using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MystanTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MystanTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mystan the Alchemist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1157)); // Dark blue robe
        AddItem(new Sandals(1175)); // Light grey sandals
        AddItem(new WizardsHat(1109)); // Black wizard's hat
        AddItem(new GoldNecklace()); // A gold necklace to signify his status

        VirtualArmor = 15;
    }

    public MystanTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Mystan, a humble alchemist in search of rare components. Are you here to learn the secrets of alchemy or perhaps to make a trade?");

        // Start with dialogue about his work
        greeting.AddOption("Tell me about alchemy.",
            p => true,
            p =>
            {
                DialogueModule alchemyModule = new DialogueModule("Alchemy is the art of transmutation, the blending of ingredients to produce something greater. I study rare and peculiar components, always on the hunt for new discoveries.");
                alchemyModule.AddOption("What rare components do you seek?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule componentsModule = new DialogueModule("Lately, I've been in search of RareGrease, an elusive and potent substance. If you happen to come across it, I would gladly offer you something special in return.");
                        componentsModule.AddOption("What will you give me for RareGrease?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeIntroModule = new DialogueModule("For RareGrease, I can offer you a DistillationFlask, a crucial tool for any alchemist. And, as a token of my gratitude, I will also provide you with a MaxxiaScroll.");
                                tradeIntroModule.AddOption("I have RareGrease to trade.",
                                    plaa => HasRareGrease(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeIntroModule.AddOption("I don't have RareGrease right now.",
                                    plaa => !HasRareGrease(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have RareGrease. I shall be waiting.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeIntroModule.AddOption("I traded recently; I'll return later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroModule));
                            });
                        componentsModule.AddOption("Perhaps another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, componentsModule));
                    });

                alchemyModule.AddOption("What are your experiments about?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule experimentModule = new DialogueModule("My experiments? Ah, you ask the right questions, traveler. I have been studying the limits of life and death, the transformation of matter, and even the power to cure ailments that no other healer dares to attempt. Of course, my methods are... unconventional.");
                        experimentModule.AddOption("Unconventional? How so?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule unconventionalModule = new DialogueModule("Well, some might say that I am willing to go too far. You see, to truly understand alchemy, one must push boundaries. If a few lives must be sacrificed to find a cure that saves many, isn't it a price worth paying?");
                                unconventionalModule.AddOption("That sounds dangerous... and unethical.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule ethicsModule = new DialogueModule("Ethics, you say? Ethics are a limitation, a chain that binds the hands of those who could achieve greatness. I do not seek to harm the innocent, but I will not shy away from the hard choices. The pursuit of knowledge demands sacrifice.");
                                        ethicsModule.AddOption("I don't agree with your methods.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Mystan frowns, but nods. 'I understand, many do not see things my way. Perhaps you will, in time.'");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        ethicsModule.AddOption("I suppose sacrifices are sometimes necessary...",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Mystan's eyes light up with a fervent gleam. 'Yes, exactly! You understand! Only through perseverance and an unwavering will can we achieve the impossible.'");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, ethicsModule));
                                    });
                                unconventionalModule.AddOption("What have you discovered so far?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule discoveriesModule = new DialogueModule("Ah, the discoveries! I have concocted elixirs that can extend life, potions that heal the gravest of wounds, and tinctures that can grant visions beyond the mortal realm. But I am still seeking the ultimate cure - the elixir that can heal any affliction, no matter how dire.");
                                        discoveriesModule.AddOption("That sounds incredible. Can I help?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Mystan nods, his expression a mix of excitement and obsession. 'Yes, yes! There are many ingredients I still need. RareGrease is but one of them. There are others - ShadowBloom, Dragon's Tears, and the Heart of the Withered. If you bring me these, I can push my work even further.'");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        discoveriesModule.AddOption("It sounds dangerous. I'll leave it to you.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Mystan smiles knowingly. 'Not everyone has the stomach for this kind of work. But fear not, I shall continue in my endeavors.'");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, discoveriesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, unconventionalModule));
                            });
                        experimentModule.AddOption("Goodbye.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Mystan nods, a glint of obsession still in his eyes.");
                            });
                        pl.SendGump(new DialogueGump(pl, experimentModule));
                    });
                alchemyModule.AddOption("Who taught you alchemy?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule mentorModule = new DialogueModule("I had many teachers, but none were quite like my mentor, Alrath the Insatiable. He was a visionary, a man who believed that alchemy was the answer to all of life's ailments. He taught me that only through complete devotion could one uncover true power.");
                        mentorModule.AddOption("Where is Alrath now?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule fateModule = new DialogueModule("Alrath? He... is no longer with us. In his quest for immortality, he experimented on himself. He believed he could replace his mortal flesh with something... eternal. The last I saw of him, he had become something neither alive nor dead. A tragic fate, but a testament to his dedication.");
                                fateModule.AddOption("That sounds terrifying.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Mystan nods solemnly. 'It is. But sometimes, greatness demands that we face the terrors of the unknown.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                fateModule.AddOption("He sounds like a true visionary.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Mystan smiles, his eyes filled with admiration. 'Indeed, he was. I strive to continue his work, though I hope to avoid his... mistakes.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, fateModule));
                            });
                        mentorModule.AddOption("Goodbye.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Mystan gives you a knowing look. 'Farewell, traveler. Remember, true knowledge demands sacrifice.'");
                            });
                        pl.SendGump(new DialogueGump(pl, mentorModule));
                    });
                alchemyModule.AddOption("Goodbye.",
                    pla => true,
                    pla =>
                    {
                        pla.SendMessage("Mystan nods sagely.");
                    });
                p.SendGump(new DialogueGump(p, alchemyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Mystan waves you off with a knowing smile.");
            });

        return greeting;
    }

    private bool HasRareGrease(PlayerMobile player)
    {
        // Check the player's inventory for RareGrease
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RareGrease)) != null;
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
        // Remove the RareGrease and give the DistillationFlask and MaxxiaScroll, then set the cooldown timer
        Item rareGrease = player.Backpack.FindItemByType(typeof(RareGrease));
        if (rareGrease != null)
        {
            rareGrease.Delete();
            player.AddToBackpack(new DistillationFlask());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the RareGrease and receive a DistillationFlask and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have RareGrease.");
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