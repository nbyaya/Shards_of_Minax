using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DiogenesTheCynic : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DiogenesTheCynic() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Diogenes the Cynic";
        Body = 0x190; // Human male body

        // Stats
        SetStr(75);
        SetDex(45);
        SetInt(115);

        SetHits(63);
        
        // Appearance
        AddItem(new Robe(2210));
        AddItem(new Boots(1179));
        AddItem(new Lantern { Name = "Diogenes' Lamp" });

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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler. I am Diogenes the Cynic, a philosopher. What brings you to converse with me amidst the absurdity of existence?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a philosopher seeking to understand the human condition, its virtues, and vices. I contemplate the absurdity of existence and challenge the conventions of society.");
                aboutModule.AddOption("What do you mean by absurdity?",
                    p => true,
                    p =>
                    {
                        DialogueModule absurdityModule = new DialogueModule("Life is full of contradictions and inexplicable phenomena. The more we understand its absurdity, the closer we come to true wisdom.");
                        absurdityModule.AddOption("How do I find meaning in the absurd?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule meaningModule = new DialogueModule("Each person must find their own meaning. What is meaningless to one might be the entire world to another. Question everything, even your own existence, and perhaps meaning will emerge.");
                                meaningModule.AddOption("How do I question my own existence?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule questionExistenceModule = new DialogueModule("To question your existence, you must strip away the distractions of life. Consider what remains when you remove wealth, power, and societal expectations. What is truly left of you? Reflect on whether you are living your own life, or merely a life dictated by others.");
                                        questionExistenceModule.AddOption("That sounds challenging. How do I begin?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule beginModule = new DialogueModule("Begin by simplifying your life. Reduce your attachments to material things, question your desires, and see what truly brings you fulfillment. Cynicism teaches that many of our desires are unnecessary, and we must challenge them to find true freedom.");
                                                beginModule.AddOption("How do I let go of unnecessary desires?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule desiresModule = new DialogueModule("Letting go of desires is a process of realization. You must see the futility in them. Ask yourself: why do I want this? Does it truly add to my life, or am I seeking it because society tells me to? True Cynicism is about detaching from these imposed needs and embracing simplicity.");
                                                        desiresModule.AddOption("I will reflect on that.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, desiresModule));
                                                    });
                                                beginModule.AddOption("Thank you for the advice.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, beginModule));
                                            });
                                        questionExistenceModule.AddOption("This seems overwhelming. Is it worth it?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule worthItModule = new DialogueModule("It is indeed challenging, but consider the alternative. Living without questioning is akin to sleepwalking through life. To be truly alive, one must engage with the deeper questions, face discomfort, and seek a genuine understanding of oneself and the world.");
                                                worthItModule.AddOption("I see your point.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, worthItModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, questionExistenceModule));
                                    });
                                meaningModule.AddOption("Thank you for your wisdom.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, meaningModule));
                            });
                        absurdityModule.AddOption("I shall ponder on that.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, absurdityModule));
                    });
                aboutModule.AddOption("What is your lifestyle like?",
                    p => true,
                    p =>
                    {
                        DialogueModule lifestyleModule = new DialogueModule("I live a life of radical simplicity, rejecting material possessions and societal expectations. I believe that true happiness comes from reducing one's needs to the bare essentials and living in harmony with nature.");
                        lifestyleModule.AddOption("Why do you reject material possessions?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule possessionsModule = new DialogueModule("Material possessions are burdens. They tie us down and distract us from the pursuit of wisdom. By letting go of them, I am free to focus on what truly matters: understanding myself and the world. Possessions often possess us, rather than the other way around.");
                                possessionsModule.AddOption("How do you manage without them?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule manageModule = new DialogueModule("I find joy in simplicity. I have my lamp, my robe, and my wits. By shedding excess, I gain the freedom to think deeply, to experience life without distraction, and to discover inner peace. The less you own, the less you fear losing.");
                                        manageModule.AddOption("That sounds freeing.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, manageModule));
                                    });
                                possessionsModule.AddOption("I find that hard to accept.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule acceptModule = new DialogueModule("It is not an easy path, and it is not for everyone. But consider: what is the true cost of the things you own? Often, we pay with our time, our freedom, and our peace of mind. Perhaps in reflecting upon this, you may find a new perspective.");
                                        acceptModule.AddOption("I will consider it.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, acceptModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, possessionsModule));
                            });
                        lifestyleModule.AddOption("What do you mean by harmony with nature?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule natureModule = new DialogueModule("To live in harmony with nature means to embrace simplicity and live according to one's true needs, rather than desires imposed by society. It means understanding that nature provides us with everything we need, and excess only leads to suffering.");
                                natureModule.AddOption("How do I live in harmony with nature?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule harmonyModule = new DialogueModule("Start by understanding your needs versus your wants. Spend time in nature, reflect on the natural order, and recognize that many of our struggles come from wanting more than what is necessary. The more you align yourself with what is natural, the more peace you will find.");
                                        harmonyModule.AddOption("I will try to simplify.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, harmonyModule));
                                    });
                                natureModule.AddOption("I need more time to think about this.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, natureModule));
                            });
                        lifestyleModule.AddOption("Thank you for sharing your lifestyle.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, lifestyleModule));
                    });
                aboutModule.AddOption("I wish to know more about your wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is wisdom?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("Wisdom requires questioning everything, even your own existence. It is found in understanding both virtues and vices, light and dark, the contradictions of life.");
                wisdomModule.AddOption("Can you share some virtues?",
                    p => true,
                    p =>
                    {
                        DialogueModule virtuesModule = new DialogueModule("The virtues are what guide us in life: honesty, compassion, valor, and justice. Honesty, above all, is the foundation of one’s character.");
                        virtuesModule.AddOption("How can I practice honesty?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule honestyModule = new DialogueModule("Honesty is more than telling the truth; it is living it. It means being genuine in your actions and steadfast in your character.");
                                honestyModule.AddOption("I will strive to live honestly.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, honestyModule));
                            });
                        virtuesModule.AddOption("Thank you for sharing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, virtuesModule));
                    });
                wisdomModule.AddOption("I will reflect on your words.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Can you offer me guidance for peace?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward for you right now. Please return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    DialogueModule peaceModule = new DialogueModule("To achieve true peace, one must meditate and reflect upon one's actions and thoughts. For your genuine interest, I bestow upon you this small token of appreciation. Use it well on your journey.");
                    peaceModule.AddOption("Thank you for the gift.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Replace with actual item
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, peaceModule));
                }
            });

        greeting.AddOption("Farewell, philosopher.",
            player => true,
            player =>
            {
                player.SendMessage("Diogenes nods thoughtfully, watching you as you leave.");
            });

        return greeting;
    }

    public DiogenesTheCynic(Serial serial) : base(serial) { }

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