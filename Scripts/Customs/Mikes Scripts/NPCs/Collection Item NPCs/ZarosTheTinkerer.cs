using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZarosTheTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZarosTheTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zaros the Tinkerer";
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
        AddItem(new FancyShirt(1150)); // Bright blue fancy shirt
        AddItem(new LongPants(1109)); // Dark brown pants
        AddItem(new Boots(1175)); // Light-colored boots
        AddItem(new HalfApron(1265)); // White tinkerer apron
        AddItem(new WideBrimHat(1153)); // Dark blue hat
        AddItem(new AncientRunes()); // An item representing his tinkering background

        VirtualArmor = 15;
    }

    public ZarosTheTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Zaros, the tinkerer of peculiar devices and strange elixirs. Tell me, have you ever encountered something truly... unique?");

        // Dialogue about his inventions
        greeting.AddOption("What kind of inventions do you make?",
            p => true,
            p =>
            {
                DialogueModule inventionsModule = new DialogueModule("Oh, all sorts of gadgets! From wind-up familiars to automatons that brew tea! But I am currently working on something that requires a very special substance. Would you be willing to help me?");

                inventionsModule.AddOption("What do you need?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I need an item called HydroxFluid. It’s quite rare, but if you happen to have one, I could offer you an AncientRune in exchange, plus a MaxxiaScroll as a token of my gratitude.");
                        
                        tradeIntroductionModule.AddOption("I have HydroxFluid. Let's make the trade.",
                            pla => HasHydroxFluid(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });

                        tradeIntroductionModule.AddOption("I don't have HydroxFluid right now.",
                            pla => !HasHydroxFluid(pla),
                            pla =>
                            {
                                pla.SendMessage("Ah, a pity. Come back if you manage to find some HydroxFluid!");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        tradeIntroductionModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("Patience, my friend. I can only make such trades once every 10 minutes. Come back later!");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                // Adding detailed nested options about his backstory
                inventionsModule.AddOption("Why do you tinker?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Tinkering... It keeps my hands busy, my mind distracted. Once, I was a soldier, a decorated warrior of the old wars. I saw horrors that no person should ever witness. Now, the gears and gadgets help keep the darkness at bay.");

                        backstoryModule.AddOption("What happened during the war?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule warModule = new DialogueModule("The battles were endless. I fought on the frontlines, standing shoulder to shoulder with comrades, most of whom never made it back. There was bravery, yes, but also much loss. We faced monstrous foes, some human, others... not. I still hear their voices at times, in the silence of the night.");

                                warModule.AddOption("It sounds like you've been through a lot.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule struggleModule = new DialogueModule("Aye, that I have. The nights are the hardest. I dream of the past, of friends I couldn't save. But I am still here, and I protect this town as best I can. My tinkering is my way of giving back, of finding a small measure of peace.");

                                        struggleModule.AddOption("How do you cope with it all?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule copingModule = new DialogueModule("I stay busy. I tinker, I build, I try to help those in need. The HydroxFluid, for instance, is for a device I hope will protect this place from what lurks beyond. It might not be enough, but it's something. And sometimes, that's all we can do—just keep moving, keep building, keep hoping.");

                                                copingModule.AddOption("You are brave, Zaros. Thank you for your service.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Zaros nods, his expression softening for a moment. 'Thank you, traveler. Your words mean more than you know.'");
                                                        plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, copingModule));
                                            });

                                        struggleModule.AddOption("You are a strong person, Zaros.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Zaros gives a small, appreciative smile. 'Strong, perhaps. But strength isn't always enough.'");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, struggleModule));
                                    });

                                warModule.AddOption("That must be difficult to live with.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Zaros closes his eyes for a moment, as if to push away the memories. 'It is. But we must all carry our burdens, one way or another.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, warModule));
                            });

                        backstoryModule.AddOption("You are haunted by your past, aren't you?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule hauntedModule = new DialogueModule("Haunted... Yes, that's a fitting word. I see their faces, the comrades lost, the innocents I couldn't protect. It never truly leaves you, but I try to turn that pain into something useful. These inventions, these oddities—they're my way of trying to make things right, even if just a little.");

                                hauntedModule.AddOption("It's admirable that you keep trying.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Zaros's eyes glisten for a moment before he turns away. 'Thank you, traveler. It is all I know how to do.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, hauntedModule));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                inventionsModule.AddOption("Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, inventionsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Zaros waves you off, his mind already drifting back to his tinkering.");
            });

        return greeting;
    }

    private bool HasHydroxFluid(PlayerMobile player)
    {
        // Check the player's inventory for HydroxFluid
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HydroxFluid)) != null;
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
        // Remove the HydroxFluid and give the AncientRune and MaxxiaScroll, then set the cooldown timer
        Item hydroxFluid = player.Backpack.FindItemByType(typeof(HydroxFluid));
        if (hydroxFluid != null)
        {
            hydroxFluid.Delete();
            player.AddToBackpack(new AncientRunes());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the HydroxFluid and receive an AncientRune and a MaxxiaScroll in return. Zaros nods approvingly.");
        }
        else
        {
            player.SendMessage("It seems you no longer have HydroxFluid.");
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