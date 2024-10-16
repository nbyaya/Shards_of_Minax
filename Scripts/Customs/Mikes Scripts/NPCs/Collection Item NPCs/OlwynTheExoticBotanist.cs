using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class OlwynTheExoticBotanist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public OlwynTheExoticBotanist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Olwyn the Exotic Botanist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2129)); // Green robe
        AddItem(new Sandals(1443)); // Light brown sandals
        AddItem(new HalfApron(2213)); // Dark green apron
        AddItem(new StrawHat(2413)); // Straw hat
        AddItem(new ZombieHand()); // A basket with herbs, decorative

        VirtualArmor = 15;
    }

    public OlwynTheExoticBotanist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Olwyn, a botanist with a passion for exotic and rare plants. Have you come to trade?");

        // Dialogue options
        greeting.AddOption("Tell me about your work.", 
            p => true, 
            p =>
            {
                DialogueModule workModule = new DialogueModule("I travel far and wide to collect rare herbs and exotic woods. My research helps bring the beauty of nature to places where it has long since faded. Are you carrying anything special today?");

                workModule.AddOption("Why do you collect exotic plants?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule collectionReasonModule = new DialogueModule("There are secrets hidden within these plants. Some are medicinal, some are magical, and others... well, they whisper to me in the quiet of night. The world has forgotten their power, but I have not. The books I keep, the ones that tell of their use, they speak to me too, urging me to guard this knowledge."
                            + " Do you understand the weight of secrets, traveler?");

                        collectionReasonModule.AddOption("Secrets? Tell me more.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule secretsModule = new DialogueModule("Ah, secrets are like roots beneath the soil, tangled and deep. I am the guardian of the forbidden library, a place where texts that should never see the light of day reside."
                                    + " These books, they whisper to me. They share forgotten wisdom, but they also have a dark hunger. It is a burden I carry—one that makes me careful about what I share and with whom. Do you think you could bear such a burden?");

                                secretsModule.AddOption("I could try.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule burdenModule = new DialogueModule("Perhaps you could. But beware, for the knowledge in those books is not always kind."
                                            + " The exotic woods, too, have a history. They come from forests that no longer exist, torn down or burned. I have a few relics from those places. Would you like to see them?");

                                        burdenModule.AddOption("Yes, show me the relics.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule relicModule = new DialogueModule("These relics are all I have left of what once was. ExoticWoods from long-lost forests, whispered tales of spirits that guarded them."
                                                    + " If you have any ExoticWoods, I would gladly exchange them for something special. You may choose between a ZombieHand or an EasterDayEgg.");
                                                relicModule.AddOption("I'd like to make the trade.",
                                                    plaaaa => CanTradeWithPlayer(plaaaa),
                                                    plaaaa =>
                                                    {
                                                        DialogueModule tradeModule = new DialogueModule("Do you have ExoticWoods for me?");
                                                        tradeModule.AddOption("Yes, I have ExoticWoods.",
                                                            plaaaaa => HasExoticWoods(plaaaaa) && CanTradeWithPlayer(plaaaaa),
                                                            plaaaaa =>
                                                            {
                                                                CompleteTrade(plaaaaa);
                                                            });
                                                        tradeModule.AddOption("No, I don't have any right now.",
                                                            plaaaaa => !HasExoticWoods(plaaaaa),
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Come back when you have some ExoticWoods.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                                            plaaaaa => !CanTradeWithPlayer(plaaaaa),
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, tradeModule));
                                                    });
                                                relicModule.AddOption("Maybe another time.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, relicModule));
                                            });
                                        burdenModule.AddOption("No, I don't think I can handle this.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Wise choice. Knowledge, once learned, cannot be unlearned.");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, burdenModule));
                                    });
                                secretsModule.AddOption("No, I do not wish to know such things.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("That is wise. Some secrets are better left forgotten.");
                                    });
                                pla.SendGump(new DialogueGump(pla, secretsModule));
                            });

                        collectionReasonModule.AddOption("I understand. Such knowledge can be dangerous.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Indeed. The balance between knowledge and danger is delicate.");
                            });
                        pl.SendGump(new DialogueGump(pl, collectionReasonModule));
                    });

                // Trade option after story
                workModule.AddOption("Do you need any materials?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am always in search of ExoticWoods. If you have some, I would gladly trade you a special item in return. You may choose between a ZombieHand or an EasterDayEgg.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have ExoticWoods for me?");
                                tradeModule.AddOption("Yes, I have ExoticWoods.", 
                                    plaa => HasExoticWoods(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have any right now.", 
                                    plaa => !HasExoticWoods(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have some ExoticWoods.");
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

                p.SendGump(new DialogueGump(p, workModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Olwyn nods and smiles warmly.");
            });

        return greeting;
    }

    private bool HasExoticWoods(PlayerMobile player)
    {
        // Check the player's inventory for ExoticWoods
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExoticWoods)) != null;
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
        // Remove the ExoticWoods and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item exoticWoods = player.Backpack.FindItemByType(typeof(ExoticWoods));
        if (exoticWoods != null)
        {
            exoticWoods.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ZombieHand and EasterDayEgg
            rewardChoiceModule.AddOption("ZombieHand", pl => true, pl => 
            {
                pl.AddToBackpack(new ZombieHand());
                pl.SendMessage("You receive a ZombieHand!");
            });

            rewardChoiceModule.AddOption("EasterDayEgg", pl => true, pl =>
            {
                pl.AddToBackpack(new EasterDayEgg());
                pl.SendMessage("You receive an EasterDayEgg!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have ExoticWoods.");
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