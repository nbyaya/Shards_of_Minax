using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ShepherdessMary : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ShepherdessMary() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Shepherdess Mary";
        Body = 0x191; // Human female body

        // Stats
        SetStr(75);
        SetDex(65);
        SetInt(55);
        SetHits(65);

        // Appearance
        AddItem(new Kilt() { Hue = 1115 });
        AddItem(new Shirt() { Hue = 1114 });
        AddItem(new Sandals() { Hue = 0 });
        AddItem(new ShepherdsCrook() { Name = "Shepherdess Mary's Crook" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public ShepherdessMary(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Oh, it's you. What do you want?");
        
        greeting.AddOption("Tell me about your job.",
            player => true,
            player => {
                DialogueModule jobModule = new DialogueModule("My job? I herd sheep. It's as dull as it sounds, but there's more to my story than that.");
                jobModule.AddOption("What do you mean?",
                    pl => true,
                    pl => {
                        DialogueModule storyModule = new DialogueModule("You see, I wasn't always just a shepherdess. I once had a very different life before... before they took me.");
                        storyModule.AddOption("Who took you?",
                            pla => true,
                            pla => {
                                DialogueModule abductionModule = new DialogueModule("Strange beings. I still remember the night they came, shimmering lights in the sky, and before I knew it, I was aboard their ship.");
                                abductionModule.AddOption("Tell me more about the ship.",
                                    plb => true,
                                    plb => {
                                        DialogueModule shipModule = new DialogueModule("It was unlike anything I had ever seen. Metallic walls, glowing buttons, and a strange humming sound. I felt as though I had stepped into a dream.");
                                        shipModule.AddOption("What did they want with you?",
                                            plc => true,
                                            plc => {
                                                DialogueModule wantModule = new DialogueModule("They were curious, I suppose. They studied me like an animal in a zoo. I remember being frightened but also oddly fascinated.");
                                                wantModule.AddOption("Did they hurt you?",
                                                    pld => true,
                                                    pld => {
                                                        DialogueModule hurtModule = new DialogueModule("No, they didn't hurt me. But they were very different from us. Their eyes were large, and they communicated through thoughts, not words.");
                                                        hurtModule.AddOption("That's terrifying!",
                                                            ple => true,
                                                            ple => ple.SendGump(new DialogueGump(ple, CreateGreetingModule())));
                                                        hurtModule.AddOption("Did you escape?",
                                                            ple => true,
                                                            ple => {
                                                                DialogueModule escapeModule = new DialogueModule("Eventually, yes. After what felt like an eternity, I managed to slip away during a moment of distraction. I found myself in a strange world... this one.");
                                                                escapeModule.AddOption("What was this world like?",
                                                                    plf => true,
                                                                    plf => {
                                                                        DialogueModule newWorldModule = new DialogueModule("It was beautiful and wild, with rolling hills and thick forests. But it was also dangerous, filled with creatures I had never seen before.");
                                                                        newWorldModule.AddOption("What kind of creatures?",
                                                                            plg => true,
                                                                            plg => {
                                                                                DialogueModule creaturesModule = new DialogueModule("Wolves that could talk, deer that glimmered like stars, and shadows that moved without bodies. I felt like a character in a fairy tale.");
                                                                                creaturesModule.AddOption("Did you make friends?",
                                                                                    plh => true,
                                                                                    plh => {
                                                                                        DialogueModule friendsModule = new DialogueModule("Yes! I met a talking wolf named Greymane who became my protector. He helped me adapt to this world.");
                                                                                        friendsModule.AddOption("What about the aliens?",
                                                                                            plj => true,
                                                                                            plj => {
                                                                                                DialogueModule aliensModule = new DialogueModule("I often wonder if they'll come back. I hope they learned from me, but part of me is afraid they might try to take me again.");
                                                                                                aliensModule.AddOption("Stay strong; you have survived.",
                                                                                                    plk => true,
                                                                                                    plk => plk.SendGump(new DialogueGump(plk, CreateGreetingModule())));
                                                                                                plh.SendGump(new DialogueGump(plh, aliensModule));
                                                                                            });
                                                                                        plg.SendGump(new DialogueGump(plg, friendsModule));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, creaturesModule));
                                                                            });
                                                                        plf.SendGump(new DialogueGump(plf, newWorldModule));
                                                                    });
                                                                ple.SendGump(new DialogueGump(ple, escapeModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, hurtModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, wantModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, abductionModule));
                                    });
                                player.SendGump(new DialogueGump(player, storyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, jobModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player => {
                DialogueModule healthModule = new DialogueModule("Healthy as a sheep, unlike you, I suppose.");
                healthModule.AddOption("Glad to hear it.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What do you know about the wilderness?",
            player => true,
            player => {
                DialogueModule wildernessModule = new DialogueModule("The wilderness is a harsh mistress. There are wolves, thieves, and worse. But I've befriended a few animals that help keep me and my sheep safe.");
                wildernessModule.AddOption("That sounds tough.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, wildernessModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player => {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the legendary black sheep. I found one once, and for helping me, I'll share a piece of its wool with you.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return greeting;
    }

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
