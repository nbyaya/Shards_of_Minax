using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class TatteredTina : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TatteredTina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tattered Tina";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SetStr(30);
        SetDex(31);
        SetInt(22);
        SetHits(38);

        AddItem(new Skirt() { Hue = 1150 });
        AddItem(new BodySash() { Hue = 48 });
        AddItem(new Boots() { Hue = 67 });

        SpeechHue = 0; // Default speech hue
        lastRewardTime = DateTime.MinValue;
    }

    public TatteredTina(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Tattered Tina, a wretch of this forsaken place. Life has not been kind. What would you like to know about my existence?");

        greeting.AddOption("Tell me about living on the streets.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateStreetLifeModule())); });

        greeting.AddOption("What do you eat?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFoodModule())); });

        greeting.AddOption("What do you do all day?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDailyLifeModule())); });

        greeting.AddOption("Do you have any friends?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFriendsModule())); });

        return greeting;
    }

    private DialogueModule CreateStreetLifeModule()
    {
        DialogueModule streetLifeModule = new DialogueModule("Living on the streets is a struggle. The cold bites at my skin, and the rain drenches my meager belongings. Sometimes, I find shelter in doorways, but it’s never safe. I've seen others like me, lost and broken, seeking warmth in the shadows.");

        streetLifeModule.AddOption("What do you fear the most?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFearModule())); });

        streetLifeModule.AddOption("How do you survive?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSurvivalModule())); });

        return streetLifeModule;
    }

    private DialogueModule CreateFoodModule()
    {
        DialogueModule foodModule = new DialogueModule("Ah, food... I often sift through old garbage. It’s amazing what people discard. I find stale bread, rotten fruits, and sometimes a half-eaten meal. The taste is often... questionable, but it keeps me alive.");

        foodModule.AddOption("What’s the worst thing you’ve eaten?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateWorstFoodModule())); });

        foodModule.AddOption("Do you ever get sick from it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateSicknessModule())); });

        return foodModule;
    }

    private DialogueModule CreateDailyLifeModule()
    {
        DialogueModule dailyLifeModule = new DialogueModule("My days are spent scavenging. I wake at dawn, searching through the alleys for anything edible. I watch the bustling city life around me, filled with laughter and joy that feels worlds away. When the sun sets, I huddle in a corner, hoping to stay warm.");

        dailyLifeModule.AddOption("Do you ever get lonely?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateLonelinessModule())); });

        dailyLifeModule.AddOption("What do you think about the wealthy?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateWealthyModule())); });

        return dailyLifeModule;
    }

    private DialogueModule CreateFriendsModule()
    {
        DialogueModule friendsModule = new DialogueModule("Friends? I have acquaintances on the street, but true friends are hard to come by. We look out for one another, sharing scraps of food or a warm spot to sleep, but trust is fragile among us.");

        friendsModule.AddOption("What happens when someone betrays that trust?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateBetrayalModule())); });

        return friendsModule;
    }

    private DialogueModule CreateFearModule()
    {
        DialogueModule fearModule = new DialogueModule("I fear the night. That's when the worst of humanity comes out. I've seen violence erupt over petty arguments, and the shadows hold dangers I cannot see.");

        fearModule.AddOption("Have you ever been attacked?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAttackModule())); });

        return fearModule;
    }

    private DialogueModule CreateSurvivalModule()
    {
        DialogueModule survivalModule = new DialogueModule("To survive, I’ve learned to be resourceful. I find warmth in discarded blankets, and I’ve become skilled at locating food others overlook. It’s a tough life, but it’s mine.");

        survivalModule.AddOption("What do you dream of?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateDreamModule())); });

        return survivalModule;
    }

    private DialogueModule CreateWorstFoodModule()
    {
        DialogueModule worstFoodModule = new DialogueModule("The worst? Once, I found a container of old fish. It had seen better days—smelled worse. I tried to eat it, but... well, let's just say it didn’t end well.");

        worstFoodModule.AddOption("What did you do after that?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return worstFoodModule;
    }

    private DialogueModule CreateSicknessModule()
    {
        DialogueModule sicknessModule = new DialogueModule("I’ve had my share of sickness. Eating spoiled food is a gamble. Sometimes I’m fine; other times, I’m bedridden for days. There’s no healer on the streets to help me.");

        sicknessModule.AddOption("How do you cope with that?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateCopingModule())); });

        return sicknessModule;
    }

    private DialogueModule CreateLonelinessModule()
    {
        DialogueModule lonelinessModule = new DialogueModule("Loneliness is my only companion. I watch families pass by, sharing laughter, while I hide in the shadows. I’ve learned to smile at my own misery; it’s a strange sort of comfort.");

        lonelinessModule.AddOption("Have you ever thought of leaving the streets?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateLeavingModule())); });

        return lonelinessModule;
    }

    private DialogueModule CreateWealthyModule()
    {
        DialogueModule wealthyModule = new DialogueModule("The wealthy move through life oblivious to our struggles. They toss their scraps without a thought, yet they never see the hunger in our eyes. I sometimes wonder if they even know we exist.");

        wealthyModule.AddOption("Do you think they could help?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHelpModule())); });

        return wealthyModule;
    }

    private DialogueModule CreateBetrayalModule()
    {
        DialogueModule betrayalModule = new DialogueModule("Betrayal can be deadly. I once shared food with someone who later stole my blanket while I slept. I woke up shivering, and it taught me to trust no one completely.");

        betrayalModule.AddOption("That's a harsh lesson.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return betrayalModule;
    }

    private DialogueModule CreateAttackModule()
    {
        DialogueModule attackModule = new DialogueModule("I’ve been attacked before, but I’ve learned to avoid confrontation. It’s better to walk away than to fight; I can’t afford injuries in this life.");

        attackModule.AddOption("I admire your strength.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return attackModule;
    }

    private DialogueModule CreateDreamModule()
    {
        DialogueModule dreamModule = new DialogueModule("I dream of a warm bed and a full meal. Of a life where I don’t have to scour through trash. But dreams are for the hopeful, and hope is a luxury I can barely afford.");

        dreamModule.AddOption("Perhaps one day that will come true.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return dreamModule;
    }

    private DialogueModule CreateCopingModule()
    {
        DialogueModule copingModule = new DialogueModule("I cope by focusing on small victories, like finding an extra piece of bread or a warm corner. Sometimes, I talk to myself—it helps keep the loneliness at bay.");

        copingModule.AddOption("That sounds tough.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return copingModule;
    }

    private DialogueModule CreateLeavingModule()
    {
        DialogueModule leavingModule = new DialogueModule("I’ve thought about it, but where would I go? The streets are familiar. They’re my home, albeit a sad one. Leaving means stepping into the unknown, and that frightens me.");

        leavingModule.AddOption("I understand your fear.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return leavingModule;
    }

    private DialogueModule CreateHelpModule()
    {
        DialogueModule helpModule = new DialogueModule("Help? I’d like to think so. A little kindness can go a long way. But most pass by without a glance, lost in their own world.");

        helpModule.AddOption("Maybe I can help you.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return helpModule;
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
