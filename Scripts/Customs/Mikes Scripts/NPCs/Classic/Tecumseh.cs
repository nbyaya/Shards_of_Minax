using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Tecumseh")]
public class Tecumseh : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Tecumseh() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tecumseh";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(110);
        SetInt(70);
        SetHits(80);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1109 });
        AddItem(new LeatherChest() { Hue = 1109 });
        AddItem(new LeatherGloves() { Hue = 1109 });
        AddItem(new Bandana() { Hue = 1112 });
        AddItem(new Bardiche() { Name = "Tecumseh's Axe" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public Tecumseh(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Thou standest before Tecumseh, a watcher of the land. How may I assist you?");

        greeting.AddOption("Tell me about your struggle against the settlers.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateStruggleModule())); });

        greeting.AddOption("What is your vision for the future?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateVisionModule())); });

        greeting.AddOption("How can I help your people?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHelpModule())); });

        return greeting;
    }

    private DialogueModule CreateStruggleModule()
    {
        DialogueModule struggleModule = new DialogueModule("The settlers come with fire and greed, seeking to take what is not theirs. We stand firm, united in our purpose to protect our lands and our way of life.");

        struggleModule.AddOption("What tactics do you use in your resistance?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTacticsModule())); });

        struggleModule.AddOption("Tell me about your allies.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAlliesModule())); });

        struggleModule.AddOption("What do the settlers hope to achieve?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSettlerGoalsModule())); });

        return struggleModule;
    }

    private DialogueModule CreateTacticsModule()
    {
        DialogueModule tacticsModule = new DialogueModule("We employ stealth and strategy, striking when least expected. The land is our ally; we know it well, allowing us to outmaneuver them in the shadows.");

        tacticsModule.AddOption("Have you won many battles?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateBattleWinsModule())); });

        tacticsModule.AddOption("What about the civilians caught in between?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateCivilianModule())); });

        return tacticsModule;
    }

    private DialogueModule CreateBattleWinsModule()
    {
        return new DialogueModule("Yes, but each victory comes at a cost. We mourn our losses, and each battle strengthens our resolve to continue the fight.");
    }

    private DialogueModule CreateCivilianModule()
    {
        DialogueModule civilianModule = new DialogueModule("The innocents suffer most in this conflict. We strive to protect our villages and our people, while also seeking to avoid unnecessary bloodshed.");

        civilianModule.AddOption("How can you protect them?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateProtectionModule())); });

        return civilianModule;
    }

    private DialogueModule CreateProtectionModule()
    {
        return new DialogueModule("We teach them to be vigilant, to know the signs of danger. Our scouts keep watch over the lands, and we build alliances to provide further strength.");
    }

    private DialogueModule CreateAlliesModule()
    {
        DialogueModule alliesModule = new DialogueModule("Many tribes unite under a common banner, recognizing that together we are stronger against the encroachment of settlers.");

        alliesModule.AddOption("Which tribes have joined you?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTribesModule())); });

        return alliesModule;
    }

    private DialogueModule CreateTribesModule()
    {
        return new DialogueModule("The Shawnee, the Delaware, and even some from the Iroquois confederation have pledged their support. Each tribe brings its own strength and wisdom.");
    }

    private DialogueModule CreateSettlerGoalsModule()
    {
        return new DialogueModule("The settlers desire land, resources, and the expansion of their territories. Their greed knows no bounds, and they do not understand our connection to the land.");
    }

    private DialogueModule CreateVisionModule()
    {
        DialogueModule visionModule = new DialogueModule("I envision a world where our people can live in peace, where the balance of nature is respected and protected. A world where our children can thrive without fear of invasion.");

        visionModule.AddOption("What steps are you taking to achieve this vision?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateStepsModule())); });

        return visionModule;
    }

    private DialogueModule CreateStepsModule()
    {
        DialogueModule stepsModule = new DialogueModule("We educate our people about their rights, strengthen our alliances, and prepare our warriors. We aim to negotiate with those who are willing to listen, while remaining firm against those who would harm us.");

        stepsModule.AddOption("Have you had any success with negotiations?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateNegotiationSuccessModule())); });

        return stepsModule;
    }

    private DialogueModule CreateNegotiationSuccessModule()
    {
        return new DialogueModule("Some settlers show understanding, but many are driven by greed. Trust is hard to build, yet we must try, for peace is the ultimate goal.");
    }

    private DialogueModule CreateHelpModule()
    {
        DialogueModule helpModule = new DialogueModule("Your willingness to assist is noble. There are many ways you could help, whether by gathering information, providing supplies, or simply spreading awareness of our plight.");

        helpModule.AddOption("I can gather information.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGatherInfoModule())); });

        helpModule.AddOption("What supplies do you need?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSuppliesModule())); });

        return helpModule;
    }

    private DialogueModule CreateGatherInfoModule()
    {
        return new DialogueModule("Speak to the people in the settlements, learn their plans, and return with knowledge that could aid our cause.");
    }

    private DialogueModule CreateSuppliesModule()
    {
        DialogueModule suppliesModule = new DialogueModule("We need food, arrows, and medicine for our wounded. Every small contribution helps strengthen our resistance.");

        suppliesModule.AddOption("I will gather supplies for you.",
            player => true,
            player => { player.SendMessage("You have chosen to help Tecumseh."); });

        return suppliesModule;
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
