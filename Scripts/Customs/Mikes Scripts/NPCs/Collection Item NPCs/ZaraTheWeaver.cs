using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZaraTheWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZaraTheWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zara the Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(110);

        SetHits(75);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1157)); // Unique hooded robe
        AddItem(new Sandals(1161)); // Bright blue sandals
        AddItem(new HalfApron(2413)); // Weaver's apron
        AddItem(new FancyShirt(1153)); // Elegant weaver's shirt

        VirtualArmor = 8;
    }

    public ZaraTheWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Zara, the master weaver of rare and enchanted textiles. Perhaps you have something interesting for me?");
        
        // Dialogue options
        greeting.AddOption("What are you looking for?", 
            p => true, 
            p =>
            {
                DialogueModule requestModule = new DialogueModule("I have a passion for unique knitting! If you have 'GrandmasKnitting', I can offer you a choice: a FancyShipWheel or some ExoticBoots. And, of course, a special scroll to aid you in your adventures.");
                
                // Adding investigative personality and paranoid traits
                requestModule.AddOption("Why are you so interested in this knitting?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule investigationModule = new DialogueModule("You want to know, do you? They say knitting is just an innocent pastime, but it's more than that. It holds secrets, patterns that speak to those who know how to listen. I believe there is a code hidden within, a message the cult left behind, and I need to uncover it.");
                        
                        investigationModule.AddOption("A cult? Are you being followed?", 
                            p2 => true, 
                            p2 =>
                            {
                                DialogueModule cultModule = new DialogueModule("Yes! You see it too, don't you? They're everywhere. Shadowy figures, whispering in the dark. Ever since I uncovered their secrets, I haven't had a moment's peace. They watch me, even now. That knitting you bring me—it might help reveal their plans, or maybe it's their way of keeping track of me.");
                                
                                cultModule.AddOption("That sounds dangerous. Why keep investigating?", 
                                    p3 => true, 
                                    p3 =>
                                    {
                                        DialogueModule dangerousModule = new DialogueModule("Dangerous? Ha! I've faced danger before. I've seen things that would make your hair turn white. I can't stop now; I need to expose them, bring their deeds into the light. If I let fear control me, they win. Besides, the truth... it's worth every risk.");
                                        
                                        dangerousModule.AddOption("You're brave, but reckless. How can I help?", 
                                            p4 => true, 
                                            p4 =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("You can help by bringing me 'GrandmasKnitting'. Each piece brings me closer to deciphering their plans. But beware—once you involve yourself, they might start following you too. Are you sure you're ready for that?");
                                                
                                                helpModule.AddOption("I am ready to help you. Let's trade.", 
                                                    plq => HasGrandmasKnitting(plq) && CanTradeWithPlayer(plq), 
                                                    plq =>
                                                    {
                                                        CompleteTrade(pl);
                                                    });
                                                
                                                helpModule.AddOption("This sounds too dangerous. I'll think about it.", 
                                                    plw => true, 
                                                    plw =>
                                                    {
                                                        pl.SendMessage("Zara nods solemnly, understanding your hesitation.");
                                                    });
                                                
                                                p4.SendGump(new DialogueGump(p4, helpModule));
                                            });
                                        
                                        p3.SendGump(new DialogueGump(p3, dangerousModule));
                                    });
                                
                                p2.SendGump(new DialogueGump(p2, cultModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, investigationModule));
                    });
                
                // Trade option after request
                requestModule.AddOption("I have GrandmasKnitting. Let's trade.", 
                    pl => HasGrandmasKnitting(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                requestModule.AddOption("I don't have it right now.", 
                    pl => !HasGrandmasKnitting(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have 'GrandmasKnitting'.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                requestModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, requestModule));
            });

        greeting.AddOption("Who are you, really?", 
            p => true, 
            p =>
            {
                DialogueModule whoModule = new DialogueModule("I used to be just a simple weaver, until I stumbled onto something I shouldn't have. I found a pattern—woven into an old cloak—that spoke of secrets, hidden places, and dangerous people. I started to investigate, and that's when everything changed. They call me paranoid, but I know the truth. The cult is out there, watching.");
                
                whoModule.AddOption("Tell me more about the cult.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule cultDetailsModule = new DialogueModule("They're everywhere, hiding in plain sight. They use symbols and codes in the fabric of everyday life. I've found them in tapestries, in clothing, even in knitting patterns. They communicate through these threads. I don’t know exactly what they want, but I know they're dangerous. And now, they know I know.");
                        
                        cultDetailsModule.AddOption("Do you think they're after me now?", 
                            ple => true, 
                            ple =>
                            {
                                DialogueModule afterYouModule = new DialogueModule("Perhaps. By talking to me, you've already put yourself at risk. But fear not—knowledge is power. The more we know, the harder it is for them to manipulate us. Stay vigilant. If you find anything unusual—patterns, symbols—bring them to me.");
                                
                                afterYouModule.AddOption("I'll be careful. Goodbye.", 
                                    p2 => true, 
                                    p2 =>
                                    {
                                        p2.SendMessage("Zara gives you a knowing look, her eyes filled with both fear and determination.");
                                    });
                                
                                pl.SendGump(new DialogueGump(pl, afterYouModule));
                            });
                        
                        cultDetailsModule.AddOption("This is too much for me. Goodbye.", 
                            plr => true, 
                            plr =>
                            {
                                pl.SendMessage("Zara sighs softly, her eyes weary from her endless struggle.");
                            });
                        
                        pl.SendGump(new DialogueGump(pl, cultDetailsModule));
                    });
                
                p.SendGump(new DialogueGump(p, whoModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zara nods and continues weaving her intricate patterns, her eyes occasionally darting to the shadows.");
            });

        return greeting;
    }

    private bool HasGrandmasKnitting(PlayerMobile player)
    {
        // Check the player's inventory for GrandmasKnitting
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GrandmasKnitting)) != null;
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
        // Remove the GrandmasKnitting and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item grandmasKnitting = player.Backpack.FindItemByType(typeof(GrandmasKnitting));
        if (grandmasKnitting != null)
        {
            grandmasKnitting.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for FancyShipWheel and ExoticBoots
            rewardChoiceModule.AddOption("FancyShipWheel", pl => true, pl => 
            {
                pl.AddToBackpack(new FancyShipWheel());
                pl.SendMessage("You receive a FancyShipWheel!");
            });
            
            rewardChoiceModule.AddOption("ExoticBoots", pl => true, pl =>
            {
                pl.AddToBackpack(new ExoticBoots());
                pl.SendMessage("You receive ExoticBoots!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have 'GrandmasKnitting'.");
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