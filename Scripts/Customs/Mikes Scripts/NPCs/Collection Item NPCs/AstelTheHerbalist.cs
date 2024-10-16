using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AstelTheHerbalist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AstelTheHerbalist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Astel the Herbalist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1109)); // Dark green hooded robe
        AddItem(new Sandals(1153)); // Sandals with a light blue hue
        AddItem(new ExoticBoots()); // The item involved in the trade (can also be stolen)

        VirtualArmor = 15;
    }

    public AstelTheHerbalist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Astel, a humble herbalist exploring the mysteries of nature. Do you perhaps have something of interest for me today?");

        // Offer some dialogue about the natural remedies
        greeting.AddOption("Tell me about your natural remedies.",
            p => true,
            p =>
            {
                DialogueModule remediesModule = new DialogueModule("My remedies come from herbs and other natural materials. I've concocted mixtures that can heal wounds, cure afflictions, and even grant vitality to the weary. Nature holds all answers, if one knows where to look.");
                remediesModule.AddOption("What kind of materials do you need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule materialsModule = new DialogueModule("Ah, I am often in need of rare items. In fact, I am seeking ExoticBoots at the moment. If you have a pair, I would be willing to trade something useful in return.");
                        materialsModule.AddOption("I have ExoticBoots for trade.",
                            pla => HasExoticBoots(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });
                        materialsModule.AddOption("I don't have ExoticBoots right now.",
                            pla => !HasExoticBoots(pla),
                            pla =>
                            {
                                pla.SendMessage("No worries, bring them to me when you have them.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        materialsModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("I can only trade once every 10 minutes. Please return later.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, materialsModule));
                    });
                remediesModule.AddOption("Tell me more about your work.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule workModule = new DialogueModule("Ah, but my work is not without its complexities. You see, I am also an artist, painting with... shall we say, unconventional materials. Radiation, to be exact. It gives a marvelous glow to my creations, though I often struggle with the consequences of my art.");
                        workModule.AddOption("Radiation? Isn't that dangerous?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule radiationModule = new DialogueModule("Dangerous? Yes, yes, indeed! But beauty is born from chaos, and the glow... oh, the glow, it is unlike anything else. A luminous reminder of destruction's potential for beauty. My art reflects that duality. Would you care to see one of my pieces?");
                                radiationModule.AddOption("I'd love to see one.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule showArtModule = new DialogueModule("Behold, 'The Shimmering Wasteland'! A painting made with radium dust and luminescent herbs. It glows in the dark, a testament to the beauty hidden within the ruins. But alas, it also carries a cost... prolonged exposure is unwise.");
                                        showArtModule.AddOption("It's beautiful, but dangerous indeed.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Astel smiles sadly, his eyes lost in the painting's glow.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, showArtModule));
                                    });
                                radiationModule.AddOption("No, I think I'll pass.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Wise decision. Not all can bear the glow.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, radiationModule));
                            });
                        workModule.AddOption("What do you mean by the consequences of your art?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule consequenceModule = new DialogueModule("Ah, the consequences, yes. Radiation is both my muse and my curse. It sickens me, as it would sicken anyone exposed for too long. Yet, I cannot stop. The glowing pigments call to me, whispering of beauty that must be revealed. I often wonder, is it worth it?");
                                consequenceModule.AddOption("That sounds like a terrible burden.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule burdenModule = new DialogueModule("A burden, yes. But art is never easy, is it? It is suffering and joy intertwined. The world must see beauty even in destruction. And if I am to bear this cost for others to see, so be it. Perhaps one day, my glowing works will inspire others to find balance between creation and devastation.");
                                        burdenModule.AddOption("You are very dedicated.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Astel's eyes glimmer with both pride and exhaustion.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, burdenModule));
                                    });
                                consequenceModule.AddOption("You should stop, for your own health.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Astel sighs deeply. 'Perhaps... but the call of the glow is too strong, and beauty must have its champions.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, consequenceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, workModule));
                    });
                remediesModule.AddOption("Thank you for the information.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, remediesModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Astel nods thoughtfully.");
            });

        return greeting;
    }

    private bool HasExoticBoots(PlayerMobile player)
    {
        // Check the player's inventory for ExoticBoots
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExoticBoots)) != null;
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
        // Remove the ExoticBoots and give the FillerPowder and MaxxiaScroll, then set the cooldown timer
        Item exoticBoots = player.Backpack.FindItemByType(typeof(ExoticBoots));
        if (exoticBoots != null)
        {
            exoticBoots.Delete();
            player.AddToBackpack(new FillerPowder());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ExoticBoots and receive FillerPowder and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the ExoticBoots.");
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