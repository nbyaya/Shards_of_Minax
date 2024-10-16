using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TheodricTheWanderingTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TheodricTheWanderingTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Theodric the Wandering Tinkerer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(110);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 200;

        // Outfit
        AddItem(new Robe(1126)); // A deep blue robe
        AddItem(new Boots(144)); // Dark leather boots
        AddItem(new SmithHammer()); // A hammer hanging from his belt
        AddItem(new WizardsHat(1153)); // A unique tinkerer hat
        AddItem(new HalfApron(2413)); // A tinkerer-style half apron

        VirtualArmor = 15;
    }

    public TheodricTheWanderingTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Theodric, a wandering tinkerer in search of rare and curious artifacts. Do you have something interesting for me?");
        
        // Dialogue options
        greeting.AddOption("What kind of artifacts are you interested in?", 
            p => true, 
            p =>
            {
                DialogueModule artifactsModule = new DialogueModule("I am particularly interested in items of mysterious origin. One such item is a 'LexxVase'. If you happen to have one, I would be willing to offer you a choice between a FineIronWire or a HangingMask.");
                
                // Trade option after explanation
                artifactsModule.AddOption("I have a LexxVase. Would you like to trade?", 
                    pl => HasLexxVase(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                artifactsModule.AddOption("I don't have a LexxVase right now.", 
                    pl => !HasLexxVase(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a LexxVase, and we shall make a deal.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                artifactsModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                // New detailed dialogue options exploring Theodric's backstory and traits
                artifactsModule.AddOption("You seem a bit anxious. Is everything alright?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule anxiousModule = new DialogueModule("Ah, well... you noticed, did you? I suppose I can't hide it forever. You see, I have been tinkering with things far beyond my expertise—strange elixirs, mind-expanding substances. Sometimes, I see things... things I shouldn't be seeing."
                            + " The world beyond this one, glimpses of something else, perhaps another dimension. It's both fascinating and terrifying.");

                        anxiousModule.AddOption("Another dimension? Are you sure you're not just hallucinating?", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule hallucinationModule = new DialogueModule("Maybe... maybe you're right. But there's something about these visions that feels real, like I'm actually there, touching something that should remain untouched. I've spent my life trying to uncover secrets, but these might be secrets too dangerous to reveal.");
                                hallucinationModule.AddOption("Why do you continue then?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule obsessionModule = new DialogueModule("I suppose it's the curiosity that keeps me going. That, and the obsession. You see, once you peek into the unknown, it's impossible to unsee it. It haunts you, whispers to you in the dead of night. I know I should stop, but my hands won't let me.");
                                        obsessionModule.AddOption("That's... unsettling. You should seek help.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Theodric sighs deeply, his eyes distant. 'Perhaps you're right, traveler. Perhaps there is no peace down this path.'");
                                            });
                                        obsessionModule.AddOption("I understand. Knowledge can be an insatiable hunger.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Theodric smiles weakly. 'Yes, yes, exactly that. You understand, don't you? The quest for knowledge has its price, and I'm willing to pay it.'");
                                            });
                                        plll.SendGump(new DialogueGump(plll, obsessionModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, hallucinationModule));
                            });

                        anxiousModule.AddOption("Is that why you're wandering? To find answers?", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule wanderingModule = new DialogueModule("Yes, precisely. I wander because I need to know if anyone else has seen what I've seen, or if I'm truly losing my mind. There are whispers of a hidden knowledge, ancient texts that might shed light on my condition."
                                    + " I search for those who understand—those who have glimpsed beyond.");
                                wanderingModule.AddOption("What happens if you find these answers?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule outcomeModule = new DialogueModule("I don't know. Maybe I'll finally find peace, or maybe the answers will drive me even further into madness. But one thing is for certain: I can't stop now. Not when I'm so close.");
                                        outcomeModule.AddOption("I wish you luck, Theodric.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Theodric nods solemnly, his eyes filled with both hope and fear. 'Thank you, traveler. I hope I can find what I'm searching for.'");
                                            });
                                        outcomeModule.AddOption("Be careful. Some doors are best left closed.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Theodric shivers slightly, as if a chill had passed over him. 'Wise words, traveler. I only hope I have the strength to heed them.'");
                                            });
                                        plll.SendGump(new DialogueGump(plll, outcomeModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, wanderingModule));
                            });

                        pl.SendGump(new DialogueGump(pl, anxiousModule));
                    });

                p.SendGump(new DialogueGump(p, artifactsModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Theodric nods and continues tinkering with his tools.");
            });

        return greeting;
    }

    private bool HasLexxVase(PlayerMobile player)
    {
        // Check the player's inventory for LexxVase
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LexxVase)) != null;
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
        // Remove the LexxVase and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item lexxVase = player.Backpack.FindItemByType(typeof(LexxVase));
        if (lexxVase != null)
        {
            lexxVase.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for FineIronWire and HangingMask
            rewardChoiceModule.AddOption("FineIronWire", pl => true, pl => 
            {
                pl.AddToBackpack(new FineIronWire());
                pl.SendMessage("You receive a FineIronWire!");
            });

            rewardChoiceModule.AddOption("HangingMask", pl => true, pl =>
            {
                pl.AddToBackpack(new HangingMask());
                pl.SendMessage("You receive a HangingMask!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a LexxVase.");
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