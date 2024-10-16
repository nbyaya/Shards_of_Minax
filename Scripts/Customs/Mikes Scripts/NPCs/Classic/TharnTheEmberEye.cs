using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Tharn the Ember Eye")]
public class TharnTheEmberEye : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TharnTheEmberEye() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tharn the Ember Eye";
        Body = 0x190; // Human male body

        // Stats
        SetStr(70);
        SetDex(50);
        SetInt(150);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1177 });
        AddItem(new Sandals() { Hue = 1176 });
        AddItem(new Item(0x1F13) { Name = "Tharn's Oracle" });

        Hue = 1175;
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public TharnTheEmberEye(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Tharn the Ember Eye, a watery enigma. How may I assist you?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a seeker of elemental balance, a guardian of the ethereal currents. My essence flows with the tides, and I am untouched by the flames of affliction.");
                aboutModule.AddOption("What about your past?",
                    p => true,
                    p => 
                    {
                        DialogueModule pastModule = new DialogueModule("Ah, my past is filled with shadows and whispers. I once served as an assassin for the Fire Kingdom, tasked with a perilous hunt.");
                        pastModule.AddOption("What were you hunting?",
                            pl => true,
                            pl => 
                            {
                                DialogueModule huntModule = new DialogueModule("I was hired to track down the Avatar of Elements, a being of immense power said to control the elemental forces themselves. It was a daunting mission.");
                                huntModule.AddOption("What drove you to take such a job?",
                                    p2 => true,
                                    p2 =>
                                    {
                                        DialogueModule motiveModule = new DialogueModule("I sought the thrill of the hunt and the promise of power. The Fire Kingdom promised great rewards for capturing the Avatar. But the cost was steep, as I soon discovered.");
                                        motiveModule.AddOption("What happened during the hunt?",
                                            p3 => true,
                                            p3 =>
                                            {
                                                DialogueModule eventModule = new DialogueModule("As I tracked the Avatar across treacherous terrains, I encountered fierce elemental guardians. Each step closer to my target revealed the true nature of power and responsibility.");
                                                eventModule.AddOption("Did you succeed in your mission?",
                                                    p4 => true,
                                                    p4 =>
                                                    {
                                                        DialogueModule successModule = new DialogueModule("I came close, but in that moment, I realized that capturing the Avatar would upset the delicate balance of the elements. I chose to spare them instead.");
                                                        successModule.AddOption("What did you learn from that experience?",
                                                            p5 => true,
                                                            p5 =>
                                                            {
                                                                DialogueModule lessonModule = new DialogueModule("I learned that true strength lies not in domination, but in understanding and harmony. My time with the Fire Kingdom changed me forever.");
                                                                lessonModule.AddOption("Would you tell me more about the Fire Kingdom?",
                                                                    p6 => true,
                                                                    p6 =>
                                                                    {
                                                                        DialogueModule fireKingdomModule = new DialogueModule("The Fire Kingdom is a realm of passion and power, ruled by those who value strength above all else. Their desire for control often leads to conflict with other elemental realms.");
                                                                        fireKingdomModule.AddOption("How does it compare to the other kingdoms?",
                                                                            p7 => true,
                                                                            p7 =>
                                                                            {
                                                                                DialogueModule comparisonModule = new DialogueModule("Each kingdom has its own values. The Water Kingdom, for instance, cherishes adaptability and serenity, while the Earth Kingdom embodies stability and endurance.");
                                                                                comparisonModule.AddOption("What about the Air Kingdom?",
                                                                                    p8 => true,
                                                                                    p8 =>
                                                                                    {
                                                                                        DialogueModule airModule = new DialogueModule("The Air Kingdom values freedom and innovation, constantly seeking new horizons. They often clash with the Fire Kingdom, as their ideals are fundamentally opposed.");
                                                                                        airModule.AddOption("Fascinating! What else can you share?",
                                                                                            p9 => true,
                                                                                            p9 =>
                                                                                            {
                                                                                                p9.SendGump(new DialogueGump(p9, CreateGreetingModule()));
                                                                                            });
                                                                                        p8.SendGump(new DialogueGump(p8, airModule));
                                                                                    });
                                                                                p7.SendGump(new DialogueGump(p7, comparisonModule));
                                                                            });
                                                                        p6.SendGump(new DialogueGump(p6, fireKingdomModule));
                                                                    });
                                                                p5.SendGump(new DialogueGump(p5, lessonModule));
                                                            });
                                                        p4.SendGump(new DialogueGump(p4, successModule));
                                                    });
                                                p3.SendGump(new DialogueGump(p3, eventModule));
                                            });
                                        p2.SendGump(new DialogueGump(p2, motiveModule));
                                    });
                                player.SendGump(new DialogueGump(player, huntModule));
                            });
                        player.SendGump(new DialogueGump(player, pastModule));
                    });
                aboutModule.AddOption("I have other matters to attend to.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What can you tell me about the tides?",
            player => true,
            player =>
            {
                DialogueModule tidesModule = new DialogueModule("The ebb and flow of the tides are like the rhythm of life. When in harmony, health and serenity flourish.");
                tidesModule.AddOption("What about elemental balance?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, tidesModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => CanReward(player),
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.SendMessage("Here, for your earnest curiosity, take this token of appreciation.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("Tell me a tale.",
            player => true,
            player =>
            {
                DialogueModule talesModule = new DialogueModule("Ancient stories passed down through generations hold lessons and truths. If you can uncover one, the path forward may be illuminated.");
                talesModule.AddOption("Interesting, tell me more.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, talesModule));
            });

        return greeting;
    }

    private bool CanReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
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
