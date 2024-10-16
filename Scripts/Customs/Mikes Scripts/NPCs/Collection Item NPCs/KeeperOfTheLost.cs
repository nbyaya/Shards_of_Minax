using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class KeeperOfTheLost : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public KeeperOfTheLost() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Niamh, Keeper of the Lost";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2125)); // A midnight blue robe
        AddItem(new Sandals(1109)); // Dark sandals
        AddItem(new Cloak(1157)); // A crimson cloak
        AddItem(new Circlet()); // A golden circlet
        AddItem(new StaffOfPower()); // Decorative item to suggest magical power

        VirtualArmor = 20;
    }

    public KeeperOfTheLost(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Niamh, Keeper of the Lost. Many secrets pass through my hands, and perhaps I could share something... if you have what I seek.");
        
        // Dialogue options
        greeting.AddOption("What do you seek?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I am in search of a Large Tome. If you have one, I can offer you a reward. You may choose between a Gargish Totem or an Inscription Talisman.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pla => CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a Large Tome for me?");
                        tradeModule.AddOption("Yes, I have a Large Tome.", 
                            plaa => HasLargeTome(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasLargeTome(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Return when you have a Large Tome. The secrets await...");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Patience, traveler. You may only trade once every 10 minutes.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pla.SendGump(new DialogueGump(pla, tradeModule));
                    });
                
                tradeIntroductionModule.AddOption("Why are you seeking a Large Tome?", 
                    pla => true, 
                    pla =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Ah, the tome contains secrets, whispers of an era long forgotten. The words within, they speak to the soul—especially a soul that has suffered as mine has. It is a map to the past, a tool for reclaiming lost moments, for rekindling lost loves.");
                        reasonModule.AddOption("It sounds like you have suffered greatly.", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule sufferingModule = new DialogueModule("Indeed, I have loved and lost more deeply than words can describe. My heart has been shattered more times than I can count, and each time, the shards have grown sharper, leaving wounds that never heal. But those moments of love, even fleeting, were worth the pain. I seek the tome to find a way back to those moments, even if only for an instant.");
                                sufferingModule.AddOption("Love is worth any price, even the pain.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        DialogueModule passionModule = new DialogueModule("You understand, then. The fire of passion, even when it burns you, is the only true light in this dark world. I have chased that light, and I will continue to chase it, even if it leads to my destruction.");
                                        passionModule.AddOption("What would you do if you found true love again?", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                DialogueModule loveModule = new DialogueModule("If I found it again... I would hold onto it with every ounce of strength left in me. I would fight for it, I would cherish it, and I would protect it from the ravages of time. But such love is elusive, like a dream that fades with the dawn. Perhaps that is why I keep searching.");
                                                loveModule.AddOption("Dreams can sometimes become reality.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Niamh smiles faintly, her eyes glistening with unshed tears. 'Perhaps, traveler. Perhaps.'");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                loveModule.AddOption("Sometimes, it is better to let go of dreams.", 
                                                    plaaaaa => true, 
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Niamh's expression darkens, and she nods slowly. 'You may be right. But I am not ready to give up just yet.'");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, loveModule));
                                            });
                                        passionModule.AddOption("It sounds like a dangerous path.", 
                                            plaaaa => true, 
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Niamh laughs, a sound both bitter and longing. 'The most beautiful paths are often the most perilous. But what is life without a little danger?' She looks at you, her eyes filled with both challenge and hope.");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, passionModule));
                                    });
                                sufferingModule.AddOption("Perhaps there is a way to heal your heart.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Niamh looks away, her eyes distant. 'Healing... perhaps. But scars are reminders of what we have lost, and I am not yet ready to let go of those memories.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, sufferingModule));
                            });
                        pla.SendGump(new DialogueGump(pla, reasonModule));
                    });

                tradeIntroductionModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("You seem troubled. What burdens you?", 
            p => true, 
            p =>
            {
                DialogueModule burdenModule = new DialogueModule("I carry the weight of memories, traveler. Memories of love that burned brightly but was extinguished by the cold winds of fate. I am haunted by those who left, by those who chose other paths, leaving me behind.");
                burdenModule.AddOption("Why do you continue to hold onto those memories?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule holdOnModule = new DialogueModule("Because they are all I have left. The echoes of laughter, the warmth of a touch, the whispered promises... they are the only remnants of a life that once meant everything to me. To let go would be to lose myself entirely.");
                        holdOnModule.AddOption("Perhaps it is time to create new memories.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Niamh looks at you with a mixture of hope and sorrow. 'New memories... perhaps one day. But until then, I will cling to what I have left.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        holdOnModule.AddOption("Memories can be both a blessing and a curse.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Niamh nods slowly, her expression weary. 'You understand, then. They are my greatest joy and my deepest sorrow. It is a delicate balance, one I must maintain.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, holdOnModule));
                    });
                burdenModule.AddOption("Do you regret loving so deeply?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule regretModule = new DialogueModule("No, I could never regret it. To love deeply, to give oneself entirely, is the most beautiful thing one can do. Even if it leads to pain, the moments of true connection are worth every tear shed.");
                        regretModule.AddOption("That is a beautiful sentiment.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Niamh smiles, her eyes softening. 'Thank you, traveler. It is rare to find someone who understands.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        regretModule.AddOption("It must be difficult to live with such pain.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Niamh sighs, her shoulders sagging slightly. 'It is. But I would rather feel the pain than feel nothing at all. It reminds me that I am still alive.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, regretModule));
                    });
                p.SendGump(new DialogueGump(p, burdenModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Niamh nods knowingly, her eyes shimmering with mystery.");
            });

        return greeting;
    }

    private bool HasLargeTome(PlayerMobile player)
    {
        // Check the player's inventory for LargeTome
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LargeTome)) != null;
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
        // Remove the LargeTome and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item largeTome = player.Backpack.FindItemByType(typeof(LargeTome));
        if (largeTome != null)
        {
            largeTome.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for GargishTotem and InscriptionTalisman
            rewardChoiceModule.AddOption("Gargish Totem", pl => true, pl => 
            {
                pl.AddToBackpack(new GargishTotem());
                pl.SendMessage("You receive a Gargish Totem!");
            });
            
            rewardChoiceModule.AddOption("Inscription Talisman", pl => true, pl =>
            {
                pl.AddToBackpack(new InscriptionTalisman());
                pl.SendMessage("You receive an Inscription Talisman!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a Large Tome.");
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