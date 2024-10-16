using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AriettaTheWispWhisperer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AriettaTheWispWhisperer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Arietta the Wisp Whisperer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new GoldNecklace()); // A glowing necklace shaped like a wisp
        AddItem(new HoodedShroudOfShadows(1153)); // A shimmery dark blue shroud
        AddItem(new Sandals(1165)); // Mystic blue sandals
        AddItem(new Kilt(293)); // Flowing white skirt
        AddItem(new QuarterStaff()); // A special staff with a glowing crystal

        VirtualArmor = 20;
    }

    public AriettaTheWispWhisperer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Arietta, whisperer to the wisps. These ethereal beings hold secrets untold, and sometimes, they reveal wonders in exchange for peculiar objects. Have you something for me?");

        // Dialogue options
        greeting.AddOption("Tell me about the wisps.",
            p => true,
            p =>
            {
                DialogueModule wispModule = new DialogueModule("Wisps are ancient beings, woven from the threads of magic itself. They share their knowledge only with those they deem worthy. I can communicate with them, but I require special objects to appease their curiosity.");

                // Nested options for wisp information
                wispModule.AddOption("How did you learn to communicate with them?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule learnWispModule = new DialogueModule("It took years of living in the ruins, scavenging what little magic I could find. I learned to listen to the whispers in the air, and eventually, I could understand their language. It wasn't easy—most people would have given up, but I'm not like most people.");

                        // Additional options within this branch
                        learnWispModule.AddOption("Why didn't you give up?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule persistenceModule = new DialogueModule("Because I have hope. Despite everything, I believe we can rebuild. The wisps showed me glimpses of what once was and what could be again. If I can bring back even a small part of that beauty, then all the hardship is worth it.");
                                pla.SendGump(new DialogueGump(pla, persistenceModule));
                            });

                        learnWispModule.AddOption("What do the wisps tell you about the world?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule worldKnowledgeModule = new DialogueModule("They speak of the world before the fall, when magic was plentiful and civilizations thrived. They warn me of dangers too—there are dark forces in the wastelands, and I must stay nimble to avoid them. It's a constant dance of evasion, but one I'm skilled at.");
                                pla.SendGump(new DialogueGump(pla, worldKnowledgeModule));
                            });

                        pl.SendGump(new DialogueGump(pl, learnWispModule));
                    });

                wispModule.AddOption("Do the wisps help you survive?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule survivalModule = new DialogueModule("They do, in their own way. They guide me to hidden caches, warn me of dangers, and sometimes, they grant me small boons. It's not always clear why they help, but I've learned to trust them. My agility and resourcefulness are what keep me alive, but the wisps give me that extra edge.");

                        survivalModule.AddOption("What kind of dangers do you face?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule dangersModule = new DialogueModule("The wastelands are full of dangers—rogue scavengers, mutated beasts, and the remnants of dark magic left behind by those who abused it. I've learned to be quick on my feet, to avoid conflict whenever possible. My dream is to rebuild, not destroy, so I try to evade rather than fight.");
                                pla.SendGump(new DialogueGump(pla, dangersModule));
                            });

                        survivalModule.AddOption("How do you stay so hopeful?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule hopeModule = new DialogueModule("Hope is all I have, really. When you're picking through ruins, finding scraps of what once was, you realize that humanity can endure anything. I want to be part of the rebuilding process—one brick at a time, one small step forward.");
                                pla.SendGump(new DialogueGump(pla, hopeModule));
                            });

                        pl.SendGump(new DialogueGump(pl, survivalModule));
                    });

                wispModule.AddOption("Do you need any materials?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Yes, indeed. If you possess an EvilCandle, the wisps would be willing to share their blessings. In return, you may choose between a DecorativeFishTank or an EssenceOfToad.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an EvilCandle for me?");
                                tradeModule.AddOption("Yes, I have an EvilCandle.",
                                    plaa => HasEvilCandle(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasEvilCandle(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an EvilCandle, and perhaps we can talk.");
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

                p.SendGump(new DialogueGump(p, wispModule));
            });

        greeting.AddOption("Why do you wander the ruins?",
            p => true,
            p =>
            {
                DialogueModule ruinsModule = new DialogueModule("I wander because there's so much still to find. The world before wasn't perfect, but it was full of wonders, and I believe we can bring some of that back. I scavenge not just for myself, but for the dream of a new civilization—one where we learn from the mistakes of the past.");

                ruinsModule.AddOption("Do you think we can truly rebuild?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule rebuildModule = new DialogueModule("I do. We have to believe that, or else what's the point of surviving? Every small piece we salvage, every bit of knowledge we recover, brings us closer to a better future. The wisps have shown me glimpses—it's possible, but it will take people like you and me.");
                        pl.SendGump(new DialogueGump(pl, rebuildModule));
                    });

                ruinsModule.AddOption("What kind of things do you find?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule findModule = new DialogueModule("All sorts of things—strange artifacts, lost books, pieces of machinery that still hum with energy. I once found a music box, still working after all these years. It played the sweetest melody, a reminder of what was lost but also of what could be regained.");
                        pl.SendGump(new DialogueGump(pl, findModule));
                    });

                p.SendGump(new DialogueGump(p, ruinsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Arietta smiles and nods. 'May the wisps guide your path.'");
            });

        return greeting;
    }

    private bool HasEvilCandle(PlayerMobile player)
    {
        // Check the player's inventory for EvilCandle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(EvilCandle)) != null;
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
        // Remove the EvilCandle and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item evilCandle = player.Backpack.FindItemByType(typeof(EvilCandle));
        if (evilCandle != null)
        {
            evilCandle.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for DecorativeFishTank and EssenceOfToad
            rewardChoiceModule.AddOption("DecorativeFishTank", pl => true, pl =>
            {
                pl.AddToBackpack(new DecorativeFishTank());
                pl.SendMessage("You receive a DecorativeFishTank!");
            });

            rewardChoiceModule.AddOption("EssenceOfToad", pl => true, pl =>
            {
                pl.AddToBackpack(new EssenceOfToad());
                pl.SendMessage("You receive an EssenceOfToad!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an EvilCandle.");
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