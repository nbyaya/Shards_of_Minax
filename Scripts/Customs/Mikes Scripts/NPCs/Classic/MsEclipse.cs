using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Ms. Eclipse")]
public class MsEclipse : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MsEclipse() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ms. Eclipse";
        Body = 0x191; // Human female body

        // Stats
        SetStr(70);
        SetDex(70);
        SetInt(90);
        SetHits(40);

        // Appearance
        AddItem(new Robe() { Hue = 1150 });
        AddItem(new Cap() { Hue = 1150 });
        AddItem(new LeatherGloves() { Hue = 1150 });
        AddItem(new Shoes() { Hue = 1150 });
        AddItem(new Item(0x1F14) { Name = "Ms. Eclipse's Chip" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("I am Ms. Eclipse, the brilliant scientist! What do you wish to discuss?");
        
        greeting.AddOption("Tell me about your health.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("My health? Irrelevant! I'm busy with important work! What more could a mere mortal like you understand?")));
            });
        
        greeting.AddOption("What is your job?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("My job? I'm a scientist! But alas, you seem rather unversed in the matters of the arcane and the scientific.")));
            });

        greeting.AddOption("What research are you doing?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("My research delves into the enigmatic effects of lunar eclipses on arcane energies. Fascinating, isn't it?")));
            });

        greeting.AddOption("Why are you so brilliant?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Ah, 'brilliant'? Thank you! I have sacrificed much for my knowledge. Countless nights spent studying ancient tomes and performing experiments!")));
            });

        greeting.AddOption("What do you need help with?",
            player => true,
            player => {
                DialogueModule helpModule = new DialogueModule("I am currently in need of rare ingredients for my experiments. If you have the courage, you might assist me.");
                helpModule.AddOption("What ingredients do you need?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I require the Essence of Nightshade, found only in the darkest parts of the forest, and a vial of Starfall Dew, collected during a meteor shower. Will you fetch them for me?")));
                    });
                helpModule.AddOption("That sounds dangerous; I can't help.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well, perhaps another brave soul will step up. Time is of the essence!")));
                    });
                player.SendGump(new DialogueGump(player, helpModule));
            });

        greeting.AddOption("What about arcane energies?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("These 'arcane energies' are powerful forces! When harnessed, they can grant abilities beyond imagination, but they can be volatile and unpredictable.")));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Ah, a confident one, aren't we? Very well, for your earnestness, I shall reward you. Take this, and use it wisely!")));
                player.AddToBackpack(new PhysicalHitAreaCrystal()); // Replace with actual reward item class
                lastRewardTime = DateTime.UtcNow;
            });

        greeting.AddOption("Tell me about lunar eclipses.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Lunar eclipses are not merely celestial events! They have profound effects on magical energies. During an eclipse, the very fabric of reality shifts. You must learn to observe closely!")));
            });

        greeting.AddOption("What do you know about the cosmos?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("The cosmos is a vast tapestry of stars and mysteries. Each star is a beacon of ancient knowledge, and the universe has secrets waiting to be uncovered!")));
            });

        greeting.AddOption("What do you think of mortals?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Mortals are amusing yet limited. You possess potential, but it is often squandered on trivial pursuits. Dare to reach for the stars!")));
            });

        greeting.AddOption("Can you teach me about science?",
            player => true,
            player => {
                DialogueModule teachModule = new DialogueModule("Ah, a seeker of knowledge! Science is a method of understanding the universe through observation and experimentation. What would you like to learn?");
                teachModule.AddOption("How do I conduct experiments?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("To conduct an experiment, formulate a hypothesis, gather materials, and document your findings. Never forget to control your variables!")));
                    });
                teachModule.AddOption("What is the importance of observation?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Observation is the cornerstone of science! Without careful observation, we are left in the dark, unable to decipher the mysteries around us.")));
                    });
                teachModule.AddOption("Perhaps I’ll study another time.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well, knowledge is not to be rushed. Return when you are ready to learn!")));
                    });
                player.SendGump(new DialogueGump(player, teachModule));
            });

        greeting.AddOption("What do you think of nature?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Nature is a marvelous, yet often chaotic force. It holds the key to many mysteries that science strives to unravel. Do you appreciate it?")));
            });

        greeting.AddOption("Do you have any predictions?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Predictions are difficult! But I foresee a time when the boundaries of reality will be challenged, and the true nature of the cosmos revealed!")));
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

    public MsEclipse(Serial serial) : base(serial) { }
}
