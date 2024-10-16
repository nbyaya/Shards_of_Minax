using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Marie Antoinette")]
public class MarieAntoinette : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MarieAntoinette() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marie Antoinette";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(60);
        SetInt(95);
        SetHits(75);

        // Appearance
        AddItem(new FancyDress() { Hue = 1157 }); // Fancy dress with hue 1157
        AddItem(new Sandals() { Hue = 1905 }); // Sandals with hue 1905

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime
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
        DialogueModule greeting = new DialogueModule("Ah! Welcome, traveler! I am Marie Antoinette from the exquisite land of France. What knowledge or tales do you seek?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateHealthModule()));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateJobModule()));
            });

        greeting.AddOption("What do you think about virtues?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateVirtueModule()));
            });

        greeting.AddOption("Tell me about the revolution.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateRevolutionModule()));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player => {
                player.SendMessage("Farewell! May your path be filled with wisdom.");
            });

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        DialogueModule healthModule = new DialogueModule("Maintaining my integrity is a challenge, akin to preserving a delicate flower in a storm.");

        healthModule.AddOption("What do you mean by integrity?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateIntegrityModule()));
            });

        healthModule.AddOption("Do you have any remedies?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateRemedyModule()));
            });

        healthModule.AddOption("How can one improve their health?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateHealthAdviceModule()));
            });

        return healthModule;
    }

    private DialogueModule CreateIntegrityModule()
    {
        return new DialogueModule("Integrity is the unwavering commitment to one's principles. In my world, it is a precious asset, one that is often tested.");
    }

    private DialogueModule CreateRemedyModule()
    {
        return new DialogueModule("I rely on herbal concoctions and the wisdom of experienced healers. Have you ever tried using herbs for healing?");
    }

    private DialogueModule CreateHealthAdviceModule()
    {
        DialogueModule adviceModule = new DialogueModule("To improve one's health, one must balance mind, body, and spirit. Meditation, exercise, and proper nutrition are vital.");

        adviceModule.AddOption("What exercises do you recommend?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateExerciseModule()));
            });

        adviceModule.AddOption("Tell me about nutrition.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateNutritionModule()));
            });

        return adviceModule;
    }

    private DialogueModule CreateExerciseModule()
    {
        return new DialogueModule("I find that dancing and walking in nature uplift both spirit and body. Movement is essential to vitality.");
    }

    private DialogueModule CreateNutritionModule()
    {
        return new DialogueModule("Eating a variety of fresh fruits and vegetables, along with lean meats, can nourish the body. Have you tasted the delights of the garden?");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My role is to inspire compassion in those I meet. I believe in the power of kindness to change the world.");

        jobModule.AddOption("How do you inspire compassion?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateInspirationModule()));
            });

        jobModule.AddOption("Do you believe in charity?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCharityModule()));
            });

        return jobModule;
    }

    private DialogueModule CreateInspirationModule()
    {
        return new DialogueModule("I share stories of courage and empathy, reminding others of the light that resides in every heart.");
    }

    private DialogueModule CreateCharityModule()
    {
        DialogueModule charityModule = new DialogueModule("Charity is essential in our world. It is the act of giving without expecting anything in return.");

        charityModule.AddOption("How can I contribute?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateContributionModule()));
            });

        return charityModule;
    }

    private DialogueModule CreateContributionModule()
    {
        return new DialogueModule("Every small act of kindness counts. You can help a stranger or share your knowledge with those in need.");
    }

    private DialogueModule CreateVirtueModule()
    {
        DialogueModule virtueModule = new DialogueModule("Justice is a mirror reflecting one's own actions. Virtue is the foundation upon which we build our lives.");

        virtueModule.AddOption("What are the core virtues?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCoreVirtuesModule()));
            });

        return virtueModule;
    }

    private DialogueModule CreateCoreVirtuesModule()
    {
        return new DialogueModule("The core virtues include courage, honesty, compassion, and humility. Each plays a vital role in the tapestry of life.");
    }

    private DialogueModule CreateRevolutionModule()
    {
        DialogueModule revolutionModule = new DialogueModule("The Revolution was a time of great change and strife in France. Many were sacrificed in the name of equality.");

        if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
        {
            revolutionModule.AddOption("I would like to know more.",
                player => true,
                player => {
                    player.SendMessage("I have no reward right now. Please return later.");
                });
        }
        else
        {
            revolutionModule.AddOption("Tell me more!",
                player => true,
                player => {
                    player.SendMessage("Here, take this for showing interest in history.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                });
        }

        revolutionModule.AddOption("What caused the Revolution?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCausesModule()));
            });

        revolutionModule.AddOption("Was it worth it?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateWorthModule()));
            });

        return revolutionModule;
    }

    private DialogueModule CreateCausesModule()
    {
        return new DialogueModule("The Revolution was ignited by widespread discontent with inequality, the burden of taxes, and the thirst for freedom and rights.");
    }

    private DialogueModule CreateWorthModule()
    {
        return new DialogueModule("It is difficult to measure worth in times of upheaval. Many lives were lost, but it paved the way for the rights we cherish today.");
    }

    public MarieAntoinette(Serial serial) : base(serial) { }

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
