using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DrZappa : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DrZappa() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dr. Zappa";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(60);
        SetInt(100);
        SetHits(50);

        // Appearance
        AddItem(new Robe() { Hue = 1181 });
        AddItem(new PlateHelm() { Hue = 1181 });
        AddItem(new LeatherGloves() { Hue = 1181 });
        AddItem(new Shoes() { Hue = 1181 });
        AddItem(new FireballWand() { Name = "Dr. Zappa's Pistol" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Dr. Zappa, the scientist. What brings you to my humble corner of experimentation?");

        greeting.AddOption("Tell me about your work.",
            player => true,
            player =>
            {
                DialogueModule workModule = new DialogueModule("My job is to unravel the mysteries of the universe through science and experimentation! It's a pursuit of knowledge that often leads to explosive discoveries, sometimes literally. Lately, I've been working on something rather peculiar: generating electricity in this medieval, magical realm. It has proven to be quite the challenge.");
                workModule.AddOption("Generating electricity? How is that possible here?",
                    p => true,
                    p =>
                    {
                        DialogueModule electricityModule = new DialogueModule("Ah, yes! Electricity—it's an incredibly powerful force, yet elusive in this land dominated by magic. I have experimented with various methods, such as using copper wires, magnets, and even arcane crystals to induce current. But, it's not as simple as back in more technologically advanced times.");
                        electricityModule.AddOption("What challenges have you faced?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule challengesModule = new DialogueModule("The biggest challenge is the lack of consistent materials. Copper is scarce, and the arcane crystals I need are very unstable. Not to mention, the interference from the natural magic of this realm often disrupts my experiments. It's as if magic itself is resisting my attempts at harnessing electricity.");
                                challengesModule.AddOption("How do you manage the magical interference?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule interferenceModule = new DialogueModule("I've been attempting to create a hybrid solution—combining alchemical components with traditional conductive materials. For instance, I've tried coating copper wires with an alchemical potion to insulate them from magical energy. It works... sometimes. But more often than not, the potion either wears off or reacts unpredictably with the magic in the air.");
                                        interferenceModule.AddOption("Have you had any success?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule successModule = new DialogueModule("A few small successes, yes! I managed to light a small bulb for a brief moment. It was an incredible sight—a warm, steady glow in the midst of all my failures. But keeping it stable is another matter entirely. The natural magic fluctuations in the area cause the light to flicker and often short out.");
                                                successModule.AddOption("That must be frustrating.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule frustrationModule = new DialogueModule("Oh, you have no idea! It feels like two steps forward and three steps back. But, every failure is also a lesson. I believe that one day, I'll figure out how to balance magic and science in harmony. The key might lie in understanding the natural ley lines of magic that crisscross the land.");
                                                        frustrationModule.AddOption("Ley lines? Tell me more.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule leyLinesModule = new DialogueModule("Ley lines are essentially rivers of magic that flow beneath the surface of the world. They are incredibly powerful, and tapping into them could potentially provide a stable power source. However, it's risky—if I miscalculate, I could cause a magical backlash that might destroy everything around me.");
                                                                leyLinesModule.AddOption("That sounds dangerous. Why take the risk?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule riskModule = new DialogueModule("Because the potential reward is too great to ignore! Imagine a world where we can blend the wonders of technology with the magic that already exists here. It could revolutionize life for everyone—better tools, better healing methods, even better defenses against the dangers that lurk in the wilds. But, yes, it's a perilous path.");
                                                                        riskModule.AddOption("I admire your determination.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, riskModule));
                                                                    });
                                                                leyLinesModule.AddOption("Perhaps there's another way to harness power?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule otherWaysModule = new DialogueModule("I've considered other methods, such as wind or water power. The challenge there is converting that energy into something usable for my experiments. Without the right components—like gears, belts, and proper storage devices—it's difficult to harness it effectively. But I haven't given up yet.");
                                                                        otherWaysModule.AddOption("I wish you luck in your endeavors.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, otherWaysModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, leyLinesModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, frustrationModule));
                                                    });
                                                successModule.AddOption("Could I assist in some way?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule assistModule = new DialogueModule("You would assist me? That would be fantastic! I could use help gathering more arcane crystals or perhaps testing different alchemical insulators. Be warned, it's not without its dangers. These experiments can be volatile.");
                                                        assistModule.AddOption("I'm willing to help.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("You agree to assist Dr. Zappa in his experiments.");
                                                            });
                                                        assistModule.AddOption("I'm not sure I'm ready for that.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, assistModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, successModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, interferenceModule));
                                    });
                                challengesModule.AddOption("It must be hard to find those materials.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule materialsModule = new DialogueModule("Oh, absolutely. Copper is rare, and the arcane crystals are even harder to come by. Most are guarded by dangerous creatures or located in treacherous locations. I've had to hire adventurers like yourself to gather them, but not everyone returns from those expeditions.");
                                        materialsModule.AddOption("I'll keep an eye out for materials.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You agree to keep an eye out for the materials Dr. Zappa needs.");
                                            });
                                        materialsModule.AddOption("That sounds too dangerous for me.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, materialsModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, challengesModule));
                            });
                        p.SendGump(new DialogueGump(p, electricityModule));
                    });
                workModule.AddOption("What drives you to experiment?",
                    p => true,
                    p =>
                    {
                        DialogueModule driveModule = new DialogueModule("Every quest begins with a question, a curiosity to uncover the unknown. I seek to understand the very fabric of reality!");
                        driveModule.AddOption("That's admirable.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, driveModule));
                    });
                workModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, workModule));
            });

        greeting.AddOption("Can you teach me about science?",
            player => true,
            player =>
            {
                DialogueModule teachModule = new DialogueModule("Science is a pursuit of knowledge and understanding, just as valor is the pursuit of courage. Do you seek knowledge?");
                teachModule.AddOption("Yes, I seek knowledge.",
                    p => true,
                    p =>
                    {
                        DialogueModule knowledgeModule = new DialogueModule("Knowledge is the foundation of progress. Keep seeking it, and you shall find answers!");
                        knowledgeModule.AddOption("Thank you for the encouragement.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, knowledgeModule));
                    });
                teachModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, teachModule));
            });

        greeting.AddOption("Do you need assistance?",
            player => true,
            player =>
            {
                DialogueModule assistanceModule = new DialogueModule("Ah, so you're interested in assisting me? Very well, I could always use help with my experiments.");
                assistanceModule.AddOption("What do you need?",
                    p => true,
                    p =>
                    {
                        DialogueModule taskModule = new DialogueModule("I need someone to gather some rare components. If you could bring me Essence of the Phoenix, it would greatly aid my research.");
                        taskModule.AddOption("I'll bring you the Essence.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You set off to gather the Essence of the Phoenix.");
                            });
                        taskModule.AddOption("That sounds too dangerous.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, taskModule));
                    });
                assistanceModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, assistanceModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Dr. Zappa nods to you, a glimmer of curiosity still in his eyes.");
            });

        return greeting;
    }

    public DrZappa(Serial serial) : base(serial) { }

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