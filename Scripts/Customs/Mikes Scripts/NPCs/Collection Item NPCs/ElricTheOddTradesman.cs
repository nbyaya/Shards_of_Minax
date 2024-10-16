using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ElricTheOddTradesman : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ElricTheOddTradesman() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elric the Odd Tradesman";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(2957)); // A bright purple fancy shirt
        AddItem(new LongPants(1124)); // Dark blue pants
        AddItem(new Sandals(1358)); // Yellow sandals
        AddItem(new Cloak(1175)); // A cloak in a deep green
        AddItem(new TricorneHat(1153)); // A vibrant red tricorne hat
        AddItem(new Lantern()); // Carries a lantern, always lit

        VirtualArmor = 15;
    }

    public ElricTheOddTradesman(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Elric, a tradesman of rare and peculiar goods. Do you fancy something a bit... unconventional?");

        // Dialogue options
        greeting.AddOption("What sort of goods do you trade?",
            p => true,
            p =>
            {
                DialogueModule tradeInfoModule = new DialogueModule("Ah, I deal in items that most wouldn't dare touch. I have an eye for the odd and the extraordinary. Do you happen to have a TwentyfiveShield? I could offer you a unique trade.");

                // Trade option after story
                tradeInfoModule.AddOption("What kind of trade are you offering?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you bring me a TwentyfiveShield, I shall let you choose between a DaggerSign or a MultiPump. Additionally, I always provide a MaxxiaScroll as a token of my gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a TwentyfiveShield for me?");
                                tradeModule.AddOption("Yes, I have a TwentyfiveShield.",
                                    plaa => HasTwentyfiveShield(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasTwentyfiveShield(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a TwentyfiveShield.");
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

                tradeInfoModule.AddOption("Why do you trade such strange items?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah... a fair question, indeed. I was once a man of faith, a leader, a priest. My devotion was steadfast, and my followers revered me. But power, true power, is fleeting, and I found myself drawn to... forbidden practices. Practices that could restore what I had lost.");

                        backstoryModule.AddOption("Forbidden practices? What do you mean?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule corruptionModule = new DialogueModule("Yes, forbidden. I sought ancient tomes, relics of dark magic, anything that could make me whole again. It was not without cost. My spirit is conflicted—torn between the light I once knew and the shadows I now walk.");
                                corruptionModule.AddOption("Do you regret your choices?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret... it is a heavy burden, one I carry each day. I regret the loss of my purity, the trust of my followers. But there is also a part of me that craves the power I have touched. It whispers to me, even now.");
                                        regretModule.AddOption("Is there a way back for you?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule redemptionModule = new DialogueModule("Perhaps there is, but the path is not clear. The light I once followed seems so distant, and the shadows are ever closer. I do what I must to survive, and perhaps, one day, I shall find the strength to turn away from the darkness.");
                                                redemptionModule.AddOption("I hope you find your way, Elric.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Elric's eyes soften for a moment, and he nods. 'Thank you, traveler. Perhaps there is hope yet.'");
                                                    });
                                                redemptionModule.AddOption("The darkness has its own power. Embrace it.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Elric's eyes glint with a strange light. 'You understand, don't you? The power... it is intoxicating. Perhaps you and I are not so different.'");
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, redemptionModule));
                                            });
                                        regretModule.AddOption("Power is worth any price.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Elric smiles darkly. 'Spoken like someone who knows the value of strength. The world is harsh, and only the powerful thrive.'");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, regretModule));
                                    });
                                corruptionModule.AddOption("Power always comes with a cost.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Elric nods solemnly. 'Indeed, and I have paid dearly. But sometimes, the cost is worth the reward, if one is willing to bear it.'");
                                    });
                                pla.SendGump(new DialogueGump(pla, corruptionModule));
                            });
                        backstoryModule.AddOption("It sounds like a difficult path.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Elric sighs, his eyes distant. 'Difficult, yes. But it is the path I chose, and I must see it through.'");
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                tradeInfoModule.AddOption("Can you teach me about these forbidden practices?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule teachingModule = new DialogueModule("Teach you? Hah, you are either brave or foolish. The knowledge I possess is dangerous. It corrupts, twists the soul. But perhaps... if you are truly willing, I could share a fragment.");
                        teachingModule.AddOption("I am willing to learn.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Elric grins, a hint of madness in his eyes. 'Very well. Seek out the Obsidian Tome in the ruins west of here. It will begin your journey, but heed my warning—there is no turning back once the first step is taken.'");
                            });
                        teachingModule.AddOption("No, I value my soul too much.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Elric nods approvingly. 'A wise choice. Not all knowledge is meant to be grasped.'");
                            });
                        pl.SendGump(new DialogueGump(pl, teachingModule));
                    });

                p.SendGump(new DialogueGump(p, tradeInfoModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Elric nods and smiles mysteriously.");
            });

        return greeting;
    }

    private bool HasTwentyfiveShield(PlayerMobile player)
    {
        // Check the player's inventory for TwentyfiveShield
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(TwentyfiveShield)) != null;
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
        // Remove the TwentyfiveShield and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item twentyfiveShield = player.Backpack.FindItemByType(typeof(TwentyfiveShield));
        if (twentyfiveShield != null)
        {
            twentyfiveShield.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for DaggerSign and MultiPump
            rewardChoiceModule.AddOption("DaggerSign", pl => true, pl =>
            {
                pl.AddToBackpack(new DaggerSign());
                pl.SendMessage("You receive a DaggerSign!");
            });

            rewardChoiceModule.AddOption("MultiPump", pl => true, pl =>
            {
                pl.AddToBackpack(new MultiPump());
                pl.SendMessage("You receive a MultiPump!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a TwentyfiveShield.");
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