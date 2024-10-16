using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class FeralFred : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public FeralFred() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Feral Fred";
        Body = 0x190; // Human male body

        // Stats
        SetStr(95);
        SetDex(60);
        SetInt(70);
        SetHits(95);

        // Appearance
        AddItem(new LongPants() { Hue = 2967 });
        AddItem(new PlainDress() { Hue = 2965 });
        AddItem(new ThighBoots() { Hue = 1154 });
        AddItem(new ShepherdsCrook() { Name = "Fred's Taming Stick" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public FeralFred(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Feral Fred, the animal tamer. What brings you here today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule selfModule = new DialogueModule("I am Feral Fred, an animal tamer of some renown! I have spent years among creatures great and small, learning their ways and sharing their wisdom.");
                selfModule.AddOption("What do you do?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("I tame and care for animals. Animals teach us compassion, loyalty, and strength. Do you have an animal companion yourself?");
                        jobModule.AddOption("Yes, I have a pet.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule petModule = new DialogueModule("Wonderful! Animals make the best of friends. They are loyal and kind. Treasure them, and they will reward you with love and protection.");
                                petModule.AddOption("Thank you, Fred.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, petModule));
                            });
                        jobModule.AddOption("No, I don't have one.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule noPetModule = new DialogueModule("That's alright. Not everyone is suited to the life of a tamer, but if you ever wish to learn, I can help.");
                                noPetModule.AddOption("Maybe someday.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, noPetModule));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                selfModule.AddOption("You seem to know a lot about animals.",
                    p => true,
                    p =>
                    {
                        DialogueModule animalKnowledgeModule = new DialogueModule("Indeed, animals are my passion. I've tamed many creatures, each with their own unique personalities. From the majestic griffin to the mystical Silver Lynx, I've seen it all.");
                        animalKnowledgeModule.AddOption("Tell me about the griffin.",
                            pl => true,
                            pl =>
                            {
                                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                                {
                                    DialogueModule griffinModule = new DialogueModule("Ah, the griffin! A magnificent beast of both land and sky. I tamed one once, and in gratitude, it gave me a rare feather. Here, take this as a token of our conversation.");
                                    pl.AddToBackpack(new VeterinaryAugmentCrystal()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow;
                                    griffinModule.AddOption("Thank you, Fred.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        });
                                    pl.SendGump(new DialogueGump(pl, griffinModule));
                                }
                                else
                                {
                                    DialogueModule griffinWaitModule = new DialogueModule("You must wait a while before I can give you another reward.");
                                    griffinWaitModule.AddOption("I understand.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        });
                                    pl.SendGump(new DialogueGump(pl, griffinWaitModule));
                                }
                            });
                        animalKnowledgeModule.AddOption("Tell me about the Silver Lynx.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule lynxModule = new DialogueModule("The Silver Lynx is a mystical creature, its fur shimmering under the moonlight. They say its purr can heal wounds, and I've been lucky enough to see one up close.");
                                lynxModule.AddOption("That's amazing!",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, lynxModule));
                            });
                        animalKnowledgeModule.AddOption("Have you ever tamed a dragon?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule dragonModule = new DialogueModule("Ah, dragons... now there's a challenge unlike any other. Taming a dragon requires immense courage, strength, and above all, patience. Dragons are intelligent creatures, proud and fiercely independent.");
                                dragonModule.AddOption("How do you tame a dragon?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule tamingDragonModule = new DialogueModule("Taming a dragon is no simple feat. First, you must earn its respect. Dragons do not respond to force; instead, you must prove your worth through perseverance. They will test you—often violently. One must understand their language, not just of words, but of fire, flight, and fury.");
                                        tamingDragonModule.AddOption("What do you mean by 'test'?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule dragonTestModule = new DialogueModule("Dragons are proud creatures, and they do not suffer fools lightly. When I attempted to tame my first dragon, it took me weeks just to get close without being scorched. Every roar, every flick of their tail, is a test of your intent. Are you worthy? Are you brave? Dragons sense fear, and if you show it, you will never succeed.");
                                                dragonTestModule.AddOption("How did you overcome your fear?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule fearModule = new DialogueModule("Overcoming fear is about understanding it. I realized that fear was the barrier between myself and the dragon. I had to respect the power of the dragon, but not let it overwhelm me. I had to show that I was there not to dominate, but to coexist. It was only when I embraced my fear, rather than fought it, that the dragon began to trust me.");
                                                        fearModule.AddOption("That sounds incredibly difficult.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule difficultyModule = new DialogueModule("Indeed, it was. But taming a dragon is not supposed to be easy. The bond forged with such a creature is unlike any other. Once a dragon accepts you, it becomes a companion like no other—powerful, wise, and fiercely protective.");
                                                                difficultyModule.AddOption("What happens after the dragon accepts you?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule bondModule = new DialogueModule("Once a dragon accepts you, it will allow you to approach it without hostility. You can begin to communicate in subtle ways—through body language, gentle words, and offerings. A dragon's bond is forged over time. You must care for it, hunt with it, and even learn to fly together. It is a partnership, not ownership.");
                                                                        bondModule.AddOption("That sounds truly rewarding.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, bondModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, difficultyModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, fearModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, dragonTestModule));
                                            });
                                        tamingDragonModule.AddOption("What qualities do you need to tame a dragon?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule qualitiesModule = new DialogueModule("To tame a dragon, you need courage, patience, empathy, and a deep respect for the creature. You also need wisdom—to know when to approach and when to give space. Strength alone will not win a dragon's loyalty, but strength of character might.");
                                                qualitiesModule.AddOption("I think I understand now.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, qualitiesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, tamingDragonModule));
                                    });
                                dragonModule.AddOption("Why are dragons so difficult to tame?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule challengeModule = new DialogueModule("Dragons are not like other creatures. They are ancient, wise, and powerful beyond measure. They remember the old times, when they ruled the skies and the lands. They are fiercely proud and will not submit to anyone they deem unworthy. A dragon must see you as an equal, not as a master.");
                                        challengeModule.AddOption("How do you show equality to a dragon?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule equalityModule = new DialogueModule("To show equality, you must treat the dragon with respect. You must understand its needs, fears, and desires. Offer food not as a bribe, but as a gesture of goodwill. Stand your ground when challenged, but never raise your hand in anger. It is a delicate balance, and one that takes time and patience to achieve.");
                                                equalityModule.AddOption("I see, that makes sense.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, equalityModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, challengeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, dragonModule));
                            });
                        p.SendGump(new DialogueGump(p, animalKnowledgeModule));
                    });
                player.SendGump(new DialogueGump(player, selfModule));
            });

        greeting.AddOption("Do you need any help?",
            player => true,
            player =>
            {
                DialogueModule helpModule = new DialogueModule("Caring for animals is not easy, and there are always beasts in need of aid. If you are willing, I could always use an extra hand taming wild creatures or gathering supplies.");
                helpModule.AddOption("I would love to help.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You have offered to help Feral Fred. Perhaps he will have a task for you soon.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                helpModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, helpModule));
            });

        greeting.AddOption("Goodbye, Fred.",
            player => true,
            player =>
            {
                player.SendMessage("Feral Fred waves at you as you leave.");
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