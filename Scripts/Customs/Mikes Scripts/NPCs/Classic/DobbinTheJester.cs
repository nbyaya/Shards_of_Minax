using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DobbinTheJester : BaseCreature
{
    [Constructable]
    public DobbinTheJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dobbin the Jester";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(75);
        SetInt(75);
        SetHits(75);

        // Appearance
        AddItem(new ShortPants() { Hue = 1122 });
        AddItem(new FancyShirt() { Hue = 1123 });
        AddItem(new JesterHat() { Hue = 1121 });
        AddItem(new Boots() { Hue = 1124 });
        AddItem(new FeatheredHat() { Hue = 1120 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue
    }

    public DobbinTheJester(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Well, hello there! I am Dobbin, the court's jester, at your service. Care for a joke, a riddle, or just some good old banter? Perhaps I could even tell you about my greatest jest of all time—the time I invented money!");

        greeting.AddOption("Tell me about your greatest jest!",
            player => true,
            player =>
            {
                DialogueModule jestModule = new DialogueModule("Ah, my greatest jest, you ask? Gather 'round, for it is a tale of wit and whimsy, the likes of which have never been seen! You see, I once invented money... or so they say.");
                jestModule.AddOption("How did you invent money?",
                    p => true,
                    p =>
                    {
                        DialogueModule inventionModule = new DialogueModule("It all began when I noticed that people were endlessly trading chickens for grain, and grain for wool. It was chaos! So, I thought, what if there was something everyone could agree was valuable? Something shiny, something that wouldn't spoil or wear out, something that made people feel important!");
                        inventionModule.AddOption("What did you come up with?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule shinyModule = new DialogueModule("I gathered all the shiny trinkets I could find—buttons, polished rocks, bits of metal—and I declared them to be 'Dobbin's Dazzling Coins'! I told everyone that these coins could be used to trade for anything they desired. And, to my surprise, people believed me!");
                                shinyModule.AddOption("Did it work?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule successModule = new DialogueModule("Oh, it worked splendidly—for a while. People started using my dazzling coins for everything! They traded them for food, for clothes, even for favors. But then, of course, they started asking questions. 'Why should this shiny rock be worth more than my chicken?' they'd say. And I'd answer, 'Because it's dazzling, of course!' But doubt began to spread.");
                                        successModule.AddOption("What happened next?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule chaosModule = new DialogueModule("Eventually, people grew tired of my coins. They realized that a shiny button couldn't keep them warm at night or fill their bellies. They started demanding their chickens and grain back, and the whole thing fell apart in a day! But oh, the laughter, the chaos—it was glorious! I had pulled off the ultimate jest.");
                                                chaosModule.AddOption("Did anyone get angry?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule angryModule = new DialogueModule("Oh, many were furious! The blacksmith threatened to forge my head into an anvil, and the farmer wanted to turn me into a scarecrow! But in the end, they couldn't stay mad at old Dobbin for long. They knew it was all in good fun, and besides, they had their chickens back.");
                                                        angryModule.AddOption("How did you calm them down?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule calmModule = new DialogueModule("I put on the greatest performance of my life! I juggled flaming torches, told the most side-splitting jokes, and even danced atop a barrel of pickles! By the end of it, everyone was laughing so hard they forgot why they were angry in the first place. That's the power of a true jester!");
                                                                calmModule.AddOption("You truly are a master of jest!",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, calmModule));
                                                            });
                                                        angryModule.AddOption("They must have admired your bravery.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, angryModule));
                                                    });
                                                chaosModule.AddOption("Sounds like quite the adventure!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, chaosModule));
                                            });
                                        successModule.AddOption("It must have been quite a sight.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, successModule));
                                    });
                                shinyModule.AddOption("Why did people believe you?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule beliefModule = new DialogueModule("Ah, that's the real magic of being a jester! You see, people are always looking for something to believe in—something that makes their lives a little easier, a little brighter. And I gave them that, even if only for a short while. The trick is confidence, my friend! Confidence and a little bit of sparkle.");
                                        beliefModule.AddOption("You certainly are confident!",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, beliefModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, shinyModule));
                            });
                        p.SendGump(new DialogueGump(p, inventionModule));
                    });
                jestModule.AddOption("That sounds like quite the jest!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jestModule));
            });

        greeting.AddOption("Tell me a joke!",
            player => true,
            player =>
            {
                DialogueModule jokeModule = new DialogueModule("Why did the knight refuse to fight the dragon? Because it was a 'drag' on his time! Haha! Want to hear another?");
                jokeModule.AddOption("Yes, tell me another joke!",
                    p => true,
                    p =>
                    {
                        DialogueModule secondJokeModule = new DialogueModule("Of course! Why did the mage stay calm during the spell test? He knew how to keep his cool!");
                        secondJokeModule.AddOption("Haha, good one!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        secondJokeModule.AddOption("No more jokes for now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, secondJokeModule));
                    });
                jokeModule.AddOption("No, that is enough jokes.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jokeModule));
            });

        greeting.AddOption("What's your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I jest and entertain the court! My purpose is to bring laughter and joy, even in the most dire of times. After all, a smile can be as strong as a sword!");
                jobModule.AddOption("That sounds delightful!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("Fit as a fiddle and ready for a riddle! Speaking of which, would you like to hear one?");
                healthModule.AddOption("Sure, tell me a riddle!",
                    p => true,
                    p =>
                    {
                        DialogueModule riddleModule = new DialogueModule("What has keys but can't open locks? A piano, of course! Haha!");
                        riddleModule.AddOption("Good one!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        riddleModule.AddOption("No more riddles, thanks.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, riddleModule));
                    });
                healthModule.AddOption("No riddles for me, thanks.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Goodbye, Dobbin.",
            player => true,
            player =>
            {
                player.SendMessage("Dobbin waves you off with a flourish of his feathered hat.");
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