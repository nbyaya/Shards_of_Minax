using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AleeraTheBard : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AleeraTheBard() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aleera the Bard";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(80);
        SetInt(110);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyDress(1853)); // A vibrant blue elegant dress
        AddItem(new Sandals(1165)); // Dark green sandals
        AddItem(new Bandana(1150)); // Golden bandana
        AddItem(new Lute()); // A lute to show her bardic nature

        VirtualArmor = 15;
    }

    public AleeraTheBard(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Aleera, a wandering bard in search of tales and melodies. Do you enjoy a good song or a fair trade?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your songs.", 
            p => true, 
            p =>
            {
                DialogueModule songsModule = new DialogueModule("Ah, my songs tell tales of heroism, lost love, and grand adventures! But lately, I've been quite thirsty from all this singing. I could use something to wet my throat.");
                
                // Deep branching into songs and experiments
                songsModule.AddOption("Why do you sing of lost love?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule lostLoveModule = new DialogueModule("Ah, lost love... It is a haunting melody that clings to my heart. Once, long ago, I sought to keep a love eternal, but my attempts were... misguided. The alchemical secrets I sought eluded me, and in my pursuit, I lost more than I could ever gain.");
                        
                        lostLoveModule.AddOption("What alchemical secrets did you seek?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule alchemyModule = new DialogueModule("The secrets of immortality, of binding the soul to this world beyond the limits of flesh. I was obsessed, blinded by my own ambition. I experimented with dark tinctures and forgotten rituals, thinking I could defy the natural order. Instead, I was left disfigured, a shadow of who I once was.");
                                
                                alchemyModule.AddOption("Why did you seek immortality?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule immortalityReasonModule = new DialogueModule("For love, of course. To keep my beloved with me for eternity. I believed that if I could live forever, so too could we find a way to preserve love eternally. But immortality is a curse, not a gift. I see that now, though it took years of regret and darkness to understand.");
                                        
                                        immortalityReasonModule.AddOption("What happened to your beloved?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule belovedModule = new DialogueModule("My beloved... they could not bear the path I had chosen. They left me when they saw the lengths I was willing to go, the darkness I embraced. I have not seen them since. I sing now in hopes that perhaps one day, my song might reach them, wherever they may be.");
                                                belovedModule.AddOption("I'm sorry to hear that. Your songs are beautiful.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Aleera smiles sadly, her eyes misty with old memories. 'Thank you, traveler. Perhaps beauty is all I have left.'");
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, belovedModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, immortalityReasonModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, alchemyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, lostLoveModule));
                    });

                songsModule.AddOption("Do you need a drink?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you happen to have a FineHoochJug, I would gladly trade you something special. You may choose between a FancyHornOfPlenty or some GamerJelly.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a FineHoochJug for me?");
                                tradeModule.AddOption("Yes, I have a FineHoochJug.", 
                                    plaa => HasFineHoochJug(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasFineHoochJug(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a FineHoochJug.");
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

                p.SendGump(new DialogueGump(p, songsModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Aleera nods and hums a soft tune as you part ways.");
            });

        return greeting;
    }

    private bool HasFineHoochJug(PlayerMobile player)
    {
        // Check the player's inventory for FineHoochJug
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FineHoochJug)) != null;
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
        // Remove the FineHoochJug and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fineHoochJug = player.Backpack.FindItemByType(typeof(FineHoochJug));
        if (fineHoochJug != null)
        {
            fineHoochJug.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for FancyHornOfPlenty and GamerJelly
            rewardChoiceModule.AddOption("FancyHornOfPlenty", pl => true, pl => 
            {
                pl.AddToBackpack(new FancyHornOfPlenty());
                pl.SendMessage("You receive a FancyHornOfPlenty!");
            });
            
            rewardChoiceModule.AddOption("GamerJelly", pl => true, pl =>
            {
                pl.AddToBackpack(new GamerJelly());
                pl.SendMessage("You receive some GamerJelly!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a FineHoochJug.");
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