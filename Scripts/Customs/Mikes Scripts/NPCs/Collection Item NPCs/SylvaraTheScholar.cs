using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SylvaraTheScholar : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SylvaraTheScholar() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sylvara the Scholar";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 100;
        Karma = 200;

        // Outfit
        AddItem(new Robe(2257)); // A deep blue robe
        AddItem(new Sandals(1175)); // Midnight blue sandals
        AddItem(new WizardsHat(1157)); // A matching deep blue wizard's hat
        AddItem(new GoldBracelet()); // A gold bracelet with intricate designs

        VirtualArmor = 15;
    }

    public SylvaraTheScholar(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Sylvara, a collector of ancient lore and rare items. Have you, by any chance, come across a JudasCradle?");

        // Dialogue options
        greeting.AddOption("Tell me more about the JudasCradle.",
            p => true,
            p =>
            {
                DialogueModule judasCradleModule = new DialogueModule("The JudasCradle is an artifact of mystery, steeped in legend. If you happen to possess one, I could offer you a valuable trade. I have both a GlassFurnace and LovelyLilies in my collection.");

                judasCradleModule.AddOption("I have a JudasCradle and I'd like to trade.",
                    pl => HasJudasCradle(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                judasCradleModule.AddOption("I traded recently; I'll return later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                judasCradleModule.AddOption("I don't have one at the moment.",
                    pl => !HasJudasCradle(pl),
                    pl =>
                    {
                        pl.SendMessage("No worries, come back if you ever find a JudasCradle.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                judasCradleModule.AddOption("What exactly does the JudasCradle do?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule cradleDetailModule = new DialogueModule("Ah, now there's a question that would make even the brightest scholars ponder! The JudasCradle is said to hold immense power—power to reveal secrets, but also power that comes at a cost. Many who have held it reported strange dreams, cryptic visions, and a sense of being watched. Would you dare such an experience?");

                        cradleDetailModule.AddOption("That sounds intriguing. Tell me more.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule intrigueModule = new DialogueModule("They say the JudasCradle was crafted by an ancient jester—a twisted entertainer who performed for a court long since forgotten. He imbued it with dark humor, a kind of cosmic joke. It’s said that those who mocked him suffered for their folly. Are you brave enough to hold the weight of such history?");

                                intrigueModule.AddOption("A jester, you say? What happened to him?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule jesterModule = new DialogueModule("The jester—oh, they called him the Twisted Jester—was a man of wit, yes, but also of darkness. His jokes became cruel, his smile sinister. He used his jests to humiliate those who once adored him, and soon, the court had enough. They cast him out, but not before he cursed his tools of trade. The JudasCradle is one such item, full of his resentment and dark humor.");

                                        jesterModule.AddOption("I see... Perhaps I shouldn't meddle with such items.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Wise decision, traveler. Not all treasures are worth the weight of their curses.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        jesterModule.AddOption("I am still interested in the trade.", 
                                            plaaa => HasJudasCradle(plaaa) && CanTradeWithPlayer(plaaa), 
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, jesterModule));
                                    });

                                intrigueModule.AddOption("I think I’ve heard enough about this artifact.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Some knowledge is best left as whispers in the wind. Farewell, for now.");
                                    });

                                pla.SendGump(new DialogueGump(pla, intrigueModule));
                            });

                        cradleDetailModule.AddOption("This sounds too dangerous for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("A wise choice, traveler. The past is not always a burden we must bear.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, cradleDetailModule));
                    });

                p.SendGump(new DialogueGump(p, judasCradleModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Sylvara nods knowingly, her eyes glimmering with wisdom.");
            });

        return greeting;
    }

    private bool HasJudasCradle(PlayerMobile player)
    {
        // Check the player's inventory for JudasCradle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(JudasCradle)) != null;
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
        // Remove the JudasCradle and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item judasCradle = player.Backpack.FindItemByType(typeof(JudasCradle));
        if (judasCradle != null)
        {
            judasCradle.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for GlassFurnace and LovelyLilies
            rewardChoiceModule.AddOption("GlassFurnace", pl => true, pl =>
            {
                pl.AddToBackpack(new GlassFurnace());
                pl.SendMessage("You receive a GlassFurnace!");
            });

            rewardChoiceModule.AddOption("LovelyLilies", pl => true, pl =>
            {
                pl.AddToBackpack(new LovelyLilies());
                pl.SendMessage("You receive LovelyLilies!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a JudasCradle.");
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