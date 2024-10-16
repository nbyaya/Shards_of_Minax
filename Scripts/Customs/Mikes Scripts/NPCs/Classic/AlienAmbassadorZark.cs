using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Alien Ambassador Zark")]
public class AlienAmbassadorZark : BaseCreature
{
    [Constructable]
    public AlienAmbassadorZark() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Alien Ambassador Zark";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(90);
        SetInt(130);
        SetHits(70);

        // Appearance
        AddItem(new Robe(2400)); // Robe with Hue 4400
        AddItem(new BlackStaff { Name = "Zark's Energy Staff" });

        SpeechHue = 0; // Default speech hue
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
        DialogueModule greeting = new DialogueModule("Greetings, I am Alien Ambassador Zark, representing the interstellar council. How may I assist you?");

        greeting.AddOption("Tell me about your council.",
            player => true,
            player =>
            {
                DialogueModule councilModule = new DialogueModule("The interstellar council comprises various species from distant galaxies, working together to ensure peace and prosperity in the cosmos. We meet regularly to discuss universal matters.");
                
                councilModule.AddOption("What species are part of the council?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule speciesModule = new DialogueModule("There are countless species, including the wise Aeloxians, the resilient Yarnarians, and the enigmatic Crystallans. Each brings unique perspectives and knowledge.");
                        
                        speciesModule.AddOption("Tell me more about the Aeloxians.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule aeloxianModule = new DialogueModule("The Aeloxians are beings of pure energy, known for their profound wisdom and serene nature. They often serve as mediators in council discussions.");
                                aeloxianModule.AddOption("How do they communicate?",
                                    p => true,
                                    p =>
                                    {
                                        p.SendGump(new DialogueGump(p, aeloxianModule));
                                    });
                                speciesModule.AddOption("What do you value in your council?",
                                    pls => true,
                                    pls =>
                                    {
                                        DialogueModule valuesModule = new DialogueModule("We value kindness, understanding, and the pursuit of knowledge. The universal language of kindness resonates across galaxies.");
                                        valuesModule.AddOption("I see, that's enlightening.",
                                            p => true,
                                            p =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        pl.SendGump(new DialogueGump(pl, valuesModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, aeloxianModule));
                            });
                        speciesModule.AddOption("What are the Yarnarians like?",
                            plw => true,
                            plw =>
                            {
                                DialogueModule yarnarianModule = new DialogueModule("Yarnarians are known for their resilience and craftsmanship. They have a deep connection with the materials of their world, using them to create stunning artifacts.");
                                yarnarianModule.AddOption("That sounds fascinating.",
                                    p => true,
                                    p =>
                                    {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, yarnarianModule));
                            });
                        pl.SendGump(new DialogueGump(pl, speciesModule));
                    });

                councilModule.AddOption("How do you ensure peace?",
                    playerd => true,
                    playerd =>
                    {
                        DialogueModule peaceModule = new DialogueModule("We utilize a combination of diplomacy, cultural exchanges, and sometimes even joint missions to foster understanding and harmony among species.");
                        peaceModule.AddOption("What kind of missions?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule missionsModule = new DialogueModule("Joint missions can range from exploration endeavors to research projects aimed at solving interstellar challenges, such as resource scarcity or health crises.");
                                missionsModule.AddOption("That's interesting! Can you tell me about a specific mission?",
                                    p => true,
                                    p =>
                                    {
                                        DialogueModule specificMissionModule = new DialogueModule("One notable mission involved a collaboration with the Aeloxians to explore a newly discovered planet rich in rare minerals. We aimed to assess its viability for habitation.");
                                        specificMissionModule.AddOption("What were the results?",
                                            plt => true,
                                            plt =>
                                            {
                                                plt.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, specificMissionModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, missionsModule));
                            });
                        player.SendGump(new DialogueGump(player, peaceModule));
                    });

                player.SendGump(new DialogueGump(player, councilModule));
            });

        greeting.AddOption("What is your role?",
            player => true,
            player =>
            {
                DialogueModule roleModule = new DialogueModule("My role is to foster diplomacy and understanding between our two worlds. It requires keen insight and the ability to navigate complex cultural landscapes.");
                
                roleModule.AddOption("What challenges do you face?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule challengesModule = new DialogueModule("The greatest challenges often arise from misunderstandings. Different cultures have unique values and norms, which can lead to friction if not approached with care.");
                        challengesModule.AddOption("How do you overcome these challenges?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, challengesModule));
                    });
                player.SendGump(new DialogueGump(player, roleModule));
            });

        greeting.AddOption("Can you demonstrate your technology?",
            player => true,
            player =>
            {
                DialogueModule techModule = new DialogueModule("Watch closely. [Ambassador Zark produces a small device, and with a press, a holographic universe unfolds before your eyes]. This is but a fraction of what we have explored.");
                techModule.AddOption("What is this device?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule deviceModule = new DialogueModule("This is a portable universe map, a tool for exploration and understanding. It allows one to visualize the cosmos in stunning detail.");
                        deviceModule.AddOption("Can I keep it?",
                            p => true,
                            p =>
                            {
                                p.AddToBackpack(new TailoringAugmentCrystal()); // Adjust item type as needed
                                p.SendGump(new DialogueGump(p, deviceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, deviceModule));
                    });
                player.SendGump(new DialogueGump(player, techModule));
            });

        greeting.AddOption("What do you think about your world?",
            player => true,
            player =>
            {
                DialogueModule worldModule = new DialogueModule("Your world is unique, filled with its own challenges and joys. It reminds me of a planet in the Andromeda sector, rich in biodiversity and cultural heritage.");
                
                worldModule.AddOption("What can you tell me about Andromeda?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule andromedaModule = new DialogueModule("Andromeda is home to many fascinating civilizations. One in particular, the Seraphim, are known for their breathtaking architecture and harmonious living with nature.");
                        andromedaModule.AddOption("I would love to visit one day.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, andromedaModule));
                    });
                player.SendGump(new DialogueGump(player, worldModule));
            });

        greeting.AddOption("What are your thoughts on technology?",
            player => true,
            player =>
            {
                DialogueModule techThoughtsModule = new DialogueModule("Technology, when used wisely, can bridge gaps and foster understanding. However, it also poses risks if not handled responsibly.");
                
                techThoughtsModule.AddOption("What kind of risks?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule riskModule = new DialogueModule("The greatest risk is the potential for misuse. Advanced technology can become a tool for oppression rather than liberation if in the wrong hands.");
                        riskModule.AddOption("That's a valid concern.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, riskModule));
                    });
                player.SendGump(new DialogueGump(player, techThoughtsModule));
            });

        greeting.AddOption("Tell me about your travels.",
            player => true,
            player =>
            {
                DialogueModule travelModule = new DialogueModule("Traveling across galaxies has opened my eyes to the diversity of life and culture. I've seen planets where the skies shimmer with colors unknown to your world.");
                
                travelModule.AddOption("What was your most memorable trip?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule memorableTripModule = new DialogueModule("One of my most memorable trips was to the planet Quorath, known for its crystalline forests and luminescent flora. The beauty was breathtaking, and the inhabitants welcomed us warmly.");
                        memorableTripModule.AddOption("Sounds enchanting!",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, memorableTripModule));
                    });
                player.SendGump(new DialogueGump(player, travelModule));
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }

    public AlienAmbassadorZark(Serial serial) : base(serial) { }
}
