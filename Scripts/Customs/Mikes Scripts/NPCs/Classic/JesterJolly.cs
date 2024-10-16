using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Jester Jolly")]
public class JesterJolly : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public JesterJolly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jester Jolly";
        Body = 0x190; // Human male body

        // Stats
        Str = 120;
        Dex = 80;
        Int = 100;
        Hits = 80;

        // Appearance
        AddItem(new JesterHat() { Hue = 2210 });
        AddItem(new JesterSuit() { Hue = 1260 });
        AddItem(new Boots() { Hue = 1154 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public JesterJolly(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Jester Jolly, the whimsical entertainer! What tickles your fancy today?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => {
                DialogueModule healthModule = new DialogueModule("Oh, I'm always in high spirits, thank you for asking! The laughter of the crowd fills my heart! But tell me, how are you faring on your adventures?");
                healthModule.AddOption("I'm doing well, thank you!",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, greeting)); });
                healthModule.AddOption("Things could be better.",
                    pl => true,
                    pl => {
                        DialogueModule adviceModule = new DialogueModule("Ah, the weight of the world can be heavy! Remember, even the darkest clouds can bring the sweetest rain. Have you tried to find joy in small things?");
                        adviceModule.AddOption("I try to laugh whenever I can.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        adviceModule.AddOption("It's hard to laugh right now.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, adviceModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => {
                DialogueModule jobModule = new DialogueModule("My job is to bring laughter and joy through jests and pranks! Every day is a new stage, and the world is my audience! What about you, brave adventurer? What do you do?");
                jobModule.AddOption("I'm an adventurer, seeking treasures.",
                    pl => true,
                    pl => {
                        DialogueModule adventureModule = new DialogueModule("Ah, the thrill of adventure! Each treasure has its own tale. Have you found anything extraordinary on your journeys?");
                        adventureModule.AddOption("Yes, I found a magical artifact!",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        adventureModule.AddOption("Just some mundane things, unfortunately.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, adventureModule));
                    });
                jobModule.AddOption("I'm a merchant, trading goods.",
                    pl => true,
                    pl => {
                        DialogueModule merchantModule = new DialogueModule("Ah, a merchant! You hold the keys to the world's treasures! What's the most valuable item you’ve ever sold?");
                        merchantModule.AddOption("A rare gemstone from the mountains.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        merchantModule.AddOption("Just common wares, nothing special.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, merchantModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you have any funny stories?",
            player => true,
            player =>
            {
                DialogueModule storyModule = new DialogueModule("Ah, pranks! They're the spice of a jester's life! Would you like to hear about my most memorable prank, or perhaps one of my performances?");
                storyModule.AddOption("Tell me about a prank!",
                    pl => true,
                    pl => {
                        DialogueModule prankModule = new DialogueModule("Once, I replaced the mayor's speech scroll with a scroll of silly dances! The entire town burst into laughter as he attempted to dance his speech! Have you pulled any pranks of your own?");
                        prankModule.AddOption("I once tricked a friend into thinking their horse could fly!",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        prankModule.AddOption("I prefer to keep things serious.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, prankModule));
                    });
                storyModule.AddOption("Tell me about a performance!",
                    pl => true,
                    pl => {
                        DialogueModule performanceModule = new DialogueModule("During one festival, I juggled flaming torches while reciting silly poems! The crowd roared with laughter! Have you ever performed in front of an audience?");
                        performanceModule.AddOption("Yes, I sang a song once!",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        performanceModule.AddOption("No, I couldn't handle the stage fright.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, performanceModule));
                    });
                player.SendGump(new DialogueGump(player, storyModule));
            });

        greeting.AddOption("What do you think about the eight virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("Ah, the eight virtues! Each a facet of a radiant gem. Which one do you find most resonates with you?");
                virtuesModule.AddOption("I hold honesty dear.",
                    pl => true,
                    pl => {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            pl.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            pl.SendMessage("Ah, so that virtue resonates with your soul! Here, for being so open about your values, accept this small token from me.");
                            pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                        pl.SendGump(new DialogueGump(pl, greeting));
                    });
                virtuesModule.AddOption("Compassion is my guiding light.",
                    pl => true,
                    pl => {
                        DialogueModule compassionModule = new DialogueModule("Compassion! A noble virtue! It’s the thread that binds us all in times of hardship. How do you express compassion in your life?");
                        compassionModule.AddOption("I help those in need whenever I can.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        compassionModule.AddOption("It's not always easy for me.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, compassionModule));
                    });
                virtuesModule.AddOption("I admire valor.",
                    pl => true,
                    pl => {
                        DialogueModule valorModule = new DialogueModule("Valor! The courage to face fear! It’s the spirit that propels us into the unknown. What acts of valor have you encountered in your journeys?");
                        valorModule.AddOption("I once faced a dragon and lived to tell the tale!",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        valorModule.AddOption("I prefer to avoid danger.",
                            pll => true,
                            pll => { pll.SendGump(new DialogueGump(pll, greeting)); });
                        pl.SendGump(new DialogueGump(pl, valorModule));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
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
