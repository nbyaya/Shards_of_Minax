using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class TaliaTheEnchantress : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TaliaTheEnchantress() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Talia the Enchantress";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(353)); // Robe with a dark blue hue
        AddItem(new Sandals(1175)); // Sandals with a light blue hue
        AddItem(new WizardsHat(1153)); // A matching wizard's hat
        AddItem(new GoldBracelet()); // A golden bracelet to add a touch of mysticism
        AddItem(new Spellbook()); // Her spellbook, which can also be stolen

        VirtualArmor = 15;
    }

    public TaliaTheEnchantress(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Talia, an enchantress of forgotten arts. Do you perhaps dabble in the mysteries of baking and old charms?");

        // Start with dialogue about her interests
        greeting.AddOption("Tell me more about your enchantments.",
            p => true,
            p =>
            {
                DialogueModule enchantmentsModule = new DialogueModule("I study ancient relics and spells, especially those tied to domestic arts like baking. You wouldn't believe how many powerful charms are hidden in everyday items.");
                enchantmentsModule.AddOption("What kind of charms?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule charmModule = new DialogueModule("Oh, charms for luck, protection, even love. Many of them were baked into bread or sewn into clothes. But lately, I've been seeking a special BakingBoard, rumored to hold old enchantments.");
                        charmModule.AddOption("Can I help you find it?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, if you bring me a BakingBoard, I shall reward you handsomely with a NixieStatue and a MaxxiaScroll. But I must limit such trades to once every ten minutes.");
                                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                                    plaa => CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        DialogueModule tradeModule = new DialogueModule("Do you have a BakingBoard for me?");
                                        tradeModule.AddOption("Yes, I have a BakingBoard.",
                                            plaaa => HasBakingBoard(plaaa) && CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });
                                        tradeModule.AddOption("No, I don't have one right now.",
                                            plaaa => !HasBakingBoard(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Come back when you have a BakingBoard.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                            plaaa => !CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tradeModule));
                                    });
                                tradeIntroductionModule.AddOption("Perhaps another time.",
                                    plaq => true,
                                    plaq =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        
                        // Additional Nested Dialogue - Exploring Talia's Personality
                        charmModule.AddOption("Why are you so interested in baking charms?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule bakingInterestModule = new DialogueModule("Oh, it's because baking reminds me of simpler times. I grew up in a small village where my mother used to bake bread every morning. She always told me that baking was like casting a spell—putting love and intention into something to nourish others. I've always believed there is magic in that.");
                                bakingInterestModule.AddOption("That sounds beautiful. Did you always want to be an enchantress?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule childhoodModule = new DialogueModule("Not really! As a child, I wanted to be a healer, like my father. He was a medical officer, always helping others. But I soon discovered that my talents lay elsewhere. I may not heal wounds with bandages, but I like to think that my spells and charms can heal in their own way—whether it's someone's spirit or giving them a bit of luck.");
                                        childhoodModule.AddOption("That's very compassionate of you. Do you help a lot of people here?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule helpPeopleModule = new DialogueModule("Oh yes, I try my best! Sometimes travelers come to me with broken hearts, curses, or just in need of a kind word. I may not be able to fix everything, but I think even listening can help. I know there are dangers out there, but I choose to believe that people are good at heart, and I want to do whatever I can to help them.");
                                                helpPeopleModule.AddOption("You're very kind, Talia. Do you ever worry about the dangers of space travel?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule naiveModule = new DialogueModule("Well, I suppose I should... I've heard the stories, of course. Pirates, void creatures, strange magic. But honestly, I think if we all just help each other, things will be okay. Maybe that's naive, but I can't help it. I believe that even in the darkest places, there's light to be found.");
                                                        naiveModule.AddOption("I admire your optimism. It's rare to find such hope.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Talia smiles warmly, her eyes shining with hope.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        naiveModule.AddOption("Be careful, Talia. The universe can be harsher than you think.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Talia nods, her expression growing serious for a moment. 'I will, but I can't stop trying to make it better.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, naiveModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, helpPeopleModule));
                                            });
                                        childhoodModule.AddOption("That's admirable. But isn't it dangerous to work with magic like this?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule dangerModule = new DialogueModule("Sometimes, yes. Magic can be unpredictable, and there are forces out there far stronger than I am. But I think the key is to always use magic with love and good intentions. Besides, I have faith that the good we do will come back to protect us when we need it most.");
                                                dangerModule.AddOption("You truly are brave, Talia.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Talia blushes slightly, waving a hand dismissively. 'Oh, I'm not brave. I just do what I can.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, dangerModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, childhoodModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, bakingInterestModule));
                            });
                        
                        // Option to discuss curiosity about relics
                        charmModule.AddOption("You seem very curious about everything. Where does that come from?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule curiosityModule = new DialogueModule("Oh, I've always been curious! My father used to tell me that curiosity is what keeps us alive. He said that every question we ask leads us to discover new worlds, new ways to help others. I think that's why I became an enchantress. Every spell, every relic has a story, and I love uncovering those stories.");
                                curiosityModule.AddOption("What kind of stories do these relics have?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule relicStoriesModule = new DialogueModule("Some relics are said to be blessed by ancient beings, others were made by people just like us, trying to protect their loved ones. There is one relic I found—a simple silver spoon—that was enchanted by a mother to always stir her child's porridge perfectly, even when she was away. Isn't that beautiful? It shows how magic isn't just about power, it's about love and care.");
                                        relicStoriesModule.AddOption("That really is beautiful. Thank you for sharing.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Talia smiles, her eyes soft. 'Thank you for listening. Not everyone appreciates these little stories.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, relicStoriesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, curiosityModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, charmModule));
                    });
                
                enchantmentsModule.AddOption("Sounds intriguing, but I must go.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, enchantmentsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Talia smiles enigmatically and nods.");
            });

        return greeting;
    }

    private bool HasBakingBoard(PlayerMobile player)
    {
        // Check the player's inventory for BakingBoard
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BakingBoard)) != null;
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
        // Remove the BakingBoard and give the NixieStatue and MaxxiaScroll, then set the cooldown timer
        Item bakingBoard = player.Backpack.FindItemByType(typeof(BakingBoard));
        if (bakingBoard != null)
        {
            bakingBoard.Delete();
            player.AddToBackpack(new NixieStatue());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the BakingBoard and receive a NixieStatue and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a BakingBoard.");
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