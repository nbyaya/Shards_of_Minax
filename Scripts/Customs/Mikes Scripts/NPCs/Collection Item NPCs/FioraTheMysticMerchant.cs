using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FioraTheMysticMerchant : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FioraTheMysticMerchant() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Fiora the Mystic Merchant";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1175)); // Mystic purple robe
        AddItem(new Sandals(1150)); // Golden sandals
        AddItem(new WizardsHat(1153)); // Mysterious wizard's hat
        AddItem(new GoldBracelet());
        AddItem(new GoldRing());

        VirtualArmor = 15;
    }

    public FioraTheMysticMerchant(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Fiora, a collector of rare artifacts. Tell me, do you happen to possess a MahJongTile?");

        // Dialogue options
        greeting.AddOption("What do you offer in exchange for a MahJongTile?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("If you have a MahJongTile, I can offer you a choice between an AlchemyTalisman or a CartographyTable, along with a MaxxiaScroll for your efforts.");

                tradeIntroductionModule.AddOption("I have a MahJongTile and would like to trade.",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have the MahJongTile with you?");

                        tradeModule.AddOption("Yes, I have it right here.",
                            plaa => HasMahJongTile(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });

                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasMahJongTile(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a MahJongTile.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });

                tradeIntroductionModule.AddOption("Maybe another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });

                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Tell me about yourself.",
            p => true,
            p =>
            {
                DialogueModule backstoryModule = new DialogueModule("I wasn't always a merchant. I had a husband once, a man who was brave and foolish. He dealt in dangerous artifacts, and one day he didn't return. They told me it was an accident, but I know better. Now, I continue his work, trying to find the truth, one artifact at a time.");

                backstoryModule.AddOption("That must be difficult for you.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule resilienceModule = new DialogueModule("Difficult? Yes. But I am resilient. My husband taught me well. I learned to navigate the shadows, to be cunning when faced with danger. This trade, this search... it keeps me going.");

                        resilienceModule.AddOption("What happened to your husband?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule husbandModule = new DialogueModule("He was betrayed. I don't know by whom, but I have my suspicions. There are those who would kill for the power these artifacts hold. He was too trusting, and it cost him his life.");

                                husbandModule.AddOption("Do you seek revenge?",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule revengeModule = new DialogueModule("Revenge? Perhaps. But more than that, I seek justice. I want to see those who wronged him brought to light, to strip away their secrets and power. And if vengeance is part of that, so be it.");

                                        revengeModule.AddOption("How can I help?",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("If you truly wish to help, bring me artifacts. The rarer, the better. Each piece brings me closer to the truth. And who knows, perhaps we can expose those in the shadows together.");

                                                helpModule.AddOption("I will keep an eye out.",
                                                    plaabcd => true,
                                                    plaabcd =>
                                                    {
                                                        plaabcd.SendMessage("Fiora nods, her eyes filled with determination. 'Good. Together, we will find the answers we seek.'");
                                                    });

                                                plaabc.SendGump(new DialogueGump(plaabc, helpModule));
                                            });

                                        revengeModule.AddOption("That sounds dangerous. I wish you luck.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                plaabc.SendMessage("Fiora gives a wry smile. 'Danger is all I have left. But thank you, traveler.'");
                                            });

                                        plaab.SendGump(new DialogueGump(plaab, revengeModule));
                                    });

                                husbandModule.AddOption("I'm sorry for your loss.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Fiora nods, her expression softening slightly. 'Thank you. But save your sorrow for those still in danger.'");
                                    });

                                plaa.SendGump(new DialogueGump(plaa, husbandModule));
                            });

                        resilienceModule.AddOption("You are strong to carry on like this.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Fiora smiles, though it doesn't reach her eyes. 'Strength is all I have left. Strength, and a purpose.'");
                            });

                        pl.SendGump(new DialogueGump(pl, resilienceModule));
                    });

                backstoryModule.AddOption("What kind of artifacts do you deal in?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("Artifacts of power, of mystery. Some are simple trinkets, others hold secrets that could shake the world. My husband believed that knowledge must be protected, and now I do too. But I also know that such power draws dangerous people.");

                        artifactsModule.AddOption("Dangerous people? Like who?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule enemiesModule = new DialogueModule("There are factions, cults, and even some nobles who would stop at nothing to acquire what I have. They want control, power over others. They hide behind masks, but I see them for what they are.");

                                enemiesModule.AddOption("Do they know about you?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule knowledgeModule = new DialogueModule("Some do, and they watch me closely. Others underestimate me, believing I am just a widow playing a dangerous game. I let them think that. It keeps me alive.");

                                        knowledgeModule.AddOption("You are braver than most.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Fiora gives a sad smile. 'Bravery is often born of necessity, traveler. I do what I must.'");
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, knowledgeModule));
                                    });

                                enemiesModule.AddOption("You must be careful.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Fiora nods, her expression darkening. 'Careful, yes. Always. But I will not stop until I find the truth.'");
                                    });

                                pla.SendGump(new DialogueGump(pla, enemiesModule));
                            });

                        pl.SendGump(new DialogueGump(pl, artifactsModule));
                    });

                p.SendGump(new DialogueGump(p, backstoryModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Fiora nods knowingly, her eyes glimmering with secrets untold.");
            });

        return greeting;
    }

    private bool HasMahJongTile(PlayerMobile player)
    {
        // Check the player's inventory for MahJongTile
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MahJongTile)) != null;
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
        // Remove the MahJongTile and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item mahJongTile = player.Backpack.FindItemByType(typeof(MahJongTile));
        if (mahJongTile != null)
        {
            mahJongTile.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for AlchemyTalisman and CartographyTable
            rewardChoiceModule.AddOption("AlchemyTalisman", pl => true, pl =>
            {
                pl.AddToBackpack(new AlchemyTalisman());
                pl.SendMessage("You receive an AlchemyTalisman!");
            });

            rewardChoiceModule.AddOption("CartographyTable", pl => true, pl =>
            {
                pl.AddToBackpack(new CartographyTable());
                pl.SendMessage("You receive a CartographyTable!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a MahJongTile.");
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