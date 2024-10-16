using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SeraphinaTheWanderingBrewer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SeraphinaTheWanderingBrewer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Seraphina the Wandering Brewer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1358)); // A deep green robe
        AddItem(new Sandals(1175)); // Bright yellow sandals
        AddItem(new StrawHat(1109)); // A straw hat for her wandering journeys
        AddItem(new HalfApron(1446)); // A leather apron, used in her brewing process

        VirtualArmor = 15;
    }

    public SeraphinaTheWanderingBrewer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello, wanderer! I'm Seraphina, a brewer of exotic concoctions. Do you happen to have a MiniKeg for trade?");
        
        // Dialogue options
        greeting.AddOption("What kind of trade are you talking about?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntro = new DialogueModule("Ah, it's simple. I use MiniKegs in my brewing, and I'm willing to offer you a choice of delicacies in exchange. You may choose between a DemonPlatter or Hotdogs. And of course, you'll also get a MaxxiaScroll.");
                tradeIntro.AddOption("I'd like to make the trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a MiniKeg for me?");
                        tradeModule.AddOption("Yes, I have a MiniKeg.", 
                            plaa => HasMiniKeg(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasMiniKeg(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a MiniKeg for me!");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I recently traded; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                tradeIntro.AddOption("I sense there is more to you than meets the eye...", 
                    pla => true, 
                    pla =>
                    {
                        DialogueModule confessionModule = new DialogueModule("You see through me, traveler. Indeed, I am not just a brewer. Once, I was a holy woman, a priestess devoted to the light. But the light... the light abandoned me.");
                        confessionModule.AddOption("What happened to you?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule backstoryModule = new DialogueModule("I witnessed horrors that shattered my faith. The cries of the innocent, the screams of those lost to darkness—these echoed in my soul until I could no longer believe in the benevolence of the gods. I turned to other means, darker means, to understand the abyss and perhaps save those lost souls.");
                                backstoryModule.AddOption("Do you think you can save them by delving into the dark?", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule tormentedBeliefModule = new DialogueModule("I do not know. I tell myself that by understanding the darkness, I can find a way to shield others from it. But sometimes, I wonder if I am just lying to myself, if I am already lost, and this is merely an excuse to keep going.");
                                        tormentedBeliefModule.AddOption("You are conflicted. Is there still hope for you?", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                DialogueModule hopeModule = new DialogueModule("Hope... such a fragile thing. I hold on to it, though it flickers like a candle in the storm. Maybe, just maybe, through my brews, through these small acts of kindness, I can find redemption. Or perhaps, I will succumb to the abyss.");
                                                hopeModule.AddOption("Perhaps these acts of kindness are enough.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Seraphina looks at you with a flicker of gratitude in her eyes. 'Perhaps... perhaps you are right. Thank you, traveler. For now, I will keep brewing, and I will keep hoping.'");
                                                    });
                                                hopeModule.AddOption("You walk a dangerous path. Beware, Seraphina.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Seraphina nods solemnly. 'I know, traveler. The darkness is seductive, and I must be vigilant. Thank you for your concern.'");
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, hopeModule));
                                            });
                                        tormentedBeliefModule.AddOption("Perhaps it is too late for you. Be careful not to fall further.", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Seraphina sighs deeply, a shadow passing over her face. 'Perhaps... perhaps you are right. But I must keep moving forward, even if the path is uncertain.'");
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, tormentedBeliefModule));
                                    });
                                backstoryModule.AddOption("The darkness you engage in—what does it entail?", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule darkRitualsModule = new DialogueModule("I study forbidden texts, speak with spirits, and even conduct rituals under the pale moonlight. Some would call it blasphemy. I call it necessity. The abyss is vast, and there is much to learn. But it comes at a cost.");
                                        darkRitualsModule.AddOption("Do you regret it?", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                DialogueModule regretModule = new DialogueModule("Every day. There are moments when I feel the weight of my choices crushing me. But I made a vow to save those lost souls, and I cannot turn back now.");
                                                regretModule.AddOption("May you find peace one day, Seraphina.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Seraphina smiles faintly. 'Peace... it feels like a distant dream. But perhaps one day, I will find it. Thank you, traveler.'");
                                                    });
                                                regretModule.AddOption("The path you walk is perilous. Tread carefully.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Seraphina nods. 'I will. The darkness is always watching, but I will do my best to stay on my course.'");
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, regretModule));
                                            });
                                        darkRitualsModule.AddOption("You are brave, but the darkness is unforgiving.", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Seraphina gives a small, sad smile. 'The darkness does not forgive, no. But I have already given too much to turn back. Thank you for your words.'");
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, darkRitualsModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, backstoryModule));
                            });
							confessionModule.AddOption("You sound tormented. Why do you continue?",
								plaa => true,
								plaa =>
								{
									DialogueModule tormentModule = new DialogueModule("Because I must. There is a part of me that believes, even after everything, that there is a reason for my suffering. If I can save just one soul, if I can spare just one person from the horrors I witnessed, then perhaps it will all have been worth it.");
									tormentModule.AddOption("Your resolve is admirable, Seraphina.",
										plaaa => true,
										plaaa =>
										{
											plaaa.SendMessage("Seraphina smiles, a hint of warmth returning to her eyes. 'Thank you, traveler. Your words give me strength.'");
										});
									tormentModule.AddOption("But at what cost to your own soul?",
										plaaa => true,
										plaaa =>
										{
											plaaa.SendMessage("Seraphina closes her eyes, a pained expression crossing her face. 'That is the question, isn't it? I fear that my soul may already be forfeit. But if it means saving others, perhaps that is a price I am willing to pay.'");
										});
									plaa.SendGump(new DialogueGump(plaa, tormentModule));
								});

                        pla.SendGump(new DialogueGump(pla, confessionModule));
                    });
                tradeIntro.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntro));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Seraphina gives you a friendly nod and returns to her brewing.");
            });

        return greeting;
    }

    private bool HasMiniKeg(PlayerMobile player)
    {
        // Check the player's inventory for MiniKeg
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MiniKeg)) != null;
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
        // Remove the MiniKeg and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item miniKeg = player.Backpack.FindItemByType(typeof(MiniKeg));
        if (miniKeg != null)
        {
            miniKeg.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for DemonPlatter and Hotdogs
            rewardChoiceModule.AddOption("DemonPlatter", pl => true, pl => 
            {
                pl.AddToBackpack(new DemonPlatter());
                pl.SendMessage("You receive a DemonPlatter!");
            });
            
            rewardChoiceModule.AddOption("Hotdogs", pl => true, pl =>
            {
                pl.AddToBackpack(new Hotdogs());
                pl.SendMessage("You receive Hotdogs!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a MiniKeg.");
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