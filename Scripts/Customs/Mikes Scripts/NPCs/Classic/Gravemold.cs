using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class Gravemold : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Gravemold() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Gravemold the Shadow";
        Body = 0x190; // Human male body

        // Stats
        SetStr(135);
        SetDex(52);
        SetInt(140);
        SetHits(112);

        // Appearance
        AddItem(new Robe() { Hue = 1 });
        AddItem(new ThighBoots() { Hue = 1109 });
        AddItem(new WizardsHat() { Hue = 1109 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public Gravemold(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Gravemold the Shadow, master of the dark arts. What knowledge do you seek about hemomancy?");

        greeting.AddOption("Tell me about hemomancy.",
            player => true,
            player =>
            {
                DialogueModule hemomancyIntro = new DialogueModule("Hemomancy, the art of blood magic, is a path fraught with danger and power. It requires a deep understanding of life and death. What aspect intrigues you?");
                hemomancyIntro.AddOption("What are the principles of hemomancy?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreatePrinciplesModule()));
                    });
                hemomancyIntro.AddOption("What are the dangers of practicing hemomancy?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateDangersModule()));
                    });
                hemomancyIntro.AddOption("Is hemomancy considered evil?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateEthicsModule()));
                    });
                player.SendGump(new DialogueGump(player, hemomancyIntro));
            });

        greeting.AddOption("Can you teach me hemomancy?",
            player => true,
            player =>
            {
                DialogueModule teachModule = new DialogueModule("Teaching hemomancy is no trivial matter. The knowledge is powerful and often misused. Are you ready to embrace this dark art?");
                teachModule.AddOption("Yes, I am ready.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTrainingModule()));
                    });
                teachModule.AddOption("No, I need more information first.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, teachModule));
            });

        greeting.AddOption("What do you know about blood rituals?",
            player => true,
            player =>
            {
                DialogueModule ritualsModule = new DialogueModule("Blood rituals can amplify your magical abilities, but they often come with a steep cost. Would you like to know about specific rituals?");
                ritualsModule.AddOption("Tell me about a specific ritual.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateRitualsModule()));
                    });
                ritualsModule.AddOption("How do I perform these rituals?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateRitualPerformanceModule()));
                    });
                player.SendGump(new DialogueGump(player, ritualsModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.SendMessage("You have shown curiosity and courage. As a token of my acknowledgment, accept this gift from the shadows.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return greeting;
    }

    private DialogueModule CreatePrinciplesModule()
    {
        DialogueModule principlesModule = new DialogueModule("The principles of hemomancy revolve around the manipulation of life force through blood. Blood is the essence of life; by controlling it, one can influence life and death itself.");
        principlesModule.AddOption("How does one manipulate blood?",
            pl => true,
            pl =>
            {
                DialogueModule manipulationModule = new DialogueModule("To manipulate blood, one must learn to connect with the life force within it. This involves complex spells that require both skill and sacrifice.");
                manipulationModule.AddOption("What kind of sacrifices?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateSacrificeModule()));
                    });
                principlesModule.AddOption("Is there a cost to using this power?",
                    plq => true,
                    plq =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateCostModule()));
                    });
                pl.SendGump(new DialogueGump(pl, manipulationModule));
            });

        return principlesModule;
    }

    private DialogueModule CreateDangersModule()
    {
        DialogueModule dangersModule = new DialogueModule("Practicing hemomancy can corrupt the soul. The more one delves into its secrets, the more one risks becoming a puppet of the dark forces they seek to control.");
        dangersModule.AddOption("What happens if I lose control?",
            pl => true,
            pl =>
            {
                DialogueModule controlModule = new DialogueModule("Losing control can lead to madness or worse—becoming a thrall to the very blood you manipulate. Many have lost their humanity in pursuit of power.");
                controlModule.AddOption("That sounds terrifying.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                dangersModule.AddOption("Is there a way to safeguard against this?",
                    plw => true,
                    plw =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateSafeguardModule()));
                    });
                pl.SendGump(new DialogueGump(pl, controlModule));
            });

        return dangersModule;
    }

    private DialogueModule CreateEthicsModule()
    {
        DialogueModule ethicsModule = new DialogueModule("Hemomancy is often viewed as evil due to its association with bloodshed and sacrifice. However, it is the intent of the practitioner that determines the true nature of the art.");
        ethicsModule.AddOption("What if my intentions are pure?",
            pl => true,
            pl =>
            {
                DialogueModule pureIntentModule = new DialogueModule("Pure intentions can guide your use of hemomancy towards healing and protection. Yet, be warned: the temptation to misuse such power lurks always.");
                pureIntentModule.AddOption("I want to use hemomancy for good.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGoodUseModule()));
                    });
                ethicsModule.AddOption("How can I ensure my intentions remain pure?",
                    ple => true,
                    ple =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateIntentionModule()));
                    });
                pl.SendGump(new DialogueGump(pl, pureIntentModule));
            });

        return ethicsModule;
    }

    private DialogueModule CreateTrainingModule()
    {
        DialogueModule trainingModule = new DialogueModule("To begin your training, you must first undergo a trial. Only those who prove their worth can learn the secrets of hemomancy. Are you prepared to face your trial?");
        trainingModule.AddOption("Yes, I'm ready for the trial.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Gravemold gestures, summoning a shadowy portal.");
                // Implement the trial logic here
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        trainingModule.AddOption("No, I need more time.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return trainingModule;
    }

    private DialogueModule CreateRitualsModule()
    {
        DialogueModule ritualsModule = new DialogueModule("Blood rituals can vary greatly in their effects. Some can enhance your powers, while others may curse you. Would you like to learn about a specific ritual or its general types?");
        ritualsModule.AddOption("Tell me about a specific ritual.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateSpecificRitualModule()));
            });
        ritualsModule.AddOption("What are the general types of blood rituals?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGeneralRitualTypesModule()));
            });
        return ritualsModule;
    }

    private DialogueModule CreateRitualPerformanceModule()
    {
        DialogueModule performanceModule = new DialogueModule("Performing a blood ritual requires specific ingredients and a proper incantation. You must also be in the right state of mind.");
        performanceModule.AddOption("What ingredients are needed?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateIngredientsModule()));
            });
        performanceModule.AddOption("What incantations must I learn?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateIncantationsModule()));
            });
        return performanceModule;
    }

    private DialogueModule CreateSacrificeModule()
    {
        DialogueModule sacrificeModule = new DialogueModule("Sacrifices can vary from small offerings of blood to significant life forces. Each sacrifice strengthens your connection to the blood magic.");
        sacrificeModule.AddOption("What is a small offering?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateSmallOfferingModule()));
            });
        sacrificeModule.AddOption("What about significant sacrifices?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateSignificantSacrificeModule()));
            });
        return sacrificeModule;
    }

    private DialogueModule CreateCostModule()
    {
        DialogueModule costModule = new DialogueModule("Using hemomancy exacts a toll on both the user and the target. The balance of life energy must be maintained, or dire consequences may follow.");
        costModule.AddOption("What are the consequences of imbalance?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateImbalanceConsequencesModule()));
            });
        return costModule;
    }

    private DialogueModule CreateSafeguardModule()
    {
        DialogueModule safeguardModule = new DialogueModule("To safeguard against losing control, one must practice meditation and maintain a clear focus on their intentions. Some practitioners carry charms or talismans to aid them.");
        safeguardModule.AddOption("What charms are effective?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateCharmsModule()));
            });
        return safeguardModule;
    }

    private DialogueModule CreateGoodUseModule()
    {
        DialogueModule goodUseModule = new DialogueModule("Using hemomancy for good requires a deep understanding of the balance of life and death. Your motives must be pure, and you must never act out of vengeance or greed.");
        goodUseModule.AddOption("How can I practice this responsibly?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return goodUseModule;
    }

    private DialogueModule CreateIntentionModule()
    {
        DialogueModule intentionModule = new DialogueModule("To ensure your intentions remain pure, reflect on your motives before every use of hemomancy. Keep a journal of your practices and the feelings behind them.");
        intentionModule.AddOption("That sounds wise.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return intentionModule;
    }

    private DialogueModule CreateSpecificRitualModule()
    {
        DialogueModule specificRitualModule = new DialogueModule("One powerful ritual involves binding a spirit to a vessel using your own blood. This requires a rare gemstone and a full moon to perform correctly.");
        specificRitualModule.AddOption("How do I perform this binding?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateBindingInstructionsModule()));
            });
        return specificRitualModule;
    }

    private DialogueModule CreateGeneralRitualTypesModule()
    {
        DialogueModule generalTypesModule = new DialogueModule("There are many types of blood rituals, including those for enhancement, protection, and summoning. Each type serves a different purpose.");
        generalTypesModule.AddOption("Tell me about enhancement rituals.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateEnhancementRitualsModule()));
            });
        generalTypesModule.AddOption("What about protection rituals?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateProtectionRitualsModule()));
            });
        generalTypesModule.AddOption("And summoning rituals?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateSummoningRitualsModule()));
            });
        return generalTypesModule;
    }

    private DialogueModule CreateEnhancementRitualsModule()
    {
        DialogueModule enhancementModule = new DialogueModule("Enhancement rituals amplify your magical abilities, allowing for greater power in spells. They often require rare ingredients and a personal sacrifice.");
        enhancementModule.AddOption("What ingredients are needed for these rituals?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return enhancementModule;
    }

    private DialogueModule CreateProtectionRitualsModule()
    {
        DialogueModule protectionModule = new DialogueModule("Protection rituals are designed to shield the practitioner from harm. They often involve creating blood wards or sigils.");
        protectionModule.AddOption("How do I create a blood ward?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return protectionModule;
    }

    private DialogueModule CreateSummoningRitualsModule()
    {
        DialogueModule summoningModule = new DialogueModule("Summoning rituals can call forth spirits or entities from other realms. They are incredibly complex and dangerous.");
        summoningModule.AddOption("What do I need to summon an entity?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return summoningModule;
    }

    private DialogueModule CreateBindingInstructionsModule()
    {
        DialogueModule bindingInstructionsModule = new DialogueModule("To bind a spirit, you must create a circle of protection, chant the binding words, and offer your blood as a sacrifice to seal the pact.");
        bindingInstructionsModule.AddOption("What are the binding words?",
            pl => true,
            pl =>
            {
                pl.SendMessage("The words are ancient and sacred, meant only for those who are ready to take on such a responsibility.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return bindingInstructionsModule;
    }

    private DialogueModule CreateIngredientsModule()
    {
        DialogueModule ingredientsModule = new DialogueModule("The ingredients for blood rituals vary widely, but commonly used items include rare herbs, gemstones, and, of course, your own blood.");
        ingredientsModule.AddOption("Where can I find these ingredients?",
            pl => true,
            pl =>
            {
                pl.SendMessage("Many can be found in remote areas or purchased from dark merchants.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return ingredientsModule;
    }

    private DialogueModule CreateIncantationsModule()
    {
        DialogueModule incantationsModule = new DialogueModule("Incantations for hemomancy often require deep focus and emotional connection to the blood being used. Practice is essential.");
        incantationsModule.AddOption("Can you teach me an incantation?",
            pl => true,
            pl =>
            {
                pl.SendMessage("Only when you have proven your commitment to this dark art.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return incantationsModule;
    }

    private DialogueModule CreateSmallOfferingModule()
    {
        DialogueModule smallOfferingModule = new DialogueModule("Small offerings can be as simple as a few drops of your blood mixed with herbs. These are often used to enhance spells.");
        smallOfferingModule.AddOption("How do I prepare it?",
            pl => true,
            pl =>
            {
                pl.SendMessage("Simply mix your blood with the chosen herbs and focus your intent on the desired outcome.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return smallOfferingModule;
    }

    private DialogueModule CreateSignificantSacrificeModule()
    {
        DialogueModule significantSacrificeModule = new DialogueModule("Significant sacrifices could mean giving up something dear to you or even risking your own life essence. Such acts are not to be taken lightly.");
        significantSacrificeModule.AddOption("What kind of things should I give up?",
            pl => true,
            pl =>
            {
                pl.SendMessage("It varies for each practitioner, but it could be a cherished item or a deeply held belief.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return significantSacrificeModule;
    }

    private DialogueModule CreateImbalanceConsequencesModule()
    {
        DialogueModule imbalanceConsequencesModule = new DialogueModule("If one loses balance while practicing hemomancy, it can lead to corruption, madness, or even death. The consequences are dire.");
        imbalanceConsequencesModule.AddOption("How can I recognize the signs of imbalance?",
            pl => true,
            pl =>
            {
                pl.SendMessage("Symptoms include erratic magical output, emotional instability, and nightmares related to your practices.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return imbalanceConsequencesModule;
    }

    private DialogueModule CreateCharmsModule()
    {
        DialogueModule charmsModule = new DialogueModule("Charms that help maintain control over hemomancy often include bloodstone amulets and wards inscribed with protective sigils.");
        charmsModule.AddOption("Where can I find these charms?",
            pl => true,
            pl =>
            {
                pl.SendMessage("Seek out dark merchants or ancient tombs. Such charms are rare and well-guarded.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return charmsModule;
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
