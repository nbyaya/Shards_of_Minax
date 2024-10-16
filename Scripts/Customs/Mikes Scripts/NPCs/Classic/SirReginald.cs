using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class SirReginald : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirReginald() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Reginald";
        Body = 0x190; // Human male body

        // Stats
        SetStr(70);
        SetDex(50);
        SetInt(90);
        SetHits(55);

        // Appearance
        AddItem(new LongPants() { Hue = 1906 });
        AddItem(new Tunic() { Hue = 1906 });
        AddItem(new Shoes() { Hue = 1906 });
        AddItem(new FeatheredHat() { Hue = 1906 });
        AddItem(new MortarPestle() { Name = "Reginald's Mortar and Pestle" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
    }

	public SirReginald(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Sir Reginald. Have you ever pondered the nature of our reality?");

        greeting.AddOption("What do you mean by reality?",
            player => true,
            player =>
            {
                DialogueModule realityModule = new DialogueModule("I believe this world is but a grand simulation. Everything we perceive may be mere illusions crafted by a higher power.");
                realityModule.AddOption("But why would anyone create a simulation?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule purposeModule = new DialogueModule("Perhaps to learn, to observe, or to test the limits of morality. What if our choices have no consequence beyond this digital realm?");
                        purposeModule.AddOption("That sounds troubling.",
                            p => true,
                            p =>
                            {
                                DialogueModule troublingModule = new DialogueModule("Indeed, it is. It leads one to question the very fabric of our existence. Are we truly free, or just puppets in a play?");
                                p.SendGump(new DialogueGump(p, troublingModule));
                            });
                        pl.SendGump(new DialogueGump(pl, purposeModule));
                    });
                player.SendGump(new DialogueGump(player, realityModule));
            });

        greeting.AddOption("How do you know it's a simulation?",
            player => true,
            player =>
            {
                DialogueModule knowledgeModule = new DialogueModule("I have seen patterns, anomalies in our world. Moments that feel too perfect, too coincidental to be random.");
                knowledgeModule.AddOption("Can you give me an example?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule exampleModule = new DialogueModule("Consider how certain events unfold. A chance encounter leads to a life-changing moment, as if scripted by an unseen hand. Even our battles feel predetermined at times.");
                        exampleModule.AddOption("That's an interesting perspective.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to the main greeting
                            });
                        pl.SendGump(new DialogueGump(pl, exampleModule));
                    });
                player.SendGump(new DialogueGump(player, knowledgeModule));
            });

        greeting.AddOption("What about your father?",
            player => true,
            player =>
            {
                DialogueModule fatherModule = new DialogueModule("Ah, my father, Sir Roderick. He was a brave soul, yet even he seemed to play his part in this grand simulation. Was he merely following a script, or did he have free will?");
                fatherModule.AddOption("What did he teach you?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule lessonsModule = new DialogueModule("He taught me honor, bravery, and the importance of protecting the weak. Yet, I wonder, were these lessons part of my programming?");
                        lessonsModule.AddOption("You sound conflicted.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to the main greeting
                            });
                        pl.SendGump(new DialogueGump(pl, lessonsModule));
                    });
                player.SendGump(new DialogueGump(player, fatherModule));
            });

        greeting.AddOption("Can you tell me about Lady Emeline?",
            player => true,
            player =>
            {
                DialogueModule emelineModule = new DialogueModule("Lady Emeline is a brilliant cook and skilled herbalist. Yet, I wonder if her kindness is a result of her programming, or if she truly has a choice in her actions.");
                emelineModule.AddOption("Do you think she believes in the simulation too?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule beliefModule = new DialogueModule("It’s hard to say. She seems blissfully unaware of the nature of our reality. But should she ever question it, I would guide her.");
                        beliefModule.AddOption("What if she never questions it?",
                            p => true,
                            p =>
                            {
                                DialogueModule fateModule = new DialogueModule("Then perhaps she is one of the fortunate ones, living without the burden of doubt. Ignorance can sometimes be a blessing.");
                                p.SendGump(new DialogueGump(p, fateModule));
                            });
                        pl.SendGump(new DialogueGump(pl, beliefModule));
                    });
                player.SendGump(new DialogueGump(player, emelineModule));
            });

        greeting.AddOption("Is there a way to escape this simulation?",
            player => true,
            player =>
            {
                DialogueModule escapeModule = new DialogueModule("Some say enlightenment is the key, while others believe it’s in the acceptance of our roles. But can one truly escape a construct designed to hold us?");
                escapeModule.AddOption("What if we rebel against it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule rebellionModule = new DialogueModule("Rebellion may lead to chaos, but chaos can also reveal truths hidden beneath the surface. What if we were meant to challenge this reality?");
                        rebellionModule.AddOption("Perhaps it's time for a revolution!",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to the main greeting
                            });
                        pl.SendGump(new DialogueGump(pl, rebellionModule));
                    });
                player.SendGump(new DialogueGump(player, escapeModule));
            });

        greeting.AddOption("What do you hope to achieve?",
            player => true,
            player =>
            {
                DialogueModule hopeModule = new DialogueModule("My hope is to find a way to understand the nature of our existence. Perhaps, in understanding, I can find peace.");
                hopeModule.AddOption("That’s a noble pursuit.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule nobleModule = new DialogueModule("Thank you. Even if I am but a character in a simulation, I seek to play my part with honor.");
                        pl.SendGump(new DialogueGump(pl, nobleModule));
                    });
                player.SendGump(new DialogueGump(player, hopeModule));
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
