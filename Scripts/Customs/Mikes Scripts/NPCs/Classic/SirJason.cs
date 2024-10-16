using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Sir Jason")]
public class SirJason : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirJason() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Jason";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(100);
        SetInt(80);
        SetHits(90);

        // Appearance
        AddItem(new PlateLegs() { Hue = 37 });
        AddItem(new PlateChest() { Hue = 37 });
        AddItem(new PlateHelm() { Hue = 37 });
        AddItem(new PlateGloves() { Hue = 37 });
        AddItem(new Boots() { Hue = 37 });
        AddItem(new BattleAxe() { Name = "Sir Jason's Axe" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public SirJason(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Sir Jason, once the leader of the Power Rangers under Zordon. What tales of adventure do you seek?");

        greeting.AddOption("Tell me about Zordon.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateZordonModule())));
        
        greeting.AddOption("What was it like being a Power Ranger?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreatePowerRangerModule())));
        
        greeting.AddOption("Tell me about your battles.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateBattlesModule())));
        
        greeting.AddOption("What happened to the Power Rangers?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateRangersFateModule())));
        
        greeting.AddOption("Do you have any secrets from your adventures?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateSecretsModule())));

        return greeting;
    }

    private DialogueModule CreateZordonModule()
    {
        DialogueModule zordonModule = new DialogueModule("Zordon was our wise mentor, guiding us through battles and teaching us the ways of justice. His holographic form inspired us to be better. Do you wish to hear more about our training under him?");
        
        zordonModule.AddOption("Yes, tell me about your training.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateTrainingModule())));
        
        zordonModule.AddOption("What was his greatest lesson?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateLessonModule())));

        return zordonModule;
    }

    private DialogueModule CreateTrainingModule()
    {
        DialogueModule trainingModule = new DialogueModule("Our training was intense. We faced numerous challenges that tested our strength, courage, and teamwork. We learned how to morph, summon our Zords, and fight against evil.");
        trainingModule.AddOption("What was the most challenging part?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateChallengesModule())));
        
        return trainingModule;
    }

    private DialogueModule CreateChallengesModule()
    {
        return new DialogueModule("The most challenging part was mastering our powers while learning to work as a team. Every battle required us to trust each other completely, or we risked losing everything.");
    }

    private DialogueModule CreateLessonModule()
    {
        return new DialogueModule("Zordon taught us that true strength comes from within, and that we must always protect those who cannot protect themselves. His wisdom remains with me to this day.");
    }

    private DialogueModule CreatePowerRangerModule()
    {
        DialogueModule rangerModule = new DialogueModule("Being a Power Ranger was both an honor and a burden. We fought to protect our world from threats like Rita Repulsa and Lord Zedd. The thrill of battle was exhilarating. Would you like to hear about my first battle?");
        
        rangerModule.AddOption("Yes, tell me about your first battle.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateFirstBattleModule())));
        
        rangerModule.AddOption("What were your powers?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreatePowersModule())));
        
        return rangerModule;
    }

    private DialogueModule CreateFirstBattleModule()
    {
        return new DialogueModule("My first battle was against Rita's monstrous minions. I was terrified, but the moment I morphed into the Red Ranger, I felt a surge of confidence. We fought valiantly and ultimately prevailed, but it was just the beginning of our struggles.");
    }

    private DialogueModule CreatePowersModule()
    {
        DialogueModule powersModule = new DialogueModule("As the Red Ranger, I wielded the power of the Tyrannosaurus Rex. My abilities included enhanced strength, agility, and combat skills. Each Ranger had unique powers corresponding to their Zords.");
        
        powersModule.AddOption("What about the Zords?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateZordsModule())));
        
        return powersModule;
    }

    private DialogueModule CreateZordsModule()
    {
        return new DialogueModule("Our Zords were powerful vehicles that we could summon to combat larger threats. I commanded the Tyrannosaurus Zord, and together, we formed the mighty Megazord in battle.");
    }

    private DialogueModule CreateBattlesModule()
    {
        DialogueModule battlesModule = new DialogueModule("We faced many foes, including the fearsome Lord Zedd and the cunning Rita Repulsa. Each battle brought new challenges. Would you like to hear about a specific enemy?");
        
        battlesModule.AddOption("Yes, tell me about Lord Zedd.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateLordZeddModule())));
        
        battlesModule.AddOption("What about Rita Repulsa?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateRitaModule())));
        
        return battlesModule;
    }

    private DialogueModule CreateLordZeddModule()
    {
        return new DialogueModule("Lord Zedd was one of our greatest adversaries. He was powerful and relentless, always finding new ways to challenge us. His dark magic was formidable, and he sought to conquer our world.");
    }

    private DialogueModule CreateRitaModule()
    {
        return new DialogueModule("Rita Repulsa was our first enemy. She was crafty and manipulative, using her magic to create monstrous minions. Her schemes often tested our resolve and unity.");
    }

    private DialogueModule CreateRangersFateModule()
    {
        DialogueModule fateModule = new DialogueModule("After many battles, we faced a great crisis. The Rangers eventually disbanded, each going their separate ways. However, our bond remains strong, and the legacy of the Power Rangers continues to inspire others.");
        
        fateModule.AddOption("Do you miss being a Ranger?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateMissingRangerModule())));
        
        return fateModule;
    }

    private DialogueModule CreateMissingRangerModule()
    {
        return new DialogueModule("Every day. The thrill of adventure, the camaraderie—it was more than just a fight against evil. It was a family. But my purpose now lies in a different path.");
    }

    private DialogueModule CreateSecretsModule()
    {
        DialogueModule secretsModule = new DialogueModule("I hold many secrets from my adventures. Some are about our battles, and others about the lessons learned. Would you like to hear about a specific secret?");
        
        secretsModule.AddOption("Yes, tell me about a battle secret.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateBattleSecretModule())));
        
        secretsModule.AddOption("Tell me a lesson you learned.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateLessonSecretModule())));
        
        return secretsModule;
    }

    private DialogueModule CreateBattleSecretModule()
    {
        return new DialogueModule("One secret I learned is that true victory often comes from strategy rather than brute force. We had to outsmart our enemies, finding their weaknesses and exploiting them.");
    }

    private DialogueModule CreateLessonSecretModule()
    {
        return new DialogueModule("A vital lesson I learned is that fear can be a powerful motivator, but courage must always prevail. Facing one's fears is the first step toward true strength.");
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
