using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BunnyBob : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BunnyBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Bunny Bob";
        Body = 0x190; // Human male body

        SetStr(80);
        SetDex(60);
        SetInt(120);

        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1153 });
        AddItem(new FloppyHat() { Hue = 1153 });
        AddItem(new Sandals() { Hue = 1153 });
        AddItem(new ShepherdsCrook() { Name = "Bunny Bob's Crook" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BunnyBob(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Bunny Bob, the animal tamer! What would you like to know?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoAmI = new DialogueModule("I am Bunny Bob, an animal tamer who has a deep bond with all kinds of creatures. It's in my family for generations!");
                whoAmI.AddOption("Tell me about your family.",
                    p => true,
                    p =>
                    {
                        DialogueModule familyStory = new DialogueModule("Many years ago, my great-great-grandfather found a rabbit with a twisted foot. Instead of hunting it, he cared for it. The village kids started calling him Bunny Bob, and the name stuck for future generations!");
                        familyStory.AddOption("That's a wonderful story.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        familyStory.AddOption("Tell me more about your ancestors.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule ancestorStory = new DialogueModule("My ancestors were known for their kindness towards animals. One of them even tamed a legendary rabbit, a beast so fierce it once killed twenty knights before being destroyed by the holy hand grenade.");
                                ancestorStory.AddOption("A legendary rabbit? Tell me more!",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule legendaryRabbit = new DialogueModule("Ah, the Legendary Rabbit of Britannia. It was no ordinary creature. With eyes that glowed red like embers and teeth as sharp as daggers, it was said to be an ancient guardian of the forest. Many believed it was cursed, a creature born from dark magic.");
                                        legendaryRabbit.AddOption("Why did it attack the knights?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule rabbitAttack = new DialogueModule("The knights had ventured into its territory, seeking glory and riches. They thought it was just a simple rabbit guarding a treasure, but they were wrong. The rabbit was the protector of the sacred grove, a place that should never be disturbed.");
                                                rabbitAttack.AddOption("What happened during the battle?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule battleStory = new DialogueModule("The battle was fierce. The knights charged with swords and shields, but the rabbit moved with blinding speed. It leapt from one knight to another, tearing through their armor like paper. One by one, they fell to its ferocity until only one knight remained, the bearer of the holy hand grenade.");
                                                        battleStory.AddOption("The holy hand grenade?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule grenadeStory = new DialogueModule("Yes, the holy hand grenade. It was a relic blessed by the High Priests of Britannia, a weapon meant to banish evil. The last knight, knowing he couldn't defeat the rabbit in direct combat, pulled out the grenade and recited the sacred chant to activate it.");
                                                                grenadeStory.AddOption("What was the sacred chant?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule chantStory = new DialogueModule("The chant went like this: 'One, two, five!' - though some say it was actually 'three' instead of 'five'. The knight, in his panic, almost got it wrong, but managed to throw the grenade just in time. The explosion was brilliant, and the rabbit was finally destroyed, leaving behind only a puff of smoke and a single tuft of fur.");
                                                                        chantStory.AddOption("What happened to the tuft of fur?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule tuftStory = new DialogueModule("The tuft of fur was kept as a relic by my family. It is said to possess magical properties, though none have managed to unlock its secrets. Some say it holds the spirit of the rabbit, waiting for the day it can be reborn.");
                                                                                tuftStory.AddOption("Can I see the tuft of fur?",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendMessage("Bunny Bob shows you a small glass vial containing a tuft of white fur. It seems to shimmer with a faint, otherworldly glow.");
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                tuftStory.AddOption("That's a fascinating tale.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, tuftStory));
                                                                            });
                                                                        chantStory.AddOption("That's an incredible story.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, chantStory));
                                                                    });
                                                                grenadeStory.AddOption("What happened to the knight?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule knightStory = new DialogueModule("The knight returned to his kingdom a hero, but he was forever haunted by the memory of the rabbit. He hung up his sword and vowed never to fight again, dedicating his life to protecting the sacred places of Britannia.");
                                                                        knightStory.AddOption("He must have been very brave.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, knightStory));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, grenadeStory));
                                                            });
                                                        battleStory.AddOption("That must have been terrifying.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, battleStory));
                                                    });
                                                rabbitAttack.AddOption("It sounds like the knights were reckless.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, rabbitAttack));
                                            });
                                        legendaryRabbit.AddOption("Did anyone else try to tame the rabbit?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule tamingAttempt = new DialogueModule("Many tried, but none succeeded. The rabbit was not meant to be tamed; it was a force of nature, a guardian. It was said that only one with a pure heart and true understanding of the forest could ever hope to befriend it, but no one ever proved worthy.");
                                                tamingAttempt.AddOption("A tragic tale.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, tamingAttempt));
                                            });
                                        pla.SendGump(new DialogueGump(pla, legendaryRabbit));
                                    });
                                pl.SendGump(new DialogueGump(pl, ancestorStory));
                            });
                        p.SendGump(new DialogueGump(p, familyStory));
                    });
                whoAmI.AddOption("That's all I wanted to know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, whoAmI));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job is to tame and care for animals. I have a special bond with these creatures, and I ensure they are well-treated.");
                jobModule.AddOption("What kind of animals have you tamed?",
                    p => true,
                    p =>
                    {
                        DialogueModule animalsModule = new DialogueModule("Over the years, I've tamed a variety of animals, from the humble rabbit to the majestic griffin. Have you ever encountered a rare creature in your travels?");
                        animalsModule.AddOption("Yes, I have seen many rare creatures.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        animalsModule.AddOption("No, tell me more about them.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule rareCreatures = new DialogueModule("There are many rare creatures in the land of Britannia. I once had the chance to tame a phoenix! Do you have any tales of encounters with legendary beasts?");
                                rareCreatures.AddOption("That sounds amazing.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, rareCreatures));
                            });
                        p.SendGump(new DialogueGump(p, animalsModule));
                    });
                jobModule.AddOption("That's all I wanted to know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you need help with anything?",
            player => true,
            player =>
            {
                DialogueModule helpModule = new DialogueModule("The forest around here is rich with herbs and plants that have healing properties. If you bring me some blueleaf herbs, I might reward you with something special.");
                helpModule.AddOption("What are blueleaf herbs?",
                    p => true,
                    p =>
                    {
                        DialogueModule blueleafModule = new DialogueModule("Ah, blueleaf! It's a remarkable herb with azure leaves. Not only can it heal, but it can also rejuvenate the spirit. If you bring me some, I will reward you.");
                        blueleafModule.AddOption("I have brought the blueleaf herbs.",
                            pl => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                            pl =>
                            {
                                pl.SendMessage("Ah, blueleaf! Here's a reward for your efforts in bringing it to me.");
                                pl.AddToBackpack(new ImbuingAugmentCrystal()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        blueleafModule.AddOption("I haven't got them yet.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, blueleafModule));
                    });
                helpModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, helpModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Bunny Bob waves goodbye as you leave.");
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