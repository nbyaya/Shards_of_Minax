using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class SirMorgar : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirMorgar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Morgar";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(90);
        SetInt(40);
        SetHits(85);

        // Appearance
        AddItem(new ChainChest() { Hue = 2417 });
        AddItem(new ChainLegs() { Hue = 2417 });
        AddItem(new ChainCoif() { Hue = 2417 });
        AddItem(new LeatherGloves() { Hue = 2417 });
        AddItem(new Boots() { Hue = 2417 });
        AddItem(new WarMace() { Name = "Sir Morgar's Mace" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
    }

    public SirMorgar(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Sir Morgar, the shadow in the night. Have you heard the delightful news? Cat-girls are real in this world!");

        greeting.AddOption("Tell me more about cat-girls!",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateCatGirlModule()));
            });

        greeting.AddOption("What do you think of them?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateOpinionModule()));
            });

        return greeting;
    }

    private DialogueModule CreateCatGirlModule()
    {
        DialogueModule catGirlModule = new DialogueModule("Ah, the enchanting cat-girls! They embody grace and playfulness, unlike any creature I've seen.");

        catGirlModule.AddOption("What makes them special?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateSpecialModule()));
            });

        catGirlModule.AddOption("Have you met any?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateMeetModule()));
            });

        return catGirlModule;
    }

    private DialogueModule CreateSpecialModule()
    {
        DialogueModule specialModule = new DialogueModule("Their elegance in movement is mesmerizing, and their playful demeanor brings joy to even the darkest of souls. It is said they possess a connection to magic!");

        specialModule.AddOption("Tell me more about their magic.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateMagicModule()));
            });

        specialModule.AddOption("Do they have any weaknesses?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateWeaknessesModule()));
            });

        return specialModule;
    }

    private DialogueModule CreateMagicModule()
    {
        DialogueModule magicModule = new DialogueModule("Some say they can charm creatures with their soft purring, and they have an innate ability to heal with their touch. Truly wondrous!");

        magicModule.AddOption("How can I learn their magic?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return magicModule;
    }

    private DialogueModule CreateWeaknessesModule()
    {
        DialogueModule weaknessesModule = new DialogueModule("Like all beings, they have their vulnerabilities. Their playful nature may lead them into trouble, and they are easily distracted by shiny objects.");

        weaknessesModule.AddOption("Interesting! What else can you tell me?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateMoreInfoModule()));
            });

        return weaknessesModule;
    }

    private DialogueModule CreateMoreInfoModule()
    {
        DialogueModule moreInfoModule = new DialogueModule("I’ve heard rumors of a hidden village where they reside in harmony with nature. It is said that if you bring them gifts, they may reveal their secrets.");

        moreInfoModule.AddOption("What kind of gifts?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGiftsModule()));
            });

        return moreInfoModule;
    }

    private DialogueModule CreateGiftsModule()
    {
        DialogueModule giftsModule = new DialogueModule("They are fond of rare fish, soft blankets, and trinkets that sparkle in the sunlight. Such gifts may earn their favor!");

        giftsModule.AddOption("I will seek these gifts!",
            player => true,
            player =>
            {
                player.SendMessage("You set off on a quest to find the perfect gifts for the cat-girls!");
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return giftsModule;
    }

    private DialogueModule CreateMeetModule()
    {
        DialogueModule meetModule = new DialogueModule("Indeed! I encountered a group of them near the Whispering Forest. They were dancing among the trees, their laughter echoing like sweet music.");

        meetModule.AddOption("What did they look like?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateAppearanceModule()));
            });

        return meetModule;
    }

    private DialogueModule CreateAppearanceModule()
    {
        DialogueModule appearanceModule = new DialogueModule("Their ears were perked up, and their tails swayed gracefully. Each had unique markings and colors that reflected their playful spirits.");

        appearanceModule.AddOption("Do they wear anything special?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateAttireModule()));
            });

        return appearanceModule;
    }

    private DialogueModule CreateAttireModule()
    {
        DialogueModule attireModule = new DialogueModule("They often adorn themselves with vibrant fabrics, adorned with beads and feathers that capture the sunlight beautifully.");

        attireModule.AddOption("Sounds captivating! Can they be trusted?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateTrustModule()));
            });

        return attireModule;
    }

    private DialogueModule CreateTrustModule()
    {
        DialogueModule trustModule = new DialogueModule("As with all beings, trust must be earned. They seem playful, yet their cunning is unmatched. Approach with kindness, and they may surprise you.");

        trustModule.AddOption("I will approach them carefully.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return trustModule;
    }

    private DialogueModule CreateOpinionModule()
    {
        DialogueModule opinionModule = new DialogueModule("They bring joy and a sense of wonder to our world. Their existence is a reminder that magic is real, and life can be full of surprises.");

        opinionModule.AddOption("What do you think about their magic?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateMagicOpinionModule()));
            });

        return opinionModule;
    }

    private DialogueModule CreateMagicOpinionModule()
    {
        DialogueModule magicOpinionModule = new DialogueModule("Their magic is unlike anything I have witnessed! It can heal wounds and mend broken hearts. I find it utterly enchanting.");

        magicOpinionModule.AddOption("Can you teach me about magic?",
            player => true,
            player =>
            {
                player.SendMessage("Magic requires dedication and an open heart. Seek out the wise and learn from them.");
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return magicOpinionModule;
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
