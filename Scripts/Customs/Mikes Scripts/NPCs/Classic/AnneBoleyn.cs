using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AnneBoleyn : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public AnneBoleyn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Anne Boleyn";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(60);
        SetInt(90);

        SetHits(65);
        VirtualArmor = 10;

        // Appearance
        AddItem(new FancyDress(1156)); // Clothing item with hue 1156
        AddItem(new GoldNecklace());
        AddItem(new Boots(1174)); // Boots with hue 1174
        AddItem(new Spellbook() { Name = "Anne's Diary" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public AnneBoleyn(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Anne Boleyn. How may I assist you?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Aye, once a queen of England, my fate was sealed at the Tower of London. It's a tale of romance, ambition, and betrayal.");
                aboutModule.AddOption("What happened to you?",
                    p => true,
                    p =>
                    {
                        DialogueModule storyModule = new DialogueModule("I was accused of treason and conspiracy, and my life ended at the Tower. Yet, even after such a fate, I find solace here, tending to the gardens.");
                        storyModule.AddOption("Tell me about your marriage to the King.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule marriageModule = new DialogueModule("Ah, my marriage to King Henry VIII was a complex dance of love, power, and ambition. At first, it was filled with passion and the promise of greatness. Henry was captivated by me, and I believed that together we could change England forever.");
                                marriageModule.AddOption("What was the early part of your marriage like?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule earlyMarriageModule = new DialogueModule("The early days were filled with love and hope. Henry showered me with gifts and attention, and I felt truly adored. We spoke of reforming the church, of bringing a new era to the kingdom. But there was always pressure—pressure to provide a male heir, to prove my worth.");
                                        earlyMarriageModule.AddOption("How did you handle the pressure to provide an heir?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule heirPressureModule = new DialogueModule("It was difficult, more so than I could have imagined. The court was rife with whispers, and every failed pregnancy felt like a dagger to my heart. I knew that my position depended on it, and yet, fate was cruel. My daughter Elizabeth was my pride, but she was not the son Henry so desperately wanted.");
                                                heirPressureModule.AddOption("What was Henry's reaction?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule henryReactionModule = new DialogueModule("At first, Henry was patient, or so it seemed. He doted on Elizabeth, but as time passed, his demeanor changed. He became distant, frustrated. The love we once shared was replaced by tension and fear. I knew that my failure to give him a son was tearing us apart.");
                                                        henryReactionModule.AddOption("Did you try to reconcile with him?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule reconcileModule = new DialogueModule("I tried, oh how I tried. I attempted to remind him of the love we once had, of our shared dreams. But by then, other voices had his ear—advisors, courtiers, and even other women. The King was a man easily swayed, and I could feel my influence slipping away.");
                                                                reconcileModule.AddOption("Who were these other women?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule otherWomenModule = new DialogueModule("Jane Seymour, in particular, caught his eye. She was everything I was not—gentle, obedient, and willing to play the role of the dutiful wife without question. I knew then that my time was limited. The King desired a simpler love, one without the complications that I brought.");
                                                                        otherWomenModule.AddOption("How did you feel about Jane Seymour?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule janeFeelingsModule = new DialogueModule("I bore her no ill will, though it pained me to see her take my place. In truth, I pitied her. She would soon learn that the King's affections were fickle, and that the crown came with a heavy burden. I only hoped that she would fare better than I did.");
                                                                                janeFeelingsModule.AddOption("What happened next?",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        DialogueModule downfallModule = new DialogueModule("The accusations began soon after. Treason, infidelity, conspiracy—each charge more outrageous than the last. I was imprisoned in the Tower, the very place where I had once been crowned queen. My trial was a farce, and the verdict was decided long before I ever stepped into the courtroom.");
                                                                                        downfallModule.AddOption("That must have been terrifying.",
                                                                                            plh => true,
                                                                                            plh =>
                                                                                            {
                                                                                                DialogueModule fearModule = new DialogueModule("It was. The walls of the Tower seemed to close in on me, and the knowledge that my fate was sealed weighed heavily on my mind. Yet, I found moments of peace—brief flashes of acceptance. I prayed for my daughter, for her future, and hoped that she would grow up strong and wise.");
                                                                                                fearModule.AddOption("What were your final thoughts?",
                                                                                                    pli => true,
                                                                                                    pli =>
                                                                                                    {
                                                                                                        DialogueModule finalThoughtsModule = new DialogueModule("In my final moments, I thought of Elizabeth. I thought of the England I had dreamed of—a land of prosperity and peace. I knew that I would not live to see it, but I had hope that my daughter would one day rule and bring about the change that I could not. My love for her was my final comfort.");
                                                                                                        finalThoughtsModule.AddOption("Thank you for sharing your story.",
                                                                                                            plj => true,
                                                                                                            plj =>
                                                                                                            {
                                                                                                                plj.SendGump(new DialogueGump(plj, CreateGreetingModule()));
                                                                                                            });
                                                                                                        pli.SendGump(new DialogueGump(pli, finalThoughtsModule));
                                                                                                    });
                                                                                                plh.SendGump(new DialogueGump(plh, fearModule));
                                                                                            });
                                                                                        plg.SendGump(new DialogueGump(plg, downfallModule));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, janeFeelingsModule));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, otherWomenModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, reconcileModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, henryReactionModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, heirPressureModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, earlyMarriageModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, marriageModule));
                            });
                        storyModule.AddOption("That sounds tragic.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, storyModule));
                    });
                aboutModule.AddOption("That's enough for now.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("How are you feeling?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in fine health, thank you. Thankfully, the serenity of these gardens has kept my spirit in a state of tranquility. I find nature to be healing.");
                healthModule.AddOption("Tell me about the gardens.",
                    p => true,
                    p =>
                    {
                        DialogueModule gardenModule = new DialogueModule("These gardens are home to many rare and mystical herbs. Some even say there's an ancient herb with powerful healing abilities hidden here.");
                        gardenModule.AddOption("What kind of herbs?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule herbModule = new DialogueModule("These herbs carry the wisdom of ages. If you're keen to learn, seek the elder druid in the forest. He knows their secrets.");
                                herbModule.AddOption("Thank you for the information.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, herbModule));
                            });
                        gardenModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, gardenModule));
                    });
                healthModule.AddOption("Glad to hear you're well.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My daily duty is to tend to the gardens of this fine estate.");
                jobModule.AddOption("You must enjoy gardening.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Can you tell me about the virtue of humility?",
            player => true,
            player =>
            {
                DialogueModule humilityModule = new DialogueModule("The virtue of humility is a noble one. True humility is understanding our place in the vastness of the universe. It teaches us to be compassionate to others.");
                humilityModule.AddOption("That is inspiring.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, humilityModule));
            });

        greeting.AddOption("I wish to ponder the virtues.",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    noRewardModule.AddOption("Understood.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new BeltSlotChangeDeed());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Anne smiles warmly at you.");
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