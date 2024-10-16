using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CassandraTheSeer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CassandraTheSeer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cassandra";
        Body = 0x191; // Human female body

        // Stats
        SetStr(100);
        SetDex(60);
        SetInt(100);

        SetHits(70);

        // Appearance
        AddItem(new Robe() { Hue = 1157 }); // Robe with hue 1157
        AddItem(new Sandals() { Hue = 1157 });
        AddItem(new Spellbook() { Name = "Cassandra's Book of Prophecies" });
        AddItem(new HoodedShroudOfShadows() { Hue = 1157 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue
        lastRewardTime = DateTime.MinValue;
    }

    public CassandraTheSeer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Cassandra, the seer of fates. What wisdom do you seek?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Cassandra, a vessel for the cosmic energies, guiding those who seek answers. My visions reveal glimpses of the threads of fate.");
                identityModule.AddOption("What do you see in my fate?",
                    p => true,
                    p =>
                    {
                        DialogueModule fateModule = new DialogueModule("Destiny is a tapestry, and your choices are the threads. I see shadows in your future... and a glimmer of hope. Choose wisely at every crossroad.");
                        fateModule.AddOption("Can you give me more details?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule detailModule = new DialogueModule("The choices you make will lead you to a significant crossroad. Be mindful, as the cosmos favors those with courage and wisdom. However, I see dark omens too... a looming figure, a shrouded presence that will challenge your strength.");
                                detailModule.AddOption("What is this looming figure?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule figureModule = new DialogueModule("The figure is one that represents your deepest fears. It is not just a foe of flesh and blood, but a manifestation of doubt, hesitation, and your inner conflicts. To defeat it, you must first conquer your own heart.");
                                        figureModule.AddOption("How can I conquer my inner conflicts?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule conflictModule = new DialogueModule("The path to conquering oneself is arduous. It requires introspection, honesty, and the willingness to face the parts of yourself you may not wish to see. Seek allies who understand you, and be open to their wisdom. Together, you may overcome the shadows within.");
                                                conflictModule.AddOption("I will reflect on this.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                conflictModule.AddOption("That sounds too difficult.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule difficultyModule = new DialogueModule("Difficulty is the forge of strength. Without challenge, there can be no growth. But I understand—sometimes it feels insurmountable. You must proceed at your own pace.");
                                                        difficultyModule.AddOption("Thank you for understanding.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, difficultyModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, conflictModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, figureModule));
                                    });
                                detailModule.AddOption("Thank you for the insight.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, detailModule));
                            });
                        fateModule.AddOption("I will be cautious.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, fateModule));
                    });
                identityModule.AddOption("I seek no more answers.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("What do you know about fate and destiny?",
            player => true,
            player =>
            {
                DialogueModule destinyModule = new DialogueModule("Fate is like a river, flowing ever onward, while destiny is the destination that awaits. But remember, you have the power to steer your vessel amidst the currents.");
                destinyModule.AddOption("How can I change my fate?",
                    p => true,
                    p =>
                    {
                        DialogueModule changeModule = new DialogueModule("By understanding the forces around you and making courageous decisions, you may alter the course of your journey. Remember, wisdom is not in knowing, but in understanding. But be warned: altering fate comes at a cost.");
                        changeModule.AddOption("What cost must I pay?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule costModule = new DialogueModule("The cost may be in the form of sacrifice—something precious to you, be it time, relationships, or even a part of yourself. Every choice has consequences, and to change the river's flow, you must be willing to lose something dear.");
                                costModule.AddOption("I am willing to sacrifice.",
                                    plc => true,
                                    plc =>
                                    {
                                        DialogueModule sacrificeModule = new DialogueModule("Your resolve is admirable. The universe takes note of such determination. Remember, though, that sacrifice is not always physical. Sometimes, it is the letting go of fears or desires that binds you.");
                                        sacrificeModule.AddOption("I understand.",
                                            pld => true,
                                            pld =>
                                            {
                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                            });
                                        plc.SendGump(new DialogueGump(plc, sacrificeModule));
                                    });
                                costModule.AddOption("I cannot make such a sacrifice.",
                                    plc => true,
                                    plc =>
                                    {
                                        DialogueModule reconsiderModule = new DialogueModule("It is wise to know one's limits. Sometimes, waiting for the right moment to act is better than forcing change before you are ready. Patience, too, is a virtue.");
                                        reconsiderModule.AddOption("Thank you, Cassandra.",
                                            pld => true,
                                            pld =>
                                            {
                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                            });
                                        plc.SendGump(new DialogueGump(plc, reconsiderModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, costModule));
                            });
                        changeModule.AddOption("I will keep that in mind.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, changeModule));
                    });
                destinyModule.AddOption("That is profound. Thank you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, destinyModule));
            });

        greeting.AddOption("Can you offer me a glimpse of the future?",
            player => true,
            player =>
            {
                DialogueModule glimpseModule = new DialogueModule("Very well. Close your eyes. I see shadows... and a glimmer of hope. The decisions you make will lead you to a crossroad. Choose wisely, and perhaps, a reward from the cosmos awaits you. But beware, for I also see great dangers.");
                glimpseModule.AddOption("What dangers do you see?",
                    p => true,
                    p =>
                    {
                        DialogueModule dangerModule = new DialogueModule("I see dark figures—enemies that lurk not only in the physical world but in your mind as well. Doubt, despair, and fear will try to consume you. The path ahead is fraught with trials that will test your spirit.");
                        dangerModule.AddOption("How can I prepare for these trials?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule preparationModule = new DialogueModule("You must cultivate resilience. Strengthen your body, sharpen your mind, and surround yourself with allies you trust. Trust is a light in the darkness, and your courage will be your shield.");
                                preparationModule.AddOption("I will gather my strength.",
                                    plc => true,
                                    plc =>
                                    {
                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, preparationModule));
                            });
                        dangerModule.AddOption("I fear I am not ready.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule fearModule = new DialogueModule("Fear is natural, but do not let it paralyze you. Even the smallest step forward is progress. Take your time, and when you are ready, face the darkness head-on.");
                                fearModule.AddOption("Thank you for your counsel.",
                                    plc => true,
                                    plc =>
                                    {
                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, fearModule));
                            });
                        p.SendGump(new DialogueGump(p, dangerModule));
                    });
                glimpseModule.AddOption("Can I receive the reward now?",
                    p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    p =>
                    {
                        p.SendMessage("Ah, the reward I spoke of... Here, take this. The cosmos recognizes those who seek understanding and wisdom. May it serve you well on your path.");
                        p.AddToBackpack(new RodOfOrcControl());
                        lastRewardTime = DateTime.UtcNow;
                    });
                glimpseModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, glimpseModule));
            });

        greeting.AddOption("What wisdom do you have about the journey?",
            player => true,
            player =>
            {
                DialogueModule journeyModule = new DialogueModule("Journeys are not just about destinations, but the experiences and lessons along the way. Remember, it is the journey that shapes us, not the destination. But beware, there are obstacles ahead.");
                journeyModule.AddOption("What obstacles?",
                    p => true,
                    p =>
                    {
                        DialogueModule obstaclesModule = new DialogueModule("The obstacles are many: betrayal, doubt, and loss. Friends may turn, paths may grow dark, and your resolve may weaken. But through perseverance, you will learn what truly matters.");
                        obstaclesModule.AddOption("How can I overcome these obstacles?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule overcomeModule = new DialogueModule("You must stay true to your purpose. Focus on what drives you, and never lose sight of your values. Accept help when needed, and be willing to adapt. Resilience is forged through hardship.");
                                overcomeModule.AddOption("I will stay strong.",
                                    plc => true,
                                    plc =>
                                    {
                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, overcomeModule));
                            });
                        obstaclesModule.AddOption("I fear the path may be too hard.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule fearPathModule = new DialogueModule("The hardest paths often lead to the most rewarding destinations. Take it one step at a time, and you may find strength you never knew you had.");
                                fearPathModule.AddOption("I will try my best.",
                                    plc => true,
                                    plc =>
                                    {
                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, fearPathModule));
                            });
                        p.SendGump(new DialogueGump(p, obstaclesModule));
                    });
                journeyModule.AddOption("That is wise. Thank you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, journeyModule));
            });

        greeting.AddOption("Goodbye, Cassandra.",
            player => true,
            player =>
            {
                player.SendMessage("Cassandra nods at you knowingly as you take your leave.");
            });

        return greeting;
    }

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