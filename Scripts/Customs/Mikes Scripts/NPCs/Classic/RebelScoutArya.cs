using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

public class RebelScoutArya : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public RebelScoutArya() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rebel Scout Arya";
        Body = 0x191; // Human female body

        // Stats
        SetStr(130);
        SetDex(120);
        SetInt(50);
        SetHits(90);

        // Appearance
        AddItem(new ShortPants() { Hue = 5005 });
        AddItem(new Shirt() { Hue = 5005 });
        AddItem(new Cap() { Hue = 5005 });
        AddItem(new Boots() { Hue = 5005 });
        AddItem(new Crossbow() { Name = "Arya's Quickbow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize the reward timer
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
        DialogueModule greeting = new DialogueModule("I am Rebel Scout Arya, hiding from the Empire's gaze. Why have you approached me?");

        greeting.AddOption("Why are you hiding?",
            player => true,
            player =>
            {
                DialogueModule hidingModule = new DialogueModule("I deserted the rebellion against the Galactic Empire. They turned from ideals of freedom to ruthless tyranny. I could no longer support their cause.");
                hidingModule.AddOption("What made you desert?",
                    p => true,
                    p =>
                    {
                        DialogueModule desertModule = new DialogueModule("It was the moment I realized the rebellion had become as oppressive as the Empire itself. Too many innocent lives lost for a cause that had strayed from its original path.");
                        desertModule.AddOption("What do you mean?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule meaningModule = new DialogueModule("The Empire is evil, yes, but the rebels began executing dissenters within their ranks. I could no longer align myself with such brutality.");
                                meaningModule.AddOption("That sounds brutal. What happened next?",
                                    p1 => true,
                                    p1 =>
                                    {
                                        DialogueModule nextModule = new DialogueModule("I fled during the chaos of a battle, hiding among the fallen. It was my only chance. I've been living in the shadows ever since, evading both the Empire and my former comrades.");
                                        nextModule.AddOption("Do you regret it?",
                                            p2 => true,
                                            p2 =>
                                            {
                                                DialogueModule regretModule = new DialogueModule("Regret? Sometimes, yes. I miss the camaraderie and the fight for justice, but I refuse to be part of a system that condones murder.");
                                                p2.SendGump(new DialogueGump(p2, regretModule));
                                            });
                                        p1.SendGump(new DialogueGump(p1, nextModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, meaningModule));
                            });
                        p.SendGump(new DialogueGump(p, desertModule));
                    });

                greeting.AddOption("What do you do now?",
                    playerq => true,
                    playerq =>
                    {
                        DialogueModule currentModule = new DialogueModule("I gather information, lurking in the shadows. I can't stand idle while the Empire grows stronger. I hope to expose their crimes one day.");
                        currentModule.AddOption("How do you gather information?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule gatherModule = new DialogueModule("I observe the movements of Empire troops, eavesdrop on conversations in the taverns, and exchange whispers with other outcasts. Every scrap of knowledge is a weapon.");
                                gatherModule.AddOption("Have you ever been caught?",
                                    p1 => true,
                                    p1 =>
                                    {
                                        DialogueModule caughtModule = new DialogueModule("Once, I was nearly caught. The Empire has eyes everywhere, and one careless mistake nearly cost me my life. I learned to be more cautious.");
                                        p1.SendGump(new DialogueGump(p1, caughtModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, gatherModule));
                            });
                        player.SendGump(new DialogueGump(player, currentModule));
                    });

            });

        greeting.AddOption("Do you have any tasks for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule taskModule = new DialogueModule("There’s a location not far from here where we’ve hidden supplies. Retrieve them for me, and I'll give you a reward fitting for your efforts.");
                    taskModule.AddOption("I'll do it!",
                        pl => true,
                        pl =>
                        {
                            pl.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            pl.SendMessage("You've received a Maxxia Scroll as a reward!");
                        });
                    player.SendGump(new DialogueGump(player, taskModule));
                }
            });

        greeting.AddOption("What are your thoughts on the Empire?",
            player => true,
            player =>
            {
                DialogueModule empireModule = new DialogueModule("The Empire is a ruthless force, crushing any who oppose it. Their grip on power tightens every day, and they will stop at nothing to eliminate dissent.");
                empireModule.AddOption("How can we fight them?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule fightModule = new DialogueModule("We must unite those who are willing to stand against them. Knowledge is power; we need to expose their tyranny to the galaxy.");
                        fightModule.AddOption("Is there a chance for peace?",
                            p1 => true,
                            p1 =>
                            {
                                DialogueModule peaceModule = new DialogueModule("Peace? In my experience, it’s a fleeting dream. The Empire only understands power. It’s up to us to show them they cannot silence every voice.");
                                p1.SendGump(new DialogueGump(p1, peaceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, fightModule));
                    });
                player.SendGump(new DialogueGump(player, empireModule));
            });

        greeting.AddOption("Tell me about your past.",
            player => true,
            player =>
            {
                DialogueModule pastModule = new DialogueModule("I was once a proud member of the rebellion, fighting for freedom. But as time passed, the lines between right and wrong blurred.");
                pastModule.AddOption("What do you miss about it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule missModule = new DialogueModule("I miss the camaraderie, the sense of purpose. But what I miss most is the hope that we were fighting for something greater.");
                        missModule.AddOption("Hope is important.",
                            p1 => true,
                            p1 =>
                            {
                                DialogueModule hopeModule = new DialogueModule("Indeed. Without hope, we are lost. I still cling to that hope, even as I hide away from those who once fought beside me.");
                                p1.SendGump(new DialogueGump(p1, hopeModule));
                            });
                        pl.SendGump(new DialogueGump(pl, missModule));
                    });
                player.SendGump(new DialogueGump(player, pastModule));
            });

        return greeting;
    }

    public RebelScoutArya(Serial serial) : base(serial) { }

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
