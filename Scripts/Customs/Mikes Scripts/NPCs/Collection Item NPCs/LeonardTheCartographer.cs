using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class LeonardTheCartographer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LeonardTheCartographer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Leonard the Cartographer";
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
        AddItem(new FancyShirt(1153)); // A blue fancy shirt
        AddItem(new LongPants(1109)); // Dark gray pants
        AddItem(new Boots(1175)); // Black boots
        AddItem(new FeatheredHat(1150)); // A bright blue feathered hat
        AddItem(new Backpack()); // Leonard carries his cartography tools in his backpack

        VirtualArmor = 12;
    }

    public LeonardTheCartographer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Leonard, a master cartographer. Are you perhaps interested in the secrets of the world, mapped by my very own hand?");
        
        // Dialogue options
        greeting.AddOption("Tell me more about your maps.", 
            p => true, 
            p =>
            {
                DialogueModule mapsModule = new DialogueModule("My maps capture the unknown, the unseen, and the forgotten. I have traveled far and wide, but even I need help gathering certain artifacts for my work.");
                
                // More depth about his work and personal struggle
                mapsModule.AddOption("Why do you need help?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule helpReasonModule = new DialogueModule("I have seen many things, but the burden of discovery is not without its toll. I find myself haunted by the echoes of those who came before, their stories carved into the land. My hands, skilled as they may be, are weary, and I need others to assist in gathering what I cannot. My sculptures are not mere stone and wood. They are shaped from agony, from the remains of those who suffered. Each piece holds a story, a fragment of a soul, a glimpse into torment long past.");
                        
                        helpReasonModule.AddOption("That sounds... dark. Why do you do it?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule darkReasonModule = new DialogueModule("Dark? Yes, I suppose it is. I do it because I must. The souls of the forgotten are restless, seeking release, and I give them form. Through me, they tell their story to the world, in hopes that someone, anyone, will remember their suffering. My art is not for beauty; it is a reflection of the grotesque, a voice for the voiceless.");
                                
                                // Player Reaction Choices
                                darkReasonModule.AddOption("I understand, there is beauty even in the macabre.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Leonard gazes at you deeply, a flicker of emotion in his eyes. 'Few understand, but you... you might just get it.'");
                                    });
                                darkReasonModule.AddOption("That is too grim for me. I wish you luck in your work.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Leonard nods solemnly. 'It is not a path for everyone. Farewell, traveler.'");
                                    });
                                pla.SendGump(new DialogueGump(pla, darkReasonModule));
                            });
                        
                        helpReasonModule.AddOption("How can I assist you?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule assistModule = new DialogueModule("Indeed! If you have a CartographyDesk, I would gladly trade you something special. You may choose between a BirdStatue or Hamburgers, plus I always provide a MaxxiaScroll as a token of my gratitude.");
                                assistModule.AddOption("I'd like to make the trade.", 
                                    plaa => CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        DialogueModule tradeModule = new DialogueModule("Do you have a CartographyDesk for me?");
                                        tradeModule.AddOption("Yes, I have a CartographyDesk.", 
                                            plaab => HasCartographyDesk(plaab) && CanTradeWithPlayer(plaab), 
                                            plaab =>
                                            {
                                                CompleteTrade(plaab);
                                            });
                                        tradeModule.AddOption("No, I don't have one right now.", 
                                            plaab => !HasCartographyDesk(plaab), 
                                            plaab =>
                                            {
                                                plaab.SendMessage("Come back when you have a CartographyDesk.");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                                            plaab => !CanTradeWithPlayer(plaab), 
                                            plaab =>
                                            {
                                                plaab.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tradeModule));
                                    });
                                assistModule.AddOption("Maybe another time.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, assistModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, helpReasonModule));
                    });
                
                mapsModule.AddOption("Is there anything I can help with?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a CartographyDesk, I would gladly trade you something special. You may choose between a BirdStatue or Hamburgers, plus I always provide a MaxxiaScroll as a token of my gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a CartographyDesk for me?");
                                tradeModule.AddOption("Yes, I have a CartographyDesk.", 
                                    plaa => HasCartographyDesk(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasCartographyDesk(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a CartographyDesk.");
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

                p.SendGump(new DialogueGump(p, mapsModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Leonard nods and returns to examining his maps.");
            });

        return greeting;
    }

    private bool HasCartographyDesk(PlayerMobile player)
    {
        // Check the player's inventory for CartographyDesk
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CartographyDesk)) != null;
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
        // Remove the CartographyDesk and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item cartographyDesk = player.Backpack.FindItemByType(typeof(CartographyDesk));
        if (cartographyDesk != null)
        {
            cartographyDesk.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for BirdStatue and Hamburgers
            rewardChoiceModule.AddOption("BirdStatue", pl => true, pl => 
            {
                pl.AddToBackpack(new BirdStatue());
                pl.SendMessage("You receive a BirdStatue!");
            });
            
            rewardChoiceModule.AddOption("Hamburgers", pl => true, pl =>
            {
                pl.AddToBackpack(new Hamburgers());
                pl.SendMessage("You receive some Hamburgers!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a CartographyDesk.");
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