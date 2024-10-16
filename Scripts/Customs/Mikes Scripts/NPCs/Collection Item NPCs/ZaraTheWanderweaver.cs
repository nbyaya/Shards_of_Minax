using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ZaraTheWanderweaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZaraTheWanderweaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zara the Wanderweaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 1500;
        Karma = 500;

        // Outfit
        AddItem(new FancyDress(1153)); // Azure fancy dress
        AddItem(new Sandals(1153)); // Matching azure sandals
        AddItem(new ChaliceOfPilfering()); // A beaded necklace
        AddItem(new Cloak(2412)); // Dark green cloak
        AddItem(new GnarledStaff()); // A staff for her wandering

        VirtualArmor = 15;
    }

    public ZaraTheWanderweaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Zara, the Wanderweaver. My loom creates not only tapestries but also tales of distant lands. Do you, by any chance, have something exotic for me?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your tapestries.", 
            p => true, 
            p =>
            {
                DialogueModule tapestryModule = new DialogueModule("My tapestries capture the stories of lands I have wandered. Each thread, each color tells of places most will never see. But I am always in need of new materials to craft my next masterpiece.");
                
                // Nested dialogue: Learn about Zara's past
                tapestryModule.AddOption("Why do you wander so much, Zara?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("Once, I had a home, a quiet village where I thought I could create in peace. But the world isn't as kind as it should be. Corruption, power, greed—it drove me from my haven, forcing me to wander. I learned that beauty can only thrive where freedom exists.");
                        
                        // More depth on corruption
                        pastModule.AddOption("Corruption? You sound like a jaded journalist.", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule corruptionModule = new DialogueModule("Ah, yes, a 'jaded journalist' indeed. I once believed in the purity of creation. I was naive. Now, I weave tapestries that show the truth, the ugly truth of those who seek to control and suppress. Do you understand what it's like to fight against something so much larger than yourself?");
                                
                                // Player choices
                                corruptionModule.AddOption("I understand. It sounds brave, but exhausting.", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule braveModule = new DialogueModule("It is exhausting, and frightening. But there's no alternative. Someone must stand against tyranny, even if all they have is a loom and a story. I suppose that makes me brave, or perhaps just stubborn.");
                                        
                                        braveModule.AddOption("You sound relentless.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                DialogueModule relentlessModule = new DialogueModule("Relentless, yes. When you've seen enough cruelty, enough corruption, you realize there's no stopping. You either keep fighting or you let yourself be consumed by despair. I chose the fight. Perhaps one day my stories will help others see what I see.");
                                                pllll.SendGump(new DialogueGump(pllll, relentlessModule));
                                            });
                                        
                                        braveModule.AddOption("Thank you for sharing your story.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Zara nods, her eyes reflecting both weariness and determination.");
                                            });
                                        
                                        plll.SendGump(new DialogueGump(plll, braveModule));
                                    });
                                
                                corruptionModule.AddOption("Why fight at all? Can't you just ignore it?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule fightModule = new DialogueModule("Ignoring evil doesn't make it go away. It festers and grows, like a blight on the world. If you ignore it, you're complicit. I'd rather die trying to make a difference than live knowing I did nothing.");
                                        
                                        fightModule.AddOption("I respect that. It's a hard choice.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Zara smiles faintly, her resolve unbroken.");
                                            });
                                        
                                        plll.SendGump(new DialogueGump(plll, fightModule));
                                    });
                                
                                pll.SendGump(new DialogueGump(pll, corruptionModule));
                            });
                        
                        pastModule.AddOption("I understand. The world can be a harsh place.", 
                            pll => true, 
                            pll =>
                            {
                                pll.SendMessage("Zara gives a knowing nod, her eyes filled with old memories.");
                            });
                        
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });
                
                // Trade option after story
                tapestryModule.AddOption("Do you need any materials?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have ExoticWoods, I can offer you a choice between a ChaliceOfPilfering or a NameTapestry, along with a special gift of mine.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have ExoticWoods for me?");
                                tradeModule.AddOption("Yes, I have ExoticWoods.", 
                                    plaa => HasExoticWoods(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have them right now.", 
                                    plaa => !HasExoticWoods(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have ExoticWoods to trade.");
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

                p.SendGump(new DialogueGump(p, tapestryModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zara nods softly, her eyes filled with stories yet untold.");
            });

        return greeting;
    }

    private bool HasExoticWoods(PlayerMobile player)
    {
        // Check the player's inventory for ExoticWoods
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExoticWoods)) != null;
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
        // Remove the ExoticWoods and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item exoticWoods = player.Backpack.FindItemByType(typeof(ExoticWoods));
        if (exoticWoods != null)
        {
            exoticWoods.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for ChaliceOfPilfering and NameTapestry
            rewardChoiceModule.AddOption("ChaliceOfPilfering", pl => true, pl => 
            {
                pl.AddToBackpack(new ChaliceOfPilfering());
                pl.SendMessage("You receive a ChaliceOfPilfering!");
            });
            
            rewardChoiceModule.AddOption("NameTapestry", pl => true, pl =>
            {
                pl.AddToBackpack(new NameTapestry());
                pl.SendMessage("You receive a NameTapestry!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have ExoticWoods.");
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