using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class AriannaTheCartographer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AriannaTheCartographer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Arianna the Cartographer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(60);
        SetMana(200);
        SetStam(50);

        Fame = 100;
        Karma = 200;

        // Outfit
        AddItem(new FancyShirt(1153)); // Light blue fancy shirt
        AddItem(new LongPants(1109)); // Dark blue pants
        AddItem(new Boots(1175)); // Light brown boots
        AddItem(new FeatheredHat(1150)); // A hat with a feather, light yellow
        AddItem(new Lantern()); // A lantern to give her a traveler appearance

        VirtualArmor = 15;
    }

    public AriannaTheCartographer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Arianna, a cartographer and collector of ancient maps. Do you have an interest in exploration and trade?");

        // Dialogue about her work
        greeting.AddOption("Tell me about your cartography work.", 
            p => true, 
            p =>
            {
                DialogueModule cartographyModule = new DialogueModule("I traverse the lands, mapping every nook and cranny, from the bustling cities to the forgotten ruins. Every map tells a story, and I take pride in recording them. But my story doesn’t start with maps; it started on a farm in the wasteland.");
                cartographyModule.AddOption("Tell me about your life as a farmer.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule farmingModule = new DialogueModule("Before I became a cartographer, I was a farmer in the wasteland. It was a harsh life, but I grew crops using old-world techniques. I had always hoped for a peaceful life, growing food and caring for the land, but the wasteland is unforgiving.");
                        farmingModule.AddOption("What kind of crops did you grow?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule cropsModule = new DialogueModule("I grew hearty crops that could withstand the wasteland's harsh conditions. Corn, potatoes, and a few rare herbs that were passed down through generations. The soil was never easy to work with, but I nurtured it, pouring all my effort into making it fertile again.");
                                cropsModule.AddOption("That sounds difficult. How did you manage it?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule manageModule = new DialogueModule("It was indeed hard work. Every day, I would tend to the crops from sunrise to sunset, making sure they had enough water and that the pests stayed away. I used old-world knowledge, things my grandparents taught me. But more than that, I think it was my love for the land that kept me going.");
                                        manageModule.AddOption("Your love for the land is admirable.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule admirationModule = new DialogueModule("Thank you. I’ve always believed that if you take care of the earth, it will take care of you. I may be a bit naive, but I still hold on to that dream of a peaceful life. I think that’s why I took up cartography—to find a place where I could finally settle down without the constant threat of raiders.");
                                                admirationModule.AddOption("Raiders? Were they a frequent problem?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule raidersModule = new DialogueModule("Raiders were always a threat. They would come out of nowhere, demanding food or supplies. Sometimes, they would destroy everything just because they could. It was heartbreaking to see all my hard work trampled. I lost friends, neighbors... Eventually, I knew I had to leave if I wanted to survive.");
                                                        raidersModule.AddOption("That sounds terrible. How did you escape?", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule escapeModule = new DialogueModule("One night, I gathered what I could carry—some food, my maps, and a few precious seeds. I slipped away under the cover of darkness, following the old trails I knew so well. It wasn’t easy; the wasteland is full of dangers, but my maps guided me to safer places. It was then I realized that mapping the land could help others too, not just me.");
                                                                escapeModule.AddOption("That must have been a difficult choice.", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule difficultChoiceModule = new DialogueModule("It was. Leaving behind everything I had worked for was heartbreaking. But I knew that if I stayed, there would be nothing left to save. My dream of a peaceful life still keeps me going, even now as I travel and map the world. One day, I hope to find a place where raiders can’t reach and where people can live without fear.");
                                                                        difficultChoiceModule.AddOption("I hope you find that place someday.", 
                                                                            plaaaaaaa => true, 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                DialogueModule hopeModule = new DialogueModule("Thank you, traveler. I truly appreciate your kind words. It’s why I continue to map the world—to help others who dream of a better future. Maybe one day, we will all find a place where we belong, free from the fears of the wasteland.");
                                                                                hopeModule.AddOption("Tell me more about your maps.", 
                                                                                    plaaaaaaaa => true, 
                                                                                    plaaaaaaaa =>
                                                                                    {
                                                                                        DialogueModule mapsModule = new DialogueModule("My maps are more than just ink on parchment. They are the culmination of my journeys, my struggles, and my hope. Every landmark, every hidden grove, every dangerous path is marked so others can learn from my travels. I map not just for myself, but for all those who come after me, so they can avoid the dangers and find safe places to rest.");
                                                                                        mapsModule.AddOption("That is noble of you, Arianna.", 
                                                                                            plaaaaaaaaa => true, 
                                                                                            plaaaaaaaaa =>
                                                                                            {
                                                                                                DialogueModule nobleModule = new DialogueModule("Thank you. I suppose I am a bit naive to think I can make a difference, but if my maps help even one person, it will be worth it. The world is harsh, but it’s also beautiful in its own way. I want people to see both—to understand the dangers, but also to find the beauty that still exists.");
                                                                                                nobleModule.AddOption("Is there anything you need for your maps?", 
                                                                                                    plaaaaaaaaaa => true, 
                                                                                                    plaaaaaaaaaa =>
                                                                                                    {
                                                                                                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am searching for a CartographyDesk to help me document my findings more effectively. In exchange, I can offer you a LargeVat, and I will always add a MaxxiaScroll as an extra reward. I must warn you, I can only make this trade once every ten minutes per person.");
                                                                                                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                                                                            plaq => CanTradeWithPlayer(pla), 
                                                                                                            plaq =>
                                                                                                            {
                                                                                                                DialogueModule tradeModule = new DialogueModule("Do you have a CartographyDesk for me?");
                                                                                                                tradeModule.AddOption("Yes, I have a CartographyDesk.", 
                                                                                                                    plaaw => HasCartographyDesk(plaa) && CanTradeWithPlayer(plaa), 
                                                                                                                    plaaw =>
                                                                                                                    {
                                                                                                                        CompleteTrade(plaa);
                                                                                                                    });
                                                                                                                tradeModule.AddOption("No, I don't have one right now.", 
                                                                                                                    plaae => !HasCartographyDesk(plaa), 
                                                                                                                    plaae =>
                                                                                                                    {
                                                                                                                        plaa.SendMessage("Come back when you have a CartographyDesk.");
                                                                                                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                                                                    });
                                                                                                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                                                                    plaar => !CanTradeWithPlayer(plaa), 
                                                                                                                    plaar =>
                                                                                                                    {
                                                                                                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                                                                    });
                                                                                                                pla.SendGump(new DialogueGump(pla, tradeModule));
                                                                                                            });
                                                                                                        tradeIntroductionModule.AddOption("Perhaps another time.", 
                                                                                                            plat => true, 
                                                                                                            plat =>
                                                                                                            {
                                                                                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                                                                                                            });
                                                                                                        plaaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaaa, tradeIntroductionModule));
                                                                                                    });
                                                                                                nobleModule.AddOption("Goodbye for now.", 
                                                                                                    plaaaaaaaaaa => true, 
                                                                                                    plaaaaaaaaaa =>
                                                                                                    {
                                                                                                        plaaaaaaaaaa.SendMessage("Arianna nods at you, her eyes still scanning her maps.");
                                                                                                    });
                                                                                                plaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaa, nobleModule));
                                                                                            });
                                                                                        plaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaa, mapsModule));
                                                                                    });
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, hopeModule));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, difficultChoiceModule));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, escapeModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, raidersModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, admirationModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, manageModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, cropsModule));
                            });
                        farmingModule.AddOption("It must have been hard work.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule hardWorkModule = new DialogueModule("It was. Farming in the wasteland isn’t for the faint of heart. Every day was a battle against the elements, pests, and raiders. But despite it all, there was something deeply satisfying about watching the crops grow. It made me believe that even in a harsh world, life could still thrive.");
                                hardWorkModule.AddOption("You must be very hardworking.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule hardworkingModule = new DialogueModule("Thank you. I like to think that hard work is the only way to make a difference. I’ve never been the strongest or the smartest, but I’ve always given everything my best effort. It’s what led me to leave the farm and take up cartography. I wanted to find a place where my hard work would be rewarded with peace.");
                                        hardworkingModule.AddOption("I hope you find that peace someday.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Arianna smiles warmly, her eyes glistening with hope.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, hardworkingModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, hardWorkModule));
                            });
                        pl.SendGump(new DialogueGump(pl, farmingModule));
                    });
                p.SendGump(new DialogueGump(p, cartographyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Arianna nods at you, her eyes still scanning her maps.");
            });

        return greeting;
    }

    private bool HasCartographyDesk(PlayerMobile player)
    {
        // Check the player's inventory for CartographyDesk
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CartographyDesk)) != null;
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
        // Remove the CartographyDesk and give the LargeVat and MaxxiaScroll, then set the cooldown timer
        Item cartographyDesk = player.Backpack.FindItemByType(typeof(CartographyDesk));
        if (cartographyDesk != null)
        {
            cartographyDesk.Delete();
            player.AddToBackpack(new LargeVat());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the CartographyDesk and receive a LargeVat and a MaxxiaScroll in return. Thank you for aiding my research!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a CartographyDesk.");
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