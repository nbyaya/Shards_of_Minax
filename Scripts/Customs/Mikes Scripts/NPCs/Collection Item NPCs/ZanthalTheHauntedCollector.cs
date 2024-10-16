using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZanthalTheHauntedCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZanthalTheHauntedCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zanthal the Haunted Collector";
        Body = 0x190; // Human male body
        Hue = 33770; // Pale, almost ghostly skin tone

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 100;
        Karma = -100;

        // Outfit
        AddItem(new Robe(1109)); // Dark, tattered robe
        AddItem(new Sandals(1175)); // Dark blue sandals
        AddItem(new Cloak(1150)); // Shadowy black cloak
        AddItem(new QuarterStaff()); // Worn wooden staff

        VirtualArmor = 15;
    }

    public ZanthalTheHauntedCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler... I am Zanthal, collector of haunted artifacts. Have you perhaps come across a 'SpookyGhost' in your travels? Such items hold a peculiar power, and I may have something to offer in exchange.");
        
        // Dialogue options
        greeting.AddOption("Tell me more about your collection.", 
            p => true, 
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("My collection consists of items touched by spirits, cursed objects, and artifacts that hum with an unseen energy. The SpookyGhost is particularly valuable to me." + 
                " My fascination with these artifacts goes beyond mere collecting. I believe that each holds a fragment of the soul that once inhabited them." + 
                " I spend my days carefully preserving these items, much like my work with taxidermy, capturing the essence of creatures, their spirits trapped forever in a grotesque display." + 
                " Yes, I am also a taxidermist of sorts, but my craft is not merely about stuffing lifeless bodies. No, it is about preserving the spirit, the whispers of what once was.");
                
                collectionModule.AddOption("Taxidermy? Tell me more about that.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule taxidermyModule = new DialogueModule("Ah, yes, taxidermy... It's a practice that requires patience, precision, and a touch of madness." + 
                        " I work alone, as others do not understand the beauty of what I create. Each creature I preserve is frozen in a perfect moment, a reflection of its true nature." + 
                        " You see, I believe that by capturing their form just right, I can trap a fragment of their soul. The townsfolk think me mad, or worse, but they don't understand. They fear what they cannot comprehend." + 
                        " My workshop is filled with creatures in poses that would make most shiver, but I find solace in their eyes, forever watching, forever there.");

                        taxidermyModule.AddOption("That sounds... unsettling. Why do you do it?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule obsessionModule = new DialogueModule("Unsettling, perhaps. But why do I do it? Because it is the only way I can fight against the inevitability of decay." + 
                                " Everything fades, everything dies, but I can keep a part of it here, with me, forever." + 
                                " There is something deeply obsessive about my work, I admit. It is my way of keeping death at bay, of holding onto something that would otherwise slip through my fingers." + 
                                " Each stuffed creature, each artifact, tells a story that the living often forget. It is my duty to remember for them.");

                                obsessionModule.AddOption("Do you ever feel lonely, doing all this alone?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule lonelinessModule = new DialogueModule("Lonely? Perhaps. But solitude is the price I pay for my craft. The whispers of the dead and the spirits of these creatures are my companions." + 
                                        " The townsfolk avoid me, yes, and they whisper about my dark arts, but I find comfort in my collection. The shadows that dance in my workshop keep me company." + 
                                        " There are times when I wonder what it would be like to be among others, to feel the warmth of a friend. But that life is not for me. My purpose is here, among the relics of what once was.");

                                        lonelinessModule.AddOption("It sounds tragic, in a way.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule tragicModule = new DialogueModule("Tragic? Perhaps. But it is the path I have chosen, and I accept it. There is beauty in the tragic, don't you think?" + 
                                                " The fleeting nature of life, the inevitability of death... these are truths that many turn away from. But I embrace them." + 
                                                " My work, my collection, they are a testament to what has been lost, a reminder that even in death, something can endure.");

                                                tragicModule.AddOption("I think I understand now. Perhaps I have a SpookyGhost for you.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a SpookyGhost, I will gladly offer you a choice: a ScentedCandle or a SnowSculpture. Both have unique qualities that might interest you.");
                                                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                            plaaaa => CanTradeWithPlayer(plaaaa), 
                                                            plaaaa =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Do you have a SpookyGhost for me?");
                                                                tradeModule.AddOption("Yes, I have a SpookyGhost.", 
                                                                    plaaaaa => HasSpookyGhost(plaaaaa) && CanTradeWithPlayer(plaaaaa), 
                                                                    plaaaaa =>
                                                                    {
                                                                        CompleteTrade(plaaaaa);
                                                                    });
                                                                tradeModule.AddOption("No, I don't have one right now.", 
                                                                    plaaaaa => !HasSpookyGhost(plaaaaa), 
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendMessage("Come back when you have a SpookyGhost.");
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
                                                        tradeIntroductionModule.AddOption("Maybe another time.", 
                                                            plaaaa => true, 
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, tradeIntroductionModule));
                                                    });

                                                tragicModule.AddOption("Goodbye, Zanthal. May you find peace in your work.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("Zanthal nods slightly, his eyes still fixed on something beyond the mortal realm.");
                                                    });

                                                plaab.SendGump(new DialogueGump(plaab, tragicModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, lonelinessModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, obsessionModule));
                            });

                        pl.SendGump(new DialogueGump(pl, taxidermyModule));
                    });

                collectionModule.AddOption("Do you wish to trade for a SpookyGhost?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you have a SpookyGhost, I will gladly offer you a choice: a ScentedCandle or a SnowSculpture. Both have unique qualities that might interest you.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SpookyGhost for me?");
                                tradeModule.AddOption("Yes, I have a SpookyGhost.", 
                                    plaa => HasSpookyGhost(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasSpookyGhost(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SpookyGhost.");
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

                p.SendGump(new DialogueGump(p, collectionModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zanthal nods slightly, his eyes still fixed on something beyond the mortal realm.");
            });

        return greeting;
    }

    private bool HasSpookyGhost(PlayerMobile player)
    {
        // Check the player's inventory for SpookyGhost
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SpookyGhost)) != null;
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
        // Remove the SpookyGhost and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item spookyGhost = player.Backpack.FindItemByType(typeof(SpookyGhost));
        if (spookyGhost != null)
        {
            spookyGhost.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for ScentedCandle and SnowSculpture
            rewardChoiceModule.AddOption("ScentedCandle", pl => true, pl => 
            {
                pl.AddToBackpack(new ScentedCandle());
                pl.SendMessage("You receive a ScentedCandle, its scent strangely calming.");
            });
            
            rewardChoiceModule.AddOption("SnowSculpture", pl => true, pl =>
            {
                pl.AddToBackpack(new SnowSculpture());
                pl.SendMessage("You receive a SnowSculpture, its form as delicate as winter's touch.");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SpookyGhost.");
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