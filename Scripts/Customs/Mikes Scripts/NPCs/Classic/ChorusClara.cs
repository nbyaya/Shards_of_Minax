using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ChorusClara : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ChorusClara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Chorus Clara";
        Body = 0x191; // Human female body

        // Stats
        SetStr(119);
        SetDex(67);
        SetInt(82);
        SetHits(86);

        // Appearance
        AddItem(new FancyDress() { Hue = 94 }); // Fancy dress with hue 94
        AddItem(new Boots() { Hue = 48 }); // Boots with hue 48
        AddItem(new LeatherGloves() { Name = "Clara's Chorus Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ChorusClara(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Chorus Clara, the wandering bard. How can I entertain or assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Chorus Clara, a bard who sings songs of valor and courage. I have wandered many lands, sharing stories from the frozen tundras to the scorching deserts.");
                aboutModule.AddOption("Tell me more about your tales of valor.",
                    p => true,
                    p =>
                    {
                        DialogueModule talesModule = new DialogueModule("Are you interested in tales of bravery? One of my favorites is about the knight Sir Lancel, who faced a dragon to save a village. Valor is the essence of bravery, a virtue to be admired.");
                        talesModule.AddOption("That sounds fascinating. Tell me more!",
                            pl => true,
                            pl =>
                            {
                                DialogueModule moreTalesModule = new DialogueModule("Sir Lancel's courage knew no bounds. He ventured alone into the dragon's lair, armed only with his wits and determination. The battle was fierce, but his heart, full of valor, never wavered.");
                                moreTalesModule.AddOption("What happened next?",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Clara smiles as she continues her tale, describing Sir Lancel's victorious return.");
                                    });
                                moreTalesModule.AddOption("I must take my leave for now.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, moreTalesModule));
                            });
                        talesModule.AddOption("Perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, talesModule));
                    });
                aboutModule.AddOption("Tell me about your favorite instruments.",
                    p => true,
                    p =>
                    {
                        DialogueModule instrumentsModule = new DialogueModule("Ah, my favorite instruments! Music is my heart's true language. Each instrument has its own magic and personality.");
                        instrumentsModule.AddOption("Which instrument do you love the most?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule favoriteInstrumentModule = new DialogueModule("It's hard to choose, but if I must, it would be the lute. Its warm, melodic tones can capture both joy and sorrow perfectly. When I strum the strings, it's as if I'm painting emotions into the air.");
                                favoriteInstrumentModule.AddOption("Tell me more about the lute.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule luteDetailsModule = new DialogueModule("The lute has always been a symbol of storytelling. Its strings resonate with the emotions of those who play it. I remember once playing for a small village, and by the end of my song, there wasn't a dry eye in the crowd. The lute, in the right hands, can touch hearts like no other.");
                                        luteDetailsModule.AddOption("It sounds enchanting.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Clara smiles, her eyes filled with nostalgia. 'Indeed, it is. It is an instrument of pure emotion.'");
                                            });
                                        luteDetailsModule.AddOption("Do you play any other string instruments?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule otherStringsModule = new DialogueModule("Yes, I also play the harp. The harp's ethereal sound can make you feel as though you're drifting through the clouds. It requires a gentle touch, and each note can linger, almost like a whisper from a dream.");
                                                otherStringsModule.AddOption("What kind of songs do you play on the harp?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule harpSongsModule = new DialogueModule("On the harp, I often play songs of tranquility and reflection. The soft notes are perfect for conveying a sense of peace. One of my favorite pieces is 'The Echoes of Dawn', a melody that embodies the beauty of a new beginning.");
                                                        harpSongsModule.AddOption("That sounds beautiful.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Clara nods, her smile serene. 'It truly is. Music has the power to heal the soul.'");
                                                            });
                                                        harpSongsModule.AddOption("Do you play anything more upbeat?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule upbeatModule = new DialogueModule("Oh, absolutely! I sometimes play lively jigs on the harp, though it’s not as common. My favorite for upbeat tunes would be the fiddle. It brings such energy and joy that it's impossible not to dance along.");
                                                                upbeatModule.AddOption("Tell me about the fiddle.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule fiddleModule = new DialogueModule("The fiddle is an instrument of pure merriment. Its fast-paced, vibrant notes can get even the weariest traveler tapping their feet. I often play it at festivals, where the air is filled with laughter and the sound of dancing feet.");
                                                                        fiddleModule.AddOption("It must be wonderful to bring people so much joy.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendMessage("Clara beams, her eyes sparkling. 'It truly is. There is no greater reward for a bard than seeing the joy on people's faces.'");
                                                                            });
                                                                        fiddleModule.AddOption("I'd love to hear you play sometime.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendMessage("Clara gives a warm smile. 'Perhaps one day, our paths will cross again, and I shall play for you.'");
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, fiddleModule));
                                                                    });
                                                                upbeatModule.AddOption("Maybe another time.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, upbeatModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, harpSongsModule));
                                                    });
                                                otherStringsModule.AddOption("Maybe another time.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, otherStringsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, luteDetailsModule));
                                    });
                                favoriteInstrumentModule.AddOption("Do you have other favorites?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule otherInstrumentsModule = new DialogueModule("Of course! I also love the flute. The flute's clear, sweet notes remind me of a gentle breeze rustling through the leaves. It's perfect for woodland melodies and light-hearted tunes.");
                                        otherInstrumentsModule.AddOption("Tell me about the flute.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule fluteModule = new DialogueModule("The flute is simple yet elegant. I play it when I wish to feel connected to nature. Its sound carries across the wind, making it perfect for outdoor performances. Once, I played it atop a hill at sunrise, and the beauty of that moment has stayed with me ever since.");
                                                fluteModule.AddOption("That sounds magical.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Clara sighs wistfully. 'It truly was. Music can turn even the simplest of moments into something extraordinary.'");
                                                    });
                                                fluteModule.AddOption("I'd love to learn to play the flute.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Clara nods encouragingly. 'It's a wonderful choice. With practice and patience, I'm sure you could create your own magical moments.'");
                                                    });
                                                plb.SendGump(new DialogueGump(plb, fluteModule));
                                            });
                                        otherInstrumentsModule.AddOption("Maybe another time.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, otherInstrumentsModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, favoriteInstrumentModule));
                            });
                        instrumentsModule.AddOption("Do you have a least favorite instrument?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule leastFavoriteModule = new DialogueModule("If I had to choose, I would say the bagpipes. They have their charm, and in the right hands, they can be quite stirring. But for me, their sound is a bit too brash and overpowering for most occasions.");
                                leastFavoriteModule.AddOption("I can see why that might be.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Clara laughs softly. 'Indeed. Though I must admit, I've heard them played beautifully during Highland festivals.'");
                                    });
                                leastFavoriteModule.AddOption("I think bagpipes are wonderful!",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Clara nods thoughtfully. 'To each their own, traveler. Music is a matter of the heart, and what speaks to one may not speak to another.'");
                                    });
                                pl.SendGump(new DialogueGump(pl, leastFavoriteModule));
                            });
                        instrumentsModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, instrumentsModule));
                    });
                aboutModule.AddOption("That sounds lovely. Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Can you tell me about valor and virtue?",
            player => true,
            player =>
            {
                DialogueModule virtueModule = new DialogueModule("Valor is a virtue to be admired. It is the courage to face any challenge, no matter how daunting. Many have sought to truly embody it, but only a few succeed.");
                virtueModule.AddOption("I wish to embody valor myself.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Clara nods solemnly. 'It is a noble pursuit, one that requires dedication and heart.'");
                    });
                virtueModule.AddOption("Perhaps another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("Do you have a reward for a true hero?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                    cooldownModule.AddOption("Understood. Goodbye.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("If you truly possess the heart of a hero, I'd like to reward you with a special song I've composed. Would you like to hear it?");
                    rewardModule.AddOption("Yes, I would love to hear it!",
                        p => true,
                        p =>
                        {
                            p.SendMessage("Clara hands you a scroll, her eyes sparkling. 'May this inspire you on your journey.'");
                            p.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow;
                        });
                    rewardModule.AddOption("Perhaps another time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Clara smiles and nods. 'Farewell, traveler.'");
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