using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Zara the Flame Whisperer")]
public class ZaraTheFlameWhisperer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ZaraTheFlameWhisperer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zara the Flame Whisperer";
        Body = 0x190; // Human female body

        // Stats
        SetStr(80);
        SetDex(60);
        SetInt(90);
        SetHits(85);

        // Appearance
        AddItem(new Robe() { Hue = 1172 });
        AddItem(new Sandals() { Hue = 1171 });
        AddItem(new FireballWand() { Name = "Zara's Ember" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Zara the Flame Whisperer. What brings you to my hearth?");

        greeting.AddOption("What is your health like?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in tune with the flames, my health is always vibrant. What do you know of the healing properties of fire?");
                healthModule.AddOption("I've heard fire can heal.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule healModule = new DialogueModule("Indeed! Fire can mend wounds and renew spirits, but it must be used wisely. Have you ever felt its rejuvenating touch?");
                        healModule.AddOption("No, but I wish to learn more.",
                            p => true,
                            p =>
                            {
                                DialogueModule moreHealModule = new DialogueModule("To harness fire's healing, one must first understand its nature. Fire is both friend and foe. Are you ready to explore its depths?");
                                moreHealModule.AddOption("Yes, I'm ready.",
                                    pp => true,
                                    pp =>
                                    {
                                        pp.SendMessage("Zara nods approvingly, eager to share her knowledge.");
                                        pp.SendGump(new DialogueGump(pp, CreateHealingKnowledgeModule()));
                                    });
                                moreHealModule.AddOption("Not right now.",
                                    pp => true,
                                    pp =>
                                    {
                                        pp.SendMessage("Very well, knowledge waits for those who seek it.");
                                    });
                                p.SendGump(new DialogueGump(p, moreHealModule));
                            });
                        pl.SendGump(new DialogueGump(pl, healModule));
                    });
                healthModule.AddOption("I've never thought about it that way.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My calling is to harness the power of flames and guide others in their path. Flames have a language of their own. Do you wish to learn this language?");
                jobModule.AddOption("Yes, teach me the language of flames.",
                    p => true,
                    p =>
                    {
                        DialogueModule languageModule = new DialogueModule("Very well! The flames speak of passion, destruction, and rebirth. Each flicker tells a story. What story do you wish to uncover?");
                        languageModule.AddOption("Tell me about passion.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Zara's eyes glow with enthusiasm as she explains the fire of passion, igniting your curiosity.");
                                pl.SendGump(new DialogueGump(pl, CreatePassionModule()));
                            });
                        languageModule.AddOption("What about destruction?",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Destruction can be a powerful force, cleansing the old to make way for the new. It is often misunderstood.");
                                pl.SendGump(new DialogueGump(pl, CreateDestructionModule()));
                            });
                        languageModule.AddOption("I'm not ready for this.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("The flames will always await your return.");
                            });
                        p.SendGump(new DialogueGump(p, languageModule));
                    });
                jobModule.AddOption("Tell me more about guiding others.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule guideModule = new DialogueModule("Many seek guidance, and I help them discover the power of the flame within themselves. Do you wish to be one of them?");
                        guideModule.AddOption("Yes, I want to discover my inner flame.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("Zara smiles warmly, sensing your determination.");
                                p.SendGump(new DialogueGump(p, CreateGuidanceModule()));
                            });
                        guideModule.AddOption("Not at the moment.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("The journey is yours to choose, traveler.");
                            });
                        player.SendGump(new DialogueGump(player, guideModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What can you tell me about flames?",
            player => true,
            player =>
            {
                DialogueModule flamesModule = new DialogueModule("Fire is both a destructive and transformative force. Have you ever felt its warmth?");
                flamesModule.AddOption("Yes, it's powerful.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule powerfulModule = new DialogueModule("Indeed! It can destroy, but also renew. The cycle of fire is essential. What would you like to know about its secrets?");
                        powerfulModule.AddOption("Tell me about fire's secrets.",
                            p => true,
                            p =>
                            {
                                DialogueModule secretsModule = new DialogueModule("Fire can both cleanse and consume. Its secrets are for those who dare to understand. Are you daring enough to seek them?");
                                secretsModule.AddOption("I am daring; share them with me.",
                                    pp => true,
                                    pp =>
                                    {
                                        pp.SendMessage("Zara's eyes spark with excitement as she prepares to share ancient knowledge.");
                                        pp.SendGump(new DialogueGump(pp, CreateSecretsModule()));
                                    });
                                secretsModule.AddOption("I’m not sure about that.",
                                    pp => true,
                                    pp =>
                                    {
                                        pp.SendMessage("Very well, knowledge awaits your readiness.");
                                    });
                                p.SendGump(new DialogueGump(p, secretsModule));
                            });
                        flamesModule.AddOption("What about warmth?",
                            plz => true,
                            plz =>
                            {
                                DialogueModule warmthModule = new DialogueModule("Warmth is the embrace of the flame. It reminds us that even in the darkest times, there is a spark of hope.");
                                warmthModule.AddOption("Can I receive a gift of warmth?",
                                    pla => CanReward(pla),
                                    pla =>
                                    {
                                        GiveReward(pla);
                                    });
                                warmthModule.AddOption("I’m content with just knowledge.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Knowledge is indeed a gift of its own.");
                                    });
                                pl.SendGump(new DialogueGump(pl, warmthModule));
                            });
                        pl.SendGump(new DialogueGump(pl, powerfulModule));
                    });
                flamesModule.AddOption("I've never thought of fire that way.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, flamesModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => CanReward(player),
            player =>
            {
                GiveReward(player);
            });

        return greeting;
    }

    private bool CanReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveReward(PlayerMobile player)
    {
        Say("Warmth is the embrace of the flame. It reminds us that even in the darkest times, there is a spark of hope. For those who seek, I can bestow a gift of this warmth upon you.");
        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
    }

    private DialogueModule CreateHealingKnowledgeModule()
    {
        DialogueModule healingKnowledge = new DialogueModule("To harness fire's healing, one must first understand its nature. What aspect of fire do you wish to explore?");
        // Add options related to healing knowledge
        healingKnowledge.AddOption("How can I learn to control fire?",
            player => true,
            player => { /* Implement further dialogue here */ });

        return healingKnowledge;
    }

    private DialogueModule CreatePassionModule()
    {
        DialogueModule passionModule = new DialogueModule("Passion fuels our desires and ambitions. It's a flame that can either illuminate or consume us. How do you find balance in your own life?");
        passionModule.AddOption("I struggle with balance.",
            player => true,
            player => { /* Further dialogue about balance */ });

        return passionModule;
    }

    private DialogueModule CreateDestructionModule()
    {
        DialogueModule destructionModule = new DialogueModule("Destruction is not the end, but a beginning. Like the phoenix, we can rise from the ashes. Do you embrace change?");
        destructionModule.AddOption("Yes, I welcome change.",
            player => true,
            player => { /* Further dialogue about change */ });

        return destructionModule;
    }

    private DialogueModule CreateGuidanceModule()
    {
        DialogueModule guidanceModule = new DialogueModule("Guidance comes from understanding oneself. What do you seek to discover about your inner flame?");
        // Implement nested options for guidance
        guidanceModule.AddOption("I wish to find my true purpose.",
            player => true,
            player => { /* Further dialogue about purpose */ });

        return guidanceModule;
    }

    private DialogueModule CreateSecretsModule()
    {
        DialogueModule secretsModule = new DialogueModule("The true secrets of fire are revealed only to those who seek them with an open heart. Are you ready to take the first step?");
        // Implement nested options for secrets
        secretsModule.AddOption("Yes, I am ready.",
            player => true,
            player => { /* Further dialogue about secrets */ });

        return secretsModule;
    }

    public ZaraTheFlameWhisperer(Serial serial) : base(serial) { }

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
