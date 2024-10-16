using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class EldrinTheAlchemicalArtisan : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EldrinTheAlchemicalArtisan() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Eldrin the Alchemical Artisan";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1260)); // A deep purple robe
        AddItem(new Sandals(1153)); // Golden sandals
        AddItem(new WizardsHat(1260)); // A matching purple wizard's hat
        AddItem(new GoldBracelet()); // A shiny gold bracelet

        VirtualArmor = 15;
    }

    public EldrinTheAlchemicalArtisan(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Eldrin, an artisan of rare alchemical wonders. Do you seek something extraordinary?");

        // Dialogue options
        greeting.AddOption("What kind of alchemical wonders do you create?",
            p => true,
            p =>
            {
                DialogueModule wondersModule = new DialogueModule("I specialize in transmuting ordinary items into something truly magical. But for such a task, I require a rare item. Do you perhaps have a HorrodrickCube?");

                wondersModule.AddOption("I have a HorrodrickCube. Can we make a trade?",
                    pl => HasHorrodrickCube(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                wondersModule.AddOption("I don't have a HorrodrickCube right now.",
                    pl => !HasHorrodrickCube(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a HorrodrickCube.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                wondersModule.AddOption("I recently traded; I'll return later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                wondersModule.AddOption("Tell me more about yourself.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah, my story... It is not a tale of joy. I was once a journalist, a seeker of truth. I wrote stories of ghosts and legends, but no one believed me. They called me delusional, obsessed. Eventually, I gave up writing and turned to alchemy, seeking ways to prove the existence of spirits.");

                        backstoryModule.AddOption("Ghosts? Do you still believe in them?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule ghostsModule = new DialogueModule("Oh, I do more than believe, traveler. I see them, I hear them, I feel their cold whispers. They are out there, in the shadows, waiting. Each day I grow closer to them, and each day I lose a little more of myself. It is a dangerous pursuit, but one I cannot abandon. Would you be interested in helping me prove their existence?");

                                ghostsModule.AddOption("What do you need to prove their existence?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("I need more artifacts, objects imbued with spiritual energy. The HorrodrickCube is one such item, but there are others—strange relics, enchanted crystals, haunted trinkets. Bring me such things, and perhaps together we can unveil the truth.");

                                        helpModule.AddOption("I will keep an eye out for these artifacts.",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendMessage("Eldrin nods, his eyes alight with a fervent gleam. 'Good, good. The spirits shall reveal themselves in time.'");
                                            });

                                        helpModule.AddOption("This sounds dangerous. Are you sure it's worth it?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendMessage("Eldrin's expression grows dark. 'Dangerous? Yes, indeed. I have faced death many times, but the truth is worth any price. I must prove that I am not mad. I must.'");
                                            });

                                        plll.SendGump(new DialogueGump(plll, helpModule));
                                    });

                                ghostsModule.AddOption("You seem a bit... unstable. Are you sure this is wise?",
                                    plll => true,
                                    plll =>
                                    {
                                        plll.SendMessage("Eldrin laughs, a sound that borders on manic. 'Wise? Perhaps not. But wisdom has not brought me peace, and so I choose madness, for at least it gives me purpose.'");
                                    });

                                pll.SendGump(new DialogueGump(pll, ghostsModule));
                            });

                        backstoryModule.AddOption("Why did you give up journalism?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule journalismModule = new DialogueModule("They called me a fool. My editor laughed at my stories, the readers mocked my work. I thought I could show them the truth, but they were blind. Eventually, I realized that writing would never be enough. I needed to act, to prove it firsthand.");

                                journalismModule.AddOption("So you turned to alchemy?",
                                    plll => true,
                                    plll =>
                                    {
                                        plll.SendMessage("Eldrin nods slowly. 'Yes. Alchemy offered me a way to interact with the unseen, to grasp what others could not. It has cost me much, but it has given me purpose.'");
                                    });

                                pll.SendGump(new DialogueGump(pll, journalismModule));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                p.SendGump(new DialogueGump(p, wondersModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Eldrin nods knowingly and returns to his work.");
            });

        return greeting;
    }

    private bool HasHorrodrickCube(PlayerMobile player)
    {
        // Check the player's inventory for HorrodrickCube
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HorrodrickCube)) != null;
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
        // Remove the HorrodrickCube and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item horrodrickCube = player.Backpack.FindItemByType(typeof(HorrodrickCube));
        if (horrodrickCube != null)
        {
            horrodrickCube.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for TrophieAward and ClutteredDesk
            rewardChoiceModule.AddOption("TrophieAward", pl => true, pl =>
            {
                pl.AddToBackpack(new TrophieAward());
                pl.SendMessage("You receive a TrophieAward!");
            });

            rewardChoiceModule.AddOption("ClutteredDesk", pl => true, pl =>
            {
                pl.AddToBackpack(new ClutteredDesk());
                pl.SendMessage("You receive a ClutteredDesk!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a HorrodrickCube.");
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