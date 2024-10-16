using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZelaraTheWebWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZelaraTheWebWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zelara the Web Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(90);
        SetMana(180);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1509)); // Dark, mysterious cloak
        AddItem(new Boots(1409)); // Dark boots
        AddItem(new BookTwentyfive()); // Unique bracelet item that could be custom added
        AddItem(new BookTwentyfive()); // A belt that seems to shimmer like spider silk
        AddItem(new MaxxiaScroll()); // Her reward item is also in her inventory, so it can be stolen

        VirtualArmor = 15;
    }

    public ZelaraTheWebWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a curious soul approaches! I am Zelara, a weaver of tales and secrets, drawn to the mystical threads that bind this world together. Are you intrigued by the mysteries of the SpiderTree?");

        // Start with dialogue about her backstory
        greeting.AddOption("Who are you, Zelara?", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("I was once a simple herbalist, but my path changed when I discovered the secrets of the SpiderTree. Its web-like branches spoke to me, revealing hidden truths and forgotten lore. Since then, I've wandered, seeking those who share my fascination with the arcane and the strange. Each day I see new visions, some wondrous, others horrifying.");
                storyModule.AddOption("What kind of visions?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule visionsModule = new DialogueModule("Ah, the visions... They are not always kind. My brush captures them on canvas, twisted shapes, creeping horrors, eyes that see too much and mouths that speak in silence. Each painting I create draws me deeper into the nightmare. It is as if the SpiderTree weaves itself into my very thoughts, whispering, taunting, guiding my hand.");
                        visionsModule.AddOption("That sounds terrifying. Why do you keep painting?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule reasonModule = new DialogueModule("Terrifying? Yes. But there is beauty in terror, don't you see? Each stroke of my brush pulls the horror out from within me, onto the canvas. It is a madness I cannot escape, yet I cannot stop. The colors, the lines, the shadows—they speak to me. The only way to silence them, even for a moment, is to give them form.");
                                reasonModule.AddOption("What do these paintings mean?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule meaningModule = new DialogueModule("Meaning? Hah! They mean whatever they must. Some see nightmares, others see reflections of themselves. I once painted a tree—its roots dug into the earth, its branches reaching the sky, but within it were trapped faces—faces of those I knew, faces of strangers. They were screaming, and yet... they were smiling too. What does it mean? Perhaps it is my madness. Perhaps it is the truth beneath this world's fragile surface.");
                                        meaningModule.AddOption("Do you ever wish to stop?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule stopModule = new DialogueModule("Stop? Stopping would mean surrendering to the darkness completely. The SpiderTree, the threads of fate—it would consume me whole. No, I must paint. I must weave these visions into something tangible, even if it costs me my sanity. It is the only way I know to survive.");
                                                stopModule.AddOption("That's tragic. I hope you find peace someday.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Zelara looks at you with a hollow smile, her eyes haunted yet grateful.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, stopModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, meaningModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, reasonModule));
                            });
                        visionsModule.AddOption("It must be difficult to bear such visions.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule difficultModule = new DialogueModule("It is. There are nights I do not sleep, for the dreams are worse than the visions I paint. I hear voices—sometimes gentle, sometimes screaming. But this is my burden to bear. The SpiderTree chose me, and I cannot deny its call. Perhaps it is a punishment, or perhaps it is a gift. I am not sure anymore.");
                                difficultModule.AddOption("You have my sympathy, Zelara.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Zelara gives you a weary nod, her fingers trembling slightly.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, difficultModule));
                            });
                        pl.SendGump(new DialogueGump(pl, visionsModule));
                    });
                storyModule.AddOption("Tell me about the SpiderTree.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule spiderTreeModule = new DialogueModule("The SpiderTree is a rare and mystical plant, its branches resembling the delicate threads of a spider's web. It is said to be both a blessing and a curse, for those who can unravel its secrets may gain powerful knowledge, but its guardians—vengeful spirits—are never far. Its whispers haunt my dreams, its essence runs through my veins. Do you dare to know more?");
                        spiderTreeModule.AddOption("Can you use it for something?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule useModule = new DialogueModule("Indeed, I seek the SpiderTree's essence for my research. It can be distilled, twisted, molded into potions or paints. It is a substance like no other—alive, and yet not. If you bring me a piece of it, I can offer you something in return, something drawn from the depths of my visions.");
                                useModule.AddOption("Is there a reward for the SpiderTree?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule rewardModule = new DialogueModule("If you bring me a SpiderTree item, I shall offer you a BookTwentyfive and a MaxxiaScroll. However, I can only trade with each person once every 10 minutes, as the essence takes time to prepare, and my mind must rest between each ritual. The SpiderTree demands patience.");
                                        rewardModule.AddOption("I'd like to make the trade.", 
                                            plaaa => CanTradeWithPlayer(plaaa), 
                                            plaaa =>
                                            {
                                                DialogueModule tradeModule = new DialogueModule("Do you have a SpiderTree for me?");
                                                tradeModule.AddOption("Yes, I have a SpiderTree.", 
                                                    plaaaa => HasSpiderTree(plaaaa) && CanTradeWithPlayer(plaaaa), 
                                                    plaaaa =>
                                                    {
                                                        CompleteTrade(plaaaa);
                                                    });
                                                tradeModule.AddOption("No, I don't have one right now.", 
                                                    plaaaa => !HasSpiderTree(plaaaa), 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Come back when you have a SpiderTree item.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                    plaaaa => !CanTradeWithPlayer(plaaaa), 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, tradeModule));
                                            });
                                        rewardModule.AddOption("Perhaps another time.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, rewardModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, useModule));
                            });
                        pl.SendGump(new DialogueGump(pl, spiderTreeModule));
                    });
                storyModule.AddOption("Goodbye.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("Zelara nods knowingly, her eyes reflecting a mix of exhaustion and fierce determination.");
                    });
                p.SendGump(new DialogueGump(p, storyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zelara smiles mysteriously, her fingers brushing against the silk threads at her waist.");
            });

        return greeting;
    }

    private bool HasSpiderTree(PlayerMobile player)
    {
        // Check the player's inventory for SpiderTree
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SpiderTree)) != null;
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
        // Remove the SpiderTree and give the rewards, then set the cooldown timer
        Item spiderTree = player.Backpack.FindItemByType(typeof(SpiderTree));
        if (spiderTree != null)
        {
            spiderTree.Delete();
            player.AddToBackpack(new BookTwentyfive());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the SpiderTree and receive a BookTwentyfive and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a SpiderTree item.");
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