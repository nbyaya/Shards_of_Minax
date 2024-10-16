using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class NolaraTheWanderer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NolaraTheWanderer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Nolara the Wanderer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 100;
        Karma = 200;

        // Outfit
        AddItem(new Cloak(1153)); // Deep blue cloak
        AddItem(new ThighBoots(1175)); // Light green boots
        AddItem(new FeatheredHat(1157)); // A unique hat with a feather
        AddItem(new TribalHelm()); // Her signature spear that symbolizes her wanderer roots

        VirtualArmor = 20;
    }

    public NolaraTheWanderer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Nolara, a wanderer of distant lands. I've seen much and learned more. Perhaps I could share some tales, or maybe you're here for something else?");

        // Storytelling options
        greeting.AddOption("Tell me about your travels.",
            p => true,
            p =>
            {
                DialogueModule travelModule = new DialogueModule("Ah, the world is full of mysteries! I have seen mountains that touch the sky, forests teeming with spirits, and deserts that whisper secrets in the wind. There are wonders like the Shimmering Sands and the Midnight Grove. Which story intrigues you?");
                
                travelModule.AddOption("Tell me about the Shimmering Sands.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule sandsModule = new DialogueModule("The Shimmering Sands lie far to the east, a desert that glows under the moonlight. It's said that long ago, a star fell there, scattering its light into the sands. Now, the dunes glitter, and travelers say the sands have the power to show glimpses of forgotten memories.");
                        sandsModule.AddOption("Fascinating, tell me more!",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moreSandsModule = new DialogueModule("Legend has it that those who stand upon the highest dune at midnight may see visions of their future or even unlock powers that lie dormant within them. But beware, the desert is also haunted by spectral guardians who do not take kindly to intruders.");
                                moreSandsModule.AddOption("What are these spectral guardians?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule guardiansModule = new DialogueModule("The spectral guardians are restless spirits bound to protect the secrets of the Shimmering Sands. Some say they were once explorers who became too greedy, seeking power from the fallen star. They are drawn to those who show fear or ill intentions, making it crucial to keep your heart pure while traversing the dunes.");
                                        guardiansModule.AddOption("How can one avoid them?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule avoidModule = new DialogueModule("To avoid the guardians, one must walk with respect and humility. They respond to acts of kindness and are repelled by the sound of wind chimes, which some travelers carry as protection. The guardians only appear to those who threaten the balance of the desert.");
                                                avoidModule.AddOption("That's very helpful, thank you.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, avoidModule));
                                            });
                                        guardiansModule.AddOption("I think I'll stay away from that place.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, guardiansModule));
                                    });
                                moreSandsModule.AddOption("Thank you for the story.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, moreSandsModule));
                            });
                        sandsModule.AddOption("Perhaps another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, sandsModule));
                    });

                travelModule.AddOption("Tell me about the Midnight Grove.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule groveModule = new DialogueModule("The Midnight Grove is a forest that only appears during the darkest nights. Hidden deep within the forest are herbs with powerful medicinal properties. Some say the grove was created by an ancient order of healers who wanted to protect their knowledge from those who would misuse it.");
                        groveModule.AddOption("What kind of herbs grow there?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule herbsModule = new DialogueModule("The grove contains rare herbs like the Shadowbloom and the Nightroot. The Shadowbloom glows faintly in the dark and is used to cure ailments of the mind, while the Nightroot can strengthen the body against mutations. However, harvesting these herbs is not easy, as the grove is protected by enchanted animals.");
                                herbsModule.AddOption("Enchanted animals? Tell me more.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule animalsModule = new DialogueModule("Yes, the animals of the Midnight Grove are unlike any others. There are luminescent deer that can vanish in the blink of an eye, and foxes with eyes that see into your very soul. They judge the intentions of those who enter the grove. If you are pure of heart, they may guide you. If not, they will lead you astray until you are lost forever.");
                                        animalsModule.AddOption("How do you prove your intentions?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule intentionsModule = new DialogueModule("The key is to enter the grove with a selfless purpose. Those who seek the herbs for greed or power will find the grove endlessly shifting, and they will be unable to leave. Carrying a token of kindness, like a gift from a loved one, may also help prove your good intentions.");
                                                intentionsModule.AddOption("Thank you for the advice.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, intentionsModule));
                                            });
                                        animalsModule.AddOption("I'll keep that in mind.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, animalsModule));
                                    });
                                herbsModule.AddOption("Thank you, Nolara.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, herbsModule));
                            });
                        groveModule.AddOption("That sounds too dangerous for me.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, groveModule));
                    });

                // Introduce the trade option
                travelModule.AddOption("Do you need anything, Nolara?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in search of a rare item known as the MasterShrubbery. It has properties vital to my research. If you happen to have one, I can offer you a Tribal Helm and a Maxxia Scroll in return. However, I can only trade once every 10 minutes.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a MasterShrubbery with you?");
                                tradeModule.AddOption("Yes, I have a MasterShrubbery.",
                                    plaa => HasMasterShrubbery(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasMasterShrubbery(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a MasterShrubbery.");
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
                        tradeIntroductionModule.AddOption("Perhaps another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, travelModule));
            });

        // Backstory - Scientist and Escape from Research Facility
        greeting.AddOption("You seem to know a lot about rare herbs and places. What's your story?",
            p => true,
            p =>
            {
                DialogueModule storyModule = new DialogueModule("I wasn't always a wanderer, you know. Once, I was part of a research facility. I worked with other brilliant minds, trying to unlock the secrets of mutations and their cure. It wasn't easy work, and it wasn't always ethical. That's why I left.");
                
                storyModule.AddOption("What happened at the research facility?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule facilityModule = new DialogueModule("The facility was hidden away, far from prying eyes. We were tasked with finding a cure for mutations, but the methods... the methods became cruel. Experiments were conducted on unwilling subjects, and I could no longer stand by and watch innocent lives being used like that.");
                        facilityModule.AddOption("That sounds terrible. How did you escape?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule escapeModule = new DialogueModule("One night, I made up my mind. I stole some research notes and a few key items, and I ran. The facility had guards, but I had planned well. I knew their patrol schedules, and I knew where the blind spots were. It wasn't easy, but I managed to slip away into the night.");
                                escapeModule.AddOption("What are you doing now?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule nowModule = new DialogueModule("Now, I continue my research, but on my own terms. I want to find a cure for mutations, but I refuse to harm others in the process. It's a harder path, but it's the right one. I still have the research notes, and every day, I get a little closer to a breakthrough.");
                                        nowModule.AddOption("I admire your dedication.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        nowModule.AddOption("Do you need any help with your research?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("If you are truly willing to help, there are a few items I need for my experiments. The MasterShrubbery is one of them. It's crucial for my studies, as it contains properties that could help stabilize the mutations.");
                                                helpModule.AddOption("I'll keep an eye out for it.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        plaabc.SendMessage("Thank you, traveler. Your help means more to me than you know.");
                                                        plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                                    });
                                                helpModule.AddOption("I'm not sure I can help right now.",
                                                    plaabc => true,
                                                    plaabc =>
                                                    {
                                                        plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, helpModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, nowModule));
                                    });
                                escapeModule.AddOption("That must have been difficult.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, escapeModule));
                            });
                        facilityModule.AddOption("I can't imagine what that was like.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, facilityModule));
                    });
                
                storyModule.AddOption("You did the right thing by leaving.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, storyModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Nolara.",
            p => true,
            p =>
            {
                p.SendMessage("Nolara smiles and nods as you part ways.");
            });

        return greeting;
    }

    private bool HasMasterShrubbery(PlayerMobile player)
    {
        // Check the player's inventory for MasterShrubbery
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MasterShrubbery)) != null;
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
        // Remove the MasterShrubbery and give the TribalHelm and MaxxiaScroll, then set the cooldown timer
        Item masterShrubbery = player.Backpack.FindItemByType(typeof(MasterShrubbery));
        if (masterShrubbery != null)
        {
            masterShrubbery.Delete();
            player.AddToBackpack(new TribalHelm());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MasterShrubbery and receive a Tribal Helm and Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a MasterShrubbery.");
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