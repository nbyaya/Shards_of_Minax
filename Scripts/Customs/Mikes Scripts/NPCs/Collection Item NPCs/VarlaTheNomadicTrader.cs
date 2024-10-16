using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class VarlaTheNomadicTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public VarlaTheNomadicTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Varla the Nomadic Trader";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(120);
        SetMana(200);
        SetStam(70);

        Fame = 700;
        Karma = 600;

        // Outfit
        AddItem(new FancyShirt(2128)); // A fine crimson shirt
        AddItem(new LongPants(1109)); // Dark pants
        AddItem(new Cloak(1102)); // Deep brown cloak
        AddItem(new Sandals(2413)); // Bright, stylish sandals
        AddItem(new Bandana(1157)); // A colorful bandana to match her adventurous spirit
        AddItem(new GoldEarrings()); // Gold earrings to hint at her wealth

        VirtualArmor = 20;
    }

    public VarlaTheNomadicTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a fellow traveler! I am Varla, a trader of wonders and seeker of adventures. Tell me, do you have stories to share, or are you perhaps interested in a trade?");

        greeting.AddOption("Tell me about your travels.", p => true, p =>
        {
            DialogueModule travelsModule = new DialogueModule("Oh, where do I begin? I've crossed the seas, trekked through deserts, and bartered with the strangest of folks. Every settlement has its secrets, every person their own story. It’s the thrill of the unknown that keeps me moving.");

            travelsModule.AddOption("What was your most dangerous journey?", pl => true, pl =>
            {
                DialogueModule dangerModule = new DialogueModule("Ah, danger? There was this one time I traveled through the cursed marshes of Velgoth. They say no one returns, but with a sharp wit and some clever bartering, I made it through. I traded a simple silver ring for safe passage with a ghostly guardian – quite the bargain if you ask me.");

                dangerModule.AddOption("How did you convince a ghost to trade?", pla => true, pla =>
                {
                    DialogueModule ghostTradeModule = new DialogueModule("It’s all about knowing what someone wants. Even spirits have desires, often tied to the life they once lived. This one was a blacksmith in his time, longing for the touch of silver once more. I happened to have a trinket and a convincing story – the rest, as they say, is history.");
                    pla.SendGump(new DialogueGump(pla, ghostTradeModule));
                });

                dangerModule.AddOption("That’s incredible. You truly are fearless.", pla => true, pla =>
                {
                    pla.SendMessage("Varla grins with a glint of pride. 'Fearless or foolish, who can say? But it's been a life worth living.'");
                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                });

                pl.SendGump(new DialogueGump(pl, dangerModule));
            });

            travelsModule.AddOption("Do you have any advice for a fellow adventurer?", pl => true, pl =>
            {
                DialogueModule adviceModule = new DialogueModule("Keep your wits about you, and always know the value of what you carry. The world is full of charlatans and swindlers. But, if you’re sharp enough, you can turn any situation to your favor. And remember, a smile can be more disarming than a sword.");
                pl.SendGump(new DialogueGump(pl, adviceModule));
            });

            travelsModule.AddOption("What kind of goods do you trade?", pl => true, pl =>
            {
                DialogueModule tradeGoodsModule = new DialogueModule("I trade in anything of value – exotic spices, rare gems, enchanted items, and sometimes even secrets. But lately, I've been in search of something truly special... Do you, by chance, have an ExcitingTome?");

                tradeGoodsModule.AddOption("Yes, I have an ExcitingTome. Can we trade?", pla => HasExcitingTome(pla) && CanTradeWithPlayer(pla), pla => InitiateTrade(pla));

                tradeGoodsModule.AddOption("I don’t have an ExcitingTome right now.", pla => !HasExcitingTome(pla), pla =>
                {
                    pla.SendMessage("Varla nods, 'Well, keep your eyes open! The world is full of treasures just waiting to be found.'");
                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                });

                tradeGoodsModule.AddOption("I traded recently. I will come back later.", pla => !CanTradeWithPlayer(pla), pla =>
                {
                    pla.SendMessage("I can only trade once every 10 minutes. Please return later.");
                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                });

                pl.SendGump(new DialogueGump(pl, tradeGoodsModule));
            });

            p.SendGump(new DialogueGump(p, travelsModule));
        });

        greeting.AddOption("What kind of trader are you?", p => true, p =>
        {
            DialogueModule traderTypeModule = new DialogueModule("I am a nomadic trader, a dealer in rare and valuable goods, but also in stories and connections. You see, my charm is my best currency – it’s opened more doors than gold ever could. I make friends, allies, and the occasional rival wherever I go.");

            traderTypeModule.AddOption("Do you have many rivals?", pl => true, pl =>
            {
                DialogueModule rivalsModule = new DialogueModule("Oh, indeed. In my line of work, stepping on a few toes is inevitable. There was a merchant named Balthazar, a real snake of a man. We once competed over a sapphire of the purest blue. I won, of course, but he’s been sour ever since.");

                rivalsModule.AddOption("How did you outwit him?", pla => true, pla =>
                {
                    DialogueModule outwitModule = new DialogueModule("Simple. While he was busy flaunting his wealth, I charmed the gem’s owner with a tale of adventure and a promise of future favors. Some things are worth more than coin, and knowing what people value is key.");
                    pla.SendGump(new DialogueGump(pla, outwitModule));
                });

                rivalsModule.AddOption("He sounds like trouble.", pla => true, pla =>
                {
                    pla.SendMessage("Varla chuckles, 'Trouble? Perhaps. But he makes the game more interesting.'");
                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                });

                pl.SendGump(new DialogueGump(pl, rivalsModule));
            });

            traderTypeModule.AddOption("I admire your resourcefulness.", pl => true, pl =>
            {
                pl.SendMessage("Varla smiles warmly, 'Thank you. In this life, a sharp mind and a brave heart are the greatest treasures.'");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });

            p.SendGump(new DialogueGump(p, traderTypeModule));
        });

        greeting.AddOption("Goodbye.", p => true, p =>
        {
            p.SendMessage("Varla nods and waves, 'Safe travels, friend. May fortune favor you on the road ahead.'");
        });

        return greeting;
    }

    private bool HasExcitingTome(PlayerMobile player)
    {
        // Check if the player has an ExcitingTome in their backpack
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExcitingTome)) != null;
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

    private void InitiateTrade(PlayerMobile player)
    {
        // Remove the ExcitingTome and give the rewards, then set the cooldown timer
        Item excitingTome = player.Backpack.FindItemByType(typeof(ExcitingTome));
        if (excitingTome != null)
        {
            excitingTome.Delete();

            // Always give MaxxiaScroll
            player.AddToBackpack(new MaxxiaScroll());
            player.SendMessage("You receive a MaxxiaScroll!");

            // Reward choice: FineSilverWire or WallFlowers
            DialogueModule rewardModule = new DialogueModule("Which reward would you like to choose?");

            rewardModule.AddOption("FineSilverWire", pl => true, pl =>
            {
                pl.AddToBackpack(new FineSilverWire());
                pl.SendMessage("You receive FineSilverWire!");
            });

            rewardModule.AddOption("WallFlowers", pl => true, pl =>
            {
                pl.AddToBackpack(new WallFlowers());
                pl.SendMessage("You receive WallFlowers!");
            });

            player.SendGump(new DialogueGump(player, rewardModule));
            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an ExcitingTome.");
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