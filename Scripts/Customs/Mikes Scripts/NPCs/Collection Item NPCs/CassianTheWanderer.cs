using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CassianTheWanderer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CassianTheWanderer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cassian the Wanderer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(85);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(85);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(2153)); // Blue shirt
        AddItem(new LongPants(1109)); // Dark pants
        AddItem(new Boots(1175)); // Light grey boots
        AddItem(new Cloak(1165)); // Dark green cloak
        AddItem(new WideBrimHat(1157)); // Hat with a unique hue
        AddItem(new Backpack()); // Backpack containing his goods

        VirtualArmor = 15;
    }

    public CassianTheWanderer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Cassian, a wanderer of the lands. I've collected many wonders during my journeys. Are you interested in a trade?");

        // Offer dialogue for trading the ExoticWhistle
        greeting.AddOption("What kind of trade are you offering?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I'm looking for an ExoticWhistle. In return, I can offer you a delicious SeaSerpantSteak and a rare MaxxiaScroll. But remember, I can only make this exchange once every ten minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have an ExoticWhistle for me?");
                        tradeModule.AddOption("Yes, I have an ExoticWhistle.",
                            plaa => HasExoticWhistle(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasExoticWhistle(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have an ExoticWhistle. I shall be waiting.");
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
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // New dialogue options with backstory and character traits
        greeting.AddOption("You seem... burdened. Is something troubling you?",
            p => true,
            p =>
            {
                DialogueModule troubledModule = new DialogueModule("Burdened, you say? Yes... burdened by time itself. The gears keep turning, ticking, and tocking, yet they can never bring back what was lost.");
                troubledModule.AddOption("What do you mean by that?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("I once had a family, you see. My wife, my daughter... they were my world. But time, with its cruel hands, took them from me. Since then, I've tried to control it, to bend it, to make it yield to my will.");
                        pastModule.AddOption("You tried to control time?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule clockModule = new DialogueModule("Indeed. I became a clockmaker, building intricate mechanisms, gears upon gears. I thought, if I could master timepieces, I could master time itself. But each clock I made, each minute I counted, only reminded me of my failure. Time cannot be stopped, nor reversed.");
                                clockModule.AddOption("Did you succeed in manipulating time at all?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule successModule = new DialogueModule("In a way, yes. I built a clock so precise, so powerful, it seemed to slow the world around me. For a moment, I felt I could bring them back, if only I could keep time still. But it was only an illusion. Time does not yield; it is indifferent to our wishes.");
                                        successModule.AddOption("That's... incredible, but also tragic.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Cassian's eyes grow distant, his voice heavy with melancholy.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, successModule));
                                    });
                                clockModule.AddOption("That's a heavy burden to bear. I'm sorry.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Cassian nods slowly, lost in thought.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, clockModule));
                            });
                        pastModule.AddOption("That must have been very difficult.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Cassian's expression softens, and he sighs deeply.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });
                troubledModule.AddOption("Perhaps time is best left alone.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Cassian smiles sadly. 'Perhaps you are right, traveler. Perhaps you are right.'");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, troubledModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Cassian nods and adjusts his cloak as you part ways.");
            });

        return greeting;
    }

    private bool HasExoticWhistle(PlayerMobile player)
    {
        // Check the player's inventory for ExoticWhistle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExoticWhistle)) != null;
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
        // Remove the ExoticWhistle and give the SeaSerpantSteak and MaxxiaScroll, then set the cooldown timer
        Item exoticWhistle = player.Backpack.FindItemByType(typeof(ExoticWhistle));
        if (exoticWhistle != null)
        {
            exoticWhistle.Delete();
            player.AddToBackpack(new SeaSerpantSteak());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ExoticWhistle and receive a SeaSerpantSteak and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have an ExoticWhistle.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
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