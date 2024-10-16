using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AuroraTheFair : BaseCreature
{
    [Constructable]
    public AuroraTheFair() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aurora the Fair";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(75);
        SetInt(75);

        SetHits(75);
        SetMana(75);
        SetStam(75);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 20;

        // Appearance
        AddItem(new FancyDress(1153)); // Clothing item with hue 1153
        AddItem(new Sandals(1153)); // Sandals with hue 1153
        AddItem(new FeatheredHat(1153)); // Feathered hat with hue 1153
        AddItem(new HeavyCrossbow() { Name = "Aurora's Heavy Crossbow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0;
    }

    public AuroraTheFair(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Aurora the Fair, a humble archer and protector of the virtue of Valor. How may I assist you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule introModule = new DialogueModule("I am Aurora the Fair, a humble archer. I've dedicated my life to protecting the virtue of Valor. What else would you like to know?");
                introModule.AddOption("What does it mean to protect Valor?",
                    p => true,
                    p =>
                    {
                        DialogueModule valorModule = new DialogueModule("To protect Valor means not only defending it in battles but also upholding its principles in everyday life. Valor represents courage, the ability to stand up for what is right even in the face of adversity.");
                        valorModule.AddOption("That's admirable.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, valorModule));
                    });
                introModule.AddOption("Tell me about your training as an archer.",
                    p => true,
                    p =>
                    {
                        DialogueModule archerModule = new DialogueModule("I've trained with the bow for many years. It requires patience, precision, and humility. Being an archer has taught me much about both combat and the importance of restraint.");
                        archerModule.AddOption("Humility? Tell me more.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule humilityModule = new DialogueModule("Humility is one of the eight virtues, and it teaches us to be grounded and respect others. It is the opposite of pride. Did you know, the first syllable of the mantra of Humility is 'LUM'?");
                                humilityModule.AddOption("What is 'LUM'?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule lumModule = new DialogueModule("'LUM' is a powerful syllable that, when meditated upon, can bring you closer to understanding true humility. It helps remind us of the importance of staying humble.");
                                        lumModule.AddOption("Thank you for the wisdom.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, lumModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, humilityModule));
                            });
                        p.SendGump(new DialogueGump(p, archerModule));
                    });
                introModule.AddOption("What are the eight virtues?",
                    p => true,
                    p =>
                    {
                        DialogueModule virtuesModule = new DialogueModule("The eight virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each virtue helps shape one's character and guide their actions.");
                        virtuesModule.AddOption("Tell me about Valor.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule valorDetailModule = new DialogueModule("Valor represents courage and determination in the face of adversity. It's about doing what's right, even when it's hard.");
                                valorDetailModule.AddOption("That sounds inspiring.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, valorDetailModule));
                            });
                        p.SendGump(new DialogueGump(p, virtuesModule));
                    });
                introModule.AddOption("I have other questions.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, introModule));
            });

        greeting.AddOption("Can you tell me about Valor?",
            player => true,
            player =>
            {
                DialogueModule valorModule = new DialogueModule("Valor is one of the eight virtues. It represents courage and determination in the face of adversity. It encourages us to act rightly, even when we are afraid.");
                valorModule.AddOption("How do you practice Valor?",
                    p => true,
                    p =>
                    {
                        DialogueModule practiceModule = new DialogueModule("I practice Valor by standing up for those who cannot defend themselves, and by constantly challenging myself to face my fears. Courage is not the absence of fear, but the ability to act despite it.");
                        practiceModule.AddOption("Thank you for sharing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, practiceModule));
                    });
                valorModule.AddOption("I wish you well on your journey.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, valorModule));
            });

        greeting.AddOption("I noticed your hair is in wonderful condition. Do you have a secret?",
            player => true,
            player =>
            {
                DialogueModule hairModule = new DialogueModule("Ah, you've noticed! Keeping my hair in good condition is no easy task. I use a special mixture of herbs to maintain its health and shine.");
                hairModule.AddOption("What herbs do you use?",
                    p => true,
                    p =>
                    {
                        DialogueModule herbsModule = new DialogueModule("The main ingredients are Silverleaf, Golden Heather, and Nightshade Dew. Each has unique properties that help strengthen and nourish the hair.");
                        herbsModule.AddOption("Can you tell me more about Silverleaf?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule silverleafModule = new DialogueModule("Silverleaf is known for its strengthening properties. It can be found in the shaded groves of the forest, where it absorbs the essence of moonlight, making it excellent for fortifying hair and preventing breakage.");
                                silverleafModule.AddOption("Where can I find Silverleaf?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule silverleafLocationModule = new DialogueModule("Silverleaf grows in the Whispering Forest, often under the canopy where the moonlight barely touches the ground. It's a rare herb, but worth the effort to find.");
                                        silverleafLocationModule.AddOption("I will look for it.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You decide to search the Whispering Forest for Silverleaf.");
                                            });
                                        silverleafLocationModule.AddOption("That sounds challenging.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, silverleafLocationModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, silverleafModule));
                            });
                        herbsModule.AddOption("Tell me about Golden Heather.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule heatherModule = new DialogueModule("Golden Heather is a beautiful herb that brings shine to the hair. It grows in sunlit clearings and is known for its radiant golden hue. It symbolizes warmth and vitality.");
                                heatherModule.AddOption("How do you use Golden Heather?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule heatherUsageModule = new DialogueModule("I create a paste with Golden Heather and mix it with spring water. Applying this to my hair once a week keeps it shiny and healthy.");
                                        heatherUsageModule.AddOption("That sounds lovely.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, heatherUsageModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, heatherModule));
                            });
                        herbsModule.AddOption("What is Nightshade Dew?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule nightshadeModule = new DialogueModule("Nightshade Dew is collected from the Nightshade plant during dawn. It has a unique ability to nourish the scalp and promote healthy growth, but it must be used cautiously due to its potency.");
                                nightshadeModule.AddOption("How do you collect it?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule collectionModule = new DialogueModule("Collecting Nightshade Dew requires patience. You must gather it right at dawn, just as the first light touches the petals. Too late, and the dew loses its potency.");
                                        collectionModule.AddOption("I understand.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, collectionModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, nightshadeModule));
                            });
                        herbsModule.AddOption("This is fascinating. Thank you.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, herbsModule));
                    });
                hairModule.AddOption("Can I help you gather these herbs?",
                    p => true,
                    p =>
                    {
                        DialogueModule helpModule = new DialogueModule("That would be wonderful! Gathering these herbs is no easy task. If you could bring me Silverleaf, Golden Heather, and Nightshade Dew, I would be most grateful.");
                        helpModule.AddOption("I will return with the herbs.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You set off to gather the special herbs for Aurora.");
                            });
                        helpModule.AddOption("I'm not sure I'm ready for that.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, helpModule));
                    });
                hairModule.AddOption("I see, thank you for sharing your secret.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, hairModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Aurora nods respectfully at you.");
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