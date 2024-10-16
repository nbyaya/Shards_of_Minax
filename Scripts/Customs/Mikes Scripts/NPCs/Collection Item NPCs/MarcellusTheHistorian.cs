using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MarcellusTheHistorian : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MarcellusTheHistorian() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marcellus the Historian";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2406)); // A dark red robe
        AddItem(new Sandals(1153)); // Black sandals
        AddItem(new FeatheredHat(1153)); // Black feathered hat
        AddItem(new LongPants(2406)); // Dark red pants
        AddItem(new Lantern()); // Lantern for a scholarly look

        VirtualArmor = 12;
    }

    public MarcellusTheHistorian(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Marcellus, a seeker of forgotten tales, ancient relics, and secrets best left undisturbed. Have you come across any artifacts in your journeys?");

        // Dialogue options
        greeting.AddOption("Tell me about your research.",
            p => true,
            p =>
            {
                DialogueModule researchModule = new DialogueModule("I study artifacts that contain stories of old. Each relic has a history, often dark and twisted, and I strive to uncover their mysteries. Sometimes, I find myself lost in the whispers that emanate from the silver, particularly my own creations.");

                researchModule.AddOption("Your creations?", 
                    pl => true,
                    pl =>
                    {
                        DialogueModule creationsModule = new DialogueModule("Yes, I am also a silversmith. Not just any silversmith, mind you, but one who practices a craft that intertwines magic with silver. The pieces I forge are not just metal; they are vessels, each capable of holding something... more. I have become somewhat obsessed with the idea of capturing the essence of a soul in my silver. They whisper secrets to me, secrets that only I can hear.");

                        creationsModule.AddOption("Isn't that dangerous?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule dangerModule = new DialogueModule("Dangerous, yes. But therein lies the allure, does it not? There is an exquisite beauty in control—containing chaos within something as delicate as silver. The souls are restless, and their whispers are incessant, but it is that very darkness that gives my creations their power.");

                                dangerModule.AddOption("What kind of power?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule powerModule = new DialogueModule("The kind of power that draws the desperate and the curious alike. Each piece grants insight into the unknown, knowledge of things forgotten, and sometimes... a fleeting glimpse of the beyond. However, not all who seek such power are prepared for the price they must pay.");

                                        powerModule.AddOption("What is the price?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule priceModule = new DialogueModule("The price is not always something you can count in coins, traveler. Sometimes it is a fragment of one's sanity, other times, a sliver of one's own soul. The silver takes what it needs, and often, it is more than the owner expects. Few are truly prepared for the bond they form with my creations.");

                                                priceModule.AddOption("Tell me more about the artifacts you seek.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule artifactModule = new DialogueModule("Ah, yes, the artifacts. I seek those that have stories still untold, those that may hold lost knowledge or secrets that could enhance my craft. The WeddingChest, for instance, is one such item. It holds a tale of love, betrayal, and power—perfect for my needs.");

                                                        artifactModule.AddOption("Do you need a WeddingChest?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you possess a WeddingChest, I can offer you something truly special in return. You may choose between an ExoticShipInABottle or a FancySewingMachine, each imbued with their own unique properties.");
                                                                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                                                                    plb => CanTradeWithPlayer(plb),
                                                                    plb =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a WeddingChest for me?");
                                                                        tradeModule.AddOption("Yes, I have a WeddingChest.",
                                                                            plbb => HasWeddingChest(plbb) && CanTradeWithPlayer(plbb),
                                                                            plbb =>
                                                                            {
                                                                                CompleteTrade(plbb);
                                                                            });
                                                                        tradeModule.AddOption("No, I don't have one right now.",
                                                                            plbb => !HasWeddingChest(plbb),
                                                                            plbb =>
                                                                            {
                                                                                plbb.SendMessage("Come back when you have a WeddingChest.");
                                                                                plbb.SendGump(new DialogueGump(plbb, CreateGreetingModule(plbb)));
                                                                            });
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                                                            plbb => !CanTradeWithPlayer(plbb),
                                                                            plbb =>
                                                                            {
                                                                                plbb.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plbb.SendGump(new DialogueGump(plbb, CreateGreetingModule(plbb)));
                                                                            });
                                                                        plb.SendGump(new DialogueGump(plb, tradeModule));
                                                                    });
                                                                tradeIntroductionModule.AddOption("Maybe another time.",
                                                                    plb => true,
                                                                    plb =>
                                                                    {
                                                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule(plb)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, tradeIntroductionModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, artifactModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, priceModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, powerModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, dangerModule));
                            });
                        pl.SendGump(new DialogueGump(pl, creationsModule));
                    });
                researchModule.AddOption("Do you need any specific artifacts?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you possess a WeddingChest, I can offer you something truly special in return. You may choose between an ExoticShipInABottle or a FancySewingMachine.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a WeddingChest for me?");
                                tradeModule.AddOption("Yes, I have a WeddingChest.",
                                    plaa => HasWeddingChest(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasWeddingChest(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a WeddingChest.");
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

                p.SendGump(new DialogueGump(p, researchModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Marcellus nods respectfully, lost again in his thoughts of ancient times and silver-bound secrets.");
            });

        return greeting;
    }

    private bool HasWeddingChest(PlayerMobile player)
    {
        // Check the player's inventory for WeddingChest
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WeddingChest)) != null;
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
        // Remove the WeddingChest and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item weddingChest = player.Backpack.FindItemByType(typeof(WeddingChest));
        if (weddingChest != null)
        {
            weddingChest.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ExoticShipInABottle and FancySewingMachine
            rewardChoiceModule.AddOption("ExoticShipInABottle", pl => true, pl =>
            {
                pl.AddToBackpack(new ExoticShipInABottle());
                pl.SendMessage("You receive an ExoticShipInABottle!");
            });

            rewardChoiceModule.AddOption("FancySewingMachine", pl => true, pl =>
            {
                pl.AddToBackpack(new FancySewingMachine());
                pl.SendMessage("You receive a FancySewingMachine!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a WeddingChest.");
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