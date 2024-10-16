using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of King Edmund")]
public class KingEdmund : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KingEdmund() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "King Edmund";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(100);
        SetInt(100);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1124 }); // Robe with hue 1124
        AddItem(new Sandals() { Hue = 1124 }); // Sandals with hue 1124
        AddItem(new QuarterStaff() { Name = "King Edmund's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public KingEdmund(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am King Edmund, the ruler of this wretched kingdom. How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("What about your kingdom?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateKingdomModule())); });

        greeting.AddOption("How can I help you?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHelpModule())); });

        greeting.AddOption("What can you tell me about the traitor?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTraitorModule())); });

        greeting.AddOption("What is the Oracle?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateOracleModule())); });

        greeting.AddOption("Ponder my choices.",
            player => true,
            player => { HandlePondering(player); });

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        DialogueModule healthModule = new DialogueModule("My health is as miserable as my reign. The weight of my crown bears heavily on my heart.");
        
        healthModule.AddOption("What troubles you?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthTroubleModule())); });

        healthModule.AddOption("Is there a remedy?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthRemedyModule())); });

        return healthModule;
    }

    private DialogueModule CreateHealthTroubleModule()
    {
        return new DialogueModule("The betrayals of those close to me have left me in despair. I feel their treachery like a dagger in my back.");
    }

    private DialogueModule CreateHealthRemedyModule()
    {
        return new DialogueModule("If only I could find the rare Elixir of Hope! It is said to cure any ailment. Alas, it is hidden deep within the cursed caverns.");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My 'job' is to sit on this accursed throne and watch my kingdom crumble. I must bear witness to my people's suffering.");

        jobModule.AddOption("What is your greatest burden?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobBurdenModule())); });

        jobModule.AddOption("Do you have advisors?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobAdvisorsModule())); });

        return jobModule;
    }

    private DialogueModule CreateJobBurdenModule()
    {
        return new DialogueModule("The greatest burden is the knowledge that my decisions affect countless lives. Each choice I make could lead to joy or ruin.");
    }

    private DialogueModule CreateJobAdvisorsModule()
    {
        DialogueModule advisorModule = new DialogueModule("I have advisors, but trust is a fragile thing. Many have their own agendas. It is hard to find a loyal counselor in these treacherous times.");
        
        advisorModule.AddOption("Who do you trust?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAdvisorsTrustModule())); });

        return advisorModule;
    }

    private DialogueModule CreateAdvisorsTrustModule()
    {
        return new DialogueModule("Only a few have proven their loyalty, but even they have shadows lurking in their pasts. Can you help me determine who is truly trustworthy?");
    }

    private DialogueModule CreateKingdomModule()
    {
        DialogueModule kingdomModule = new DialogueModule("This realm was once a land of prosperity and joy, but now shadows have fallen upon it. The people suffer.");

        kingdomModule.AddOption("What caused this downfall?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateKingdomDownfallModule())); });

        kingdomModule.AddOption("Is there hope for the future?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateKingdomHopeModule())); });

        return kingdomModule;
    }

    private DialogueModule CreateKingdomDownfallModule()
    {
        return new DialogueModule("The kingdom has been plagued by treachery and disloyalty. The once-great knights have turned against me.");
    }

    private DialogueModule CreateKingdomHopeModule()
    {
        return new DialogueModule("Hope lies in the hands of brave souls like you. If you can restore faith among my people, perhaps the light can return.");
    }

    private DialogueModule CreateHelpModule()
    {
        DialogueModule helpModule = new DialogueModule("If you truly wish to assist, seek out the Oracle in the Whispering Woods. She may have insights into the issues plaguing my realm.");

        helpModule.AddOption("What should I ask her?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHelpAskModule())); });

        return helpModule;
    }

    private DialogueModule CreateHelpAskModule()
    {
        return new DialogueModule("Ask her about the traitor in my court. She sees things beyond the mortal eye.");
    }

    private DialogueModule CreateTraitorModule()
    {
        DialogueModule traitorModule = new DialogueModule("The traitor is cunning, hiding in plain sight. Gather evidence and present it to me.");

        traitorModule.AddOption("How will I recognize the traitor?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTraitorRecognitionModule())); });

        traitorModule.AddOption("What is the reward for exposing them?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTraitorRewardModule())); });

        return traitorModule;
    }

    private DialogueModule CreateTraitorRecognitionModule()
    {
        return new DialogueModule("Watch for suspicious behavior, especially among those closest to me. A traitor often reveals themselves in times of stress.");
    }

    private DialogueModule CreateTraitorRewardModule()
    {
        return new DialogueModule("The reward shall be great—gold, titles, and perhaps even a place by my side in the council. But beware, false accusations can lead to dire consequences.");
    }

    private DialogueModule CreateOracleModule()
    {
        DialogueModule oracleModule = new DialogueModule("The Oracle is a mysterious being, residing deep within the Whispering Woods. Many seek her wisdom, but not all are deemed worthy.");

        oracleModule.AddOption("How can I prove myself worthy?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateOracleWorthinessModule())); });

        return oracleModule;
    }

    private DialogueModule CreateOracleWorthinessModule()
    {
        return new DialogueModule("To prove your worth, you must complete a trial of courage. Face the challenges of the woods and return with a token of your bravery.");
    }

    private void HandlePondering(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            player.SendMessage("I have no reward right now. Please return later.");
        }
        else
        {
            player.SendMessage("To ponder is to reflect deeply. As a token of appreciation, take this.");
            player.AddToBackpack(new MurderRemovalDeed()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }
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
