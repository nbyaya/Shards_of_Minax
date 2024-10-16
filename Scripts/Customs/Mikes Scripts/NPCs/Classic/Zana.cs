using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Zana")]
public class Zana : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Zana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zana";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(80);
        SetInt(100);
        SetHits(70);

        // Appearance
        AddItem(new Skirt() { Hue = 1440 });
        AddItem(new Boots() { Hue = 1440 });
        AddItem(new Cloak() { Hue = 1440 });
        AddItem(new MagicWand() { Name = "Zana's Wand" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
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
        DialogueModule greeting = new DialogueModule("I am Zana, the exiled cartographer. How can I assist you?");

        greeting.AddOption("What is your name?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateNameModule())); });

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

        greeting.AddOption("What do you do?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("What dangers exist?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDangersModule())); });

        greeting.AddOption("Can you tell me about maps?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMapsModule())); });

        greeting.AddOption("What task do you need help with?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTaskModule())); });

        greeting.AddOption("Goodbye.",
            player => true,
            player => { player.SendMessage("Zana nods goodbye."); });

        return greeting;
    }

    private DialogueModule CreateNameModule()
    {
        return new DialogueModule("I am Zana, the exiled cartographer. Once, my name was known in every town.");
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("My health is none of your concern, exile. It is my spirit that has withered.");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("I used to explore maps, but now I'm stuck in this wretched place.");

        jobModule.AddOption("What did you map?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMappedTerritoriesModule())); });

        jobModule.AddOption("Why are you here?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateReasonModule())); });

        return jobModule;
    }

    private DialogueModule CreateMappedTerritoriesModule()
    {
        DialogueModule territoriesModule = new DialogueModule("I mapped the Lost Caves of Elara, the Whispering Forest, and the Sunken City.");

        territoriesModule.AddOption("What makes those places special?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSpecialPlacesModule())); });

        territoriesModule.AddOption("Are there dangers in those places?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDangersInTerritoryModule())); });

        return territoriesModule;
    }

    private DialogueModule CreateSpecialPlacesModule()
    {
        return new DialogueModule("Each place holds secrets. The Whispering Forest is said to be enchanted, while the Sunken City is filled with treasures, yet haunted by spirits.");
    }

    private DialogueModule CreateDangersInTerritoryModule()
    {
        return new DialogueModule("Indeed, each place has its own perils. You must be cautious of the creatures that guard their secrets.");
    }

    private DialogueModule CreateReasonModule()
    {
        return new DialogueModule("I was cursed, left to wander these lands as a reminder of my failures. I long to explore once more.");
    }

    private DialogueModule CreateDangersModule()
    {
        DialogueModule dangersModule = new DialogueModule("Do you think you can handle the dangers of Wraeclast, exile? The creatures here are not forgiving.");

        dangersModule.AddOption("What kind of creatures?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateCreaturesModule())); });

        dangersModule.AddOption("How can I prepare?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreatePreparationModule())); });

        return dangersModule;
    }

    private DialogueModule CreateCreaturesModule()
    {
        return new DialogueModule("You may encounter the Wretched, vile beasts that thrive in darkness, or the Harbingers, creatures that prey on the lost.");
    }

    private DialogueModule CreatePreparationModule()
    {
        DialogueModule preparationModule = new DialogueModule("Prepare yourself with strong potions and allies by your side. Knowledge is your greatest weapon.");

        preparationModule.AddOption("Where can I find potions?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreatePotionLocationModule())); });

        preparationModule.AddOption("What allies do you recommend?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAlliesModule())); });

        return preparationModule;
    }

    private DialogueModule CreatePotionLocationModule()
    {
        return new DialogueModule("The village nearby has a wise herbalist. She can guide you in crafting potions.");
    }

    private DialogueModule CreateAlliesModule()
    {
        return new DialogueModule("Seek those who share your goals. Other exiles often band together to survive.");
    }

    private DialogueModule CreateMapsModule()
    {
        DialogueModule mapsModule = new DialogueModule("The vast lands I once roamed with excitement have turned into a prison for me. Yet, I still possess some maps of secret places. Help me with a task, and I might share one with you.");

        mapsModule.AddOption("What territories have you mapped?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        mapsModule.AddOption("Can you teach me about map-making?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMapMakingModule())); });

        return mapsModule;
    }

    private DialogueModule CreateMapMakingModule()
    {
        DialogueModule mapMakingModule = new DialogueModule("Map-making is an art. You must capture the essence of the land with your eyes and heart.");

        mapMakingModule.AddOption("What tools do I need?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateToolsModule())); });

        mapMakingModule.AddOption("Is there a technique to it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTechniqueModule())); });

        return mapMakingModule;
    }

    private DialogueModule CreateToolsModule()
    {
        return new DialogueModule("You will need parchment, ink, and a steady hand. The right tools can make a map come alive.");
    }

    private DialogueModule CreateTechniqueModule()
    {
        return new DialogueModule("Observe the terrain carefully. The key is in the details—the shapes of the mountains, the flow of rivers.");
    }

    private DialogueModule CreateTaskModule()
    {
        DialogueModule taskModule = new DialogueModule("There's a particular artifact I've been searching for, known as the Cartographer's Compass. Find it for me, and I'll reward you generously.");

        taskModule.AddOption("What is the Cartographer's Compass?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateCompassModule())); });

        taskModule.AddOption("Where can I find it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateLocationModule())); });

        return taskModule;
    }

    private DialogueModule CreateCompassModule()
    {
        DialogueModule compassModule = new DialogueModule("Ah, you've heard of it? It's said to reveal hidden paths on maps. Bring it to me, and you shall have your reward.");

        compassModule.AddOption("What reward will I get?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateRewardModule())); });

        return compassModule;
    }

    private DialogueModule CreateLocationModule()
    {
        return new DialogueModule("It is rumored to be hidden in the depths of the Whispering Forest, guarded by ancient spirits.");
    }

    private DialogueModule CreateRewardModule()
    {
        DialogueModule rewardModule = new DialogueModule("I have many treasures from my travels. Once you bring me the Compass, I promise to give you something of great value.");
        
        rewardModule.AddOption("I'll find the Cartographer's Compass!",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.AddToBackpack(new MaxxiaScroll());
                    lastRewardTime = DateTime.UtcNow;
                    player.SendMessage("You've received a reward!");
                }
            });

        return rewardModule;
    }

    public Zana(Serial serial) : base(serial) { }

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
