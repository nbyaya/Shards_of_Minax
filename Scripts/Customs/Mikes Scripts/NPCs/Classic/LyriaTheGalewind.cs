using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lyria the Galewind")]
public class LyriaTheGalewind : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LyriaTheGalewind() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lyria the Galewind";
        Body = 0x191; // Human female body

        // Stats
        SetStr(60);
        SetDex(140);
        SetInt(70);
        SetHits(85);

        // Appearance
        AddItem(new ElvenBoots() { Hue = 1182 });
        AddItem(new ElvenShirt() { Hue = 1181 });
        AddItem(new Bow() { Name = "Lyria's Windbow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Lyria the Galewind, born of the winds and skies. How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

        greeting.AddOption("What is your job?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

        greeting.AddOption("Tell me about valor.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateValorModule())); });

        greeting.AddOption("What do you know about winds?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateWindsModule())); });

        greeting.AddOption("Do you have any tales of your adventures?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTalesModule())); });

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("I am untouched by the ailments of mortals, a gift from the winds themselves.");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My duty is to soar the skies and keep watch over the realms. The winds guide my path.");

        jobModule.AddOption("What do you observe from the skies?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateObservationModule())); });

        jobModule.AddOption("Do you ever get tired of it?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTiredModule())); });

        return jobModule;
    }

    private DialogueModule CreateObservationModule()
    {
        return new DialogueModule("From above, I witness the rise and fall of kingdoms. Each realm holds its own mysteries, waiting to be unveiled.");
    }

    private DialogueModule CreateTiredModule()
    {
        return new DialogueModule("Tiredness is not a concern for me. The winds rejuvenate my spirit, and every journey brings new wonders.");
    }

    private DialogueModule CreateValorModule()
    {
        DialogueModule valorModule = new DialogueModule("Valor is like the northern star, guiding the lost and brave. In my flights, I've witnessed countless acts of valor.");

        valorModule.AddOption("Can you share an example of valor?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateValorExampleModule())); });

        valorModule.AddOption("How does one become valorous?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateValorousPathModule())); });

        return valorModule;
    }

    private DialogueModule CreateValorExampleModule()
    {
        return new DialogueModule("Once, I saw a lone warrior standing against an army, his bravery unmatched. He fought not for glory, but to protect his village. It inspired all who witnessed it.");
    }

    private DialogueModule CreateValorousPathModule()
    {
        DialogueModule pathModule = new DialogueModule("To become valorous, one must embrace courage in the face of fear. It's about selflessness and the willingness to stand up for others.");

        pathModule.AddOption("What if I fear failure?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateFearModule())); });

        return pathModule;
    }

    private DialogueModule CreateFearModule()
    {
        return new DialogueModule("Fear is natural. The key is to acknowledge it and act despite it. Each step taken is a step toward strength.");
    }

    private DialogueModule CreateWindsModule()
    {
        DialogueModule windsModule = new DialogueModule("The winds whisper secrets to those who listen. One of them spoke of a mantra where its second syllable is VEX.");

        windsModule.AddOption("What is VEX?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateVexModule())); });

        windsModule.AddOption("Do you often hear the winds?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateHearWindsModule())); });

        return windsModule;
    }

    private DialogueModule CreateVexModule()
    {
        DialogueModule vexModule = new DialogueModule("VEX is said to be the second syllable of the mantra of Justice. It's a word of power and importance.");

        vexModule.AddOption("What does it mean?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateMeaningModule())); });

        return vexModule;
    }

    private DialogueModule CreateMeaningModule()
    {
        return new DialogueModule("It represents balance and fairness, a guiding principle for those who seek justice in their actions.");
    }

    private DialogueModule CreateHearWindsModule()
    {
        return new DialogueModule("Indeed, the winds speak to me. They carry tales from distant lands, stories of heroism, tragedy, and hope.");
    }

    private DialogueModule CreateTalesModule()
    {
        DialogueModule talesModule = new DialogueModule("Ah, tales! Let me share one with you. There was once a brave sorceress who defied a dark lord...");

        talesModule.AddOption("What happened next?",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateTaleContinuationModule())); });

        return talesModule;
    }

    private DialogueModule CreateTaleContinuationModule()
    {
        return new DialogueModule("She gathered allies from all corners of the realm, united by a common purpose. Together, they stormed the dark lord's fortress, their courage lighting the way.");
    }

    public LyriaTheGalewind(Serial serial) : base(serial) { }

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
