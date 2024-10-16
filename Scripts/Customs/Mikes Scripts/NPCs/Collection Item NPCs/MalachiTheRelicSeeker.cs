using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MalachiTheRelicSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MalachiTheRelicSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Malachi the Relic Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(2453)); // Deep blue shirt
        AddItem(new LongPants(1109)); // Dark brown pants
        AddItem(new Boots(1175)); // Midnight black boots
        AddItem(new FeatheredHat(1359)); // A striking red hat
        AddItem(new Cloak(1194)); // A cloak with a mystical purple hue

        VirtualArmor = 15;
    }

    public MalachiTheRelicSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Malachi, seeker of ancient relics and curious artifacts. You look like someone who may have seen wonders of the world.");

        // Dialogue options
        greeting.AddOption("What kind of relics are you looking for?",
            p => true,
            p =>
            {
                DialogueModule relicsModule = new DialogueModule("I am always on the lookout for rare and mysterious items. Lately, I've been searching for a particular object: a LampPostC. If you have one, I would gladly exchange it for something just as unique.");

                relicsModule.AddOption("Why do you need a LampPostC?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule secretModule = new DialogueModule("Ah, the LampPostC... it's not just an ordinary artifact. It holds power—power that my master desires. I dare not speak of it too much, but suffice it to say, it could be my key to freedom from his grasp.");

                        secretModule.AddOption("Who is your master?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule masterModule = new DialogueModule("My master is a cruel and powerful being. His name is whispered in the darkest corners of the realm. I am bound to serve him, though it brings me great suffering. I have been searching for a way to escape his clutches, and perhaps the LampPostC might be my salvation.");

                                masterModule.AddOption("Why do you continue to serve him?",
                                    plab => true,
                                    plab =>
                                    {
                                        DialogueModule loyaltyModule = new DialogueModule("It is not a matter of choice. I am bound by ancient magic, a pact made long before I could comprehend its weight. My loyalty is forced, but I cling to the hope that one day I may break free. Perhaps someone like you could help me... if only I could find the courage to ask.");

                                        loyaltyModule.AddOption("How can I help you?",
                                            plac => true,
                                            plac =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("There is a relic, one that my master fears. It is called the Shard of Severance. If you could find it, perhaps... just perhaps, it could sever the bond that holds me. But be warned, the path to it is perilous, and my master has eyes everywhere.");

                                                helpModule.AddOption("I will seek the Shard of Severance.",
                                                    plad => true,
                                                    plad =>
                                                    {
                                                        plad.SendMessage("Malachi's eyes well up with hope. 'Thank you, traveler. If you succeed, you will have my eternal gratitude—and my freedom.'");
                                                    });

                                                helpModule.AddOption("This sounds too dangerous for me.",
                                                    plad => true,
                                                    plad =>
                                                    {
                                                        plad.SendMessage("Malachi nods solemnly. 'I understand. It is not a task for the faint of heart. Still, if you ever change your mind, I will be here.'");
                                                    });

                                                plac.SendGump(new DialogueGump(plac, helpModule));
                                            });

                                        loyaltyModule.AddOption("I cannot help you.",
                                            plac => true,
                                            plac =>
                                            {
                                                plac.SendMessage("Malachi looks down, a shadow of despair crossing his face. 'I understand. Not everyone can shoulder another's burden.'");
                                            });

                                        plab.SendGump(new DialogueGump(plab, loyaltyModule));
                                    });

                                masterModule.AddOption("I must be going.",
                                    plab => true,
                                    plab =>
                                    {
                                        plab.SendMessage("Malachi gives you a weary nod. 'Safe travels, wanderer. May your path be less burdened than mine.'");
                                    });

                                plaa.SendGump(new DialogueGump(plaa, masterModule));
                            });

                        secretModule.AddOption("I don't want to get involved.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Malachi sighs, a haunted look in his eyes. 'I cannot blame you. My troubles are my own to bear.'");
                            });

                        pl.SendGump(new DialogueGump(pl, secretModule));
                    });

                relicsModule.AddOption("Do you have something to trade in exchange?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you bring me a LampPostC, I can offer you either a BlueSand or a FunMushroom, along with a special scroll that I always carry.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a LampPostC for me?");
                                tradeModule.AddOption("Yes, I have a LampPostC.",
                                    plaa => HasLampPostC(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasLampPostC(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a LampPostC.");
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

                p.SendGump(new DialogueGump(p, relicsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Malachi gives you a knowing nod and returns to examining his notes.");
            });

        return greeting;
    }

    private bool HasLampPostC(PlayerMobile player)
    {
        // Check the player's inventory for LampPostC
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LampPostC)) != null;
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
        // Remove the LampPostC and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item lampPostC = player.Backpack.FindItemByType(typeof(LampPostC));
        if (lampPostC != null)
        {
            lampPostC.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for BlueSand and FunMushroom
            rewardChoiceModule.AddOption("BlueSand", pl => true, pl =>
            {
                pl.AddToBackpack(new BlueSand());
                pl.SendMessage("You receive a BlueSand!");
            });

            rewardChoiceModule.AddOption("FunMushroom", pl => true, pl =>
            {
                pl.AddToBackpack(new FunMushroom());
                pl.SendMessage("You receive a FunMushroom!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a LampPostC.");
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