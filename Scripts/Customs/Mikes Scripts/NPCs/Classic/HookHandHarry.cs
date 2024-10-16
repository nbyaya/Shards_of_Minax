using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Hook Hand Harry")]
public class HookHandHarry : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public HookHandHarry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Hook Hand Harry";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(80);
        SetInt(40);
        SetHits(80);

        // Appearance
        AddItem(new LongPants() { Hue = 1904 });
        AddItem(new FancyShirt() { Hue = 64 });
        AddItem(new Boots() { Hue = 64 });
        AddItem(new TricorneHat() { Hue = 1904 });
        AddItem(new Cutlass() { Name = "Harry's Hook" });
        AddItem(new GoldRing() { Name = "Harry's Eye Patch" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
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
        DialogueModule greeting = new DialogueModule("Ahoy, matey! I be Hook Hand Harry, the fiercest pirate to sail these treacherous seas!");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("Tell me about your battles.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateBattlesModule())); });

        greeting.AddOption("Do you have any treasure?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTreasureModule())); });

        greeting.AddOption("Can I earn a reward?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateRewardModule(player))); });

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        DialogueModule healthModule = new DialogueModule("Me health be as sturdy as a ship's hull in a storm!");

        healthModule.AddOption("What do you do to stay healthy?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthTipsModule())); });

        healthModule.AddOption("Have you ever been injured?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateInjuryModule())); });

        return healthModule;
    }

    private DialogueModule CreateHealthTipsModule()
    {
        DialogueModule tipsModule = new DialogueModule("A true pirate keeps his spirit high and his belly full!");

        tipsModule.AddOption("What do you eat?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFoodModule())); });

        tipsModule.AddOption("Any special drinks?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDrinkModule())); });

        return tipsModule;
    }

    private DialogueModule CreateFoodModule()
    {
        return new DialogueModule("I feast on whatever I can find—dried meats, hardtack, and the occasional fish caught fresh from the sea!");
    }

    private DialogueModule CreateDrinkModule()
    {
        return new DialogueModule("Rum! 'Tis the drink of choice for a pirate! It warms the heart and lifts the spirits!");
    }

    private DialogueModule CreateInjuryModule()
    {
        return new DialogueModule("Aye, I lost me hand to a monstrous shark! A fierce battle that left me with a hook instead. But I emerged victorious!");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("I be a pirate through and through, a swashbuckler of the high seas!");

        jobModule.AddOption("What’s your best adventure?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAdventureModule())); });

        jobModule.AddOption("Have you ever considered retiring?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateRetirementModule())); });

        return jobModule;
    }

    private DialogueModule CreateAdventureModule()
    {
        return new DialogueModule("Ah, the best adventure be when me crew and I plundered the Spanish galleon 'Golden Dove'! A treasure trove of riches awaited us!");
    }

    private DialogueModule CreateRetirementModule()
    {
        return new DialogueModule("Retire? Nay! The sea be my life! The thrill of adventure keeps me young at heart!");
    }

    private DialogueModule CreateBattlesModule()
    {
        DialogueModule battlesModule = new DialogueModule("Ye see, true valor be in outwittin' yer foes and takin' risks when the booty be temptin'! Be ye valiant, matey?");

        battlesModule.AddOption("Tell me about a great battle.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreatBattleModule())); });

        battlesModule.AddOption("Do you fear anything?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFearModule())); });

        return battlesModule;
    }

    private DialogueModule CreateGreatBattleModule()
    {
        return new DialogueModule("One time, we faced the notorious Captain Redbeard and his crew! The clash of steel echoed for miles, but we claimed victory and his treasure!");
    }

    private DialogueModule CreateFearModule()
    {
        return new DialogueModule("Fear? A pirate fears naught but the depths of the ocean! But the thought of a cursed treasure sends shivers down me spine!");
    }

    private DialogueModule CreateTreasureModule()
    {
        DialogueModule treasureModule = new DialogueModule("Legends speak of the 'Heart of the Sea', a gem so radiant it's said to outshine the moon. I've been searchin' for it me whole life.");

        treasureModule.AddOption("Where do you think it lies?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateLocationModule())); });

        treasureModule.AddOption("Have you ever found treasure?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFoundTreasureModule())); });

        return treasureModule;
    }

    private DialogueModule CreateLocationModule()
    {
        return new DialogueModule("Aye, it be hidden in the Cave of Whispers, guarded by the spirits of the damned!");
    }

    private DialogueModule CreateFoundTreasureModule()
    {
        return new DialogueModule("Once I discovered a chest of gold doubloons on a deserted isle! A real stroke of luck!");
    }

    private DialogueModule CreateRewardModule(PlayerMobile player)
    {
        DialogueModule rewardModule = new DialogueModule("Ah, so ye wish to earn me reward? Very well.");

        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            rewardModule.NPCText += " I have no reward right now. Please return later.";
        }
        else
        {
            rewardModule.NPCText += " Bring me a rare golden doubloon from the sunken ship 'Siren's Lament', and I shall reward ye handsomely. A taste, here.";
            player.AddToBackpack(new MaxxiaScroll()); // Example item; replace with the actual reward item
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }

        return rewardModule;
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

    public HookHandHarry(Serial serial) : base(serial) { }
}
