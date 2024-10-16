using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Ravishing Rosa")]
public class RavishingRosa : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public RavishingRosa() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ravishing Rosa";
        Body = 0x191; // Human female body

        // Stats
        SetStr(89);
        SetDex(73);
        SetInt(52);
        SetHits(64);

        // Appearance
        AddItem(new LongPants() { Hue = 2967 });
        AddItem(new BodySash() { Hue = 2968 });
        AddItem(new Sandals() { Hue = 2969 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public RavishingRosa(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, kind traveler. I am Ravishing Rosa, a courtesan of these lands. How may I entertain you today?");
        
        greeting.AddOption("Tell me about your most disgusting customers.",
            player => true,
            player =>
            {
                DialogueModule disgustingCustomersModule = new DialogueModule("Oh, where do I begin? My line of work attracts quite an array of characters, some of whom are far from charming.");
                
                disgustingCustomersModule.AddOption("What was the worst customer like?",
                    p => true,
                    p =>
                    {
                        DialogueModule worstCustomerModule = new DialogueModule("Once, I had a customer who arrived with a smell so foul it could wilt flowers! He thought himself a great poet, but his verses were as unpleasant as his odor. I struggled to keep my composure while he recited his work.");
                        worstCustomerModule.AddOption("How did you handle that?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("I forced a smile and complimented his creativity while mentally planning my escape. Sometimes, a little flattery can help defuse an awkward situation.")));
                            });
                        worstCustomerModule.AddOption("Did you ever tell him the truth?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Oh heavens, no! A courtesan must always maintain a façade. Honesty is a luxury I can't afford in my profession.")));
                            });
                        p.SendGump(new DialogueGump(p, worstCustomerModule));
                    });

                disgustingCustomersModule.AddOption("What about the one who made you feel uncomfortable?",
                    p => true,
                    p =>
                    {
                        DialogueModule uncomfortableCustomerModule = new DialogueModule("Ah, yes! There was a fellow who thought it was amusing to compare me to his pet iguana. He went on and on about how I reminded him of it, claiming it was a compliment. I was horrified and intrigued at the same time.");
                        uncomfortableCustomerModule.AddOption("What did you say?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("I laughed nervously and asked him if the iguana danced as well. Humor can often be a shield against discomfort.")));
                            });
                        uncomfortableCustomerModule.AddOption("Did you ever see him again?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Fortunately, no! I learned to avoid establishments he frequented. The last thing I wanted was a repeat performance of that strange comparison.")));
                            });
                        p.SendGump(new DialogueGump(p, uncomfortableCustomerModule));
                    });

                disgustingCustomersModule.AddOption("Any others you'd like to share?",
                    p => true,
                    p =>
                    {
                        DialogueModule additionalCustomersModule = new DialogueModule("There was also the man who insisted on wearing a cheese-scented cologne. I mean, who does that? His idea of romance was bringing me a cheese platter. I was tempted to run away screaming.");
                        additionalCustomersModule.AddOption("Did you eat any cheese?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("I nibbled on a piece out of sheer politeness. However, I discreetly spat it out when he turned away. Some things are best left uneaten!")));
                            });
                        additionalCustomersModule.AddOption("What was his reaction?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("He seemed pleased, thinking he was impressing me. Little did he know, it was a complete turn-off!")));
                            });
                        p.SendGump(new DialogueGump(p, additionalCustomersModule));
                    });

                disgustingCustomersModule.AddOption("How do you cope with such experiences?",
                    p => true,
                    p =>
                    {
                        DialogueModule copingModule = new DialogueModule("I take each experience as a lesson. Sometimes, I find humor in the absurdity of it all. A good laugh can clear the air and lift my spirits.");
                        copingModule.AddOption("Do you share these stories with friends?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Absolutely! My fellow courtesans and I often exchange tales of our worst customers over drinks. It’s a bonding experience!")));
                            });
                        copingModule.AddOption("What else helps you unwind?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("I often indulge in soothing baths with aromatic oils. It washes away the day's memories, leaving me refreshed and ready for new adventures.")));
                            });
                        p.SendGump(new DialogueGump(p, copingModule));
                    });

                player.SendGump(new DialogueGump(player, disgustingCustomersModule));
            });

        greeting.AddOption("Tell me about your best customers.",
            player => true,
            player =>
            {
                DialogueModule bestCustomersModule = new DialogueModule("Ah, the best customers! They are always a pleasure to serve. They know how to treat a lady with respect and kindness.");
                bestCustomersModule.AddOption("What makes them special?",
                    p => true,
                    p =>
                    {
                        DialogueModule specialCustomersModule = new DialogueModule("They engage in pleasant conversations, make me laugh, and often bring little gifts like flowers or sweet treats. It's the small gestures that count!");
                        specialCustomersModule.AddOption("Can you share a memorable experience?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("One gentleman surprised me with a beautiful bouquet of rare blossoms. He said they reminded him of my beauty, and it truly made my day.")));
                            });
                        specialCustomersModule.AddOption("What do they usually want?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("They mostly want companionship, a good conversation, and sometimes a dance. It’s a delightful exchange of stories and laughter.")));
                            });
                        p.SendGump(new DialogueGump(p, specialCustomersModule));
                    });

                player.SendGump(new DialogueGump(player, bestCustomersModule));
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
