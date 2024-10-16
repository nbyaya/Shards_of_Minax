using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class NozomiTheForlornJester : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NozomiTheForlornJester() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Nozomi the Forlorn Jester";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(80);
        SetInt(120);

        SetHits(70);
        SetMana(180);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new JesterSuit(1153)); // A brightly colored jester suit
        AddItem(new JesterHat(1153)); // A matching jester hat
        AddItem(new Sandals(144)); // Bright yellow sandals
        AddItem(new GoldBracelet()); // A golden bracelet as an accessory

        VirtualArmor = 15;
    }

    public NozomiTheForlornJester(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Nozomi, once the laughter of courts, now a wanderer of forgotten roads. Tell me, have you ever seen a Jester's Skull?");
        
        // Dialogue options
        greeting.AddOption("Tell me about the Jester's Skull.", 
            p => true, 
            p =>
            {
                DialogueModule skullStoryModule = new DialogueModule("The Jester's Skull is an oddity, a relic from a time when fools like me danced for royalty. If you possess one, I might have something special for you.");
                
                // Deeply nested dialogue options for lore
                skullStoryModule.AddOption("Why are you interested in the Jester's Skull?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule interestModule = new DialogueModule("The Jester's Skull is no mere trinket. It carries with it the memories of a time long gone, a cursed symbol of joy turned to sorrow. The skull belonged to the very first royal jester, a man who made the world laugh but wept in solitude. I feel... a connection to it.");
                        
                        interestModule.AddOption("You feel connected to it? Why?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule connectionModule = new DialogueModule("I too was once a royal entertainer. I danced, sang, and made merry for the court, hiding the truth of my nature. You see, I am cursed. The witch's spell binds me to this form, my true identity concealed beneath the facade of a fool.");
                                
                                connectionModule.AddOption("What was your true identity?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule identityModule = new DialogueModule("I was once Prince Nozomu, heir to a prosperous kingdom. My pride led me to spurn the love of a witch who sought my favor. In her wrath, she cursed me to live as a jester, forever shunned by those who once revered me.");
                                        
                                        identityModule.AddOption("Do you wish to break the curse?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule breakCurseModule = new DialogueModule("Of course I do. Yet, the way is not simple. The curse can only be broken by an act of true compassion, one that comes from a heart untainted by pride or malice. That is why I seek the Jester's Skull, for it may hold the key to my redemption.");
                                                
                                                breakCurseModule.AddOption("How can I help you break the curse?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("If you bring me the Jester's Skull, I can try to unlock its secrets. Perhaps together, we can find a way to undo the witch's spell. But be warned, this path is fraught with danger, and the witch herself may not be pleased with our defiance.");
                                                        
                                                        helpModule.AddOption("I will help you.", 
                                                            plaaaaa => HasJesterSkull(plaaaaa) && CanTradeWithPlayer(plaaaaa), 
                                                            plaaaaa =>
                                                            {
                                                                CompleteTrade(plaaaaa);
                                                            });
                                                        
                                                        helpModule.AddOption("I will return when I am ready.", 
                                                            plaaaaa => !HasJesterSkull(plaaaaa), 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Very well. Remember, the path to redemption is not one that can be walked alone. Come back when you have the Jester's Skull.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, breakCurseModule));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, identityModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, connectionModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, interestModule));
                    });
                
                // Trade option after story
                skullStoryModule.AddOption("I have a JesterSkull. Can we trade?", 
                    pl => HasJesterSkull(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                skullStoryModule.AddOption("I have a JesterSkull, but recently traded.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                skullStoryModule.AddOption("I don't have a JesterSkull.", 
                    pl => !HasJesterSkull(pl), 
                    pl =>
                    {
                        pl.SendMessage("No worries, friend. Return if you ever find one.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, skullStoryModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Nozomi waves sadly, a trace of her former joy in her eyes.");
            });

        return greeting;
    }

    private bool HasJesterSkull(PlayerMobile player)
    {
        // Check the player's inventory for JesterSkull
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(JesterSkull)) != null;
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
        // Remove the JesterSkull and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item jesterSkull = player.Backpack.FindItemByType(typeof(JesterSkull));
        if (jesterSkull != null)
        {
            jesterSkull.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for GamerGirlFeet and HildebrandtTapestry
            rewardChoiceModule.AddOption("GamerGirlFeet", pl => true, pl => 
            {
                pl.AddToBackpack(new GamerGirlFeet());
                pl.SendMessage("You receive GamerGirlFeet!");
            });
            
            rewardChoiceModule.AddOption("HildebrandtTapestry", pl => true, pl =>
            {
                pl.AddToBackpack(new HildebrandtTapestry());
                pl.SendMessage("You receive a HildebrandtTapestry!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a JesterSkull.");
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