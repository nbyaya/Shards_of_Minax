using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ElaraTheMystic : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ElaraTheMystic() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elara the Mystic of Seasons";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(75);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 1000;
        Karma = 1000;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1150)); // A mystic shroud
        AddItem(new Sandals(1358)); // Light blue sandals
        AddItem(new FancyDress(1372)); // A flowing blue-green dress
        AddItem(new QuarterStaff()); // Staff to symbolize her mystical nature

        VirtualArmor = 15;
    }

    public ElaraTheMystic(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Elara, guardian of the seasons. Do you have an ArtisanHolidayTree to celebrate the ever-changing cycle of nature?");

        // Dialogue options
        greeting.AddOption("Tell me more about the ArtisanHolidayTree.",
            p => true,
            p =>
            {
                DialogueModule infoModule = new DialogueModule("The ArtisanHolidayTree is a symbol of creativity and harmony. If you have one, I can offer you a special gift in return, to honor your efforts.");

                // Trade option after story
                infoModule.AddOption("What kind of trade do you offer?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have an ArtisanHolidayTree, you may choose between a DeathBlowItem or a WeddingChest, as well as receiving a special MaxxiaScroll.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an ArtisanHolidayTree for me?");
                                tradeModule.AddOption("Yes, I have an ArtisanHolidayTree.",
                                    plaa => HasArtisanHolidayTree(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasArtisanHolidayTree(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an ArtisanHolidayTree.");
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

                infoModule.AddOption("Why do you want the ArtisanHolidayTree?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("The ArtisanHolidayTree is not just a tree; it embodies the essence of creativity and unity, forged by the hands of artisans across the land. In it, I see the potential of magic and craftsmanship entwined.");
                        reasonModule.AddOption("That's quite fascinating. How did you come to be here?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule backstoryModule = new DialogueModule("Ah, I was once celebrated for my inventions. People called me brilliant, a genius even. But as my fame grew, so did the paranoia. I feared that my secrets, my work, would be taken from me, twisted by those who lacked understanding. I withdrew into my workshop, here in solitude. It's better this way, where my inventions are safe.");
                                backstoryModule.AddOption("What kind of inventions did you create?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule inventionModule = new DialogueModule("Inventions that could change the world, or so I believed. Devices that could manipulate the elements, mechanisms that could harness the power of the seasons. But such power comes at a cost, and the world is not ready for them. There are those who would abuse my creations for their own gain.");
                                        inventionModule.AddOption("Do you still create new inventions?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule currentWorkModule = new DialogueModule("I do, but in secrecy. The world may never see them, but I continue to create. I work with the seasons, drawing inspiration from the changes in nature. The ArtisanHolidayTree is part of that work—a manifestation of the harmony I strive to achieve.");
                                                currentWorkModule.AddOption("It must be lonely here.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule lonelinessModule = new DialogueModule("Lonely, yes, but peaceful. The silence allows me to think, to hear the whispers of the world. Besides, I have my creations to keep me company. They may not speak, but they understand.");
                                                        lonelinessModule.AddOption("I admire your dedication.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Elara smiles, a hint of warmth touching her otherwise guarded expression. 'Thank you, traveler. Few understand the burden of creation, but you seem to.'");
                                                            });
                                                        lonelinessModule.AddOption("Perhaps one day, you will share your creations with the world.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Elara looks away, her eyes distant. 'Perhaps... but not until the world is ready. Until then, I shall remain here, guarding my work.'");
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, lonelinessModule));
                                                    });
                                                currentWorkModule.AddOption("I hope your work brings you fulfillment.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Elara nods solemnly. 'It does, in its own way. Creation is both a joy and a burden, but it is the path I have chosen.'");
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, currentWorkModule));
                                            });
                                        inventionModule.AddOption("The world could use your brilliance.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Elara shakes her head slightly. 'Brilliance is a double-edged sword, traveler. Not all are ready for the light it brings.'");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, inventionModule));
                                    });
                                backstoryModule.AddOption("You must have seen much in your time.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Elara nods thoughtfully. 'I have seen the rise and fall of many, the changing of seasons both literal and metaphorical. Time moves forward, but some things remain constant.'");
                                    });
                                pla.SendGump(new DialogueGump(pla, backstoryModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });

                p.SendGump(new DialogueGump(p, infoModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Elara smiles and nods, her presence as calm as the changing seasons.");
            });

        return greeting;
    }

    private bool HasArtisanHolidayTree(PlayerMobile player)
    {
        // Check the player's inventory for ArtisanHolidayTree
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ArtisanHolidayTree)) != null;
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
        // Remove the ArtisanHolidayTree and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item holidayTree = player.Backpack.FindItemByType(typeof(ArtisanHolidayTree));
        if (holidayTree != null)
        {
            holidayTree.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for DeathBlowItem and WeddingChest
            rewardChoiceModule.AddOption("DeathBlowItem", pl => true, pl =>
            {
                pl.AddToBackpack(new DeathBlowItem());
                pl.SendMessage("You receive a DeathBlowItem!");
            });

            rewardChoiceModule.AddOption("WeddingChest", pl => true, pl =>
            {
                pl.AddToBackpack(new WeddingChest());
                pl.SendMessage("You receive a WeddingChest!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an ArtisanHolidayTree.");
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