using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Major Zeta")]
public class MajorZeta : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MajorZeta() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Major Zeta";
        Body = 0x190; // Human male body

        // Stats
        Str = 90;
        Dex = 50;
        Int = 70;
        Hits = 60;

        // Appearance
        AddItem(new PlateLegs() { Hue = 1908 });
        AddItem(new PlateChest() { Hue = 1908 });
        AddItem(new PlateHelm() { Hue = 1908 });
        AddItem(new PlateGloves() { Hue = 1908 });
        AddItem(new Boots() { Hue = 1908 });
        AddItem(new FireballWand() { Name = "Major Zeta's Rifle" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public MajorZeta(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, I am Major Zeta, a scientist in these parts. How may I assist you?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateJobModule()));
            });

        greeting.AddOption("What do you know about knowledge?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateKnowledgeModule()));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                HandleReward(player);
            });

        greeting.AddOption("What do you think about the world?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateWorldViewModule()));
            });

        return greeting;
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My profession is that of a scientist. I spend my days conducting experiments and unraveling the mysteries of the world.");
        
        jobModule.AddOption("What kind of experiments do you conduct?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateExperimentModule()));
            });

        jobModule.AddOption("What inspired you to become a scientist?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateInspirationModule()));
            });

        return jobModule;
    }

    private DialogueModule CreateExperimentModule()
    {
        DialogueModule experimentModule = new DialogueModule("I conduct a variety of experiments, from alchemical processes to biological studies. Each one reveals new aspects of our universe.");

        experimentModule.AddOption("Tell me about your most recent experiment.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateRecentExperimentModule()));
            });

        experimentModule.AddOption("What are your favorite experiments?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateFavoriteExperimentsModule()));
            });

        return experimentModule;
    }

    private DialogueModule CreateRecentExperimentModule()
    {
        DialogueModule recentExperimentModule = new DialogueModule("Recently, I was studying the properties of a rare mineral I discovered. Its luminescence and energy properties suggest it might have extraterrestrial origins.");

        recentExperimentModule.AddOption("Extraterrestrial? Tell me more.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return recentExperimentModule;
    }

    private DialogueModule CreateFavoriteExperimentsModule()
    {
        DialogueModule favoriteExperimentsModule = new DialogueModule("One of my favorite experiments was a synthesis of various elements to create a potion that enhances mental acuity. The results were astonishing!");

        favoriteExperimentsModule.AddOption("What were the effects of that potion?",
            player => true,
            player =>
            {
                favoriteExperimentsModule.AddOption("It increased cognitive function and focus, but had some side effects like temporary visual distortions.", pl => true, pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, favoriteExperimentsModule));
            });

        return favoriteExperimentsModule;
    }

    private DialogueModule CreateInspirationModule()
    {
        DialogueModule inspirationModule = new DialogueModule("My inspiration comes from the wonders of the universe and the desire to unravel its secrets. I believe knowledge is the key to progress.");

        inspirationModule.AddOption("What do you hope to achieve with your research?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGoalsModule()));
            });

        return inspirationModule;
    }

    private DialogueModule CreateGoalsModule()
    {
        DialogueModule goalsModule = new DialogueModule("I aim to uncover lost knowledge and perhaps even unlock the secrets of ancient artifacts. Who knows what treasures lie in the past?");

        goalsModule.AddOption("What kind of artifacts are you looking for?",
            player => true,
            player =>
            {
                goalsModule.AddOption("I'm particularly interested in items with unique properties or magical abilities. Each artifact tells a story and could lead to new discoveries.", pl => true, pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, goalsModule));
            });

        return goalsModule;
    }

    private DialogueModule CreateKnowledgeModule()
    {
        DialogueModule knowledgeModule = new DialogueModule("The pursuit of knowledge is a virtue in itself. Do you value knowledge as well?");
        
        knowledgeModule.AddOption("Yes, I do!",
            player => true,
            player =>
            {
                knowledgeModule.AddOption("Knowledge is the foundation of understanding the universe. We must always seek to learn.", pl => true, pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, knowledgeModule));
            });

        knowledgeModule.AddOption("Not really.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return knowledgeModule;
    }

    private DialogueModule CreateWorldViewModule()
    {
        DialogueModule worldViewModule = new DialogueModule("The world is full of wonders and mysteries. I believe it's our duty to explore and learn from it.");

        worldViewModule.AddOption("What do you find most fascinating?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateFascinationModule()));
            });

        worldViewModule.AddOption("Do you think we'll ever understand everything?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateUnderstandingModule()));
            });

        return worldViewModule;
    }

    private DialogueModule CreateFascinationModule()
    {
        DialogueModule fascinationModule = new DialogueModule("I find the phenomena of the cosmos to be the most fascinating. The vastness and unknown possibilities fuel my curiosity.");

        fascinationModule.AddOption("What do you think about space travel?",
            player => true,
            player =>
            {
                fascinationModule.AddOption("Space travel is the next frontier! Imagine the discoveries awaiting us beyond our world.", pl => true, pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, fascinationModule));
            });

        return fascinationModule;
    }

    private DialogueModule CreateUnderstandingModule()
    {
        DialogueModule understandingModule = new DialogueModule("Understanding everything might be a lofty goal, but each discovery brings us closer. Every answer leads to more questions, and that's the beauty of knowledge.");

        understandingModule.AddOption("So we should never stop asking questions?",
            player => true,
            player =>
            {
                understandingModule.AddOption("Exactly! Curiosity is what drives innovation and progress.", pl => true, pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, understandingModule));
            });

        return understandingModule;
    }

    private void HandleReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            player.SendMessage("I have no reward right now. Please return later.");
        }
        else
        {
            player.SendMessage("For your efforts and interest, I shall grant you a special token of appreciation.");
            player.AddToBackpack(new ParryingAugmentCrystal()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule()));
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
