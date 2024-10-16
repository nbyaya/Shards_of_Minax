using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Lark the Merry")]
public class LarkTheMerry2 : BaseCreature
{
    [Constructable]
    public LarkTheMerry2() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lark the Merry";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(70);

        // Appearance
        AddItem(new JesterSuit() { Hue = 1153 });
        AddItem(new JesterHat() { Hue = 1153 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        Fame = 0;
        Karma = 0;
    }

    public LarkTheMerry2(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Lark the Merry, the kingdom's jester! How can I entertain you today?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                DialogueModule healthModule = new DialogueModule("As fit as a fiddle! But sometimes, a jester's heart aches for the world. Care to hear a tale?");
                healthModule.AddOption("Yes, tell me your tale.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule taleModule = new DialogueModule("Once, I traveled to the Whispering Woods where I encountered a mischievous sprite! She led me on a wild chase. I never did catch her, but the stories I heard were worth the effort!");
                        taleModule.AddOption("What stories did you hear?",
                            p => true,
                            p => 
                            {
                                DialogueModule storiesModule = new DialogueModule("They spoke of ancient treasures hidden deep within the forest, guarded by a fierce dragon. I wonder if they are true!");
                                p.SendGump(new DialogueGump(p, storiesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, taleModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("I jest and entertain! A riddle here, a song there, all to bring smiles to the faces of weary travelers!");
                jobModule.AddOption("Do you know any good riddles?",
                    p => true,
                    p => 
                    {
                        DialogueModule riddleModule = new DialogueModule("Ah, a riddle for thee! What has keys but opens no doors?");
                        riddleModule.AddOption("I give up, what is it?",
                            pl => true,
                            pl => 
                            {
                                DialogueModule answerModule = new DialogueModule("A piano, of course! Well guessed!");
                                pl.SendGump(new DialogueGump(pl, answerModule));
                            });
                        riddleModule.AddOption("I think I know! It's a piano!",
                            pl => true,
                            pl => 
                            {
                                DialogueModule correctModule = new DialogueModule("Indeed, a piano it is! Well guessed! Would you like to hear another?");
                                correctModule.AddOption("Yes, please!",
                                    pq => true,
                                    pq => 
                                    {
                                        DialogueModule anotherRiddle = new DialogueModule("What can travel around the world while staying in a corner?");
                                        anotherRiddle.AddOption("I know this one! It's a stamp!",
                                            plw => true,
                                            plw => 
                                            {
                                                DialogueModule stampModule = new DialogueModule("Bravo! You're quite clever! Want to hear more or ask about something else?");
                                                stampModule.AddOption("More riddles!",
                                                    pe => true,
                                                    pe => 
                                                    {
                                                        p.SendGump(new DialogueGump(p, anotherRiddle));
                                                    });
                                                stampModule.AddOption("Tell me about your adventures.",
                                                    pr => true,
                                                    pr => 
                                                    {
                                                        DialogueModule adventureModule = new DialogueModule("Oh, the tales I could share! From a dance with goblins to a narrow escape from a band of brigands!");
                                                        p.SendGump(new DialogueGump(p, adventureModule));
                                                    });
                                                pl.SendGump(new DialogueGump(pl, stampModule));
                                            });
                                        anotherRiddle.AddOption("I don't know.",
                                            plt => true,
                                            plt => 
                                            {
                                                DialogueModule wrongAnswerModule = new DialogueModule("A stamp! A simple yet clever answer!");
                                                pl.SendGump(new DialogueGump(pl, wrongAnswerModule));
                                            });
                                        p.SendGump(new DialogueGump(p, anotherRiddle));
                                    });
                                pl.SendGump(new DialogueGump(pl, correctModule));
                            });
                        riddleModule.AddOption("Nay, try again!",
                            pl => true,
                            pl => 
                            {
                                DialogueModule tryAgainModule = new DialogueModule("Nay, try again!");
                                pl.SendGump(new DialogueGump(pl, tryAgainModule));
                            });
                        player.SendGump(new DialogueGump(player, riddleModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think of this kingdom?",
            player => true,
            player => 
            {
                DialogueModule kingdomModule = new DialogueModule("Ah, our kingdom is a tapestry of stories! From brave knights to wise mages, each has their part to play. But it’s the people that make it shine!");
                kingdomModule.AddOption("And what about the dangers?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule dangerModule = new DialogueModule("Dangers lurk in every shadow! Bandits roam the roads, and the forest hides creatures of the night. But fear not! For every danger, there’s a hero willing to face it!");
                        dangerModule.AddOption("What makes a hero?",
                            p => true,
                            p => 
                            {
                                DialogueModule heroModule = new DialogueModule("A hero is someone who stands up for others, who faces their fears, and who brings hope to the hopeless!");
                                p.SendGump(new DialogueGump(p, heroModule));
                            });
                        pl.SendGump(new DialogueGump(pl, dangerModule));
                    });
                player.SendGump(new DialogueGump(player, kingdomModule));
            });

        greeting.AddOption("Do you have any advice for adventurers?",
            player => true,
            player => 
            {
                DialogueModule adviceModule = new DialogueModule("Ah, heed this advice: always trust your instincts, never underestimate a foe, and always carry a few healing potions. Laughter can be the best medicine, too!");
                adviceModule.AddOption("What about potions?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule potionsModule = new DialogueModule("Potions can heal, grant strength, or even make you invisible! But be wary—some concoctions have unexpected effects!");
                        pl.SendGump(new DialogueGump(pl, potionsModule));
                    });
                player.SendGump(new DialogueGump(player, adviceModule));
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
