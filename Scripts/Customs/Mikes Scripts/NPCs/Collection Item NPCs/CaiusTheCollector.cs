using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CaiusTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CaiusTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Caius the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(50);

        Fame = 500;
        Karma = 200;

        // Outfit
        AddItem(new FancyShirt(450)); // Blue fancy shirt
        AddItem(new LongPants(1109)); // Dark grey pants
        AddItem(new Sandals(1175)); // Light green sandals
        AddItem(new Cloak(1157)); // Deep purple cloak
        AddItem(new TricorneHat(1109)); // Dark grey hat

        VirtualArmor = 12;
    }

    public CaiusTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Caius, a collector of rare and curious artifacts. Tell me, do you happen to possess anything... unusual?");

        // Start dialogue about his collection and backstory
        greeting.AddOption("What kind of artifacts do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I have a fascination with relics of old - things long forgotten by most. Items like the Crystal of Lost Memories, the Obsidian Tablet, or even the enigmatic StoneHead have caught my interest. Each holds a story untold, a piece of history lost to the sands of time.");
                collectionModule.AddOption("Tell me about the StoneHead.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule stoneHeadModule = new DialogueModule("Ah, the StoneHead! It is said to be part of an ancient statue that predates even the oldest ruins in the land. Many believe it holds mystical properties, but I haven't yet deciphered its secrets. It speaks to those who listen, but only if you have the patience of ages. Are you intrigued by ancient mysteries, young one?");
                        stoneHeadModule.AddOption("Yes, I am intrigued. Please, tell me more.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule deeperStoneHeadModule = new DialogueModule("The StoneHead is more than just an artifact, it is a key - a key to understanding a civilization long gone. They were a people of wisdom, builders of grand monuments, and scholars of the celestial paths. They revered the balance of nature and the harmony of existence. The StoneHead was rumored to be an oracle, a source of guidance for those who sought its wisdom. But remember, the answers it gives are not always straightforward. One must interpret its meanings with care and thought.");
                                deeperStoneHeadModule.AddOption("What happened to this civilization?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule lostCivilizationModule = new DialogueModule("Their fate is a tragic tale, as are many of the ancient peoples. They grew too ambitious, seeking power over the natural forces they once respected. They built great machines, and in their arrogance, they believed they could control the very essence of life. It was their undoing. The earth itself rose against them, and their cities crumbled. Now, all that remains are fragments - like the StoneHead - scattered across the world, waiting for someone wise enough to learn from their mistakes.");
                                        lostCivilizationModule.AddOption("That is a sobering tale. Thank you for sharing it.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, lostCivilizationModule));
                                    });
                                deeperStoneHeadModule.AddOption("How can one listen to the StoneHead?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule listeningModule = new DialogueModule("Listening to the StoneHead requires more than just your ears. It requires a stillness of the heart, a silence of the mind. It is said that only those who are truly at peace with themselves can hear its whispers. The StoneHead speaks in riddles, in metaphors - it is not a guide for the impatient or the reckless. Many have tried, but few have succeeded in understanding its wisdom.");
                                        listeningModule.AddOption("I see. Thank you for your wisdom.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, listeningModule));
                                    });
                                deeperStoneHeadModule.AddOption("I think I need more time to understand this.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, deeperStoneHeadModule));
                            });
                        stoneHeadModule.AddOption("No, it sounds too mysterious for me.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, stoneHeadModule));
                    });
                collectionModule.AddOption("Tell me about the Crystal of Lost Memories.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule crystalModule = new DialogueModule("The Crystal of Lost Memories... It is said to contain echoes of the past - memories that have been forgotten by the world. Those who peer into the crystal may witness events long past, see the faces of those who have left this world, or even relive moments from their own lives that they have lost to time. It is a powerful object, but not without its dangers.");
                        crystalModule.AddOption("What kind of dangers?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule dangersModule = new DialogueModule("The danger lies in becoming lost in the past. The memories held within the crystal are vivid, more real than any dream. Some who gaze into it find themselves unable to return to the present, caught in the beauty or sorrow of what once was. It is a tool for the wise, but a trap for the unwary. You must always remember that the past is gone - it is the present that needs your attention.");
                                dangersModule.AddOption("I understand. The present is what truly matters.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, dangersModule));
                            });
                        crystalModule.AddOption("Can one learn from these memories?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule learningModule = new DialogueModule("Indeed, there is much to learn from the past. The memories within the crystal can offer insights into forgotten knowledge, ancient techniques, or even the mistakes of those who came before us. But one must approach with caution - knowledge is only as useful as the wisdom with which it is applied. Remember, young one, that understanding the past is a gift, but living in the past is a curse.");
                                learningModule.AddOption("Thank you for your wisdom, Caius.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, learningModule));
                            });
                        crystalModule.AddOption("This is too much for me to handle.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, crystalModule));
                    });
                collectionModule.AddOption("Tell me about the Obsidian Tablet.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tabletModule = new DialogueModule("The Obsidian Tablet is a relic from an age of great scholars. It is covered in ancient runes, a language lost to most, but not to all. The scholars of that age recorded their greatest discoveries, their deepest philosophies, and even their darkest secrets upon such tablets. To possess one is to hold a fragment of a civilization's soul in your hands.");
                        tabletModule.AddOption("Can the runes be deciphered?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule decipherModule = new DialogueModule("Deciphering the runes is no simple task. It requires knowledge of the ancient tongue, an understanding of their culture, and the patience of a thousand suns. I have spent many years studying these runes, and yet there is still much I do not understand. But with dedication and time, the secrets of the Obsidian Tablet can be revealed, and with them, the wisdom of the ancients.");
                                decipherModule.AddOption("I admire your dedication, Caius.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, decipherModule));
                            });
                        tabletModule.AddOption("What kind of secrets are on the tablet?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule secretsModule = new DialogueModule("The secrets on the Obsidian Tablet are varied. Some are scientific, detailing experiments and observations about the natural world. Others are philosophical, exploring the meaning of life and the nature of existence. But there are also darker secrets - tales of forbidden experiments, of power sought and lost, of mistakes that cost entire civilizations. It is a reminder that knowledge, without wisdom, can lead to ruin.");
                                secretsModule.AddOption("A sobering thought. Thank you for sharing.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, secretsModule));
                            });
                        tabletModule.AddOption("Thank you, Caius. I will reflect on this.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tabletModule));
                    });

                // After they’ve learned about the artifacts, introduce the trade option
                collectionModule.AddOption("Is there anything you need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of a StoneHead for my research. In return, I can offer you a BrassFountain and a MaxxiaScroll. However, I must be prudent - I can only afford to make such a trade once every 10 minutes.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a StoneHead for me?");
                                tradeModule.AddOption("Yes, I have a StoneHead.",
                                    plaa => HasStoneHead(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasStoneHead(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a StoneHead. I am eager to examine it!");
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
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Caius waves you off with a knowing smile.");
            });

        return greeting;
    }

    private bool HasStoneHead(PlayerMobile player)
    {
        // Check the player's inventory for StoneHead
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(StoneHead)) != null;
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
        // Remove the StoneHead and give the BrassFountain and MaxxiaScroll, then set the cooldown timer
        Item stoneHead = player.Backpack.FindItemByType(typeof(StoneHead));
        if (stoneHead != null)
        {
            stoneHead.Delete();
            player.AddToBackpack(new BrassFountain());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the StoneHead and receive a BrassFountain and a MaxxiaScroll in return. Caius smiles in satisfaction.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a StoneHead.");
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