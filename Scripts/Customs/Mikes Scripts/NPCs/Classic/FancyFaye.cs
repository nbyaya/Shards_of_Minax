using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class FancyFaye : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public FancyFaye() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Fancy Faye";
        Body = 0x191; // Human female body

        // Stats
        SetStr(100);
        SetDex(100);
        SetInt(60);

        SetHits(70);

        // Appearance
        AddItem(new Skirt() { Hue = 38 });
        AddItem(new FemaleLeatherChest() { Hue = 38 });
        AddItem(new ThighBoots() { Hue = 38 });
        AddItem(new FeatheredHat() { Hue = 2126 });
        AddItem(new Cutlass() { Name = "Faye's Rapier" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
    }

    public FancyFaye(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Arr, I be Fancy Faye, the most illustrious pirate ye ever laid eyes on! How can I help ye, landlubber?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Ah, I be a pirate, through and through! Stealing treasure and causing mayhem, that be me job. I've sailed through the fiercest of storms and faced the mightiest of foes. What else do ye want to know?");
                aboutModule.AddOption("What about your health?",
                    p => true,
                    p =>
                    {
                        DialogueModule healthModule = new DialogueModule("Me health? Ha, a pirate's health be as good as the rum she drinks! Now, are ye really concerned about that?");
                        healthModule.AddOption("Good to know.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, healthModule));
                    });
                aboutModule.AddOption("Ever fought in any battles?",
                    p => true,
                    p =>
                    {
                        DialogueModule battleModule = new DialogueModule("Ye think ye can handle the life of a pirate, landlubber? Battles are fierce, and only the bravest survive. Do ye have the courage?");
                        battleModule.AddOption("Yes, I do!",
                            pl => true,
                            pl =>
                            {
                                DialogueModule courageModule = new DialogueModule("Ah, ye've got a bit o' fire in ye! But can ye stand tall in a storm, or will ye cower like a scared seagull?");
                                courageModule.AddOption("I can stand tall.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Fancy Faye gives you an approving nod.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                courageModule.AddOption("I'm not so sure...",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Fancy Faye chuckles at your hesitation.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, courageModule));
                            });
                        battleModule.AddOption("Maybe not...",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, battleModule));
                    });
                aboutModule.AddOption("Tell me about your favorite places to find treasure.",
                    p => true,
                    p =>
                    {
                        DialogueModule treasureModule = new DialogueModule("Ah, treasure hunting, is it? Well, I've traveled far and wide, and there be a few places where the booty is grand, but the dangers even grander. Let me tell ye about them.");
                        treasureModule.AddOption("Tell me about the Whispering Isles.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule whisperingIslesModule = new DialogueModule("The Whispering Isles are a mysterious place, full of ancient ruins and haunted whispers. They say the spirits of past pirates guard their treasures there, and many have tried to steal from them, only to be driven mad by the voices. But for those with courage, the gold is plentiful.");
                                whisperingIslesModule.AddOption("How do you deal with the spirits?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule spiritsModule = new DialogueModule("The spirits, aye, they're not easy to deal with. I bring offerings - trinkets and baubles, not of great value but enough to appease 'em. And sometimes, a bit o' rum helps, too. Spirits like rum, ye know. If ye want to try your hand there, bring something shiny and don't forget the rum.");
                                        spiritsModule.AddOption("Thanks for the advice.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, spiritsModule));
                                    });
                                whisperingIslesModule.AddOption("Sounds too risky for me.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, whisperingIslesModule));
                            });
                        treasureModule.AddOption("What about the Cavern of the Lost?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule cavernModule = new DialogueModule("The Cavern of the Lost, now that's a place where only the bold dare venture. It's hidden beneath the cliffs of Skullcrag, and it's said that the entrance only reveals itself at low tide. Inside, the air is damp, and the walls seem to close in on ye. But beyond the shadows, there be chests filled with gold, guarded by creatures that thrive in the dark.");
                                cavernModule.AddOption("What kind of creatures?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule creaturesModule = new DialogueModule("Ah, the creatures. There be giant crabs, with pincers that could snap a man in half, and bats as big as a man's head. But the worst are the shadow serpents, slithering in the dark, near invisible. If ye tread carefully and keep a torch lit, ye might just make it through.");
                                        creaturesModule.AddOption("I'll keep that in mind.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, creaturesModule));
                                    });
                                cavernModule.AddOption("I think I'll pass on that one.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, cavernModule));
                            });
                        treasureModule.AddOption("Have you been to the Sunken City?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule sunkenCityModule = new DialogueModule("The Sunken City lies deep beneath the waves, and only those with the skill to hold their breath or magic to breathe underwater can reach it. It's filled with ancient relics and the gold of an empire long lost. But beware the merfolk; they don't take kindly to intruders, and their songs can lure ye to a watery grave.");
                                sunkenCityModule.AddOption("How do you avoid the merfolk?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule merfolkModule = new DialogueModule("Avoiding the merfolk ain't easy. Their songs are hypnotic. I use earplugs made of beeswax to block out their voices. And if they get too close, a sharp blade can sometimes convince 'em to leave ye be. But it's risky business, and many have drowned trying to take their treasures.");
                                        merfolkModule.AddOption("That's useful to know.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, merfolkModule));
                                    });
                                sunkenCityModule.AddOption("That sounds terrifying. I'll stay on dry land.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, sunkenCityModule));
                            });
                        treasureModule.AddOption("I've heard enough for now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, treasureModule));
                    });
                aboutModule.AddOption("Never mind.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Tell me about the Cursed Emerald of Calypso.",
            player => true,
            player =>
            {
                DialogueModule emeraldModule = new DialogueModule("Ah, ye've heard of me reputation, have ya? Many tales have been spun 'bout the legendary adventures of Fancy Faye. That emerald, it's said to grant untold power to its possessor, but at a heavy cost. I've been searching for it, and if ye help me, there might be a reward in it for ye.");
                emeraldModule.AddOption("What kind of reward?",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            DialogueModule rewardModule = new DialogueModule("Very well, if ye prove to be helpful in me quest for the Cursed Emerald, I might part with some of me treasured doubloons or maybe even a piece of a treasure map. A sample for ye.");
                            rewardModule.AddOption("I'll help you!",
                                pl => true,
                                pl =>
                                {
                                    pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                    pl.SendMessage("Fancy Faye gives you a mysterious scroll as a reward.");
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            rewardModule.AddOption("Maybe later.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, rewardModule));
                        }
                    });
                emeraldModule.AddOption("That sounds dangerous. I think I'll pass.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, emeraldModule));
            });

        greeting.AddOption("Do you know anything about the Lost Medallion of Marauders' Cove?",
            player => true,
            player =>
            {
                DialogueModule medallionModule = new DialogueModule("Legend has it that the medallion is hidden deep within Marauders' Cove, protected by puzzles and traps that have claimed many a pirate's life. Are ye brave enough to seek it?");
                medallionModule.AddOption("I might give it a try.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Fancy Faye grins. 'Good luck, ye'll need it.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                medallionModule.AddOption("Maybe some other time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, medallionModule));
            });

        greeting.AddOption("Goodbye, Faye.",
            player => true,
            player =>
            {
                player.SendMessage("Fancy Faye nods and returns to her musings.");
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