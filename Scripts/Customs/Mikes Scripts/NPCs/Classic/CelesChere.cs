using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CelesChere : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CelesChere() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Celes Chere";
        Body = 0x191; // Human female body

        // Stats
        SetStr(95);
        SetDex(85);
        SetInt(110);
        SetHits(80);

        // Appearance
        AddItem(new ChainLegs() { Hue = 1910 });
        AddItem(new ChainChest() { Hue = 1910 });
        AddItem(new ChainCoif() { Hue = 1910 });
        AddItem(new Longsword() { Name = "Celes's Rapier" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Celes Chere. How may I assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a former general of the Empire, skilled in the ways of combat. I've seen both the glories and the shadows of the Empire. But my transformation into a Magi Knight was perhaps the most defining moment of my life.");
                aboutModule.AddOption("What is a Magi Knight?",
                    p => true,
                    p =>
                    {
                        DialogueModule magiKnightModule = new DialogueModule("A Magi Knight is a warrior infused with the power of magic, an experiment conducted by Cid, the Empire's leading scientist. Cid believed that harnessing esper energy could create soldiers of unparalleled might.");
                        magiKnightModule.AddOption("How did Cid transform you?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule transformationModule = new DialogueModule("Cid used a process involving direct exposure to esper energy. Espers are mystical creatures of immense magical power, and their energy can be extracted and channeled into humans. The process was excruciating, as the raw magic coursed through my body, altering me forever. But it granted me abilities beyond those of a mere human.");
                                transformationModule.AddOption("Was it painful?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule painModule = new DialogueModule("Yes, the pain was unimaginable. Every cell in my body felt like it was being torn apart and rebuilt. Many did not survive the process, and those who did were forever changed. I remember screaming until I had no voice left, and even then, the agony continued.");
                                        painModule.AddOption("Why did you go through with it?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule whyModule = new DialogueModule("I did it because I believed it was the only way to protect the people I cared about. The Empire was relentless in its pursuit of power, and I wanted to be strong enough to stand against the darkness I saw growing within it. Cid promised that I could become a force for good, a beacon to lead others away from tyranny.");
                                                whyModule.AddOption("Did you trust Cid?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule trustModule = new DialogueModule("I trusted Cid, though it was complicated. He was a scientist, driven by discovery, and he genuinely believed that he was creating something to help the world. He wasn't evil, but his loyalty to the Empire clouded his judgment. He saw me as both a daughter and as an experiment, and that duality made our relationship complex.");
                                                        trustModule.AddOption("How do you feel about him now?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule feelingModule = new DialogueModule("I have mixed feelings. On one hand, he gave me the power to protect myself and others. On the other, he put me through something no one should endure. I still care for him, but I also recognize the damage he caused by blindly following orders. He tried to do right by me in the end, but the scars remain.");
                                                                feelingModule.AddOption("Thank you for sharing your story.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, feelingModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, trustModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, whyModule));
                                            });
                                        painModule.AddOption("I can't imagine such suffering.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, painModule));
                                    });
                                transformationModule.AddOption("What powers did you gain?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule powersModule = new DialogueModule("The infusion with esper energy granted me abilities to wield powerful magic in addition to my combat skills. I can cast spells that would take normal mages years to master. I can also sense magical auras and draw upon the energy of the world around me. But this power comes at a cost—using it drains my strength, and there are limits to how far I can push myself.");
                                        powersModule.AddOption("What kind of magic can you use?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule magicTypeModule = new DialogueModule("I have a natural affinity for ice magic—perhaps it's reflective of my own nature. I can summon blizzards, create barriers of solid ice, and even freeze my enemies in place. The power is immense, but I must be careful not to lose myself in it.");
                                                magicTypeModule.AddOption("Thank you for explaining.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, magicTypeModule));
                                            });
                                        powersModule.AddOption("Thank you for sharing your abilities.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, powersModule));
                                    });
                                transformationModule.AddOption("Thank you for sharing your story.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, transformationModule));
                            });
                        magiKnightModule.AddOption("Why did Cid conduct these experiments?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule cidModule = new DialogueModule("Cid believed that the power of espers could be harnessed to protect the Empire from its enemies. He wanted to create guardians, powerful enough to deter any threat. In his mind, he was acting for the greater good, though he underestimated the toll it would take on those of us who were experimented on.");
                                cidModule.AddOption("Did anyone else undergo the transformation?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule othersModule = new DialogueModule("Yes, others did. Many did not survive, and some lost their minds in the process. The magic is powerful, but it's also volatile and dangerous. Only a few of us managed to endure and maintain our sanity. I often think of those who didn't make it—their faces still haunt me.");
                                        othersModule.AddOption("I'm sorry to hear that.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, othersModule));
                                    });
                                cidModule.AddOption("Thank you for sharing.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, cidModule));
                            });
                        magiKnightModule.AddOption("Thank you for telling me.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, magiKnightModule));
                    });
                aboutModule.AddOption("Thank you for the information.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What virtues do you follow?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("I believe in the virtues of Honesty, Compassion, and Justice. They guide my actions.");
                virtuesModule.AddOption("Tell me about Honesty.",
                    p => true,
                    p =>
                    {
                        DialogueModule honestyModule = new DialogueModule("Honesty is the foundation of trust. Without it, societies crumble and friendships fade. It's more than just telling the truth; it's living a life of transparency and integrity.");
                        honestyModule.AddOption("Thank you for your insight.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, honestyModule));
                    });
                virtuesModule.AddOption("Tell me about Compassion.",
                    p => true,
                    p =>
                    {
                        DialogueModule compassionModule = new DialogueModule("Compassion is the light that shines in the darkest of times. It is the act of understanding and caring for others, even when it's challenging or inconvenient.");
                        compassionModule.AddOption("Thank you for your wisdom.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, compassionModule));
                    });
                virtuesModule.AddOption("Tell me about Justice.",
                    p => true,
                    p =>
                    {
                        DialogueModule justiceModule = new DialogueModule("Justice is the balance that ensures fairness. It's not just about punishment, but about ensuring that the scales are even for all. In my travels, I've seen justice both served and denied.");
                        justiceModule.AddOption("I understand. Thank you.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, justiceModule));
                    });
                virtuesModule.AddOption("Thank you for sharing your beliefs.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Can you share your story about the potion?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule potionModule = new DialogueModule("Ah, that potion was a rare elixir given to me by a skilled alchemist when I was gravely wounded. In gratitude, I can share a vial with you. Consider it a gift.");
                    potionModule.AddOption("Thank you, I will accept it.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new ResistingSpellsAugmentCrystal());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    potionModule.AddOption("No, thank you.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, potionModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Celes nods at you in farewell.");
            });

        return greeting;
    }

    public CelesChere(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}