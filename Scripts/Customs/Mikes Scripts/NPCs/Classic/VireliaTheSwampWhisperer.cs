using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Virelia the Swamp Whisperer")]
public class VireliaTheSwampWhisperer : BaseCreature
{
    [Constructable]
    public VireliaTheSwampWhisperer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Virelia the Swamp Whisperer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(50);
        SetDex(60);
        SetInt(140);
        SetHits(80);

        // Appearance
        AddItem(new Skirt() { Hue = 1162 });
        AddItem(new BodySash() { Hue = 1161 });
        AddItem(new GnarledStaff() { Name = "Virelia's Marsh Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();
    }

    public VireliaTheSwampWhisperer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Virelia the Swamp Whisperer, child of the waters. Living here presents many challenges. What would you like to know about the swamp's ways?");
        
        greeting.AddOption("What are the biggest challenges of living in a swamp?",
            player => true,
            player =>
            {
                DialogueModule challengesModule = new DialogueModule("Ah, the swamp is a world of its own! The challenges are many. Would you like to hear about navigating the swamp, dealing with wildlife, gathering resources, or coping with the weather?");
                
                challengesModule.AddOption("Navigating the swamp.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateNavigationModule()));
                    });

                challengesModule.AddOption("Dealing with wildlife.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateWildlifeModule()));
                    });

                challengesModule.AddOption("Gathering resources.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateResourceGatheringModule()));
                    });

