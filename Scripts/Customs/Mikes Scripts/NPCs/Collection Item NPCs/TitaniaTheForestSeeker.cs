using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class TitaniaTheForestSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TitaniaTheForestSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Titania the Forest Seeker";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(80);
        SetInt(120);

        SetHits(90);
        SetMana(200);
        SetStam(70);

        Fame = 100;
        Karma = 200;

        // Outfit
        AddItem(new Cap(0x28E)); // Green cloak made of enchanted leaves
        AddItem(new Sandals(0x59B)); // Light sandals, forest-themed
        AddItem(new PlainDress(0x7DA)); // Earthy brown skirt
        AddItem(new FlowerGarland(0x485)); // Garland of forest flowers
        AddItem(new WildStaff()); // A staff carved from ancient wood

        VirtualArmor = 20;
    }

    public TitaniaTheForestSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Titania, a seeker of knowledge in the ways of the forest. Do you have a moment to discuss nature's wonders?");

        // Dialogue options
        greeting.AddOption("Tell me about your quest.",
            p => true,
            p =>
            {
                DialogueModule questModule = new DialogueModule("I wander these lands, seeking the secrets hidden in the trees and whispering among the winds. But alas, the city folk rarely value such pursuits.");

                questModule.AddOption("Why do they fear you?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule fearModule = new DialogueModule("Ah, the fear of the unknown. They fear what they cannot control. I possess knowledge that challenges their simple views of the world. They call it dark magic, but I call it understanding the power that lurks within nature. Those who seek my help often pay a price far more profound than gold.");

                        fearModule.AddOption("Dark magic? What do you mean?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule darkMagicModule = new DialogueModule("Yes, I dabble in magic that others dare not touch. The essence of curses, potions brewed with forbidden herbs, whispers of long-forgotten entities... it is dangerous knowledge, alluring, and powerful. But remember, power comes at a cost. Those who seek my help must be prepared to part with more than coin.");

                                darkMagicModule.AddOption("What sort of potions do you sell?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule potionModule = new DialogueModule("I craft potions for every desire: love potions that bewitch the heart, poisons that corrupt the soul, elixirs that grant sight beyond sight. But beware, traveler, these concoctions are not without consequence. The mind is a fragile thing, and power can shatter it like glass.");

                                        potionModule.AddOption("I am interested in a potion.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule potionChoiceModule = new DialogueModule("Tell me, do you seek love, revenge, or perhaps a glimpse into the future? Each comes with a price.");

                                                potionChoiceModule.AddOption("A love potion.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Titania smirks, her eyes glinting with mischief. 'Love is a dangerous game, traveler. I can give you what you seek, but be warned—the heart that is enchanted may not be easily released.'");
                                                        plaaaa.AddToBackpack(new FairyOil());
                                                    });

                                                potionChoiceModule.AddOption("A potion of revenge.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Titania's smile darkens, and her voice lowers to a whisper. 'Vengeance is a dish best served cold, they say. My potion will ensure your enemies know fear, but be careful—you may find yourself lost in the darkness it brings.'");
                                                        plaaaa.AddToBackpack(new CompoundF());
                                                    });

                                                potionChoiceModule.AddOption("A glimpse into the future.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Titania takes a deep breath, her expression becoming unreadable. 'The future is a twisted path, full of shadows and mirrors. Are you sure you wish to see what lies ahead? Knowledge of the future is a heavy burden.'");
                                                        plaaaa.AddToBackpack(new DistilledEssence());
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, potionChoiceModule));
                                            });

                                        potionModule.AddOption("Perhaps not. The risk is too great.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Wise choice, traveler. Power is seductive, but the cost can be too high.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, potionModule));
                                    });

                                darkMagicModule.AddOption("What sort of curses do you cast?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule curseModule = new DialogueModule("Curses are my specialty. From minor hexes that cause ill fortune to powerful spells that twist a person's fate. Those who cross me or my allies rarely live a life without regret. But remember, once a curse is cast, it binds not only the victim but often the caster as well.");

                                        curseModule.AddOption("Could you curse someone for me?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Titania narrows her eyes, studying you carefully. 'To bind someone with a curse is not to be taken lightly. I can do it, yes, but the price will be dear. Are you certain you wish to proceed?' Perhaps a fragment of your soul, or a memory you hold dear, would suffice.");
                                            });

                                        curseModule.AddOption("No, I do not wish to curse anyone.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("A wise decision. Curses are not easily undone, and they bind more than just the target.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, curseModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, darkMagicModule));
                            });

                        fearModule.AddOption("Why would anyone seek your help then?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule helpModule = new DialogueModule("Ah, but that is the beauty of power. Those who come to me are desperate, lost, or blinded by ambition. They seek that which they cannot find elsewhere—love, revenge, knowledge, or power. And I, dear traveler, offer them what they desire, though they rarely comprehend the true cost.");
                                pla.SendGump(new DialogueGump(pla, helpModule));
                            });

                        pl.SendGump(new DialogueGump(pl, fearModule));
                    });

                // Trade option after story
                questModule.AddOption("Is there anything you need for your quest?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed there is! If you have a DailyNewspaper, I would be happy to trade something special for it. Choose between a FunPumpkinCannon or a VirtueRune.");
                        tradeIntroductionModule.AddOption("I have a DailyNewspaper and I'd like to trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a DailyNewspaper for me?");
                                tradeModule.AddOption("Yes, I have a DailyNewspaper.",
                                    plaa => HasDailyNewspaper(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasDailyNewspaper(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a DailyNewspaper.");
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

                p.SendGump(new DialogueGump(p, questModule));
            });

        greeting.AddOption("Farewell.",
            p => true,
            p =>
            {
                p.SendMessage("Titania bows slightly, her garland of flowers swaying gently.");
            });

        return greeting;
    }

    private bool HasDailyNewspaper(PlayerMobile player)
    {
        // Check the player's inventory for DailyNewspaper
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DailyNewspaper)) != null;
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
        // Remove the DailyNewspaper and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item dailyNewspaper = player.Backpack.FindItemByType(typeof(DailyNewspaper));
        if (dailyNewspaper != null)
        {
            dailyNewspaper.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for FunPumpkinCannon and VirtueRune
            rewardChoiceModule.AddOption("FunPumpkinCannon", pl => true, pl =>
            {
                pl.AddToBackpack(new FunPumpkinCannon());
                pl.SendMessage("You receive a FunPumpkinCannon!");
            });

            rewardChoiceModule.AddOption("VirtueRune", pl => true, pl =>
            {
                pl.AddToBackpack(new VirtueRune());
                pl.SendMessage("You receive a VirtueRune!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a DailyNewspaper.");
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