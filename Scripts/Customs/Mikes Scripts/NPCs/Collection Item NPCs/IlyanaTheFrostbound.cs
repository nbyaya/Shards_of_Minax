using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class IlyanaTheFrostbound : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public IlyanaTheFrostbound() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ilyana the Frostbound";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(55);
        SetDex(65);
        SetInt(110);

        SetHits(90);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Cloak(1150)); // Icy blue cloak
        AddItem(new Boots(1109)); // Dark boots
        AddItem(new FurCape()); // Fur cape for warmth
        AddItem(new Kilt(1153)); // Frosty skirt
        AddItem(new ElementRegular()); // Unique headpiece

        VirtualArmor = 15;
    }

    public IlyanaTheFrostbound(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Ilyana, bound to the chill winds of the north. Have you come seeking wisdom, or perhaps... a trade?");

        // Start with some lore dialogue about her story
        greeting.AddOption("Tell me your story.",
            p => true,
            p =>
            {
                DialogueModule storyModule = new DialogueModule("I was once a guardian of the Frost Shrine, deep within the glacier. When the shrine fell, I became bound to the ice, wandering in search of lost knowledge and rare artifacts.");
                storyModule.AddOption("What happened to the Frost Shrine?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule shrineModule = new DialogueModule("The shrine was attacked by those who sought power without understanding the cost. I fought alongside my comrades, but we were overwhelmed. The magic of the shrine was shattered, and I was left as the last vestige of its guardianship.");
                        shrineModule.AddOption("It must have been hard to lose everything.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule hardshipModule = new DialogueModule("Yes, it was. Many I called friends were lost that day. I bear these scars not only on my skin but in my heart. Yet, I endure. I fight for their memory, and for the hope that one day, perhaps, the shrine may be restored.");
                                hardshipModule.AddOption("You're strong to keep going.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ilyana gives a solemn nod, her eyes filled with a mixture of pain and determination.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                hardshipModule.AddOption("I can't imagine such loss.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ilyana sighs, her gaze distant. 'It is not something I would wish upon anyone.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, hardshipModule));
                            });
                        shrineModule.AddOption("Is there any hope of restoring the shrine?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule restorationModule = new DialogueModule("Perhaps. If the ancient relics and scrolls can be found, there may be a chance. It would take many brave souls, and great power, to undo the damage done.");
                                restorationModule.AddOption("I'd like to help, if I can.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ilyana smiles faintly. 'Your offer is kind. Should you find any relics, bring them to me.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                restorationModule.AddOption("It sounds almost impossible.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ilyana nods. 'Perhaps. But I must hold onto hope, for without it, there is nothing.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, restorationModule));
                            });
                        pl.SendGump(new DialogueGump(pl, shrineModule));
                    });
                storyModule.AddOption("You must have seen much in your time.",
                    pla => true,
                    pla =>
                    {
                        DialogueModule experiencesModule = new DialogueModule("Indeed. I have walked through blizzards, faced creatures born of ice and darkness, and seen the greed of men destroy what they do not understand. My scars are a testament to the battles I have fought, but also to the loyalty I hold for those who stood beside me.");
                        experiencesModule.AddOption("Tell me about the creatures of ice.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule creaturesModule = new DialogueModule("The Frost Wraiths are among the most dangerous. They are spirits bound to the ice, twisted by dark magic. They guard the remnants of the shrine, attacking any who come near. Then there are the Frostbite Wolves, relentless and cunning, always hunting in packs.");
                                creaturesModule.AddOption("How do you fight them?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule fightModule = new DialogueModule("With caution and steel. The Wraiths can only be harmed by enchanted weapons, while the wolves fear fire. It takes skill, bravery, and the right tools to survive against them.");
                                        fightModule.AddOption("I admire your courage.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Ilyana gives a small, weary smile. 'Courage, or perhaps stubbornness. Either way, it has kept me alive.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        fightModule.AddOption("Thank you for the insight.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Ilyana nods. 'May it serve you well, should you face them.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, fightModule));
                                    });
                                creaturesModule.AddOption("They sound terrifying.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Ilyana nods. 'They are. But fear can be a weapon, if you learn to wield it.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, creaturesModule));
                            });
                        experiencesModule.AddOption("How do you keep hope after all this?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule hopeModule = new DialogueModule("Hope is all I have left. I fight for my friends, for those who fell at the shrine. I cannot allow their sacrifice to be in vain. I believe that, one day, we will see a better tomorrow, even if I must carve it out of the ice myself.");
                                hopeModule.AddOption("You're an inspiration.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Ilyana's eyes soften, her voice almost a whisper. 'Thank you. It means more than you know.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                hopeModule.AddOption("I hope you're right.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Ilyana nods, her expression resolute. 'We must make it so.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, hopeModule));
                            });
                        pla.SendGump(new DialogueGump(pla, experiencesModule));
                    });
                storyModule.AddOption("Thank you for sharing.",
                    pla => true,
                    pla =>
                    {
                        pla.SendMessage("Ilyana gives a faint smile.");
                    });
                p.SendGump(new DialogueGump(p, storyModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you need something?",
            p => true,
            p =>
            {
                DialogueModule tradeModule = new DialogueModule("I do seek something rare... a FrostToken. If you bring one to me, I can offer an ElementRegular and a MaxxiaScroll as a token of gratitude. But beware, I can only perform this exchange once every 10 minutes.");
                tradeModule.AddOption("I'd like to trade.",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule confirmTradeModule = new DialogueModule("Do you have a FrostToken for me?");
                        confirmTradeModule.AddOption("Yes, I have a FrostToken.",
                            plaa => HasFrostToken(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        confirmTradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasFrostToken(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a FrostToken.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        confirmTradeModule.AddOption("I've traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, confirmTradeModule));
                    });
                tradeModule.AddOption("Perhaps another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Ilyana bows slightly, her eyes glinting like ice.");
            });

        return greeting;
    }

    private bool HasFrostToken(PlayerMobile player)
    {
        // Check the player's inventory for FrostToken
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FrostToken)) != null;
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
        // Remove the FrostToken and give the ElementRegular and MaxxiaScroll, then set the cooldown timer
        Item frostToken = player.Backpack.FindItemByType(typeof(FrostToken));
        if (frostToken != null)
        {
            frostToken.Delete();
            player.AddToBackpack(new ElementRegular());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the FrostToken and receive an ElementRegular and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a FrostToken.");
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