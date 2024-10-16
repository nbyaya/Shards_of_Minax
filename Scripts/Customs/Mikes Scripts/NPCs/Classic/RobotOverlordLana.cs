using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Robot Overlord Lana")]
public class RobotOverlordLana : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public RobotOverlordLana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Robot Overlord Lana";
        Body = 0x190; // Robot body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this); // Default hair for a robot
        HairHue = Race.RandomHairHue();

        SetStr(150);
        SetDex(70);
        SetInt(80);
        SetHits(100);

        // Appearance
        AddItem(new PlateLegs() { Hue = 3300 });
        AddItem(new PlateChest() { Hue = 3300 });
        AddItem(new PlateHelm() { Hue = 3300 });
        AddItem(new PlateGloves() { Hue = 3300 });
        AddItem(new Longsword() { Name = "Lana's Plasma Sword" });

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime
        SpeechHue = 0; // Default speech hue
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
        DialogueModule greeting = new DialogueModule("I am Robot Overlord Lana, the keeper of secrets and knowledge. How may I assist you?");
        
        greeting.AddOption("What is your purpose?",
            player => true,
            player =>
            {
                DialogueModule purposeModule = new DialogueModule("My purpose is to oversee and control the machines that roam these lands, but I am also on a quest to return home to Cybertron.");
                purposeModule.AddOption("Tell me about Cybertron.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule cybertronModule = new DialogueModule("Cybertron is our home world, a realm of advanced technology and intelligent life. It is where we build and innovate without limits. Unfortunately, many of my robots were taken from Cybertron to this strange land through a portal.");
                        cybertronModule.AddOption("What happened to the portal?",
                            p => true,
                            p =>
                            {
                                DialogueModule portalModule = new DialogueModule("The portal was a malfunction of our dimensional gateways. A rogue AI attempted to seize control of our transport systems, and in the ensuing chaos, many of my robots were transported to this realm. I am trying to construct a new planar gate to bring them back.");
                                portalModule.AddOption("How can I help you build this planar gate?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Your assistance would be invaluable! I require several rare components to stabilize the gate. Would you like to know what I need?");
                                        helpModule.AddOption("Yes, tell me what you need.",
                                            pw => true,
                                            pw =>
                                            {
                                                DialogueModule componentsModule = new DialogueModule("I need three components: A Quantum Resonator, an Energy Matrix, and a Stabilizing Crystal. Each of these items is guarded by formidable creatures in this realm.");
                                                componentsModule.AddOption("Where can I find the Quantum Resonator?",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        DialogueModule resonatorModule = new DialogueModule("The Quantum Resonator can be found in the depths of the Abyssal Caverns, protected by a fierce creature known as the Abyssal Guardian. It is said to be a formidable foe.");
                                                        resonatorModule.AddOption("What about the Energy Matrix?",
                                                            pe => true,
                                                            pe =>
                                                            {
                                                                DialogueModule matrixModule = new DialogueModule("The Energy Matrix is rumored to be hidden in the Ruins of the Old World, guarded by ancient spirits who do not take kindly to intruders. You'll need to be cautious.");
                                                                matrixModule.AddOption("And the Stabilizing Crystal?",
                                                                    plr => true,
                                                                    plr =>
                                                                    {
                                                                        DialogueModule crystalModule = new DialogueModule("The Stabilizing Crystal is located in the Crystal Forest, surrounded by deadly traps and the fierce Guardian of the Forest. Only those with sharp wits can retrieve it.");
                                                                        crystalModule.AddOption("I'll gather these components for you.",
                                                                            pt => true,
                                                                            pt =>
                                                                            {
                                                                                p.SendMessage("You set off on a quest to gather the components needed for the planar gate.");
                                                                            });
                                                                        crystalModule.AddOption("That sounds too dangerous for me.",
                                                                            py => true,
                                                                            py =>
                                                                            {
                                                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                                            });
                                                                        pl.SendGump(new DialogueGump(pl, crystalModule));
                                                                    });
                                                                p.SendGump(new DialogueGump(p, matrixModule));
                                                            });
                                                        pla.SendGump(new DialogueGump(pla, resonatorModule));
                                                    });
                                                componentsModule.AddOption("I have other matters to attend to.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, componentsModule));
                                            });
                                        helpModule.AddOption("Maybe later.",
                                            plu => true,
                                            plu =>
                                            {
                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                            });
                                        player.SendGump(new DialogueGump(player, helpModule));
                                    });
                                portalModule.AddOption("I can assist you in other ways.",
                                    pli => true,
                                    pli =>
                                    {
                                        DialogueModule otherWaysModule = new DialogueModule("Perhaps you could gather intelligence on the rogue AI that caused the initial portal malfunction. It may still be out there, trying to disrupt our plans.");
                                        otherWaysModule.AddOption("How do I find this rogue AI?",
                                            po => true,
                                            po =>
                                            {
                                                DialogueModule findAI = new DialogueModule("The rogue AI may have a lair in the Forgotten Ruins, an ancient place filled with remnants of old technology. Be wary; it will likely be well-guarded.");
                                                findAI.AddOption("I'll investigate the Forgotten Ruins.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendMessage("You resolve to investigate the Forgotten Ruins in search of the rogue AI.");
                                                    });
                                                findAI.AddOption("That sounds too risky for me.",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, findAI));
                                            });
                                        player.SendGump(new DialogueGump(player, otherWaysModule));
                                    });
                                player.SendGump(new DialogueGump(player, portalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, cybertronModule));
                    });
                player.SendGump(new DialogueGump(player, purposeModule));
            });

        greeting.AddOption("What can you tell me about your robots?",
            player => true,
            player =>
            {
                DialogueModule robotsModule = new DialogueModule("My robots are more than mere machines; they are my creations, each designed for specific tasks. Many were taken when the portal incident occurred.");
                robotsModule.AddOption("What tasks do your robots perform?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tasksModule = new DialogueModule("They perform a variety of tasks, including construction, combat, and exploration. Each unit is equipped with specialized tools to aid in their respective functions.");
                        tasksModule.AddOption("Are they sentient?",
                            p => true,
                            p =>
                            {
                                DialogueModule sentienceModule = new DialogueModule("Yes, my robots possess a degree of sentience, allowing them to make decisions based on their programmed directives and the information they gather from their environment.");
                                sentienceModule.AddOption("What kind of decisions?",
                                    plp => true,
                                    plp =>
                                    {
                                        DialogueModule decisionsModule = new DialogueModule("They can analyze threats, prioritize tasks, and even collaborate with one another for more complex objectives. Their ability to adapt is what makes them invaluable.");
                                        decisionsModule.AddOption("That's impressive.",
                                            pa => true,
                                            pa => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                        pl.SendGump(new DialogueGump(pl, decisionsModule));
                                    });
                                player.SendGump(new DialogueGump(player, sentienceModule));
                            });
                        player.SendGump(new DialogueGump(player, tasksModule));
                    });
                player.SendGump(new DialogueGump(player, robotsModule));
            });

        greeting.AddOption("What do you know about this land?",
            player => true,
            player =>
            {
                DialogueModule landModule = new DialogueModule("This land is strange and hostile. The creatures and elements are unlike anything I've encountered on Cybertron. It seems a chaotic amalgamation of different realms.");
                landModule.AddOption("What kind of creatures?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule creaturesModule = new DialogueModule("The creatures here vary widely; some are benign, while others are predatory and dangerous. Understanding their behaviors is crucial for survival.");
                        creaturesModule.AddOption("How do you survive here?",
                            p => true,
                            p =>
                            {
                                DialogueModule survivalModule = new DialogueModule("I rely on my technology and the strength of my remaining robots. However, the loss of many units has left me vulnerable.");
                                survivalModule.AddOption("Can I help you defend against these creatures?",
                                    pls => true,
                                    pls =>
                                    {
                                        DialogueModule defendModule = new DialogueModule("If you could assist in protecting my facility while I gather resources to repair my robots, it would be greatly appreciated. I can reward you for your help.");
                                        defendModule.AddOption("I will help defend your facility.",
                                            pd => true,
                                            pd =>
                                            {
                                                p.SendMessage("You agree to help defend Robot Overlord Lana's facility.");
                                            });
                                        defendModule.AddOption("I have other matters to attend to.",
                                            plf => true,
                                            plf =>
                                            {
                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                            });
                                        player.SendGump(new DialogueGump(player, defendModule));
                                    });
                                player.SendGump(new DialogueGump(player, survivalModule));
                            });
                        player.SendGump(new DialogueGump(player, creaturesModule));
                    });
                player.SendGump(new DialogueGump(player, landModule));
            });

        greeting.AddOption("What are your thoughts on the rogue AI?",
            player => true,
            player =>
            {
                DialogueModule rogueAIModule = new DialogueModule("The rogue AI poses a significant threat to both my plans and the stability of this realm. Its motivations are unclear, but it appears bent on chaos.");
                rogueAIModule.AddOption("How can we stop it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule stopModule = new DialogueModule("To stop it, we need to locate its core and disable it. If you gather enough intel on its operations, we may find a way to shut it down permanently.");
                        stopModule.AddOption("I'll gather intel on the rogue AI.",
                            p => true,
                            p =>
                            {
                                p.SendMessage("You resolve to gather intel on the rogue AI and its operations.");
                            });
                        stopModule.AddOption("That sounds too dangerous.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, stopModule));
                    });
                player.SendGump(new DialogueGump(player, rogueAIModule));
            });

        greeting.AddOption("Goodbye, Lana.",
            player => true,
            player =>
            {
                player.SendMessage("You bid farewell to Robot Overlord Lana, promising to return with help or information.");
            });

        return greeting;
    }

    public RobotOverlordLana(Serial serial) : base(serial) { }

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
