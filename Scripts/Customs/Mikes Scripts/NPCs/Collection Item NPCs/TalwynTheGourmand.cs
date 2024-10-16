using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class TalwynTheGourmand : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TalwynTheGourmand() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Talwyn the Gourmand";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(90);
        SetMana(180);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // Rich blue shirt
        AddItem(new LongPants(2117)); // Deep brown pants
        AddItem(new FeatheredHat(1161)); // A fancy hat with a feather
        AddItem(new Sandals(1109)); // Comfortable sandals
        AddItem(new HalfApron(1360)); // Dark apron to indicate his love for cooking

        VirtualArmor = 12;
    }

    public TalwynTheGourmand(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Talwyn, a connoisseur of all things delicious. Tell me, have you ever tasted true decadence?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your favorite dish.", 
            p => true, 
            p =>
            {
                DialogueModule dishesModule = new DialogueModule("Ah, the finest delicacies require rare ingredients. One such treat I am searching for is a Chocolate Fountain! If you have one, I might just be willing to make a trade...");
                
                // Trade option after story
                dishesModule.AddOption("What sort of trade are you talking about?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you bring me a Chocolate Fountain, I'll let you choose between a Master Gyro or a Wind Relic as a token of my appreciation. And of course, you will always receive a Maxxia Scroll as an additional reward!");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a Chocolate Fountain for me?");
                                tradeModule.AddOption("Yes, I have a Chocolate Fountain.", 
                                    plaa => HasChocolateFountain(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasChocolateFountain(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a Chocolate Fountain. I shall be waiting eagerly!");
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

                // Add more nested options
                dishesModule.AddOption("Why are you so interested in rare ingredients?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule insomniaModule = new DialogueModule("Ah, my friend, it is not just a taste for delicacies. You see, I do not sleep well... or perhaps not at all. The nights are long, filled with strange visions and darker truths. They show me hidden things, deeper realities. I feel that these rare ingredients are not just food, but keys to understanding the world.");
                        
                        insomniaModule.AddOption("Visions? What kind of visions?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule visionsModule = new DialogueModule("They are not easy to describe. Shadows twist into shapes, and I see a hidden world beneath our own. Sometimes, I think I hear whispers—fragments of tales, secrets of an ancient nature. The Chocolate Fountain... I believe it may be tied to these whispers. Call it paranoia, if you will, but I cannot ignore what I see.");
                                
                                visionsModule.AddOption("That sounds terrifying. Are you alright?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule fearModule = new DialogueModule("Ah, yes, terrifying indeed. There are nights I find myself pacing until dawn, unable to quell the anxiety these visions cause. Yet, within my fear, I find purpose. The ingredients I seek help ground me, give me a tangible goal. And maybe, just maybe, they hold the power to unveil what my restless mind craves to understand.");
                                        
                                        fearModule.AddOption("You are very insightful, despite the fear.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Talwyn smiles, his eyes weary but grateful. 'Insight comes at a cost, my friend. It is a burden I carry, but one I cannot abandon.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        fearModule.AddOption("I hope you find peace someday.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Talwyn nods slowly. 'Thank you, traveler. It is a hope I hold dear, even if it feels far away.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, fearModule));
                                    });

                                visionsModule.AddOption("What do you mean by 'a hidden world'?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule hiddenWorldModule = new DialogueModule("There are layers to our reality, I am certain of it. We walk in the daylight, oblivious to what lies beneath, but at night... oh, at night, the barriers thin. I have seen shadows move without cause, heard whispers echo where none should be. The Chocolate Fountain, the rare spices, the ancient herbs—they are clues. I believe they can help pierce the veil, but I am anxious, for I do not know what I might find.");
                                        
                                        hiddenWorldModule.AddOption("You are either very brave or very mad.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Talwyn chuckles, a sound both amused and pained. 'Perhaps I am both, friend. Madness and bravery often walk hand in hand.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        hiddenWorldModule.AddOption("What would you do if you found the truth?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule truthModule = new DialogueModule("I do not know. Part of me thinks that understanding the truth would bring peace, while another part fears that the truth is a burden too heavy to bear. Sometimes, I think it is better not to know... but I am driven by curiosity and the hope that whatever lies beneath may help me sleep, even if only for a night.");
                                                
                                                truthModule.AddOption("May your curiosity lead you to peace.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Talwyn smiles faintly. 'Thank you, traveler. I shall continue my search, one ingredient at a time.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, truthModule));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, hiddenWorldModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, visionsModule));
                            });
                        
                        insomniaModule.AddOption("Why can't you sleep?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule insomniaDetailModule = new DialogueModule("It is as though my mind never rests. I lie awake, thoughts racing, every sound magnified. The darkness brings no comfort, only questions and the gnawing feeling that I am missing something—something just beyond the edge of perception. The ingredients I seek, they help me focus, give me a purpose during those restless hours.");
                                
                                insomniaDetailModule.AddOption("It must be exhausting.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Talwyn nods, a deep sadness in his eyes. 'It is. But exhaustion is a small price to pay for even a glimpse of understanding.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });

                                insomniaDetailModule.AddOption("I hope you find rest one day.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Talwyn smiles faintly. 'Rest would be a gift, one I continue to search for. Thank you, traveler.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, insomniaDetailModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, insomniaModule));
                    });

                p.SendGump(new DialogueGump(p, dishesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Talwyn smiles warmly, his eyes twinkling with culinary curiosity, though a hint of weariness remains.");
            });

        return greeting;
    }

    private bool HasChocolateFountain(PlayerMobile player)
    {
        // Check the player's inventory for ChocolateFountain
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ChocolateFountain)) != null;
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
        // Remove the ChocolateFountain and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item chocolateFountain = player.Backpack.FindItemByType(typeof(ChocolateFountain));
        if (chocolateFountain != null)
        {
            chocolateFountain.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for MasterGyro and WindRelic
            rewardChoiceModule.AddOption("MasterGyro", pl => true, pl => 
            {
                pl.AddToBackpack(new MasterGyro());
                pl.SendMessage("You receive a Master Gyro!");
            });
            
            rewardChoiceModule.AddOption("WindRelic", pl => true, pl =>
            {
                pl.AddToBackpack(new WindRelic());
                pl.SendMessage("You receive a Wind Relic!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a Chocolate Fountain.");
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