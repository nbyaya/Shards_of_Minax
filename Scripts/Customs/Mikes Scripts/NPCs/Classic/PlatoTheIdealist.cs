using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class PlatoTheIdealist : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public PlatoTheIdealist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Plato the Idealist";
        Body = 0x190; // Human male body

        // Stats
        SetStr(65);
        SetDex(55);
        SetInt(125);
        SetHits(58);

        // Appearance
        AddItem(new Robe() { Hue = 2208 });
        AddItem(new Sandals() { Hue = 1176 });
        AddItem(new Spellbook() { Name = "Plato's Dialogues" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public PlatoTheIdealist(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Plato the Idealist, a philosopher lost in this wretched world! How may I enlighten you?");

        greeting.AddOption("Tell me about your philosophy.",
            player => true,
            player =>
            {
                DialogueModule philosophyModule = new DialogueModule("Philosophy is the pursuit of wisdom and truth. It enables us to contemplate the nature of reality. Do you seek to understand the essence of things?");
                philosophyModule.AddOption("Yes, I seek deeper truths.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTruthModule()));
                    });
                philosophyModule.AddOption("No, I prefer the material.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateMaterialModule()));
                    });
                player.SendGump(new DialogueGump(player, philosophyModule));
            });

        greeting.AddOption("What do you seek?",
            player => true,
            player =>
            {
                DialogueModule questModule = new DialogueModule("I seek the ultimate truth. The Realm of Forms holds the key to understanding reality. Would you like to explore this concept?");
                questModule.AddOption("Yes, what is the Realm of Forms?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateFormsModule()));
                    });
                questModule.AddOption("No, I'm not interested in abstract concepts.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, questModule));
            });

        greeting.AddOption("Tell me about your injuries.",
            player => true,
            player =>
            {
                DialogueModule injuryModule = new DialogueModule("Yes, I was injured by those who felt threatened by my thoughts. The pen is indeed mightier than the sword.");
                injuryModule.AddOption("How did that happen?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateInjuryDetailModule()));
                    });
                injuryModule.AddOption("Words can cut deeply.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, injuryModule));
            });

        return greeting;
    }

    private DialogueModule CreateTruthModule()
    {
        DialogueModule truthModule = new DialogueModule("Ideas have the power to change the world. If you can grasp the depth of an idea, you can reshape reality.");
        truthModule.AddOption("What is the most significant idea you've encountered?",
            pl => true,
            pl =>
            {
                DialogueModule significantIdeaModule = new DialogueModule("The most significant idea is that of the Form of the Good. It is the ultimate source of all truth and knowledge. Without it, we cannot understand anything.");
                significantIdeaModule.AddOption("What does the Form of the Good represent?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                significantIdeaModule.AddOption("Can you explain it further?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, significantIdeaModule));
            });
        return truthModule;
    }

    private DialogueModule CreateMaterialModule()
    {
        DialogueModule materialModule = new DialogueModule("The material world is an illusion, a shadow of the true reality that lies in the Realm of Forms. Are you satisfied with mere shadows?");
        materialModule.AddOption("I seek to break free from these shadows.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateTruthModule()));
            });
        materialModule.AddOption("I find comfort in the material.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return materialModule;
    }

    private DialogueModule CreateFormsModule()
    {
        DialogueModule formsModule = new DialogueModule("The Realm of Forms is where perfect and unchanging ideals exist. All physical objects are merely reflections of these Forms. Do you grasp this concept?");
        formsModule.AddOption("Can you give me an example?",
            pl => true,
            pl =>
            {
                DialogueModule exampleModule = new DialogueModule("Consider a beautiful chair. The physical chair you see is imperfect, but the Form of the chair exists in the Realm of Forms, embodying true beauty and utility.");
                exampleModule.AddOption("What other Forms exist?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateOtherFormsModule()));
                    });
                exampleModule.AddOption("So, all objects are flawed representations?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, exampleModule));
            });
        return formsModule;
    }

    private DialogueModule CreateOtherFormsModule()
    {
        DialogueModule otherFormsModule = new DialogueModule("Other Forms include Justice, Beauty, and Equality. Each of these Forms represents the purest expression of their qualities. Would you like to discuss one?");
        otherFormsModule.AddOption("Tell me about the Form of Justice.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateJusticeModule()));
            });
        otherFormsModule.AddOption("What about the Form of Beauty?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateBeautyModule()));
            });
        otherFormsModule.AddOption("I wish to know about Equality.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateEqualityModule()));
            });
        return otherFormsModule;
    }

    private DialogueModule CreateJusticeModule()
    {
        DialogueModule justiceModule = new DialogueModule("The Form of Justice represents the ideal state of fairness and righteousness. In the material world, justice can be distorted and misinterpreted. Do you believe true justice exists?");
        justiceModule.AddOption("I believe true justice is a construct.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        justiceModule.AddOption("I believe it can be achieved.",
            pl => true,
            pl =>
            {
                justiceModule.AddOption("How can we strive for true justice?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateJusticeStrivingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, justiceModule));
            });
        return justiceModule;
    }

    private DialogueModule CreateJusticeStrivingModule()
    {
        DialogueModule strivingModule = new DialogueModule("Striving for true justice requires wisdom, virtue, and a commitment to the greater good. Each individual's actions contribute to a just society. What role do you see yourself playing?");
        strivingModule.AddOption("I wish to be a beacon of virtue.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Your aspirations are noble! Remember, the journey is as important as the destination.");
            });
        strivingModule.AddOption("I feel powerless in this world.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Every small act of kindness contributes to the greater whole. You have more power than you realize.");
            });
        return strivingModule;
    }

    private DialogueModule CreateBeautyModule()
    {
        DialogueModule beautyModule = new DialogueModule("The Form of Beauty is the essence of all that is aesthetically pleasing. It transcends mere physical appearance and resonates with the soul. Do you appreciate beauty in your life?");
        beautyModule.AddOption("Yes, beauty inspires me.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Then you are attuned to the true nature of things. Seek beauty in both the mundane and extraordinary.");
            });
        beautyModule.AddOption("No, I focus on practicality.",
            pl => true,
            pl =>
            {
                beautyModule.AddOption("Can practicality coexist with beauty?",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Absolutely! The most practical solutions can often be the most beautiful in their simplicity.");
                    });
                pl.SendGump(new DialogueGump(pl, beautyModule));
            });
        return beautyModule;
    }

    private DialogueModule CreateEqualityModule()
    {
        DialogueModule equalityModule = new DialogueModule("The Form of Equality embodies the ideal of fairness and sameness in value. In our world, equality often feels like a fleeting concept. How do you perceive equality in your interactions?");
        equalityModule.AddOption("I strive for equality in all my dealings.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Your commitment to equality is commendable! It is a vital component of a harmonious society.");
            });
        equalityModule.AddOption("I see many inequalities around me.",
            pl => true,
            pl =>
            {
                equalityModule.AddOption("What can be done to address these inequalities?",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Change begins with awareness and action. Each small effort contributes to a greater movement.");
                    });
                pl.SendGump(new DialogueGump(pl, equalityModule));
            });
        return equalityModule;
    }

    private DialogueModule CreateInjuryDetailModule()
    {
        DialogueModule injuryDetailModule = new DialogueModule("I faced opposition from those who feared the ideas I presented. It is the nature of the philosopher to challenge conventions. Have you faced similar opposition?");
        injuryDetailModule.AddOption("Yes, challenging norms can be perilous.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Indeed, the pursuit of truth often leads to conflict. But remember, it is worth it for the clarity it brings.");
            });
        injuryDetailModule.AddOption("No, I prefer to avoid conflict.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Avoiding conflict can lead to stagnation. Sometimes, one must stir the waters to gain understanding.");
            });
        return injuryDetailModule;
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
