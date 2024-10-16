using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class OrionTheWanderingSoothsayer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public OrionTheWanderingSoothsayer() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Orion the Wandering Soothsayer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // Dark blue shirt
        AddItem(new LongPants(2209)); // Black pants
        AddItem(new Sandals(1161)); // Light blue sandals
        AddItem(new Cloak(1153)); // Mysterious purple cloak
        AddItem(new WizardsHat(1157)); // Matching dark blue wizard hat
        AddItem(new CrystalBall()); // Adds a unique crystal ball as a prop

        VirtualArmor = 15;
    }

    public OrionTheWanderingSoothsayer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a curious soul approaches! I am Orion, a wandering soothsayer with glimpses of fate. Once, I was a soldier—a man with purpose—but now I wander, burdened with knowledge of what lies beyond. Do you seek to know your destiny, traveler?");

        // Start with dialogue about his past and philosophy
        greeting.AddOption("You mentioned being a soldier. Tell me about your past.",
            p => true,
            p =>
            {
                DialogueModule pastModule = new DialogueModule("Yes, I was once a soldier, fighting for a cause I believed in. We thought we fought for the good of the realm, but what I learned on those fields of blood was that our enemy was not what we thought. The true enemy lurks beyond the stars, beyond what most can see or comprehend.");
                pastModule.AddOption("What do you mean by 'beyond the stars'?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule beyondStarsModule = new DialogueModule("During the darkest nights on the battlefield, I saw them—shadows that moved like smoke, whispering unspeakable horrors. They weren't of this world. They manipulated both sides, pulling strings we couldn't see. It wasn't until I stood alone in the silence that I understood their motives. The war was just a distraction. They feed on fear, pain, and despair.");
                        beyondStarsModule.AddOption("Did anyone else see them?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule othersSawModule = new DialogueModule("Some did, but most refused to acknowledge it. They were too blinded by orders and patriotism. The few who spoke of these beings disappeared without a trace. I learned to keep quiet, to observe. Those who survive learn to adapt—to pretend they see nothing. It was the only way to stay alive.");
                                othersSawModule.AddOption("Why didn’t you warn others?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule warningModule = new DialogueModule("I tried. My superiors called me mad, a coward who had lost his nerve. They ridiculed me until I stopped trying. But deep down, I knew the truth. The horrors weren't just visions—they were real. The war was merely a smokescreen for something far more insidious.");
                                        warningModule.AddOption("How did you survive it all?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule survivalModule = new DialogueModule("Surviving wasn't about bravery. It was about luck and knowing when to run. I learned to be invisible, to stay in the shadows, just like those beings. I grew resentful, haunted by the things I saw—the friends I lost. The enemy wasn’t just flesh and blood; it was something much darker. It changes you. Even now, I feel their gaze upon me.");
                                                survivalModule.AddOption("You seem haunted by this. Do they still follow you?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule hauntedModule = new DialogueModule("Yes, they do. I see them sometimes in the corners of my vision, lingering in the darkness of night. They are patient. They don’t need to rush. I am but one man. They have eternity. I’ve learned to accept it—to stay quiet and endure. It’s all I can do.");
                                                        hauntedModule.AddOption("Why haven’t you given up?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule enduranceModule = new DialogueModule("Giving up would mean they win. Every day I endure is a small victory against the darkness. And maybe, just maybe, my words will reach someone who can do more than I could—someone with the courage to face them directly. Until then, I share my tale, hoping it’s not too late.");
                                                                enduranceModule.AddOption("Your story is powerful. I will remember it.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Orion nods, his eyes distant. 'Perhaps that is all I can ask.'");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, enduranceModule));
                                                            });
                                                        hauntedModule.AddOption("I’m sorry you carry this burden.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Orion sighs, his expression weary. 'We all carry our burdens, some heavier than others.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, hauntedModule));
                                                    });
                                                survivalModule.AddOption("I can see why this haunts you.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Orion nods slowly. 'It’s a weight I carry every day, one that never truly leaves.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, survivalModule));
                                            });
                                        warningModule.AddOption("I see. Thank you for sharing.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Orion offers a weary smile. 'Perhaps sharing is all I have left.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, warningModule));
                                    });
                                othersSawModule.AddOption("That must have been terrifying.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Orion's eyes grow distant. 'Terrifying, yes. But even more so, it was disheartening. Knowing that the enemy wasn't one we could fight—not with swords or steel.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, othersSawModule));
                            });
                        beyondStarsModule.AddOption("How did you cope with this knowledge?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule copeModule = new DialogueModule("I didn’t, not really. I drank, I isolated myself, and eventually, I walked away from everything—from the army, from the people I knew. I became a wanderer. The weight of knowledge is a heavy burden, and most days, I wonder if I’ve made the right choice in keeping it to myself.");
                                copeModule.AddOption("It must be a difficult existence.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Orion gives a hollow laugh. 'Difficult, yes. But it is mine, and I suppose that’s enough.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, copeModule));
                            });
                        pl.SendGump(new DialogueGump(pl, beyondStarsModule));
                    });
                pastModule.AddOption("That sounds like a terrible experience.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Orion closes his eyes, his expression pained. 'You have no idea. The horrors of war are nothing compared to what lies beyond.'");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, pastModule));
            });

        // Trade-related dialogue
        greeting.AddOption("Do you need the 'Hand of Fate'?",
            p => true,
            p =>
            {
                DialogueModule tradeModule = new DialogueModule("Indeed, if you possess the 'Hand of Fate', I can offer you an AnimalBox in exchange, alongside a special scroll of my own making. But beware, my help is limited and can only be offered once every 10 minutes.");
                tradeModule.AddOption("I have the 'Hand of Fate'. Let's trade.",
                    pl => HasHandOfFate(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                tradeModule.AddOption("I don’t have it right now.",
                    pl => !HasHandOfFate(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have the 'Hand of Fate'.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                tradeModule.AddOption("I recently traded; I'll return later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, tradeModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Orion nods knowingly, as if seeing something just beyond your vision.");
            });

        return greeting;
    }

    private bool HasHandOfFate(PlayerMobile player)
    {
        // Check the player's inventory for HandOfFate
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HandOfFate)) != null;
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
        // Remove the HandOfFate and give the AnimalBox and MaxxiaScroll, then set the cooldown timer
        Item handOfFate = player.Backpack.FindItemByType(typeof(HandOfFate));
        if (handOfFate != null)
        {
            handOfFate.Delete();
            player.AddToBackpack(new AnimalBox());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the 'Hand of Fate' and receive an AnimalBox and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the 'Hand of Fate'.");
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