using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class XyaraTheArtifactCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public XyaraTheArtifactCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Xyara the Artifact Collector";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1153)); // Fancy Shirt with a unique blue hue
        AddItem(new Skirt(1175)); // Skirt with a light teal hue
        AddItem(new Sandals(1109)); // Sandals with a dark blue hue
        AddItem(new GoldBracelet()); // A gold bracelet to add a bit of flair
        AddItem(new FeatheredHat(1150)); // A feathered hat for a distinct look

        VirtualArmor = 15;
    }

    public XyaraTheArtifactCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! You seem like someone who appreciates the wonders of rare artifacts. I am Xyara, a collector of mystical items and curiosities. Might you be interested in a trade?");

        // Start with dialogue about her collection
        greeting.AddOption("What kind of artifacts do you collect?",
            p => true,
            p =>
            {
                DialogueModule artifactModule = new DialogueModule("I collect anything with a story to tell. Ancient tomes, enchanted trinkets, and items that hold the whispers of forgotten times. But lately, I've been seeking something very specific... a BlueSand item. Perhaps you have one?");
                artifactModule.AddOption("What is a BlueSand item?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule blueSandModule = new DialogueModule("Ah, BlueSand items are mysterious artifacts found in the depths of the desert dunes. They hold a unique, shimmering blue grain that can enhance other magical artifacts. They are very rare, but I can offer something special if you bring one to me.");
                        blueSandModule.AddOption("What will you offer in return?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule offerModule = new DialogueModule("In exchange for a BlueSand item, I can offer you a HildebrandtFlag, an item of great significance to explorers. I will also include a MaxxiaScroll, a useful reward for any adventurer. However, I can only make this trade once every 10 minutes per visitor.");
                                offerModule.AddOption("I have a BlueSand item.",
                                    plaa => HasBlueSandItem(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                offerModule.AddOption("I don't have one right now.",
                                    plaa => !HasBlueSandItem(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("No worries, traveler. Come back when you have found a BlueSand item.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                offerModule.AddOption("I recently traded; I'll return later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Patience is a virtue, dear traveler. You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, offerModule));
                            });
                        blueSandModule.AddOption("Tell me more about you, Xyara.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule aboutModule = new DialogueModule("Oh, I could speak for days about my passions! I am not only a collector but also an inventor, a tinkerer. My true passion lies in the study of mechanics and cybernetics. I dream of creating a new breed of machines that could assist us in surviving these harsh lands.");
                                aboutModule.AddOption("Cybernetics? That sounds fascinating!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule cyberneticsModule = new DialogueModule("Indeed! Imagine melding the organic with the mechanical, creating something that is both alive and enhanced. I believe we could use mechanical limbs to aid those who are injured or machines that could help gather resources without tiring. My dream is to build a world where humanity and technology coexist harmoniously.");
                                        cyberneticsModule.AddOption("What inspired you to pursue this?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule inspirationModule = new DialogueModule("Ah, it all started when I found an old, broken automaton in the ruins of a forgotten city. It was a magnificent piece of craftsmanship, even in disrepair. I spent months studying it, trying to understand how it worked. That experience sparked my passion for mechanical enhancements. I want to bring such wonders back to life.");
                                                inspirationModule.AddOption("Do you think such creations could change the world?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule changeWorldModule = new DialogueModule("Absolutely! Imagine a future where we no longer fear injury because mechanical enhancements could replace lost limbs. Imagine a future where mundane tasks are handled by tireless machines, leaving us free to pursue knowledge, art, and exploration. I am eccentric, some say, but I am also passionate about what I do.");
                                                        changeWorldModule.AddOption("That is truly innovative, Xyara.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Xyara beams at your words, clearly proud of her ambitions.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, changeWorldModule));
                                                    });
                                                inspirationModule.AddOption("That is quite a story. Thank you for sharing.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Xyara nods, her eyes gleaming with enthusiasm.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, inspirationModule));
                                            });
                                        cyberneticsModule.AddOption("It sounds risky. Are you not afraid of failure?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule riskModule = new DialogueModule("Of course, there is always a risk. But without risk, there can be no reward. Failure is simply a step along the path to success. I have failed countless times, but each failure teaches me something new. I am nothing if not persistent.");
                                                riskModule.AddOption("That is a brave way to look at it.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Xyara smiles, her eyes filled with determination.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                riskModule.AddOption("I wish you luck in your endeavors.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Xyara nods appreciatively.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, riskModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, cyberneticsModule));
                                    });
                                aboutModule.AddOption("Why mix mechanics with magic?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule mechanicsMagicModule = new DialogueModule("Magic alone is powerful, but combining it with mechanics opens up new possibilities. Magic can be unstable, but mechanics provide structure. Together, they create something greater than the sum of their parts. My inventions are not just machines; they are alive with magic.");
                                        mechanicsMagicModule.AddOption("That is quite visionary.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Xyara smiles warmly, clearly enjoying the conversation.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        mechanicsMagicModule.AddOption("It must be difficult to balance the two.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule balanceModule = new DialogueModule("Oh, it is indeed! Magic can be unpredictable, and mechanics are rigid. Finding harmony between the two is an art. But I thrive on challenges, and I will not stop until I perfect my craft.");
                                                balanceModule.AddOption("I admire your dedication, Xyara.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Xyara gives you a grateful nod, her eyes glinting with determination.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, balanceModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, mechanicsMagicModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, aboutModule));
                            });
                        pl.SendGump(new DialogueGump(pl, blueSandModule));
                    });
                p.SendGump(new DialogueGump(p, artifactModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Xyara nods and smiles mysteriously.");
            });

        return greeting;
    }

    private bool HasBlueSandItem(PlayerMobile player)
    {
        // Check the player's inventory for BlueSand item
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BlueSand)) != null;
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
        // Remove the BlueSand item and give the HildebrandtFlag and MaxxiaScroll, then set the cooldown timer
        Item blueSandItem = player.Backpack.FindItemByType(typeof(BlueSand));
        if (blueSandItem != null)
        {
            blueSandItem.Delete();
            player.AddToBackpack(new HildebrandtFlag());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the BlueSand item and receive a HildebrandtFlag and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a BlueSand item.");
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