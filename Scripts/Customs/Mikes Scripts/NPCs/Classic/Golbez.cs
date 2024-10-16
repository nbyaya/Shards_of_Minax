using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class Golbez : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Golbez() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Golbez";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(100);
        SetInt(100);
        SetHits(100);

        // Appearance
        AddItem(new Robe() { Hue = 1109 }); // Robe with hue 1109
        AddItem(new Cloak() { Hue = 1109 }); // Cloak with hue 1109
        AddItem(new Sandals() { Hue = 1109 }); // Sandals with hue 1109
        AddItem(new SkullCap() { Hue = 1109 }); // SkullCap with hue 1109
        AddItem(new Spellbook() { Name = "Golbez's Grimoire" });

        Hue = Utility.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
    }

    public Golbez(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Golbez, a seeker of power! What brings you to me, traveler?");

        greeting.AddOption("What is your quest?",
            player => true,
            player =>
            {
                DialogueModule questModule = new DialogueModule("I seek the elemental crystals to awaken the legendary Babel Giant. Only with its power can I control the very elements. Do you wish to aid me in this noble endeavor?");
                questModule.AddOption("How can I help you?",
                    p => true,
                    p =>
                    {
                        DialogueModule helpModule = new DialogueModule("I need you to find the four elemental crystals: Earth, Water, Fire, and Air. Each is guarded by powerful beings and hidden in dangerous locations. Are you prepared for such a quest?");
                        helpModule.AddOption("I'm ready for the challenge.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Excellent! Let us begin by discussing the Earth Crystal. It is hidden deep within the Verdant Caverns, guarded by a fierce Earth Elemental. Do you wish to know more?")));
                            });
                        helpModule.AddOption("Tell me more about the crystals.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule crystalsModule = new DialogueModule("The crystals embody the raw power of their elements. Each crystal can grant immense strength to the one who possesses it. Would you like to know about a specific crystal?");
                                crystalsModule.AddOption("What about the Earth Crystal?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule earthModule = new DialogueModule("The Earth Crystal lies in the Verdant Caverns, protected by the Earth Elemental, a creature of immense strength. To defeat it, you must understand the earth's will. Are you willing to take on this challenge?");
                                        earthModule.AddOption("Yes, I will seek the Earth Crystal.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You set off towards the Verdant Caverns to seek the Earth Crystal.");
                                            });
                                        earthModule.AddOption("What if I fail?",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, new DialogueModule("Failure is merely a stepping stone to success. Learn from your mistakes, and you will grow stronger.")));
                                            });
                                        pla.SendGump(new DialogueGump(pla, earthModule));
                                    });
                                crystalsModule.AddOption("What about the Water Crystal?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule waterModule = new DialogueModule("The Water Crystal is hidden beneath the Serpent's Bay, guarded by the Water Guardian, a serpentine creature of immense power. It tests both courage and wisdom. Will you seek it?");
                                        waterModule.AddOption("Yes, I will brave the waters.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You prepare to dive into the depths of Serpent's Bay for the Water Crystal.");
                                            });
                                        waterModule.AddOption("I fear the water.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, new DialogueModule("Fear can be a powerful motivator. Use it to guide your resolve and find courage.")));
                                            });
                                        pla.SendGump(new DialogueGump(pla, waterModule));
                                    });
                                crystalsModule.AddOption("Tell me about the Fire Crystal.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule fireModule = new DialogueModule("The Fire Crystal resides in the Molten Peaks, guarded by the Fire Drake, a creature of flames and fury. You will need to be quick and clever to claim this crystal. Will you accept this quest?");
                                        fireModule.AddOption("I will challenge the Fire Drake!",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("With determination, you set your sights on the Molten Peaks for the Fire Crystal.");
                                            });
                                        fireModule.AddOption("That sounds too dangerous!",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, new DialogueModule("Caution is wise, but do not let it paralyze you. Adventure awaits those who dare.")));
                                            });
                                        pla.SendGump(new DialogueGump(pla, fireModule));
                                    });
                                crystalsModule.AddOption("What about the Air Crystal?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule airModule = new DialogueModule("The Air Crystal is held high atop the Stormspire, guarded by the Wind Elemental. Only those who can master the winds can claim it. Are you ready to ascend?");
                                        airModule.AddOption("I will ascend the Stormspire.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You gather your resolve and begin your ascent up the Stormspire.");
                                            });
                                        airModule.AddOption("I'm not skilled with heights.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, new DialogueModule("Overcoming fear is part of the journey. You must find the courage within.")));
                                            });
                                        pla.SendGump(new DialogueGump(pla, airModule));
                                    });
                                player.SendGump(new DialogueGump(player, crystalsModule));
                            });
                        p.SendGump(new DialogueGump(p, helpModule));
                    });
                questModule.AddOption("What happens if we awaken the Babel Giant?",
                    p => true,
                    p =>
                    {
                        DialogueModule babelModule = new DialogueModule("The Babel Giant is said to possess the ability to control the very forces of nature. With its power, I could reshape the world, balance the elements, and perhaps even usher in a new era of understanding.");
                        babelModule.AddOption("That sounds powerful. Will you control it?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Control is a delicate thing. I seek to understand and ally with its power, not to dominate it.")));
                            });
                        babelModule.AddOption("Will it be dangerous?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("With great power comes great responsibility. The consequences of awakening the Giant are unknown, but I must take this risk.")));
                            });
                        player.SendGump(new DialogueGump(player, babelModule));
                    });

                player.SendGump(new DialogueGump(player, questModule));
            });

        greeting.AddOption("What do you know about the Ritual of Ascension?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("The Ritual of Ascension is a path to enlightenment. It requires great understanding and sacrifice. Seek the Tome of the Ancients for further knowledge.")));
            });

        greeting.AddOption("What about the Tome of the Ancients?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("The legendary Tome is said to hold the secrets of true power. Many have searched for it, few have succeeded. It may even contain the location of the elemental crystals.")));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Remember, the equilibrium between light and shadow is crucial. Farewell, traveler.")));
            });

        return greeting;
    }

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
