using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FarrokhTheWanderer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FarrokhTheWanderer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Farrokh the Wanderer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 100;
        Karma = 100;

        // Outfit
        AddItem(new FancyShirt(553)); // Rich purple fancy shirt
        AddItem(new LongPants(1109)); // Dark blue pants
        AddItem(new Boots(1107)); // Dark leather boots
        AddItem(new Cloak(1157)); // Bright orange cloak
        AddItem(new TricorneHat(1109)); // Stylish dark blue hat
        AddItem(new QuarterStaff()); // A walking staff, Farrokh's trademark

        VirtualArmor = 15;
    }

    public FarrokhTheWanderer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Farrokh, a wanderer of distant lands and even distant worlds. I've seen many wonders, both on this earth and beyond the stars, and gathered a trove of curiosities. Might you be interested in a trade?");

        // Introduce item exchange dialogue
        greeting.AddOption("What kind of trade are you offering?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I am seeking a CandleStick, a humble yet cherished artifact. In return, I can offer you OldBones, along with a special scroll known as the MaxxiaScroll. But remember, I can only trade every ten minutes.");
                
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a CandleStick for me?");
                        tradeModule.AddOption("Yes, I have a CandleStick.",
                            plaa => HasCandleStick(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasCandleStick(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a CandleStick. I shall be waiting.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later, my friend.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                
                tradeIntroductionModule.AddOption("Maybe another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Add more layers of dialogue for a charismatic and adventurous personality
        greeting.AddOption("Tell me about your travels.",
            p => true,
            p =>
            {
                DialogueModule travelsModule = new DialogueModule("Ah, my travels! Where do I even begin? I've wandered across burning deserts, sailed the tumultuous seas, and even ventured into the vast reaches of space itself. Each journey left its mark on me, and I gather stories wherever I go.");
                
                travelsModule.AddOption("Space, you say? Tell me more about that.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule spaceModule = new DialogueModule("Ah, the great black ocean above us! I've stood on the surface of moons where the ground glows beneath your feet, and I've seen stars so bright they rival the sun. Out there, life takes on strange and magnificent forms. Some friendly, some... less so.");
                        
                        spaceModule.AddOption("Did you encounter any creatures out there?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule creaturesModule = new DialogueModule("Oh, indeed I did! There are the gentle Lumivores, creatures that feed on starlight and drift like glowing jellyfish. Then there are the vicious Starwraiths, beings of pure energy that guard ancient relics. Once, I even faced a Starwraith to retrieve an artifact that allowed me to understand alien languages.");
                                
                                creaturesModule.AddOption("That sounds terrifying! How did you survive?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("Ah, it was a combination of luck, skill, and a bit of charm! You see, the Starwraiths are drawn to music. I played a melody on my flute, one I learned from a kind old traveler on Earth, and it soothed the Starwraith long enough for me to escape. Sometimes, it pays to be a little playful even in the face of danger!");
                                        
                                        survivalModule.AddOption("You're quite brave, Farrokh.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Farrokh smiles, a twinkle in his eye. 'Thank you, friend. Bravery is just fear wrapped in determination.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        
                                        plaaa.SendGump(new DialogueGump(plaaa, survivalModule));
                                    });

                                creaturesModule.AddOption("I hope to never encounter one myself.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Farrokh chuckles. 'Wise words, traveler. It is best to be prepared, but not all adventures need to be dangerous.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                
                                plaa.SendGump(new DialogueGump(plaa, creaturesModule));
                            });

                        spaceModule.AddOption("Did you find anything valuable during your space travels?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule treasureModule = new DialogueModule("Ah, treasures beyond your imagination! Not all of them are gold or jewels. Some are knowledge, wisdom from forgotten civilizations. I once found a crystal that holds the memories of an entire species. With it, I learned their songs and stories—some of which I could teach you, if you're interested.");
                                
                                treasureModule.AddOption("I would love to learn one of those songs!",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule songModule = new DialogueModule("Very well! Listen closely, for this is a song of the Celestines, a peaceful people who lived among the stars. The melody is said to bring calm and clarity to those who hear it.");
                                        
                                        songModule.AddOption("Thank you, Farrokh. I feel at peace.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Farrokh nods, pleased. 'Music has a way of reaching the soul, my friend. Carry it with you on your journey.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        
                                        plaaa.SendGump(new DialogueGump(plaaa, songModule));
                                    });

                                treasureModule.AddOption("Maybe another time.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                
                                plaa.SendGump(new DialogueGump(plaa, treasureModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, spaceModule));
                    });

                travelsModule.AddOption("Tell me about your adventures on Earth.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule earthModule = new DialogueModule("Ah, Earth! A place of many wonders, if you know where to look. I've trekked through jungles so dense you could barely see the sky, and climbed mountains that pierce the heavens. Once, I even joined an expedition to find a lost city in the depths of the jungle.");
                        
                        earthModule.AddOption("A lost city? Did you find it?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule cityModule = new DialogueModule("Oh, we did! It was hidden beneath layers of vines and earth, as if nature herself had taken it back. Within its crumbling walls, we found murals depicting ancient stories—stories of gods, heroes, and even travelers from the stars. It made me wonder just how connected all life truly is.");
                                
                                cityModule.AddOption("What did you learn from the murals?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule muralModule = new DialogueModule("The murals told of a hero who was guided by a being from the heavens—a star traveler, much like myself. This hero brought knowledge and peace to their people. It made me realize that, no matter where you come from, the desire to explore and learn unites us all.");
                                        
                                        muralModule.AddOption("That's quite inspiring.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Farrokh smiles warmly. 'Indeed, inspiration can be found in the most unexpected places, if only we dare to look.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        
                                        plaaa.SendGump(new DialogueGump(plaaa, muralModule));
                                    });

                                cityModule.AddOption("I wish I could see it myself.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Farrokh nods. 'Perhaps one day, you shall. The world is full of mysteries, and all it takes is the will to seek them.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                
                                plaa.SendGump(new DialogueGump(plaa, cityModule));
                            });

                        earthModule.AddOption("What else have you discovered on Earth?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule discoveryModule = new DialogueModule("Oh, many things! Rare herbs with healing properties, hidden caves filled with glowing crystals, and even a tribe of people who could communicate with animals. The world is full of wonders, and each day brings a new discovery.");
                                
                                discoveryModule.AddOption("Tell me about the tribe that communicates with animals.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule tribeModule = new DialogueModule("They were a remarkable people, deeply connected to nature. They could call birds to scout the area or ask wolves for protection. It wasn't magic, but a bond built on trust and respect. They taught me that true power comes from understanding and harmony, not domination.");
                                        
                                        tribeModule.AddOption("I wish I could learn from them.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Farrokh nods thoughtfully. 'Perhaps you can, in your own way. The key is to listen, truly listen, to the world around you.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        
                                        plaaa.SendGump(new DialogueGump(plaaa, tribeModule));
                                    });

                                discoveryModule.AddOption("Thank you for sharing, Farrokh.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Farrokh bows slightly. 'The pleasure is mine, traveler. May your journey be filled with wonders.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                
                                plaa.SendGump(new DialogueGump(plaa, discoveryModule));
                            });

                        pl.SendGump(new DialogueGump(pl, earthModule));
                    });

                p.SendGump(new DialogueGump(p, travelsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Farrokh nods and tips his hat gracefully.");
            });

        return greeting;
    }

    private bool HasCandleStick(PlayerMobile player)
    {
        // Check the player's inventory for CandleStick
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CandleStick)) != null;
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
        // Remove the CandleStick and give the OldBones and MaxxiaScroll, then set the cooldown timer
        Item candleStick = player.Backpack.FindItemByType(typeof(CandleStick));
        if (candleStick != null)
        {
            candleStick.Delete();
            player.AddToBackpack(new OldBones());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the CandleStick and receive OldBones and a MaxxiaScroll in return. Safe travels!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a CandleStick.");
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