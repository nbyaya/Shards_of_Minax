using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BrawnTheWrestler : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BrawnTheWrestler() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Brawn the Wrestler";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(80);
        SetInt(70);

        SetHits(130);

        // Appearance
        AddItem(new ShortPants(2126));
        AddItem(new FancyShirt(2126));
        AddItem(new ThighBoots(1904));
        AddItem(new BodySash { Name = "Brawn's Championship Belt" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BrawnTheWrestler(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there! I'm Brawn the Wrestler. What can I do for you today, traveler?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I am Brawn the Wrestler! Known for my strength and passion for training the youth of today. Do you have any questions about wrestling or strength?");
                whoModule.AddOption("Tell me about wrestling.",
                    p => true,
                    p =>
                    {
                        DialogueModule wrestlingModule = new DialogueModule("Wrestling is not just about might. It's about wit, determination, and heart. Do you have heart?");
                        wrestlingModule.AddOption("Yes, I do!",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("That's the spirit! Always fight with passion and never give up!");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        wrestlingModule.AddOption("No, I don't.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("One must find their heart before they can truly wrestle.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        wrestlingModule.AddOption("How do I train for the Moonglow Wrestling Championships?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule trainingModule = new DialogueModule("Ah, the Moonglow Wrestling Championships! It's the pinnacle of our sport. To prepare, you need rigorous training in both body and mind. Let me break it down for you.");
                                trainingModule.AddOption("Tell me about physical training.",
                                    pq => true,
                                    pq =>
                                    {
                                        DialogueModule physicalTrainingModule = new DialogueModule("Physical training is all about building strength, agility, and endurance. You must run every morning to build stamina, lift weights to increase power, and practice grappling techniques with a partner. Focus on exercises like squats, push-ups, and rope climbs. Consistency is key!");
                                        physicalTrainingModule.AddOption("What about diet?",
                                            pl2 => true,
                                            pl2 =>
                                            {
                                                DialogueModule dietModule = new DialogueModule("Diet is crucial. You need plenty of protein to build muscle—meat, eggs, beans. Stay hydrated, drink plenty of water, and avoid too much sugar. Eat your greens too, they keep your body in balance.");
                                                dietModule.AddOption("Got it. What's next?",
                                                    pl3 => true,
                                                    pl3 =>
                                                    {
                                                        pl3.SendGump(new DialogueGump(pl3, physicalTrainingModule));
                                                    });
                                                dietModule.AddOption("That's too much for me.",
                                                    pl3 => true,
                                                    pl3 =>
                                                    {
                                                        pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                    });
                                                pl2.SendGump(new DialogueGump(pl2, dietModule));
                                            });
                                        physicalTrainingModule.AddOption("Sounds tough, but I'm ready!",
                                            pl2 => true,
                                            pl2 =>
                                            {
                                                pl2.SendMessage("Good! Remember, discipline is what separates champions from the rest.");
                                                pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, physicalTrainingModule));
                                    });
                                trainingModule.AddOption("Tell me about mental preparation.",
                                    pw => true,
                                    pw =>
                                    {
                                        DialogueModule mentalTrainingModule = new DialogueModule("Mental preparation is just as important. You must visualize your success—imagine yourself in the ring, facing your opponents, using your skills. Meditation can help calm your mind and improve focus. Confidence comes from within, and the Championships are as much a test of will as they are of strength.");
                                        mentalTrainingModule.AddOption("How do I build confidence?",
                                            pl2 => true,
                                            pl2 =>
                                            {
                                                DialogueModule confidenceModule = new DialogueModule("Confidence comes from facing challenges head-on. Start by setting small goals and achieving them. Each success builds upon the last. Remember, everyone gets nervous, even the greats. The difference is how you handle it.");
                                                confidenceModule.AddOption("That's great advice.",
                                                    pl3 => true,
                                                    pl3 =>
                                                    {
                                                        pl3.SendMessage("I'm glad you think so! Now, go out there and show the world what you're made of!");
                                                        pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                    });
                                                pl2.SendGump(new DialogueGump(pl2, confidenceModule));
                                            });
                                        p.SendGump(new DialogueGump(p, mentalTrainingModule));
                                    });
                                trainingModule.AddOption("Tell me about my rivals.",
                                    pe => true,
                                    pe =>
                                    {
                                        DialogueModule rivalsModule = new DialogueModule("Ah, the Moonglow Wrestling Championships are filled with formidable opponents. There's Garth the Iron Fist, known for his raw power and unbreakable grip. Then there's Silvia the Swift, who moves like the wind and can slip out of any hold. And don't forget Krel the Crusher, a brute whose sheer strength is unmatched. Each of them has their own style and strengths.");
                                        rivalsModule.AddOption("How do I defeat Garth the Iron Fist?",
                                            pl2 => true,
                                            pl2 =>
                                            {
                                                DialogueModule garthModule = new DialogueModule("To beat Garth, you must use speed and strategy. His strength is his greatest weapon, but also his weakness. If you can tire him out and avoid his holds, you'll have the upper hand. Focus on quick jabs and evading his grasp.");
                                                garthModule.AddOption("Got it, thanks!",
                                                    pl3 => true,
                                                    pl3 =>
                                                    {
                                                        pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                    });
                                                pl2.SendGump(new DialogueGump(pl2, garthModule));
                                            });
                                        rivalsModule.AddOption("How do I counter Silvia the Swift?",
                                            pl2 => true,
                                            pl2 =>
                                            {
                                                DialogueModule silviaModule = new DialogueModule("Silvia is quick, but if you can predict her movements, you can catch her. Keep your eyes on her hips—they telegraph where she's going. Timing is everything. Wait for the right moment, and then strike.");
                                                silviaModule.AddOption("Thanks for the tip!",
                                                    pl3 => true,
                                                    pl3 =>
                                                    {
                                                        pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                    });
                                                pl2.SendGump(new DialogueGump(pl2, silviaModule));
                                            });
                                        rivalsModule.AddOption("What about Krel the Crusher?",
                                            pl2 => true,
                                            pl2 =>
                                            {
                                                DialogueModule krelModule = new DialogueModule("Krel is all about brute force. You can't match him in raw power, so you must outthink him. Use his momentum against him—when he charges, sidestep and counterattack. Agility and a cool head will be your greatest assets.");
                                                krelModule.AddOption("I'll keep that in mind!",
                                                    pl3 => true,
                                                    pl3 =>
                                                    {
                                                        pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                    });
                                                pl2.SendGump(new DialogueGump(pl2, krelModule));
                                            });
                                        p.SendGump(new DialogueGump(p, rivalsModule));
                                    });
                                trainingModule.AddOption("Thank you, Brawn.",
                                    pr => true,
                                    pr =>
                                    {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, trainingModule));
                            });
                        p.SendGump(new DialogueGump(p, wrestlingModule));
                    });
                whoModule.AddOption("What do you teach the youth?",
                    p => true,
                    p =>
                    {
                        DialogueModule youthModule = new DialogueModule("The youth of today are our future. I teach them to be strong in body, mind, and spirit. Determination is key in wrestling, but also in life. Speaking of truths, have you ever pondered the mantra of Honesty? The first syllable is ZIM.");
                        youthModule.AddOption("Tell me more about determination.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Determination is key in wrestling, but also in understanding the deeper truths of life.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, youthModule));
                    });
                whoModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("Fit as a fiddle!");
                healthModule.AddOption("That's good to hear!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Can I get a reward?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                player.AddToBackpack(new BeltSlotChangeDeed());
                lastRewardTime = DateTime.UtcNow;
                player.SendMessage("For your thoughtful inquiry, please accept this reward.");
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("No reward right now?",
            player => DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10),
            player =>
            {
                player.SendMessage("I have no reward right now. Please return later.");
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Brawn nods at you respectfully.");
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