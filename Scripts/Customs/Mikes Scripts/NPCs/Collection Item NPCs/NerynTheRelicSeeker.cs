using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class NerynTheRelicSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NerynTheRelicSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Neryn the Relic Seeker";
        Body = 0x190; // Human male body
        Hue = 33770; // Unique skin hue

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1157)); // Dark purple robe
        AddItem(new Sandals(2211)); // Golden sandals
        AddItem(new WizardsHat(1153)); // Dark blue wizard hat
        AddItem(new Scepter()); // A unique scepter held by Neryn

        VirtualArmor = 12;
    }

    public NerynTheRelicSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Neryn, a seeker of ancient relics and a brewer of potions made from rare herbs. Do you happen to possess any rare artifacts, perhaps a ColoredLamppost?");

        // Dialogue options
        greeting.AddOption("Tell me more about the relics you seek.", 
            p => true, 
            p =>
            {
                DialogueModule relicsModule = new DialogueModule("I am on a quest to collect relics that resonate with ancient magic. Among these, the ColoredLamppost holds a special energy I can harness. Perhaps we could strike a deal if you have one? I also use these relics to create potions, mysterious concoctions that can bring both healing and harm.");

                relicsModule.AddOption("What kind of potions do you make?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule potionModule = new DialogueModule("I brew potions using rare plants, many of which can only be found in the darkest corners of the forest. Some of these potions can heal grievous wounds, while others can grant visions or protect you from harm. But beware, some have... darker properties. Not all who seek my remedies come back unchanged.");

                        potionModule.AddOption("Why would anyone take the risk?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule riskModule = new DialogueModule("Desperation, my friend. Those who come to me often have no other choice. Perhaps they seek to cure an incurable ailment, or perhaps they seek power that cannot be obtained by ordinary means. My remedies are not for the faint of heart, but they work when all else fails.");

                                riskModule.AddOption("Can you brew me a potion?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule brewModule = new DialogueModule("I could brew a potion for you, but it comes at a cost. I will need rare herbs and a relic to channel the magic. Bring me a ColoredLamppost, and I shall see what I can do.");
                                        plaa.SendGump(new DialogueGump(plaa, brewModule));
                                    });

                                riskModule.AddOption("I think I'll pass for now.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Wise choice. Not all are prepared for the consequences of my potions.");
                                    });

                                pla.SendGump(new DialogueGump(pla, riskModule));
                            });

                        potionModule.AddOption("Do you have any potions for sale?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule saleModule = new DialogueModule("I do not sell my potions lightly. Each one is crafted for a specific purpose and requires rare ingredients. However, if you are truly in need, I may be able to part with one—for the right price, of course.");

                                saleModule.AddOption("What do you need in exchange?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule exchangeModule = new DialogueModule("I require something of value—perhaps a relic, or a rare herb that grows only in the shadows of ancient ruins. Bring me these, and I will consider your request.");
                                        plaa.SendGump(new DialogueGump(plaa, exchangeModule));
                                    });

                                saleModule.AddOption("I don't have anything like that.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Then I am afraid I cannot help you. My potions are not for the common barter.");
                                    });

                                pla.SendGump(new DialogueGump(pla, saleModule));
                            });

                        pl.SendGump(new DialogueGump(pl, potionModule));
                    });

                // Trade option after story
                relicsModule.AddOption("Do you wish to trade?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you bring me a ColoredLamppost, I will offer you either an EarthRelic or an ExoticWhistle, plus a MaxxiaScroll as an additional token of appreciation. These items hold great power, but they are only for those who dare to delve into the mysteries of the world.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a ColoredLamppost for me?");
                                tradeModule.AddOption("Yes, I have a ColoredLamppost.", 
                                    plaa => HasColoredLamppost(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasColoredLamppost(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a ColoredLamppost.");
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

                p.SendGump(new DialogueGump(p, relicsModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Neryn nods thoughtfully and returns to his contemplation of the relics.");
            });

        return greeting;
    }

    private bool HasColoredLamppost(PlayerMobile player)
    {
        // Check the player's inventory for ColoredLamppost
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ColoredLamppost)) != null;
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
        // Remove the ColoredLamppost and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item coloredLamppost = player.Backpack.FindItemByType(typeof(ColoredLamppost));
        if (coloredLamppost != null)
        {
            coloredLamppost.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for EarthRelic and ExoticWhistle
            rewardChoiceModule.AddOption("EarthRelic", pl => true, pl =>
            {
                pl.AddToBackpack(new EarthRelic());
                pl.SendMessage("You receive an EarthRelic!");
            });

            rewardChoiceModule.AddOption("ExoticWhistle", pl => true, pl =>
            {
                pl.AddToBackpack(new ExoticWhistle());
                pl.SendMessage("You receive an ExoticWhistle!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a ColoredLamppost.");
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