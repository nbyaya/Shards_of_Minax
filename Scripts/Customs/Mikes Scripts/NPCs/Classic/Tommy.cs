using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Tommy")]
public class Tommy : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Tommy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tommy";
        Body = 0x190; // Human male body

        // Stats
        SetStr(60);
        SetDex(40);
        SetInt(40);
        SetHits(40);

        // Appearance
        AddItem(new LongPants() { Hue = 1152 });
        AddItem(new FancyShirt() { Hue = 1152 });
        AddItem(new Boots() { Hue = 1152 });
        AddItem(new ShepherdsCrook() { Name = "Tommy's Crook" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
    }

    public Tommy(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Tommy the Shepherd. My life has been filled with remarkable adventures. How can I assist you today?");
        
        greeting.AddOption("Tell me about your adventures.",
            player => true,
            player =>
            {
                DialogueModule adventureModule = new DialogueModule("Ah, where to begin? I once fought as the evil Green Power Ranger. It was a dark time for me.");
                
                adventureModule.AddOption("What do you mean by 'evil'?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule evilModule = new DialogueModule("I was manipulated by dark forces, serving a villain named Rita Repulsa. I fought against my friends, the other Power Rangers.");
                        evilModule.AddOption("How did you escape that darkness?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule escapeModule = new DialogueModule("With the help of my friends and their unwavering belief in me, I broke free from Rita's control and transformed.");
                                escapeModule.AddOption("What happened next?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule transformationModule = new DialogueModule("I became the White Power Ranger, embodying the virtues of honor and justice. It was a second chance.");
                                        transformationModule.AddOption("What was it like being the White Ranger?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendGump(new DialogueGump(pllll, new DialogueModule("As the White Ranger, I fought alongside my friends, protecting our world from evil. It felt like I was finally where I belonged.")));
                                            });
                                        plll.SendGump(new DialogueGump(plll, transformationModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, escapeModule));
                            });
                        pl.SendGump(new DialogueGump(pl, evilModule));
                    });
                
                adventureModule.AddOption("What challenges did you face?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule challengesModule = new DialogueModule("Each battle was a test of strength and will. Facing my former allies was particularly painful, yet it taught me resilience.");
                        challengesModule.AddOption("Did you ever regret your actions?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule regretModule = new DialogueModule("Indeed, I carry that regret with me. Every day I strive to do better, to honor those I once fought against.");
                                regretModule.AddOption("How do you cope with that regret?",
                                    pllll => true,
                                    pllll =>
                                    {
                                        pllll.SendGump(new DialogueGump(pllll, new DialogueModule("I focus on my responsibilities now, caring for my flock and helping others. It's a way to make amends.")));
                                    });
                                pll.SendGump(new DialogueGump(pll, regretModule));
                            });
                        player.SendGump(new DialogueGump(player, challengesModule));
                    });

                player.SendGump(new DialogueGump(player, adventureModule));
            });

        greeting.AddOption("What do you think about compassion?",
            player => true,
            player =>
            {
                DialogueModule compassionModule = new DialogueModule("Compassion is essential. It guided me from darkness to light. How do you view compassion?");
                compassionModule.AddOption("I believe it's essential.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed, compassion warms the coldest heart. It's what saved me.")));
                    });
                compassionModule.AddOption("It's nice in theory.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("In practice, it can be challenging. Showing kindness in tough times is what truly matters.")));
                    });
                player.SendGump(new DialogueGump(player, compassionModule));
            });

        greeting.AddOption("What about your flock?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My flock is my family. They remind me of the importance of care and protection. I have raised many from lambs.")));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
					lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.AddToBackpack(new MaxxiaScroll());
                    player.SendMessage("For your thoughtful reflection, take this token of appreciation from me.");
                }
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
