using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class LorithTheVeilSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LorithTheVeilSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lorith the Veil Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 100;
        Karma = 100;

        // Outfit - a mysterious look
        AddItem(new HoodedShroudOfShadows(1150)); // A dark, mysterious hooded cloak
        AddItem(new Sandals(1109)); // Simple black sandals
        AddItem(new Robe(2413)); // Dark robe with unique hue
        AddItem(new BodySash(137)); // Decorative sash
        AddItem(new Lantern()); // Holding a lantern to enhance mystery

        VirtualArmor = 15;
    }

    public LorithTheVeilSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lorith, a seeker of veils between worlds. Have you come seeking hidden knowledge?");
        
        // Dialogue options
        greeting.AddOption("Tell me more about the veils.", 
            p => true, 
            p =>
            {
                DialogueModule veilModule = new DialogueModule("The veils between worlds are thin here. With the right artifacts, one can glimpse secrets long forgotten. Do you happen to possess a HangingMask? If so, I can offer you a special exchange.");
                
                // Nested options within veil conversation
                veilModule.AddOption("Why are you interested in the veils?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("I once fought in the underground pits, where violence and brutality ruled my life. I sought power through blood, but the faces of those I defeated haunt me still. The veils between worlds, they give me a glimpse of redemption, a chance to make amends, perhaps even save those souls."
                            + "The more I understand, the more I might find a way to lift the burden of those lives from my conscience.");
                        
                        reasonModule.AddOption("You were an underground fighter?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule fighterModule = new DialogueModule("Yes, I fought for survival, for gold, for a reason to keep going after I lost everything. The ring was brutal, the fighters were ruthless, and I became one of them. Every punch, every kick was an attempt to drown out the pain of my past."
                                    + "I was drawn to violence after witnessing the worst humanity had to offer. A tragedy befell my family—one that left me broken, haunted by their screams, driven to bury that pain with bloodshed.");
                                
                                fighterModule.AddOption("Did fighting help you forget?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule forgetModule = new DialogueModule("No. It never truly helped me forget. The faces of my opponents—they visit me in my dreams, their eyes filled with confusion and fear. I thought violence would make me stronger, but it only deepened my wounds."
                                            + "Now I seek the veils, hoping to find solace or redemption. Perhaps, by helping others glimpse the truths of these worlds, I can find my own way to peace.");
                                        plaa.SendGump(new DialogueGump(plaa, forgetModule));
                                    });
                                
                                fighterModule.AddOption("Did you ever find peace in the ring?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule peaceModule = new DialogueModule("Peace? No, not in the ring. The ring was chaos. Each fight was a reminder of what I had lost, a desperate struggle to hold onto something. The ring became a symbol of my failure, of the man I never wanted to become."
                                            + "It wasn't until I discovered the existence of the veils that I realized there might be another way—something beyond violence, beyond the brutality that consumed me.");
                                        
                                        peaceModule.AddOption("What made you turn to the veils?", 
                                            plaad => true, 
                                            plaad =>
                                            {
                                                DialogueModule veilsPathModule = new DialogueModule("I met a mystic after one of my fights—a strange, hooded figure who seemed to know me better than I knew myself. They spoke of the veils, of glimpses into other worlds where answers could be found."
                                                    + "I laughed at them at first, but something in their words resonated with me. The next day, I found myself seeking them out, and they led me to this path. Since then, I've been unraveling the mysteries of the veils, hoping to find a way to make amends for the pain I've caused.");
                                                plaad.SendGump(new DialogueGump(plaad, veilsPathModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, peaceModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, fighterModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });
                
                // Trade option after story
                veilModule.AddOption("I do have a HangingMask. What can you offer?", 
                    pl => HasHangingMask(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                veilModule.AddOption("I do not have a HangingMask.", 
                    pl => !HasHangingMask(pl), 
                    pl =>
                    {
                        pl.SendMessage("Return when you have obtained a HangingMask, and I will show you wonders.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                veilModule.AddOption("I traded recently; I will return later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You must wait before another exchange can occur. The magic takes time to replenish.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, veilModule));
            });

        greeting.AddOption("Farewell, mysterious one.", 
            p => true, 
            p =>
            {
                p.SendMessage("Lorith nods knowingly, as if understanding far more than he lets on.");
            });

        return greeting;
    }

    private bool HasHangingMask(PlayerMobile player)
    {
        // Check the player's inventory for HangingMask
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HangingMask)) != null;
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
        // Remove the HangingMask and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item hangingMask = player.Backpack.FindItemByType(typeof(HangingMask));
        if (hangingMask != null)
        {
            hangingMask.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("I can offer you either a SkullIncense or AncientRunes. Which do you choose?");
            
            // Add options for SkullIncense and AncientRunes
            rewardChoiceModule.AddOption("SkullIncense", pl => true, pl => 
            {
                pl.AddToBackpack(new SkullIncense());
                pl.SendMessage("You receive a SkullIncense, its fragrance thick with mystery.");
            });
            
            rewardChoiceModule.AddOption("AncientRunes", pl => true, pl =>
            {
                pl.AddToBackpack(new AncientRunes());
                pl.SendMessage("You receive AncientRunes, humming faintly with forgotten power.");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a HangingMask.");
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