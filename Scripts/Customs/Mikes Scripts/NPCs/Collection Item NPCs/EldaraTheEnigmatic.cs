using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class EldaraTheEnigmatic : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EldaraTheEnigmatic() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Eldara the Enigmatic";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit - unique and mysterious attire
        AddItem(new HoodedShroudOfShadows()); // A dark hooded shroud
        AddItem(new Sandals(1153)); // Deep blue sandals
        AddItem(new GnarledStaff()); // A gnarled staff, hinting at her magical prowess

        VirtualArmor = 15;
    }

    public EldaraTheEnigmatic(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Eldara, seeker of hidden knowledge. Have you come to exchange secrets with me?");

        // Dialogue options
        greeting.AddOption("Tell me more about your secrets.",
            p => true,
            p =>
            {
                DialogueModule secretsModule = new DialogueModule("The world is filled with mysteries, and I have collected many. However, I am in search of something rare: a FancyCopperSunflower. If you possess it, I am prepared to offer you a choice of rewards.");

                secretsModule.AddOption("Why do you seek the FancyCopperSunflower?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah, the FancyCopperSunflower... It is no mere trinket. It is said to embody hope, a reminder of what has been lost. I once served as a priest, guiding the lost and downtrodden, offering them hope. But that was before the world turned cold, and my faith was shattered. Now, I seek the sunflower as a small flicker of what I once believed.");

                        backstoryModule.AddOption("What happened to your faith?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule faithLostModule = new DialogueModule("I once stood at the head of a congregation, a beacon of light in dark times. But tragedy struck. I lost them all, one by one, to the chaos of this world. I prayed for guidance, for intervention, but my pleas went unanswered. My faith crumbled as I buried those who looked to me for solace.");

                                faithLostModule.AddOption("I am sorry for your loss. How do you continue on?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule continueOnModule = new DialogueModule("Compassion, I suppose. Even in despair, I cannot abandon those in need. I still offer solace to the weary, even if my own hope has waned. The FancyCopperSunflower, perhaps, could rekindle something within me. Or perhaps it is simply a foolish pursuit.");

                                        continueOnModule.AddOption("It's not foolish to seek hope.",
                                            plaap => true,
                                            plaap =>
                                            {
                                                DialogueModule hopeModule = new DialogueModule("Perhaps you are right, traveler. Hope, no matter how small, may yet have power. If you possess the FancyCopperSunflower, I will gladly offer you something that may aid you in your journey.");
                                                hopeModule.AddOption("I have a FancyCopperSunflower. What can you offer?",
                                                    plaapp => true,
                                                    plaapp =>
                                                    {
                                                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a FancyCopperSunflower, you may choose between a ForbiddenTome or a FancyCrystalSkull. Additionally, I shall grant you a MaxxiaScroll, a scroll of forgotten lore.");
                                                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                                                            plaappp => CanTradeWithPlayer(plaappp),
                                                            plaappp =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Do you have a FancyCopperSunflower for me?");
                                                                tradeModule.AddOption("Yes, I have a FancyCopperSunflower.",
                                                                    plaaappp => HasFancyCopperSunflower(plaaappp) && CanTradeWithPlayer(plaaappp),
                                                                    plaaappp =>
                                                                    {
                                                                        CompleteTrade(plaaappp);
                                                                    });
                                                                tradeModule.AddOption("No, I don't have one right now.",
                                                                    plaaappp => !HasFancyCopperSunflower(plaaappp),
                                                                    plaaappp =>
                                                                    {
                                                                        plaaappp.SendMessage("Return when you have obtained a FancyCopperSunflower.");
                                                                        plaaappp.SendGump(new DialogueGump(plaaappp, CreateGreetingModule(plaaappp)));
                                                                    });
                                                                tradeModule.AddOption("I traded recently; I'll come back later.",
                                                                    plaaappp => !CanTradeWithPlayer(plaaappp),
                                                                    plaaappp =>
                                                                    {
                                                                        plaaappp.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                        plaaappp.SendGump(new DialogueGump(plaaappp, CreateGreetingModule(plaaappp)));
                                                                    });
                                                                plaappp.SendGump(new DialogueGump(plaappp, tradeModule));
                                                            });
                                                        tradeIntroductionModule.AddOption("Perhaps another time.",
                                                            plaappp => true,
                                                            plaappp =>
                                                            {
                                                                plaappp.SendGump(new DialogueGump(plaappp, CreateGreetingModule(plaappp)));
                                                            });
                                                        plaapp.SendGump(new DialogueGump(plaapp, tradeIntroductionModule));
                                                    });
                                                plaap.SendGump(new DialogueGump(plaap, hopeModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, continueOnModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, faithLostModule));
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                secretsModule.AddOption("I have a FancyCopperSunflower. What can you offer?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a FancyCopperSunflower, you may choose between a ForbiddenTome or a FancyCrystalSkull. Additionally, I shall grant you a MaxxiaScroll, a scroll of forgotten lore.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a FancyCopperSunflower for me?");
                                tradeModule.AddOption("Yes, I have a FancyCopperSunflower.",
                                    plaa => HasFancyCopperSunflower(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasFancyCopperSunflower(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Return when you have obtained a FancyCopperSunflower.");
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
                        tradeIntroductionModule.AddOption("Perhaps another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, secretsModule));
            });

        greeting.AddOption("Farewell, Eldara.",
            p => true,
            p =>
            {
                p.SendMessage("Eldara nods, her eyes glimmering with arcane knowledge.");
            });

        return greeting;
    }

    private bool HasFancyCopperSunflower(PlayerMobile player)
    {
        // Check the player's inventory for FancyCopperSunflower
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FancyCopperSunflower)) != null;
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
        // Remove the FancyCopperSunflower and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fancyCopperSunflower = player.Backpack.FindItemByType(typeof(FancyCopperSunflower));
        if (fancyCopperSunflower != null)
        {
            fancyCopperSunflower.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ForbiddenTome and FancyCrystalSkull
            rewardChoiceModule.AddOption("ForbiddenTome", pl => true, pl =>
            {
                pl.AddToBackpack(new ForbiddenTome());
                pl.SendMessage("You receive a ForbiddenTome, filled with dark knowledge.");
            });

            rewardChoiceModule.AddOption("FancyCrystalSkull", pl => true, pl =>
            {
                pl.AddToBackpack(new FancyCrystalSkull());
                pl.SendMessage("You receive a FancyCrystalSkull, glowing with strange energies.");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a FancyCopperSunflower.");
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