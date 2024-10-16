using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class FumTheBotanist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FumTheBotanist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Fum the Botanist";
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
        AddItem(new PineResin()); // Her reward item is also in her inventory, so it can be stolen

        VirtualArmor = 10;
    }

    public FumTheBotanist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings! I sense you are drawn to nature as I am. I'm Fum, a botanist devoted to the preservation of rare flora. Do you seek knowledge? Perhaps some advice on the plants of the wasteland? I am also an experienced cartographer, mapping the dangerous terrain of this land. My maps have saved many lives, though I can't help but remember those who weren't so fortunate...");

        // Start with dialogue about her work
        greeting.AddOption("Tell me about the plants you study.", 
            p => true, 
            p =>
            {
                DialogueModule plantsModule = new DialogueModule("Ah, the plants of the realm! Each one holds a secret waiting to be uncovered. I have encountered many in my journeys. Shall I tell you about the Moonflower, Crimson Briar, Sunpetal, or perhaps the elusive Glowroot?");
                
                // Nested options for each plant
                plantsModule.AddOption("Tell me about the Moonflower.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule moonflowerModule = new DialogueModule("The Moonflower is a rare nocturnal plant. It only blooms under the full moon's light. Its petals are said to possess restorative properties, used in healing elixirs. However, gathering it is dangerous, as it attracts shadowy creatures that guard it fiercely. The shadows... they almost got me once. I still remember that chilling feeling, as if they were whispering to me.");
                        moonflowerModule.AddOption("How is it used in healing elixirs?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule healingModule = new DialogueModule("The petals, when carefully mixed with dew collected at dawn, create a potent elixir that accelerates natural healing. But you must be precise—any misstep and the elixir could become toxic. I can still remember the first time I tried; the anxiety was overwhelming, but I succeeded.");
                                healingModule.AddOption("Fascinating! Thank you.", 
                                    plaaq => true, 
                                    plaaq =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, healingModule));
                            });
                        moonflowerModule.AddOption("That sounds quite risky.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendMessage("Indeed, only those with steady hands and nerves of steel should attempt it.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, moonflowerModule));
                    });

                plantsModule.AddOption("Tell me about the Crimson Briar.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule briarModule = new DialogueModule("Crimson Briar is a deadly vine that grows in deep, shadowed forests. Its thorns are poisonous, but it produces a bright red berry that, when properly distilled, can serve as an antidote to its own poison. Few dare to collect it, for the vine has a mind of its own, ensnaring those who approach. It... it once took someone dear to me. I tried to map out a safe route, but I failed them.");
                        briarModule.AddOption("How do you distill the antidote?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule antidoteModule = new DialogueModule("To prepare the antidote, you must extract the juice from the berries and mix it with the ash of burnt sage. Timing is crucial, as the mixture must be left to sit under starlight for exactly three hours. It’s an anxious process, but if done correctly, the antidote can neutralize almost any poison.");
                                antidoteModule.AddOption("I'll keep that in mind. Thank you.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, antidoteModule));
                            });
                        briarModule.AddOption("That sounds too risky for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, briarModule));
                    });

                plantsModule.AddOption("Tell me about the Sunpetal.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule sunpetalModule = new DialogueModule("The Sunpetal thrives in bright, open meadows. It's vibrant yellow petals are said to carry the warmth of the sun, and can be used in concoctions that protect against frostbite. I have used it myself during my expeditions into the cold northern wastelands. It gave me the courage to keep going despite the odds.");
                        sunpetalModule.AddOption("How do you prepare it for frostbite protection?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule preparationModule = new DialogueModule("You must crush the petals into a paste and mix it with bear fat. It's not a pleasant mixture, but it insulates the skin against even the harshest cold. I always carry a small jar with me, just in case.");
                                preparationModule.AddOption("Thank you for the information.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, preparationModule));
                            });
                        sunpetalModule.AddOption("Thank you, but I am not fond of the cold.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, sunpetalModule));
                    });

                plantsModule.AddOption("Tell me about the Glowroot.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule glowrootModule = new DialogueModule("Glowroot is a plant that grows in the deepest caves, where no natural light reaches. It emits a faint bioluminescent glow, enough to light one's way in the dark. I once relied on Glowroot during a mapping expedition in the caverns of the wasteland. It was both terrifying and exhilarating—each step deeper brought new wonders, but also new dangers.");
                        glowrootModule.AddOption("How does it emit light?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule lightModule = new DialogueModule("The roots contain a natural enzyme that reacts with the cave's moisture, causing a glow. It's fragile, though—any disturbance can cause the plant to cease glowing. I learned that the hard way once, and had to find my way out in complete darkness. It was a harrowing experience.");
                                lightModule.AddOption("Incredible! Thank you.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, lightModule));
                            });
                        glowrootModule.AddOption("That sounds too dangerous for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, glowrootModule));
                    });

                // After they’ve learned about the plants, introduce the trade option
                plantsModule.AddOption("Is there anything you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of a Decorative Orchid for some of my research. In return, I can offer you an Important Book and always a Maxxia Scroll as thanks. Please understand, I can only afford to make this exchange once every ten minutes.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a Decorative Orchid for me?");
                                tradeModule.AddOption("Yes, I have a Decorative Orchid.", 
                                    plaa => HasDecorativeOrchid(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasDecorativeOrchid(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a Decorative Orchid.");
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
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, plantsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Fum nods gracefully, though her eyes seem to wander as if lost in thought.");
            });

        return greeting;
    }

    private bool HasDecorativeOrchid(PlayerMobile player)
    {
        // Check the player's inventory for Decorative Orchid
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DecorativeOrchid)) != null;
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
        // Remove the Decorative Orchid and give the Important Book, then set the cooldown timer
        Item orchid = player.Backpack.FindItemByType(typeof(DecorativeOrchid));
        if (orchid != null)
        {
            orchid.Delete();
            player.AddToBackpack(new ImportantBooks());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Decorative Orchid and receive an Important Book and a Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Decorative Orchid.");
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