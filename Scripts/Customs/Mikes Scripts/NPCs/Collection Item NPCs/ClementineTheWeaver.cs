using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ClementineTheWeaver : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ClementineTheWeaver() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Clementine the Weaver";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(45);
        SetDex(55);
        SetInt(90);

        SetHits(70);
        SetMana(120);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1173)); // A light purple shirt
        AddItem(new LongPants(609)); // Pants with a subtle blue hue
        AddItem(new Sandals(1150)); // Sandals with a pale pink hue
        AddItem(new FloppyHat(1133)); // A lavender floppy hat
        AddItem(new SewingKit()); // Her reward item also in her inventory, making it stealable

        VirtualArmor = 8;
    }

    public ClementineTheWeaver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Oh, hello there dearie! I'm Clementine, a weaver of stories and threads alike. Have you brought something cozy for me today?");

        // Start with some background
        greeting.AddOption("Who are you, Clementine?", 
            p => true, 
            p =>
            {
                DialogueModule aboutModule = new DialogueModule("I am but a simple weaver, stitching tales and textiles from the threads of life. My grandmother taught me the craft, and I carry on her legacy with every thread I spin.");
                aboutModule.AddOption("That sounds wonderful.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule deeperAboutModule = new DialogueModule("Yes, my grandmother was a methodical woman, reserved but passionate. She taught me not only how to weave fabric but also how to weave patience into every detail. It was not just about creating, but about understanding the technology of our tools and the nature of our materials.");
                        deeperAboutModule.AddOption("How do you apply this now?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule weavingTechModule = new DialogueModule("I apply it by understanding the intricacies of each loom, each thread, and each process. You see, weaving is much like engineering. Every single action has a purpose, and every piece of cloth is a mechanism that functions only when each thread is in perfect alignment. It requires precision and a deep focus on the details.");
                                weavingTechModule.AddOption("That's fascinating. Are there any other similarities?",
                                    plaab => true, 
                                    plaab =>
                                    {
                                        DialogueModule engineeringComparisonModule = new DialogueModule("Indeed! You might be surprised to know that I once studied to be an engineer—an aerospace engineer, to be precise. Spacecraft design was my first passion, and I carry that same detailed-oriented mindset to my weaving. Designing a shuttle is no different than designing a tapestry; both require structure, foresight, and a well-planned execution.");
                                        engineeringComparisonModule.AddOption("Spacecraft? That's incredible!", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                DialogueModule spacecraftModule = new DialogueModule("Yes, spacecraft. I specialized in creating reliable, advanced designs that required little maintenance but were full of hidden intricacies. The propulsion systems, the communication modules—everything had to be perfect. It was thrilling work, but it also made me quite reserved and perhaps a bit... disconnected from people.");
                                                spacecraftModule.AddOption("Why did you leave that work?", 
                                                    plaabcd => true, 
                                                    plaabcd =>
                                                    {
                                                        DialogueModule leavingEngineeringModule = new DialogueModule("It became... difficult. I struggled with the social side of things. My mind worked so well with technology, with the order and predictability of machinery, but with people, I felt lost. It became overwhelming, and I sought refuge in something simpler. Weaving provided me that sense of control and purpose once more.");
                                                        leavingEngineeringModule.AddOption("I understand. I'm glad you found weaving.", 
                                                            plaabcde => true, 
                                                            plaabcde =>
                                                            {
                                                                plaabcde.SendGump(new DialogueGump(plaabcde, CreateGreetingModule(plaabcde)));
                                                            });
                                                        plaabcd.SendGump(new DialogueGump(plaabcd, leavingEngineeringModule));
                                                    });
                                                spacecraftModule.AddOption("That must have been tough.", 
                                                    plaabcd => true, 
                                                    plaabcd =>
                                                    {
                                                        plaabcd.SendGump(new DialogueGump(plaabcd, CreateGreetingModule(plaabcd)));
                                                    });
                                                plaabc.SendGump(new DialogueGump(plaabc, spacecraftModule));
                                            });
                                        engineeringComparisonModule.AddOption("I never thought weaving could be like engineering.", 
                                            plaabq => true, 
                                            plaabq =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, engineeringComparisonModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, weavingTechModule));
                            });
                        deeperAboutModule.AddOption("It sounds like a careful practice.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, deeperAboutModule));
                    });
                aboutModule.AddOption("Tell me more about your grandmother.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule grandmotherModule = new DialogueModule("My grandmother was the embodiment of patience. She always told me that a single mistake could unravel an entire piece, and that diligence and repetition were keys to success. She was a perfectionist, and her weaving was known far and wide for its quality.");
                        grandmotherModule.AddOption("She sounds like a great teacher.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        grandmotherModule.AddOption("Did she also work with technology?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule technologyModule = new DialogueModule("Not in the way I did, no. She was more attuned to the natural rhythms of the world—using old looms, taking inspiration from nature. But her influence is what led me into more advanced forms of crafting. My path diverged into engineering, but her values of precision and patience always stayed with me.");
                                technologyModule.AddOption("It all comes full circle, doesn't it?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, technologyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, grandmotherModule));
                    });
                p.SendGump(new DialogueGump(p, aboutModule));
            });

        // Trade option
        greeting.AddOption("Do you need anything in particular?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Ah, yes! I'm in need of GrandmasKnitting for a special project. If you have it, I could offer you a BonFire in return, along with a special MaxxiaScroll as a token of my appreciation.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have GrandmasKnitting for me?");
                        tradeModule.AddOption("Yes, I have GrandmasKnitting.", 
                            plaa => HasGrandmasKnitting(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have it right now.", 
                            plaa => !HasGrandmasKnitting(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have GrandmasKnitting, dearie.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
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
        greeting.AddOption("Goodbye, Clementine.", 
            p => true, 
            p =>
            {
                p.SendMessage("Clementine waves warmly at you as you leave.");
            });

        return greeting;
    }

    private bool HasGrandmasKnitting(PlayerMobile player)
    {
        // Check the player's inventory for GrandmasKnitting
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GrandmasKnitting)) != null;
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
        // Remove the GrandmasKnitting and give the BonFire and MaxxiaScroll, then set the cooldown timer
        Item grandmasKnitting = player.Backpack.FindItemByType(typeof(GrandmasKnitting));
        if (grandmasKnitting != null)
        {
            grandmasKnitting.Delete();
            player.AddToBackpack(new BonFire());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the GrandmasKnitting and receive a BonFire and a MaxxiaScroll in return. Thank you, dearie!");
        }
        else
        {
            player.SendMessage("It seems you no longer have GrandmasKnitting.");
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