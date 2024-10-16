using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DeckardCain : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DeckardCain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Deckard Cain";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(60);
        SetInt(80);

        SetHits(60);

        // Appearance
        AddItem(new Robe(1102));
        AddItem(new Sandals(1102));
        AddItem(new GnarledStaff { Name = "Deckard Cain's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DeckardCain(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Deckard Cain, the last of the Horadrim. How may I assist you?");

        greeting.AddOption("Who are the Horadrim?",
            player => true,
            player =>
            {
                DialogueModule horadrimModule = new DialogueModule("The Horadrim were ancient mages, dedicated to capturing the Prime Evils. I am the last of this once great order.");
                horadrimModule.AddOption("Tell me more about the Prime Evils.",
                    p => true,
                    p =>
                    {
                        DialogueModule primeEvilsModule = new DialogueModule("The Prime Evils were the most powerful demons of the Burning Hells. The Horadrim's duty was to imprison them using the Soulstones.");
                        primeEvilsModule.AddOption("That must have been a dangerous mission.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule missionModule = new DialogueModule("Indeed, it was. Many Horadrim sacrificed their lives to keep the world safe from these evils. It was our sacred duty.");
                                missionModule.AddOption("You are truly a hero, Deckard.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Deckard nods solemnly at you.");
                                    });
                                pl.SendGump(new DialogueGump(pl, missionModule));
                            });
                        primeEvilsModule.AddOption("I must be going now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, primeEvilsModule));
                    });
                horadrimModule.AddOption("I must be going now.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, horadrimModule));
            });

        greeting.AddOption("Can you share some wisdom?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("The path of wisdom often leads to sacrifice. For those who truly pursue it, I have a token of appreciation. May this aid you in your quest for knowledge.");
                wisdomModule.AddOption("I would appreciate the token.",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendMessage("Deckard Cain gives you a scroll.");
                        }
                    });
                wisdomModule.AddOption("I must be going now.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("What do you know about knowledge and power?",
            player => true,
            player =>
            {
                DialogueModule knowledgeModule = new DialogueModule("Ah, I sense a curious spirit in you. Tell me, do you believe that knowledge is power?");
                knowledgeModule.AddOption("Yes, knowledge is power.",
                    p => true,
                    p =>
                    {
                        DialogueModule powerModule = new DialogueModule("Indeed, knowledge is power. But power can be both a gift and a curse. It depends on how one wields it.");
                        powerModule.AddOption("I will use it wisely.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        powerModule.AddOption("Perhaps knowledge is dangerous.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule dangerModule = new DialogueModule("You are right. Knowledge, in the wrong hands, can lead to great suffering. It is our responsibility to wield it with care.");
                                dangerModule.AddOption("I understand.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, dangerModule));
                            });
                        p.SendGump(new DialogueGump(p, powerModule));
                    });
                knowledgeModule.AddOption("No, power corrupts.",
                    p => true,
                    p =>
                    {
                        DialogueModule corruptModule = new DialogueModule("Power can indeed corrupt, but only if the heart is weak. Those with pure intentions must strive to use power for good.");
                        corruptModule.AddOption("Thank you for your insight.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, corruptModule));
                    });
                player.SendGump(new DialogueGump(player, knowledgeModule));
            });

        greeting.AddOption("Can you identify this magic item for me?",
            player => true,
            player =>
            {
                DialogueModule identifyModule = new DialogueModule("Ah, identifying magic items is one of my greatest joys! Each item carries a story, a hidden power, and a mystery waiting to be unveiled. Please, hand it over so I can examine it closely.");
                identifyModule.AddOption("What do you enjoy about identifying items?",
                    p => true,
                    p =>
                    {
                        DialogueModule joyModule = new DialogueModule("Oh, adventurer, there is so much to love! Imagine holding an unassuming trinket, only to discover it harbors great magic within. The thrill of revealing a lost artifact's name or a long-forgotten enchantment is a feeling like no other. It's almost as though I'm peering through the fog of history to uncover the past.");
                        joyModule.AddOption("Tell me about an interesting item you've identified.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule storyModule = new DialogueModule("Ah, I remember identifying a peculiar ring once. It was charred and blackened, and adventurers thought it worthless. But as I peered deeper, it was revealed to be a Ring of the Phoenix, enchanted with fire immunity and said to grant the wearer rebirth in times of dire peril. The adventurer who brought it could hardly believe their luck!");
                                storyModule.AddOption("That sounds incredible!",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule incredibleModule = new DialogueModule("Indeed, it was! The beauty of it lies in how seemingly ordinary items can hold such profound power. It is a reminder that there is magic all around us, often hidden in plain sight, waiting for someone to take notice.");
                                        incredibleModule.AddOption("I hope I find such an item one day.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Deckard Cain smiles at you warmly.");
                                            });
                                        incredibleModule.AddOption("Perhaps you could identify something for me now?",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Deckard Cain takes your item and examines it closely, muttering ancient words of identification.");
                                            });
                                        pla.SendGump(new DialogueGump(pla, incredibleModule));
                                    });
                                storyModule.AddOption("Do you have any other stories?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule moreStoriesModule = new DialogueModule("Oh, I have plenty of stories! There was once an enchanted shield that bore a curse—the Shield of Eternal Vigilance. Anyone who wielded it could never sleep, always standing guard. It took a great deal of research to identify the nature of the curse and help its owner find peace.");
                                        moreStoriesModule.AddOption("That sounds like a terrible curse.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule curseModule = new DialogueModule("It was indeed a terrible burden. The adventurer who brought it to me was exhausted, having gone weeks without proper rest. Fortunately, knowledge can also be a key to breaking curses. Together, we lifted the enchantment, and the adventurer was finally able to sleep.");
                                                curseModule.AddOption("You're a true hero, Deckard.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Deckard Cain bows his head humbly.");
                                                    });
                                                curseModule.AddOption("Thank you for sharing these stories.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, curseModule));
                                            });
                                        moreStoriesModule.AddOption("Thank you, Deckard. I must be on my way.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, moreStoriesModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, storyModule));
                            });
                        joyModule.AddOption("Can you identify my item now?",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Deckard Cain takes your item and begins to chant in an ancient tongue, revealing its hidden properties.");
                            });
                        joyModule.AddOption("I must be going now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, joyModule));
                    });
                identifyModule.AddOption("Here's my item for identification.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Deckard Cain takes your item and examines it closely, revealing its hidden properties.");
                    });
                identifyModule.AddOption("Maybe another time, Deckard.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identifyModule));
            });

        greeting.AddOption("Goodbye, Deckard.",
            player => true,
            player =>
            {
                player.SendMessage("Deckard Cain nods at you wisely.");
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