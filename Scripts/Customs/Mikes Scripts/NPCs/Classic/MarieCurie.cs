using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Marie Curie")]
public class MarieCurie : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MarieCurie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marie Curie";
        Body = 0x191; // Human female body
        Str = 60;
        Dex = 60;
        Int = 120;
        Hits = 50;

        AddItem(new Robe() { Hue = 2301 });
        AddItem(new Sandals() { Hue = 2301 });
        AddItem(new GnarledStaff() { Name = "Marie's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Oh, it's you again. What do you want?");
        
        greeting.AddOption("What is your name?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateNameModule())));

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateHealthModule())));

        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateJobModule())));

        greeting.AddOption("Do you have any battles to share?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateBattlesModule())));

        greeting.AddOption("Can you teach me about your research?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateResearchModule())));

        return greeting;
    }

    private DialogueModule CreateNameModule()
    {
        DialogueModule nameModule = new DialogueModule("Marie Curie, that's who I am. I hail from a place called France. Ever heard of it?");
        nameModule.AddOption("What’s special about France?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateFranceModule())));
        return nameModule;
    }

    private DialogueModule CreateFranceModule()
    {
        DialogueModule franceModule = new DialogueModule("France, the land of wine and wonders. A place filled with rich history and beautiful landscapes. You should visit if you can.");
        franceModule.AddOption("What are the famous landmarks?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateLandmarksModule())));
        franceModule.AddOption("What about the culture?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateCultureModule())));
        return franceModule;
    }

    private DialogueModule CreateLandmarksModule()
    {
        return new DialogueModule("Some notable landmarks include the Eiffel Tower, the Louvre Museum, and the Palace of Versailles. Each tells a story of our glorious past.");
    }

    private DialogueModule CreateCultureModule()
    {
        DialogueModule cultureModule = new DialogueModule("French culture is vibrant and diverse, known for its art, fashion, and culinary delights. Have you ever tried a real French pastry?");
        cultureModule.AddOption("Yes, I love pastries!",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreatePastryModule())));
        cultureModule.AddOption("I haven't had the chance yet.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreatePastryAdviceModule())));
        return cultureModule;
    }

    private DialogueModule CreatePastryModule()
    {
        return new DialogueModule("Then you must try a croissant! Flaky, buttery, and best enjoyed fresh. Pair it with a café au lait for the full experience.");
    }

    private DialogueModule CreatePastryAdviceModule()
    {
        return new DialogueModule("You should definitely try them. When you do, make sure to find a local bakery; they make all the difference!");
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("Health? What's it to you? I'm not your personal physician.");
    }

    private DialogueModule CreateJobModule()
    {
        return new DialogueModule("My 'job'? I'm stuck here answering your silly questions. That's my job, I suppose.");
    }

    private DialogueModule CreateBattlesModule()
    {
        return new DialogueModule("Valiant? Hah! Do I look valiant to you?");
    }

    private DialogueModule CreateResearchModule()
    {
        DialogueModule researchModule = new DialogueModule("I studied radioactivity and made many discoveries, including radium and polonium. It’s a field that brings both admiration and challenges.");
        researchModule.AddOption("What are your discoveries?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateDiscoveriesModule())));
        researchModule.AddOption("What challenges did you face?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateChallengesModule())));
        return researchModule;
    }

    private DialogueModule CreateDiscoveriesModule()
    {
        DialogueModule discoveriesModule = new DialogueModule("I discovered radium in 1898, a groundbreaking finding that opened new doors in medical treatments. Polonium was another significant discovery, though lesser-known.");
        discoveriesModule.AddOption("What is radium used for?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateRadiumUsesModule())));
        return discoveriesModule;
    }

    private DialogueModule CreateRadiumUsesModule()
    {
        return new DialogueModule("Radium has been used in cancer treatments, helping to target and eliminate harmful cells. However, it must be handled with care!");
    }

    private DialogueModule CreateChallengesModule()
    {
        DialogueModule challengesModule = new DialogueModule("I faced many challenges, including skepticism from peers and the dangers of handling radioactive materials. It was a perilous path.");
        challengesModule.AddOption("How did you overcome skepticism?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateSkepticismModule())));
        return challengesModule;
    }

    private DialogueModule CreateSkepticismModule()
    {
        return new DialogueModule("I continued my research with unwavering determination, proving my findings through rigorous experimentation and persistence.");
    }

    private DialogueModule CreateRadiumModule()
    {
        DialogueModule radiumModule = new DialogueModule("Radium, a fascinating element I once researched. It's elusive and dangerous. I could give you a sample, but only if you promise to handle it with care.");
        radiumModule.AddOption("I promise to handle it with care.",
            player => CanReward(player),
            player => 
            {
                GiveReward(player);
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });
        radiumModule.AddOption("Never mind.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
        return radiumModule;
    }

    private bool CanReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveReward(PlayerMobile player)
    {
        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
    }

    public MarieCurie(Serial serial) : base(serial) { }

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
