using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lila the Explorer")]
public class LilaTheExplorer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LilaTheExplorer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lila the Explorer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(40);
        SetDex(40);
        SetInt(80);
        SetHits(60);

        // Appearance
        AddItem(new Skirt() { Hue = 38 });
        AddItem(new FancyShirt() { Hue = 64 });
        AddItem(new Boots() { Hue = 64 });
        AddItem(new FeatheredHat() { Hue = 38 });
        AddItem(new Cloak() { Name = "Lila's Map" });

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
        DialogueModule greeting = new DialogueModule("I'm Lila the Explorer, cartographer extraordinaire. What would you like to know?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My 'job'? I'm a cartographer, drawing maps for the ungrateful masses.");
                jobModule.AddOption("What do you think of health?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateHealthModule())); });
                jobModule.AddOption("What challenges do you face?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateChallengesModule())); });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you know about exploring?",
            player => true,
            player =>
            {
                DialogueModule exploreModule = new DialogueModule("Interested in exploring the wilds? Do you have the guts to face the unknown?");
                exploreModule.AddOption("Yes, I do!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateYesModule())); });
                exploreModule.AddOption("No, I’m hesitant.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateHesitantModule())); });
                player.SendGump(new DialogueGump(player, exploreModule));
            });

        greeting.AddOption("Tell me about Trinsic.",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Ah, Trinsic. A coastal city with sturdy walls. It’s known for its brave knights and beautiful beaches. But what lies beyond the city's boundaries is where the true adventure begins.")));
            });

        greeting.AddOption("What about Yew?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Yew's forests are filled with mysteries. Some say spirits of ancient guardians still roam there. It's a place of both beauty and danger.")));
            });

        greeting.AddOption("Give me a reward for bravery.",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                player.SendMessage("I've faced many perils. Take this - may it serve you well on your adventures.");
                player.AddToBackpack(new CartographyAugmentCrystal()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            });

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        DialogueModule healthModule = new DialogueModule("Health? Ha, who cares about health when you're mapping the unknown? But it's essential, I suppose. Sometimes I’ve pushed myself too far during expeditions.");
        healthModule.AddOption("Have you ever been injured?",
            pl => true,
            pl => 
            {
                DialogueModule injuryModule = new DialogueModule("Injured? Yes, several times! Once I slipped in a ravine while chasing a rare bird. Luckily, I only sprained my ankle, but it taught me to be more careful.");
                injuryModule.AddOption("What did you learn from that experience?",
                    pla => true,
                    pla => 
                    {
                        pla.SendGump(new DialogueGump(pla, new DialogueModule("Always watch your step and trust your instincts. Nature is unpredictable, and I respect that. Always keep a healer's kit on hand!")));
                    });
                pl.SendGump(new DialogueGump(pl, injuryModule));
            });
        return healthModule;
    }

    private DialogueModule CreateChallengesModule()
    {
        DialogueModule challengesModule = new DialogueModule("Challenges? Oh, countless! From navigating through thick fog to facing aggressive wildlife. But the thrill of discovery makes it all worthwhile.");
        challengesModule.AddOption("What’s the most dangerous situation you faced?",
            pl => true,
            pl =>
            {
                DialogueModule dangerModule = new DialogueModule("Once, I found myself cornered by a bear while trying to sketch a rare flower. I had to climb a tree to escape! It was terrifying but exhilarating.");
                dangerModule.AddOption("How did you escape?",
                    pla => true,
                    pla => 
                    {
                        pla.SendGump(new DialogueGump(pla, new DialogueModule("I waited patiently until it lost interest. When the coast was clear, I made my escape. Nature teaches patience and caution.")));
                    });
                pl.SendGump(new DialogueGump(pl, dangerModule));
            });
        return challengesModule;
    }

    private DialogueModule CreateYesModule()
    {
        DialogueModule yesModule = new DialogueModule("Oh, you're brave! We'll see if you're still fearless after exploring a few treacherous places. What kind of exploration are you interested in?");
        yesModule.AddOption("Discovering hidden treasures.",
            pl => true,
            pl =>
            {
                DialogueModule treasureModule = new DialogueModule("Ah, treasure! It often lies in the most dangerous places. Have you heard of the Ruins of Eldor? They say there's gold hidden there, but many have perished seeking it.");
                treasureModule.AddOption("What should I know before going?",
                    pla => true,
                    pla => 
                    {
                        pla.SendGump(new DialogueGump(pla, new DialogueModule("Prepare well! Gather provisions, study old maps, and ensure you have company. The ruins are filled with traps and wild beasts!")));
                    });
                pl.SendGump(new DialogueGump(pl, treasureModule));
            });
        yesModule.AddOption("Exploring ancient forests.",
            pl => true,
            pl =>
            {
                DialogueModule forestModule = new DialogueModule("The ancient forests hold many secrets! Some say they are enchanted, home to creatures not seen by ordinary eyes.");
                forestModule.AddOption("What kind of creatures?",
                    pla => true,
                    pla => 
                    {
                        pla.SendGump(new DialogueGump(pla, new DialogueModule("Spirits, mythical beasts, and perhaps even lost souls. Always tread carefully; respect their home.")));
                    });
                pl.SendGump(new DialogueGump(pl, forestModule));
            });
        return yesModule;
    }

    private DialogueModule CreateHesitantModule()
    {
        return new DialogueModule("Hesitant, are you? That’s wise. Not all adventures are for the faint-hearted. Maybe you should start with small excursions.");
    }

    public LilaTheExplorer(Serial serial) : base(serial) { }

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
