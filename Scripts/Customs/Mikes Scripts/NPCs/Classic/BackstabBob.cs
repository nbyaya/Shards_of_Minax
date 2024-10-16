using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BackstabBob : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BackstabBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Backstab Bob";
        Body = 0x190; // Human male body

        // Stats
        SetStr(115);
        SetDex(100);
        SetInt(60);

        SetHits(85);
        SetMana(0);
        SetStam(100);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 15;

        // Appearance
        AddItem(new ShortPants() { Hue = 1153 });
        AddItem(new BoneArms() { Hue = 1153 });
        AddItem(new Dagger { Name = "Bob's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0;

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BackstabBob(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, stranger. I am Backstab Bob, the rogue. What brings you to my neck of the shadows?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule introModule = new DialogueModule("I am Backstab Bob. Some call me a rogue, others a scoundrel. I prefer to think of myself as an expert in acquiring the unattainable.");
                introModule.AddOption("Tell me about your job.",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My job? Well, let's just say I specialize in acquiring hard-to-get items. Discretion is my ally, and shadows my friends.");
                        jobModule.AddOption("That sounds dangerous.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule dangerModule = new DialogueModule("It certainly is. But, in this world, you need to take risks to reap the rewards. And I have become very adept at managing those risks.");
                                dangerModule.AddOption("Fascinating. Goodbye for now.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, dangerModule));
                            });
                        jobModule.AddOption("Where is the best place to backstab someone in town?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule placeModule = new DialogueModule("Ah, now we're getting into the real secrets. The best places depend on what you're looking for: stealth, escape routes, or minimal witnesses.");
                                placeModule.AddOption("Tell me about the stealthiest places.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule stealthModule = new DialogueModule("If you're looking for a stealthy backstab, the alley behind The Broken Dagger tavern is ideal. It's dimly lit, and most folk who wander there are either drunk or looking for trouble. No one asks questions, and the shadows work in your favor.");
                                        stealthModule.AddOption("What about escape routes?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule escapeModule = new DialogueModule("For quick getaways, the market square at dusk is perfect. The crowd provides cover, and there are multiple exits. You can blend in with traders or duck into the nearby sewer grate. It leads directly to the lower tunnels—great for vanishing without a trace.");
                                                escapeModule.AddOption("What if I want minimal witnesses?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule witnessModule = new DialogueModule("Minimal witnesses? The chapel courtyard at midnight is your best bet. Most folks avoid the place after dark due to ghost stories, and the lone guardsman on duty is usually half-asleep. A quick strike, and you're gone before anyone knows what happened.");
                                                        witnessModule.AddOption("Thank you for the tips.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, witnessModule));
                                                    });
                                                escapeModule.AddOption("Thanks, I'll keep that in mind.",
                                                    pld => true,
                                                    pld =>
                                                    {
                                                        pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, escapeModule));
                                            });
                                        stealthModule.AddOption("Thanks for the info.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, stealthModule));
                                    });
                                placeModule.AddOption("Tell me more about these places.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule moreModule = new DialogueModule("There's also the warehouse district. It's full of crates and blind corners. The guards there are easily bribed, and they tend to look the other way if coin crosses their palm. Timing is key though—wait for the shift change.");
                                        moreModule.AddOption("Sounds useful. Goodbye.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, moreModule));
                                    });
                                placeModule.AddOption("Goodbye.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, placeModule));
                            });
                        jobModule.AddOption("Goodbye.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                introModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, introModule));
            });

        greeting.AddOption("Can you tell me about virtue and vice?",
            player => true,
            player =>
            {
                DialogueModule virtueModule = new DialogueModule("True rogues like us must always consider the balance of virtue and vice. Lean too far to either side, and you'll find yourself vulnerable. It's a delicate dance, indeed.");
                virtueModule.AddOption("Are you more honest or deceitful?",
                    p => true,
                    p =>
                    {
                        DialogueModule honestyModule = new DialogueModule("Honesty has its place, but so does deceit. Sometimes, the truth opens doors. Other times, a well-crafted lie does the trick. The real art is knowing when to use which.");
                        honestyModule.AddOption("Thank you for your insight.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, honestyModule));
                    });
                virtueModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("Do you have any items to share?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward for you right now. Please return later.");
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, you're interested in rewards, are you? Since you've shown genuine interest in my tales, I'll gift you something from my collection.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new FireHitAreaCrystal());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendMessage("Backstab Bob gives you a mysterious crystal.");
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Backstab Bob gives you a sly grin as you part ways.");
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