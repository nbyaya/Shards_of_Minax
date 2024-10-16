using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of the Yellow Ranger")]
public class YellowRanger : BaseCreature
{
    [Constructable]
    public YellowRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Yellow Ranger";
        Body = 0x190; // Human body
        Hue = 54; // Armor color

        // Stats
        SetStr(102);
        SetDex(108);
        SetInt(65);
        SetHits(78);

        // Appearance
        AddItem(new StuddedLegs() { Hue = 54 });
        AddItem(new StuddedChest() { Hue = 54 });
        AddItem(new ChainCoif() { Hue = 54 });
        AddItem(new StuddedGloves() { Hue = 54 });
        AddItem(new Boots() { Hue = 54 });
        AddItem(new Spear() { Name = "Yellow Ranger's Spear" });
    }

    public YellowRanger(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am the Yellow Ranger, once a mighty hero under the guidance of Zordon. How may I assist you?");

        greeting.AddOption("Tell me about your time as a Power Ranger.",
            player => true,
            player => 
            {
                DialogueModule rangerModule = new DialogueModule("Ah, those were the days! Fighting alongside my fellow Rangers was a true honor. Zordon led us with wisdom and strength. Would you like to know more about Zordon?");
                
                rangerModule.AddOption("Who is Zordon?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule zordonModule = new DialogueModule("Zordon is the wise and powerful mentor who guided us. He provided us with our powers and the knowledge to fight evil. He believed in the potential of each Ranger.");
                        
                        zordonModule.AddOption("What was it like to train with him?",
                            p => true,
                            p => 
                            {
                                DialogueModule trainingModule = new DialogueModule("Training with Zordon was intense yet rewarding. He pushed us to our limits, teaching us not just to fight, but to work as a team. Our bond grew stronger with every battle.");
                                trainingModule.AddOption("Did you have any special techniques?",
                                    plq => true,
                                    plq => 
                                    {
                                        DialogueModule techniquesModule = new DialogueModule("Absolutely! Each Ranger had unique techniques. I mastered the 'Tiger Strike'—a powerful move that could take down even the toughest foes. Would you like to hear about specific battles?");
                                        
                                        techniquesModule.AddOption("Tell me about a specific battle.",
                                            pw => true,
                                            pw => 
                                            {
                                                DialogueModule battleModule = new DialogueModule("One of our fiercest battles was against Rita Repulsa's monstrous creations. We fought valiantly to protect Angel Grove. It was a test of our strength and unity.");
                                                
                                                battleModule.AddOption("What happened in that battle?",
                                                    ple => true,
                                                    ple => 
                                                    {
                                                        DialogueModule battleDetailModule = new DialogueModule("We faced a giant monster, and it seemed unbeatable. But with Zordon's guidance, we combined our powers into the 'Power Blaster' and defeated it. The victory felt surreal.");
                                                        battleDetailModule.AddOption("Did you feel fear during that battle?",
                                                            pr => true,
                                                            pr => 
                                                            {
                                                                DialogueModule fearModule = new DialogueModule("Fear is natural in battle, but Zordon taught us to channel it into courage. We fought not just for ourselves, but for everyone we loved.");
                                                                p.SendGump(new DialogueGump(p, fearModule));
                                                            });
                                                        pl.SendGump(new DialogueGump(pl, battleDetailModule));
                                                    });
                                                p.SendGump(new DialogueGump(p, battleModule));
                                            });
                                        player.SendGump(new DialogueGump(player, techniquesModule));
                                    });
                                p.SendGump(new DialogueGump(p, trainingModule));
                            });

                        pl.SendGump(new DialogueGump(pl, zordonModule));
                    });

                player.SendGump(new DialogueGump(player, rangerModule));
            });

        greeting.AddOption("What is your job now?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("Now, I guard this wretched place, ensuring the secrets of our past remain safe. It's a duty I take very seriously.");
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What is your greatest memory as a Ranger?",
            player => true,
            player => 
            {
                DialogueModule memoryModule = new DialogueModule("My greatest memory is the first time I morphed into the Yellow Ranger. The feeling of power and responsibility was overwhelming. Would you like to hear how that moment changed me?");
                
                memoryModule.AddOption("Yes, tell me more.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule momentModule = new DialogueModule("The first time I morphed, I felt a surge of energy unlike anything before. I realized I was not just a fighter but a protector. That moment defined my path as a hero.");
                        player.SendGump(new DialogueGump(player, momentModule));
                    });

                player.SendGump(new DialogueGump(player, memoryModule));
            });

        return greeting;
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
