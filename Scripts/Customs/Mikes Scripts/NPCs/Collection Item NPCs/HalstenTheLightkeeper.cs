using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class HalstenTheLightkeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public HalstenTheLightkeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Halsten the Lightkeeper";
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
        AddItem(new Cloak(1150)); // Dark blue cloak
        AddItem(new LongPants(1109)); // Deep gray pants
        AddItem(new FancyShirt(2402)); // White shirt
        AddItem(new Boots(1175)); // Light brown boots
        AddItem(new Lantern()); // Carries a lantern, fitting for a lightkeeper

        VirtualArmor = 15;
    }

    public HalstenTheLightkeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler. I am Halsten, keeper of lights that once guided souls through darkness. Do you perhaps possess something that may rekindle their forgotten glow?");

        // Offer to tell a story about the lampposts
        greeting.AddOption("Tell me about the lights you keep.",
            p => true,
            p =>
            {
                DialogueModule storyModule = new DialogueModule("The lampposts were once spread across old roads, guiding travelers safely. Each one has a spirit, a spark if you will, that yearns to be rekindled. Perhaps you have found such an item, a 'LampPostB'? It may help me restore their glow.");
                
                storyModule.AddOption("How do I help you rekindle them?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule helpModule = new DialogueModule("If you have a LampPostB, I can perform a ritual to restore its glow. I would be grateful, and I can offer you something in return, a special CharcuterieBoard for your trouble.");
                        
                        helpModule.AddOption("I have a LampPostB. Can we make the trade?",
                            plaa => HasLampPostB(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        helpModule.AddOption("I don't have a LampPostB right now.",
                            plaa => !HasLampPostB(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a LampPostB. The light's spirit waits patiently.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        helpModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, helpModule));
                    });
                
                storyModule.AddOption("Why do you keep these lights?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule purposeModule = new DialogueModule("Ah, it is a tale of obsession, I must admit. Once, I was a journalist, a man of words and facts. I traveled far and wide, documenting stories for the people. But one night, while covering a story about a haunted village, I encountered something that changed me. A spirit, an entity, it whispered secrets that I could not understand, yet I was compelled to listen.");
                        
                        purposeModule.AddOption("What did the spirit tell you?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule spiritModule = new DialogueModule("The spirit spoke of the lights—of ancient guardians, of paths that must remain lit for the souls that wander. It spoke of dangers lurking in the shadows, of forgotten promises. It spoke to me until I could no longer sleep without hearing its voice. I left my career, my life, to seek these spirits, to rekindle the lights that would keep them at bay.");
                                
                                spiritModule.AddOption("That sounds dangerous. Are you sure this is real?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule delusionModule = new DialogueModule("Real? Ha! What is real, traveler? The light I see, the voices I hear—they are as real as the ground beneath your feet, or perhaps they are not. I have seen things that others would call illusions, but to me, they are undeniable truths. I am not mad, though many would call me so. I am simply... enlightened.");
                                        
                                        delusionModule.AddOption("I see. You are very brave.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Halsten smiles, his eyes reflecting the flickering light of his lantern. 'Bravery, or madness? Sometimes, they are one and the same.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        delusionModule.AddOption("This seems too dangerous for me.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Halsten nods solemnly. 'The path I walk is not for everyone. Darkness is not something many wish to face.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, delusionModule));
                                    });
                                
                                spiritModule.AddOption("What happened to the village?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule villageModule = new DialogueModule("The village? Ah, it is gone now, swallowed by darkness. The spirits there were restless, and no one believed me when I said the lights must stay lit. One by one, the villagers left, and soon the spirits had no barriers, no guiding lights. The village is a ruin now, a place where no one dares tread.");
                                        
                                        villageModule.AddOption("That is tragic.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Halsten lowers his head, his voice barely a whisper. 'Indeed, it is. And I have vowed never to let it happen again.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        villageModule.AddOption("Do you think the lights can protect others?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule protectionModule = new DialogueModule("Yes, that is my hope. The lights are not just beacons for travelers—they are wards, barriers that spirits cannot cross. They are symbols of hope, of guidance. If I can restore them, perhaps I can prevent another village from falling into despair.");
                                                
                                                protectionModule.AddOption("I hope you succeed, Halsten.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Halsten smiles, the light of his lantern seeming to grow warmer. 'Thank you, traveler. Your words give me strength.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                protectionModule.AddOption("This is a noble cause.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Halsten nods, determination etched in his features. 'Noble, perhaps. Or simply necessary.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, protectionModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, villageModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, spiritModule));
                            });
                        
                        purposeModule.AddOption("Why did you abandon journalism?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule abandonModule = new DialogueModule("I was obsessed, traveler. The stories I wrote became mere words, devoid of meaning compared to the whispers I heard from the spirits. My editor called me delusional, my colleagues whispered behind my back. I realized that I could no longer live in the world of mundane news when the world of spirits was calling to me, demanding my attention.");
                                
                                abandonModule.AddOption("Do you regret it?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? There are moments, yes. Moments when I miss the simplicity of my old life, the certainty of facts and the comfort of reason. But then I remember the village, the spirits, and I know that I am doing what must be done. Even if it costs me my sanity, I must continue.");
                                        
                                        regretModule.AddOption("You are dedicated, I'll give you that.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Halsten chuckles softly, his eyes distant. 'Dedication, obsession—sometimes they blur together.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        regretModule.AddOption("I hope it is worth it.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Halsten's expression hardens, his voice resolute. 'It must be. There is no other choice.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, regretModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, abandonModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, purposeModule));
                    });
                
                storyModule.AddOption("Maybe another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, storyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Halsten nods, his lantern flickering as he turns away.");
            });

        return greeting;
    }

    private bool HasLampPostB(PlayerMobile player)
    {
        // Check the player's inventory for LampPostB
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LampPostB)) != null;
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
        // Remove the LampPostB and give the CharcuterieBoard and MaxxiaScroll, then set the cooldown timer
        Item lampPostB = player.Backpack.FindItemByType(typeof(LampPostB));
        if (lampPostB != null)
        {
            lampPostB.Delete();
            player.AddToBackpack(new CharcuterieBoard());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the LampPostB and receive a CharcuterieBoard and MaxxiaScroll in return. Halsten smiles warmly, the lantern glowing a little brighter.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a LampPostB.");
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