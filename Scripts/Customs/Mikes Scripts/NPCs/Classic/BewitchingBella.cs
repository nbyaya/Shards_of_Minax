using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BewitchingBella : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BewitchingBella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Bewitching Bella";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(90);
        SetInt(60);
        SetHits(55);

        // Appearance
        AddItem(new FancyDress(2968)); // PlainDress with hue 2968
        AddItem(new Sandals(2128));    // Heels with hue 2128
        AddItem(new GoldBracelet() { Name = "Bella's Bracelet" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this); // Female hair
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public BewitchingBella(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            Say("I have no reward right now. Please return later.");
        }
        else
        {
            Say("For your thoughtful inquiry, please accept this reward.");
            from.AddToBackpack(new Gold(1000)); // Example reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, darling. I'm Bewitching Bella, but you knew that, didn't you? What brings you here today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Well, darling, I specialize in the art of seduction and companionship. My health is as radiant as a rose in full bloom, and I cherish the beauty of the rose gardens of Yew. Have you ever been to Yew?");
                aboutModule.AddOption("Tell me more about Yew.",
                    p => true,
                    p =>
                    {
                        DialogueModule yewModule = new DialogueModule("Ah, the town of Yew. It's surrounded by lush forests and has the most mesmerizing trees. Some say they hold secrets of their own. Have you ever wondered about tree secrets?");
                        yewModule.AddOption("That sounds intriguing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        yewModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, yewModule));
                    });
                aboutModule.AddOption("What do you do here?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My \"job,\" you ask? Well, darling, I specialize in the art of seduction and companionship. Even in my line of work, one must uphold certain principles—such as the virtue of Honor.");
                        jobModule.AddOption("Tell me about the virtue of Honor.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Bella speaks about the importance of honor even in delicate professions.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        jobModule.AddOption("How does honor fit into seduction?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule honorSeductionModule = new DialogueModule("Honor, darling, is about respecting boundaries and understanding desires. True seduction isn't just about charm—it's about empathy and connection, knowing what the other person needs, and giving it to them... if they deserve it.");
                                honorSeductionModule.AddOption("You make it sound almost noble.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Bella smiles, a glint of admiration in her eyes.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                honorSeductionModule.AddOption("I'm not sure I understand.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Bella chuckles softly. 'Perhaps it's something you need to experience rather than understand.'");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, honorSeductionModule));
                            });
                        jobModule.AddOption("What services do you offer?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule servicesModule = new DialogueModule("Ah, curious, are we? My services vary, darling, depending on what you desire. Companionship, a listening ear, or perhaps... more intimate delights. Each has its price, of course, but I'm sure we can come to an arrangement.");
                                servicesModule.AddOption("Tell me more about companionship.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule companionshipModule = new DialogueModule("Companionship, darling, is about presence. Sometimes, a person simply needs someone to share their thoughts with—a partner who will listen without judgment, who will laugh at their jokes, or just sit in silence under the stars. It's an underrated comfort, wouldn't you agree?");
                                        companionshipModule.AddOption("That does sound comforting.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Bella smiles warmly. 'It can be, and I'm very good at it.'");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        companionshipModule.AddOption("I was thinking of something... more.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule intimateModule = new DialogueModule("Oh, I see where your mind is, darling. Intimacy is a dance—one that requires both trust and desire. I assure you, no one knows the steps better than I. But know that true intimacy is not just physical... it's about baring your soul.");
                                                intimateModule.AddOption("I'm interested.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Bella leans closer, her voice a whisper. 'Then perhaps, we should explore this further in a more private setting.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                intimateModule.AddOption("Maybe I've overstepped.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Bella pulls back slightly, her smile still kind. 'No harm in curiosity, darling. We all have our limits.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, intimateModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, companionshipModule));
                                    });
                                servicesModule.AddOption("What are your prices?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule pricesModule = new DialogueModule("The price depends on what you seek, darling. A conversation might cost a few coins, while a night of enchantment might be far more. But rest assured, every gold piece will be worth the experience.");
                                        pricesModule.AddOption("What if I can't afford it?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule bargainModule = new DialogueModule("Oh, darling, not everything has to be about gold. There are other ways to pay—services, favors, information. If you're truly interested, I'm sure we could come to an... understanding.");
                                                bargainModule.AddOption("What kind of favors?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule favorsModule = new DialogueModule("Favors can be many things, darling. Perhaps you could retrieve something for me—an item I've long desired. Or maybe, there's a troublesome individual whose attention I need diverted. Are you willing to get your hands dirty for me?");
                                                        favorsModule.AddOption("I'm willing to help.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Bella's eyes glimmer with satisfaction. 'I knew you had potential, darling.'");
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        favorsModule.AddOption("I think I'd rather keep my hands clean.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Bella sighs, her expression a mix of disappointment and understanding. 'Very well, darling. We all have our boundaries.'");
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, favorsModule));
                                                    });
                                                bargainModule.AddOption("Maybe I should reconsider.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Bella nods, her smile returning. 'Take your time, darling. The offer is always open.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, bargainModule));
                                            });
                                        pricesModule.AddOption("That's reasonable.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Bella's smile widens. 'I'm glad you think so, darling.'");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, pricesModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, servicesModule));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                aboutModule.AddOption("Thank you, that's enough for now.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you know anything about enchantments?",
            player => true,
            player =>
            {
                DialogueModule enchantmentsModule = new DialogueModule("Enchantments are everywhere—in the twinkle of stars, the whisper of winds, and the allure of forbidden spells. Are you curious about spells, darling?");
                enchantmentsModule.AddOption("Yes, tell me more about spells.",
                    p => true,
                    p =>
                    {
                        DialogueModule spellModule = new DialogueModule("Spells can be both beautiful and dangerous. The forbidden spells hold secrets that only the brave dare uncover.");
                        spellModule.AddOption("I think I'll stick to the safer spells.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        spellModule.AddOption("I'm ready for a challenge!",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Bella smiles knowingly, as if she's heard that before.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, spellModule));
                    });
                enchantmentsModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, enchantmentsModule));
            });

        greeting.AddOption("Goodbye, Bella.",
            player => true,
            player =>
            {
                player.SendMessage("Bella smiles warmly at you as you depart.");
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