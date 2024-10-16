using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ArinTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ArinTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Arin the Collector";
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
        AddItem(new FancyShirt(1153)); // Deep blue fancy shirt
        AddItem(new LongPants(2406)); // Dark brown pants
        AddItem(new Sandals(2301)); // Light brown sandals
        AddItem(new FeatheredHat(1175)); // A feathered hat with a distinct color
        AddItem(new GoldRing()); // A shiny gold ring

        VirtualArmor = 15;
    }

    public ArinTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Arin, a collector of rare and unique artworks. Do you happen to have an Anniversary Painting with you? I may have something special to offer in return.");
        
        // Dialogue options
        greeting.AddOption("Tell me more about your collection.", 
            p => true, 
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("Art tells stories that words often cannot. I collect paintings, especially those that mark important events. The Anniversary Painting is one such treasure. But I am more than just a collector of art; I am also the keeper of knowledge."
                + "\n\nThis town, traveler, has secrets. Dark secrets. Secrets buried beneath years of silence. Only a few dare to even whisper of them, and fewer still live to tell the tale."
                + "\n\nPerhaps, if you are brave enough, I could share with you some of what I know.");
                
                // Nested options for secrets of the town
                collectionModule.AddOption("I am interested in the town's secrets.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Ah, curiosity. It is a dangerous thing, but often rewarding. This town, once vibrant, hides a dark history."
                        + "\n\nLong ago, there was a library, a place of knowledge and forbidden texts. I was its guardian, and I have read things that cannot be unread. There are forces, ancient and cold, that dwell beneath this town."
                        + "\n\nThere was once a scholar, a librarian who went too far. She uncovered something she shouldn't have, something meant to be forgotten."
                        + "\n\nDo you wish to know more, traveler? Are you certain you want to tread this path?");
                        
                        // Deeper secrets
                        secretsModule.AddOption("Tell me about the librarian.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule librarianModule = new DialogueModule("The librarian's name was Elara. She was brilliant but reclusive, much like myself. She sought knowledge that others feared. One day, she found a text known as the Codex of Shadows."
                                + "\n\nThe Codex spoke of rituals, of ways to bind and control the forces beneath the earth. Elara thought she could use this power to protect the town. But power has a cost, and the price was her very soul."
                                + "\n\nThe town elders feared her, and rightly so. They sealed the library, buried its knowledge. But Elara's spirit lingers, and the Codex was never destroyed."
                                + "\n\nIf you were to find the Codex, you could perhaps unlock secrets long lost... or unleash horrors unimaginable.");
                                
                                // Offering a quest
                                librarianModule.AddOption("I wish to find the Codex of Shadows.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Finding the Codex is no simple task. It lies deep within the forgotten ruins beneath the town, guarded by shadows and the restless spirit of Elara herself.");
                                        plaa.SendMessage("If you are serious, you will need more than courage. You will need wisdom, artifacts of power, and perhaps... my assistance.");
                                        DialogueModule questModule = new DialogueModule("Should you bring me an Anniversary Painting, I may be able to help you further on your quest. Remember, traveler, knowledge always has a price.");
                                        plaa.SendGump(new DialogueGump(plaa, questModule));
                                    });
                                
                                librarianModule.AddOption("Perhaps this is too dangerous for me.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("A wise choice, traveler. Some doors are better left unopened.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, librarianModule));
                            });

                        secretsModule.AddOption("No, I do not wish to know more.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Very well, traveler. Some paths are better left untrodden.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, secretsModule));
                    });
                
                // Trade option after story
                collectionModule.AddOption("I have an Anniversary Painting. What can you offer in exchange?", 
                    pl => HasAnniversaryPainting(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                collectionModule.AddOption("I don’t have an Anniversary Painting right now.", 
                    pl => !HasAnniversaryPainting(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have an Anniversary Painting. I would love to make a deal.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                collectionModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Arin nods and says, 'Farewell, traveler. May your journey be filled with art and wonder.'");
            });

        return greeting;
    }

    private bool HasAnniversaryPainting(PlayerMobile player)
    {
        // Check the player's inventory for AnniversaryPainting
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AnniversaryPainting)) != null;
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
        // Remove the AnniversaryPainting and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item painting = player.Backpack.FindItemByType(typeof(AnniversaryPainting));
        if (painting != null)
        {
            painting.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("I have two rare items to offer you in exchange. Which one do you choose?");
            
            // Add options for PlagueBanner and MillStones
            rewardChoiceModule.AddOption("PlagueBanner", pl => true, pl => 
            {
                pl.AddToBackpack(new PlagueBanner());
                pl.SendMessage("You receive a PlagueBanner!");
            });
            
            rewardChoiceModule.AddOption("MillStones", pl => true, pl =>
            {
                pl.AddToBackpack(new MillStones());
                pl.SendMessage("You receive MillStones!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an Anniversary Painting.");
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