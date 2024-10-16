using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MysticMilos : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MysticMilos() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mystic Milos";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1266)); // A mystic dark blue robe
        AddItem(new Sandals(1325)); // White sandals
        AddItem(new WizardsHat(1153)); // A bright blue wizard's hat
        AddItem(new QuarterStaff()); // A wooden staff

        VirtualArmor = 15;
    }

    public MysticMilos(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Milos, a mystic wandering the lands in search of rare wonders. Do you have something unique for me?");

        // Dialogue options
        greeting.AddOption("What kind of wonders are you looking for?",
            p => true,
            p =>
            {
                DialogueModule wondersModule = new DialogueModule("I am particularly interested in a ChocolateFountain. If you have one, I can offer you a choice of a PowerGem or an AtomicRegulator in exchange, along with a special scroll of Maxxia's secrets. But beware, my demands are not for the faint of heart.");

                wondersModule.AddOption("Why are you interested in a ChocolateFountain?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Ah, the ChocolateFountain, you see, is not just a whimsical trinket. It contains an alchemical essence, a key to my research into the arcane mysteries of transformation. Its properties are... brilliant, though the true value may be lost on those less gifted in intellect.");
                        
                        reasonModule.AddOption("What kind of research are you doing?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule researchModule = new DialogueModule("My research? You wish to understand? I am delving into the very fabric of matter, trying to bridge the gap between mundane elements and their transcendent counterparts. The ChocolateFountain represents a nexus of transformation - crude, perhaps, but a stepping stone to greatness.");

                                researchModule.AddOption("That sounds fascinating!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule enthusiasmModule = new DialogueModule("Of course it is! But it takes a special mind to appreciate it. Most people, sadly, lack the intellectual rigor. They think me arrogant, but it is not arrogance if it is true.");
                                        
                                        enthusiasmModule.AddOption("Why do people think you are arrogant?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule arroganceModule = new DialogueModule("Because they fail to comprehend my brilliance! They call me arrogant simply because I refuse to lower myself to their level of understanding. It is not my fault that others are unable or unwilling to grasp the complexities of my work. They chose ignorance, and I chose enlightenment.");

                                                arroganceModule.AddOption("Have you always been this way?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule pastModule = new DialogueModule("No... there was a time when I tried to share my knowledge. I had loved ones, friends, colleagues. But they could not keep up. They wanted me to stop, to slow down, to be 'normal.' In the end, I chose my work over them. Perhaps it was the right decision, perhaps not. But here I am, alone, yet enlightened.");

                                                        pastModule.AddOption("Do you regret losing your loved ones?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule regretModule = new DialogueModule("Regret? Regret is a useless emotion. I made my choice, and they made theirs. I cannot allow sentiment to interfere with progress. Yet, sometimes... late at night, when the silence is deafening, I wonder if perhaps there was another way. But it is too late now.");

                                                                regretModule.AddOption("Maybe it's not too late to reconnect?",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule reconnectModule = new DialogueModule("Reconnect? Hah! Who would I even reconnect with? The world moves on, and those who once knew me have likely forgotten or moved past my brilliance. Perhaps they are happier in their ignorance, while I stand at the threshold of unlocking secrets they could never dream of.");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, reconnectModule));
                                                                    });

                                                                regretModule.AddOption("Let's talk about something else.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, regretModule));
                                                            });

                                                        pastModule.AddOption("I can understand why you made that choice.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Mystic Milos nods solemnly, as if recognizing a kindred spirit.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, pastModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, arroganceModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, enthusiasmModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, researchModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });

                wondersModule.AddOption("I have a ChocolateFountain. Let's trade.",
                    pl => HasChocolateFountain(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                wondersModule.AddOption("I don't have one yet.",
                    pl => !HasChocolateFountain(pl),
                    pl =>
                    {
                        pl.SendMessage("Return when you have acquired a ChocolateFountain. I shall be here, waiting.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                wondersModule.AddOption("I have traded recently; I will return later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You must wait a while before we can trade again. Come back in about 10 minutes.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, wondersModule));
            });

        greeting.AddOption("Farewell.",
            p => true,
            p =>
            {
                p.SendMessage("Mystic Milos bows slightly, his robes swaying gently.");
            });

        return greeting;
    }

    private bool HasChocolateFountain(PlayerMobile player)
    {
        // Check the player's inventory for ChocolateFountain
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ChocolateFountain)) != null;
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
        // Remove the ChocolateFountain and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item chocolateFountain = player.Backpack.FindItemByType(typeof(ChocolateFountain));
        if (chocolateFountain != null)
        {
            chocolateFountain.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for PowerGem and AtomicRegulator
            rewardChoiceModule.AddOption("PowerGem", pl => true, pl =>
            {
                pl.AddToBackpack(new PowerGem());
                pl.SendMessage("You receive a PowerGem!");
            });

            rewardChoiceModule.AddOption("AtomicRegulator", pl => true, pl =>
            {
                pl.AddToBackpack(new AtomicRegulator());
                pl.SendMessage("You receive an AtomicRegulator!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a ChocolateFountain.");
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