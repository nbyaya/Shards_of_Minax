using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class GripGareth : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public GripGareth() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Grip Gareth";
        Body = 0x190; // Human male body

        // Stats
        SetStr(150);
        SetDex(105);
        SetInt(60);
        SetHits(150);

        // Appearance
        AddItem(new LongPants() { Hue = 1153 });
        AddItem(new BodySash() { Hue = 36 });
        AddItem(new ThighBoots() { Hue = 38 });
        AddItem(new PlateGloves() { Name = "Gareth's Gripping Gloves" });

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
        DialogueModule greeting = new DialogueModule("I am Grip Gareth, the wrestler! What brings you to me today?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("My job is to wrestle and entertain the crowd! It's a family tradition.");
                jobModule.AddOption("What do you mean by tradition?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTraditionModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What virtues do you believe in?",
            player => true,
            player => 
            {
                DialogueModule virtueModule = new DialogueModule("Strength and humility go hand in hand. Do you agree?");
                virtueModule.AddOption("Yes, I agree.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateAgreementModule()));
                    });
                virtueModule.AddOption("No, I disagree.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateDisagreementModule()));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("What do you think of gripping gloves?",
            player => true,
            player => 
            {
                DialogueModule glovesModule = new DialogueModule("Gripping gloves are essential for any wrestler. They can make a huge difference in a match. Do you have a brand you prefer?");
                glovesModule.AddOption("I like the Iron Grip gloves.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateIronGripModule()));
                    });
                glovesModule.AddOption("What about the Titan Tights brand?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTitanTightsModule()));
                    });
                glovesModule.AddOption("I've heard good things about the GripMaster gloves.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGripMasterModule()));
                    });
                glovesModule.AddOption("I'm not familiar with any brands.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateBrandUnknownModule()));
                    });
                player.SendGump(new DialogueGump(player, glovesModule));
            });

        greeting.AddOption("Can you share your secret technique?",
            player => true,
            player => 
            {
                DialogueModule secretModule = new DialogueModule("Prove your worth, and maybe I'll let you in on it.");
                player.SendGump(new DialogueGump(player, secretModule));
            });

        greeting.AddOption("I want a reward for my interest in mental strength.",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player => 
            {
                lastRewardTime = DateTime.UtcNow;
                player.AddToBackpack(new FireballAugmentCrystal());
                player.SendMessage("You have been rewarded for your interest in mental strength!");
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player => 
            {
                player.SendMessage("Take care, traveler!");
            });

        return greeting;
    }

    private DialogueModule CreateIronGripModule()
    {
        DialogueModule ironGripModule = new DialogueModule("Ah, the Iron Grip gloves! They're quite popular among wrestlers for their durability. The grip is firm, and they really help maintain control during a match.");
        ironGripModule.AddOption("What about their comfort?",
            pl => true,
            pl => 
            {
                DialogueModule comfortModule = new DialogueModule("While they're great for grip, some say they can be a bit stiff. It might take some time to break them in properly.");
                comfortModule.AddOption("I've heard of a technique to soften them.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateSofteningTechniqueModule()));
                    });
                comfortModule.AddOption("That sounds like a hassle.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateComfortDislikeModule()));
                    });
                pl.SendGump(new DialogueGump(pl, comfortModule));
            });

        return ironGripModule;
    }

    private DialogueModule CreateSofteningTechniqueModule()
    {
        return new DialogueModule("A good technique is to wear them while doing light exercises. This helps mold them to your hands and softens the material over time.");
    }

    private DialogueModule CreateComfortDislikeModule()
    {
        return new DialogueModule("I understand! Comfort is essential. If it doesn't feel right, it can distract you during a match.");
    }

    private DialogueModule CreateTitanTightsModule()
    {
        DialogueModule titanModule = new DialogueModule("Titan Tights? They’ve got a decent reputation. Many claim they offer excellent flexibility, which is crucial for executing moves.");
        titanModule.AddOption("Do they provide good grip as well?",
            pl => true,
            pl => 
            {
                DialogueModule gripOpinionModule = new DialogueModule("Their grip is generally good, but not as strong as Iron Grip. They might slip a bit during intense grappling.");
                gripOpinionModule.AddOption("Would you recommend them?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateTitanRecommendationModule()));
                    });
                gripOpinionModule.AddOption("That sounds like a drawback.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateTitanDrawbackModule()));
                    });
                pl.SendGump(new DialogueGump(pl, gripOpinionModule));
            });

        return titanModule;
    }

    private DialogueModule CreateTitanRecommendationModule()
    {
        return new DialogueModule("If you value flexibility over grip, they could be worth a try. Just keep an eye on how they perform in matches.");
    }

    private DialogueModule CreateTitanDrawbackModule()
    {
        return new DialogueModule("Yes, it's crucial to have a secure grip. If you lose that during a match, it could cost you the win!");
    }

    private DialogueModule CreateGripMasterModule()
    {
        DialogueModule gripMasterModule = new DialogueModule("The GripMaster gloves are known for their unique texture. They claim to enhance grip without compromising comfort.");
        gripMasterModule.AddOption("Have you tried them?",
            pl => true,
            pl => 
            {
                DialogueModule triedModule = new DialogueModule("I have! They feel great at first, but durability can be an issue. They tend to wear down faster than others.");
                triedModule.AddOption("That's disappointing.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateGripMasterDisappointmentModule()));
                    });
                triedModule.AddOption("Do they come in different styles?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateGripMasterStylesModule()));
                    });
                pl.SendGump(new DialogueGump(pl, triedModule));
            });

        return gripMasterModule;
    }

    private DialogueModule CreateGripMasterDisappointmentModule()
    {
        return new DialogueModule("It is! You want gloves that last. With wrestling being so tough on equipment, you need something reliable.");
    }

    private DialogueModule CreateGripMasterStylesModule()
    {
        return new DialogueModule("Yes, they offer various styles. Some are thicker for added grip, while others are lightweight for agility. It really depends on your preference.");
    }

    private DialogueModule CreateBrandUnknownModule()
    {
        return new DialogueModule("Ah, no worries! The world of gripping gloves is vast. I recommend trying out a few different brands to see what fits your style best.");
    }

    private DialogueModule CreateTraditionModule()
    {
        return new DialogueModule("Wrestling runs deep in my veins. My grandfather, Grasp Graham, was a titan in his time. He taught me that the heart of a wrestler is forged in humility and strength.");
    }

    private DialogueModule CreateAgreementModule()
    {
        return new DialogueModule("Indeed! True strength is not in overpowering others but in mastering oneself.");
    }

    private DialogueModule CreateDisagreementModule()
    {
        return new DialogueModule("It's interesting to hear your perspective. Everyone has their own journey.");
    }

    public GripGareth(Serial serial) : base(serial) { }

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
