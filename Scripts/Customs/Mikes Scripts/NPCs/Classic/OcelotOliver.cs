using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Ocelot Oliver")]
public class OcelotOliver : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public OcelotOliver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ocelot Oliver";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(80);
        SetInt(90);
        SetHits(100);

        // Appearance
        AddItem(new FurCape() { Hue = 1173 });
        AddItem(new FurBoots() { Hue = 1173 });
        AddItem(new FurSarong() { Hue = 1173 });
        AddItem(new Dagger() { Name = "Ocelot Oliver's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public OcelotOliver(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Ocelot Oliver, the animal tamer! I have a special fondness for cats. Do you love cats too?");
        
        greeting.AddOption("Yes, I adore cats!",
            player => true,
            player => 
            {
                DialogueModule loveCatsModule = new DialogueModule("Ah, fellow cat lover! I feed a delightful group of stray cats every day. Each one has a unique name and personality. Would you like to hear about them?");
                loveCatsModule.AddOption("Yes, tell me about them!",
                    pl => true,
                    pl => 
                    {
                        DialogueModule catDetailsModule = new DialogueModule("I have a whole family of cats that visit me. There's Whiskers, the clever one, who always finds the best hiding spots. Then there's Paws, the laziest of them all, who loves to sunbathe. Do you want to know about any specific cat?");
                        catDetailsModule.AddOption("Tell me more about Whiskers.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Whiskers is always curious and adventurous. He has a knack for getting into trouble, but he always manages to charm his way out of it!")));
                            });
                        catDetailsModule.AddOption("What about Paws?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Paws is the epitome of relaxation. You can often find him sprawled out on my porch, enjoying the sun. His favorite pastime is watching the birds flutter by.")));
                            });
                        catDetailsModule.AddOption("What other cats do you have?",
                            p => true,
                            p =>
                            {
                                DialogueModule moreCatsModule = new DialogueModule("Well, there's also Mittens, the mischievous one who loves to steal food from Whiskers, and Tigger, who has the loudest purr you've ever heard. Each cat brings joy to my days.");
                                moreCatsModule.AddOption("How do you care for them?",
                                    plq => true,
                                    plq => 
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I feed them daily, of course! I prepare a special mix of cat food and sometimes even leftovers. They know when it's time for dinner and always come running.")));
                                    });
                                moreCatsModule.AddOption("What's your favorite memory with them?",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, there was this one time when all of them gathered around me on a chilly evening. They took turns climbing on my lap, purring loudly, while I read a book. It felt like a scene from a fairy tale.")));
                                    });
                                p.SendGump(new DialogueGump(p, moreCatsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, catDetailsModule));
                    });

                loveCatsModule.AddOption("Not really, I'm more of a dog person.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, dogs have their own charm! They are loyal companions. But if you ever get to know a cat, you might find they have just as much love to give.")));
                    });

                player.SendGump(new DialogueGump(player, loveCatsModule));
            });

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I train and care for a variety of animals, but my heart belongs to the cats. I believe every creature deserves love and respect.")));
            });

        greeting.AddOption("Do you have any magical items?",
            player => true,
            player =>
            {
                DialogueModule magicModule = new DialogueModule("This whistle was passed down through my family for generations. It has a magical tune that only animals can hear. If you're ever in need, use this whistle, and nature might come to your aid.");
                magicModule.AddOption("Can I have it?",
                    pl => true,
                    pl => 
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I have no reward right now. Please return later.")));
                        }
                        else
                        {
                            pl.AddToBackpack(new MaxxiaScroll()); // Reward item
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Here you go, a charm imbued with nature's essence. It might bring you luck on your journeys.")));
                        }
                    });
                player.SendGump(new DialogueGump(player, magicModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Farewell, traveler! May your path be filled with joy and furry friends.")));
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
}
