using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DaneTheHawkEyed : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DaneTheHawkEyed() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dane the Hawk-Eyed";
        Body = 0x190; // Human male body

        // Stats
        SetStr(105);
        SetDex(70);
        SetInt(50);
        SetHits(135);

        // Appearance
        AddItem(new LongPants(1172));
        AddItem(new HalfApron(1132));
        AddItem(new Shoes(1155));
        AddItem(new RepeatingCrossbow() { Name = "Dane's Crossbow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DaneTheHawkEyed(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hail, traveler. I am Dane the Hawk-Eyed, master archer of these lands. What brings you to my side today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Dane, renowned for my skill with the bow. I have trained in the eastern mountains, learning from the eagles and the winds themselves. What more would you like to know?");
                aboutModule.AddOption("What drives you to be an archer?",
                    p => true,
                    p =>
                    {
                        DialogueModule driveModule = new DialogueModule("The bow is my calling, a tool that keeps our lands safe from danger. My goal has always been to protect the realm and its people, striking from afar with unmatched precision.");
                        driveModule.AddOption("That is admirable. Farewell.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, driveModule));
                    });
                aboutModule.AddOption("Tell me about your arrows.",
                    p => true,
                    p =>
                    {
                        DialogueModule arrowModule = new DialogueModule("An archer is only as good as their arrows. The key to an effective arrow is the quality of its feathers. Let me share with you my knowledge of the best feathers.");
                        arrowModule.AddOption("What are the best feathers for arrows?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule featherModule = new DialogueModule("The finest arrows are fletched with feathers from majestic birds. Different birds provide different qualities. Let me elaborate.");
                                featherModule.AddOption("Tell me about eagle feathers.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule eagleModule = new DialogueModule("Eagle feathers are strong and resilient. They provide excellent stability, especially for long-range shots. Many archers prefer eagle feathers for their unmatched durability and the precision they lend to arrows.");
                                        eagleModule.AddOption("Are there any downsides to using eagle feathers?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule eagleDownsideModule = new DialogueModule("The downside is their rarity. Eagles are proud and elusive creatures, and obtaining their feathers requires either great skill or luck. Additionally, they are slightly heavier, which may not suit all archers.");
                                                eagleDownsideModule.AddOption("I understand. Tell me about other feathers.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, featherModule));
                                                    });
                                                eagleDownsideModule.AddOption("Thank you, Dane.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, eagleDownsideModule));
                                            });
                                        eagleModule.AddOption("Thank you, Dane.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, eagleModule));
                                    });
                                featherModule.AddOption("Tell me about hawk feathers.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule hawkModule = new DialogueModule("Hawk feathers are well-balanced, offering a great blend of stability and lightness. They are a favorite for archers who value precision without sacrificing speed. Hawks are nimble, and their feathers reflect this trait in flight.");
                                        hawkModule.AddOption("Are hawk feathers easy to obtain?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule hawkObtainModule = new DialogueModule("Hawk feathers are somewhat easier to obtain compared to eagle feathers, but hawks are still wary creatures. Many archers form bonds with hawks, and in return, the birds provide feathers. It requires patience and respect for nature.");
                                                hawkObtainModule.AddOption("That is fascinating. Tell me more about other feathers.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, featherModule));
                                                    });
                                                hawkObtainModule.AddOption("Thank you for the knowledge, Dane.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, hawkObtainModule));
                                            });
                                        hawkModule.AddOption("Thank you, Dane.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, hawkModule));
                                    });
                                featherModule.AddOption("Tell me about raven feathers.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule ravenModule = new DialogueModule("Raven feathers are lightweight and very flexible. They are ideal for short-range shots or quick volleys. They provide agility, making them perfect for fast-paced combat situations.");
                                        ravenModule.AddOption("Are raven feathers commonly used?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule ravenUseModule = new DialogueModule("Raven feathers are more commonly used by those who prefer speed over power. They are easier to obtain, as ravens are more plentiful than hawks or eagles. However, they do not provide the same level of stability for long-range shots.");
                                                ravenUseModule.AddOption("I see. Tell me about other types of feathers.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, featherModule));
                                                    });
                                                ravenUseModule.AddOption("Thank you, Dane.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, ravenUseModule));
                                            });
                                        ravenModule.AddOption("Thank you, Dane.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, ravenModule));
                                    });
                                featherModule.AddOption("Thank you for your insight, Dane.",
                                    plq => true,
                                    plq =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, featherModule));
                            });
                        arrowModule.AddOption("Thank you, Dane. I must be going.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, arrowModule));
                    });
                aboutModule.AddOption("That is all I wanted to know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you have a task for me?",
            player => true,
            player =>
            {
                DialogueModule taskModule = new DialogueModule("Indeed, traveler. There is a den of wolves to the west, threatening those who pass through. If you are willing to assist, clearing them out would prove your valor.");
                taskModule.AddOption("I will deal with the wolves.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You head west, seeking out the wolf den.");
                    });
                taskModule.AddOption("That sounds too dangerous for me.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, taskModule));
            });

        greeting.AddOption("What is valor to you?",
            player => true,
            player =>
            {
                DialogueModule valorModule = new DialogueModule("Valor is the courage to stand against adversity, even when fear grips your heart. It is a quality I value greatly, and one that defines true heroes.");
                valorModule.AddOption("How can I prove my valor?",
                    p => true,
                    p =>
                    {
                        DialogueModule proveModule = new DialogueModule("Prove your valor by aiding me with the wolves to the west. Return once the deed is done, and you shall be rewarded.");
                        proveModule.AddOption("I will do it.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You prepare yourself to face the wolves.");
                            });
                        proveModule.AddOption("I am not ready for this task.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, proveModule));
                    });
                valorModule.AddOption("Thank you for your wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, valorModule));
            });

        greeting.AddOption("I have dealt with the wolves.",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                    cooldownModule.AddOption("Understood. I shall return.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("You have shown true valor! As promised, here is your reward. May it serve you well on your journey.");
                    rewardModule.AddOption("Thank you, Dane.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Farewell, Dane.",
            player => true,
            player =>
            {
                player.SendMessage("Dane nods respectfully at you.");
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