using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Lady Fibonacci")]
public class LadyFibonacci : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LadyFibonacci() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lady Fibonacci";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(70);
        SetInt(110);
        SetHits(65);

        // Appearance
        AddItem(new PlainDress() { Hue = 1130 });
        AddItem(new Sandals() { Hue = 1130 });
        AddItem(new Spellbook() { Name = "Fibonacci's Sequence" });

        Hue = Utility.RandomSkinHue();
        HairItemID = Utility.RandomList(0x203B, 0x203C);
        HairHue = Utility.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lady Fibonacci, a philosopher of the arcane mysteries. Do you yearn for knowledge?");

        greeting.AddOption("Tell me about your philosophy.",
            player => true,
            player =>
            {
                DialogueModule philosophyModule = new DialogueModule("The cosmos is a vast puzzle, and I seek the hidden patterns within. What do you seek, traveler?");
                philosophyModule.AddOption("I seek knowledge.",
                    p => true,
                    p =>
                    {
                        DialogueModule knowledgeModule = new DialogueModule("Knowledge is the key that unlocks the door to enlightenment. Are you willing to pay the price?");
                        knowledgeModule.AddOption("Yes, I am willing.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule priceModule = new DialogueModule("Knowledge comes at a cost, be it time, effort, or sacrifice. Are you prepared for the journey ahead?");
                                priceModule.AddOption("I am ready for any challenge.",
                                    p1 => true,
                                    p1 =>
                                    {
                                        p1.SendMessage("The journey of a thousand miles begins with a single step. What would you like to explore first?");
                                        // Further nested options can be added here.
                                        ExploreOptions(p1);
                                    });
                                priceModule.AddOption("Perhaps I need more time to think.",
                                    p1 => true,
                                    p1 =>
                                    {
                                        p1.SendGump(new DialogueGump(p1, CreateGreetingModule()));
                                    });
                                player.SendGump(new DialogueGump(player, priceModule));
                            });
                        knowledgeModule.AddOption("No, perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, knowledgeModule));
                    });
                philosophyModule.AddOption("What secrets do you hold?",
                    p => true,
                    p =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Ah, secrets! Prove your worth by answering this riddle: \"I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.\" What am I?");
                        secretsModule.AddOption("Wind",
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
                                    pl.SendMessage("Impressive! You've answered correctly. Here is a reward for your keen intellect.");
                                    pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                }
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        secretsModule.AddOption("I don't know.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, secretsModule));
                    });
                greeting.AddOption("Never mind.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, philosophyModule));
            });

        return greeting;
    }

    private void ExploreOptions(PlayerMobile player)
    {
        DialogueModule exploreModule = new DialogueModule("What area of knowledge fascinates you most?");
        exploreModule.AddOption("The mysteries of the universe.",
            p => true,
            p =>
            {
                DialogueModule universeModule = new DialogueModule("The universe is a tapestry of energies and forces. Each star, each planet has its own story. Would you like to hear about the constellations or the celestial mechanics?");
                universeModule.AddOption("Tell me about the constellations.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Constellations are ancient patterns that guide sailors and inspire poets. Each has its own mythology. Would you like to learn about a specific constellation?");
                        // Additional options can be added here.
                        LearnAboutConstellation(pl);
                    });
                universeModule.AddOption("What about celestial mechanics?",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("The movements of celestial bodies are governed by gravitational forces and orbital mechanics. It’s a complex dance. Would you like to understand the basics or dive deeper into theories?");
                        // Additional options can be added here.
                    });
                player.SendGump(new DialogueGump(player, universeModule));
            });
        exploreModule.AddOption("The nature of existence.",
            p => true,
            p =>
            {
                DialogueModule existenceModule = new DialogueModule("Existence is a profound topic. Are we merely particles in a vast universe, or is there a greater purpose? Would you like to explore philosophical theories or scientific viewpoints?");
                existenceModule.AddOption("Philosophical theories.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Philosophers like Plato and Descartes have pondered the nature of existence. Do you want to learn about their thoughts?");
                        // Additional options can be added here.
                    });
                existenceModule.AddOption("Scientific viewpoints.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Science seeks to understand existence through observation and experimentation. Would you like to hear about quantum theories or evolutionary biology?");
                        // Additional options can be added here.
                    });
                player.SendGump(new DialogueGump(player, existenceModule));
            });
        exploreModule.AddOption("The arcane arts.",
            p => true,
            p =>
            {
                DialogueModule arcaneModule = new DialogueModule("The arcane arts are powerful and dangerous. Are you interested in learning about spells, potions, or the history of magic?");
                arcaneModule.AddOption("Tell me about spells.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Spells can manipulate the elements, heal, or even control minds. Would you like to learn about a specific school of magic?");
                        // Additional options can be added here.
                    });
                arcaneModule.AddOption("What about potions?",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Potions can heal, enhance abilities, or cause transformations. Would you like to know about brewing or ingredients?");
                        // Additional options can be added here.
                    });
                player.SendGump(new DialogueGump(player, arcaneModule));
            });
        player.SendGump(new DialogueGump(player, exploreModule));
    }

    private void LearnAboutConstellation(PlayerMobile player)
    {
        // Further detailed dialogue about constellations can be added here.
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

    public LadyFibonacci(Serial serial) : base(serial) { }
}
