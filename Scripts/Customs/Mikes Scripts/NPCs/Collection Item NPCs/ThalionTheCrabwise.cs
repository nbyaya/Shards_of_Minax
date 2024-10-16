using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThalionTheCrabwise : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThalionTheCrabwise() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thalion the Crabwise";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(90);

        SetHits(100);
        SetMana(100);
        SetStam(60);

        Fame = 100;
        Karma = 100;

        // Outfit
        AddItem(new TricorneHat(1175)); // Blue tricorne hat
        AddItem(new FancyShirt(1153)); // Sea-blue shirt
        AddItem(new LongPants(1109)); // Dark green pants
        AddItem(new ThighBoots(1765)); // Weathered brown boots
        AddItem(new Cutlass()); // A cutlass for the sailor look

        VirtualArmor = 15;
    }

    public ThalionTheCrabwise(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy there, traveler! I be Thalion, the one they call 'Crabwise' for my knowledge of the scuttling folk. Do ye happen to have a CrabBushel with ye?");

        // Dialogue options
        greeting.AddOption("Tell me more about crabs.",
            p => true,
            p =>
            {
                DialogueModule crabsModule = new DialogueModule("Crabs are more than just a meal, ye see. They be creatures of cunning, and their shells hold secrets of the deep. I've spent years learnin' their ways.");

                // More in-depth crab lore options
                crabsModule.AddOption("What secrets do their shells hold?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule shellSecretsModule = new DialogueModule("Aye, their shells are like the maps of the sea. Each line and groove tells a tale of where they've been, the battles they've fought, and the currents they've braved. Only those with a keen eye can read these stories.");

                        shellSecretsModule.AddOption("Can you teach me to read these tales?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule teachingModule = new DialogueModule("Heh, it's not somethin' ye learn in a day, matey. It takes years of study, patience, and a bit of obsession. But I could teach ye a thing or two if ye bring me somethin' rare... perhaps a CrimsonCrab? They be rare indeed, and their shells hold the deepest secrets.");

                                teachingModule.AddOption("Where can I find a CrimsonCrab?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule crimsonCrabModule = new DialogueModule("The CrimsonCrab be a crafty one, found only in the darkest corners of the sea where the light barely reaches. Ye'll need courage, and maybe a bit o' madness, to venture there. The locals call it the Devil's Trench.");

                                        crimsonCrabModule.AddOption("I'll see if I can find one.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Good luck, matey. Ye'll need it.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        crimsonCrabModule.AddOption("That sounds too dangerous for me.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Heh, no shame in stayin' safe. The sea can be unforgivin'.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, crimsonCrabModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, teachingModule));
                            });

                        shellSecretsModule.AddOption("Sounds too complicated for me.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Aye, it ain't for everyone. Takes a certain kind of obsession, it does.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, shellSecretsModule));
                    });

                crabsModule.AddOption("Do you need any CrabBushels?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Aye, if ye have a CrabBushel, I'd gladly trade ye for something special. Ye can choose between a PersonalCannon or a TinCowbell, and I'll throw in a MaxxiaScroll as a bonus.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do ye have a CrabBushel for me?");
                                tradeModule.AddOption("Yes, I have a CrabBushel.",
                                    plaa => HasCrabBushel(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasCrabBushel(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when ye have a CrabBushel, matey.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll come back later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ye can only trade once every 10 minutes. Come back when the tide turns.");
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

                p.SendGump(new DialogueGump(p, crabsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Thalion nods and turns back to his thoughts of the sea.");
            });

        return greeting;
    }

    private bool HasCrabBushel(PlayerMobile player)
    {
        // Check the player's inventory for CrabBushel
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CrabBushel)) != null;
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
        // Remove the CrabBushel and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item crabBushel = player.Backpack.FindItemByType(typeof(CrabBushel));
        if (crabBushel != null)
        {
            crabBushel.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do ye choose, matey?");

            // Add options for PersonalCannon and TinCowbell
            rewardChoiceModule.AddOption("PersonalCannon", pl => true, pl =>
            {
                pl.AddToBackpack(new PersonalCannon());
                pl.SendMessage("Ye receive a PersonalCannon!");
            });

            rewardChoiceModule.AddOption("TinCowbell", pl => true, pl =>
            {
                pl.AddToBackpack(new TinCowbell());
                pl.SendMessage("Ye receive a TinCowbell! More cowbell, I say!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems ye no longer have a CrabBushel.");
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