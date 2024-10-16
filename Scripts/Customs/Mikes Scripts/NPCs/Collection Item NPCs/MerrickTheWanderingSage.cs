using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class MerrickTheWanderingSage : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MerrickTheWanderingSage() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Merrick the Wandering Sage";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 1000;

        // Outfit
        AddItem(new Robe(1325)); // A deep blue robe
        AddItem(new Sandals(2116)); // Simple sandals
        AddItem(new Cloak(1150)); // A mystical green cloak
        AddItem(new WizardsHat(1153)); // A matching green wizard's hat
        AddItem(new QuarterStaff()); // Carries a quarter staff

        VirtualArmor = 15;
    }

    public MerrickTheWanderingSage(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Merrick, a sage wandering these lands in search of ancient knowledge. Tell me, do you possess an artifact of lore?");
        
        // Dialogue options
        greeting.AddOption("What kind of artifact do you seek?", 
            p => true, 
            p =>
            {
                DialogueModule artifactModule = new DialogueModule("I am searching for StrappedBooks, a collection of wisdom lost to time. If you happen to possess one, I could offer you something special in exchange.");
                
                // Nested options with more detail and manipulation
                artifactModule.AddOption("Why are these StrappedBooks so important?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule importanceModule = new DialogueModule("Ah, the knowledge within these books is priceless. They contain secrets that could help us commune with forces beyond our understanding—perhaps even the spirits of those who have passed. Imagine the chance to speak to a lost loved one again.");

                        // Manipulative hint
                        importanceModule.AddOption("Are you saying these books let people speak with the dead?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule speakWithDeadModule = new DialogueModule("Indeed, traveler. But only those who understand the hidden paths can unlock that power. I sense you may have a deep connection to the past, perhaps someone you wish to speak to again?");

                                speakWithDeadModule.AddOption("Yes, I do... Can you help me?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("I can help, but such power comes at a cost. The StrappedBooks are a key, but my guidance will require trust and... an exchange. You bring me the StrappedBooks, and I will do what I can. Perhaps, there is someone you still wish to see, someone waiting for closure?");

                                        // More manipulative and deceitful offers
                                        helpModule.AddOption("What kind of exchange are we talking about?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule exchangeModule = new DialogueModule("Oh, nothing too drastic. Just a token of your commitment. Bring me the StrappedBooks and I will open the door to the unknown for you. Trust me, traveler, the rewards will be worth the sacrifice. After all, what wouldn't you give for a chance to see a loved one again?");

                                                exchangeModule.AddOption("I will bring the StrappedBooks.", 
                                                    plaaaa => HasStrappedBooks(plaaaa) && CanTradeWithPlayer(plaaaa), 
                                                    plaaaa =>
                                                    {
                                                        CompleteTrade(plaaaa);
                                                    });

                                                exchangeModule.AddOption("I don't have the books yet, but I'll be back.", 
                                                    plaaaa => !HasStrappedBooks(plaaaa), 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Then return when you are ready. I will be waiting.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, exchangeModule));
                                            });

                                        helpModule.AddOption("This sounds suspicious. I don't trust you.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Ah, such distrust. You wound me, traveler. I offer only a chance at peace, but not everyone is ready for it. Come back if you change your mind.");
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, helpModule));
                                    });

                                speakWithDeadModule.AddOption("No, I don't believe in such things.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Many do not, but remember this, traveler: the unseen world often reveals itself to those who least expect it. Perhaps one day, you will see it too.");
                                    });

                                pla.SendGump(new DialogueGump(pla, speakWithDeadModule));
                            });

                        importanceModule.AddOption("What will you do with the StrappedBooks?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule purposeModule = new DialogueModule("The knowledge I seek is for a higher purpose, traveler. To bridge the gap between our world and the next, to help others find peace... and, of course, for my own studies. Such wisdom should not be locked away. Perhaps you and I could share in its benefits?");

                                purposeModule.AddOption("I suppose that makes sense. I'll see if I can find the books.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Good. I look forward to our next meeting. Remember, the dead are closer than you think.");
                                    });

                                purposeModule.AddOption("You seem to have your own reasons. I'm not convinced.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ah, skepticism is wise in these lands. But perhaps in time, you will see that my intentions are pure—purely driven by a desire for knowledge, and maybe a little power.");
                                    });

                                pla.SendGump(new DialogueGump(pla, purposeModule));
                            });

                        pl.SendGump(new DialogueGump(pl, importanceModule));
                    });

                // Trade option after story
                artifactModule.AddOption("I have StrappedBooks. Can we trade?", 
                    pl => HasStrappedBooks(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                artifactModule.AddOption("I don't have such an item.", 
                    pl => !HasStrappedBooks(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back if you happen to find StrappedBooks. I will be here, waiting.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                artifactModule.AddOption("I have traded recently; I'll return later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, artifactModule));
            });

        greeting.AddOption("Farewell, sage.", 
            p => true, 
            p =>
            {
                p.SendMessage("Merrick nods knowingly, as if sensing your future paths.");
            });

        return greeting;
    }

    private bool HasStrappedBooks(PlayerMobile player)
    {
        // Check the player's inventory for StrappedBooks
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(StrappedBooks)) != null;
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
        // Remove the StrappedBooks and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item strappedBooks = player.Backpack.FindItemByType(typeof(StrappedBooks));
        if (strappedBooks != null)
        {
            strappedBooks.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for CarpentryTalisman and CharcuterieBoard
            rewardChoiceModule.AddOption("CarpentryTalisman", pl => true, pl => 
            {
                pl.AddToBackpack(new CarpentryTalisman());
                pl.SendMessage("You receive a CarpentryTalisman!");
            });

            rewardChoiceModule.AddOption("CharcuterieBoard", pl => true, pl =>
            {
                pl.AddToBackpack(new CharcuterieBoard());
                pl.SendMessage("You receive a CharcuterieBoard!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have StrappedBooks.");
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