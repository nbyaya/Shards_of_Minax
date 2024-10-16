using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Patch the Pirate")]
public class PatchThePirate : BaseCreature
{
    [Constructable]
    public PatchThePirate() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Patch the Pirate";
        Body = 0x190; // Human male body

        // Stats
        SetStr(45);
        SetDex(40);
        SetInt(25);
        SetHits(45);

        // Appearance
        AddItem(new LongPants() { Hue = 1904 });
        AddItem(new FancyShirt() { Hue = 1904 });
        AddItem(new Boots() { Hue = 1904 });
        AddItem(new TricorneHat() { Hue = 1904 });
        AddItem(new Cloak() { Name = "Patch's Cloak" }); // EyePatch

        Hue = Utility.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
    }

    public PatchThePirate(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ahoy there, I be Patch the Pirate, scourge of the seven seas! What brings ye to my shores?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I've taken a few scratches in me day, but I'm still sailin' strong! Always lookin' for treasure and adventure.");
                aboutModule.AddOption("What motivates ye to be a pirate?",
                    p => true,
                    p =>
                    {
                        DialogueModule motivationModule = new DialogueModule("Ye see, after the plague took me parents, I had to care for me little sister. The seas be dangerous, but piracy is the only way I can put food on the table for her.");
                        motivationModule.AddOption("How did ye survive after your parents died?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule survivalModule = new DialogueModule("It wasn't easy, matey. I took to the seas, plundering what I could to keep her fed and safe. Every coin earned be for her well-being.");
                                survivalModule.AddOption("Tell me more about your sister.",
                                    ppp => true,
                                    ppp =>
                                    {
                                        DialogueModule sisterModule = new DialogueModule("Her name be Lily. A brave lass with a heart of gold. I do me best to keep her from the dark ways of piracy, but she’s got the spirit of a true adventurer.");
                                        sisterModule.AddOption("Does she know what you do?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule honestyModule = new DialogueModule("Aye, I keep it from her as much as I can. I want her to have dreams of the horizon, not the darkness that lurks beneath it.");
                                                honestyModule.AddOption("What are her dreams?",
                                                    pppp => true,
                                                    pppp =>
                                                    {
                                                        DialogueModule dreamsModule = new DialogueModule("She dreams of exploring the world and maybe even being a healer. She wants to help those in need, unlike our past.");
                                                        dreamsModule.AddOption("That's a noble dream.",
                                                            pllll => true,
                                                            pllll =>
                                                            {
                                                                pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                            });
                                                        pppp.SendGump(new DialogueGump(pppp, dreamsModule));
                                                    });
                                                ppp.SendGump(new DialogueGump(ppp, honestyModule));
                                            });
                                        sisterModule.AddOption("I’d like to meet her someday.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendMessage("Perhaps one day, matey. But for now, I must keep her safe from this life.");
                                            });
                                        p.SendGump(new DialogueGump(p, sisterModule));
                                    });
                                motivationModule.AddOption("That sounds challenging.",
                                    ppp => true,
                                    ppp =>
                                    {
                                        ppp.SendGump(new DialogueGump(ppp, survivalModule));
                                    });
                                p.SendGump(new DialogueGump(p, motivationModule));
                            });
                        motivationModule.AddOption("Piracy can be a harsh life.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, motivationModule));
                    });
                aboutModule.AddOption("What battles have you fought?",
                    p => true,
                    p =>
                    {
                        DialogueModule battlesModule = new DialogueModule("True valor be found in the heart of a pirate! We face danger with a grin and take on all challengers!");
                        battlesModule.AddOption("Sounds thrilling!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, battlesModule));
                    });
                greeting.AddOption("Goodbye, Patch.",
                    pl => true,
                    pl => { pl.SendMessage("Fair winds to ye, matey!"); });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("I seek treasure.",
            player => true,
            player =>
            {
                DialogueModule treasureModule = new DialogueModule("Aye, treasure be callin'! Many a map be hidin' secrets of gold and glory. Are ye brave enough to seek it?");
                treasureModule.AddOption("Aye, lead the way!",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Patch nods approvingly, ready to share tales of adventure.");
                    });
                treasureModule.AddOption("Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, treasureModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Fair winds to ye, matey!");
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
