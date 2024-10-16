using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ArcadiaTheArtisan : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ArcadiaTheArtisan() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Arcadia the Artisan";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(55);
        SetDex(70);
        SetInt(110);

        SetHits(90);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit - A unique, artisan-themed appearance
        AddItem(new FancyDress(1254)); // Fancy dress with a lavender hue
        AddItem(new Sandals(1153)); // Sandals with a dark blue hue
        AddItem(new HalfApron(1809)); // Artisan apron for an authentic craftsman look
        AddItem(new SmithHammer()); // An artisan's hammer as her signature item

        VirtualArmor = 12;
    }

    public ArcadiaTheArtisan(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Arcadia, a humble artisan known for crafting wonders. Do you have an interest in rare items and exchanges?");

        // Introduce her backstory and work with nested details
        greeting.AddOption("Tell me about your work.", 
            p => true, 
            p =>
            {
                DialogueModule workModule = new DialogueModule("I specialize in creating unique tools and accessories. My greatest joy is crafting rare items, but my work is far from ordinary. Sometimes, I delve into the arcane and ancient, piecing together lost knowledge.");
                workModule.AddOption("What kind of knowledge have you uncovered?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule knowledgeModule = new DialogueModule("Ah, you wish to know of my studies... well, I have stumbled upon ancient texts that speak of horrific rituals, rituals designed to awaken dark gods. It is said that the crafters of old wielded these rituals to forge items of immense power, but at a terrible cost.");
                        knowledgeModule.AddOption("Tell me more about these rituals.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule ritualsModule = new DialogueModule("The rituals involve the use of rare components, each gathered under specific celestial conditions. They required Moonshadow Dew, the Heart of a Forsaken One, and a MagicBookStand—items that are exceedingly difficult to procure. The texts describe them as keys to unlocking the ancient power buried beneath the earth, power that could shape or destroy our world.");
                                ritualsModule.AddOption("Why are you so interested in these rituals?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule obsessionModule = new DialogueModule("I am... obsessed, I admit it. The more I read, the more I see patterns—connections between these rituals and the events unfolding around us. I believe that if I can gather enough information, I may be able to prevent something terrible from happening. Or, at the very least, document it so future generations might understand the mistakes we made.");
                                        obsessionModule.AddOption("That sounds dangerous. Shouldn't you stop?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule dangerModule = new DialogueModule("Stop? You think I haven't tried? The truth is, once you glimpse the darkness, you cannot look away. The fear drives me, but so does the hope that perhaps I can make a difference. There are forces moving in the shadows, forces that few are aware of, and I must know the truth.");
                                                dangerModule.AddOption("What kind of forces?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule forcesModule = new DialogueModule("Dark gods, forgotten spirits, beings that feed on chaos and despair. I have read accounts of entire civilizations wiped out because they dabbled too deeply into forbidden arts. I fear we are on the same path. The MagicBookStand is a small piece of a much larger puzzle, one that I am desperately trying to solve before it's too late.");
                                                        forcesModule.AddOption("Is there any way to stop them?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule stopModule = new DialogueModule("The only way to stop them is through knowledge—understanding what they are, what they want, and how they can be contained. That is why I need items like the MagicBookStand. Each piece brings me closer to understanding their language, their rituals, and their weaknesses. But it is a race against time, and I fear I am losing.");
                                                                stopModule.AddOption("How can I help?",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule helpModule = new DialogueModule("If you wish to help, gather items of power and bring them to me. The MagicBookStand is one, but there are others—Sunstone Shards, Nightbloom Petals, and the Tears of the Forgotten. With these, I can continue my research. Just know that this path is fraught with peril, and once you begin, there is no turning back.");
                                                                        helpModule.AddOption("I'll do what I can.",
                                                                            plaaaaaaa => true,
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("Arcadia nods, a mixture of gratitude and fear in her eyes. 'Thank you, traveler. May fate be kinder to you than it has been to me.'");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        helpModule.AddOption("This is too much for me.",
                                                                            plaaaaaaa => true,
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("Arcadia sighs, her shoulders slumping. 'I understand. Not everyone is prepared to face what lies ahead. Be safe, traveler.'");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, helpModule));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, stopModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, forcesModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, dangerModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, obsessionModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, ritualsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, knowledgeModule));
                    });
                workModule.AddOption("What kind of components do you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule componentModule = new DialogueModule("I am in need of a MagicBookStand for my next creation. In exchange, I can offer you a PersonalMortar, and I will always provide a MaxxiaScroll for your efforts.");
                        componentModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MagicBookStand for me?");
                                tradeModule.AddOption("Yes, I have a MagicBookStand.", 
                                    plaa => HasMagicBookStand(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasMagicBookStand(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a MagicBookStand.");
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
                        componentModule.AddOption("Perhaps another time.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, componentModule));
                    });
                p.SendGump(new DialogueGump(p, workModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Arcadia nods thoughtfully as you part ways.");
            });

        return greeting;
    }

    private bool HasMagicBookStand(PlayerMobile player)
    {
        // Check the player's inventory for MagicBookStand
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MagicBookStand)) != null;
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
        // Remove the MagicBookStand and give the PersonalMortar and MaxxiaScroll, then set the cooldown timer
        Item magicBookStand = player.Backpack.FindItemByType(typeof(MagicBookStand));
        if (magicBookStand != null)
        {
            magicBookStand.Delete();
            player.AddToBackpack(new PersonalMortor());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MagicBookStand and receive a PersonalMortar and a MaxxiaScroll in return. Thank you for your assistance!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a MagicBookStand.");
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