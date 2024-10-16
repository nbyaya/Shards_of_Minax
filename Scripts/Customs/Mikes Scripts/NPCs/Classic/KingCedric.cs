using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of King Cedric")]
public class KingCedric : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KingCedric() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "King Cedric";
        Body = 0x190; // Human male body

        // Stats
        SetStr(115);
        SetDex(90);
        SetInt(85);
        SetHits(85);

        // Appearance
        AddItem(new Robe() { Hue = 1302 });
        AddItem(new Boots() { Hue = 1302 });
        AddItem(new VikingSword() { Name = "King Cedric's Sword" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public KingCedric(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am King Cedric, ruler of this wretched kingdom!");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => HealthDialogue(player));

        greeting.AddOption("What is your job?",
            player => true,
            player => JobDialogue(player));

        greeting.AddOption("What can you tell me about battles?",
            player => true,
            player => BattlesDialogue(player));

        greeting.AddOption("Why do you say your kingdom is doomed?",
            player => true,
            player => DoomDialogue(player));

        greeting.AddOption("What about the darkness?",
            player => true,
            player => DarknessDialogue(player));

        greeting.AddOption("Tell me about the curse.",
            player => true,
            player => CurseDialogue(player));

        greeting.AddOption("Where can I find the former Court Mage?",
            player => true,
            player => MageDialogue(player));

        greeting.AddOption("What about the lost artifact?",
            player => true,
            player => ArtifactDialogue(player));

        greeting.AddOption("Can you reward me for helping?",
            player => true,
            player => RewardPlayer(player));

        return greeting;
    }

    private void HealthDialogue(PlayerMobile player)
    {
        DialogueModule healthModule = new DialogueModule("Health? Ha! What does it matter in this miserable realm?");
        healthModule.AddOption("What do you mean by that?",
            p => true,
            p => healthResponse("In a kingdom plagued by darkness, what is health but a fleeting comfort? Many suffer, and I am powerless to aid them.", player));
        healthModule.AddOption("Is there any hope for recovery?",
            p => true,
            p => healthResponse("Perhaps if we could find the Orb of Luminance, it may restore not just health but hope itself.", player));

        player.SendGump(new DialogueGump(player, healthModule));
    }

    private void JobDialogue(PlayerMobile player)
    {
        DialogueModule jobModule = new DialogueModule("My 'job' as king? To preside over this wretched, doomed land.");
        jobModule.AddOption("What do you find most difficult?",
            p => true,
            p => jobResponse("The burden of my people's despair weighs heavily on my heart. Each day, I witness their suffering.", player));
        jobModule.AddOption("What are your royal duties?",
            p => true,
            p => jobResponse("I must make decisions that affect lives, but every choice seems to lead to more misfortune.", player));

        player.SendGump(new DialogueGump(player, jobModule));
    }

    private void BattlesDialogue(PlayerMobile player)
    {
        DialogueModule battlesModule = new DialogueModule("Valiant? Ha! In this cursed realm, valor is but a fleeting shadow.");
        battlesModule.AddOption("What battles have you fought?",
            p => true,
            p => battlesResponse("I have led my soldiers against the dark forces, but each victory is tainted by loss.", player));
        battlesModule.AddOption("Is there a chance for redemption?",
            p => true,
            p => battlesResponse("Only if we can rally true heroes to our cause. The darkness will not be vanquished easily.", player));

        player.SendGump(new DialogueGump(player, battlesModule));
    }

    private void DoomDialogue(PlayerMobile player)
    {
        DialogueModule doomModule = new DialogueModule("We were betrayed by one of our own, the former Court Mage. His treachery brought this doom upon us.");
        doomModule.AddOption("What exactly did he do?",
            p => true,
            p => doomResponse("Elarion sought power above all else. His ambition unleashed forces he couldn't control.", player));
        doomModule.AddOption("Can the betrayal be undone?",
            p => true,
            p => doomResponse("Perhaps if we find the lost artifact, we might have a chance to lift this curse.", player));

        player.SendGump(new DialogueGump(player, doomModule));
    }

    private void DarknessDialogue(PlayerMobile player)
    {
        DialogueModule darknessModule = new DialogueModule("The darkness is not just a metaphor. It's a living entity, seeking to consume everything.");
        darknessModule.AddOption("What can you tell me about it?",
            p => true,
            p => darknessResponse("It corrupts the land, twisting creatures into abominations. Only light can repel it.", player));
        darknessModule.AddOption("How can we fight against it?",
            p => true,
            p => darknessResponse("We must gather allies and seek the Orb of Luminance. It's our only hope.", player));

        player.SendGump(new DialogueGump(player, darknessModule));
    }

    private void CurseDialogue(PlayerMobile player)
    {
        DialogueModule curseModule = new DialogueModule("The curse manifests as a never-ending night, and creatures of the abyss have risen.");
        curseModule.AddOption("What are these creatures?",
            p => true,
            p => curseResponse("Nightmarish beasts, born from the shadows. They roam the land, spreading terror.", player));
        curseModule.AddOption("Can we break the curse?",
            p => true,
            p => curseResponse("Only with the Orb of Luminance can we hope to restore balance and light.", player));

        player.SendGump(new DialogueGump(player, curseModule));
    }

    private void MageDialogue(PlayerMobile player)
    {
        DialogueModule mageModule = new DialogueModule("He resides in the Tower of Desolation, protected by his dark creations.");
        mageModule.AddOption("How do I reach the Tower?",
            p => true,
            p => mageResponse("The path is fraught with danger, filled with twisted creatures and traps. Prepare well.", player));
        mageModule.AddOption("Is there a way to confront him?",
            p => true,
            p => mageResponse("Only with a band of brave souls can one hope to confront Elarion and his dark magic.", player));

        player.SendGump(new DialogueGump(player, mageModule));
    }

    private void ArtifactDialogue(PlayerMobile player)
    {
        DialogueModule artifactModule = new DialogueModule("The Orb of Luminance was stolen years ago. Its light has the power to pierce the darkness.");
        artifactModule.AddOption("Where was it last seen?",
            p => true,
            p => artifactResponse("It was said to be hidden in the Caverns of Despair, guarded by a fearsome beast.", player));
        artifactModule.AddOption("Can you help me find it?",
            p => true,
            p => artifactResponse("I wish I could, but I am bound to this throne. Only you can brave the journey.", player));

        player.SendGump(new DialogueGump(player, artifactModule));
    }

    private void RewardPlayer(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            player.SendMessage("I have no reward right now. Please return later.");
        }
        else
        {
            player.SendMessage("Indeed, for those who aid this kingdom, I promise treasures beyond imagination. Here, take this.");
            player.AddToBackpack(new BagOfHealth()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }

        // Return to main options after response
        player.SendGump(new DialogueGump(player, CreateGreetingModule()));
    }

    private void healthResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void jobResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void battlesResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void doomResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void darknessResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void curseResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void mageResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
    }

    private void artifactResponse(string response, PlayerMobile player)
    {
        DialogueModule responseModule = new DialogueModule(response);
        responseModule.AddOption("Back to previous options.", 
            pl => true, 
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        player.SendGump(new DialogueGump(player, responseModule));
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
