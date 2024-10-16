using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZaraTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZaraTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zara the Alchemist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1150)); // Robe with a dark purple hue
        AddItem(new Sandals(270)); // Sandals with a light gray hue
        AddItem(new WizardsHat(1153)); // A hat to complete the alchemist look
        AddItem(new GoldBracelet()); // Adds a hint of mystery

        VirtualArmor = 15;
    }

    public ZaraTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Zara, an alchemist fascinated by the unknown properties of peculiar items. Do you seek knowledge or perhaps a fair trade?");

        // Start with dialogue about her work
        greeting.AddOption("Tell me about your work.",
            p => true,
            p =>
            {
                DialogueModule alchemyModule = new DialogueModule("Alchemy is the art of transformation. I delve into rare substances, seeking their secrets. Have you heard of the enigmatic Cup of Slime?");
                alchemyModule.AddOption("What's a Cup of Slime?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule slimeModule = new DialogueModule("The Cup of Slime is a rare concoction from the Marsh of Decay. Its consistency is unlike anything else, and its magical properties are vast. I could make good use of one if you happen to have it.");
                        slimeModule.AddOption("What do you offer in return?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeIntroModule = new DialogueModule("If you bring me a Cup of Slime, I can offer you a FancyMirror, a mysterious artifact of reflection. Additionally, I always reward my friends with a MaxxiaScroll for their assistance.");
                                tradeIntroModule.AddOption("I'd like to make the trade.",
                                    plaa => CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        DialogueModule tradeModule = new DialogueModule("Do you have a Cup of Slime for me?");
                                        tradeModule.AddOption("Yes, I have a Cup of Slime.",
                                            plaaa => HasCupOfSlime(plaaa) && CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });
                                        tradeModule.AddOption("No, I don't have one right now.",
                                            plaaa => !HasCupOfSlime(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Come back when you have a Cup of Slime.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                            plaaa => !CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tradeModule));
                                    });
                                tradeIntroModule.AddOption("Perhaps another time.",
                                    plaq => true,
                                    plaq =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroModule));
                            });
                        slimeModule.AddOption("What makes you interested in such peculiar substances?",
                            plw => true,
                            plw =>
                            {
                                DialogueModule passionModule = new DialogueModule("I am passionate about preserving the alien ecosystems that produce these wonders. Too often, explorers come through, taking what they do not understand, damaging the fragile balance. I wish to protect these unique places. Every Cup of Slime gathered must be done with respect, ensuring we do not disrupt the delicate beauty of the Marsh of Decay.");
                                passionModule.AddOption("Why do you care so much about preserving these ecosystems?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule idealismModule = new DialogueModule("I believe that every living thing has a right to thrive, even those we do not fully understand. The ecosystems of this world are filled with beauty and mystery. If we allow reckless exploration, we may lose something irreplaceable. The Marsh of Decay, for example, is not just a dangerous swamp; it is a home, teeming with unique life, all interconnected. I stand resilient against those who would exploit it.");
                                        idealismModule.AddOption("Have you faced much opposition?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule oppositionModule = new DialogueModule("Oh, indeed. Many see me as a hindrance. They wish to harvest, explore, and conquer. They see resources where I see life. I have faced those who mock my efforts, who see me as overly idealistic or naive. But I remain steadfast. I know what I am fighting for. My resilience is my greatest strength, for I will not let the beauty of these alien lands be lost to greed.");
                                                oppositionModule.AddOption("I admire your passion.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("Zara smiles warmly, her eyes filled with gratitude.");
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                                    });
                                                oppositionModule.AddOption("It sounds like a tough path.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule toughPathModule = new DialogueModule("It is. But I take solace in the small victories. Every rare bloom I protect, every creature I save from extinction, is a step forward. If even one traveler, like yourself, understands the importance of preservation, then my efforts are not in vain.");
                                                        toughPathModule.AddOption("I understand. Thank you for sharing.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                            });
                                                        toughPathModule.AddOption("Why not just focus on your alchemy?",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                DialogueModule focusModule = new DialogueModule("Alchemy is my means of understanding and connecting with these ecosystems. Through alchemy, I can learn the secrets they hold, and perhaps even find ways to protect them better. My work is not separate from my cause; it is one and the same. To transform, to preserve, to heal - that is the essence of true alchemy.");
                                                                focusModule.AddOption("That's admirable.",
                                                                    plaaaaa => true,
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendMessage("Zara nods solemnly, her resolve evident.");
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                                    });
                                                                focusModule.AddOption("I see, thank you for explaining.",
                                                                    plaaaaa => true,
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, focusModule));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, toughPathModule));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, oppositionModule));
                                            });
                                        idealismModule.AddOption("I think I understand now.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                            });
                                        pla.SendGump(new DialogueGump(pla, idealismModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, passionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, slimeModule));
                    });
                p.SendGump(new DialogueGump(p, alchemyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Zara nods knowingly and turns back to her work.");
            });

        return greeting;
    }

    private bool HasCupOfSlime(PlayerMobile player)
    {
        // Check the player's inventory for CupOfSlime
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CupOfSlime)) != null;
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
        // Remove the CupOfSlime and give the FancyMirror and MaxxiaScroll, then set the cooldown timer
        Item cupOfSlime = player.Backpack.FindItemByType(typeof(CupOfSlime));
        if (cupOfSlime != null)
        {
            cupOfSlime.Delete();
            player.AddToBackpack(new FancyMirror());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Cup of Slime and receive a FancyMirror and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Cup of Slime.");
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