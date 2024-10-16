using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class SylasTheRecluse : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SylasTheRecluse() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sylas the Recluse";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1150)); // Deep blue robe
        AddItem(new Sandals(141)); // Dark gray sandals
        AddItem(new WizardsHat(1150)); // Matching deep blue wizard hat
        AddItem(new QuarterStaff()); // A simple wooden staff

        VirtualArmor = 15;
    }

    public SylasTheRecluse(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler. I am Sylas, a recluse who prefers the quiet company of birds over men. Tell me, do you have a Birdhouse item for me?");
        
        // Dialogue options
        greeting.AddOption("What would you do with a Birdhouse?", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("Birds bring me comfort. With a Birdhouse, I can shelter them, and they repay me with secrets and knowledge. I seek these birdhouses to help my feathered friends.");
                
                storyModule.AddOption("Why do you trust the birds so much?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule birdTrustModule = new DialogueModule("The sea has taught me many things. The birds bring whispers from afar—secrets of shipwrecks, monstrous fish, and even tales of a city beneath the waves, where unspeakable horrors reside. They are my eyes and ears when the ocean grows restless.");
                        
                        birdTrustModule.AddOption("A city beneath the waves? Tell me more.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule underwaterCityModule = new DialogueModule("Aye, a dark and dreadful place. I have seen it in my dreams—towering spires covered in barnacles, and streets filled with creatures of immense, alien form. The city calls to me sometimes, in the still of night, whispering promises of power. But I know better. The sea gives, and the sea takes. No man should heed such calls.");
                                
                                underwaterCityModule.AddOption("Do you think the city is real?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule cityRealModule = new DialogueModule("Real? Ha! What is real in this world of ours? I have seen sailors lose their minds chasing such dreams. Some say it's just superstition, others claim it's a curse upon those who take more than they need from the ocean. I say it matters not. Whether real or imagined, the sea keeps her secrets, and I respect her for that.");
                                        
                                        cityRealModule.AddOption("Have you seen others seek the city?", 
                                            plaab => true, 
                                            plaab =>
                                            {
                                                DialogueModule seekersModule = new DialogueModule("Oh, I've seen many—a crew of hardy men once came to me, asking for guidance. They spoke of riches beyond imagining, pearls the size of fists, and treasures long lost. They sailed off with fire in their eyes. None returned, only their ship, shattered upon the rocks, with naught but silence aboard. The sea took them, as she always does when men grow greedy.");
                                                
                                                seekersModule.AddOption("That sounds terrifying.", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Sylas nods solemnly, his eyes staring off into the distance, lost in thought.");
                                                    });
                                                seekersModule.AddOption("Perhaps the sea spared them.", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        DialogueModule sparedModule = new DialogueModule("Spared? Ha! The sea is merciless. She has no kindness for those who disrespect her. No, they were not spared, friend. Their bodies rest beneath the waves, feeding the fish and the crabs. That's the only mercy the ocean gives—a return to her depths.");
                                                        
                                                        sparedModule.AddOption("I see...", 
                                                            plaabcd => true, 
                                                            plaabcd =>
                                                            {
                                                                plaabcd.SendMessage("Sylas gives you a hard look, as if weighing your understanding.");
                                                            });
                                                        plaabc.SendGump(new DialogueGump(plaabc, sparedModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, seekersModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, cityRealModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, underwaterCityModule));
                            });
                        
                        birdTrustModule.AddOption("That sounds... unsettling.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Sylas chuckles darkly, 'Aye, unsettling indeed. But that is the nature of the sea.'");
                            });
                        pl.SendGump(new DialogueGump(pl, birdTrustModule));
                    });
                
                // Trade option after story
                storyModule.AddOption("I have a Birdhouse for you.", 
                    pl => HasBirdhouse(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                storyModule.AddOption("I don't have one right now.", 
                    pl => !HasBirdhouse(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a Birdhouse. My birds and I will be waiting.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                storyModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Sylas nods and turns his gaze back to the trees.");
            });

        return greeting;
    }

    private bool HasBirdhouse(PlayerMobile player)
    {
        // Check the player's inventory for Birdhouse
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(Birdhouse)) != null;
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
        // Remove the Birdhouse and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item birdhouse = player.Backpack.FindItemByType(typeof(Birdhouse));
        if (birdhouse != null)
        {
            birdhouse.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for SabertoothSkull and ZebulinVase
            rewardChoiceModule.AddOption("SabertoothSkull", pl => true, pl => 
            {
                pl.AddToBackpack(new SabertoothSkull());
                pl.SendMessage("You receive a SabertoothSkull!");
            });
            
            rewardChoiceModule.AddOption("ZebulinVase", pl => true, pl =>
            {
                pl.AddToBackpack(new ZebulinVase());
                pl.SendMessage("You receive a ZebulinVase!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a Birdhouse.");
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