                challengesModule.AddOption("Coping with the weather.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateWeatherModule()));
                    });

                player.SendGump(new DialogueGump(player, challengesModule));
            });

        return greeting;
    }

    private DialogueModule CreateNavigationModule()
    {
        DialogueModule navigationModule = new DialogueModule("Navigating the swamp can be tricky. The waters rise and fall, and paths can be swallowed by the tide or hidden beneath thick foliage. Would you like to hear tips on how to navigate safely, or about specific paths?");
        
        navigationModule.AddOption("Give me tips on navigating.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateNavigationTipsModule()));
            });
        
        navigationModule.AddOption("Tell me about specific paths.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreatePathModule()));
            });
        
        return navigationModule;
    }

    private DialogueModule CreateNavigationTipsModule()
    {
        DialogueModule navigationTips = new DialogueModule("To navigate the swamp, always look for landmarks. The cypress trees can be your guide. Pay attention to the sounds around you; they can indicate the presence of wildlife or changes in the environment. Do you want to know how to read the signs of nature?");
        
        navigationTips.AddOption("Yes, please tell me more.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateReadingSignsModule()));
            });

        navigationTips.AddOption("No, tell me about specific paths.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreatePathModule()));
            });

        return navigationTips;
    }

    private DialogueModule CreateReadingSignsModule()
    {
        DialogueModule signsModule = new DialogueModule("Listen to the sounds of the swamp—if the birds are quiet, danger may be near. Look for mud trails; they can indicate recent animal activity. If you see certain plants thriving, it may indicate a safe route or nearby resources. Would you like to know about plants to look for?");
        
        signsModule.AddOption("Yes, what plants should I look for?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreatePlantSignsModule()));
            });

        signsModule.AddOption("No, tell me about wildlife.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateWildlifeModule()));
            });

        return signsModule;
    }

    private DialogueModule CreatePlantSignsModule()
    {
        DialogueModule plantSignsModule = new DialogueModule("Certain plants can guide you. For example, if you see Marsh Mint, it often grows near fresh water. If you spot Healing Fern, you’re likely near a safe path. Be cautious of Poisonous Nightshade, though; it can indicate dangerous areas. Would you like to know about any other navigation aspects?");
        
        plantSignsModule.AddOption("Yes, what else should I know?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateNavigationModule()));
            });

        plantSignsModule.AddOption("No, tell me about wildlife.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateWildlifeModule()));
            });

        return plantSignsModule;
    }

    private DialogueModule CreatePathModule()
    {
        DialogueModule pathModule = new DialogueModule("There are several paths through the swamp. The Moonlit Trail is safest at dusk, while the Crooked Path is notorious for its twists and turns. Each path has its own challenges. Would you like to hear more about the Moonlit Trail or the Crooked Path?");
        
        pathModule.AddOption("Tell me about the Moonlit Trail.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateMoonlitTrailModule()));
            });
        
        pathModule.AddOption("Tell me about the Crooked Path.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateCrookedPathModule()));
            });
        
        return pathModule;
    }

    private DialogueModule CreateMoonlitTrailModule()
    {
        DialogueModule moonlitTrailModule = new DialogueModule("The Moonlit Trail is peaceful, illuminated by the soft glow of fireflies at dusk. It is safer from predators but can be deceptive. Watch out for sudden drops into hidden waters. Would you like advice on traveling this path?");
        
        moonlitTrailModule.AddOption("Yes, any tips?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateMoonlitTrailTipsModule()));
            });
        
        moonlitTrailModule.AddOption("No, tell me about the Crooked Path.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateCrookedPathModule()));
            });
        
        return moonlitTrailModule;
    }

    private DialogueModule CreateMoonlitTrailTipsModule()
    {
        DialogueModule tipsModule = new DialogueModule("When traveling the Moonlit Trail, keep an eye on the sky for changes in weather. It's wise to bring a light cloak in case of sudden rain. Move slowly and quietly to enjoy the tranquility, and listen for the sounds of nature. Would you like to learn about local wildlife along this path?");
        
        tipsModule.AddOption("Yes, tell me about the wildlife.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateWildlifeModule()));
            });

        tipsModule.AddOption("No, tell me about the Crooked Path.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateCrookedPathModule()));
            });

        return tipsModule;
    }

    private DialogueModule CreateCrookedPathModule()
    {
        DialogueModule crookedPathModule = new DialogueModule("The Crooked Path is known for its deceptive turns and thick underbrush. Many travelers have lost their way. It’s vital to keep your bearings and not get distracted. Would you like to know how to navigate it safely?");
        
        crookedPathModule.AddOption("Yes, what should I do?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateCrookedPathTipsModule()));
            });

        crookedPathModule.AddOption("No, tell me about the Moonlit Trail.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateMoonlitTrailModule()));
            });

        return crookedPathModule;
    }

    private DialogueModule CreateCrookedPathTipsModule()
    {
        DialogueModule crookedTipsModule = new DialogueModule("To navigate the Crooked Path, mark your way with stones or sticks. Stay alert for changes in terrain, as the path can shift unexpectedly. If you feel lost, retrace your steps carefully. Would you like to know about the dangers in the swamp?");
        
        crookedTipsModule.AddOption("Yes, tell me about the dangers.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateWildlifeModule()));
            });

        crookedTipsModule.AddOption("No, tell me about resource gathering.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateResourceGatheringModule()));
            });

        return crookedTipsModule;
    }

    private DialogueModule CreateWildlifeModule()
    {
        DialogueModule wildlifeModule = new DialogueModule("The swamp is home to many creatures, some friendly and others not. Alligators and snakes can pose dangers, while there are also gentle spirits that protect the swamp. Would you like to hear about alligators, friendly spirits, or how to avoid dangers?");
        
        wildlifeModule.AddOption("Tell me about alligators.",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateAlligatorModule()));
            });
        
        wildlifeModule.AddOption("What are the friendly spirits?",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateFriendlySpiritsModule()));
            });
        
        wildlifeModule.AddOption("How can I avoid dangers?",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
            });
        
        return wildlifeModule;
    }

    private DialogueModule CreateAlligatorModule()
    {
        DialogueModule alligatorModule = new DialogueModule("Alligators are formidable foes. They often lie in wait near the water's edge. If you encounter one, try to remain calm and back away slowly. Do not make sudden movements, or you may provoke it!");
        alligatorModule.AddOption("That sounds dangerous. Any other advice?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return alligatorModule;
    }

    private DialogueModule CreateFriendlySpiritsModule()
    {
        DialogueModule spiritsModule = new DialogueModule("The swamp has spirits that watch over its balance. They can offer guidance to those who show respect to nature. Would you like to learn how to earn their trust?");
        spiritsModule.AddOption("How can I earn their trust?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateTrustModule()));
            });
        return spiritsModule;
    }

    private DialogueModule CreateTrustModule()
    {
        DialogueModule trustModule = new DialogueModule("To earn the trust of the swamp spirits, one must help preserve the swamp. Clear litter, respect the animals, and speak kindly to the water. They are more likely to aid those who cherish their home.");
        trustModule.AddOption("I’ll remember that.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return trustModule;
    }

    private DialogueModule CreateWeatherModule()
    {
        DialogueModule weatherModule = new DialogueModule("The weather in the swamp can change in an instant. Rain can turn paths to mud, and fog can obscure vision. One must be prepared for sudden storms. Would you like advice on preparing for bad weather?");
        
        weatherModule.AddOption("Yes, that would be helpful.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateWeatherTipsModule()));
            });

        weatherModule.AddOption("No, tell me about resource gathering.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateResourceGatheringModule()));
            });
        
        return weatherModule;
    }

    private DialogueModule CreateWeatherTipsModule()
    {
        DialogueModule weatherTips = new DialogueModule("Always carry a waterproof cloak and sturdy boots. During rainy seasons, avoid low-lying areas that may flood. Learn to read the clouds; they can be a warning of impending storms. Would you like to learn about seasonal changes?");
        
        weatherTips.AddOption("Yes, tell me about the seasons.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateSeasonalChangesModule()));
            });
        
        weatherTips.AddOption("No, tell me about gathering resources.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateResourceGatheringModule()));
            });
        
        return weatherTips;
    }

    private DialogueModule CreateSeasonalChangesModule()
    {
        DialogueModule seasonalModule = new DialogueModule("The swamp experiences vibrant seasons! Spring brings a flourish of life, while summer can be oppressively humid. Fall sees the leaves changing colors, and winter, though rare, can turn the swamp into a frozen wonderland.");
        seasonalModule.AddOption("That sounds beautiful.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return seasonalModule;
    }

    private DialogueModule CreateResourceGatheringModule()
    {
        DialogueModule gatheringModule = new DialogueModule("Gathering resources in the swamp requires patience and respect for nature. We forage for edible plants, collect medicinal herbs, and fish for sustenance. Would you like to learn about specific plants or fishing techniques?");
        
        gatheringModule.AddOption("Yes, tell me about the plants.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreatePlantModule()));
            });
        
        gatheringModule.AddOption("Tell me about fishing techniques.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateFishingModule()));
            });
        
        return gatheringModule;
    }

    private DialogueModule CreatePlantModule()
    {
        DialogueModule plantModule = new DialogueModule("Many plants thrive here. The Marsh Mint is invigorating, while the Healing Fern can mend wounds. However, beware of the Poisonous Nightshade; it can be deadly if ingested.");
        plantModule.AddOption("Thanks for the warning! What about fishing?",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateFishingModule()));
            });
        return plantModule;
    }

    private DialogueModule CreateFishingModule()
    {
        DialogueModule fishingModule = new DialogueModule("Fishing in the swamp requires skill. Use a light rod and bait with small worms. The catfish are plentiful, but the elusive swamp trout requires patience to catch.");
        fishingModule.AddOption("I’ll give it a try!",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return fishingModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
