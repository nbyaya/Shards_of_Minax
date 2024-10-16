using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class RalinaTheWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RalinaTheWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ralina the Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(100);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyDress(2150)); // A fancy dress with a pale blue hue
        AddItem(new Sandals(1109)); // Dark blue sandals
        AddItem(new Bonnet(1153)); // A bonnet matching her dress
        AddItem(new Scissors()); // She carries scissors, fitting for a weaver

        VirtualArmor = 12;
    }

    public RalinaTheWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there, traveler! I am Ralina, a weaver of fine fabrics and tales alike. Do you have an eye for quality textiles?");

        // Start with dialogue about her work
        greeting.AddOption("What kind of fabrics do you weave?",
            p => true,
            p =>
            {
                DialogueModule fabricsModule = new DialogueModule("Oh, I weave all sorts! Velvet, silk, and even enchanted threads spun from spider silk. Each fabric tells a story, and I do my best to capture those stories in my work.");

                fabricsModule.AddOption("Enchanted threads? Tell me more!",
                    pl => true,
                    pl =>
                    {
                        DialogueModule enchantedModule = new DialogueModule("Ah, enchanted threads are rare indeed. They are spun from the silk of magical spiders, and the resulting cloth can protect its wearer from harm or even mend itself if damaged. It takes a steady hand and much patience to work with such delicate materials.");

                        enchantedModule.AddOption("What makes you so skilled at handling enchanted threads?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule skillModule = new DialogueModule("I learned much through trial and error. You see, I grew up on the streets of London. An orphan, abandoned to fend for myself. I had to be resourceful, and I taught myself the arts of weaving. Handling enchanted threads was my way of escaping a harsh reality, but even now, I still feel the shadows of my past lingering over me.");
                                
                                skillModule.AddOption("I'm sorry to hear that. It must have been difficult.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule difficultModule = new DialogueModule("Yes, it was. There were times I had nothing to eat, nowhere to go. I learned to trust no one, and to depend on my skills. It's why I am sometimes suspicious of others. I suppose it has made me stronger, but I still find it hard to let go of those old insecurities.");
                                        
                                        difficultModule.AddOption("You seem very capable. I'm sure you've come a long way.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Ralina smiles, though her eyes hold a hint of sadness.");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        
                                        difficultModule.AddOption("Why do you remain here, weaving?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule whyModule = new DialogueModule("I stay here because I have nowhere else to go. I seek something that has eluded me my entire life: a sense of belonging. Perhaps, through my weaving, I can find a purpose. Maybe I can one day exact revenge on those who wronged me, though I fear the pursuit of vengeance will bring me no peace.");
                                                
                                                whyModule.AddOption("Revenge? Who wronged you?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule revengeModule = new DialogueModule("The ones who abandoned me... my so-called family. They left me to fend for myself, and I have never forgotten the betrayal. I am still haunted by questions—why was I not good enough? What did I do to deserve such a fate? I weave to drown out the memories, but they always find a way back.");
                                                        
                                                        revengeModule.AddOption("I hope you find the answers you seek.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Ralina looks at you thoughtfully, her eyes glistening with unshed tears.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        
                                                        revengeModule.AddOption("Holding onto anger can be a heavy burden.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                DialogueModule burdenModule = new DialogueModule("I know. It eats at me, even now. But it also drives me. Without it, I wonder if I would still have the will to survive. I envy those who are free from such burdens, but I also fear losing the fire that keeps me going.");
                                                                
                                                                burdenModule.AddOption("Perhaps one day you'll find a different purpose.",
                                                                    plaaaaaaa => true,
                                                                    plaaaaaaa =>
                                                                    {
                                                                        plaaaaaaa.SendMessage("Ralina gives a faint smile, as if imagining a different life for a brief moment.");
                                                                        plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                    });
                                                                
                                                                burdenModule.AddOption("Farewell, Ralina. I hope you find peace.",
                                                                    plaaaaaaa => true,
                                                                    plaaaaaaa =>
                                                                    {
                                                                        plaaaaaaa.SendMessage("Ralina nods slowly, her expression pensive as she watches you leave.");
                                                                    });
                                                                
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, burdenModule));
                                                            });
                                                        
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, revengeModule));
                                                    });
                                                
                                                whyModule.AddOption("I hope weaving brings you comfort.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Ralina nods, her fingers absentmindedly playing with a loose thread on her dress.");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                
                                                plaaaa.SendGump(new DialogueGump(plaaaa, whyModule));
                                            });
                                        
                                        plaaa.SendGump(new DialogueGump(plaaa, difficultModule));
                                    });
                                
                                skillModule.AddOption("You are very resourceful. I admire that.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Ralina blushes slightly, clearly not used to receiving compliments.");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                
                                plaa.SendGump(new DialogueGump(plaa, skillModule));
                            });

                        enchantedModule.AddOption("Fascinating! Thank you.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, enchantedModule));
                    });

                fabricsModule.AddOption("That sounds lovely, but perhaps another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, fabricsModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you need anything in particular?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("As a matter of fact, I am in search of a DressForm to aid me in my work. If you bring me one, I can offer you a GalvanizedTub and a MaxxiaScroll as thanks. But remember, I can only trade once every 10 minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a DressForm for me?");
                        tradeModule.AddOption("Yes, I have a DressForm.",
                            plaa => HasDressForm(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasDressForm(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a DressForm.");
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
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Ralina nods and smiles, her hands already busy weaving a new thread.");
            });

        return greeting;
    }

    private bool HasDressForm(PlayerMobile player)
    {
        // Check the player's inventory for DressForm
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DressForm)) != null;
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
        // Remove the DressForm and give the GalvanizedTub and MaxxiaScroll, then set the cooldown timer
        Item dressForm = player.Backpack.FindItemByType(typeof(DressForm));
        if (dressForm != null)
        {
            dressForm.Delete();
            player.AddToBackpack(new GalvanizedTub());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the DressForm and receive a GalvanizedTub and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a DressForm.");
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