using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class MerlinP : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MerlinP() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "MerlinP";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(80);
        SetInt(80);
        SetHits(70);

        // Appearance
        AddItem(new Robe() { Hue = 1109 });
        AddItem(new Boots() { Hue = 1109 });
        AddItem(new Spellbook() { Name = "MerlinP's Spellbook" });
        AddItem(new GnarledStaff() { Name = "MerlinP's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am MerlinP the Wizard. How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateHealthModule())));
        
        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateJobModule())));
        
        greeting.AddOption("What can you tell me about magic?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateMagicModule())));
        
        greeting.AddOption("Do you have any magical artifacts?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateArtifactsModule())));
        
        greeting.AddOption("Goodbye.",
            player => true,
            player => player.SendMessage("Farewell, traveler! May your journey be filled with wonder."));

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        return new DialogueModule("My health is as robust as the currents of time. The wisdom I’ve gathered through the ages keeps me strong.");
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My role in this world is to unravel the mysteries of magic. Each spell I cast reveals another layer of the universe's secrets.");

        jobModule.AddOption("What spells do you know?",
            player => true,
            player =>
            {
                DialogueModule spellsModule = new DialogueModule("I have mastered many spells: Fireball, Frost Nova, and the elusive Time Stop. Each holds immense power.");
                spellsModule.AddOption("Can you teach me a spell?",
                    playerq => player.Skills.Magery.Base >= 50.0,
                    playerq =>
                    {
                        player.SendGump(new DialogueGump(player, CreateTeachSpellModule()));
                    });
                spellsModule.AddOption("Tell me about Time Stop.",
                    playerw => true,
                    playerw => player.SendGump(new DialogueGump(player, CreateTeachSpellModule())));
                player.SendGump(new DialogueGump(player, spellsModule));
            });

        return jobModule;
    }

    private DialogueModule CreateTeachSpellModule()
    {
        DialogueModule teachModule = new DialogueModule("I can teach you a spell, but it requires dedication and a good understanding of the arcane. Which spell would you like to learn?");
        teachModule.AddOption("Teach me Fireball.",
            player => true,
            player => { player.SendMessage("You have learned Fireball!"); });
        teachModule.AddOption("Teach me Frost Nova.",
            player => true,
            player => { player.SendMessage("You have learned Frost Nova!"); });
        teachModule.AddOption("Maybe another time.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
        return teachModule;
    }

    private DialogueModule CreateMagicModule()
    {
        DialogueModule magicModule = new DialogueModule("Powerful magic can change the course of destiny. Are you drawn to the arcane arts?");
        
        magicModule.AddOption("Yes, I seek knowledge.",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendGump(new DialogueGump(player, CreateNoRewardModule()));
                }
                else
                {
                    player.SendGump(new DialogueGump(player, CreateRewardModule()));
                }
            });
        
        magicModule.AddOption("No, not interested.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
        
        return magicModule;
    }

    private DialogueModule CreateArtifactsModule()
    {
        DialogueModule artifactsModule = new DialogueModule("I possess a few magical artifacts, each with its own unique history and powers. Would you like to learn about them?");
        
        artifactsModule.AddOption("Yes, tell me more.",
            player => true,
            player =>
            {
                DialogueModule artifactDetailsModule = new DialogueModule("One of my prized possessions is the Amulet of Serenity, said to calm the fiercest of storms. Another is the Ring of Clarity, which sharpens the mind.");
                artifactDetailsModule.AddOption("What does the Amulet do?",
                    playerr => true,
                    playerr => player.SendMessage("The Amulet of Serenity grants the wearer peace of mind and protection from chaos."));
                artifactDetailsModule.AddOption("What about the Ring?",
                    playert => true,
                    playert => player.SendMessage("The Ring of Clarity enhances mental focus, making it easier to cast spells without error."));
                artifactDetailsModule.AddOption("I'm not interested in artifacts.",
                    playery => true,
                    playery => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, artifactDetailsModule));
            });

        artifactsModule.AddOption("No, thank you.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
        
        return artifactsModule;
    }

    private DialogueModule CreateNoRewardModule()
    {
        return new DialogueModule("I have no reward right now. Please return later.");
    }

    private DialogueModule CreateRewardModule()
    {
        lastRewardTime = DateTime.UtcNow;
        // Give a reward

        return new DialogueModule("Then seek knowledge and wield it wisely, for magic is a double-edged sword.");
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

    public MerlinP(Serial serial) : base(serial) { }
}
