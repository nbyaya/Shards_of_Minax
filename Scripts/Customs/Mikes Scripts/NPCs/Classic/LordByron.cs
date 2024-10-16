using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lord Byron")]
public class LordByron : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LordByron() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lord Byron";
        Body = 0x190; // Human male body

        // Stats
        SetStr(40);
        SetDex(80);
        SetInt(80);
        SetHits(60);

        // Appearance
        AddItem(new LongPants() { Hue = 1908 });
        AddItem(new FancyShirt() { Hue = 1908 });
        AddItem(new Boots() { Hue = 1908 });
        AddItem(new Cloak() { Hue = 1908 });
        AddItem(new FeatheredHat() { Hue = 1908 });
        AddItem(new GoldRing() { Name = "Lord Byron's Signet Ring" });

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
        DialogueModule greeting = new DialogueModule("I am Lord Byron, a knight in service of this wretched kingdom. How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => {
                DialogueModule healthModule = new DialogueModule("My health is as miserable as my station in life. Every day is a struggle. Why do you ask?");
                healthModule.AddOption("Just curious.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, healthModule)));
                healthModule.AddOption("You should take care of yourself.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("I appreciate your concern. It is rare to find someone who cares. However, my duty keeps me bound."))));
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => {
                DialogueModule jobModule = new DialogueModule("My job, if you can call it that, is to serve a kingdom that cares not for honor or valor. I feel like a relic of a bygone age.");
                jobModule.AddOption("What do you mean by 'bygone age'?",
                    p => true,
                    p => {
                        DialogueModule ageModule = new DialogueModule("In my youth, knights were revered for their valor and integrity. Now, many have forsaken these ideals for power.");
                        ageModule.AddOption("That sounds disheartening.",
                            pl => true,
                            pl => pl.SendGump(new DialogueGump(pl, ageModule)));
                        ageModule.AddOption("You should be proud of your values.",
                            pl => true,
                            pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Thank you, kind traveler. It gives me strength to hear such words."))));
                        p.SendGump(new DialogueGump(p, ageModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think of battles?",
            player => true,
            player => {
                DialogueModule battlesModule = new DialogueModule("Do you fancy yourself a knight, valiant one?");
                battlesModule.AddOption("Yes, I do.",
                    p => true,
                    p => {
                        DialogueModule yesModule = new DialogueModule("If you dare call yourself valiant, then heed my advice: never flee, unless the need is dire.");
                        yesModule.AddOption("What about strategy in battles?",
                            pl => true,
                            pl => {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Strategy is vital. Know your enemy, choose your ground, and fight not just with might, but with wisdom.")));
                            });
                        yesModule.AddOption("What if I fear for my life?",
                            pl => true,
                            pl => {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Fear is natural, but remember, a true knight must face fear head-on. It is part of our duty.")));
                            });
                        p.SendGump(new DialogueGump(p, yesModule));
                    });
                battlesModule.AddOption("No, I'm just here for information.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Then perhaps you should listen closely, for knowledge is a weapon too."))));
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("What is a knight's duty?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("I took an oath to protect this kingdom, but the taint of corruption has made my role a heavy burden to bear.")));
            });

        greeting.AddOption("What do you think of honor?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("Honor used to be the lifeblood of knights, but now, many have turned their backs on it for power and greed.")));
            });

        greeting.AddOption("What is true valor?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, new DialogueModule("True valor is not just in facing battles, but in standing up for what's right, even when the whole world turns against you.")));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player => {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
                    player.AddToBackpack(new MaxxiaScroll());
                    lastRewardTime = DateTime.UtcNow;
                    player.SendGump(new DialogueGump(player, new DialogueModule("Greed has taken many of my fellow knights, but for your deeds, I wish to reward you. Please, accept this token of appreciation.")));
                }
            });

        greeting.AddOption("Tell me about your past.",
            player => true,
            player => {
                DialogueModule pastModule = new DialogueModule("Ah, my past is filled with both glory and regret. I was once a celebrated knight, honored for my bravery.");
                pastModule.AddOption("What glory did you achieve?",
                    pl => true,
                    pl => {
                        DialogueModule gloryModule = new DialogueModule("I fought in many battles, and I was once part of a noble order that protected the realm. But alas, that is a story for another time.");
                        gloryModule.AddOption("I want to hear it now!",
                            p => true,
                            p => {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Very well. We faced a great evil, a sorcerer who threatened our land. It was a long and arduous battle, but we emerged victorious. However, the cost was high.")));
                            });
                        pl.SendGump(new DialogueGump(pl, gloryModule));
                    });
                player.SendGump(new DialogueGump(player, pastModule));
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

    public LordByron(Serial serial) : base(serial) { }
}
