using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class GruntharTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public GruntharTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Grunthar the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(50);
        SetInt(40);

        SetHits(100);
        SetMana(50);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // Fancy Shirt with dark red hue
        AddItem(new LongPants(1109)); // Long Pants with a dark green hue
        AddItem(new ThighBoots(1175)); // Dark brown boots
        AddItem(new Cloak(1153)); // Deep blue cloak
        AddItem(new Bandana(1150)); // Black bandana
        AddItem(new SkullCap(1175)); // To add a bit of character
        AddItem(new SkullShield()); // The item he's interested in

        VirtualArmor = 15;
    }

    public GruntharTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there, traveler! The name's Grunthar. I'm a collector of peculiar things, a connoisseur of curiosities. I’m always looking for something special, like a certain SkullShield.");

        // Main Conversation
        greeting.AddOption("What do you do with these items?",
            p => true,
            p =>
            {
                DialogueModule explanationModule = new DialogueModule("Ah, the wonders that people leave behind! Each item tells a story. The SkullShield, for instance, contains whispers of those who wore it in battle. If you have one, I can exchange it for something equally... eerie. Interested?");
                explanationModule.AddOption("Tell me about the trade.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("If you give me a SkullShield, I will give you a ZombieHand as a token of gratitude, along with a MaxxiaScroll. But I can only make such a trade once every 10 minutes.");
                        tradeModule.AddOption("I'd like to trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule confirmTradeModule = new DialogueModule("Do you have a SkullShield for me?");
                                confirmTradeModule.AddOption("Yes, here it is.",
                                    plaa => HasSkullShield(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                confirmTradeModule.AddOption("No, I don't have one.",
                                    plaa => !HasSkullShield(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SkullShield.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                confirmTradeModule.AddOption("I need to wait a bit longer.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, confirmTradeModule));
                            });
                        tradeModule.AddOption("Not right now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                explanationModule.AddOption("Why do you collect these strange items?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Collecting these items... it's more than just a pastime. You see, I used to be different. Before the radiation, before the mutations... I was just a regular man with a family. But fate had other plans. The radiation changed me, and took them away. Now, I wander the wasteland, gathering what remains, hoping for a cure, trying to make sense of it all.");
                        backstoryModule.AddOption("That sounds horrible. I'm sorry for your loss.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule sympathyModule = new DialogueModule("Thank you... but sympathy won't bring them back. It won't fix what's broken inside me. Every SkullShield, every artifact, brings me one step closer to understanding what happened, and maybe even undoing it. It keeps me going, keeps me... resilient, I suppose.");
                                sympathyModule.AddOption("I admire your resilience.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule resilienceModule = new DialogueModule("Resilience... it's all I have left. But it comes at a cost. The bitterness, the anger... it grows every day. Still, I can't afford to stop. Not until I've found something to save what's left of me.");
                                        resilienceModule.AddOption("What keeps you going?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule motivationModule = new DialogueModule("Hope, perhaps. Or maybe just spite. I don't know anymore. I think of the friends I lost, the family I failed to protect. I think of the monsters out there, twisted by the same radiation that changed me. I can't let it all be for nothing. I have to find a way to reverse it, or at least understand it.");
                                                motivationModule.AddOption("Do you think a cure is possible?",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        DialogueModule cureModule = new DialogueModule("A cure... I don't know. Some days, I believe it's possible. Other days, it feels like chasing shadows. The radiation twisted my body, but it hasn't claimed my mind. Not yet. If I can find enough of these artifacts, maybe there's a clue somewhere, a hint of what went wrong—and how to fix it.");
                                                        cureModule.AddOption("I wish you luck, Grunthar.",
                                                            plaabcd => true,
                                                            plaabcd =>
                                                            {
                                                                plaabcd.SendMessage("Grunthar nods solemnly, his eyes distant.");
                                                                plaabcd.SendGump(new DialogueGump(plaabcd, CreateGreetingModule(plaabcd)));
                                                            });
                                                        cureModule.AddOption("Sounds like a fool's errand, but I hope you succeed.",
                                                            plaabcd => true,
                                                            plaabcd =>
                                                            {
                                                                plaabcd.SendMessage("Grunthar chuckles bitterly. 'A fool's errand, indeed. But sometimes, being a fool is all that keeps you alive.'");
                                                                plaabcd.SendGump(new DialogueGump(plaabcd, CreateGreetingModule(plaabcd)));
                                                            });
                                                        plaabc.SendGump(new DialogueGump(plaabc, cureModule));
                                                    });
                                                motivationModule.AddOption("I hope you find what you're looking for.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Grunthar nods. 'So do I, traveler. So do I.'");
                                                        plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, motivationModule));
                                            });
                                        resilienceModule.AddOption("I hope you find peace someday.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Grunthar sighs. 'Peace... maybe one day. But for now, there's too much to do.'");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, resilienceModule));
                                    });
                                sympathyModule.AddOption("I can't imagine what that must be like.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Grunthar gives a weary smile. 'You don't want to. It's a nightmare I wouldn't wish on my worst enemy.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, sympathyModule));
                            });
                        backstoryModule.AddOption("You seem bitter. Does it ever get easier?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule bitternessModule = new DialogueModule("Bitter? Ha, you're not wrong. The bitterness is like a poison. It seeps in, takes hold, and never lets go. But I survive. That's all that matters. It doesn't get easier, but you learn to live with it. It's either that or give up, and I'm not ready for that just yet.");
                                bitternessModule.AddOption("I hope you find a way to let go of the bitterness.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Grunthar shakes his head. 'Maybe. But until then, I'll keep fighting.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                bitternessModule.AddOption("Sometimes anger is what keeps us alive.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Grunthar nods. 'Exactly. Anger, bitterness, whatever it takes to keep putting one foot in front of the other.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, bitternessModule));
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });
                explanationModule.AddOption("What happened to your family?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule familyModule = new DialogueModule("They were everything to me. When the radiation hit, some of us survived... or at least, we thought we did. But the mutations came slowly, taking them one by one. I tried to save them, but there was nothing I could do. Watching them change, losing their humanity... it broke me.");
                        familyModule.AddOption("I'm so sorry, Grunthar.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Grunthar's eyes well up slightly, but he quickly blinks it away. 'Thank you. But there's no changing the past.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        familyModule.AddOption("Do you think you'll ever find peace?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule peaceModule = new DialogueModule("Peace... it feels like a distant dream. Maybe one day, if I find a cure, if I make things right. But for now, I can only keep moving forward, one step at a time.");
                                peaceModule.AddOption("I hope you get there.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Grunthar gives a weary nod. 'So do I, traveler. So do I.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, peaceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, familyModule));
                    });
                p.SendGump(new DialogueGump(p, explanationModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Grunthar waves you off, turning back to inspect his collection.");
            });

        return greeting;
    }

    private bool HasSkullShield(PlayerMobile player)
    {
        // Check the player's inventory for SkullShield
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SkullShield)) != null;
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
        // Remove the SkullShield and give the ZombieHand and MaxxiaScroll, then set the cooldown timer
        Item skullShield = player.Backpack.FindItemByType(typeof(SkullShield));
        if (skullShield != null)
        {
            skullShield.Delete();
            player.AddToBackpack(new ZombieHand());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the SkullShield and receive a ZombieHand and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a SkullShield.");
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