using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThaliaTheWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaliaTheWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thalia the Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(110);

        SetHits(90);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1150)); // Light blue fancy shirt
        AddItem(new Skirt(2413)); // Deep green skirt
        AddItem(new Sandals(144)); // Simple sandals
        AddItem(new LeatherGloves()); // Beaded necklace

        VirtualArmor = 15;
    }

    public ThaliaTheWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Thalia, a weaver of rare and exquisite fabrics. Might you be interested in a special exchange today?");

        // Dialogue options
        greeting.AddOption("What kind of exchange are you offering?",
            p => true,
            p =>
            {
                DialogueModule exchangeModule = new DialogueModule("I am in need of a SheepCarcass. If you bring me one, I will reward you with either a TailoringTalisman or a DecorativeOrchid. Oh, and I shall also grant you a MaxxiaScroll for your troubles. You see, my dear traveler, there are deeper forces at work here, powers that most would not dare meddle with...");

                exchangeModule.AddOption("Deeper forces? What do you mean?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule darkPowersModule = new DialogueModule("Ah, I see you are curious. Very well. The fabrics I weave are not just of thread and silk; they carry the essence of something darker, something powerful. It is said that those who dare offer sacrifices to the shadows may gain unimaginable strength. Would you care to learn more of the Cult of the Woven Shadow?");

                        darkPowersModule.AddOption("Tell me more about the Cult of the Woven Shadow.",
                            p2 => true,
                            p2 =>
                            {
                                DialogueModule cultModule = new DialogueModule("The Cult of the Woven Shadow is an ancient order. We are devoted to harnessing the dark energies that flow unseen through this world. With each thread I weave, I bind the powers of the shadows closer to my will. Many fear what they cannot understand, but those who join us see the true potential. Fear can be a tool, and devotion a path to greatness.");

                                cultModule.AddOption("What would joining your cult entail?",
                                    p3 => true,
                                    p3 =>
                                    {
                                        DialogueModule joiningModule = new DialogueModule("To join the Cult of the Woven Shadow, one must prove their devotion. There are rituals, trials that test both body and mind. You must be willing to sacrifice, to let go of what you hold dear, and embrace the darkness that lies within. Only then can you truly harness the power that we offer.");

                                        joiningModule.AddOption("What kind of rituals?",
                                            p4 => true,
                                            p4 =>
                                            {
                                                DialogueModule ritualsModule = new DialogueModule("The rituals are varied, some requiring rare items, others requiring feats of bravery or cunning. One such ritual involves gathering the essence of a creature that embodies purity, like a SheepCarcass. It is through corrupting purity that we grow stronger. Bring me the SheepCarcass, and I shall reward you... perhaps even initiate you further into our ways.");

                                                ritualsModule.AddOption("I have a SheepCarcass, let's trade.",
                                                    plq => HasSheepCarcass(pl) && CanTradeWithPlayer(pl),
                                                    plq =>
                                                    {
                                                        CompleteTrade(pl);
                                                    });

                                                ritualsModule.AddOption("I need more time to decide.",
                                                    p5 => true,
                                                    p5 =>
                                                    {
                                                        p5.SendMessage("Take your time, but remember... opportunities like these do not wait forever.");
                                                        p5.SendGump(new DialogueGump(p5, CreateGreetingModule(p5)));
                                                    });

                                                p4.SendGump(new DialogueGump(p4, ritualsModule));
                                            });

                                        joiningModule.AddOption("I need more time to think.",
                                            p4 => true,
                                            p4 =>
                                            {
                                                p4.SendMessage("The shadows are patient, but they are always watching. Come back when you are ready.");
                                                p4.SendGump(new DialogueGump(p4, CreateGreetingModule(p4)));
                                            });

                                        p3.SendGump(new DialogueGump(p3, joiningModule));
                                    });

                                cultModule.AddOption("This sounds dangerous, I think I'll pass.",
                                    p3 => true,
                                    p3 =>
                                    {
                                        p3.SendMessage("Very well, but remember, true power comes only to those who dare to reach for it. Farewell, traveler.");
                                    });

                                p2.SendGump(new DialogueGump(p2, cultModule));
                            });

                        darkPowersModule.AddOption("No, I don't want to meddle with dark forces.",
                            plw => true,
                            plw =>
                            {
                                pl.SendMessage("Wise choice, perhaps. The darkness is not for everyone.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                            });

                        pl.SendGump(new DialogueGump(pl, darkPowersModule));
                    });

                exchangeModule.AddOption("I have a SheepCarcass, let's trade.",
                    pl => HasSheepCarcass(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                exchangeModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                exchangeModule.AddOption("Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, exchangeModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Thalia nods and smiles, but you sense a lingering darkness in her eyes.");
            });

        return greeting;
    }

    private bool HasSheepCarcass(PlayerMobile player)
    {
        // Check the player's inventory for SheepCarcass
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SheepCarcass)) != null;
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
        // Remove the SheepCarcass and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item sheepCarcass = player.Backpack.FindItemByType(typeof(SheepCarcass));
        if (sheepCarcass != null)
        {
            sheepCarcass.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for TailoringTalisman and DecorativeOrchid
            rewardChoiceModule.AddOption("TailoringTalisman", pl => true, pl =>
            {
                pl.AddToBackpack(new TailoringTalisman());
                pl.SendMessage("You receive a TailoringTalisman!");
            });

            rewardChoiceModule.AddOption("DecorativeOrchid", pl => true, pl =>
            {
                pl.AddToBackpack(new DecorativeOrchid());
                pl.SendMessage("You receive a DecorativeOrchid!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SheepCarcass.");
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