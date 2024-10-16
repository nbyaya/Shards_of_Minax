using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class Adria : BaseCreature
{
    [Constructable]
    public Adria() : base(AIType.AI_Healer, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Adria";
        Body = 0x191; // Human female body

        SetStr(50);
        SetDex(50);
        SetInt(140);
        SetHits(60);

        // Appearance
        AddItem(new Robe(1152));
        AddItem(new Sandals(0));
        AddItem(new Spellbook() { Name = "Adria's Grimoire" });

        SpeechHue = 0; // Default speech hue
    }

    public Adria(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Adria, the Sorceress of Tristram. What do you want?");
        
        greeting.AddOption("Tell me about your magic.",
            player => true,
            player =>
            {
                DialogueModule magicModule = new DialogueModule("Do you truly believe you have the wit to understand the forces I command? Are you even capable of comprehending true power?");
                magicModule.AddOption("I seek to understand.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateKnowledgeModule()));
                    });
                magicModule.AddOption("Perhaps I should move on.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, greeting));
                    });
                player.SendGump(new DialogueGump(player, magicModule));
            });

        greeting.AddOption("What is your ambition?",
            player => true,
            player =>
            {
                DialogueModule ambitionModule = new DialogueModule("Ha! You are amusingly confident for a mere mortal. Tell me, what would you do with true power if you had it?");
                ambitionModule.AddOption("I would use it for good.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateKnowledgeModule()));
                    });
                ambitionModule.AddOption("That's for me to know.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, greeting));
                    });
                player.SendGump(new DialogueGump(player, ambitionModule));
            });

        greeting.AddOption("Can you teach me something?",
            player => true,
            player =>
            {
                DialogueModule teachModule = new DialogueModule("Hmph. Perhaps you are not entirely without potential. But potential alone means nothing. Prove your worth to me, and I may share some of my knowledge.");
                teachModule.AddOption("What must I do to prove myself?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateQuestModule()));
                    });
                teachModule.AddOption("Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, greeting));
                    });
                player.SendGump(new DialogueGump(player, teachModule));
            });

        return greeting;
    }

    private DialogueModule CreateKnowledgeModule()
    {
        DialogueModule knowledgeModule = new DialogueModule("Knowledge is a double-edged sword. The more you learn, the more dangerous it can become. Are you ready for the burden it carries?");
        knowledgeModule.AddOption("I am ready.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Continue the conversation
            });
        knowledgeModule.AddOption("I'm not sure.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to the main greeting
            });
        return knowledgeModule;
    }

    private DialogueModule CreateQuestModule()
    {
        DialogueModule questModule = new DialogueModule("To prove yourself, bring me a rare tome from the depths of the Mystic Library. Only then will I consider sharing my knowledge with you.");
        questModule.AddOption("I will find it!",
            pl => true,
            pl =>
            {
                pl.SendMessage("You set off to find the rare tome.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Continue the conversation
            });
        questModule.AddOption("That sounds too difficult.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to the main greeting
            });
        return questModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
