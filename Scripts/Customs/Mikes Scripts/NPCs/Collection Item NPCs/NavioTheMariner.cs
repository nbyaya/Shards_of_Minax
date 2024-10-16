using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class NavioTheMariner : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NavioTheMariner() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Navio the Mariner";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(80);

        SetHits(90);
        SetMana(100);
        SetStam(70);

        Fame = 100;
        Karma = 50;

        // Outfit
        AddItem(new TricorneHat(2750)); // Dark blue tricorne hat
        AddItem(new FancyShirt(1321)); // White fancy shirt
        AddItem(new LongPants(1109)); // Navy blue pants
        AddItem(new Boots(1109)); // Matching navy blue boots
        AddItem(new Cutlass()); // Wields a cutlass, not usable by him but adds to the look

        VirtualArmor = 15;
    }

    public NavioTheMariner(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy, traveler! I am Navio, a mariner who has sailed the high seas and faced many perils. Do you fancy an adventure or perhaps an exchange of treasures?");

        // Dialogue about his backstory
        greeting.AddOption("Tell me about your adventures.",
            p => true,
            p =>
            {
                DialogueModule adventureModule = new DialogueModule("I've crossed paths with sea monsters, elusive mermaids, and even a Kraken! Aye, it was quite the battle. But let me tell ye, it's not just sea creatures that trouble a mariner. It's the men, the power-hungry, corrupt men on land that are far worse.");
                
                adventureModule.AddOption("What do you mean by corrupt men?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule corruptionModule = new DialogueModule("Once, I was an idealistic man. I thought I could change things, bring about justice. But the truth of it is, power corrupts, and those in high places have long forgotten what it means to serve the people. I've seen bribes change hands, truth buried beneath lies, and innocents made to suffer so that the mighty can stay in control.");
                        corruptionModule.AddOption("That sounds grim. Why do you keep fighting?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule motivationModule = new DialogueModule("Aye, it is grim. But I can't turn away, not now. Once you've seen the ugliness behind the curtain, you can't unsee it. I've been relentless in exposing the truth, even if it means risking my own life. Some call it bravery, but I'd call it stubbornness. I'm too far gone to stop now.");
                                
                                motivationModule.AddOption("You must have some stories to tell.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule storyModule = new DialogueModule("Stories? Aye, I've plenty. There was a time I infiltrated a governor's ball disguised as a deckhand. They were all there, laughing, drinking fine wine, while the poor suffered in the streets. I overheard them discussing plans to raise taxes to fund their lavish lifestyles. I barely escaped with my life after they discovered I wasn't who I claimed to be.");
                                        storyModule.AddOption("How did you escape?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule escapeModule = new DialogueModule("It was a close call. I had to dive out of a window into the bay below. The waters were freezing, and the guards were relentless in their pursuit. I swam for hours until I reached a fisherman's boat who hid me beneath his nets. I owe that old man my life.");
                                                
                                                escapeModule.AddOption("That sounds terrifying.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule terrifyingModule = new DialogueModule("Terrifying? Aye, but it wasn't the worst of it. The sea itself can be just as merciless. There was another time when my ship was caught in a storm. The waves were as tall as mountains, and the wind howled like a thousand banshees. We lost half the crew that night. But that's the life of a mariner—danger on every side, whether it be man, beast, or nature.");
                                                        terrifyingModule.AddOption("Why do you keep going back to the sea, then?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule seaLoveModule = new DialogueModule("The sea is freedom, lad. On land, there's corruption, greed, and deceit. But on the open waters, it's just you, the ship, and the horizon. The sea doesn't lie; it doesn't pretend to be something it's not. It's fierce, but it's honest. And for a man like me, that's all I can ask for.");
                                                                seaLoveModule.AddOption("It sounds like you love the sea, despite everything.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Navio gives you a wistful smile, his eyes seeming to look far beyond the horizon.");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, seaLoveModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, terrifyingModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, escapeModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, storyModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, motivationModule));
                            });
                        pl.SendGump(new DialogueGump(pl, corruptionModule));
                    });
                
                adventureModule.AddOption("What about the Kraken? Tell me more.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule krakenModule = new DialogueModule("The Kraken... a beast unlike any other. It lurks in the deepest waters, waiting for ships that stray too far from safe harbors. The first time I saw it, its massive tentacles rose from the depths, wrapping around the ship like it was nothing more than a toy. We fought hard, hacking at its limbs, but it wasn't bravery that saved us. It was sheer, dumb luck and a sudden storm that distracted the beast.");
                        krakenModule.AddOption("How did you survive that encounter?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule survivalModule = new DialogueModule("We were tossed about like driftwood in the storm. The mast cracked, the sails torn to shreds. But somehow, we held on. The Kraken, confused by the chaos, slipped back into the depths. We were battered, bruised, but alive. And let me tell you, facing a Kraken changes a man. You come face to face with your own mortality, and you realize just how small you really are in this world.");
                                survivalModule.AddOption("Would you face the Kraken again?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule faceKrakenModule = new DialogueModule("Face it again? Aye, I would. Not because I'm brave, but because I have no choice. The sea is my life, and the Kraken is part of it. You can't fear what you can't avoid. You face it, you fight it, and you hope the sea shows you mercy.");
                                        faceKrakenModule.AddOption("You're a braver man than I.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Navio chuckles, though there's a hint of sadness in his eyes. 'Brave, or foolish? Sometimes, I can't tell the difference.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, faceKrakenModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, survivalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, krakenModule));
                    });

                adventureModule.AddOption("Is there anything you need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I'm in need of a KrakenTrophy. If ye have one, I'll give ye a ZttyCrystal in return. I'll also throw in a MaxxiaScroll as a bonus.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a KrakenTrophy for me?");
                                tradeModule.AddOption("Yes, I have a KrakenTrophy.",
                                    plaa => HasKrakenTrophy(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasKrakenTrophy(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a KrakenTrophy.");
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

                p.SendGump(new DialogueGump(p, adventureModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Navio nods and returns to gazing at the horizon, a mix of longing and resolve in his eyes.");
            });

        return greeting;
    }

    private bool HasKrakenTrophy(PlayerMobile player)
    {
        // Check the player's inventory for KrakenTrophy
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(KrakenTrophy)) != null;
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
        // Remove the KrakenTrophy and give the ZttyCrystal and MaxxiaScroll, then set the cooldown timer
        Item krakenTrophy = player.Backpack.FindItemByType(typeof(KrakenTrophy));
        if (krakenTrophy != null)
        {
            krakenTrophy.Delete();
            player.AddToBackpack(new ZttyCrystal());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the KrakenTrophy and receive a ZttyCrystal and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a KrakenTrophy.");
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