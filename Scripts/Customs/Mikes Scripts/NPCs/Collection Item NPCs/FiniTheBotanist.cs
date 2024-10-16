using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FiniTheBotanist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FiniTheBotanist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Fini the Botanist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(100);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1161)); // Robe with a greenish hue
        AddItem(new Sandals(1175)); // Sandals with a light green hue
        AddItem(new FlowerGarland()); // A floral headpiece

        VirtualArmor = 10;
    }

    public FiniTheBotanist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Fini, a botanist with a deep fascination for the natural wonders of our world. Could I perhaps interest you in an exchange, or perhaps a story from my travels?");

        // Start with dialogue about her work and travels
        greeting.AddOption("Tell me about your research.",
            p => true,
            p =>
            {
                DialogueModule researchModule = new DialogueModule("I study the rare and magical flora scattered across our lands. Each plant holds secrets, and I use them for remedies, potions, and sometimes, enchantments.");
                researchModule.AddOption("That's fascinating!", pl => true, pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl))); });
                researchModule.AddOption("Can you tell me about a specific plant?", pl => true, pl =>
                {
                    DialogueModule plantModule = new DialogueModule("Of course! There are so many intriguing ones. Let me tell you about a few...");
                    plantModule.AddOption("Tell me about the Moonflower.", pla => true, pla =>
                    {
                        DialogueModule moonflowerModule = new DialogueModule("The Moonflower blooms only under the light of the full moon. Its petals are known for their restorative properties, perfect for healing elixirs. However, it attracts shadowy creatures who guard it fiercely.");
                        moonflowerModule.AddOption("How do you gather it safely?", plaa => true, plaa =>
                        {
                            DialogueModule gatherModule = new DialogueModule("Ah, it takes a shrewd mind and careful steps. You must bring silverroot to deter the shadows, and gather the petals just as the moon reaches its peak. Timing and precision are everything.");
                            gatherModule.AddOption("You must be quite brave!", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            gatherModule.AddOption("That sounds too dangerous for me.", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            plaa.SendGump(new DialogueGump(plaa, gatherModule));
                        });
                        moonflowerModule.AddOption("Where can I find Moonflower?", plaa => true, plaa =>
                        {
                            DialogueModule locationModule = new DialogueModule("It grows in secluded glades, usually hidden by dense forest. The journey is not for the faint-hearted, but if you can find the Silver Glade, you may just find it in bloom.");
                            locationModule.AddOption("I'll keep an eye out for it.", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            plaa.SendGump(new DialogueGump(plaa, locationModule));
                        });
                        pla.SendGump(new DialogueGump(pla, moonflowerModule));
                    });
                    plantModule.AddOption("Tell me about the Crimson Briar.", pla => true, pla =>
                    {
                        DialogueModule briarModule = new DialogueModule("Crimson Briar is a deadly vine that grows in deep, shadowed forests. Its thorns are poisonous, but its berries can be distilled into an antidote. The vine itself has a mind of its own, ensnaring anyone foolish enough to get too close.");
                        briarModule.AddOption("How do you safely harvest the berries?", plaa => true, plaa =>
                        {
                            DialogueModule harvestModule = new DialogueModule("To harvest Crimson Briar berries, you must use a special gauntlet woven from moonspider silk. The vine senses movement and will strike, but the silk confuses it, allowing for a quick pluck of the berry.");
                            harvestModule.AddOption("You seem very resourceful!", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            plaa.SendGump(new DialogueGump(plaa, harvestModule));
                        });
                        briarModule.AddOption("What kind of antidote can you make?", plaa => true, plaa =>
                        {
                            DialogueModule antidoteModule = new DialogueModule("The antidote made from Crimson Briar berries can neutralize many poisons, including those from venomous creatures. It must be mixed with dew gathered at dawn and ground ashroot to be effective.");
                            antidoteModule.AddOption("I'll remember that.", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            plaa.SendGump(new DialogueGump(plaa, antidoteModule));
                        });
                        pla.SendGump(new DialogueGump(pla, briarModule));
                    });
                    plantModule.AddOption("Tell me about Dragonroot.", pla => true, pla =>
                    {
                        DialogueModule dragonrootModule = new DialogueModule("Dragonroot is a rare plant found near old dragon nests. It is said to absorb the magic left behind by the dragons, making it highly sought after by alchemists. The root itself can be ground into a powder used in powerful enchantments.");
                        dragonrootModule.AddOption("Are there any dangers in harvesting Dragonroot?", plaa => true, plaa =>
                        {
                            DialogueModule dangerModule = new DialogueModule("Oh, certainly! Dragons may be gone, but their magic lingers. Harvesting Dragonroot can awaken the slumbering magic in the area. One must be careful not to draw too much attention.");
                            dangerModule.AddOption("I'll tread carefully.", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            plaa.SendGump(new DialogueGump(plaa, dangerModule));
                        });
                        dragonrootModule.AddOption("What enchantments can it be used for?", plaa => true, plaa =>
                        {
                            DialogueModule enchantModule = new DialogueModule("Dragonroot powder can be used to enhance weapons and armor, giving them resistance to fire or a touch of magical energy. However, it requires a skilled enchanter to unlock its true potential.");
                            enchantModule.AddOption("I see, thank you.", plaaa => true, plaaa => { plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa))); });
                            plaa.SendGump(new DialogueGump(plaa, enchantModule));
                        });
                        pla.SendGump(new DialogueGump(pla, dragonrootModule));
                    });
                    pl.SendGump(new DialogueGump(pl, plantModule));
                });
                p.SendGump(new DialogueGump(p, researchModule));
            });

        // Trade option
        greeting.AddOption("Do you need help with anything?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am always in need of certain rare items. Currently, I seek a particular item called a FountainWall. If you bring me one, I can offer you a RopeSpindle in return, and also a MaxxiaScroll as thanks. However, I can only do this once every 10 minutes.");
                tradeIntroductionModule.AddOption("I have a FountainWall to trade.",
                    pla => HasFountainWall(pla) && CanTradeWithPlayer(pla),
                    pla =>
                    {
                        CompleteTrade(pla);
                    });
                tradeIntroductionModule.AddOption("I don't have a FountainWall right now.",
                    pla => !HasFountainWall(pla),
                    pla =>
                    {
                        pla.SendMessage("Come back when you have a FountainWall.");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                tradeIntroductionModule.AddOption("I traded recently; I'll come back later.",
                    pla => !CanTradeWithPlayer(pla),
                    pla =>
                    {
                        pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Her past as a wandering merchant
        greeting.AddOption("Tell me about your travels.",
            p => true,
            p =>
            {
                DialogueModule travelModule = new DialogueModule("Ah, the life of a wandering merchant! I have traveled far and wide, from the golden deserts of Jhelom to the icy peaks of Daggerfall. Everywhere I go, I make friends, exchange stories, and of course, trade goods.");
                travelModule.AddOption("Do you have any memorable stories?", pl => true, pl =>
                {
                    DialogueModule storyModule = new DialogueModule("Oh, I have many stories. Let me tell you about the time I outsmarted a band of thieves in the Great Forest. They thought they had me surrounded, but they underestimated my knowledge of the terrain and my connections with the local rangers.");
                    storyModule.AddOption("How did you escape?", pla => true, pla =>
                    {
                        DialogueModule escapeModule = new DialogueModule("With a bit of quick thinking and charm. I offered them a 'rare' potion I had concocted—a sleeping draught disguised as a healing tonic. Once they drank it, they were asleep within moments, and I slipped away unnoticed.");
                        escapeModule.AddOption("That was clever!", plaa => true, plaa => { plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa))); });
                        pla.SendGump(new DialogueGump(pla, escapeModule));
                    });
                    storyModule.AddOption("Did you ever see them again?", pla => true, pla =>
                    {
                        DialogueModule encounterModule = new DialogueModule("Oh yes, they tried to track me down later, but by then, I had already made friends with the local blacksmith and his family. They hid me, and I ended up helping them with some... 'tricky' customers of their own. It all worked out in the end, and I still visit them when I'm in the area.");
                        encounterModule.AddOption("You certainly know how to make friends.", plaa => true, plaa => { plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa))); });
                        pla.SendGump(new DialogueGump(pla, encounterModule));
                    });
                    pl.SendGump(new DialogueGump(pl, storyModule));
                });
                travelModule.AddOption("Where is your favorite place to visit?", pl => true, pl =>
                {
                    DialogueModule favoriteModule = new DialogueModule("Ah, that's a tough one. I would say the coastal town of Trinsic. The people there are kind, and the ocean breeze is refreshing. Plus, they have the best markets filled with exotic spices and trinkets. It always feels like home.");
                    favoriteModule.AddOption("Maybe I'll visit there someday.", pla => true, pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla))); });
                    pl.SendGump(new DialogueGump(pl, favoriteModule));
                });
                p.SendGump(new DialogueGump(p, travelModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Fini nods gracefully and returns to her work.");
            });

        return greeting;
    }

    private bool HasFountainWall(PlayerMobile player)
    {
        // Check the player's inventory for FountainWall
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FountainWall)) != null;
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
        // Remove the FountainWall and give the RopeSpindle and MaxxiaScroll, then set the cooldown timer
        Item fountainWall = player.Backpack.FindItemByType(typeof(FountainWall));
        if (fountainWall != null)
        {
            fountainWall.Delete();
            player.AddToBackpack(new RopeSpindle());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the FountainWall and receive a RopeSpindle and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a FountainWall.");
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