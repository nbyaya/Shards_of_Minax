using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class LucianTheCuriousAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LucianTheCuriousAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lucian the Curious Alchemist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1157)); // A dark blue robe
        AddItem(new Sandals(1161)); // Light blue sandals
        AddItem(new WizardsHat(1150)); // A mystical wizard's hat
        AddItem(new GoldNecklace()); // A shiny gold necklace

        VirtualArmor = 12;
    }

    public LucianTheCuriousAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Lucian, an alchemist seeking rare ingredients and knowledge. Perhaps you have something of interest for me?");

        // Dialogue options
        greeting.AddOption("What are you looking for?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I am in need of a particular item known as CompoundF. If you happen to have one, I can offer you a choice between MonsterBones or a MagicOrb in exchange.");

                // Nested dialogue about history and madness
                tradeIntroductionModule.AddOption("Why are you interested in CompoundF?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah, CompoundF... it's not just an alchemical reagent, you see. It's a key to unlocking something much older, something buried beneath the surface of history. I once was a scholar, you know, a historian delving into the ancient civilizations of the world. But the things I uncovered... they haunt me to this day.");

                        backstoryModule.AddOption("What did you uncover?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule horrorModule = new DialogueModule("The ancient texts spoke of civilizations that thrived long before our own. They were powerful, yes, but twisted. They delved into the arcane, into things man was never meant to touch. I found notes of rituals, of beings beyond comprehension—things that dwell beneath our feet, in the dark places of the earth. I became obsessed, you see, driven to learn more, until... until I realized some knowledge is a curse.");

                                horrorModule.AddOption("That sounds terrifying. Why keep going?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule obsessionModule = new DialogueModule("Terrifying? Yes, it is. But I can't stop. The truth gnaws at my mind, a constant itch I can't scratch. I must know more. I must understand what lies beneath, even if it drives me mad. And CompoundF... it is a piece of that puzzle. Perhaps it can help me, or perhaps it will only deepen my torment. But I must try.");

                                        obsessionModule.AddOption("I have a CompoundF. What will you give me for it?",
                                            pllll => HasCompoundF(pllll) && CanTradeWithPlayer(pllll),
                                            pllll =>
                                            {
                                                CompleteTrade(pllll);
                                            });

                                        obsessionModule.AddOption("I don't have CompoundF right now.",
                                            pllll => !HasCompoundF(pllll),
                                            pllll =>
                                            {
                                                pllll.SendMessage("Come back when you have a CompoundF. I am always interested in a good trade!");
                                                pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule(pllll)));
                                            });

                                        obsessionModule.AddOption("I recently traded, I'll return later.",
                                            pllll => !CanTradeWithPlayer(pllll),
                                            pllll =>
                                            {
                                                pllll.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule(pllll)));
                                            });

                                        plll.SendGump(new DialogueGump(plll, obsessionModule));
                                    });

                                horrorModule.AddOption("That sounds fascinating. Tell me more.",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule detailedHistoryModule = new DialogueModule("The ancient civilizations were not like us. They communicated with forces that dwell beyond the stars, and the things they learned granted them power... but at a cost. The price was their sanity, their very souls. They built monuments that still stand today, hidden in the far reaches of the world, each a gateway to something dark and forgotten. I found one such place, and what I saw there... it cannot be unseen.");

                                        detailedHistoryModule.AddOption("I must be going now.",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendMessage("Lucian nods, his eyes filled with a mix of fear and longing. 'Perhaps it is for the best,' he mutters.");
                                            });

                                        plll.SendGump(new DialogueGump(plll, detailedHistoryModule));
                                    });

                                pll.SendGump(new DialogueGump(pll, horrorModule));
                            });

                        backstoryModule.AddOption("That sounds like a lot to handle.",
                            pll => true,
                            pll =>
                            {
                                pll.SendMessage("Lucian nods nervously, his hands trembling slightly. 'Yes, yes it is. But I cannot stop. I am too far gone now.'");
                                pll.SendGump(new DialogueGump(pll, CreateGreetingModule(pll)));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                // Trade option after story
                tradeIntroductionModule.AddOption("I have a CompoundF. What will you give me for it?",
                    pl => HasCompoundF(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                tradeIntroductionModule.AddOption("I don't have CompoundF right now.",
                    pl => !HasCompoundF(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a CompoundF. I am always interested in a good trade!");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                tradeIntroductionModule.AddOption("I recently traded, I'll return later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Lucian nods, his eyes still glinting with curiosity.");
            });

        return greeting;
    }

    private bool HasCompoundF(PlayerMobile player)
    {
        // Check the player's inventory for CompoundF
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CompoundF)) != null;
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
        // Remove the CompoundF and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item compoundF = player.Backpack.FindItemByType(typeof(CompoundF));
        if (compoundF != null)
        {
            compoundF.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for MonsterBones and MagicOrb
            rewardChoiceModule.AddOption("MonsterBones", pl => true, pl =>
            {
                pl.AddToBackpack(new MonsterBones());
                pl.SendMessage("You receive a set of MonsterBones!");
            });

            rewardChoiceModule.AddOption("MagicOrb", pl => true, pl =>
            {
                pl.AddToBackpack(new MagicOrb());
                pl.SendMessage("You receive a MagicOrb!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a CompoundF.");
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