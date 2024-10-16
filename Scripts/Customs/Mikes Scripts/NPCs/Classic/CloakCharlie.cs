using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CloakCharlie : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CloakCharlie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cloak Charlie";
        Body = 0x190; // Human male body

        // Stats
        Str = 80;
        Dex = 70;
        Int = 85;
        Hits = 80;

        // Appearance
        AddItem(new BoneLegs() { Hue = 1285 });
        AddItem(new Kryss());
        AddItem(new Cloak() { Hue = 1285 });
        
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize reward timer
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
        DialogueModule greeting = new DialogueModule("I am Cloak Charlie, a rogue skilled in shadows and secrets. What do you seek, wanderer?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I dance in the moon's shadow and unravel the threads of fate. My life is one of secrets and mysteries. But of all things, I find the most fascination in observing people—especially those who tread close to the edge of darkness.");
                aboutModule.AddOption("Who do you enjoy observing the most?",
                    p => true,
                    p =>
                    {
                        DialogueModule stalkModule = new DialogueModule("Ah, I have my favorites. Some are powerful, others cunning, and yet others completely unaware of how interesting they truly are. Who would you like to hear about?");
                        stalkModule.AddOption("Tell me about the powerful one.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule powerfulModule = new DialogueModule("There is one—a lord in the heart of Britain. He thinks his castle walls keep him safe, but I see his weaknesses. Lord Haedrig, a man whose power is unmatched, yet he fears the darkness outside his door. I have seen him pacing at night, looking towards the shadows, as if he knows I am there. Perhaps he suspects the secrets I have heard whispered through his own halls.");
                                powerfulModule.AddOption("What kind of secrets?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule secretDetailsModule = new DialogueModule("Rumors of alliances, plots to overthrow rivals, and even whispers of forbidden alchemical practices. Lord Haedrig's life is a tangled web of politics and power—one wrong step, and the entire structure could crumble.");
                                        secretDetailsModule.AddOption("That sounds dangerous.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Cloak Charlie smirks, a glimmer of excitement in his eyes.");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        secretDetailsModule.AddOption("I want to know more.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule moreSecretsModule = new DialogueModule("If you truly wish to know more, perhaps you would be interested in seeing the evidence for yourself. I could show you—if you can handle the truth.");
                                                moreSecretsModule.AddOption("Show me the evidence.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Cloak Charlie grins and whispers, 'Meet me at the old stone bridge at midnight. Come alone.'");
                                                    });
                                                moreSecretsModule.AddOption("No, that sounds too risky.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, moreSecretsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, secretDetailsModule));
                                    });
                                powerfulModule.AddOption("I think I'll leave Lord Haedrig to his own devices.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, powerfulModule));
                            });
                        stalkModule.AddOption("Tell me about the cunning one.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule cunningModule = new DialogueModule("Ah, the cunning one. Her name is Eliza, a merchant with an eye for opportunities. She moves through the markets of Minoc, always three steps ahead of her competition. I've watched her charm rivals into submission and make deals that would leave lesser men destitute. She has secrets, too—secrets she guards fiercely.");
                                cunningModule.AddOption("What secrets does Eliza have?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule elizaSecretsModule = new DialogueModule("Eliza has connections to the underground. She deals in rare artifacts, some of which are believed to be stolen. Her charm and influence allow her to move goods under the noses of even the most diligent guards. I've seen her pass off a priceless relic as a trinket to avoid suspicion.");
                                        elizaSecretsModule.AddOption("She sounds dangerous.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Cloak Charlie nods knowingly. 'Dangerous, yes. But also fascinating.'");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        elizaSecretsModule.AddOption("Can she be trusted?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule trustModule = new DialogueModule("Trust? Ha! Trust is a luxury one cannot afford when dealing with people like Eliza. She will only help you if it benefits her, but cross her, and you'll find yourself in a very precarious position.");
                                                trustModule.AddOption("I will tread carefully.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                trustModule.AddOption("Perhaps I'll avoid her altogether.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, trustModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, elizaSecretsModule));
                                    });
                                cunningModule.AddOption("I think I'll leave Eliza to her affairs.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, cunningModule));
                            });
                        stalkModule.AddOption("Tell me about the unaware one.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule unawareModule = new DialogueModule("The unaware one is a simple farmer named Thomas. He lives on the outskirts of Yew, completely oblivious to the attention he garners. He possesses a rare amulet—a family heirloom passed down through generations, though he doesn't understand its significance. I have watched many try to swindle him out of it, but his honesty protects him.");
                                unawareModule.AddOption("What is special about the amulet?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule amuletModule = new DialogueModule("The amulet is said to be enchanted, though even Thomas does not know of its true power. It glows faintly under the moonlight, and I believe it holds the key to a long-forgotten secret—perhaps a hidden treasure or a powerful spell. Many covet it, but Thomas's naive nature keeps it safe.");
                                        amuletModule.AddOption("Can the amulet be taken?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule takeAmuletModule = new DialogueModule("Taking the amulet would be no simple task. Thomas trusts very few, and any attempt to steal it would draw the attention of the villagers. However, there might be a way to convince him to part with it willingly, if you can find the right leverage.");
                                                takeAmuletModule.AddOption("What kind of leverage?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Cloak Charlie leans closer, whispering, 'Perhaps a favor, or something he values even more than the amulet.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                takeAmuletModule.AddOption("It sounds too complicated.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, takeAmuletModule));
                                            });
                                        amuletModule.AddOption("I will leave Thomas in peace.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, amuletModule));
                                    });
                                unawareModule.AddOption("I think I'll let Thomas live his simple life.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, unawareModule));
                            });
                        stalkModule.AddOption("I think I'll leave your favorite people alone.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, stalkModule));
                    });
                aboutModule.AddOption("Perhaps another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What can you tell me about shadows?",
            player => true,
            player =>
            {
                DialogueModule shadowsModule = new DialogueModule("The shadows have been my solace. They whisper tales of distant lands and ancient secrets. Do you wish to learn the art of the shadow?");
                shadowsModule.AddOption("Yes, teach me the ways of the shadow.",
                    p => true,
                    p =>
                    {
                        DialogueModule learnModule = new DialogueModule("The shadow, while a place of refuge, can also be a place of peril. Many have sought its depths, but few return. It is not a path for everyone.");
                        learnModule.AddOption("I will take my chances.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Cloak Charlie nods approvingly, a faint smile on his lips.");
                            });
                        learnModule.AddOption("Perhaps I am not ready yet.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, learnModule));
                    });
                shadowsModule.AddOption("No, I will stay in the light.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, shadowsModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
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
                    DialogueModule rewardModule = new DialogueModule("In silence, one can hear the faintest of whispers, the softest of secrets. In return for your discretion, I offer you this reward.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Replace with your actual item
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("I must be going.",
            player => true,
            player =>
            {
                player.SendMessage("Cloak Charlie nods slightly as you turn away.");
            });

        return greeting;
    }

    public CloakCharlie(Serial serial) : base(serial) { }

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