using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of Terra Branford")]
public class TerraBranford : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TerraBranford() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Terra Branford";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(120);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 2049 }); // Robe with specified hue
        AddItem(new Boots() { Hue = 1153 }); // Boots with specified hue
        AddItem(new QuarterStaff() { Name = "Terra's Staff" }); // Staff with specific name

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this); // Hair based on gender
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("I am Terra Branford, torn between two worlds. How may I assist you?");

        greeting.AddOption("Tell me about your time under the Empire.",
            player => true,
            player => 
            {
                DialogueModule empireModule = new DialogueModule("My time under the Empire was a dark chapter. I was used as a weapon, my magic twisted to serve their cruel ambitions.");
                empireModule.AddOption("What do you mean by a weapon?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule weaponModule = new DialogueModule("They sought to harness my magical powers, forcing me to unleash destruction upon those who stood against them.");
                        weaponModule.AddOption("That sounds horrifying. How did you feel?",
                            p => true,
                            p => 
                            {
                                DialogueModule feelingModule = new DialogueModule("It felt like being trapped in a nightmare, powerless to control my own fate. I was both a pawn and a monster.");
                                feelingModule.AddOption("Did you ever manage to escape?",
                                    plq => true,
                                    plq => 
                                    {
                                        DialogueModule escapeModule = new DialogueModule("It wasn't until I met allies who believed in me that I found the strength to break free from their chains.");
                                        escapeModule.AddOption("Who were these allies?",
                                            pw => true,
                                            pw => 
                                            {
                                                DialogueModule alliesModule = new DialogueModule("There was a brave group of rebels who fought against the Empire's tyranny. They helped me see that I was more than a mere weapon.");
                                                alliesModule.AddOption("What did they teach you?",
                                                    ple => true,
                                                    ple => 
                                                    {
                                                        DialogueModule teachingsModule = new DialogueModule("They taught me about hope, friendship, and the importance of fighting for one's own destiny.");
                                                        teachingsModule.AddOption("It sounds like they saved you.",
                                                            pr => true,
                                                            pr => 
                                                            {
                                                                p.SendGump(new DialogueGump(p, new DialogueModule("Indeed. Without them, I would still be lost in despair.")));
                                                            });
                                                        p.SendGump(new DialogueGump(p, teachingsModule));
                                                    });
                                                p.SendGump(new DialogueGump(p, alliesModule));
                                            });
                                        p.SendGump(new DialogueGump(p, escapeModule));
                                    });
                                p.SendGump(new DialogueGump(p, feelingModule));
                            });
                        player.SendGump(new DialogueGump(player, weaponModule));
                    });

                player.SendGump(new DialogueGump(player, empireModule));
            });

        greeting.AddOption("What was Kefka like?",
            player => true,
            player =>
            {
                DialogueModule kefkaModule = new DialogueModule("Kefka was a madman, a jester who reveled in chaos. He derived pleasure from the suffering of others.");
                kefkaModule.AddOption("How did he control you?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule controlModule = new DialogueModule("He exploited my vulnerabilities, manipulating my emotions to ensure my loyalty to the Empire. His laughter still haunts me.");
                        controlModule.AddOption("Did you ever defy him?",
                            p => true,
                            p =>
                            {
                                DialogueModule defyModule = new DialogueModule("I did, but it cost me dearly. Each act of defiance was met with brutal consequences, a reminder of who held the power.");
                                defyModule.AddOption("That sounds terrifying.",
                                    pla => true,
                                    pla => 
                                    {
                                        pla.SendGump(new DialogueGump(pla, new DialogueModule("It was, but I learned to channel that fear into strength.")));
                                    });
                                p.SendGump(new DialogueGump(p, defyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, controlModule));
                    });

                player.SendGump(new DialogueGump(player, kefkaModule));
            });

        greeting.AddOption("Do you ever think about your past?",
            player => true,
            player => 
            {
                DialogueModule pastModule = new DialogueModule("Every day. My past shapes who I am, but it also reminds me of the strength I've gained.");
                pastModule.AddOption("What do you remember most?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule memoryModule = new DialogueModule("The faces of those I lost haunt me, but I also remember the moments of kindness amidst the darkness.");
                        memoryModule.AddOption("What kind of kindness?",
                            p => true,
                            p =>
                            {
                                DialogueModule kindnessModule = new DialogueModule("Even in the depths of despair, there were those who reached out, showing me that love and compassion still existed.");
                                kindnessModule.AddOption("I’m glad you found that.",
                                    pla => true,
                                    pla => 
                                    {
                                        pla.SendGump(new DialogueGump(pla, new DialogueModule("Me too. It fuels my resolve to help others find their own light.")));
                                    });
                                p.SendGump(new DialogueGump(p, kindnessModule));
                            });
                        pl.SendGump(new DialogueGump(pl, memoryModule));
                    });
                player.SendGump(new DialogueGump(player, pastModule));
            });

        greeting.AddOption("Do you have any regrets?",
            player => true,
            player => 
            {
                DialogueModule regretModule = new DialogueModule("Regrets? Yes, but I've learned to forgive myself for the choices made under duress.");
                regretModule.AddOption("What do you wish you could change?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule changeModule = new DialogueModule("I wish I could have saved more lives, but I also understand that I was fighting against overwhelming odds.");
                        changeModule.AddOption("You did your best.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Thank you for saying that. It’s important to remember that even small acts of bravery matter.")));
                            });
                        pl.SendGump(new DialogueGump(pl, changeModule));
                    });
                player.SendGump(new DialogueGump(player, regretModule));
            });

        greeting.AddOption("What do you seek now?",
            player => true,
            player => 
            {
                DialogueModule seekModule = new DialogueModule("Now, I seek peace and a way to atone for my past. Helping others is a path I want to walk.");
                seekModule.AddOption("How can I help you?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule helpModule = new DialogueModule("Your kindness is appreciated. Just sharing your story or fighting for justice in your own way is enough.");
                        helpModule.AddOption("I will remember that.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Thank you. Together, we can create a brighter future.")));
                            });
                        pl.SendGump(new DialogueGump(pl, helpModule));
                    });
                player.SendGump(new DialogueGump(player, seekModule));
            });

        return greeting;
    }

    public TerraBranford(Serial serial) : base(serial) { }

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
