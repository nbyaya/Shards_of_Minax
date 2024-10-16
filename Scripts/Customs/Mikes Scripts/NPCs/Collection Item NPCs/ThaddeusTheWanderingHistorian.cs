using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ThaddeusTheWanderingHistorian : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaddeusTheWanderingHistorian() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thaddeus the Wandering Historian";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1157)); // A deep blue robe
        AddItem(new Sandals(54)); // Light brown sandals
        AddItem(new LongPants(2301)); // Dark gray pants
        AddItem(new WideBrimHat(1175)); // A distinguished deep blue hat
        AddItem(new GnarledStaff()); // A wooden staff, symbol of his travels

        VirtualArmor = 15;
    }

    public ThaddeusTheWanderingHistorian(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a fellow adventurer! I am Thaddeus, a historian wandering these lands in search of forgotten tales and relics. Do you happen to carry any artifacts of great value?");
        
        // Dialogue options
        greeting.AddOption("Tell me more about your quest.", 
            p => true, 
            p =>
            {
                DialogueModule questModule = new DialogueModule("I travel to gather knowledge, ancient relics, and secrets lost to time. Some call me obsessed, but I am merely devoted. My particular fascination lies with the darker aspects of history—the tales others fear to tell, the secrets powerful families have tried to bury.");
                
                questModule.AddOption("What kind of secrets?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Ah, the kinds of secrets that would make the powerful uneasy. Betrayals, forbidden practices, and lost tomes that detail unspeakable rituals. The kind of knowledge that can reshape one's understanding of history—and perhaps, one's fate.");
                        
                        secretsModule.AddOption("Why pursue such dangerous knowledge?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule obsessionModule = new DialogueModule("I believe that truth should never be hidden, no matter how dark. Power corrupts, and the truth is often its first casualty. My writings have angered powerful families who would prefer these truths remain forgotten. Yet, I press on—there is a beauty in the morbid and the macabre, a lesson in every forgotten tragedy.");
                                
                                obsessionModule.AddOption("A beauty in the morbid?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule beautyModule = new DialogueModule("Indeed. Death, decay, and betrayal are all facets of the human experience. To understand them is to understand ourselves. Consider, for example, the tale of the House of Armitage—a noble family that fell to ruin due to their secret obsession with necromancy. They thought they could cheat death, but instead they invited it to their doorstep.");
                                        
                                        beautyModule.AddOption("Tell me more about the House of Armitage.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule armitageModule = new DialogueModule("The Armitages were once a respected family, revered for their wealth and influence. But their patriarch, Lord Armitage, grew obsessed with the idea of eternal life. He found an ancient tome, the 'Codex Mortis', which promised him the secrets of necromancy. The family delved deeper into forbidden practices, and soon the line was cursed. Madness took hold, and one by one, they turned on each other. Now, only their haunted manor remains, a monument to their folly.");
                                                
                                                armitageModule.AddOption("What happened to the Codex Mortis?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule codexModule = new DialogueModule("Ah, the Codex Mortis... a fascinating and dangerous artifact. It is said to hold the power to commune with the dead, but at a terrible cost. Some say it was lost when the last of the Armitages fell, others claim it was stolen by a rival seeking its dark power. I have spent years trying to track it down, but it remains elusive—perhaps for the best.");
                                                        
                                                        codexModule.AddOption("I think I have something you might want.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule tradeIntroductionModule = new DialogueModule("If you hold the LexxVase, I would gladly exchange it for one of my prized possessions. You may choose between an Ornate Harp or a Fletching Talisman. Additionally, I shall bestow upon you a MaxxiaScroll for your aid.");
                                                                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                                    plaaaaaa => CanTradeWithPlayer(plaaaaaa), 
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a LexxVase for me?");
                                                                        tradeModule.AddOption("Yes, I have a LexxVase.", 
                                                                            plaaaaaaa => HasLexxVase(plaaaaaaa) && CanTradeWithPlayer(plaaaaaaa), 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                CompleteTrade(plaaaaaaa);
                                                                            });
                                                                        tradeModule.AddOption("No, I don't have one right now.", 
                                                                            plaaaaaaa => !HasLexxVase(plaaaaaaa), 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("Come back when you have a LexxVase.");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                            plaaaaaaa => !CanTradeWithPlayer(plaaaaaaa), 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, tradeModule));
                                                                    });
                                                                tradeIntroductionModule.AddOption("Maybe another time.", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, tradeIntroductionModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, codexModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, armitageModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, beautyModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, obsessionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, secretsModule));
                    });

                // Trade option after story
                questModule.AddOption("I think I have something you might want.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If you hold the LexxVase, I would gladly exchange it for one of my prized possessions. You may choose between an Ornate Harp or a Fletching Talisman. Additionally, I shall bestow upon you a MaxxiaScroll for your aid.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a LexxVase for me?");
                                tradeModule.AddOption("Yes, I have a LexxVase.", 
                                    plaa => HasLexxVase(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasLexxVase(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a LexxVase.");
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

                p.SendGump(new DialogueGump(p, questModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thaddeus nods thoughtfully and returns to his notes.");
            });

        return greeting;
    }

    private bool HasLexxVase(PlayerMobile player)
    {
        // Check the player's inventory for LexxVase
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LexxVase)) != null;
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
        // Remove the LexxVase and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item lexxVase = player.Backpack.FindItemByType(typeof(LexxVase));
        if (lexxVase != null)
        {
            lexxVase.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for Ornate Harp and Fletching Talisman
            rewardChoiceModule.AddOption("Ornate Harp", pl => true, pl => 
            {
                pl.AddToBackpack(new OrnateHarp());
                pl.SendMessage("You receive an Ornate Harp!");
            });
            
            rewardChoiceModule.AddOption("Fletching Talisman", pl => true, pl =>
            {
                pl.AddToBackpack(new FletchingTalisman());
                pl.SendMessage("You receive a Fletching Talisman!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a LexxVase.");
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