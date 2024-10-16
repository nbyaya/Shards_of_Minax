using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class SageThandor : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SageThandor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sage Thandor";
        Body = 0x190; // Human male body

        // Stats
        SetStr(150);
        SetDex(75);
        SetInt(100);
        SetHits(100);

        // Appearance
        AddItem(new Robe() { Hue = 2118 });
        AddItem(new Sandals() { Hue = 1105 });
        AddItem(new WideBrimHat() { Hue = 2118 });
        AddItem(new LeatherGloves() { Hue = 2118 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public SageThandor(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Sage Thandor, a collector of exotic measurement systems. Have you heard of them?");

        greeting.AddOption("What are exotic measurement systems?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateExoticMeasurementModule())); });

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("What can you tell me about the virtues?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateVirtueModule())); });

        greeting.AddOption("I've heard about a prophecy.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateProphecyModule())); });

        greeting.AddOption("Do you have any rewards for honesty?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHonestyRewardModule(player))); });

        return greeting;
    }

    private DialogueModule CreateExoticMeasurementModule()
    {
        DialogueModule exoticMeasurementModule = new DialogueModule("Exotic measurement systems are fascinating! They vary widely across dimensions, and I've collected many from travelers. Would you like to hear about my favorite?");

        exoticMeasurementModule.AddOption("Yes, tell me about your favorite.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFavoriteMeasurementModule())); });

        exoticMeasurementModule.AddOption("What other systems do you have?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateOtherSystemsModule())); });

        exoticMeasurementModule.AddOption("Why do you collect them?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateReasonForCollectingModule())); });

        return exoticMeasurementModule;
    }

    private DialogueModule CreateFavoriteMeasurementModule()
    {
        DialogueModule favoriteModule = new DialogueModule("My favorite is the 'Waverun', a unit used by the dimensional travelers from the realm of Seraphis. It's based on the frequency of ethereal waves and allows for incredibly precise calculations!");

        favoriteModule.AddOption("How does it work?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateWaverunDetailsModule())); });

        favoriteModule.AddOption("Can I see your collection?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGreetingModule())); });

        return favoriteModule;
    }

    private DialogueModule CreateWaverunDetailsModule()
    {
        return new DialogueModule("The Waverun measures distances based on the vibrations of ethereal currents. One Waverun equals the distance a sound wave travels in one nanosecond. It's truly a wonder of dimensional physics!");
    }

    private DialogueModule CreateOtherSystemsModule()
    {
        DialogueModule otherSystemsModule = new DialogueModule("I've gathered a range of systems, such as the 'Glimmer' from the Luminaris dimension, which measures light in terms of its luminal speed, and the 'Chronal Tick' from the time-weavers of Aeon.");

        otherSystemsModule.AddOption("Tell me more about the Glimmer.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateGlimmerDetailsModule())); });

        otherSystemsModule.AddOption("What about the Chronal Tick?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateChronalTickDetailsModule())); });

        return otherSystemsModule;
    }

    private DialogueModule CreateGlimmerDetailsModule()
    {
        return new DialogueModule("The Glimmer measures the intensity of light in 'Lumin', a unit derived from the energy output of a star. It's beautifully complex, incorporating both wavelength and amplitude into its calculations.");
    }

    private DialogueModule CreateChronalTickDetailsModule()
    {
        return new DialogueModule("The Chronal Tick is unique! It defines intervals of time based on the decay rate of specific particles. One Tick corresponds to the time it takes for a thousand particles to decay in unison. Time has never felt so tangible!");
    }

    private DialogueModule CreateReasonForCollectingModule()
    {
        DialogueModule reasonModule = new DialogueModule("I believe that measurement is the key to understanding our universe. Each system reflects the culture and intellect of its creators, offering insight into their worldview. It's a passion that transcends dimensions!");

        reasonModule.AddOption("How do you acquire these measurements?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAcquisitionModule())); });

        return reasonModule;
    }

    private DialogueModule CreateAcquisitionModule()
    {
        return new DialogueModule("I often trade with dimensional travelers who visit our realm. In exchange for knowledge of our world, I request samples of their unique systems. Sometimes, I even journey through portals to collect them myself!");
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("I am in good health, thank you.");
    }

    private DialogueModule CreateJobModule()
    {
        return new DialogueModule("I am a sage, dedicated to the pursuit of knowledge.");
    }

    private DialogueModule CreateVirtueModule()
    {
        DialogueModule virtueModule = new DialogueModule("True wisdom lies in understanding the virtues that guide our lives. Do you seek knowledge of these virtues?");
        virtueModule.AddOption("Tell me more about the virtues.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateVirtueDetailsModule())); });
        return virtueModule;
    }

    private DialogueModule CreateVirtueDetailsModule()
    {
        DialogueModule detailsModule = new DialogueModule("The virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Which virtue intrigues you the most?");
        // Additional details or options can be added here
        return detailsModule;
    }

    private DialogueModule CreateProphecyModule()
    {
        return new DialogueModule("The prophecy speaks of a time when the land will be consumed by darkness, and only a hero pure of heart can dispel it. Do you believe you are that hero?");
    }

    private DialogueModule CreateHonestyRewardModule(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            return new DialogueModule("I have no reward right now. Please return later.");
        }
        else
        {
            return new DialogueModule("I have no reward right now. Please return later.");
        }
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
