using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AsherahWildwood : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AsherahWildwood() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Asherah Wildwood";
        Title = "the Enigmatic Herbalist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(180);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit - Unique attire that befits a mystical forest dweller
        AddItem(new Cloak(2124)); // A green cloak resembling leaves
        AddItem(new Sandals(1445)); // Dark green sandals
        AddItem(new Kilt(2213)); // A long skirt in earthy colors
        AddItem(new PlateHelm()); // A handmade beaded necklace
        AddItem(new Backpack()); // Decorative item symbolizing her craft

        VirtualArmor = 15;
    }

    public AsherahWildwood(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Asherah Wildwood, an herbalist and guardian of the wild. Have you come seeking my rare concoctions, or perhaps something more... mysterious?");

        // Dialogue options
        greeting.AddOption("Tell me more about your craft.",
            p => true,
            p =>
            {
                DialogueModule craftModule = new DialogueModule("I gather rare herbs from the deepest parts of the forest, creating potent elixirs and charms. But my talents extend beyond mere herbs... I also read the cards. Have you heard of cartomancy?");

                // Nested dialogue about cartomancy
                craftModule.AddOption("Cartomancy? What is that?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule cartomancyModule = new DialogueModule("Cartomancy is the art of reading one's fate through cards. The cards speak truths that lie beneath the surface, things unseen, and sometimes, things better left unknown. Would you care for a reading?");

                        // Options to explore cartomancy further
                        cartomancyModule.AddOption("Yes, I want a reading.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule readingModule = new DialogueModule("Very well... Let us see what the cards reveal."
                                    + "\n*Asherah shuffles her deck with a strange intensity, her eyes seeming to darken as she draws a card.*"
                                    + "\nAh, the " + GetRandomCardName() + ". This card speaks of a looming shadow, a force that may bring either power or ruin. Beware, traveler, for not all gifts come without cost.");

                                // After reading
                                readingModule.AddOption("What do you mean by 'a cost'?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule costModule = new DialogueModule("The power of the cards is not without its price. The darkness whispers to those who seek its favor. The path ahead could be bright, but it could also be fraught with peril. Sometimes, the cards are a reflection of our own hidden desires.");
                                        costModule.AddOption("Can I change my fate?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("The future is fluid, ever-changing. Perhaps you can alter your course, but remember—each choice has consequences.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, costModule));
                                    });
                                readingModule.AddOption("Thank you for the reading.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Asherah nods, her eyes now softening as she shuffles the cards back into a neat pile.");
                                    });
                                pla.SendGump(new DialogueGump(pla, readingModule));
                            });

                        cartomancyModule.AddOption("No, I am not interested in a reading.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Very well, perhaps another time. Remember, the cards reveal what we most need, not necessarily what we want.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, cartomancyModule));
                    });

                craftModule.AddOption("What do you need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have an InscriptionTalisman, I can offer you something of equal value. Choose between an AlchemyTalisman or a HorseToken, and I shall also gift you a MaxxiaScroll as a token of my gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an InscriptionTalisman for me?");
                                tradeModule.AddOption("Yes, I have an InscriptionTalisman.",
                                    plaa => HasInscriptionTalisman(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasInscriptionTalisman(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an InscriptionTalisman.");
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
                        tradeIntroductionModule.AddOption("Maybe another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, craftModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Asherah nods and returns her focus to the herbs before her, the faint rustle of her cards still audible.");
            });

        return greeting;
    }

    private string GetRandomCardName()
    {
        string[] cards = { "The Tower", "The Hanged Man", "The Fool", "The Moon", "The Devil" };
        return cards[Utility.Random(cards.Length)];
    }

    private bool HasInscriptionTalisman(PlayerMobile player)
    {
        // Check the player's inventory for InscriptionTalisman
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(InscriptionTalisman)) != null;
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
        // Remove the InscriptionTalisman and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item inscriptionTalisman = player.Backpack.FindItemByType(typeof(InscriptionTalisman));
        if (inscriptionTalisman != null)
        {
            inscriptionTalisman.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for AlchemyTalisman and HorseToken
            rewardChoiceModule.AddOption("AlchemyTalisman", pl => true, pl =>
            {
                pl.AddToBackpack(new AlchemyTalisman());
                pl.SendMessage("You receive an AlchemyTalisman!");
            });

            rewardChoiceModule.AddOption("HorseToken", pl => true, pl =>
            {
                pl.AddToBackpack(new HorseToken());
                pl.SendMessage("You receive a HorseToken!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an InscriptionTalisman.");
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