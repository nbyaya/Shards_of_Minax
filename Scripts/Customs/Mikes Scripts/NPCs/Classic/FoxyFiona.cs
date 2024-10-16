using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class FoxyFiona : BaseCreature
{
    [Constructable]
    public FoxyFiona() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Foxy Fiona";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(80);

        SetHits(90);

        // Appearance
        AddItem(new LeatherSkirt() { Hue = 2118 });
        AddItem(new FemaleLeatherChest() { Hue = 2118 });
        AddItem(new Boots() { Hue = 2118 });
        AddItem(new Bow() { Name = "Foxy Fiona's Bow" });

        // Equip items
        EquipItem(new LeatherSkirt() { Hue = 2118 });
        EquipItem(new LeatherBustierArms() { Hue = 2118 });
        EquipItem(new Boots() { Hue = 2118 });
        EquipItem(new Bow() { Name = "Foxy Fiona's Bow" });

        Direction = Direction.West;

        SpeechHue = 0; // Default speech hue
    }

    public FoxyFiona(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Foxy Fiona, the animal tamer. What brings you to my humble corner?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I am Foxy Fiona, known far and wide for my skills in taming and caring for animals, even the elusive Moon Cat.");
                whoModule.AddOption("Tell me about the Moon Cat.",
                    p => true,
                    p =>
                    {
                        DialogueModule moonCatModule = new DialogueModule("The Moon Cat is a rare creature, known to be a harbinger of lunar events. I managed to form a bond with one some years ago.");
                        moonCatModule.AddOption("That sounds fascinating!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, moonCatModule));
                    });
                whoModule.AddOption("What else do you do?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("I train and care for animals. Compassion towards animals is a virtue. Do you have compassion?");
                        jobModule.AddOption("Yes, I do.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Animals bring joy and teach us kindness.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        jobModule.AddOption("No, I don't have time for that.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                whoModule.AddOption("Tell me about the magical ferrets of Felucca.",
                    p => true,
                    p =>
                    {
                        DialogueModule ferretModule = new DialogueModule("Ah, the magical ferrets of Felucca! These creatures are truly fascinating. There are several varieties, each with unique magical properties.");
                        ferretModule.AddOption("What are the different varieties?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule varietiesModule = new DialogueModule("There are four main types of magical ferrets found in Felucca: the Shadow Ferret, the Ember Ferret, the Verdant Ferret, and the Lunar Ferret.");
                                varietiesModule.AddOption("Tell me about the Shadow Ferret.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule shadowModule = new DialogueModule("The Shadow Ferret is a nocturnal creature, often seen darting through the darkness. It has the unique ability to blend into the shadows, becoming nearly invisible. Some say it can even travel through shadow portals to other realms.");
                                        shadowModule.AddOption("How do they use their shadow abilities?",
                                            plan => true,
                                            plan =>
                                            {
                                                DialogueModule abilityModule = new DialogueModule("The Shadow Ferret can use its shadow abilities to escape predators or to sneak up on prey. They are highly intelligent and often use their powers to protect themselves and their companions. Many tamers find them to be excellent partners for stealth missions.");
                                                abilityModule.AddOption("That sounds amazing!",
                                                    plann => true,
                                                    plann =>
                                                    {
                                                        plann.SendGump(new DialogueGump(plann, CreateGreetingModule()));
                                                    });
                                                plan.SendGump(new DialogueGump(plan, abilityModule));
                                            });
                                        shadowModule.AddOption("Tell me about another type of ferret.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, varietiesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, shadowModule));
                                    });
                                varietiesModule.AddOption("Tell me about the Ember Ferret.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule emberModule = new DialogueModule("The Ember Ferret is a fiery creature, often found near volcanic regions. It has a warm, glowing fur that can emit small flames. The Ember Ferret can channel fire magic, making it a formidable creature in combat when properly trained.");
                                        emberModule.AddOption("How do they use their fire abilities?",
                                            plan => true,
                                            plan =>
                                            {
                                                DialogueModule fireAbilityModule = new DialogueModule("Ember Ferrets can use their fire abilities to protect themselves or their allies. They can create small fireballs to deter attackers or warm their surroundings during cold nights. Many tamers value their loyalty and their ability to create a protective fire barrier.");
                                                fireAbilityModule.AddOption("Incredible!",
                                                    plann => true,
                                                    plann =>
                                                    {
                                                        plann.SendGump(new DialogueGump(plann, CreateGreetingModule()));
                                                    });
                                                plan.SendGump(new DialogueGump(plan, fireAbilityModule));
                                            });
                                        emberModule.AddOption("Tell me about another type of ferret.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, varietiesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, emberModule));
                                    });
                                varietiesModule.AddOption("Tell me about the Verdant Ferret.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule verdantModule = new DialogueModule("The Verdant Ferret is a creature of the forest, known for its deep connection with nature. Its fur is covered in leaves and moss, allowing it to blend seamlessly into the forest floor. The Verdant Ferret has the ability to accelerate plant growth and even communicate with some plant species.");
                                        verdantModule.AddOption("How do they use their nature abilities?",
                                            plan => true,
                                            plan =>
                                            {
                                                DialogueModule natureAbilityModule = new DialogueModule("The Verdant Ferret can use its nature abilities to heal wounded creatures, summon vines to entangle threats, or even create a safe, hidden grove for rest. They are often sought by tamers who value harmony with nature.");
                                                natureAbilityModule.AddOption("That's wonderful!",
                                                    plann => true,
                                                    plann =>
                                                    {
                                                        plann.SendGump(new DialogueGump(plann, CreateGreetingModule()));
                                                    });
                                                plan.SendGump(new DialogueGump(plan, natureAbilityModule));
                                            });
                                        verdantModule.AddOption("Tell me about another type of ferret.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, varietiesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, verdantModule));
                                    });
                                varietiesModule.AddOption("Tell me about the Lunar Ferret.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule lunarModule = new DialogueModule("The Lunar Ferret is a mystical creature, deeply attuned to the cycles of the moon. Its silver fur seems to glow under moonlight, and it is believed to have the ability to influence lunar magic, affecting tides and even calming nocturnal beasts.");
                                        lunarModule.AddOption("How do they use their lunar abilities?",
                                            plan => true,
                                            plan =>
                                            {
                                                DialogueModule lunarAbilityModule = new DialogueModule("The Lunar Ferret can use its lunar abilities to create calming auras, influence water currents, and even sense the presence of hidden dangers during the night. They are considered wise creatures and are often companions to those who seek knowledge.");
                                                lunarAbilityModule.AddOption("Absolutely fascinating!",
                                                    plann => true,
                                                    plann =>
                                                    {
                                                        plann.SendGump(new DialogueGump(plann, CreateGreetingModule()));
                                                    });
                                                plan.SendGump(new DialogueGump(plan, lunarAbilityModule));
                                            });
                                        lunarModule.AddOption("Tell me about another type of ferret.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, varietiesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, lunarModule));
                                    });
                                varietiesModule.AddOption("Thank you for the information.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, varietiesModule));
                            });
                        ferretModule.AddOption("Why are they considered magical?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule magicModule = new DialogueModule("The ferrets of Felucca are magical because of their connection to the ancient ley lines that run beneath the land. These ley lines infuse them with unique powers, making them more than ordinary creatures.");
                                magicModule.AddOption("How do the ley lines affect them?",
                                    plan => true,
                                    plan =>
                                    {
                                        DialogueModule leyLinesModule = new DialogueModule("The ley lines grant the ferrets their powers, such as manipulating shadows, fire, nature, or lunar energies. The closer they are to a ley line, the stronger their abilities become. Some say the ferrets can even sense the flow of magic, guiding their tamers to hidden places of power.");
                                        leyLinesModule.AddOption("That's remarkable!",
                                            plann => true,
                                            plann =>
                                            {
                                                plann.SendGump(new DialogueGump(plann, CreateGreetingModule()));
                                            });
                                        plan.SendGump(new DialogueGump(plan, leyLinesModule));
                                    });
                                magicModule.AddOption("Thank you for the explanation.",
                                    plan => true,
                                    plan =>
                                    {
                                        plan.SendGump(new DialogueGump(plan, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, magicModule));
                            });
                        ferretModule.AddOption("Thank you, Fiona.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, ferretModule));
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("Can you tell me about your animals?",
            player => true,
            player =>
            {
                DialogueModule animalsModule = new DialogueModule("All my animals are in good health! Especially since I found the Elixir of Vitality. It makes them thrive.");
                animalsModule.AddOption("What is the Elixir of Vitality?",
                    p => true,
                    p =>
                    {
                        DialogueModule elixirModule = new DialogueModule("The Elixir of Vitality is a rare potion that boosts the health and vitality of any creature that consumes it. It wasn't easy to obtain, but it's worth it for the well-being of my animals.");
                        elixirModule.AddOption("That sounds powerful.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, elixirModule));
                    });
                animalsModule.AddOption("Do you train them as well?",
                    p => true,
                    p =>
                    {
                        DialogueModule trainModule = new DialogueModule("Training animals requires patience and understanding, especially the mystical ones like the Whispering Phoenix.");
                        trainModule.AddOption("Tell me about the Whispering Phoenix.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule phoenixModule = new DialogueModule("The Whispering Phoenix is a legendary bird that communicates through soft whispers. Those whispers can reveal truths to those who listen closely.");
                                phoenixModule.AddOption("Incredible!",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, phoenixModule));
                            });
                        trainModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, trainModule));
                    });
                player.SendGump(new DialogueGump(player, animalsModule));
            });

        greeting.AddOption("Do you know about the mantra of Honesty?",
            player => true,
            player =>
            {
                DialogueModule honestyModule = new DialogueModule("Ah, the mantra of Honesty. Its power lies in understanding its syllables. The second syllable is LOR. Remember this; it might guide your path.");
                honestyModule.AddOption("Thank you for the wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, honestyModule));
            });

        greeting.AddOption("Goodbye, Fiona.",
            player => true,
            player =>
            {
                player.SendMessage("Fiona nods at you and returns to her animals.");
            });

        return greeting;
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