using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class LianaThePotionmaster : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LianaThePotionmaster() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Liana the Potionmaster";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(50);
        SetInt(150);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2357)); // A deep purple robe
        AddItem(new Sandals(1109)); // Dark green sandals
        AddItem(new WizardsHat(1157)); // Matching purple wizard's hat
        AddItem(new GoldBracelet()); // A simple gold bracelet
        AddItem(new CandleStick()); // A belt filled with potion bottles

        VirtualArmor = 12;
    }

    public LianaThePotionmaster(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Liana, a master of potions and alchemy. Tell me, do you have the eye for rare and curious ingredients?");

        // Dialogue options
        greeting.AddOption("What kind of ingredients do you need?",
            p => true,
            p =>
            {
                DialogueModule ingredientsModule = new DialogueModule("I am always in need of ReactiveHormones. They are so very rare and essential for my most potent elixirs. If you have one, I would gladly offer you a choice of a reward for your generosity. But beware, my situation is... complicated.");

                ingredientsModule.AddOption("Complicated? What do you mean?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Indeed... I have been accused of witchcraft, falsely so, yet the ignorant masses know no distinction between good and evil. I live in hiding, always fearful of those who seek to harm me, but I will not give up. I use my powers to help those in need, even if it puts me in great danger.");

                        backstoryModule.AddOption("Who accused you?",
                            pl2 => true,
                            pl2 =>
                            {
                                DialogueModule accusedModule = new DialogueModule("A group of so-called 'witch hunters'—people driven by fear and hatred. They seek to control what they do not understand. They have ruined my life, but they will not have my spirit.");

                                accusedModule.AddOption("That sounds awful. How do you survive?",
                                    pl3 => true,
                                    pl3 =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("It has not been easy. I must always stay on the move, hiding in the shadows, using my skills in alchemy to survive. I gather herbs by moonlight and create potions to heal myself and those I can trust. I rely on people like you—those willing to look beyond the rumors and see who I truly am.");

                                        survivalModule.AddOption("I admire your resilience.",
                                            pl4 => true,
                                            pl4 =>
                                            {
                                                DialogueModule admirationModule = new DialogueModule("Thank you, traveler. It is rare to find kindness in these times. It is for people like you that I keep going, even when it seems impossible.");
                                                pl4.SendGump(new DialogueGump(pl4, admirationModule));
                                            });

                                        survivalModule.AddOption("Can I help you in any way?",
                                            pl4 => true,
                                            pl4 =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("If you truly wish to help, bring me ReactiveHormones. They are essential for my work and can greatly aid me in my continued survival. In exchange, I will reward you well.");
                                                pl4.SendGump(new DialogueGump(pl4, helpModule));
                                            });

                                        pl3.SendGump(new DialogueGump(pl3, survivalModule));
                                    });

                                accusedModule.AddOption("Do you plan to take revenge?",
                                    pl3 => true,
                                    pl3 =>
                                    {
                                        DialogueModule revengeModule = new DialogueModule("Revenge is a dangerous path, but one that I sometimes consider. Those who wronged me deserve justice. Yet, for now, my priority is to protect myself and those who still believe in goodness. I dream of a day when I can stand openly, without fear, but that day is not yet here.");
                                        pl3.SendGump(new DialogueGump(pl3, revengeModule));
                                    });

                                pl2.SendGump(new DialogueGump(pl2, accusedModule));
                            });

                        backstoryModule.AddOption("How can you continue helping others despite the danger?",
                            pl2 => true,
                            pl2 =>
                            {
                                DialogueModule helpingModule = new DialogueModule("I believe that the world is still worth saving. Every time I heal someone, every time I help someone in need, it reminds me why I chose this path. The fear, the hiding—it is all worth it, if I can bring hope to even a single soul.");

                                helpingModule.AddOption("You're very brave, Liana.",
                                    pl3 => true,
                                    pl3 =>
                                    {
                                        DialogueModule braveModule = new DialogueModule("Bravery is simply the decision to act in the face of fear. I am terrified every day, but I refuse to let that stop me. Thank you for seeing me for who I truly am.");
                                        pl3.SendGump(new DialogueGump(pl3, braveModule));
                                    });

                                pl2.SendGump(new DialogueGump(pl2, helpingModule));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                // Trade option after detailed story
                ingredientsModule.AddOption("Do you want to make a trade?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have ReactiveHormones, I can offer you a CandleStick or LuckyDice in exchange. You'll also get a special scroll as a token of my appreciation. You should know, however, that I can only do this once every 10 minutes—lest I draw too much attention.");
                        tradeIntroductionModule.AddOption("I'd like to trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have ReactiveHormones for me?");
                                tradeModule.AddOption("Yes, I have ReactiveHormones.",
                                    plaa => HasReactiveHormones(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have any right now.",
                                    plaa => !HasReactiveHormones(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have ReactiveHormones. My offer still stands.");
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

                p.SendGump(new DialogueGump(p, ingredientsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Liana nods knowingly, her eyes glinting with curiosity and a hint of sadness.");
            });

        return greeting;
    }

    private bool HasReactiveHormones(PlayerMobile player)
    {
        // Check the player's inventory for ReactiveHormones
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ReactiveHormones)) != null;
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
        // Remove the ReactiveHormones and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item reactiveHormones = player.Backpack.FindItemByType(typeof(ReactiveHormones));
        if (reactiveHormones != null)
        {
            reactiveHormones.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for CandleStick and LuckyDice
            rewardChoiceModule.AddOption("CandleStick", pl => true, pl =>
            {
                pl.AddToBackpack(new CandleStick());
                pl.SendMessage("You receive a CandleStick!");
            });

            rewardChoiceModule.AddOption("LuckyDice", pl => true, pl =>
            {
                pl.AddToBackpack(new LuckyDice());
                pl.SendMessage("You receive LuckyDice!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have ReactiveHormones.");
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