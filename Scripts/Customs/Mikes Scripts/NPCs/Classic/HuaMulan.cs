using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Hua Mulan")]
public class HuaMulan : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public HuaMulan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Hua Mulan";
        Body = 0x190; // Human female body

        // Stats
        Str = 110;
        Dex = 110;
        Int = 70;
        Hits = 85;

        // Appearance
        AddItem(new LeatherChest() { Hue = 1158 });
        AddItem(new Boots() { Hue = 1158 });
        AddItem(new Longsword() { Name = "Mulan's Sword" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public HuaMulan(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Hua Mulan, a warrior from the Middle Kingdom. How can I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("What can you tell me about the Middle Kingdom?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMiddleKingdomModule())); });

        greeting.AddOption("Do you have any rewards for me?",
            player => CanReceiveReward(player),
            player => { GiveReward(player); });

        greeting.AddOption("Tell me about your sword.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSwordModule())); });

        greeting.AddOption("Who is Mushu?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMushuModule())); });

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        DialogueModule health = new DialogueModule("My health is strong, for I have endured many battles. But tell me, do you wish to know about the secrets of maintaining strength?");
        health.AddOption("Yes, please tell me more.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthSecretsModule())); });
        health.AddOption("No, I'm more interested in your stories.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });
        return health;
    }

    private DialogueModule CreateHealthSecretsModule()
    {
        return new DialogueModule("A strong mind is just as important as a strong body. Daily meditation helps clear the mind. Also, consuming nourishing foods like roots and herbs can greatly enhance vitality.");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule job = new DialogueModule("I am a warrior, dedicated to protecting my people and land. My training has been rigorous and my duties many. Would you like to know about my training or my missions?");
        job.AddOption("Tell me about your training.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTrainingModule())); });
        job.AddOption("What missions have you undertaken?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMissionsModule())); });
        return job;
    }

    private DialogueModule CreateTrainingModule()
    {
        return new DialogueModule("I trained under the finest masters, learning the art of combat and strategy. Each day was filled with rigorous drills and sparring matches. The most valuable lesson was to always respect my opponents.");
    }

    private DialogueModule CreateMissionsModule()
    {
        DialogueModule missions = new DialogueModule("I have undertaken many missions to protect my homeland. Once, I led a group to recover stolen artifacts from a band of raiders. We succeeded through teamwork and strategy.");
        missions.AddOption("That sounds intense! Any other adventures?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAdventureStoriesModule())); });
        return missions;
    }

    private DialogueModule CreateAdventureStoriesModule()
    {
        DialogueModule adventures = new DialogueModule("Indeed! Another time, I journeyed to a distant village to quell a fearsome beast threatening their livestock. It took all my courage and skill to face it. But with determination, we triumphed.");
        adventures.AddOption("What kind of beast was it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateBeastModule())); });
        return adventures;
    }

    private DialogueModule CreateBeastModule()
    {
        return new DialogueModule("It was a giant wolf, its eyes gleaming with malice. It had terrorized the village for weeks, but with the help of the villagers, we set a trap and managed to capture it.");
    }

    private DialogueModule CreateMiddleKingdomModule()
    {
        DialogueModule middleKingdom = new DialogueModule("The Middle Kingdom is a land of beauty and tradition, where rivers and mountains hold tales of ancient legends. Do you seek to learn about its history or its people?");
        middleKingdom.AddOption("Tell me about its history.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHistoryModule())); });
        middleKingdom.AddOption("What about the people?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreatePeopleModule())); });
        return middleKingdom;
    }

    private DialogueModule CreateHistoryModule()
    {
        return new DialogueModule("The history of the Middle Kingdom is rich with stories of bravery, honor, and sacrifice. Many heroes have walked these lands, and their tales are etched into the very soil.");
    }

    private DialogueModule CreatePeopleModule()
    {
        return new DialogueModule("The people are diverse, each with their own traditions and customs. We celebrate the harvest with grand feasts and honor our ancestors with festivals throughout the year.");
    }

    private bool CanReceiveReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveReward(PlayerMobile player)
    {
        if (CanReceiveReward(player))
        {
            Say("The reward was a pendant, symbolic of bravery and honor. I can see you possess similar qualities. Take this as a token of my appreciation.");
            player.AddToBackpack(new MaxxiaScroll());
            lastRewardTime = DateTime.UtcNow;
        }
        else
        {
            Say("I have no reward right now. Please return later.");
        }
    }

    private DialogueModule CreateSwordModule()
    {
        DialogueModule sword = new DialogueModule("The sword was said to possess magical properties, able to cut through the hardest of substances. It was forged in the fires of a great mountain and imbued with the spirit of a hero.");
        sword.AddOption("What makes it so special?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSwordSpecialModule())); });
        return sword;
    }

    private DialogueModule CreateSwordSpecialModule()
    {
        return new DialogueModule("Legends say it can only be wielded by those who are pure of heart. Many have sought it, but few have found its resting place, hidden deep within the enchanted forests.");
    }

    private DialogueModule CreateMushuModule()
    {
        DialogueModule mushu = new DialogueModule("Mushu is a spirited dragon guardian, always eager to offer advice, whether I ask for it or not! His humor and wit have saved me from despair on many occasions.");
        mushu.AddOption("Can you tell me a story about Mushu?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMushuStoryModule())); });
        return mushu;
    }

    private DialogueModule CreateMushuStoryModule()
    {
        return new DialogueModule("Once, when I was downcast, Mushu played a prank on a group of soldiers, distracting them just long enough for me to sneak past! His antics always lighten the mood.");
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
