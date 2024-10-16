using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AstridTheStarGazer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public AstridTheStarGazer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Astrid the Star-Gazer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(60);
        SetDex(60);
        SetInt(60);
        SetHits(80);

        // Appearance
        AddItem(new Kilt(2126));
        AddItem(new Surcoat(1904));
        AddItem(new Sandals(1904));
        AddItem(new Bandana(2126));
        AddItem(new Sextant { Name = "Astrid's Sextant" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public AstridTheStarGazer(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler. I am Astrid the Star-Gazer, the finest cartographer in this wretched realm. What brings you to my humble observatory?");

        greeting.AddOption("Tell me about your work as a cartographer.",
            player => true,
            player =>
            {
                DialogueModule cartographerModule = new DialogueModule("My \"job,\" if you can call it that, is to map this forsaken land for fools who know nothing of its beauty. By observing the stars, I've uncovered many hidden truths of our realm.");
                cartographerModule.AddOption("What hidden truths have you uncovered?",
                    p => true,
                    p =>
                    {
                        DialogueModule truthsModule = new DialogueModule("The skies tell a tale of the world's fate. If only more people would look up and listen, they would see the signs of this land's decay. There is beauty in the stars, but also sorrow.");
                        truthsModule.AddOption("That sounds fascinating.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        truthsModule.AddOption("I have heard enough for now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, truthsModule));
                    });
                cartographerModule.AddOption("I see. Thank you for sharing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, cartographerModule));
            });

        greeting.AddOption("Can you tell me about the stars?",
            player => true,
            player =>
            {
                DialogueModule starsModule = new DialogueModule("Do you have any inkling of the vastness of this world? Gazing upon the stars, I have found ancient constellations that foretell of events to come. There is one particular constellation that has caught my eye lately. It aligns once every millennium.");
                starsModule.AddOption("What does the constellation signify?",
                    p => true,
                    p =>
                    {
                        DialogueModule eventsModule = new DialogueModule("The constellation's alignment could bring about a great change, perhaps even an omen. If you help me observe it tonight, I might reward you for your assistance.");
                        eventsModule.AddOption("I will help you observe it.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You agree to help Astrid observe the constellation.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        eventsModule.AddOption("I am not interested in stargazing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, eventsModule));
                    });
                starsModule.AddOption("I have no time for stargazing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, starsModule));
            });

        greeting.AddOption("I need help charting a map for a pirate crew.",
            player => true,
            player =>
            {
                DialogueModule pirateModule = new DialogueModule("Ah, a pirate crew, you say? Charting a map for the likes of sea-faring rogues is no easy task. The seas are vast, and the stars are your only guide.");
                pirateModule.AddOption("What do you need to create the map?",
                    p => true,
                    p =>
                    {
                        DialogueModule suppliesModule = new DialogueModule("First, I'll need certain supplies: a fine piece of parchment, ink mixed with the soot of a volcano, and a compass blessed by the sea priests. Without these, even the stars can't help you.");
                        suppliesModule.AddOption("Where can I find these items?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule locationModule = new DialogueModule("The parchment can be bought from a scribe in the capital. The volcanic ink requires a journey to the Ember Isle, a dangerous place filled with fiery creatures. As for the compass, you'll need to seek the blessing of the sea priests who dwell in the Azure Temple on the coast.");
                                locationModule.AddOption("I'll gather these items for you.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You set off to gather the required items for Astrid.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                locationModule.AddOption("That sounds too difficult.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, locationModule));
                            });
                        suppliesModule.AddOption("That sounds like a lot of work.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, suppliesModule));
                    });
                pirateModule.AddOption("What dangers might we face at sea?",
                    p => true,
                    p =>
                    {
                        DialogueModule dangersModule = new DialogueModule("The sea is a fickle mistress. There are tales of sirens who lure sailors to their doom, whirlpools that swallow ships whole, and ghost ships crewed by the damned. Not to mention rival pirate crews who would love nothing more than to take your map by force.");
                        dangersModule.AddOption("Tell me about the ghost ships.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule ghostModule = new DialogueModule("Ghost ships are said to appear on moonless nights, their tattered sails glowing faintly. They are captained by those who betrayed their own crew, cursed to sail the seas for eternity. Beware, for their treasure is tempting, but their wrath is boundless.");
                                ghostModule.AddOption("I'll be cautious, thank you.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                ghostModule.AddOption("I want to know more about their treasure.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule treasureModule = new DialogueModule("The treasure of a ghost ship is said to be both a blessing and a curse. Gold coins minted from realms long forgotten, jewels that shimmer with an otherworldly glow, and maps to places that exist outside of our reality. But remember, touching such treasure could bind your soul to the ship forever.");
                                        treasureModule.AddOption("That sounds too risky for me.",
                                            p2 => true,
                                            p2 =>
                                            {
                                                p2.SendGump(new DialogueGump(p2, CreateGreetingModule()));
                                            });
                                        treasureModule.AddOption("I'm willing to take the risk.",
                                            p2 => true,
                                            p2 =>
                                            {
                                                p2.SendMessage("You feel a chill as Astrid warns you of the dangers ahead.");
                                                p2.SendGump(new DialogueGump(p2, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, treasureModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, ghostModule));
                            });
                        dangersModule.AddOption("I think I've heard enough of these dangers.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, dangersModule));
                    });
                pirateModule.AddOption("I don't need a map after all.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, pirateModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    noRewardModule.AddOption("I understand.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("The reward I offer isn't materialistic, but knowledge. Knowledge that might guide you on your adventures and perhaps even save your life one day. But take this token.");
                    rewardModule.AddOption("Thank you for the knowledge.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Astrid nods at you, her eyes still fixed on the distant stars.");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}