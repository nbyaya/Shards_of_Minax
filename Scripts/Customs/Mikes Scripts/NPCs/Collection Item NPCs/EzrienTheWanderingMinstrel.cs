using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EzrienTheWanderingMinstrel : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EzrienTheWanderingMinstrel() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ezrien the Wandering Minstrel";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FeatheredHat(1157)); // A bright green feathered hat
        AddItem(new Doublet(1444)); // A colorful doublet
        AddItem(new LongPants(1109)); // Dark blue pants
        AddItem(new Sandals(1358)); // Bright red sandals
        AddItem(new Lute()); // Holds a lute for his minstrel theme

        VirtualArmor = 15;
    }

    public EzrienTheWanderingMinstrel(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Ezrien, a wandering minstrel, but I am also something of an obsessed gardener. Have you heard the melody of the MasterTrumpet? It is said to echo across worlds!");

        // Dialogue options
        greeting.AddOption("Tell me more about the MasterTrumpet.", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("The MasterTrumpet is an instrument of legend. Its notes bring joy to the downtrodden and strength to the weary. If you happen to possess one, I could offer you a fair trade.");

                // Add gardening obsession
                storyModule.AddOption("Why does a minstrel care about gardening?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule gardeningModule = new DialogueModule("Ah, you see, music and nature share a profound connection. Just as melodies grow and evolve, so do plants. My garden is my greatest masterpiece, and it harbors more than mere plants. I seek to grow a perfect flower—one that grants immortality.");

                        gardeningModule.AddOption("Immortality? That sounds impossible!", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule immortalityModule = new DialogueModule("Many think it impossible, but I have spent years nurturing rare plants, cultivating the finest soil, and learning the secrets of nature. Patience and dedication are key. If I can find the right harmony between the soil, the sun, and my care, I believe I can make it bloom.");

                                immortalityModule.AddOption("What makes this flower so special?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule specialFlowerModule = new DialogueModule("The flower I seek is said to absorb the essence of life itself. It blooms only once in a hundred years, under the perfect conditions. Some say it has the power to grant eternal youth to anyone who beholds its beauty. It is my obsession, my passion, my life's work.");

                                        specialFlowerModule.AddOption("Isn't it dangerous to pursue such power?", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                DialogueModule dangerModule = new DialogueModule("Ah, you speak wisely, traveler. Indeed, the pursuit of immortality is fraught with dangers—temptation, greed, and madness. But I believe that if one approaches it with humility and reverence, it can be a gift rather than a curse. Besides, nurturing this garden brings me peace.");

                                                dangerModule.AddOption("What else grows in your garden?", 
                                                    plllll => true, 
                                                    plllll =>
                                                    {
                                                        DialogueModule gardenDetailsModule = new DialogueModule("My garden is filled with rare herbs, vibrant flowers, and curious fungi. Some are medicinal, while others are poisonous. Each plant has its own story, its own needs. I spend hours each day tending to them, singing songs to help them grow. Would you care to see it someday?");

                                                        gardenDetailsModule.AddOption("I would love to see it!", 
                                                            pllllll => true, 
                                                            pllllll =>
                                                            {
                                                                pllllll.SendMessage("Ezrien smiles warmly. 'Perhaps one day, when the flower blooms, I will invite you. For now, it remains my secret sanctuary.'");
                                                            });

                                                        gardenDetailsModule.AddOption("I'm not sure. It sounds dangerous.", 
                                                            pllllll => true, 
                                                            pllllll =>
                                                            {
                                                                pllllll.SendMessage("Ezrien nods knowingly. 'A wise choice, traveler. The garden holds many secrets, and not all of them are safe.'");
                                                            });

                                                        plllll.SendGump(new DialogueGump(plllll, gardenDetailsModule));
                                                    });

                                                pllll.SendGump(new DialogueGump(pllll, dangerModule));
                                            });

                                        specialFlowerModule.AddOption("You must be very patient.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Ezrien smiles. 'Patience is the gardener's greatest virtue, my friend. Just as a song takes time to compose, a perfect garden takes a lifetime to grow.'");
                                            });

                                        plll.SendGump(new DialogueGump(plll, specialFlowerModule));
                                    });

                                pll.SendGump(new DialogueGump(pll, immortalityModule));
                            });

                        pl.SendGump(new DialogueGump(pl, gardeningModule));
                    });

                // Trade option after story
                storyModule.AddOption("What would you offer in exchange?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I would gladly exchange it for either a TinyWizard or a WelcomeMat. And of course, a MaxxiaScroll as a token of my appreciation.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MasterTrumpet for me?");
                                tradeModule.AddOption("Yes, I have a MasterTrumpet.", 
                                    plaa => HasMasterTrumpet(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasMasterTrumpet(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a MasterTrumpet, my friend.");
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

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Ezrien nods and strums a cheerful tune as you part ways.");
            });

        return greeting;
    }

    private bool HasMasterTrumpet(PlayerMobile player)
    {
        // Check the player's inventory for MasterTrumpet
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MasterTrumpet)) != null;
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
        // Remove the MasterTrumpet and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item masterTrumpet = player.Backpack.FindItemByType(typeof(MasterTrumpet));
        if (masterTrumpet != null)
        {
            masterTrumpet.Delete();
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for TinyWizard and WelcomeMat
            rewardChoiceModule.AddOption("TinyWizard", pl => true, pl => 
            {
                pl.AddToBackpack(new TinyWizard());
                pl.SendMessage("You receive a TinyWizard!");
            });

            rewardChoiceModule.AddOption("WelcomeMat", pl => true, pl =>
            {
                pl.AddToBackpack(new WelcomeMat());
                pl.SendMessage("You receive a WelcomeMat!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a MasterTrumpet.");
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