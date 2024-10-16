using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThaliaKeeperOfTales : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaliaKeeperOfTales() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thalia, Keeper of the Ancient Tales";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1109)); // A deep blue robe
        AddItem(new Sandals(1175)); // Light blue sandals
        AddItem(new FeatheredHat(1150)); // A mystical feathered hat
        AddItem(new GoldBracelet()); // An ancient-looking bracelet
        AddItem(new Spellbook()); // A spellbook to signify her magical nature

        VirtualArmor = 15;
    }

    public ThaliaKeeperOfTales(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Thalia, Keeper of the Ancient Tales. Do you seek knowledge or perhaps something more tangible, a reward for your journey?");
        
        // Dialogue options
        greeting.AddOption("Tell me about the ancient tales.", 
            p => true, 
            p =>
            {
                DialogueModule talesModule = new DialogueModule("The lands are rich with stories of old - of heroes, of monsters, and of treasures lost to time. But those who assist me in my endeavors may find themselves part of a new tale.");
                
                talesModule.AddOption("What kind of stories do you know?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule storyTypesModule = new DialogueModule("I have tales of valor, tales of tragedy, and tales that blur the line between myth and reality. My stories are not just words - they are the echoes of the past, sometimes beautiful, sometimes terrifying.");
                        
                        storyTypesModule.AddOption("Tell me a tale of valor.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule valorTaleModule = new DialogueModule("Once, there was a knight named Aeron who ventured into the Abyss to face the Shadow Wyrm. His bravery was unmatched, but it was not his strength that brought him victory - it was his compassion. When he found the Wyrm, wounded and desperate, he chose to heal rather than slay. The bond they formed saved both their lives and forged a legend that endures to this day.");
                                valorTaleModule.AddOption("What happened to Aeron?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule aeronFateModule = new DialogueModule("Aeron returned to his kingdom, but he was never quite the same. He spoke little of his journey, and some say he saw visions of the Abyss in every shadow. He wrote poetry, dark and foreboding, until the day he disappeared into the night, never to be seen again.");
                                        aeronFateModule.AddOption("That's quite melancholic.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Indeed. The line between heroism and sorrow is often thin, and Aeron walked it until the end.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, aeronFateModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, valorTaleModule));
                            });

                        storyTypesModule.AddOption("Tell me a tale of tragedy.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule tragedyTaleModule = new DialogueModule("There was once a sorceress named Elara, who sought the power to bring her lost love back from the dead. She delved deep into the forbidden arts, and her obsession consumed her. In the end, she succeeded, but what she brought back was not her beloved - it was a hollow shell, a creature of shadows that haunted her until her dying day.");
                                tragedyTaleModule.AddOption("Did anyone try to help her?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule helpElaraModule = new DialogueModule("Many tried, but Elara pushed them all away. Her heart had grown cold, and her mind twisted by grief. In her final days, she wandered the streets, muttering verses that seemed to draw shadows from the walls. A tragic figure, lost to her own sorrow.");
                                        helpElaraModule.AddOption("It seems her story is a warning.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Yes, obsession can lead even the brightest souls into darkness. Remember Elara, traveler, and beware the cost of unrestrained desire.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, helpElaraModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, tragedyTaleModule));
                            });

                        storyTypesModule.AddOption("Tell me a tale of myths.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule mythTaleModule = new DialogueModule("Legends speak of the Crystal of Lament, a gem forged from the tears of a thousand souls. It is said that those who gaze into it can see their deepest fears and desires. Many sought the crystal, but all who found it were driven mad, obsessed with the visions they saw, unable to look away.");
                                mythTaleModule.AddOption("Is the Crystal real?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule crystalRealModule = new DialogueModule("Some believe it lies in the ruins of the old kingdoms, hidden from the world. Others say it was shattered long ago, its fragments scattered across the land. But I have seen those who claim to have gazed into it - their eyes are hollow, their minds lost to the shadows.");
                                        crystalRealModule.AddOption("A chilling thought indeed.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("The past is full of chilling thoughts, traveler. Beware the allure of what should remain hidden.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, crystalRealModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, mythTaleModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, storyTypesModule));
                    });
                
                // Trade option after story
                talesModule.AddOption("Is there any way I can assist you?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed there is. If you possess a BookTwentyfive, I can offer you a choice between a WelcomeMat or a HolidayCandleArran, along with an additional token of appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a BookTwentyfive for me?");
                                tradeModule.AddOption("Yes, I have a BookTwentyfive.", 
                                    plaa => HasBookTwentyfive(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasBookTwentyfive(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a BookTwentyfive.");
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

                p.SendGump(new DialogueGump(p, talesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thalia smiles and nods, her eyes twinkling with ancient secrets.");
            });

        return greeting;
    }

    private bool HasBookTwentyfive(PlayerMobile player)
    {
        // Check the player's inventory for BookTwentyfive
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BookTwentyfive)) != null;
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
        // Remove the BookTwentyfive and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item bookTwentyfive = player.Backpack.FindItemByType(typeof(BookTwentyfive));
        if (bookTwentyfive != null)
        {
            bookTwentyfive.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for WelcomeMat and HolidayCandleArran
            rewardChoiceModule.AddOption("WelcomeMat", pl => true, pl => 
            {
                pl.AddToBackpack(new WelcomeMat());
                pl.SendMessage("You receive a WelcomeMat!");
            });
            
            rewardChoiceModule.AddOption("HolidayCandleArran", pl => true, pl =>
            {
                pl.AddToBackpack(new HolidayCandleArran());
                pl.SendMessage("You receive a HolidayCandleArran!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a BookTwentyfive.");
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