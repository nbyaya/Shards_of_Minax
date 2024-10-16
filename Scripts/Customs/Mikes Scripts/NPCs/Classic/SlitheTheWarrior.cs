using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Slithe the Warrior")]
public class SlitheTheWarrior : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SlitheTheWarrior() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Slithe the Warrior";
        Body = 0x190; // Human male body

        // Stats
        SetStr(150);
        SetDex(100);
        SetInt(75);
        SetHits(100);

        // Appearance
        AddItem(new PlateChest() { Hue = 1316 });
        AddItem(new PlateArms() { Hue = 1316 });
        AddItem(new Cloak() { Hue = 1316 });
        AddItem(new PlateLegs() { Hue = 1316 });
        AddItem(new Bascinet() { Hue = 1316 });
        AddItem(new PlateGorget() { Hue = 1316 });
        AddItem(new PlateGloves() { Hue = 1316 });
        AddItem(new Broadsword() { Name = "Slithe's Blade" });

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
        DialogueModule greeting = new DialogueModule("I am Slithe the Warrior! Want to hear about my greatest battle?");

        greeting.AddOption("Yes, tell me about it.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateBattleModule())); });

        greeting.AddOption("What else do you do?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return greeting;
    }

    private DialogueModule CreateBattleModule()
    {
        DialogueModule battleModule = new DialogueModule("My greatest battle was against the fearsome Mega Dragon. It was a day etched in my memory.");

        battleModule.AddOption("What happened during the battle?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateBattleDetailsModule())); });

        battleModule.AddOption("What was the Mega Dragon like?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return battleModule;
    }

    private DialogueModule CreateBattleDetailsModule()
    {
        DialogueModule detailsModule = new DialogueModule("The clash was epic! As I stood before the Mega Dragon, the ground trembled beneath me.");

        detailsModule.AddOption("How did you prepare for it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreatePreparationModule())); });

        detailsModule.AddOption("What were your allies doing?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAlliesModule())); });

        detailsModule.AddOption("What was the turning point?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTurningPointModule())); });

        return detailsModule;
    }

    private DialogueModule CreatePreparationModule()
    {
        DialogueModule prepModule = new DialogueModule("I trained for weeks, honing my skills. I crafted special potions and gathered enchanted gear.");

        prepModule.AddOption("What kind of potions?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreatePotionModule())); });

        prepModule.AddOption("What gear did you use?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGearModule())); });

        return prepModule;
    }

    private DialogueModule CreatePotionModule()
    {
        DialogueModule potionModule = new DialogueModule("I brewed potions of strength and agility, essential for surviving the dragon's ferocious attacks.");

        potionModule.AddOption("Did you use any special ingredients?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateIngredientModule())); });

        potionModule.AddOption("How did they help you?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return potionModule;
    }

    private DialogueModule CreateIngredientModule()
    {
        DialogueModule ingredientModule = new DialogueModule("Yes, I used Dragon’s Blood and Moonlit Petals. Both were crucial for potency.");

        ingredientModule.AddOption("How did you acquire them?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return ingredientModule;
    }

    private DialogueModule CreateGearModule()
    {
        DialogueModule gearModule = new DialogueModule("I wore my finest armor—crafted from the scales of a lesser dragon. It was both beautiful and protective.");

        gearModule.AddOption("Did it protect you well?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateProtectionModule())); });

        return gearModule;
    }

    private DialogueModule CreateProtectionModule()
    {
        DialogueModule protectionModule = new DialogueModule("Absolutely! The scales absorbed many blows, allowing me to press on when others fell.");

        protectionModule.AddOption("What happened to your allies?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAlliesModule())); });

        return protectionModule;
    }

    private DialogueModule CreateAlliesModule()
    {
        DialogueModule alliesModule = new DialogueModule("I fought alongside brave warriors, each prepared to lay down their lives for victory.");

        alliesModule.AddOption("Who were they?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAlliesDetailsModule())); });

        return alliesModule;
    }

    private DialogueModule CreateAlliesDetailsModule()
    {
        DialogueModule alliesDetailsModule = new DialogueModule("We had a mighty mage, a skilled archer, and a healer with us. Each played a vital role.");

        alliesDetailsModule.AddOption("What did the mage do?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMageModule())); });

        alliesDetailsModule.AddOption("And the healer?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return alliesDetailsModule;
    }

    private DialogueModule CreateMageModule()
    {
        DialogueModule mageModule = new DialogueModule("The mage cast powerful spells, conjuring fire and ice to weaken the dragon.");

        mageModule.AddOption("Did any spells backfire?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMageBackfireModule())); });

        return mageModule;
    }

    private DialogueModule CreateMageBackfireModule()
    {
        DialogueModule backfireModule = new DialogueModule("Yes, once he miscalculated and summoned a storm that nearly struck us down! But he regained control.");

        backfireModule.AddOption("Impressive! What about the archer?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateArcherModule())); });

        return backfireModule;
    }

    private DialogueModule CreateArcherModule()
    {
        DialogueModule archerModule = new DialogueModule("The archer provided cover, picking off lesser foes that surrounded us during the battle.");

        archerModule.AddOption("What foes did you face?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFoesModule())); });

        return archerModule;
    }

    private DialogueModule CreateFoesModule()
    {
        DialogueModule foesModule = new DialogueModule("We faced hordes of lesser drakes, summoned by the Mega Dragon. They were relentless!");

        foesModule.AddOption("How did you deal with them?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFoeStrategyModule())); });

        return foesModule;
    }

    private DialogueModule CreateFoeStrategyModule()
    {
        DialogueModule strategyModule = new DialogueModule("We formed a defensive line, using shields to hold them back while the mage and archer fought fiercely.");

        strategyModule.AddOption("What was the turning point of the battle?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTurningPointModule())); });

        return strategyModule;
    }

    private DialogueModule CreateTurningPointModule()
    {
        DialogueModule turningPointModule = new DialogueModule("The turning point came when I landed a strike on the Mega Dragon's underbelly, shocking it!");

        turningPointModule.AddOption("What happened next?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAftermathModule())); });

        return turningPointModule;
    }

    private DialogueModule CreateAftermathModule()
    {
        DialogueModule aftermathModule = new DialogueModule("The dragon roared in fury, and with a final thrust, we struck it down. The battlefield fell silent.");

        aftermathModule.AddOption("What did you learn from this battle?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateLessonModule())); });

        return aftermathModule;
    }

    private DialogueModule CreateLessonModule()
    {
        DialogueModule lessonModule = new DialogueModule("I learned that true strength lies not just in power, but in unity and strategy. Every voice matters in battle.");

        lessonModule.AddOption("Incredible! Do you have any other tales?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return lessonModule;
    }

    public SlitheTheWarrior(Serial serial) : base(serial) { }

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
