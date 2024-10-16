using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class MasterGauss : BaseCreature
{
    [Constructable]
    public MasterGauss() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Master Gauss";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(65);
        SetInt(105);
        SetHits(70);

        // Appearance
        AddItem(new ShortPants() { Hue = 1156 });
        AddItem(new FancyShirt() { Hue = 1156 });
        AddItem(new Shoes() { Hue = 1156 });
        AddItem(new Spellbook() { Name = "Gaussian Distributions" });
        
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
    }

    public MasterGauss(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, seeker of wisdom. I am Master Gauss, a humble philosopher.");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                DialogueModule healthModule = new DialogueModule("My physical health is of little consequence, for I seek to nourish the mind and spirit. Why do you ask?");
                healthModule.AddOption("I care about your well-being.",
                    p => true,
                    p => 
                    {
                        DialogueModule caringModule = new DialogueModule("Your concern is appreciated, but I assure you that my pursuits are far more important than mere physicality.");
                        p.SendGump(new DialogueGump(p, caringModule));
                    });
                healthModule.AddOption("What if your health affects your wisdom?",
                    p => true,
                    p => 
                    {
                        DialogueModule wisdomModule = new DialogueModule("Ah, a fair point! Indeed, the body and mind are intertwined. One cannot ignore the body's needs if one wishes to gain wisdom. What do you suggest?");
                        wisdomModule.AddOption("Perhaps you should take better care of yourself.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Wise words. I will consider them.")));
                            });
                        wisdomModule.AddOption("What does self-care mean to you?",
                            pl => true,
                            pl => 
                            {
                                DialogueModule selfCareModule = new DialogueModule("Self-care involves nourishing the body with food, rest, and meditation. It is the foundation upon which wisdom can grow.");
                                pl.SendGump(new DialogueGump(pl, selfCareModule));
                            });
                        p.SendGump(new DialogueGump(p, wisdomModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });
        
        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My occupation, if it can be called such, is to ponder the mysteries of existence and the virtues that shape our world. Care to delve deeper?");
                jobModule.AddOption("What mysteries do you ponder?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule mysteriesModule = new DialogueModule("I often contemplate the essence of sacrifice, the nature of truth, and the implications of knowledge. Each thought is a path to greater understanding.");
                        mysteriesModule.AddOption("Tell me more about sacrifice.",
                            p => true,
                            p => 
                            {
                                DialogueModule sacrificeDetail = new DialogueModule("Sacrifice is not merely about loss; it's an exchange of value. What do you think is worth sacrificing for knowledge?");
                                sacrificeDetail.AddOption("Perhaps time?",
                                    pla => true,
                                    pla => 
                                    {
                                        pla.SendGump(new DialogueGump(pla, new DialogueModule("Indeed, time is a precious commodity. Without it, knowledge cannot be cultivated.")));
                                    });
                                sacrificeDetail.AddOption("What about relationships?",
                                    pla => true,
                                    pla => 
                                    {
                                        DialogueModule relationshipsModule = new DialogueModule("Ah, relationships can be both a source of strength and distraction. It is a delicate balance, indeed.");
                                        pla.SendGump(new DialogueGump(pla, relationshipsModule));
                                    });
                                p.SendGump(new DialogueGump(p, sacrificeDetail));
                            });
                        pl.SendGump(new DialogueGump(pl, mysteriesModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What can you tell me about virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("Virtues are the guiding principles that help us navigate life's challenges. Do any particular virtues resonate with you?");
                virtuesModule.AddOption("I admire compassion.",
                    p => true,
                    p => 
                    {
                        DialogueModule compassionModule = new DialogueModule("Compassion is indeed a noble virtue. It allows us to connect with others and understand their suffering. How do you practice compassion?");
                        compassionModule.AddOption("By helping others in need.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("A commendable practice! Helping others not only uplifts them but enriches your own soul.")));
                            });
                        compassionModule.AddOption("I often feel overwhelmed.",
                            pl => true,
                            pl => 
                            {
                                DialogueModule overwhelmedModule = new DialogueModule("It is easy to feel overwhelmed in a world filled with suffering. Remember to care for yourself, so you can be a beacon of light for others.");
                                pl.SendGump(new DialogueGump(pl, overwhelmedModule));
                            });
                        p.SendGump(new DialogueGump(p, compassionModule));
                    });
                virtuesModule.AddOption("I struggle with selflessness.",
                    p => true,
                    p => 
                    {
                        DialogueModule selflessnessModule = new DialogueModule("Selflessness is a challenging virtue to embrace. It requires the understanding that the collective good often outweighs individual desires.");
                        selflessnessModule.AddOption("How can I improve?",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Practice empathy; understand the needs of others as if they were your own. It is a journey, not a destination.")));
                            });
                        p.SendGump(new DialogueGump(p, selflessnessModule));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("What is your journey about?",
            player => true,
            player =>
            {
                DialogueModule journeyModule = new DialogueModule("The journey of self-discovery is fraught with challenges and revelations. What aspect of this journey intrigues you most?");
                journeyModule.AddOption("I want to know more about challenges.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule challengesModule = new DialogueModule("Challenges are the catalysts for growth. They test our resolve and character. What challenges have you faced?");
                        challengesModule.AddOption("I've faced many in my life.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Each challenge you overcome shapes who you are. Embrace them as opportunities for learning.")));
                            });
                        challengesModule.AddOption("I fear challenges.",
                            p => true,
                            p => 
                            {
                                DialogueModule fearModule = new DialogueModule("Fear is natural. It can guide you, but do not let it paralyze you. Step forward with courage, and you will discover strength you did not know you had.");
                                p.SendGump(new DialogueGump(p, fearModule));
                            });
                        pl.SendGump(new DialogueGump(pl, challengesModule));
                    });
                journeyModule.AddOption("I'm curious about revelations.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule revelationsModule = new DialogueModule("Revelations often come when we least expect them. They can change our perspective entirely. What revelations have you experienced?");
                        revelationsModule.AddOption("I had a moment of clarity recently.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Cherish that clarity; it can serve as a guiding light in times of confusion.")));
                            });
                        revelationsModule.AddOption("I struggle to find clarity.",
                            p => true,
                            p => 
                            {
                                DialogueModule struggleModule = new DialogueModule("Clarity often requires stillness. Take time to meditate and reflect; it will come in due time.");
                                p.SendGump(new DialogueGump(p, struggleModule));
                            });
                        pl.SendGump(new DialogueGump(pl, revelationsModule));
                    });
                player.SendGump(new DialogueGump(player, journeyModule));
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
