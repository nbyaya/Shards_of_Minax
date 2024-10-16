using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class CaptainViridian : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CaptainViridian() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Captain Viridian, the Treasure Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(60);

        Fame = 100;
        Karma = 100;

        // Outfit
        AddItem(new TricorneHat(2125)); // A green pirate hat
        AddItem(new LongPants(2213)); // Dark sea-green pants
        AddItem(new Boots(1109)); // Black boots
        AddItem(new FancyShirt(2124)); // A bright teal shirt
        AddItem(new Cutlass()); // A decorative sword

        VirtualArmor = 15;
    }

    public CaptainViridian(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy there, traveler! I am Captain Viridian, seeker of treasures and secrets. Have ye stumbled upon anything valuable during your voyages?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your adventures.", 
            p => true, 
            p =>
            {
                DialogueModule adventureModule = new DialogueModule("I've sailed through tempests, fought sea monsters, and found treasures in the most unlikely places. But what good is treasure without sharing a little?");
                
                // Nested dialogues with additional detail
                adventureModule.AddOption("Tell me more about the sea monsters.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule seaMonsterModule = new DialogueModule("Ah, the sea monsters... mighty beasts they are! There was once a kraken that nearly dragged me and my crew into the abyss. We fought for hours, slashing at its tentacles as it roared and thrashed. The secret to defeating a kraken, lad, is to aim for the eyes!");
                        seaMonsterModule.AddOption("Did you manage to defeat it?", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule krakenVictoryModule = new DialogueModule("Aye, we did! But it wasn't easy. We lost half the crew, and the ship was in tatters. But from its corpse, we gathered treasures untold. Gold, pearls, and even a strange map that led me to further adventures.");
                                krakenVictoryModule.AddOption("What happened to the map?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule mapStoryModule = new DialogueModule("Ah, the map... it led us to an island shrouded in mist, where the ground itself seemed alive. The treasure we found there was unlike any other, but I can't say more. Some secrets are best left untold, unless you bring me something valuable in return.");
                                        mapStoryModule.AddOption("I understand. Perhaps we can trade.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                DialogueModule tradeIntroductionModule = new DialogueModule("Aye, indeed! If you have a SubOil, I would gladly trade you something rare. You may choose between a FancyPainting or a MedusaHead.");
                                                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                    pla => CanTradeWithPlayer(pla), 
                                                    pla =>
                                                    {
                                                        DialogueModule tradeModule = new DialogueModule("Do you have a SubOil for me?");
                                                        tradeModule.AddOption("Yes, I have a SubOil.", 
                                                            plaa => HasSubOil(plaa) && CanTradeWithPlayer(plaa), 
                                                            plaa =>
                                                            {
                                                                CompleteTrade(plaa);
                                                            });
                                                        tradeModule.AddOption("No, I don't have one right now.", 
                                                            plaa => !HasSubOil(plaa), 
                                                            plaa =>
                                                            {
                                                                plaa.SendMessage("Come back when you have a SubOil.");
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
                                                pllll.SendGump(new DialogueGump(pllll, tradeIntroductionModule));
                                            });
                                        mapStoryModule.AddOption("Goodbye.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Captain Viridian nods, his eyes glinting with untold secrets.");
                                            });
                                        plll.SendGump(new DialogueGump(plll, mapStoryModule));
                                    });
                                krakenVictoryModule.AddOption("Goodbye.", 
                                    plll => true, 
                                    plll =>
                                    {
                                        plll.SendMessage("Captain Viridian tips his hat, his gaze lost in the distance.");
                                    });
                                pll.SendGump(new DialogueGump(pll, krakenVictoryModule));
                            });
                        seaMonsterModule.AddOption("Goodbye.", 
                            pll => true, 
                            pll =>
                            {
                                pll.SendMessage("Captain Viridian smiles, his eyes hinting at unspoken stories.");
                            });
                        pl.SendGump(new DialogueGump(pl, seaMonsterModule));
                    });

                adventureModule.AddOption("What treasures have you found?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule treasureModule = new DialogueModule("Ah, treasures! I've found gold doubloons, mystical artifacts, and even a cursed ruby. They say the ruby has a spirit inside it, whispering in the night. I keep it locked away, for it brings nothing but trouble.");
                        treasureModule.AddOption("Tell me more about the cursed ruby.", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule cursedRubyModule = new DialogueModule("The ruby... it's unlike any gem I've seen. Blood red, and it glows under the moonlight. The spirit inside it speaks of long-lost empires and untold riches, but it also demands something in return. Many who've held it went mad, consumed by greed and visions.");
                                cursedRubyModule.AddOption("Can I see it?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule seeRubyModule = new DialogueModule("I'm afraid not, lad. The ruby's power is too dangerous. But, if you were to bring me something of equal worth, perhaps we could strike a deal...");
                                        seeRubyModule.AddOption("I'll see what I can find.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Captain Viridian nods thoughtfully. 'Aye, return when you're ready.'");
                                            });
                                        plll.SendGump(new DialogueGump(plll, seeRubyModule));
                                    });
                                cursedRubyModule.AddOption("Goodbye.", 
                                    plll => true, 
                                    plll =>
                                    {
                                        plll.SendMessage("Captain Viridian tucks away his memories of the ruby, his expression unreadable.");
                                    });
                                pll.SendGump(new DialogueGump(pll, cursedRubyModule));
                            });
                        treasureModule.AddOption("Goodbye.", 
                            pll => true, 
                            pll =>
                            {
                                pll.SendMessage("Captain Viridian smiles, his mind wandering back to his treasures.");
                            });
                        pl.SendGump(new DialogueGump(pl, treasureModule));
                    });

                p.SendGump(new DialogueGump(p, adventureModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Captain Viridian tips his hat and smiles.");
            });

        return greeting;
    }

    private bool HasSubOil(PlayerMobile player)
    {
        // Check the player's inventory for SubOil
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SubOil)) != null;
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
        // Remove the SubOil and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item subOil = player.Backpack.FindItemByType(typeof(SubOil));
        if (subOil != null)
        {
            subOil.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for FancyPainting and MedusaHead
            rewardChoiceModule.AddOption("FancyPainting", pl => true, pl => 
            {
                pl.AddToBackpack(new FancyPainting());
                pl.SendMessage("You receive a FancyPainting!");
            });
            
            rewardChoiceModule.AddOption("MedusaHead", pl => true, pl =>
            {
                pl.AddToBackpack(new MedusaHead());
                pl.SendMessage("You receive a MedusaHead!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SubOil.");
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