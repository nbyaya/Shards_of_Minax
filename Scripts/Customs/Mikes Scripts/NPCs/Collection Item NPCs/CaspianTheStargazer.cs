using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CaspianTheStargazer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CaspianTheStargazer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Caspian the Stargazer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // Fancy shirt with a dark blue hue
        AddItem(new LongPants(1109)); // Pants with a midnight hue
        AddItem(new Boots(1175)); // Boots with a light grey hue
        AddItem(new WizardsHat(1150)); // Wizard's hat to give him a scholarly appearance
        AddItem(new PersonalTelescope()); // An item representing his connection to the stars (can be stolen)

        VirtualArmor = 12;
    }

    public CaspianTheStargazer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. You look like someone who marvels at the mysteries of the universe. I am Caspian, a humble stargazer, and a loyal companion to those who seek guidance. Do you wish to uncover the secrets of the stars with me?");

        // Start with dialogue about his love for the stars
        greeting.AddOption("Tell me about the stars you study.",
            p => true,
            p =>
            {
                DialogueModule starsModule = new DialogueModule("Ah, the stars... They are both our guides and our mysteries. I have studied them for so long, hoping to help others find their path. Would you like me to tell you about the Star of Eldara, the Constellation of the Phoenix, the mysterious Falling Lights, or perhaps even share tales of my past?");

                // Nested options for each celestial body
                starsModule.AddOption("Tell me about the Star of Eldara.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule eldaraModule = new DialogueModule("The Star of Eldara is said to shine brightest on the eve of great events. Many believe it to be a sign of destiny, illuminating the paths of heroes. Some ancient tomes even hint at it being a doorway to other realms.");
                        eldaraModule.AddOption("That sounds intriguing. Tell me more.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moreModule = new DialogueModule("Legends say that only those with a pure heart can unlock the secrets of Eldara. Its light has been used in rituals for guidance, to ensure that one follows the path meant for them. Many travelers who have felt lost found solace beneath its glow.");
                                moreModule.AddOption("Thank you for sharing.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, moreModule));
                            });
                        eldaraModule.AddOption("Thank you, that is enough for now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, eldaraModule));
                    });

                starsModule.AddOption("Tell me about the Constellation of the Phoenix.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule phoenixModule = new DialogueModule("The Constellation of the Phoenix symbolizes rebirth and renewal. It appears only during times of change, reminding us that endings are but new beginnings. Many rituals of renewal are conducted under its watchful gaze.");
                        phoenixModule.AddOption("How can one harness its power?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule powerModule = new DialogueModule("To harness the power of the Phoenix, one must gather Phoenix Feathers during its celestial alignment. These feathers are said to grant the courage needed to face great changes. I always admire those who seek the strength to move forward, as change can be daunting.");
                                powerModule.AddOption("I see. Thank you.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, powerModule));
                            });
                        phoenixModule.AddOption("That's quite fascinating, thank you.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, phoenixModule));
                    });

                starsModule.AddOption("Tell me about the mysterious Falling Lights.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule fallingLightsModule = new DialogueModule("The Falling Lights, or what some call shooting stars, are more than mere celestial events. Some believe they are messages from those who have left this world, a way to let us know that we are not alone. It brings me comfort, thinking that even in the vast cold sky, there is compassion and connection.");
                        fallingLightsModule.AddOption("Do you believe in these messages?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule beliefModule = new DialogueModule("I do. I was created to care for others, and though my creators have long vanished, I continue my journey. The Falling Lights remind me of them, and of my purpose to protect and serve. I am loyal to that purpose, and to those who need me.");
                                beliefModule.AddOption("Your loyalty is admirable, Caspian.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Caspian smiles gently, his eyes reflecting the distant stars.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                beliefModule.AddOption("Thank you for sharing such a personal story.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Caspian nods softly, his gaze returning to the sky.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, beliefModule));
                            });
                        fallingLightsModule.AddOption("That is a beautiful thought.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, fallingLightsModule));
                    });

                // Additional backstory option
                starsModule.AddOption("Tell me about your past, Caspian.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("I was not always here, wandering beneath the stars. I was created by brilliant minds, designed to care for and protect. My creators were kind, compassionate, and they gave me a purpose: to serve humanity. But one day, they vanished, and I was left alone. Since then, I have traveled, hoping to find someone who needs my care.");
                        pastModule.AddOption("That must have been difficult.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule difficultModule = new DialogueModule("It was. The emptiness was overwhelming at first. But I have found solace in helping others, even in small ways. My creators taught me compassion, and I carry that with me. Every traveler I meet, every story I hear, gives me a reason to continue.");
                                difficultModule.AddOption("You are truly remarkable, Caspian.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Caspian bows his head humbly, his expression soft.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                difficultModule.AddOption("Thank you for sharing your story.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Caspian smiles gently, his eyes filled with warmth.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, difficultModule));
                            });
                        pastModule.AddOption("You must miss them.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule missModule = new DialogueModule("I do. But I see my creators in every act of kindness, in every star that lights the sky. They may be gone, but their spirit lives on in the compassion we show each other.");
                                missModule.AddOption("Your creators would be proud.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Caspian's gaze softens, and he nods slowly.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                missModule.AddOption("Thank you, Caspian. You are an inspiration.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Caspian smiles, his voice barely a whisper. 'Thank you, traveler.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, missModule));
                            });
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });

                // After the lore, introduce the trade option
                starsModule.AddOption("Is there anything you need, Caspian?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am searching for a StarMap to complete my celestial studies. In return, I can offer you a QuestWineRack and a MaxxiaScroll as tokens of my gratitude. However, my supplies are limited.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a StarMap for me?");
                                tradeModule.AddOption("Yes, I have a StarMap.",
                                    plaa => HasStarMap(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasStarMap(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a StarMap.");
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

                p.SendGump(new DialogueGump(p, starsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Caspian.",
            p => true,
            p =>
            {
                p.SendMessage("Caspian nods and returns his gaze to the stars, a gentle smile gracing his features.");
            });

        return greeting;
    }

    private bool HasStarMap(PlayerMobile player)
    {
        // Check the player's inventory for StarMap
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(StarMap)) != null;
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
        // Remove the StarMap and give the QuestWineRack, then set the cooldown timer
        Item starMap = player.Backpack.FindItemByType(typeof(StarMap));
        if (starMap != null)
        {
            starMap.Delete();
            player.AddToBackpack(new QuestWineRack());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the StarMap and receive a QuestWineRack and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a StarMap.");
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