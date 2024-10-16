using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DeadlyDiana : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DeadlyDiana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Deadly Diana";
        Body = 0x191; // Human female body

        // Stats
        SetStr(150);
        SetDex(65);
        SetInt(25);
        SetHits(110);

        // Appearance
        AddItem(new LeatherShorts() { Hue = 1175 });
        AddItem(new Skirt() { Hue = 1109 });
        AddItem(new ThighBoots() { Hue = 1175 });
        AddItem(new LeatherGloves() { Name = "Diana's Killing Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DeadlyDiana(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Deadly Diana, a shadow in the night. What do you want, traveler?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Deadly Diana, a name whispered in fear. I am a bringer of shadows and death. What else do you wish to know?");
                identityModule.AddOption("Tell me about your job.",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My job is not for the faint-hearted. I bring death to those who deserve it. The night is my companion, and the shadows my allies.");
                        jobModule.AddOption("Why do you do this?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule reasonModule = new DialogueModule("Justice is a lie people tell themselves. In my world, justice is defined by the heart of the beholder, and my heart speaks of vengeance.");
                                reasonModule.AddOption("That sounds dangerous.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                reasonModule.AddOption("Do you have a favorite type of person you like to kill?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule favoriteKillModule = new DialogueModule("Ah, an interesting question indeed. There are those whose demise I savor more than others.");
                                        favoriteKillModule.AddOption("Who are they?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule typesModule = new DialogueModule("I find immense pleasure in ending those who wear false facades. Those who pretend to be noble yet harbor corruption beneath their gilded words. Hypocrites, liars, and deceivers—they are the ones who deserve my blade.");
                                                typesModule.AddOption("Why do you despise hypocrites so much?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule hypocriteModule = new DialogueModule("Hypocrites are the ones who hide behind virtue, using it as a shield while they carry out their vile deeds. They pretend to be righteous, yet they are the first to condemn others for the sins they themselves commit. I make it my mission to strip away their masks and show them the true consequences of their deceit.");
                                                        hypocriteModule.AddOption("It seems personal to you.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule personalModule = new DialogueModule("Indeed, it is personal. I was betrayed by one such individual—a man who promised me salvation, yet led me into a den of betrayal. His lies cost the lives of people I cared for. Since that day, I have vowed to rid the world of those who hide behind false virtues.");
                                                                personalModule.AddOption("I understand your pain.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                personalModule.AddOption("What happened to that man?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule revengeModule = new DialogueModule("I found him, years later, living a comfortable life with a new identity. He begged for mercy, but I showed him none. His end was swift, but I made sure he knew exactly why he was dying. The look of realization in his eyes was worth every moment of my search.");
                                                                        revengeModule.AddOption("Sounds like justice was served.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, revengeModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, personalModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, hypocriteModule));
                                                    });
                                                typesModule.AddOption("Who else do you target?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule otherTargetsModule = new DialogueModule("I also relish the downfall of those who exploit the weak. Merchants who bleed their customers dry, lords who overtax their subjects, and anyone who builds their fortune upon the suffering of others. They think themselves untouchable, but I show them how fragile their power truly is.");
                                                        otherTargetsModule.AddOption("How do you choose your targets?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule choosingTargetsModule = new DialogueModule("I watch, I listen, and I gather information. My targets reveal themselves through their arrogance and disregard for others. I strike when they least expect it, when they are at their most vulnerable. Their suffering is a reminder that no one is beyond reach.");
                                                                choosingTargetsModule.AddOption("It must be difficult to live this way.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule difficultLifeModule = new DialogueModule("It is not an easy life, but it is the one I have chosen. I have made peace with the darkness, and in doing so, I have found my purpose. The path of vengeance is a lonely one, but the justice I bring is worth the solitude.");
                                                                        difficultLifeModule.AddOption("I respect your conviction.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, difficultLifeModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, choosingTargetsModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, otherTargetsModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, typesModule));
                                            });
                                        favoriteKillModule.AddOption("I don't want to know more.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, favoriteKillModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, reasonModule));
                            });
                        jobModule.AddOption("I see. Goodbye.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Diana glares at you as you turn away.");
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                identityModule.AddOption("I have no more questions.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Diana silently nods, her eyes cold and calculating.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Do you believe in justice?",
            player => true,
            player =>
            {
                DialogueModule justiceModule = new DialogueModule("The virtue of justice is but a facade in my world. Do you dare to seek justice, traveler?");
                justiceModule.AddOption("Yes, I seek justice.",
                    p => true,
                    p =>
                    {
                        DialogueModule yesModule = new DialogueModule("Your quest for justice is futile. In my world, justice is defined by power, not ideals.");
                        yesModule.AddOption("I understand.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, yesModule));
                    });
                justiceModule.AddOption("No, I don't believe in it.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Diana nods approvingly, as if she expected this answer.");
                    });
                player.SendGump(new DialogueGump(player, justiceModule));
            });

        greeting.AddOption("Tell me about the night.",
            player => true,
            player =>
            {
                DialogueModule nightModule = new DialogueModule("The night is when I'm most alive. It's when secrets are whispered and shadows take form.");
                nightModule.AddOption("What secrets do you know?",
                    p => true,
                    p =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Secrets are the currency of my world. For the right price, I may share one with you.");
                        secretsModule.AddOption("What is the price?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule priceModule = new DialogueModule("Everything has a price. If you wish to know a secret, offer something of value.");
                                priceModule.AddOption("I will think about it.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, priceModule));
                            });
                        p.SendGump(new DialogueGump(p, secretsModule));
                    });
                nightModule.AddOption("I have heard enough.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, nightModule));
            });

        greeting.AddOption("Do you have anything to offer?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Once, I showed mercy to a beggar. In gratitude, he gave me a trinket. If you prove worthy, it could be yours.");
                    rewardModule.AddOption("How can I prove myself?",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new AnimalLoreAugmentCrystal()); // Give the reward
                            lastRewardTime = DateTime.UtcNow;
                            p.SendMessage("Diana hands you a mysterious crystal, her gaze never leaving yours.");
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Diana watches you leave, her expression unreadable.");
            });

        return greeting;
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