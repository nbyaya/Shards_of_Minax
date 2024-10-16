using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class IantheTheBloomingAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public IantheTheBloomingAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ianthe the Blooming Alchemist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(60);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 1000;
        Karma = 1000;

        // Outfit
        AddItem(new Robe(1367)); // A vibrant green robe symbolizing nature
        AddItem(new Sandals(2419)); // Colorful sandals
        AddItem(new FloppyHat(1436)); // A purple floppy hat
        AddItem(new JackPumpkin()); // Alchemist's tool, representing her profession

        VirtualArmor = 15;
    }

    public IantheTheBloomingAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Ianthe, a humble alchemist seeking the mysteries hidden in blooming flora. Do you happen to hold the rare SerpentCrest herb?");

        // Dialogue options
        greeting.AddOption("What do you do with the SerpentCrest herb?",
            p => true,
            p =>
            {
                DialogueModule explanationModule = new DialogueModule("Ah, the SerpentCrest is a unique herb, a key ingredient in the most ethereal of potions. I can offer you something rare in return, if you are willing to part with it.");

                explanationModule.AddOption("What kind of potions do you make?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule potionDetailsModule = new DialogueModule("Potions of transformation, whispers of forgotten gods, and elixirs that may take you beyond the veil of this world. Each creation is a dance between nature and the arcane.");

                        potionDetailsModule.AddOption("Transformation? Tell me more.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule transformationModule = new DialogueModule("Ah, transformations of body and soul. Some seek to become more than themselves, while others yearn to escape. The SerpentCrest herb channels ancient energy for those brave enough to change their fate.");
                                transformationModule.AddOption("Is it dangerous?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Indeed. There are those who have emerged stronger, reborn, and others... who lost themselves to the whispers. Power always comes at a cost, dear traveler.");
                                        dangerModule.AddOption("I think I understand. Let's continue.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, dangerModule));
                                    });
                                transformationModule.AddOption("I see... Let's talk about something else.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, transformationModule));
                            });

                        potionDetailsModule.AddOption("Whispers of forgotten gods?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule forgottenGodsModule = new DialogueModule("Yes... the SerpentCrest carries echoes of power from ancient beings, forgotten by most. A mere taste of these whispers can open your mind to the truths beyond, but they can also consume the weak-willed.");
                                forgottenGodsModule.AddOption("What kind of truths?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule truthsModule = new DialogueModule("Truths about the nature of existence, the fragility of reality, and the dark gods that still wander unseen. Knowledge that can uplift or drive one mad.");
                                        truthsModule.AddOption("I want to hear more.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule deeperTruthsModule = new DialogueModule("Very few have delved as deep as I have. The forgotten gods, their whispers entwine with our thoughts. Some have gone mad trying to understand them, others... became something more. Do you dare to listen further?");
                                                deeperTruthsModule.AddOption("I dare.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Ianthe leans closer, her voice dropping to a whisper. 'Then listen well, traveler, and prepare your soul.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                deeperTruthsModule.AddOption("No, I fear that is too much for me.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Ianthe nods solemnly, 'Perhaps it is wise to know one's limits.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, deeperTruthsModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, truthsModule));
                                    });
                                forgottenGodsModule.AddOption("That's unsettling. Let's move on.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, forgottenGodsModule));
                            });

                        potionDetailsModule.AddOption("Let's talk about something else.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, potionDetailsModule));
                    });

                explanationModule.AddOption("Can we trade something for it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a SerpentCrest herb, I would gladly trade you either a JackPumpkin or a vial of FairyOil. Additionally, I will provide you with a MaxxiaScroll as a token of my gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SerpentCrest for me?");
                                tradeModule.AddOption("Yes, I have a SerpentCrest.",
                                    plaa => HasSerpentCrest(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasSerpentCrest(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SerpentCrest herb.");
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

                p.SendGump(new DialogueGump(p, explanationModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Ianthe smiles warmly, her eyes twinkling like morning dew.");
            });

        return greeting;
    }

    private bool HasSerpentCrest(PlayerMobile player)
    {
        // Check the player's inventory for SerpentCrest
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SerpantCrest)) != null;
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
        // Remove the SerpentCrest and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item serpentCrest = player.Backpack.FindItemByType(typeof(SerpantCrest));
        if (serpentCrest != null)
        {
            serpentCrest.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for JackPumpkin and FairyOil
            rewardChoiceModule.AddOption("JackPumpkin", pl => true, pl =>
            {
                pl.AddToBackpack(new JackPumpkin());
                pl.SendMessage("You receive a JackPumpkin!");
            });

            rewardChoiceModule.AddOption("FairyOil", pl => true, pl =>
            {
                pl.AddToBackpack(new FairyOil());
                pl.SendMessage("You receive a vial of FairyOil!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SerpentCrest herb.");
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