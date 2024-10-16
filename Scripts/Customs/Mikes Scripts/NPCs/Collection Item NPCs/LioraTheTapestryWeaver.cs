using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class LioraTheTapestryWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LioraTheTapestryWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Liora the Tapestry Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(45);
        SetDex(55);
        SetInt(120);

        SetHits(75);
        SetMana(160);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1359)); // Deep purple fancy shirt
        AddItem(new Skirt(1194)); // A black skirt
        AddItem(new Sandals(1445)); // Bright red sandals
        AddItem(new Cloak(1161)); // Golden cloak
        AddItem(new Bandana(1150)); // Bright blue bandana
        AddItem(new SexWhip()); // Decorative embroidery tool in her inventory

        VirtualArmor = 15;
    }

    public LioraTheTapestryWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Liora, a master of tapestries and embroidery, keeping alive the art of ancient threads. Do you happen to have any artifacts of old craftsmanship?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your work.", 
            p => true, 
            p =>
            {
                DialogueModule workModule = new DialogueModule("I weave tapestries that tell stories of ancient times, using techniques passed down for generations. I could use an OldEmbroideryTool if you have one...");
                
                // Nested Dialogue options
                workModule.AddOption("Why do you need an OldEmbroideryTool?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("The OldEmbroideryTool is a relic of a bygone era, made with precision and skill that modern tools cannot replicate. It allows me to bring out the true essence of the stories I weave.");
                        
                        // More nested dialogue
                        reasonModule.AddOption("What kind of stories do you weave?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule storiesModule = new DialogueModule("I weave stories of heroes and villains, of the relentless chase between justice and moral ambiguity. I used to weave tales of hope, but now, my work reflects the harsh realities of life—betrayal, survival, and the fragile nature of trust.");
                                
                                storiesModule.AddOption("You sound like you've seen much of life. Have you?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule lifeModule = new DialogueModule("Indeed. Once, I believed in the purity of art and justice. But like a bounty hunter who has seen the worst in people, I have grown stoic. Life is rarely black and white, and so are the tapestries I weave now—more shadows than light.");
                                        
                                        lifeModule.AddOption("Why compare yourself to a bounty hunter?", 
                                            plaab => true, 
                                            plaab =>
                                            {
                                                DialogueModule bountyHunterModule = new DialogueModule("Because, like a bounty hunter, I have learned that the world is filled with morally ambiguous choices. Justice is an illusion, a shifting concept, bought and sold by the highest bidder. I weave, but my hands are no cleaner than those who take contracts for coin.");
                                                
                                                bountyHunterModule.AddOption("That sounds... jaded.", 
                                                    plaabb => true, 
                                                    plaabb =>
                                                    {
                                                        DialogueModule jadedModule = new DialogueModule("Jaded, yes. But perhaps there is freedom in abandoning the illusion of righteousness. In the end, we are all just trying to survive, making our own bargains—whether with thread or with steel.");
                                                        
                                                        jadedModule.AddOption("What keeps you going, then?", 
                                                            plaabbb => true, 
                                                            plaabbb =>
                                                            {
                                                                DialogueModule motivationModule = new DialogueModule("The pursuit of perfection, perhaps. Or maybe it is simply the habit of surviving one more day. Every tapestry I complete, every thread I place, is an attempt to bring meaning where none exists.");
                                                                
                                                                motivationModule.AddOption("Do you think you'll find meaning someday?", 
                                                                    plaabbbb => true, 
                                                                    plaabbbb =>
                                                                    {
                                                                        DialogueModule meaningModule = new DialogueModule("I don't know. Perhaps meaning isn't something we find; perhaps it is something we create, however fleeting. If I can make something beautiful, even for a moment, that may be enough.");
                                                                        
                                                                        meaningModule.AddOption("Thank you for sharing your story, Liora.", 
                                                                            plaabbbbb => true, 
                                                                            plaabbbbb =>
                                                                            {
                                                                                plaabbbbb.SendMessage("Liora nods, her gaze distant as if lost in the weaving of her own thoughts.");
                                                                            });
                                                                        plaabbbb.SendGump(new DialogueGump(plaabbbb, meaningModule));
                                                                    });
                                                                plaabbb.SendGump(new DialogueGump(plaabbb, motivationModule));
                                                            });
                                                        plaabb.SendGump(new DialogueGump(plaabb, jadedModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, bountyHunterModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, lifeModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, storiesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });
                
                // Trade option after story
                workModule.AddOption("Do you need an OldEmbroideryTool?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have an OldEmbroideryTool, I would gladly trade you something unique. You may choose between a DistillationFlask or a SexWhip. Additionally, I will give you a MaxxiaScroll as a token of appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an OldEmbroideryTool for me?");
                                tradeModule.AddOption("Yes, I have an OldEmbroideryTool.", 
                                    plaa => HasOldEmbroideryTool(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasOldEmbroideryTool(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an OldEmbroideryTool.");
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

                p.SendGump(new DialogueGump(p, workModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Liora nods, her hands still busy with the loom.");
            });

        return greeting;
    }

    private bool HasOldEmbroideryTool(PlayerMobile player)
    {
        // Check the player's inventory for OldEmbroideryTool
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(OldEmbroideryTool)) != null;
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
        // Remove the OldEmbroideryTool and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item oldEmbroideryTool = player.Backpack.FindItemByType(typeof(OldEmbroideryTool));
        if (oldEmbroideryTool != null)
        {
            oldEmbroideryTool.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for DistillationFlask and SexWhip
            rewardChoiceModule.AddOption("DistillationFlask", pl => true, pl => 
            {
                pl.AddToBackpack(new DistillationFlask());
                pl.SendMessage("You receive a DistillationFlask!");
            });
            
            rewardChoiceModule.AddOption("SexWhip", pl => true, pl =>
            {
                pl.AddToBackpack(new SexWhip());
                pl.SendMessage("You receive a SexWhip!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an OldEmbroideryTool.");
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