using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EnchanterGalen : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EnchanterGalen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Enchanter Galen";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(75);
        SetInt(90);
        SetHits(100);

        // Appearance
        AddItem(new Robe() { Hue = 1153 });
        AddItem(new Shoes() { Hue = 1153 });
        AddItem(new FloppyHat() { Hue = 1153 });
        AddItem(new LeatherGloves() { Hue = 1153 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Enchanter Galen. How can I assist you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Enchanter Galen, a practitioner of the ancient arts of magic, skilled in enchantments and the mysteries of the arcane. I come from a long lineage of powerful enchanters.");
                aboutModule.AddOption("Tell me about your lineage.",
                    p => true,
                    p =>
                    {
                        DialogueModule lineageModule = new DialogueModule("Our lineage dates back to the First Age, where magic was as natural as breathing. Many secrets have been passed down through generations. Are you seeking any particular knowledge?");
                        lineageModule.AddOption("What kind of secrets?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule secretsModule = new DialogueModule("The secrets include spells, rituals, and a deep understanding of the energies that shape our world. These are passed from master to apprentice, never written, always preserved by memory.");
                                secretsModule.AddOption("That sounds fascinating.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                secretsModule.AddOption("What energies do you speak of?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule energyModule = new DialogueModule("The energies I speak of are the very forces that bind the realms together. Lately, I've felt an unsettling disturbance—a shift in these energies, perhaps related to an incursion from the Far Realm.");
                                        energyModule.AddOption("What is the Far Realm?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule farRealmModule = new DialogueModule("The Far Realm is a higher-dimensional plane, a place of chaotic, alien existence beyond our understanding. It's inhabited by beings whose very essence defies the laws of our universe. These incursions threaten to destabilize our world, and only those with true magical understanding can resist them.");
                                                farRealmModule.AddOption("How can I help resist the incursion?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("The incursion is growing stronger. There are rifts appearing across the land where Far Realm creatures break through. To resist them, we need to close these rifts. I could teach you a ritual to do so, but it requires rare components.");
                                                        helpModule.AddOption("What components are required?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule componentsModule = new DialogueModule("To perform the ritual, you will need the Essence of a Fallen Star, an Elderwood Branch, and Crystalline Dust from the Crystal Caverns. These items are rare and guarded by fearsome creatures, but without them, we cannot hope to close the rifts.");
                                                                componentsModule.AddOption("Where can I find these items?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule locationsModule = new DialogueModule("The Essence of a Fallen Star can sometimes be found where meteors have struck the land, though collecting it is no simple task. The Elderwood Branch is said to be held by an ancient treant deep in the Whispering Woods. The Crystalline Dust can be harvested from the Crystal Caverns, but beware—the caverns are infested with creatures influenced by the Far Realm's energies.");
                                                                        locationsModule.AddOption("I'll gather the components.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendMessage("You set off on a perilous journey to gather the rare components.");
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        locationsModule.AddOption("That sounds too dangerous for me.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, locationsModule));
                                                                    });
                                                                componentsModule.AddOption("This task is beyond me.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, componentsModule));
                                                            });
                                                        helpModule.AddOption("I have other matters to attend to.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, helpModule));
                                                    });
                                                farRealmModule.AddOption("What kind of creatures come from the Far Realm?",
                                                    plbq => true,
                                                    plbq =>
                                                    {
                                                        DialogueModule creaturesModule = new DialogueModule("The creatures from the Far Realm are like nightmares brought to life. They are twisted, multi-limbed beings that defy the normal rules of form and function. They induce madness merely by their presence, and their touch corrupts everything it comes into contact with.");
                                                        creaturesModule.AddOption("How can one fight such creatures?",
                                                            plc => true,
                                                            plc =>
                                                            {
                                                                DialogueModule fightModule = new DialogueModule("To fight them, one must have a strong mind and a mastery of protective magic. Weapons forged with enchanted silver are known to be effective, and certain wards can protect against their influence. But above all, bravery and resilience are required.");
                                                                fightModule.AddOption("Thank you for the advice.",
                                                                    pld => true,
                                                                    pld =>
                                                                    {
                                                                        pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                                    });
                                                                plc.SendGump(new DialogueGump(plc, fightModule));
                                                            });
                                                        creaturesModule.AddOption("I fear I am not ready to face such horrors.",
                                                            plc => true,
                                                            plc =>
                                                            {
                                                                plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                            });
                                                        plb.SendGump(new DialogueGump(plb, creaturesModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, farRealmModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, energyModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, secretsModule));
                            });
                        p.SendGump(new DialogueGump(p, lineageModule));
                    });
                aboutModule.AddOption("Thank you for the introduction.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Can you tell me about magic?",
            player => true,
            player =>
            {
                DialogueModule magicModule = new DialogueModule("Magic, the manifestation of virtue, can both heal and harm. Are you intrigued by the mysteries of magic?");
                magicModule.AddOption("What are the mysteries of magic?",
                    p => true,
                    p =>
                    {
                        if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        }
                        else
                        {
                            DialogueModule mysteriesModule = new DialogueModule("The mysteries of magic are vast and deep. Only through dedication and study can one truly grasp its essence. To aid you on your journey, I would like to bestow upon you a token of magic. May it guide you well.");
                            mysteriesModule.AddOption("Thank you for the token.",
                                pl => true,
                                pl =>
                                {
                                    pl.AddToBackpack(new FletchingAugmentCrystal()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            mysteriesModule.AddOption("What else should I know about the Far Realm?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule farRealmInfoModule = new DialogueModule("The Far Realm's influence is spreading. Even those who do not see the rifts may feel its effects—strange dreams, a sense of dread, and inexplicable events. The world is being slowly twisted, and we must be vigilant.");
                                    farRealmInfoModule.AddOption("How can I protect myself from these effects?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule protectModule = new DialogueModule("Protection requires both mental and physical defenses. Mental fortitude can be built through meditation, while physical defenses may include enchanted talismans and wards. I can teach you a simple warding spell to keep the influence at bay.");
                                            protectModule.AddOption("Teach me the warding spell.",
                                                ple => true,
                                                ple =>
                                                {
                                                    ple.SendMessage("Galen teaches you a basic warding spell to protect yourself from the influence of the Far Realm.");
                                                    ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                });
                                            protectModule.AddOption("Thank you, but I will seek protection elsewhere.",
                                                ple => true,
                                                ple =>
                                                {
                                                    ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                });
                                            pla.SendGump(new DialogueGump(pla, protectModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, farRealmInfoModule));
                                });
                            p.SendGump(new DialogueGump(p, mysteriesModule));
                        }
                    });
                magicModule.AddOption("How can magic reflect virtues?",
                    p => true,
                    p =>
                    {
                        DialogueModule virtueModule = new DialogueModule("Indeed, magic is a reflection of the virtues. It can be used for good or ill. What path will you choose? The path of magic is intertwined with the choices we make. Some seek power, others wisdom, and some balance.");
                        virtueModule.AddOption("I seek balance.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, virtueModule));
                    });
                player.SendGump(new DialogueGump(player, magicModule));
            });

        greeting.AddOption("What is it like being an enchanter?",
            player => true,
            player =>
            {
                DialogueModule enchanterModule = new DialogueModule("Being an enchanter is more than casting spells. It is about understanding the fabric of the world and weaving your will into it. Have you ever felt the call to practice enchantments?");
                enchanterModule.AddOption("Yes, I have felt the call.",
                    p => true,
                    p =>
                    {
                        DialogueModule callModule = new DialogueModule("Then you are on the right path, traveler. To practice enchantments is to truly connect with the energies of the world.");
                        callModule.AddOption("Tell me more about the energies.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule energiesModule = new DialogueModule("The energies are all around us. They flow between realms, connecting the seen and unseen. However, the incursion from the Far Realm threatens to corrupt these energies. We must learn to manipulate them to restore balance.");
                                energiesModule.AddOption("How can I manipulate these energies?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule manipulateModule = new DialogueModule("To manipulate these energies, you must first attune yourself to them. Meditation and focus are key. Once attuned, you can direct these energies through rituals to counter the corruption and heal the balance of our realm.");
                                        manipulateModule.AddOption("I am ready to learn.",
                                            ple => true,
                                            ple =>
                                            {
                                                ple.SendMessage("Galen begins teaching you the basics of energy attunement.");
                                                ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                            });
                                        manipulateModule.AddOption("I need more time to prepare.",
                                            ple => true,
                                            ple =>
                                            {
                                                ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, manipulateModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, energiesModule));
                            });
                        callModule.AddOption("Thank you for your wisdom.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, callModule));
                    });
                enchanterModule.AddOption("No, it does not appeal to me.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, enchanterModule));
            });

        return greeting;
    }

    public EnchanterGalen(Serial serial) : base(serial) { }

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