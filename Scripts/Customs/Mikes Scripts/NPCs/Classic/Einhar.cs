using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EinharTheBeastmaster : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EinharTheBeastmaster() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Einhar";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(70);
        SetInt(50);

        SetHits(80);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1001 });
        AddItem(new LeatherChest() { Hue = 1001 });
        AddItem(new Boots() { Hue = 1001 });
        AddItem(new Bow() { Name = "Einhar's Bow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
    }

    public EinharTheBeastmaster(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Einhar, the Beastmaster. What brings you to my wild abode?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I am Einhar, the Beastmaster! I capture and study exotic creatures, learning their secrets and understanding their ways.");
                whoModule.AddOption("Tell me more about your work.",
                    p => true,
                    p =>
                    {
                        DialogueModule workModule = new DialogueModule("I travel across the lands, observing and capturing beasts, from the smallest squirrel to the mightiest dragon. Each creature has a unique story. I also worked alongside an Exile, and Zana, on an incredible journey.");
                        workModule.AddOption("What was your journey with the Exile like?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule exileModule = new DialogueModule("Ah, the Exile! They were a formidable ally, and their determination was like nothing I've ever seen. Together, we faced countless challenges, from slaying monstrous beasts to navigating ancient, treacherous ruins.");
                                exileModule.AddOption("What kind of challenges did you face?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule challengesModule = new DialogueModule("We fought many beasts, each more dangerous than the last. One of the most memorable was the Chimera of the Forgotten Grove. It was a creature of immense power, with the cunning of a serpent and the strength of a lion. The Exile fought bravely, and we managed to subdue it only through clever tactics and unbreakable resolve.");
                                        challengesModule.AddOption("Tell me about the Chimera.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule chimeraModule = new DialogueModule("The Chimera was unlike any creature I had ever seen. Its scales shimmered with a sickly green hue, and its roar could shake the very ground. It took both the strength of the Exile and my knowledge of beasts to finally bring it down. The victory was bittersweet, for such creatures are part of the natural world, but we had to protect the people.");
                                                chimeraModule.AddOption("That sounds terrifying!",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, chimeraModule));
                                            });
                                        challengesModule.AddOption("Tell me about another challenge.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule anotherChallengeModule = new DialogueModule("Another time, we ventured deep into the ancient ruins of Vaal. The Vaal were an ancient civilization with powerful magic, and their traps were still very much active. It was Zana who helped us navigate those dangers—without her, we would have surely been lost.");
                                                anotherChallengeModule.AddOption("What role did Zana play?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule zanaModule = new DialogueModule("Zana is a Cartographer with the ability to open portals to unknown worlds. She was searching for her father, and her knowledge of these strange lands saved us many times. She has an unbreakable will and a sharp mind—qualities we needed to survive.");
                                                        zanaModule.AddOption("Did you ever find her father?",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                DialogueModule fatherModule = new DialogueModule("Sadly, no. Despite our best efforts, Zana's father remained lost. But Zana never gave up hope. She still believes that one day she might find him, and I admire her for that. Sometimes, the journey is more important than the destination.");
                                                                fatherModule.AddOption("That's quite the journey.",
                                                                    plaaaaa => true,
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule()));
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, fatherModule));
                                                            });
                                                        zanaModule.AddOption("Zana sounds impressive.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, zanaModule));
                                                    });
                                                anotherChallengeModule.AddOption("What were the ruins like?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule ruinsModule = new DialogueModule("The ruins were dark, filled with remnants of ancient magic and long-forgotten technology. Statues of forgotten gods loomed over us, and the air was thick with the weight of history. It was both fascinating and terrifying—a place where time itself seemed to have stopped.");
                                                        ruinsModule.AddOption("Sounds like a dangerous place.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, ruinsModule));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, anotherChallengeModule));
                                            });
                                        challengesModule.AddOption("That must have been quite the adventure.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, challengesModule));
                                    });
                                exileModule.AddOption("What was Zana like?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule zanaDetailModule = new DialogueModule("Zana was, and still is, a beacon of hope. Her knowledge of maps and portals is unmatched. She was always calm under pressure, even when we faced the fiercest of beasts or the trickiest of puzzles. She was the glue that held our group together.");
                                        zanaDetailModule.AddOption("She sounds impressive.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, zanaDetailModule));
                                    });
                                exileModule.AddOption("That's enough about the Exile.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, exileModule));
                            });
                        workModule.AddOption("I'll leave you to your studies.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, workModule));
                    });
                whoModule.AddOption("Goodbye, Beastmaster.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("Can you tell me about exotic creatures?",
            player => true,
            player =>
            {
                DialogueModule exoticModule = new DialogueModule("I've encountered creatures from the densest forests to the highest mountains. Each one unique, with its own behaviors and traits. Some are peaceful, others ferocious.");
                exoticModule.AddOption("Tell me about a ferocious creature.",
                    p => true,
                    p =>
                    {
                        DialogueModule ferociousModule = new DialogueModule("Ferocious creatures are often misunderstood. They act out of instinct or fear. I once befriended a ferocious dragon, and in return, it granted me a special token as a sign of our bond.");
                        ferociousModule.AddOption("Do you still have the token?",
                            pl => true,
                            pl =>
                            {
                                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                                if (DateTime.UtcNow - lastRewardTime < cooldown)
                                {
                                    pl.SendMessage("I have no reward right now. Please return later.");
                                }
                                else
                                {
                                    pl.SendMessage("For your curiosity, I'll give you this old relic I found during one of my travels. May it serve you well.");
                                    pl.AddToBackpack(new MaxxiaScroll());
                                    lastRewardTime = DateTime.UtcNow;
                                }
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        ferociousModule.AddOption("That's enough about dragons.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, ferociousModule));
                    });
                exoticModule.AddOption("I'll ask more later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, exoticModule));
            });

        greeting.AddOption("Goodbye, Beastmaster.",
            player => true,
            player =>
            {
                player.SendMessage("Einhar nods at you and returns to his work.");
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