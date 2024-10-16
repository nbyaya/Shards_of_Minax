using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class LazloTheExplorer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LazloTheExplorer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lazlo the Explorer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(1175)); // A navy blue fancy shirt
        AddItem(new LongPants(2124)); // Dark brown pants
        AddItem(new ThighBoots(1109)); // Black boots
        AddItem(new TricorneHat(1175)); // A navy blue tricorne hat
        AddItem(new Backpack()); // Lazlo carries a backpack

        VirtualArmor = 15;
    }

    public LazloTheExplorer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy there, traveler! The seas are full of mysteries, and I, Lazlo the Explorer, have seen many wonders. Do you happen to have something exotic to trade?");

        // Start with dialogue about his travels
        greeting.AddOption("Tell me about your travels.", 
            p => true, 
            p =>
            {
                DialogueModule travelsModule = new DialogueModule("I've sailed across treacherous waters, seen the glowing shores of the Emerald Isles, and even glimpsed the fabled Leviathan. Each journey leaves me thirsting for more adventure!");
                travelsModule.AddOption("What's the most dangerous place you've visited?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule dangerModule = new DialogueModule("The most dangerous? Ah, that would be the Wailing Abyss. The waves there seem alive, and many a ship has vanished beneath them. But with courage and a bit of luck, I made it through. Those were harrowing days indeed.");
                        dangerModule.AddOption("How did you survive such a dangerous place?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule survivalModule = new DialogueModule("Survival was a matter of wit and resilience. I had to rely on every skill I possessed. I rationed my supplies carefully, used the stars to navigate, and kept my emotions in check. The Abyss tests both body and spirit.");
                                survivalModule.AddOption("Did you encounter any strange creatures?", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        DialogueModule creaturesModule = new DialogueModule("Oh yes, the Abyss is home to many strange beings. There were the Abyssal Sirens, whose haunting songs could drive a man mad, and the Shadow Eels, lurking beneath the waves, waiting for any sign of weakness. But the most fearsome of all was the Leviathan itself.");
                                        creaturesModule.AddOption("What was the Leviathan like?", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                DialogueModule leviathanModule = new DialogueModule("The Leviathan is a creature of immense size and power. Its eyes glowed with an ancient intelligence, and its roar could shake the very sea. I managed to evade it by hiding within a cluster of sharp rocks, waiting for it to lose interest. It was a battle of patience.");
                                                leviathanModule.AddOption("Incredible. How did you manage to stay calm?", 
                                                    plaabcd => true, 
                                                    plaabcd =>
                                                    {
                                                        DialogueModule calmModule = new DialogueModule("Calmness is a skill I've honed over years of dangerous pursuits. Fear clouds judgment. I learned to compartmentalize my emotions, focus solely on the task at hand, and rely on my training. It is the only way to survive encounters like that.");
                                                        calmModule.AddOption("Thank you for sharing your story.", 
                                                            plaabcde => true, 
                                                            plaabcde =>
                                                            {
                                                                plaabcde.SendGump(new DialogueGump(plaabcde, CreateGreetingModule(plaabcde)));
                                                            });
                                                        plaabcd.SendGump(new DialogueGump(plaabcd, calmModule));
                                                    });
                                                plaabc.SendGump(new DialogueGump(plaabc, leviathanModule));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, creaturesModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, survivalModule));
                            });
                        dangerModule.AddOption("Fascinating! Thank you.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, dangerModule));
                    });
                travelsModule.AddOption("Tell me about your cybernetic enhancements.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule cyberneticModule = new DialogueModule("Ah, you've noticed. Yes, I was once a bounty hunter, tracking the most elusive of targets. The cybernetic enhancements you see were a necessity, not a luxury. Enhanced vision, reinforced musculature, and an internal navigation system—all designed to make me efficient, ruthless, and unyielding.");
                        cyberneticModule.AddOption("Why did you become a bounty hunter?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule bountyHunterModule = new DialogueModule("I became a bounty hunter out of necessity. Life was hard, and opportunities were scarce. I needed a way to survive, and I was good at tracking people. The enhancements made me one of the best, but it came at a cost—a piece of my humanity, perhaps.");
                                bountyHunterModule.AddOption("Do you regret it?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? No. Regret is for those who have the luxury of choice. My path was forged by circumstance. I became efficient, stoic, and ruthless because it was necessary for survival. Regret would only slow me down, and in my line of work, hesitation meant death.");
                                        regretModule.AddOption("That's a harsh reality.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule harshRealityModule = new DialogueModule("Indeed. The world is not a kind place. It rewards strength and punishes weakness. I chose to adapt, to augment myself to meet the challenges head-on. Some may see me as less human, but I see myself as more capable.");
                                                harshRealityModule.AddOption("Thank you for your honesty.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, harshRealityModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, regretModule));
                                    });
                                bountyHunterModule.AddOption("You must have been incredibly skilled.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule skillModule = new DialogueModule("Skill was everything. I learned to track my targets without rest, to predict their movements, to know their fears. The enhancements helped, but it was my determination and training that made me effective. I never failed to capture a target.");
                                        skillModule.AddOption("That's impressive. Thank you for sharing.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, skillModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, bountyHunterModule));
                            });
                        cyberneticModule.AddOption("What are the downsides to these enhancements?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule downsideModule = new DialogueModule("The downsides? There are always downsides. My body is not entirely my own anymore. Maintenance is constant, and the risk of malfunction is ever-present. Sometimes I wonder if I'm more machine than man. The emotional cost is significant too—connections become harder, people see you as something other than human.");
                                downsideModule.AddOption("That must be difficult.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule difficultModule = new DialogueModule("It is. But difficulty is something I've learned to accept. I chose this path for the power it gave me, for the ability to control my fate. Emotions are secondary to survival. It's the way of the world.");
                                        difficultModule.AddOption("I see. Thank you for your perspective.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, difficultModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, downsideModule));
                            });
                        pl.SendGump(new DialogueGump(pl, cyberneticModule));
                    });
                travelsModule.AddOption("I'd rather hear about your treasures.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule treasuresModule = new DialogueModule("Ah, treasures! I've found many peculiar items, but none so curious as the ExoticShipInABottle. It's a rarity, and I've been searching for more ever since.");
                        treasuresModule.AddOption("Do you want another ExoticShipInABottle?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I do! If you have one, I'll gladly trade you a HildebrandtTapestry for it, along with a MaxxiaScroll. But I must warn you, I can only offer this trade once every 10 minutes.");
                                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                    plaa => CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        DialogueModule tradeModule = new DialogueModule("Do you have an ExoticShipInABottle for me?");
                                        tradeModule.AddOption("Yes, I have an ExoticShipInABottle.", 
                                            plaaa => HasExoticShipInABottle(plaaa) && CanTradeWithPlayer(plaaa), 
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });
                                        tradeModule.AddOption("No, I don't have one right now.", 
                                            plaaa => !HasExoticShipInABottle(plaaa), 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Come back when you have an ExoticShipInABottle.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                                            plaaa => !CanTradeWithPlayer(plaaa), 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tradeModule));
                                    });
                                tradeIntroductionModule.AddOption("Perhaps another time.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, treasuresModule));
                    });
                p.SendGump(new DialogueGump(p, travelsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Lazlo waves you off with a smile, ready for his next adventure.");
            });

        return greeting;
    }

    private bool HasExoticShipInABottle(PlayerMobile player)
    {
        // Check the player's inventory for ExoticShipInABottle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ExoticShipInABottle)) != null;
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
        // Remove the ExoticShipInABottle and give the HildebrandtTapestry and MaxxiaScroll, then set the cooldown timer
        Item exoticShip = player.Backpack.FindItemByType(typeof(ExoticShipInABottle));
        if (exoticShip != null)
        {
            exoticShip.Delete();
            player.AddToBackpack(new HildebrandtTapestry());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ExoticShipInABottle and receive a HildebrandtTapestry and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have an ExoticShipInABottle.");
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