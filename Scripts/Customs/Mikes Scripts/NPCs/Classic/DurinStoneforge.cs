using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DurinStoneforge : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DurinStoneforge() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Durin Stoneforge";
        Body = 0x190; // Dwarf body
        Hue = 2406; // Default skin hue

        // Stats
        SetStr(130);
        SetDex(60);
        SetInt(50);

        SetHits(90);
        SetMana(0);
        SetStam(60);

        // Appearance
        AddItem(new RingmailLegs() { Hue = 2412 });
        AddItem(new RingmailChest() { Hue = 2412 });
        AddItem(new RingmailGloves() { Hue = 2412 });
        AddItem(new ChainCoif() { Hue = 2412 });
        AddItem(new WarHammer() { Name = "Durin's Hammer" });

        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DurinStoneforge(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Durin Stoneforge, keeper of ancient secrets. What knowledge do you seek?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a craftsman of the earth, shaping stone and metal into works of art. Many have sought my expertise in the mysteries of the earth. Through the ages, I have been a guardian of hidden truths.");
                aboutModule.AddOption("What kind of crafts do you make?",
                    p => true,
                    p =>
                    {
                        DialogueModule craftsModule = new DialogueModule("I have honed my skills for many a year, creating masterpieces that many desire. My favorite is a special pendant I crafted which is said to have mystical properties.");
                        craftsModule.AddOption("What mystical properties does it have?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule mysticalPropertiesModule = new DialogueModule("The pendant is said to provide clarity of mind and heightened perception, allowing its wearer to understand secrets hidden from most. It also grants a subtle resilience to certain types of magic.");
                                mysticalPropertiesModule.AddOption("Where did you learn to craft such items?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule learningModule = new DialogueModule("I learned my craft from the elder dwarves of the Stoneforge clan, deep within the mountain halls. They passed down their techniques from one generation to the next, along with secrets that have been guarded for centuries.");
                                        learningModule.AddOption("What kind of secrets?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule secretsModule = new DialogueModule("Secrets of ancient devices hidden deep within the earth—machines that can harness the power of the elements. These devices were created by the ancestors and are still shrouded in mystery. They require special keys and knowledge to operate.");
                                                secretsModule.AddOption("Tell me more about these ancient devices.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule devicesModule = new DialogueModule("The devices are known as the Elemental Conduits. Each conduit was designed to channel a specific element—fire, water, earth, and air. They were used to maintain balance in the natural world and to protect our people from calamities. However, most of these conduits have been lost or sealed away.");
                                                        devicesModule.AddOption("Where are the Elemental Conduits located?",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                DialogueModule locationModule = new DialogueModule("The locations of the conduits are known only to a few. The Fire Conduit is rumored to be hidden within the heart of an active volcano, while the Water Conduit is said to rest beneath an ancient lake, guarded by fierce creatures. The Earth Conduit lies deep within a collapsed mine, and the Air Conduit is believed to be atop the highest peak of the Whispering Mountains.");
                                                                locationModule.AddOption("How can I access these conduits?",
                                                                    pllllll => true,
                                                                    pllllll =>
                                                                    {
                                                                        DialogueModule accessModule = new DialogueModule("To access the conduits, you must first obtain the Elemental Keys. Each key is guarded by powerful entities that embody the essence of their respective elements. Only by proving your worth can you obtain these keys and unlock the conduits' potential.");
                                                                        accessModule.AddOption("Who guards these keys?",
                                                                            plllllll => true,
                                                                            plllllll =>
                                                                            {
                                                                                DialogueModule guardiansModule = new DialogueModule("The guardians are formidable beings: The Fire Warden, a spirit of flame dwelling within the volcano; the Water Nymph, who commands the ancient lake; the Earth Golem, who slumbers beneath the mine; and the Sky Sentinel, a swift and elusive being that soars above the highest peaks. Each one will test your resolve and your understanding of their element.");
                                                                                guardiansModule.AddOption("What happens if I obtain all the keys?",
                                                                                    pllllllll => true,
                                                                                    pllllllll =>
                                                                                    {
                                                                                        DialogueModule rewardModule = new DialogueModule("If you manage to obtain all the keys and unlock the conduits, you will gain mastery over the elemental forces. You could restore balance to the natural world or harness the power for your own ends. But beware, such power comes with great responsibility, and the elements can be fickle if not treated with respect.");
                                                                                        rewardModule.AddOption("I am ready to seek the keys.",
                                                                                            plllllllll => true,
                                                                                            plllllllll =>
                                                                                            {
                                                                                                plllllllll.SendMessage("Durin nods solemnly. 'May your journey be fruitful, and may the elements favor you.'");
                                                                                            });
                                                                                        rewardModule.AddOption("Perhaps I am not ready yet.",
                                                                                            plllllllll => true,
                                                                                            plllllllll =>
                                                                                            {
                                                                                                plllllllll.SendGump(new DialogueGump(plllllllll, CreateGreetingModule()));
                                                                                            });
                                                                                        pllllllll.SendGump(new DialogueGump(pllllllll, rewardModule));
                                                                                    });
                                                                                guardiansModule.AddOption("That sounds too dangerous.",
                                                                                    pllllllll => true,
                                                                                    pllllllll =>
                                                                                    {
                                                                                        pllllllll.SendGump(new DialogueGump(pllllllll, CreateGreetingModule()));
                                                                                    });
                                                                                plllllll.SendGump(new DialogueGump(plllllll, guardiansModule));
                                                                            });
                                                                        accessModule.AddOption("That sounds like a daunting task.",
                                                                            plllllll => true,
                                                                            plllllll =>
                                                                            {
                                                                                plllllll.SendGump(new DialogueGump(plllllll, CreateGreetingModule()));
                                                                            });
                                                                        pllllll.SendGump(new DialogueGump(pllllll, accessModule));
                                                                    });
                                                                locationModule.AddOption("Thank you for the information.",
                                                                    plllllq => true,
                                                                    plllllq =>
                                                                    {
                                                                        plllll.SendGump(new DialogueGump(plllll, CreateGreetingModule()));
                                                                    });
                                                                plllll.SendGump(new DialogueGump(plllll, locationModule));
                                                            });
                                                        devicesModule.AddOption("Why were the conduits created?",
                                                            pllllw => true,
                                                            pllllw =>
                                                            {
                                                                DialogueModule purposeModule = new DialogueModule("The conduits were created by our ancestors to maintain harmony between the elements. They realized that without balance, the elements could rage unchecked, causing untold destruction. The conduits were a way to ensure that the natural world remained in equilibrium.");
                                                                purposeModule.AddOption("That's incredible.",
                                                                    plllll => true,
                                                                    plllll =>
                                                                    {
                                                                        plllll.SendGump(new DialogueGump(plllll, CreateGreetingModule()));
                                                                    });
                                                                pllll.SendGump(new DialogueGump(pllll, purposeModule));
                                                            });
                                                        devicesModule.AddOption("I must learn more.",
                                                            plllle => true,
                                                            plllle =>
                                                            {
                                                                pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                            });
                                                        pllll.SendGump(new DialogueGump(pllll, devicesModule));
                                                    });
                                                secretsModule.AddOption("Thank you for sharing.",
                                                    plllr => true,
                                                    plllr =>
                                                    {
                                                        plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, secretsModule));
                                            });
                                        learningModule.AddOption("Thank you for the story.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, learningModule));
                                    });
                                mysticalPropertiesModule.AddOption("Thank you for explaining.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, mysticalPropertiesModule));
                            });
                        craftsModule.AddOption("That sounds fascinating.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, craftsModule));
                    });
                aboutModule.AddOption("Thank you for sharing your story.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you have any wisdom to share?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("Wisdom is the light that banishes the shadows of ignorance. I have spent lifetimes gathering wisdom from both the living and the dead. Do you seek knowledge of the earth or something more arcane?");
                wisdomModule.AddOption("Tell me about the shadows.",
                    p => true,
                    p =>
                    {
                        DialogueModule shadowsModule = new DialogueModule("Shadows are more than just a lack of light. They are the embodiment of the unknown, the unexplored, and the forgotten. Many fear them, but they can also be a source of power for those who understand them.");
                        shadowsModule.AddOption("That sounds mysterious.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, shadowsModule));
                    });
                wisdomModule.AddOption("What wisdom do you have about the mountains?",
                    p => true,
                    p =>
                    {
                        DialogueModule mountainsModule = new DialogueModule("The mountains are not just my strength, but they are also my home. Their towering peaks and deep caverns hold many secrets. They provide resilience and wisdom to those willing to listen.");
                        mountainsModule.AddOption("Thank you for sharing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, mountainsModule));
                    });
                wisdomModule.AddOption("Thank you for your wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Do you have a challenge for me?",
            player => true,
            player =>
            {
                DialogueModule riddleModule = new DialogueModule("Then prove your worth. Answer me this riddle: 'I am not alive, but I can grow; I don't have lungs, but I need air; I don't have a mouth, but water kills me. What am I?'");
                riddleModule.AddOption("The answer is fire.",
                    p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    p =>
                    {
                        p.SendMessage("Ah, you have proven your wits. For your cleverness, I shall bestow upon you a gift.");
                        p.AddToBackpack(new ChestSlotChangeDeed());
                        lastRewardTime = DateTime.UtcNow;
                    });
                riddleModule.AddOption("I don't know the answer.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, riddleModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Durin nods at you, his eyes filled with ancient wisdom.");
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