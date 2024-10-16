using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CirceTheWitch : BaseCreature
{
    [Constructable]
    public CirceTheWitch() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Circe";
        Body = 0x191; // Human female body

        // Stats
        SetStr(95);
        SetDex(65);
        SetInt(115);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1128 });
        AddItem(new Boots() { Hue = 1128 });
        AddItem(new WizardsHat() { Hue = 1128 });
        AddItem(new Spellbook() { Name = "Circe's Codex" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
    }

    public CirceTheWitch(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Circe, a witch of these lands. How may I assist you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Circe, a witch who weaves the threads of fate, concocts elixirs, and dabbles in the arcane arts. I also have a particular fascination with transforming those who cross my path into various creatures. Would you care to know more?");
                identityModule.AddOption("Transforming people? Tell me more.",
                    p => true,
                    p =>
                    {
                        DialogueModule transformationModule = new DialogueModule("Ah, yes, the art of transformation. I find it fascinating to explore the various forms a being can take. Turning men into animals is one of my favorite pastimes. It allows me to express my creativity while also reminding people of the power I wield.");
                        transformationModule.AddOption("What kinds of animals do you turn them into?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule animalModule = new DialogueModule("It depends on my mood, really. Sometimes I turn them into pigs, which are always amusing with their snouts and grunts. Other times, I prefer a majestic stag, or perhaps a timid rabbit. Each form has its own unique charm and significance.");
                                animalModule.AddOption("Why pigs?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule pigsModule = new DialogueModule("Pigs are such delightful creatures. They're a symbol of gluttony, and turning a haughty man into a pig often brings a poetic justice to the situation. Besides, the way they scramble around afterwards never fails to amuse me.");
                                        pigsModule.AddOption("That sounds quite mischievous.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule mischiefModule = new DialogueModule("Oh, indeed! There is a certain joy in mischief. But it is not all cruelty, you see. Sometimes, the transformation serves as a lesson—an opportunity for the transformed to learn humility and respect.");
                                                mischiefModule.AddOption("Do they ever learn their lesson?",
                                                    plaab => true,
                                                    plaab =>
                                                    {
                                                        DialogueModule lessonModule = new DialogueModule("Some do, yes. They come back to me, humbled, having learned a valuable lesson about themselves and the world. Others, well... they remain pigs for quite some time. It's all up to them, really.");
                                                        lessonModule.AddOption("Fascinating. Thank you for sharing.",
                                                            plaabc => true,
                                                            plaabc =>
                                                            {
                                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule()));
                                                            });
                                                        plaab.SendGump(new DialogueGump(plaab, lessonModule));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, mischiefModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, pigsModule));
                                    });
                                animalModule.AddOption("Why stags?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule stagsModule = new DialogueModule("Stags are symbols of nobility and grace. Turning a man into a stag often represents the stripping away of false pride, leaving behind a creature of true elegance. It is my way of giving them a second chance, albeit in a different form.");
                                        stagsModule.AddOption("That sounds almost kind.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule kindnessModule = new DialogueModule("Perhaps it is. I may be a witch, but even I have a sense of fairness. Not every transformation is a punishment. Some are a gift, a way to reconnect with nature and shed the burdens of human arrogance.");
                                                kindnessModule.AddOption("Thank you for your insight.",
                                                    plaab => true,
                                                    plaab =>
                                                    {
                                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, kindnessModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, stagsModule));
                                    });
                                animalModule.AddOption("Do you ever regret transforming people?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? Rarely. Each transformation is a reflection of the individual—what they need, what they deserve, or what they might become. There is an art to it, and each choice has its purpose.");
                                        regretModule.AddOption("That is a very unique perspective.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, regretModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, animalModule));
                            });
                        transformationModule.AddOption("Have you ever turned anyone back?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule revertModule = new DialogueModule("Of course. Not all transformations are permanent. Sometimes, I grant people a second chance if they show true remorse or if they intrigue me enough. After all, what's the fun in magic if there's no flexibility?");
                                revertModule.AddOption("What do they say when they return?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule returnModule = new DialogueModule("Some are grateful, others angry. It all depends on what they've learned during their time as an animal. The grateful ones tend to understand the lesson, while the angry ones often refuse to see beyond their own pride.");
                                        returnModule.AddOption("Thank you for sharing your stories.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, returnModule));
                                    });
                                revertModule.AddOption("I see. Thank you for the explanation.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, revertModule));
                            });
                        transformationModule.AddOption("Perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, transformationModule));
                    });
                identityModule.AddOption("Thank you, that's all I need to know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Can you teach me about magic?",
            player => true,
            player =>
            {
                DialogueModule magicModule = new DialogueModule("Magic is not just a skill; it's a way of perceiving the world. It takes dedication to master, and each spell carries both its power and its risk.");
                magicModule.AddOption("What are the risks?",
                    p => true,
                    p =>
                    {
                        DialogueModule riskModule = new DialogueModule("The risks, you see, are many. A failed spell could backfire, causing harm to the caster. The arcane forces are not to be toyed with lightly.");
                        riskModule.AddOption("I'll be careful. Thank you.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, riskModule));
                    });
                magicModule.AddOption("Perhaps I need more preparation.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, magicModule));
            });

        greeting.AddOption("Do you have any advice for battles?",
            player => true,
            player =>
            {
                DialogueModule battleAdviceModule = new DialogueModule("Battles are fought not only with strength but with wit. Knowing your enemy is the key, and understanding magic can often turn the tide of battle.");
                battleAdviceModule.AddOption("Thank you for the advice.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, battleAdviceModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Circe nods at you, her eyes filled with mystery.");
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