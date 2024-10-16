using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BelleTheBubbly : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BelleTheBubbly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Belle the Bubbly";
        Body = 0x191; // Human female body

        // Stats
        SetStr(85);
        SetDex(60);
        SetInt(90);
        SetHits(65);

        // Appearance
        AddItem(new JesterHat(1153)); // Jester hat with hue 1153
        AddItem(new JesterSuit(1150)); // Jester suit with hue 1150
        AddItem(new Shoes(1912)); // Shoes with hue 1912

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;

        // Speech Hue
        SpeechHue = 0; // Default speech hue
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Belle the Bubbly, the jester of this realm! How may I bring joy to your day?");

        greeting.AddOption("Tell me about your bubble wand.",
            player => true,
            player =>
            {
                DialogueModule wandModule = new DialogueModule("Ah, my magical bubble wand! It's quite the whimsical contraption, isn't it? It's not just for show—it's actually enchanted to produce bubbles filled with laughter and joy. But... it did have an unfortunate incident once. Would you like to hear about it?");
                wandModule.AddOption("Yes, tell me about the incident.",
                    p => true,
                    p =>
                    {
                        DialogueModule incidentModule = new DialogueModule("One day, I was performing in the town square, making bubbles for all the children. Everything was going splendidly until, suddenly, the wand started acting on its own! Instead of small, joyful bubbles, it started producing enormous, bouncing bubbles that trapped people inside! Can you imagine the chaos?");
                        incidentModule.AddOption("What did you do when it malfunctioned?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule chaosModule = new DialogueModule("At first, I was in shock. I had no idea why the wand was acting this way! People were bouncing around inside the bubbles, and I could barely keep up. I tried to cast a calming spell, but the bubbles just grew more mischievous. They even started floating away with some of the townsfolk!");
                                chaosModule.AddOption("How did you manage to stop it?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule stopModule = new DialogueModule("Stopping it was no easy feat! I had to chase after the bubbles, one by one, and use a special chant I'd learned from an old scroll. It turns out the wand had absorbed too much joyful energy, and the only way to calm it was to sing it a lullaby of sorts. I sang the chant, and one by one, the bubbles began to pop gently, setting everyone free.");
                                        stopModule.AddOption("That must have been quite a sight!",
                                            plan => true,
                                            plan =>
                                            {
                                                DialogueModule sightModule = new DialogueModule("Oh, it was! People were bouncing and laughing—even those who had been frightened couldn't help but giggle at the absurdity of it all. The mayor even said it was the most entertaining disaster he'd ever seen! Of course, I had to promise to be more careful with my wand in the future.");
                                                sightModule.AddOption("Did you ever find out why it malfunctioned?",
                                                    plann => true,
                                                    plann =>
                                                    {
                                                        DialogueModule reasonModule = new DialogueModule("Yes, eventually I did. It turns out that the wand was enchanted with a rare type of crystal called a Joy Shard. The shard absorbs the emotions of those around it, and during the performance, there was so much laughter and joy that the shard became overwhelmed. The magic essentially overflowed, and the wand went a bit haywire.");
                                                        reasonModule.AddOption("What did you do with the Joy Shard?",
                                                            plannn => true,
                                                            plannn =>
                                                            {
                                                                DialogueModule shardModule = new DialogueModule("I decided to keep the Joy Shard, but now I use it more carefully. I learned to regulate its power by channeling the energy into smaller bursts. It's still the heart of my wand, but now it knows not to get too carried away. Sometimes, I even use its power to cheer up those who are feeling particularly down—it just needs to be handled with care.");
                                                                shardModule.AddOption("Can you show me how it works?",
                                                                    plannnn => true,
                                                                    plannnn =>
                                                                    {
                                                                        DialogueModule demoModule = new DialogueModule("I'd love to show you! Just stand back a bit—I wouldn't want any unexpected bubbles to carry you off! [Belle waves her wand, and a series of shimmering bubbles float around you, each one reflecting a kaleidoscope of colors. You feel an inexplicable sense of warmth and joy as they gently pop around you.]");
                                                                        demoModule.AddOption("That's amazing, Belle!",
                                                                            plannnnn => true,
                                                                            plannnnn =>
                                                                            {
                                                                                plannnnn.SendGump(new DialogueGump(plannnnn, CreateGreetingModule()));
                                                                            });
                                                                        demoModule.AddOption("Maybe I should keep my distance from magical bubbles...",
                                                                            plannnnn => true,
                                                                            plannnnn =>
                                                                            {
                                                                                plannnnn.SendGump(new DialogueGump(plannnnn, CreateGreetingModule()));
                                                                            });
                                                                        plannnn.SendGump(new DialogueGump(plannnn, demoModule));
                                                                    });
                                                                shardModule.AddOption("Thank you for sharing that story, Belle.",
                                                                    plannnn => true,
                                                                    plannnn =>
                                                                    {
                                                                        plannnn.SendGump(new DialogueGump(plannnn, CreateGreetingModule()));
                                                                    });
                                                                plannn.SendGump(new DialogueGump(plannn, shardModule));
                                                            });
                                                        reasonModule.AddOption("That's quite fascinating, Belle.",
                                                            plannn => true,
                                                            plannn =>
                                                            {
                                                                plannn.SendGump(new DialogueGump(plannn, CreateGreetingModule()));
                                                            });
                                                        plann.SendGump(new DialogueGump(plann, reasonModule));
                                                    });
                                                sightModule.AddOption("You truly are an amazing jester.",
                                                    plann => true,
                                                    plann =>
                                                    {
                                                        plann.SendGump(new DialogueGump(plann, CreateGreetingModule()));
                                                    });
                                                plan.SendGump(new DialogueGump(plan, sightModule));
                                            });
                                        stopModule.AddOption("That sounds like quite the adventure!",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, stopModule));
                                    });
                                chaosModule.AddOption("That must have been terrifying!",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule terrifyingModule = new DialogueModule("It certainly was at first, but once I realized the bubbles weren't harmful, just mischievous, I couldn't help but laugh too. It's important to stay calm and think creatively in such situations, wouldn't you agree?");
                                        terrifyingModule.AddOption("Absolutely, staying calm is key.",
                                            plan => true,
                                            plan =>
                                            {
                                                plan.SendGump(new DialogueGump(plan, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, terrifyingModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, chaosModule));
                            });
                        incidentModule.AddOption("That sounds hilarious!",
                            pl => true,
                            pl =>
                            {
                                DialogueModule funnyModule = new DialogueModule("It certainly was—afterwards! At the time, I was panicking, but looking back, it was one of the funniest performances I've ever given. The children loved it, and I suppose that's what truly matters.");
                                funnyModule.AddOption("You're an incredible performer, Belle.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, funnyModule));
                            });
                        p.SendGump(new DialogueGump(p, incidentModule));
                    });
                wandModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wandModule));
            });

        return greeting;
    }

    public BelleTheBubbly(Serial serial) : base(serial) { }

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