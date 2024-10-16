using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ValenTheSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ValenTheSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Valen the Seeker";
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
        AddItem(new HoodedShroudOfShadows(1157)); // Hooded cloak with a dark, mysterious hue
        AddItem(new Boots(1109)); // Dark boots
        AddItem(new BodySash(1175)); // Sash with a light blue hue
        AddItem(new Lantern()); // Holding a lantern to symbolize his seeking nature

        VirtualArmor = 15;
    }

    public ValenTheSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Valen, a seeker of ancient mysteries and forgotten relics. Are you here in search of knowledge or perhaps something more?");

        // Start with dialogue about his purpose
        greeting.AddOption("What are you seeking, Valen?",
            p => true,
            p =>
            {
                DialogueModule seekingModule = new DialogueModule("I seek knowledge that has been long forgotten—artifacts of power, items that hold the essence of mystery itself. Recently, I have heard of a relic known as the MysteryOrb. It is said to contain the secrets of an ancient cartographer.");

                seekingModule.AddOption("What is the MysteryOrb?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule orbModule = new DialogueModule("The MysteryOrb is an elusive item, rumored to guide its bearer to lost treasures and forgotten maps. I have been searching for one myself, but they are rare indeed. Perhaps you have come across one in your travels?");
                        orbModule.AddOption("I have a MysteryOrb. Can we trade?",
                            pla => HasMysteryOrb(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });
                        orbModule.AddOption("I do not have one yet.",
                            pla => !HasMysteryOrb(pla),
                            pla =>
                            {
                                pla.SendMessage("No worries, traveler. Return if you happen to find one.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        orbModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        orbModule.AddOption("Why do you seek the MysteryOrb, Valen?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule reasonModule = new DialogueModule("The MysteryOrb is more than a mere artifact to me. I was once a soldier, fighting battles for a kingdom that forgot my sacrifice. One fateful night, mutants descended upon my village. They slaughtered my family, leaving me with nothing but pain and rage. I heard whispers that the MysteryOrb can reveal the hiding places of those mutants, guiding me to my revenge.");
                                reasonModule.AddOption("I am sorry for your loss, Valen. That must be a heavy burden.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule sorrowModule = new DialogueModule("It is a burden I carry every day, but it is also my purpose. I have no fear left—only determination. I will not stop until every last one of those creatures has paid for what they did. If you find the MysteryOrb, I urge you to help me in my quest.");
                                        sorrowModule.AddOption("I will keep an eye out for it, Valen.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Valen nods solemnly, a flicker of hope in his eyes.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        sorrowModule.AddOption("I do not wish to get involved in your vengeance.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("I understand. Not everyone is meant for this path. Be well, traveler.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, sorrowModule));
                                    });
                                reasonModule.AddOption("Vengeance is a dangerous path, Valen. Are you sure this is what you want?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dangerousPathModule = new DialogueModule("Dangerous, yes, but necessary. The mutants took everything from me. My family, my peace, my future. I have nothing left to lose. The only thing that keeps me going is the thought of avenging them. Fear does not bind me any longer—only my will to see this through.");
                                        dangerousPathModule.AddOption("I hope you find the peace you seek.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Valen's gaze hardens, but there is a glimmer of gratitude in his eyes. 'Thank you, traveler.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        dangerousPathModule.AddOption("I will leave you to your mission, then.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Valen turns away, his focus unwavering. 'Farewell.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, dangerousPathModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, reasonModule));
                            });
                        pl.SendGump(new DialogueGump(pl, orbModule));
                    });

                seekingModule.AddOption("That sounds intriguing. Perhaps I'll find it one day.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                seekingModule.AddOption("You seem determined, Valen. What drives you?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule driveModule = new DialogueModule("Determination is all I have left. I was once a soldier, serving a kingdom that abandoned me when I needed them most. My family was everything to me—until the mutants took them away. Now, I dedicate my life to hunting them down, ensuring that no one else will suffer as I have.");
                        driveModule.AddOption("I admire your strength, Valen. You are fearless.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule admirationModule = new DialogueModule("Fear is a luxury I cannot afford. When you have lost everything, fear loses its grip on you. I fight for those who cannot, and I will see my mission through, no matter the cost.");
                                admirationModule.AddOption("May your courage never waver.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Valen bows his head slightly. 'Thank you, traveler. Your words give me strength.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                admirationModule.AddOption("I hope you find what you are looking for.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Valen nods, his eyes filled with a fierce resolve. 'I will, no matter how long it takes.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, admirationModule));
                            });
                        driveModule.AddOption("Revenge can consume you, Valen. Be careful.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule warningModule = new DialogueModule("Revenge is all that keeps me alive, traveler. It is both my curse and my salvation. I know the risks, but I have already lost everything that mattered. I will see this through or die trying.");
                                warningModule.AddOption("Then may your path lead you to peace, eventually.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Valen's expression softens for a brief moment. 'Perhaps. One day.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                warningModule.AddOption("Good luck, Valen. You will need it.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Valen smirks, a fire burning in his eyes. 'Luck is for those who hesitate. I have no such luxury.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, warningModule));
                            });
                        pl.SendGump(new DialogueGump(pl, driveModule));
                    });
                p.SendGump(new DialogueGump(p, seekingModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Valen.",
            p => true,
            p =>
            {
                p.SendMessage("Valen nods, his eyes still scanning the horizon for the next mystery.");
            });

        return greeting;
    }

    private bool HasMysteryOrb(PlayerMobile player)
    {
        // Check the player's inventory for MysteryOrb
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MysteryOrb)) != null;
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
        // Remove the MysteryOrb and give the CartographyTable and MaxxiaScroll, then set the cooldown timer
        Item mysteryOrb = player.Backpack.FindItemByType(typeof(MysteryOrb));
        if (mysteryOrb != null)
        {
            mysteryOrb.Delete();
            player.AddToBackpack(new CartographyTable());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MysteryOrb and receive a CartographyTable and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a MysteryOrb.");
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