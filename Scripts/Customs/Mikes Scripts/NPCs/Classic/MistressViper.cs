using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Mistress Viper")]
public class MistressViper : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MistressViper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mistress Viper";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(130);
        SetInt(60);
        SetHits(70);

        // Appearance
        AddItem(new StuddedLegs() { Hue = 1272 });
        AddItem(new StuddedChest() { Hue = 1272 });
        AddItem(new StuddedArms() { Hue = 1272 });
        AddItem(new StuddedGloves() { Hue = 1272 });
        AddItem(new Boots() { Hue = 1272 });
        AddItem(new Dagger() { Name = "Mistress Viper's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Mistress Viper, the shadow in the night. What do you wish to know?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateHealthModule())));

        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateJobModule())));

        greeting.AddOption("What can you tell me about battles?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateBattlesModule())));

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player => HandleReward(player));

        greeting.AddOption("What secrets do you hold?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateSecretsModule())));

        greeting.AddOption("Tell me about the shadows.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateShadowsModule())));

        greeting.AddOption("What do you think of the light?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateLightModule())));

        greeting.AddOption("Are you seeking allies?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateAlliesModule())));

        return greeting;
    }

    private DialogueModule CreateHealthModule()
    {
        DialogueModule healthModule = new DialogueModule("My wounds are concealed beneath the cloak of shadows. Though I bear scars of past battles, they are merely reminders of my resilience.");
        healthModule.AddOption("You seem strong despite your injuries.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
        return healthModule;
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My job is to eliminate those who dare oppose the darkness. Every life I take is a step toward balance in this chaotic world.");
        jobModule.AddOption("How do you choose your targets?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateTargetChoiceModule())));
        return jobModule;
    }

    private DialogueModule CreateTargetChoiceModule()
    {
        return new DialogueModule("I target those whose ambitions threaten the delicate equilibrium of power. In shadows, one must learn to discern friend from foe.");
    }

    private DialogueModule CreateBattlesModule()
    {
        DialogueModule battles = new DialogueModule("True valor is found in the shadows, where the heart's darkness is tested. Are you prepared for this path?");
        battles.AddOption("Yes, I am prepared.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateEmbraceShadowsModule())));

        battles.AddOption("No, I am not ready.",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

        return battles;
    }

    private DialogueModule CreateEmbraceShadowsModule()
    {
        return new DialogueModule("Then embrace the shadows, for they shall be your closest ally. They can reveal truths hidden in the dark corners of the world.");
    }

    private void HandleReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            player.SendMessage("I have no reward right now. Please return later.");
        }
        else
        {
            player.SendMessage("Since you seek knowledge and show courage, take this as a token of my appreciation.");
            player.AddToBackpack(new DiscordanceAugmentCrystal()); // Replace with actual reward item
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule()));
    }

    private DialogueModule CreateSecretsModule()
    {
        DialogueModule secretsModule = new DialogueModule("The shadows hold many secrets, but not all are meant to be revealed. Some knowledge can be a burden.");
        secretsModule.AddOption("What secret would you share?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateSpecificSecretModule())));
        return secretsModule;
    }

    private DialogueModule CreateSpecificSecretModule()
    {
        return new DialogueModule("One secret is that true power lies not in might, but in the ability to remain unseen and unheard until the moment strikes.");
    }

    private DialogueModule CreateShadowsModule()
    {
        DialogueModule shadowsModule = new DialogueModule("The shadows are alive with whispers and stories. They are both sanctuary and prison, where the unwary can easily lose themselves.");
        shadowsModule.AddOption("What dangers lurk in the shadows?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateShadowDangersModule())));
        return shadowsModule;
    }

    private DialogueModule CreateShadowDangersModule()
    {
        return new DialogueModule("Many dangers lurk in the shadows—betrayal, deceit, and those who prey on the vulnerable. But they also offer protection to those who know how to navigate them.");
    }

    private DialogueModule CreateLightModule()
    {
        DialogueModule lightModule = new DialogueModule("The light is a double-edged sword. It reveals, but it also exposes. In the light, shadows can be harsh and unforgiving.");
        lightModule.AddOption("Do you fear the light?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateLightFearModule())));
        return lightModule;
    }

    private DialogueModule CreateLightFearModule()
    {
        return new DialogueModule("Fear is not the right word. Respect is more appropriate. The light can be a guide, but it can also lead one into traps.");
    }

    private DialogueModule CreateAlliesModule()
    {
        DialogueModule alliesModule = new DialogueModule("Allies are essential in the world of shadows. Trust must be earned, for betrayal lurks in every corner.");
        alliesModule.AddOption("How do you choose your allies?",
            player => true,
            player => player.SendGump(new DialogueGump(player, CreateAllyChoiceModule())));
        return alliesModule;
    }

    private DialogueModule CreateAllyChoiceModule()
    {
        return new DialogueModule("I choose allies based on their resolve and ability to adapt to the unpredictable nature of our surroundings.");
    }

    public MistressViper(Serial serial) : base(serial) { }

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
