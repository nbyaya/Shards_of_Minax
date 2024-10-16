using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Sly Sylvi")]
public class SlySylvi : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SlySylvi() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sly Sylvi";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(100);
        SetInt(100);
        SetHits(80);

        // Appearance
        AddItem(new Cloak() { Hue = 1109 });
        AddItem(new LeatherSkirt() { Hue = 1109 });
        AddItem(new LeatherBustierArms() { Hue = 1109 });
        AddItem(new Sandals() { Hue = 1109 });
        AddItem(new LeatherGloves() { Name = "Sylvi's Sneaky Gloves" });
        AddItem(new Dagger() { Name = "Sylvi's Stiletto" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public SlySylvi(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Sly Sylvi, the rogue of these parts! But enough about me; have you heard about the goblins hoarding pies?");

        greeting.AddOption("Goblins hoarding pies? Tell me more!",
            player => true,
            player =>
            {
                DialogueModule goblinModule = new DialogueModule("Aye! They’ve taken to baking like no tomorrow. I’ve heard they’ve got two dozen pies, each more delicious than the last! But there’s a catch...");
                goblinModule.AddOption("What’s the catch?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule catchModule = new DialogueModule("The goblins aren’t known for their hospitality. To get those pies, you’ll have to be stealthy, or else you’ll end up as their next meal!");
                        catchModule.AddOption("How do I steal the pies?",
                            p => true,
                            p =>
                            {
                                DialogueModule stealModule = new DialogueModule("First, you must approach their camp under the cover of night. Wait for the right moment when they’re distracted. They love to sing and dance around the fire!");
                                stealModule.AddOption("Sounds fun! What next?",
                                    plr => true,
                                    plr =>
                                    {
                                        DialogueModule nextModule = new DialogueModule("Once you’re in, grab as many pies as you can. But remember, there are rumors that some pies are filled with... questionable meats.");
                                        nextModule.AddOption("Questionable meats? What do you mean?",
                                            pla => true,
                                            pla =>
                                            {
                                                pla.SendGump(new DialogueGump(pla, new DialogueModule("I mean, goblin cuisine isn’t exactly gourmet. I once heard of someone biting into a pie only to find it stuffed with rat!")));
                                            });
                                        stealModule.AddOption("I’m brave enough for a little risk!",
                                            pla => true,
                                            pla =>
                                            {
                                                DialogueModule braveModule = new DialogueModule("Good spirit! Just make sure to keep your wits about you. Those goblins can be surprisingly clever.");
                                                braveModule.AddOption("What if they catch me?",
                                                    plt => true,
                                                    plt =>
                                                    {
                                                        DialogueModule caughtModule = new DialogueModule("If they catch you, you’d best have a good excuse or be prepared to run fast. They’re not fond of intruders.");
                                                        caughtModule.AddOption("I can handle myself!",
                                                            py => true,
                                                            py =>
                                                            {
                                                                p.SendGump(new DialogueGump(p, new DialogueModule("Then off you go! Steal those pies and report back to me with your findings!")));
                                                            });
                                                        braveModule.AddOption("Maybe I should think this through...",
                                                            pu => true,
                                                            pu =>
                                                            {
                                                                p.SendGump(new DialogueGump(p, new DialogueModule("Smart thinking! Just don’t take too long; those pies won’t last forever!")));
                                                            });
                                                        pl.SendGump(new DialogueGump(pl, caughtModule));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, braveModule));
                                            });
                                        p.SendGump(new DialogueGump(p, nextModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, stealModule));
                            });
                        pl.SendGump(new DialogueGump(pl, catchModule));
                    });
                player.SendGump(new DialogueGump(player, goblinModule));
            });

        greeting.AddOption("What happens if I actually eat one?",
            player => true,
            player =>
            {
                DialogueModule eatModule = new DialogueModule("Ah, that’s where things can get interesting! You might find yourself with a new taste for adventure... or regret.");
                eatModule.AddOption("What do you mean by 'regret'?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule regretModule = new DialogueModule("Well, not all goblin pies are made for human consumption. Some might contain... less desirable ingredients.");
                        regretModule.AddOption("Like what?",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, new DialogueModule("Let’s just say you might not want to know! There’s a tale of a traveler who ate one and spent the next week regretting it!")));
                            });
                        eatModule.AddOption("Yikes! I’ll be careful then.",
                            pli => true,
                            pli =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Good choice! Just grab the pies and leave the tasting to the goblins.")));
                            });
                        player.SendGump(new DialogueGump(player, regretModule));
                    });
                player.SendGump(new DialogueGump(player, eatModule));
            });

        greeting.AddOption("Any tips for avoiding goblin traps?",
            player => true,
            player =>
            {
                DialogueModule tipsModule = new DialogueModule("Ah, yes! Goblins love their traps. Look for suspiciously placed sticks or piles of leaves. They’re not subtle.");
                tipsModule.AddOption("What if I trigger one?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule triggerModule = new DialogueModule("If you trip one, be ready to dodge! Most traps are designed to capture rather than kill, but you never know.");
                        triggerModule.AddOption("Thanks for the warning!",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, new DialogueModule("Just remember to keep your eyes peeled and your senses sharp!")));
                            });
                        player.SendGump(new DialogueGump(player, triggerModule));
                    });
                player.SendGump(new DialogueGump(player, tipsModule));
            });

        greeting.AddOption("What will you do with the pies if I succeed?",
            player => true,
            player =>
            {
                DialogueModule futureModule = new DialogueModule("I have a few tricks up my sleeve! I might just host a pie party, or use them as bait for my next scheme. Who knows?");
                futureModule.AddOption("Count me in for the party!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Excellent! The more, the merrier! Just be careful of what you eat!")));
                    });
                player.SendGump(new DialogueGump(player, futureModule));
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
