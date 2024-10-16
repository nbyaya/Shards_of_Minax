using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DandyDan : BaseCreature
{
    [Constructable]
    public DandyDan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dandy Dan";
        Body = 0x190; // Human male body

        // Stats
        SetStr(85);
        SetDex(90);
        SetInt(80);
        SetHits(85);

        // Appearance
        AddItem(new FancyShirt(1153)); // Fancy shirt with hue 1153
        AddItem(new LongPants(1153)); // Long pants with hue 1153
        AddItem(new Boots(1153)); // Boots with hue 1153
        AddItem(new FeatheredHat(1153)); // Feathered hat with hue 1153
        AddItem(new GoldRing { Name = "Dan's Dazzling Ring" });
        AddItem(new Longsword { Name = "Dan's Dashing Rapier" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue
    }

    public DandyDan(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Dandy Dan, the rogue. What brings you my way?");

        greeting.AddOption("Tell me about your amazing luck.",
            player => true,
            player =>
            {
                DialogueModule luckModule = new DialogueModule("Ah, my luck is legendary, or so they say. From finding gold coins in the most unlikely places to escaping certain doom by mere inches, luck has always been on my side.");
                luckModule.AddOption("How did you first discover your luck?",
                    p => true,
                    p =>
                    {
                        DialogueModule firstLuckModule = new DialogueModule("It all started when I was just a lad. I remember finding a rare gemstone in a pile of pebbles by the river. Everyone said it was impossible, but there it was, shining brightly amidst the mundane. From that day on, strange and fortunate things seemed to follow me.");
                        firstLuckModule.AddOption("What happened after you found the gemstone?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule afterGemstoneModule = new DialogueModule("Well, a traveling merchant offered me a small fortune for it, and I used the gold to buy my first set of adventuring gear. That very gear saved my life countless times. It was as if fate itself had taken a liking to me.");
                                afterGemstoneModule.AddOption("Did you have more lucky encounters after that?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule moreLuckModule = new DialogueModule("Oh, absolutely! There was the time I accidentally stumbled into a dragon's lair. Just as it was about to roast me alive, a group of knights burst in, completely unaware I was even there. They fought the beast, and I slipped away with some of the treasure. Talk about timing!");
                                        moreLuckModule.AddOption("What did you do with the dragon's treasure?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule treasureUseModule = new DialogueModule("I used it to travel far and wide, of course! I bought passage on ships, paid for guides through treacherous mountains, and even bribed a few guards when things got too hot. You could say luck and I have always been the best of friends.");
                                                treasureUseModule.AddOption("Tell me about another lucky adventure.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule anotherLuckModule = new DialogueModule("Ah, there was that time I found myself in a goblin-infested forest. I was outnumbered, unarmed, and frankly terrified. Just when I thought it was over, a massive thunderstorm rolled in, and a lightning strike scared the goblins off! I walked away without a scratch.");
                                                        anotherLuckModule.AddOption("You must be charmed, Dan!",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule charmedModule = new DialogueModule("Haha! Many have said that. Perhaps I am. Some even say I was blessed by a wandering druid when I was a baby. Who knows? All I know is that I'm still here, and life keeps throwing me wonderful surprises.");
                                                                charmedModule.AddOption("Do you think your luck will ever run out?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule luckEndModule = new DialogueModule("Ah, that's the question, isn't it? Every gambler knows that luck can be fickle. But until the day it does run out, I'll keep living life to the fullest. And even then, well, I like to think I've got enough wit to get by without it.");
                                                                        luckEndModule.AddOption("A wise outlook, Dan.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, luckEndModule));
                                                                    });
                                                                charmedModule.AddOption("May your luck never run out, my friend.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, charmedModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, anotherLuckModule));
                                                    });
                                                treasureUseModule.AddOption("That's an incredible story.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, treasureUseModule));
                                            });
                                        moreLuckModule.AddOption("You've had quite the life, Dan.",
                                            plc => true,
                                            plc =>
                                            {
                                                plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, moreLuckModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, afterGemstoneModule));
                            });
                        firstLuckModule.AddOption("It sounds like fate has always been on your side.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, firstLuckModule));
                    });
                luckModule.AddOption("Do you think it's all just luck, or is there something more?",
                    p => true,
                    p =>
                    {
                        DialogueModule moreThanLuckModule = new DialogueModule("Ah, now there's a thought. Some say it's destiny, others say it's merely coincidence. I like to think it's a mix of both. Luck opens doors, but it's up to us to step through them. I've always taken every opportunity that comes my way.");
                        moreThanLuckModule.AddOption("A good philosophy to live by.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, moreThanLuckModule));
                    });
                player.SendGump(new DialogueGump(player, luckModule));
            });

        greeting.AddOption("Goodbye, Dan.",
            player => true,
            player =>
            {
                player.SendMessage("Dandy Dan gives you a nod and a wink.");
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