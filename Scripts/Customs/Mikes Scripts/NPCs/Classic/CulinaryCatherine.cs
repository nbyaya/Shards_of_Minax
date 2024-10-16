using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CulinaryCatherine : BaseCreature
{
    private DateTime m_NextRewardTime;

    [Constructable]
    public CulinaryCatherine() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Culinary Catherine";
        Body = 0x191; // Human female body

        // Stats
        SetStr(75);
        SetDex(55);
        SetInt(80);

        SetHits(55);

        // Appearance
        AddItem(new Kilt(2920));
        AddItem(new Surcoat(1151));
        AddItem(new Boots(495));
        AddItem(new LeatherGloves() { Name = "Catherine's Culinary Mitts" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue
    }

    public CulinaryCatherine(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Oh, it's you. What do you want?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule nameModule = new DialogueModule("Culinary Catherine, that's what they call me. Though most don't care enough to ask.");
                nameModule.AddOption("Tell me about your job.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateJobModule()));
                    });
                nameModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Catherine goes back to her cooking.");
                    });
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateJobModule()));
            });

        greeting.AddOption("Do you need any help?",
            player => true,
            player =>
            {
                if (DateTime.Now >= m_NextRewardTime)
                {
                    DialogueModule helpModule = new DialogueModule("It's a lonely world when you pour your heart into your work and no one notices. But here, I appreciate you asking. Take this as a token of my gratitude.");
                    helpModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Reward item
                            m_NextRewardTime = DateTime.Now.AddMinutes(10); // 10 minute cooldown
                            p.SendMessage("You receive a Maxxia Scroll as a token of appreciation.");
                        });
                    player.SendGump(new DialogueGump(player, helpModule));
                }
                else
                {
                    player.SendMessage("You've already received a token of appreciation recently. Come back later.");
                }
            });

        greeting.AddOption("Tell me about your cooking.",
            player => true,
            player =>
            {
                DialogueModule cookingModule = new DialogueModule("My dishes aren't just food; they're art. From the flavors to the presentation, everything is meticulous. Have you ever tried my signature beef bourguignon?");
                cookingModule.AddOption("Tell me more about the bourguignon.",
                    p => true,
                    p =>
                    {
                        DialogueModule bourguignonModule = new DialogueModule("Ah, a connoisseur! That dish takes hours of slow cooking, with the finest cuts of beef, wine, and herbs. The secret? A dash of love and patience.");
                        bourguignonModule.AddOption("Sounds delicious.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, bourguignonModule));
                    });
                cookingModule.AddOption("What about Troll Meat?",
                    p => true,
                    p =>
                    {
                        DialogueModule trollMeatModule = new DialogueModule("Ah, Troll Meat! It's a truly unique ingredient due to its regenerative properties. Properly stored, it can last indefinitely, and when cooked correctly, it can provide incredible health benefits.");
                        trollMeatModule.AddOption("How do you store Troll Meat?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule storageModule = new DialogueModule("The key to storing Troll Meat is keeping it cool and moist. Wrap it in damp cloth and store it in a shadowy place—never let it dry out. The regenerative properties will keep it from spoiling.");
                                storageModule.AddOption("Can it really regenerate?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule regenerateModule = new DialogueModule("Yes, indeed. Troll Meat has an uncanny ability to regenerate itself if stored properly. You could cut off a piece, and within a day or two, it will have regrown to nearly its original size. It's one of the reasons it's so valuable.");
                                        regenerateModule.AddOption("How does this affect cooking it?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule cookingEffectModule = new DialogueModule("Cooking Troll Meat is tricky because of its regenerative properties. If you cook it too slowly, it begins to heal itself even in the pot. The trick is a quick, high-temperature sear to lock in the juices before it has a chance to regenerate.");
                                                cookingEffectModule.AddOption("That sounds challenging.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                cookingEffectModule.AddOption("Are there any recipes?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule recipesModule = new DialogueModule("Oh, there are quite a few! My favorite is Troll Steak with Herb Reduction. The trick is to sear the steak quickly, then add a mixture of wild herbs and simmer for just a few minutes.");
                                                        recipesModule.AddOption("Tell me more about the herb reduction.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule herbModule = new DialogueModule("The herb reduction is a mix of sage, thyme, and a touch of rosemary. These herbs help counteract the somewhat gamey flavor of Troll Meat, making it more palatable. You need to simmer them just enough to release their oils, but not so much that they overpower the meat.");
                                                                herbModule.AddOption("Any other recipes?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule otherRecipesModule = new DialogueModule("Certainly! There's also Troll Meat Stew, which requires dicing the meat into small cubes and cooking it with root vegetables. The stew benefits from the regenerative properties of the meat, which make it incredibly hearty.");
                                                                        otherRecipesModule.AddOption("How do you make it?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule stewModule = new DialogueModule("First, you need to sear the cubes of Troll Meat—just like with the steak—to prevent them from regenerating during cooking. Then, add diced carrots, potatoes, and onions, along with a good bone broth. Let it simmer for an hour, and the result is a rich, filling stew that can keep you going for days.");
                                                                                stewModule.AddOption("Sounds wonderful.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, stewModule));
                                                                            });
                                                                        otherRecipesModule.AddOption("Maybe another time.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, otherRecipesModule));
                                                                    });
                                                                herbModule.AddOption("I'll have to try that.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, herbModule));
                                                            });
                                                        recipesModule.AddOption("Maybe another time.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, recipesModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, cookingEffectModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, regenerateModule));
                                    });
                                storageModule.AddOption("I'll keep that in mind.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, storageModule));
                            });
                        trollMeatModule.AddOption("What are the benefits of eating Troll Meat?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule benefitsModule = new DialogueModule("Troll Meat, when properly prepared, has incredible regenerative effects on those who consume it. It can accelerate healing, restore stamina, and even slightly increase one's resilience to poisons. However, it must be prepared correctly to unlock these properties.");
                                benefitsModule.AddOption("How do you unlock these properties?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule unlockModule = new DialogueModule("The key is to cook it at just the right temperature. Too low, and it retains too much of its raw nature, which can cause stomach upset. Too high, and the regenerative compounds break down. A medium-hot sear followed by a brief rest is ideal.");
                                        unlockModule.AddOption("Got it.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, unlockModule));
                                    });
                                benefitsModule.AddOption("That's fascinating.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, benefitsModule));
                            });
                        trollMeatModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, trollMeatModule));
                    });
                cookingModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, cookingModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Catherine goes back to her cooking.");
            });

        return greeting;
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("My job? I'm a cook, alright? Not that anyone appreciates it!");
        jobModule.AddOption("Tell me about your dishes.",
            player => true,
            player =>
            {
                DialogueModule dishesModule = new DialogueModule("My dishes aren't just food; they're art. From the flavors to the presentation, everything is meticulous.");
                dishesModule.AddOption("Have you cooked for anyone famous?",
                    p => true,
                    p =>
                    {
                        DialogueModule feastModule = new DialogueModule("Ah, that was a day to remember. Fresh game, exotic fruits, and the finest wines. All for the royals, and yet not a single compliment came my way.");
                        feastModule.AddOption("That must have been tough.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, feastModule));
                    });
                dishesModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, dishesModule));
            });
        jobModule.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Catherine goes back to her cooking.");
            });
        return jobModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}