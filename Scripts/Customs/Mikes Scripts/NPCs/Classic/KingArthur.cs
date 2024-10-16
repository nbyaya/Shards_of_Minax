using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of King Arthur")]
public class KingArthur : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KingArthur() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "King Arthur";
        Body = 0x190; // Human male body

        // Stats
        Str = 130;
        Dex = 70;
        Int = 100;
        Hits = 90;

        // Appearance
        AddItem(new PlateLegs() { Hue = 2213 });
        AddItem(new PlateChest() { Hue = 2213 });
        AddItem(new CloseHelm() { Hue = 2213 });
        AddItem(new PlateGloves() { Hue = 2213 });
        AddItem(new Boots() { Hue = 2213 });
        AddItem(new Longsword() { Name = "Excalibur" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
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
        DialogueModule greeting = new DialogueModule("I am King Arthur, the noble ruler of this land. How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateHealthModule())));
        
        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateJobModule())));
        
        greeting.AddOption("Tell me about virtue and justice.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateJusticeModule())));
        
        greeting.AddOption("What about Excalibur?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateExcaliburModule())));
        
        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player => HandleReward(player));
        
        greeting.AddOption("Tell me about Camelot.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateCamelotModule())));
        
        greeting.AddOption("What challenges does your kingdom face?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateChallengesModule())));
        
        greeting.AddOption("Tell me about the Holy Grail.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGrailModule())));

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("I am in good health, thanks to the wisdom of Merlin. He has been a guiding light through many trials.");
    }

    private DialogueModule CreateJobModule()
    {
        return new DialogueModule("My noble duty is to uphold the virtue of Justice in my kingdom. Each day, I strive to protect the innocent and maintain order.");
    }

    private DialogueModule CreateJusticeModule()
    {
        DialogueModule module = new DialogueModule("Justice is the foundation of a fair and honorable society. Do you believe in justice?");
        module.AddOption("Yes.", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("Indeed, justice is the cornerstone of a virtuous society. It is our duty to uphold it. Tell me, what does justice mean to you?"))));
        module.AddOption("No.", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("I see. It is a difficult path to walk. Perhaps you would consider the meaning of justice more deeply?"))));
        return module;
    }

    private DialogueModule CreateExcaliburModule()
    {
        DialogueModule module = new DialogueModule("Excalibur is not just a sword. It represents the heart and soul of this kingdom. Only the true ruler can wield it.");
        module.AddOption("What makes you the true ruler?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("A ruler is defined not just by power, but by wisdom, courage, and compassion. I strive to embody these qualities every day."))));
        module.AddOption("Can I wield Excalibur?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("To wield Excalibur, one must prove their worth. Perhaps a quest could demonstrate your valor."))));
        return module;
    }

    private void HandleReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
        }
        else
        {
            player.SendGump(new DialogueGump(player, new DialogueModule("Your dedication to our kingdom's cause is commendable. As a token of appreciation, accept this gift from the royal treasury.")));
            player.AddToBackpack(new ArmSlotChangeDeed()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }
    }

    private DialogueModule CreateCamelotModule()
    {
        DialogueModule module = new DialogueModule("Camelot is the jewel of our land, a fortress of hope and a beacon of resilience.");
        module.AddOption("What is special about Camelot?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("Camelot is renowned for its beauty and strength. It is a gathering place for knights, scholars, and those seeking justice."))));
        module.AddOption("Tell me about the knights.", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("The Knights of the Round Table are my most trusted companions. Each one embodies the virtues of chivalry, loyalty, and honor."))));
        return module;
    }

    private DialogueModule CreateChallengesModule()
    {
        DialogueModule module = new DialogueModule("Our kingdom faces threats from both within and outside, including Morgana and invaders from distant lands.");
        module.AddOption("Who is Morgana?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("Morgana is a sorceress and former ally turned adversary. Her cunning and ambition pose a great danger to our realm."))));
        module.AddOption("What can be done to protect Camelot?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("We must remain vigilant, training our knights and fortifying our defenses. It is also vital to gather intelligence about our enemies."))));
        return module;
    }

    private DialogueModule CreateGrailModule()
    {
        DialogueModule module = new DialogueModule("The Holy Grail, a relic of immense power and significance. Many have sought it, but its true location remains a mystery.");
        module.AddOption("What powers does the Grail hold?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("The Grail is said to grant eternal life and heal the gravest wounds. Its power is unmatched, but so too is its mystery."))));
        module.AddOption("How can one seek the Grail?", p => true, p => p.SendGump(new DialogueGump(p, new DialogueModule("The quest for the Grail is fraught with peril. It requires a pure heart, great courage, and an unwavering spirit."))));
        return module;
    }

    public KingArthur(Serial serial) : base(serial) { }

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
