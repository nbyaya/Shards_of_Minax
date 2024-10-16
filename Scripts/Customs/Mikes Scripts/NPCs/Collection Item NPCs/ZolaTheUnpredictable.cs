using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZolaTheUnpredictable : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZolaTheUnpredictable() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zola the Unpredictable";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyDress(1153)); // A bright yellow dress
        AddItem(new Sandals(33)); // Bright red sandals
        AddItem(new FeatheredHat(2118)); // A flamboyant purple feathered hat
        AddItem(new Cloak(1436)); // A bright green cloak

        VirtualArmor = 20;
    }

    public ZolaTheUnpredictable(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a brave soul approaches! I am Zola, known for my peculiar tastes and stranger trades. Are you perhaps carrying something unusual, like Steroids?");

        // Dialogue options
        greeting.AddOption("What do you want with Steroids?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Why, I have my reasons! Bring me Steroids, and I shall offer you a choice: a MilkingPail or some CowPoo. Of course, I will also give you a MaxxiaScroll for your troubles."
                + " But remember, child, the winds whisper secrets of those who stray too far...");

                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have the Steroids with you? Oh, how they reek of despair and power, don't they?");

                        tradeModule.AddOption("Yes, I have Steroids.", 
                            plaa => HasSteroids(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });

                        tradeModule.AddOption("No, I don't have any Steroids right now.", 
                            plaa => !HasSteroids(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have Steroids, dear. But hurry, the sands of time slip ever away.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes, child. The stars align, but only when they wish.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });

                tradeIntroductionModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });

                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Tell me about yourself, Zola.", 
            p => true, 
            p =>
            {
                DialogueModule backstoryModule = new DialogueModule("I have seen the end, child. The skies ablaze, the earth trembling, and the rivers running red. The people laughed at my words until the storms came."
                + " I was once just an old woman, but fate has a cruel sense of humor. Now, I see what others dare not. But be warned, child, knowledge has a price, and my words have led to both salvation and doom.");

                backstoryModule.AddOption("What did you see in your visions?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule visionModule = new DialogueModule("I saw shadows that stretch across the fields, consuming all in their path. I heard the wails of those who did not heed the warnings."
                        + " A beast of smoke and ash, with eyes that burn like molten iron. It will come, and only those who are prepared will stand a chance. The Steroids you bring me are for a purpose... they fortify those who will fight the darkness.");

                        visionModule.AddOption("How do I prepare?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule preparationModule = new DialogueModule("You must listen closely. Gather strength, gather allies, and arm yourself with knowledge. The MilkingPail I offer may seem humble, but even the simplest of tools can bring hope."
                                + " And CowPoo, well... it is more useful than you may think. In these times, one must be resourceful.");

                                preparationModule.AddOption("This all sounds... eerie.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendMessage("Eerie? Oh, child, the eerie is but the prelude to terror. Take heed, and make your choices wisely.");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });

                                preparationModule.AddOption("Thank you for your wisdom.", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        plaab.SendMessage("You are most welcome, dear. Remember, the winds are always listening.");
                                    });

                                plaa.SendGump(new DialogueGump(plaa, preparationModule));
                            });

                        visionModule.AddOption("I want to hear no more.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendMessage("As you wish, but do not forget my words, child. Ignorance is a shield, but a brittle one.");
                            });

                        pl.SendGump(new DialogueGump(pl, visionModule));
                    });

                backstoryModule.AddOption("I think I'll be going now.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("Run along then, child. The winds will still whisper in your absence.");
                    });

                p.SendGump(new DialogueGump(p, backstoryModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zola waves energetically, though her eyes seem to look beyond you, at something only she can see.");
            });

        return greeting;
    }

    private bool HasSteroids(PlayerMobile player)
    {
        // Check the player's inventory for Steroids
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(Steroids)) != null;
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
        // Remove the Steroids and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item steroids = player.Backpack.FindItemByType(typeof(Steroids));
        if (steroids != null)
        {
            steroids.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose? Ah, the choices we make... they shape our future, you know.");

            // Add options for MilkingPail and CowPoo
            rewardChoiceModule.AddOption("MilkingPail", pl => true, pl => 
            {
                pl.AddToBackpack(new MilkingPail());
                pl.SendMessage("You receive a MilkingPail! It may serve you better than you expect.");
            });

            rewardChoiceModule.AddOption("CowPoo", pl => true, pl =>
            {
                pl.AddToBackpack(new CowPoo());
                pl.SendMessage("You receive some CowPoo! Strange times call for strange solutions.");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have the Steroids. Perhaps fate has other plans for you.");
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