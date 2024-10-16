using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Laura Secord")]
public class LauraSecord : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LauraSecord() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Laura Secord";
        Body = 0x191; // Human female body

        // Stats
        SetStr(70);
        SetDex(100);
        SetInt(85);
        SetHits(65);

        // Appearance
        AddItem(new PlainDress() { Hue = 1125 });
        AddItem(new Boots() { Hue = 1103 });
        AddItem(new Bonnet() { Hue = 1132 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public LauraSecord(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Laura Secord, hailing from the beautiful lands of France! How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("I am in good health, merci! My heart is light and my spirit high."))); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("Do you sell pastries?",
            player => true,
            player =>
            {
                DialogueModule pastriesModule = new DialogueModule("My pastries are a blend of traditional French techniques and local flavors. Would you like to know more?");
                pastriesModule.AddOption("What types of pastries do you make?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreatePastryTypesModule())); });
                player.SendGump(new DialogueGump(player, pastriesModule));
            });

        greeting.AddOption("What can you tell me about France?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFranceModule())); });

        greeting.AddOption("Do you have a reward for me?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                lastRewardTime = DateTime.UtcNow;
                player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                player.SendGump(new DialogueGump(player, new DialogueModule("Your efforts won't go unnoticed. Here is a sample of my finest pastries!")));
            });

        return greeting;
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("I find solace in the art of baking delicious pastries! Each creation tells a story.");
        jobModule.AddOption("What inspired you to become a baker?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("My grandmother taught me the secrets of baking in our quaint kitchen. The scent of fresh bread was a childhood delight."))); });
        jobModule.AddOption("Do you have any special recipes?",
            player => true,
            player =>
            {
                DialogueModule recipesModule = new DialogueModule("Oh, I have many! Each recipe is a cherished memory. Would you like to hear about a specific one?");
                recipesModule.AddOption("Tell me about macarons.",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateMacaronModule())); });
                recipesModule.AddOption("What about tarts?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateTartModule())); });
                player.SendGump(new DialogueGump(player, recipesModule));
            });

        return jobModule;
    }

    private DialogueModule CreatePastryTypesModule()
    {
        DialogueModule pastryTypesModule = new DialogueModule("I specialize in a variety of pastries, including:");
        pastryTypesModule.AddOption("Macarons",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMacaronModule())); });
        pastryTypesModule.AddOption("Tarts",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTartModule())); });
        pastryTypesModule.AddOption("Eclairs",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateEclairModule())); });
        pastryTypesModule.AddOption("Croissants",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateCroissantModule())); });

        return pastryTypesModule;
    }

    private DialogueModule CreateFranceModule()
    {
        DialogueModule franceModule = new DialogueModule("France is a land of romance, art, and scrumptious cuisine! What would you like to know?");
        franceModule.AddOption("What do you miss the most?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("I miss the lavender fields and the gentle hum of the Seine river."))); });
        franceModule.AddOption("Tell me about French culture.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("French culture is rich and diverse, from art and music to food and philosophy. Each region has its unique flavors."))); });

        return franceModule;
    }

    private DialogueModule CreateMacaronModule()
    {
        DialogueModule macaronModule = new DialogueModule("Ah, the macaron! A delightful treat that is both crispy and creamy. Making them is an art. Would you like to try making them with me?");
        macaronModule.AddOption("Yes, I'd love to!",
            player => true,
            player =>
            {
                DialogueModule makingModule = new DialogueModule("Great! For a perfect batch, I’ll need almond flour, egg whites, and some unique fruit extracts. Can you gather these ingredients for me?");
                makingModule.AddOption("What ingredients do you need?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("I'll need almond flour, egg whites, and unique fruit extracts. Bring them to me for a reward!"))); });
                player.SendGump(new DialogueGump(player, makingModule));
            });
        macaronModule.AddOption("No, maybe another time.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return macaronModule;
    }

    private DialogueModule CreateTartModule()
    {
        DialogueModule tartModule = new DialogueModule("Tarts are such a delightful treat! They can be sweet or savory. Would you like to know more about making them?");
        tartModule.AddOption("Yes, please!",
            player => true,
            player =>
            {
                tartModule.AddOption("What types of tarts can you make?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("I can make fruit tarts, chocolate tarts, and even quiches! Each is a treat for the senses."))); });
                player.SendGump(new DialogueGump(player, tartModule));
            });
        tartModule.AddOption("No, thank you.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return tartModule;
    }

    private DialogueModule CreateEclairModule()
    {
        DialogueModule eclairModule = new DialogueModule("Eclairs are a classic French pastry, filled with cream and topped with chocolate. Would you like to try one?");
        eclairModule.AddOption("Yes, I'd love one!",
            player => true,
            player =>
            {
                eclairModule.AddOption("What do you need to make them?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("For eclairs, I need choux pastry dough, pastry cream, and chocolate icing."))); });
                player.SendGump(new DialogueGump(player, eclairModule));
            });
        eclairModule.AddOption("No, thank you.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return eclairModule;
    }

    private DialogueModule CreateCroissantModule()
    {
        DialogueModule croissantModule = new DialogueModule("Croissants are flaky, buttery, and utterly delicious! Have you ever tried making them?");
        croissantModule.AddOption("No, how do you make them?",
            player => true,
            player =>
            {
                croissantModule.AddOption("Tell me the steps!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, new DialogueModule("Making croissants requires patience and the right technique. It involves folding dough with butter multiple times to create layers."))); });
                player.SendGump(new DialogueGump(player, croissantModule));
            });
        croissantModule.AddOption("Yes, I have!",
            player => true,
            player => { player.SendGump(new DialogueGump(player, new DialogueModule("Ah, a fellow baker! Perhaps we can exchange tips!"))); });

        return croissantModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
