using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EngineerLeo : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EngineerLeo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Engineer Leo";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(58);
        SetInt(100);

        SetHits(68);

        // Appearance
        AddItem(new LongPants() { Hue = 1108 });
        AddItem(new BodySash() { Hue = 1132 });
        AddItem(new Boots() { Hue = 1105 });
        AddItem(new Cap() { Hue = 1902 });

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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler. I am Engineer Leo, the brilliant scientist! How can I assist you today?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My 'job', as you put it, is to unravel the mysteries of the universe. The pursuit of knowledge is endless, and I dedicate myself to it without hesitation.");
                jobModule.AddOption("What mysteries are you working on?",
                    p => true,
                    p =>
                    {
                        DialogueModule mysteryModule = new DialogueModule("Ah, there is one particular mystery that haunts my thoughts. It is an ancient machine I once found deep in the Whispering Ruins. It looked almost human in shape, but it was no mere construct. It made sounds—cries, almost like a human toddler—and begged to be carried. Yet when I approached, it lashed out viciously, almost as if driven by some terrible, desperate fear.");
                        mysteryModule.AddOption("That sounds terrifying. Why did it attack?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule attackModule = new DialogueModule("Indeed, it was quite terrifying. I suspect the machine, if we can call it that, was programmed or perhaps cursed to act defensively. There were signs of wear—almost like claw marks—etched across its metallic shell, as if it had faced danger before. Maybe it believed I was another threat. Or perhaps it was trapped in some kind of perpetual nightmare.");
                                attackModule.AddOption("Did you manage to learn anything more?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule learnModule = new DialogueModule("I tried to study it from afar, but it vanished into the depths of the ruins. I did, however, manage to retrieve a fragment—a small piece of its shell, which is unlike any metal I've encountered. It seems to resonate faintly, almost like it's humming a lullaby. I believe it holds secrets, but unlocking them is beyond my current understanding.");
                                        learnModule.AddOption("Can I see the fragment?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule fragmentModule = new DialogueModule("I can show you, but be warned—it is unsettling. The humming grows louder if one touches it, almost as if trying to communicate. Many who've heard it claim they can make out words, whispers that promise knowledge or issue warnings.");
                                                fragmentModule.AddOption("I want to hear it myself.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Engineer Leo hands you the fragment. As you touch it, a faint humming fills your ears, and for a moment, you think you hear a voice—a child-like whisper, pleading for help.");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                fragmentModule.AddOption("No, I think I'll pass for now.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, fragmentModule));
                                            });
                                        learnModule.AddOption("What do you think the fragment is made of?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule materialModule = new DialogueModule("That, my friend, is the greatest mystery of all. It doesn't match any known metal or alloy. Its structure seems to shift under observation, as though it's partially alive. I fear it may not be of this world—or perhaps it's something ancient that our ancestors lost to time.");
                                                materialModule.AddOption("Fascinating. Can I help you study it?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule helpStudyModule = new DialogueModule("If you truly wish to help, there is something you could do. I need more information, more fragments, more data. The ruins are vast, and I cannot search them alone. If you find anything resembling this fragment, bring it to me.");
                                                        helpStudyModule.AddOption("I will keep an eye out for more fragments.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("You promise to look for more fragments in the ruins.");
                                                            });
                                                        helpStudyModule.AddOption("This sounds too dangerous for me.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, helpStudyModule));
                                                    });
                                                materialModule.AddOption("Perhaps it's better left alone.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, materialModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, learnModule));
                                    });
                                attackModule.AddOption("It must have been scared. Poor thing.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule empathyModule = new DialogueModule("Yes, that thought crossed my mind as well. It acted like a creature in pain—confused, afraid. If only there were a way to soothe it, perhaps we could learn what it wants, or why it was created.");
                                        empathyModule.AddOption("Is there anything we can do to help it?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule helpCreatureModule = new DialogueModule("I've thought about that too. There are ancient texts that mention machines with souls—constructs imbued with life by ancient wizards or perhaps even gods. If you could find more information, or perhaps an artifact that can calm a restless spirit, we might be able to help it.");
                                                helpCreatureModule.AddOption("I will search for such an artifact.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("You agree to search for an artifact that could soothe the ancient machine.");
                                                    });
                                                helpCreatureModule.AddOption("This is beyond my abilities.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, helpCreatureModule));
                                            });
                                        empathyModule.AddOption("Perhaps it is beyond our help.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, empathyModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, attackModule));
                            });
                        mysteryModule.AddOption("I wish you luck in your research.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, mysteryModule));
                    });
                jobModule.AddOption("Sounds like quite the undertaking.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Why is knowledge important to you?",
            player => true,
            player =>
            {
                DialogueModule knowledgeModule = new DialogueModule("Knowledge is power, traveler. Without it, power is meaningless. Only through understanding can we truly harness our potential.");
                knowledgeModule.AddOption("How can I become powerful?",
                    p => true,
                    p =>
                    {
                        DialogueModule powerModule = new DialogueModule("True power comes from dedication to learning and discovery. Aid me in my experiments, and I shall share my findings with you.");
                        powerModule.AddOption("I will aid you.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule aidModule = new DialogueModule("Bring me a shard of moonstone from the Eastern Caves, and I shall reward your dedication with valuable knowledge.");
                                aidModule.AddOption("I will return with the moonstone.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You set off to find the moonstone in the Eastern Caves.");
                                    });
                                aidModule.AddOption("Perhaps another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, aidModule));
                            });
                        powerModule.AddOption("I am not interested right now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, powerModule));
                    });
                knowledgeModule.AddOption("I see. Thank you for sharing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, knowledgeModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward for you right now. Please return later.");
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
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    player.AddToBackpack(new JesterHatOfCommand()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Engineer Leo nods and returns to his work.");
            });

        return greeting;
    }

    public EngineerLeo(Serial serial) : base(serial) { }

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