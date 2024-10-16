using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CompassClyde : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CompassClyde() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Compass Clyde";
        Body = 0x190; // Human male body

        // Stats
        SetStr(108);
        SetDex(53);
        SetInt(117);
        SetHits(73);

        // Appearance
        AddItem(new ShortPants() { Hue = 1152 });
        AddItem(new Doublet() { Hue = 44 });
        AddItem(new Shoes() { Hue = 1155 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0;

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public CompassClyde(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I'm Compass Clyde, the world's greatest cartographer! What brings you to me today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I'm Compass Clyde, the one and only! I draw maps, chart unknown lands, and uncover secrets no one else dares. It's a life of adventure and thrill! Not to mention, my obsession with exotic cheeses has led me to some of the strangest places.");
                whoModule.AddOption("Exotic cheeses? Tell me more!",
                    p => true,
                    p =>
                    {
                        DialogueModule cheeseModule = new DialogueModule("Ah, yes! Cheeses! My travels have taken me across continents, through forests, deserts, and even perilous mountains all in search of the finest, most exotic cheeses. Each region has its own unique delicacy!");
                        cheeseModule.AddOption("What's the rarest cheese you've found?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule rareCheeseModule = new DialogueModule("The rarest cheese? That would be the 'Moonlit Gorgonzola'. It can only be made under the light of a full moon, deep within the Whispering Woods. It's said that the cows must graze on enchanted grass for the milk to gain its unique flavor. I nearly got lost in those woods, but the taste was worth it!");
                                rareCheeseModule.AddOption("That sounds incredible!",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule incredibleModule = new DialogueModule("Oh, it truly is! The texture, the flavor, the aroma—it's unlike anything you've ever experienced. The journey to obtain it is fraught with danger, but for a true cheese connoisseur, it's the ultimate reward.");
                                        incredibleModule.AddOption("Did you face any dangers in the Whispering Woods?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule dangerModule = new DialogueModule("Oh, absolutely! The Whispering Woods are home to creatures that don't take kindly to trespassers. Wolves, enchanted guardians, and even mischievous faeries tried to lead me astray. I had to rely on my compass and my wits to make it out.");
                                                dangerModule.AddOption("You must have been terrified!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule fearModule = new DialogueModule("Terrified? Ha! Perhaps a little, but the thought of that cheese kept me going. You see, when you're driven by a passion—whether it's for maps or for cheese—you find courage you never knew you had.");
                                                        fearModule.AddOption("That's admirable, Clyde.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, fearModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, dangerModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, incredibleModule));
                                    });
                                rareCheeseModule.AddOption("That sounds too risky for me.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, rareCheeseModule));
                            });
                        cheeseModule.AddOption("What other cheeses have you found?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule otherCheeseModule = new DialogueModule("There are so many! The 'Crimson Brie' from the far-off mountains of Valdor, aged in volcanic caves. Then there's the 'Golden Havarti' from the fields of Suncrest, made by monks who sing to the cows as they milk them. Each cheese has a story, and each story is an adventure.");
                                otherCheeseModule.AddOption("Tell me about the Crimson Brie.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule crimsonModule = new DialogueModule("The Crimson Brie gets its name from the volcanic soil that gives the milk its distinct flavor. The journey up the mountain is treacherous, with narrow paths and sudden rockfalls. But once you reach the aging caves, the smell of the cheese hits you—a mix of earth, fire, and something inexplicably rich.");
                                        crimsonModule.AddOption("Did you face any obstacles?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule obstaclesModule = new DialogueModule("Oh, plenty! The path was guarded by mountain trolls, and I had to negotiate my way past them. Trolls aren't usually fond of cheese, but a good map, showing them where they could find fresh goats, worked wonders.");
                                                obstaclesModule.AddOption("Smart thinking, Clyde!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, obstaclesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, crimsonModule));
                                    });
                                otherCheeseModule.AddOption("Tell me about the Golden Havarti.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule havartiModule = new DialogueModule("The Golden Havarti is a cheese of legends. The monks believe that by singing to the cows, the milk gains a serene quality, which translates into the cheese. I stayed with them for a week, learning their songs and helping with the process. It was peaceful, almost magical.");
                                        havartiModule.AddOption("Did the singing really make a difference?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule singingModule = new DialogueModule("I believe it did. The cows seemed happier, and the milk was sweeter. Whether it's the singing or just the care they put into it, the result is undeniable—Golden Havarti is like a piece of sunshine on your tongue.");
                                                singingModule.AddOption("I'd love to try it someday!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, singingModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, havartiModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, otherCheeseModule));
                            });
                        cheeseModule.AddOption("That's enough about cheese for now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, cheeseModule));
                    });
                whoModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Safe travels, adventurer!");
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("Tell me about the cave.",
            player => true,
            player =>
            {
                DialogueModule caveModule = new DialogueModule("The cave to the east is a true labyrinth. Full of twists, turns, and dangers aplenty. Legend tells of a hidden treasure within - The Compass of Truth. Dare you explore it?");
                caveModule.AddOption("Tell me more about the treasure.",
                    p => true,
                    p =>
                    {
                        DialogueModule treasureModule = new DialogueModule("The Compass of Truth! A legendary artifact, said to guide you to your heart's deepest desire. It's a perilous journey, but the reward is beyond imagination.");
                        treasureModule.AddOption("I'll take on the challenge!",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You set your sights on the cave. May fortune favor you, adventurer!");
                            });
                        treasureModule.AddOption("Perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, treasureModule));
                    });
                caveModule.AddOption("It sounds too dangerous.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, caveModule));
            });

        greeting.AddOption("Do you need any help?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no tasks for you at the moment. Come back later.");
                }
                else
                {
                    DialogueModule helpModule = new DialogueModule("Would you help me map the ocean's depths? Bring me a rare coral from the ocean's floor, and I shall reward you handsomely!");
                    helpModule.AddOption("I'll find the coral for you.",
                        p => true,
                        p =>
                        {
                            p.SendMessage("You accept the task to find the Radiant Coral.");
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        });
                    helpModule.AddOption("Maybe another time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, helpModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Safe travels, adventurer!");
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