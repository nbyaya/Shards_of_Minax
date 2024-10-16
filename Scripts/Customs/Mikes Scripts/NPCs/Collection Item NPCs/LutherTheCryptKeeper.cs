using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class LutherTheCryptKeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LutherTheCryptKeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Luther the Crypt Keeper";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 100;
        Karma = -100;

        // Outfit
        AddItem(new Robe(1109)); // Dark robe
        AddItem(new Sandals(1)); // Black sandals
        AddItem(new WizardsHat(1109)); // Dark wizard's hat
        AddItem(new SkullCap(0)); // Skullcap for added effect
        AddItem(new GnarledStaff()); // Staff for mystery

        VirtualArmor = 15;
    }

    public LutherTheCryptKeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler in search of dark secrets, perhaps? I am Luther, keeper of forgotten crypts. Do you dare meddle with the artifacts of the past?");
        
        // Dialogue options
        greeting.AddOption("Tell me about the crypts.", 
            p => true, 
            p =>
            {
                DialogueModule cryptsModule = new DialogueModule("The crypts hold the whispers of ancient souls, and relics that most fear to touch. But for the brave, there are rewards that surpass mortal wealth. I have a particular interest in one such item...");
                
                // Nested story options
                cryptsModule.AddOption("What makes these relics so special?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule relicsModule = new DialogueModule("These relics, my friend, are not just trinkets. They contain the memories, the very essence, of the ancient ones. My master, a powerful necromancer, sought these artifacts to extend his influence beyond the grave. I was merely his apprentice then, overshadowed, often left to do the menial work. But I learned much... more than he intended.");

                        relicsModule.AddOption("What happened to your master?", 
                            pl2 => true, 
                            pl2 =>
                            {
                                DialogueModule masterModule = new DialogueModule("My master, Vexnar the Undying, was feared across the lands. But he was blinded by his own arrogance. He treated me as nothing more than a tool, a forgotten apprentice. In his quest for immortality, he neglected the dangers of the crypts. One day, the spirits rose against him, enraged by his desecration. He was consumed by the very power he sought to control.");

                                masterModule.AddOption("And you survived?", 
                                    pl3 => true, 
                                    pl3 =>
                                    {
                                        DialogueModule survivedModule = new DialogueModule("Indeed, I survived. I was loyal, hardworking, and always in the shadows, unseen. The spirits spared me, perhaps seeing the truth of my servitude. Since that day, I have dedicated my life to maintaining the crypts, ensuring that none repeat the mistakes of my master. But there is still one task I must complete... one secret that must be uncovered.");

                                        survivedModule.AddOption("What secret do you seek?", 
                                            pl4 => true, 
                                            pl4 =>
                                            {
                                                DialogueModule secretModule = new DialogueModule("My master had a journal, filled with his darkest secrets. It contains knowledge that could either protect the crypts or destroy them entirely. I have searched for it for years, but it remains hidden. I fear it may be in the hands of someone who does not understand its power.");

                                                secretModule.AddOption("How can I help?", 
                                                    pl5 => true, 
                                                    pl5 =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("If you come across a journal bound in black leather, with a serpent emblem, bring it to me. I must ensure it is kept safe. In return, I will share with you some of the knowledge it holds... but only if you prove yourself worthy.");

                                                        helpModule.AddOption("I will keep an eye out.", 
                                                            pl6 => true, 
                                                            pl6 =>
                                                            {
                                                                pl6.SendMessage("Luther nods solemnly. 'I knew I could trust you. Be careful, traveler. The crypts are unforgiving.'");
                                                            });

                                                        helpModule.AddOption("This sounds dangerous. I need more information.", 
                                                            pl6 => true, 
                                                            pl6 =>
                                                            {
                                                                DialogueModule moreInfoModule = new DialogueModule("Dangerous, yes. But knowledge always comes with a price. The journal is protected by dark magic, and those who seek it may find themselves facing trials beyond imagination. If you are not ready, there is no shame in walking away.");

                                                                moreInfoModule.AddOption("I will consider it.", 
                                                                    pl7 => true, 
                                                                    pl7 =>
                                                                    {
                                                                        pl7.SendMessage("Luther nods. 'Consider wisely, traveler. The choice is yours, but once the path is taken, there is no turning back.'");
                                                                    });

                                                                pl6.SendGump(new DialogueGump(pl6, moreInfoModule));
                                                            });

                                                        pl5.SendGump(new DialogueGump(pl5, helpModule));
                                                    });

                                                secretModule.AddOption("This is not my concern.", 
                                                    pl5 => true, 
                                                    pl5 =>
                                                    {
                                                        pl5.SendMessage("Luther's expression hardens. 'Very well. Not all are meant to meddle with such matters.'");
                                                    });

                                                pl4.SendGump(new DialogueGump(pl4, secretModule));
                                            });

                                        pl3.SendGump(new DialogueGump(pl3, survivedModule));
                                    });

                                pl2.SendGump(new DialogueGump(pl2, masterModule));
                            });

                        pl.SendGump(new DialogueGump(pl, relicsModule));
                    });

                // Trade option after story
                cryptsModule.AddOption("What item do you seek?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you possess a SkullRing, I can offer you a choice of treasures. I have an old BrandingIron, or perhaps you would prefer some OldBones? I assure you, each has its... peculiar value.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SkullRing for me?");
                                tradeModule.AddOption("Yes, I have a SkullRing.", 
                                    plaa => HasSkullRing(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasSkullRing(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Return when you have a SkullRing.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll return later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("You may only trade once every 10 minutes. Return when the time is right.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeModule));
                            });
                        tradeIntroductionModule.AddOption("Perhaps another time.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, cryptsModule));
            });

        greeting.AddOption("Farewell.", 
            p => true, 
            p =>
            {
                p.SendMessage("Luther nods slowly, his gaze following you as you depart.");
            });

        return greeting;
    }

    private bool HasSkullRing(PlayerMobile player)
    {
        // Check the player's inventory for SkullRing
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SkullRing)) != null;
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
        // Remove the SkullRing and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item skullRing = player.Backpack.FindItemByType(typeof(SkullRing));
        if (skullRing != null)
        {
            skullRing.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for BrandingIron and OldBones
            rewardChoiceModule.AddOption("BrandingIron", pl => true, pl => 
            {
                pl.AddToBackpack(new BrandingIron());
                pl.SendMessage("You receive a BrandingIron!");
            });
            
            rewardChoiceModule.AddOption("OldBones", pl => true, pl =>
            {
                pl.AddToBackpack(new OldBones());
                pl.SendMessage("You receive some OldBones!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SkullRing.");
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