using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class KalimTheWanderingAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public KalimTheWanderingAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Kalim the Wandering Alchemist";
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
        AddItem(new Robe(1374)); // Colorful alchemist robe
        AddItem(new Sandals(1153)); // Bright sandals
        AddItem(new WizardsHat(1150)); // Mysterious wizard's hat
        AddItem(new HalfApron(1109)); // Utility apron with pouches

        VirtualArmor = 15;
    }

    public KalimTheWanderingAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Kalim, a wandering alchemist. Are you interested in rare and mysterious knowledge? Perhaps you seek a powerful artifact? Or maybe... something a bit more dangerous?");

        // Dialogue options
        greeting.AddOption("Tell me more about your travels.", 
            p => true, 
            p =>
            {
                DialogueModule travelsModule = new DialogueModule("I have traveled far and wide, gathering rare ingredients and discovering arcane secrets. Some say I have even dabbled in the forbidden... but what is life without a bit of risk?");

                // Nested story options
                travelsModule.AddOption("What kind of forbidden knowledge?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule forbiddenKnowledgeModule = new DialogueModule("Ah, you are a curious one, aren't you? I have dealt in curses, whispers of dark rituals, and objects that bring... unfortunate side effects. But these things come at a cost. Are you willing to pay?");

                        forbiddenKnowledgeModule.AddOption("Yes, I am interested.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule darkOfferModule = new DialogueModule("I can offer you a cursed amulet, one that brings power but also attracts the attention of... otherworldly beings. Or perhaps a vial of black ichor that can be used in rituals. But beware, once you accept these items, there is no turning back.");

                                darkOfferModule.AddOption("I will take the cursed amulet.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.AddToBackpack(new HorrodrickCube());
                                        plaa.SendMessage("You feel a chill as you take the amulet. It whispers to you...");
                                    });

                                darkOfferModule.AddOption("I will take the black ichor.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.AddToBackpack(new FairyOil());
                                        plaa.SendMessage("The vial feels unnaturally cold. You sense that it wants to be used.");
                                    });

                                darkOfferModule.AddOption("On second thought, maybe not.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("A wise decision. Some things are best left untouched.");
                                    });

                                pla.SendGump(new DialogueGump(pla, darkOfferModule));
                            });

                        forbiddenKnowledgeModule.AddOption("No, I think I'll pass.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Not everyone is ready for such knowledge. Perhaps another time.");
                            });

                        pl.SendGump(new DialogueGump(pl, forbiddenKnowledgeModule));
                    });

                travelsModule.AddOption("Do you need any special items?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a PersonalMortar, I can offer you a choice between a RareWire or a ForbiddenTome, plus an additional reward. But know this, the items I offer come with their own stories—and their own risks.");

                        // Nested trade dialogue with added deception
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a PersonalMortar for me? I promise, what I offer in return is worth every bit of danger it might bring.");

                                tradeModule.AddOption("Yes, I have a PersonalMortar.", 
                                    plaa => HasPersonalMortar(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });

                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasPersonalMortar(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a PersonalMortar. You won't regret it... or maybe you will.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });

                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Patience is key, friend. Come back when you are ready.");
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

                p.SendGump(new DialogueGump(p, travelsModule));
            });

        greeting.AddOption("Can I see what you're selling?", 
            p => true, 
            p =>
            {
                DialogueModule shopModule = new DialogueModule("Ah, a discerning customer! I have items you won't find anywhere else. Oddities, trinkets, and perhaps a few things... that may be cursed. Are you sure you wish to browse?");

                shopModule.AddOption("Show me what you have.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule itemsModule = new DialogueModule("I have a selection of goods: A Mirror of Deception, which will show you lies; A Ring of Paranoia, which heightens your senses but never lets you rest; and a Coin of Greed, which ensures you always want more.");

                        itemsModule.AddOption("Tell me about the Mirror of Deception.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("The Mirror of Deception reveals the truth... or perhaps lies. It is said that those who gaze into it may never trust what they see again.");
                            });

                        itemsModule.AddOption("Tell me about the Ring of Paranoia.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("The Ring of Paranoia makes you keenly aware of your surroundings. But beware, you may never feel safe again.");
                            });

                        itemsModule.AddOption("Tell me about the Coin of Greed.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("The Coin of Greed ensures that wealth flows your way... but at a cost. You may find yourself unable to part with even a single gold piece.");
                            });

                        itemsModule.AddOption("I will take the Mirror of Deception.", 
                            pla => true, 
                            pla =>
                            {
                                pla.AddToBackpack(new MirrorOfKalandra());
                                pla.SendMessage("You take the Mirror. For a moment, your reflection seems to smile back at you... strangely.");
                            });

                        itemsModule.AddOption("I will take the Ring of Paranoia.", 
                            pla => true, 
                            pla =>
                            {
                                pla.AddToBackpack(new RandomMagicJewelry());
                                pla.SendMessage("As you take the Ring, you feel a chill. You can't shake the feeling that someone is watching you.");
                            });

                        itemsModule.AddOption("I will take the Coin of Greed.", 
                            pla => true, 
                            pla =>
                            {
                                pla.AddToBackpack(new RandomFancyCoin());
                                pla.SendMessage("The Coin feels heavy in your hand. You suddenly feel a reluctance to let it go.");
                            });

                        itemsModule.AddOption("I need to think about it.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Take your time, but remember, opportunities like these don't last forever.");
                            });

                        pl.SendGump(new DialogueGump(pl, itemsModule));
                    });

                shopModule.AddOption("No, I think I'll pass.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("A shame... but perhaps it is for the best. Not everyone is ready for what I have to offer.");
                    });

                p.SendGump(new DialogueGump(p, shopModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Kalim nods sagely and continues mixing ingredients, his eyes watching you as you leave.");
            });

        return greeting;
    }

    private bool HasPersonalMortar(PlayerMobile player)
    {
        // Check the player's inventory for PersonalMortar
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(PersonalMortor)) != null;
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
        // Remove the PersonalMortar and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item personalMortar = player.Backpack.FindItemByType(typeof(PersonalMortor));
        if (personalMortar != null)
        {
            personalMortar.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for RareWire and ForbiddenTome
            rewardChoiceModule.AddOption("RareWire", pl => true, pl =>
            {
                pl.AddToBackpack(new RareWire());
                pl.SendMessage("You receive a RareWire!");
            });

            rewardChoiceModule.AddOption("ForbiddenTome", pl => true, pl =>
            {
                pl.AddToBackpack(new ForbiddenTome());
                pl.SendMessage("You receive a ForbiddenTome!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a PersonalMortar.");
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