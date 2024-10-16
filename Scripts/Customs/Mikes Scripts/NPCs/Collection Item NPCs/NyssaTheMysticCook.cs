using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class NyssaTheMysticCook : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NyssaTheMysticCook() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Nyssa the Mystic Cook";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Cap(1150)); // Light blue chef's hat
        AddItem(new FancyShirt(2301)); // Deep red fancy shirt
        AddItem(new Kilt(1153)); // Purple skirt
        AddItem(new Sandals(1150)); // Light blue sandals
        AddItem(new Club()); // Nyssa's signature spoon

        VirtualArmor = 15;
    }

    public NyssaTheMysticCook(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Nyssa, a cook of mystical delights. Have you ever tasted something that transcends the ordinary senses?");
        
        // Dialogue options
        greeting.AddOption("Tell me more about your mystical dishes.",
            p => true,
            p =>
            {
                DialogueModule dishesModule = new DialogueModule("Ah, I use ingredients touched by magic, gathered from realms unknown. The secret is in combining the mundane with the magical to make the extraordinary. In fact, I am in search of a special ingredient - a BlueberryPie. Do you have one?");
                
                // Add a nested backstory dialogue
                dishesModule.AddOption("Your story intrigues me. How did you become a mystical cook?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Well, it's not a story I tell often... I was a child of war, born amidst the ruins. My family was lost to raiders, and I learned to survive on my own. I scavenged, stole, and outsmarted those who would harm me. Eventually, I found solace in the little things, like cooking. It was my way of bringing a bit of comfort to an otherwise harsh world. And with time, I learned that even the darkest of places could hold magic if you knew where to look.");
                        
                        backstoryModule.AddOption("That sounds difficult. How did you manage to survive?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule survivalModule = new DialogueModule("It wasn't easy. I had to be resilient, cunning, and resourceful. I would sneak into raider camps at night, taking only what I needed. I learned to blend into the shadows, and I always had an escape plan. Cooking became a skill I could use to barter for safety, a meal in exchange for protection. Over time, I honed my skills and made a name for myself as someone who could make the impossible edible.");
                                
                                survivalModule.AddOption("And now you're here, sharing your magic through food?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule presentDayModule = new DialogueModule("Yes, exactly. I wanted more than just survival; I wanted to create something beautiful. The mysticism in my cooking is a reflection of my journey - transforming hardship into something wonderful. That's why I'm always looking for unique ingredients, like a BlueberryPie. Each dish tells a story, and each story is a chance to bring a little light to someone else.");
                                        
                                        presentDayModule.AddOption("I admire your strength. Let's talk about that BlueberryPie again.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule tradeIntroductionModule = CreateTradeIntroductionModule(plaaa);
                                                plaaa.SendGump(new DialogueGump(plaaa, tradeIntroductionModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, presentDayModule));
                                    });

                                survivalModule.AddOption("It's incredible how far you've come.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Nyssa smiles, her eyes distant as if lost in a memory.");
                                    });

                                pla.SendGump(new DialogueGump(pla, survivalModule));
                            });

                        backstoryModule.AddOption("What drives you now, after everything you've been through?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule motivationModule = new DialogueModule("Hope. After everything I've seen, I still believe there's magic in this world - in the kindness of strangers, in the warmth of a meal, in a shared story. I want to bring a little of that magic to others. It's why I offer trades, why I share my dishes, and why I tell my story. Everyone has something to offer, and I believe in giving them the opportunity to do so.");
                                
                                motivationModule.AddOption("You're truly inspiring, Nyssa. Now, about that BlueberryPie...",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule tradeIntroductionModule = CreateTradeIntroductionModule(plaa);
                                        plaa.SendGump(new DialogueGump(plaa, tradeIntroductionModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, motivationModule));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                // Trade option after story
                dishesModule.AddOption("What would you give me for a BlueberryPie?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = CreateTradeIntroductionModule(pl);
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, dishesModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Nyssa nods and returns to stirring her mystical pot.");
            });

        return greeting;
    }

    private DialogueModule CreateTradeIntroductionModule(PlayerMobile player)
    {
        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a BlueberryPie, I can offer you a choice of a PetRock or a PileOfChains. Additionally, you will receive a MaxxiaScroll for your generosity.");
        tradeIntroductionModule.AddOption("I'd like to make the trade.",
            pla => CanTradeWithPlayer(pla),
            pla =>
            {
                DialogueModule tradeModule = new DialogueModule("Do you have a BlueberryPie for me?");
                tradeModule.AddOption("Yes, I have a BlueberryPie.",
                    plaa => HasBlueberryPie(plaa) && CanTradeWithPlayer(plaa),
                    plaa =>
                    {
                        CompleteTrade(plaa);
                    });
                tradeModule.AddOption("No, I don't have one right now.",
                    plaa => !HasBlueberryPie(plaa),
                    plaa =>
                    {
                        plaa.SendMessage("Come back when you have a BlueberryPie.");
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

        return tradeIntroductionModule;
    }

    private bool HasBlueberryPie(PlayerMobile player)
    {
        // Check the player's inventory for BlueberryPie
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BlueberryPie)) != null;
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
        // Remove the BlueberryPie and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item blueberryPie = player.Backpack.FindItemByType(typeof(BlueberryPie));
        if (blueberryPie != null)
        {
            blueberryPie.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for PetRock and PileOfChains
            rewardChoiceModule.AddOption("PetRock", pl => true, pl => 
            {
                pl.AddToBackpack(new PetRock());
                pl.SendMessage("You receive a PetRock!");
            });
            
            rewardChoiceModule.AddOption("PileOfChains", pl => true, pl =>
            {
                pl.AddToBackpack(new PileOfChains());
                pl.SendMessage("You receive a PileOfChains!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a BlueberryPie.");
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