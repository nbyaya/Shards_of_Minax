using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Napoleon Bonaparte")]
public class NapoleonBone : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public NapoleonBone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Napoleon Bonaparte";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(100);
        SetInt(80);
        SetHits(70);

        // Appearance
        AddItem(new PlateLegs() { Hue = 1150 });
        AddItem(new PlateChest() { Hue = 1150 });
        AddItem(new PlateGloves() { Hue = 1150 });
        AddItem(new PlateHelm() { Hue = 1150 });
        AddItem(new Boots() { Hue = 1150 });
        AddItem(new Halberd() { Name = "Napoleon's Halberd" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("I am Napoleon Bonaparte, Emperor of France! How may I inspire you today?");
        
        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in the peak of health, just like my empire! My spirit is unwavering, ready to conquer new horizons.");
                healthModule.AddOption("That's good to hear.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job, you ask? I am a conqueror, a statesman, and a strategist! My reign is defined by the glory of victories and the wisdom of governance.");
                jobModule.AddOption("Sounds grand!",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Tell me about your ambitions.",
            player => true,
            player =>
            {
                DialogueModule ambitionModule = new DialogueModule("Do you have the ambition and cunning to achieve greatness? My ambitions know no bounds, just as my strategies are intricate and well-planned.");
                ambitionModule.AddOption("I do! What should I do?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule responseModule = new DialogueModule("Your response shows promise. Remember, ambition and cunning can shape destinies! Seek knowledge, build alliances, and never fear failure.");
                        responseModule.AddOption("I will keep that in mind.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, responseModule));
                    });
                player.SendGump(new DialogueGump(player, ambitionModule));
            });

        greeting.AddOption("What is your greatest achievement?",
            player => true,
            player =>
            {
                DialogueModule achievementModule = new DialogueModule("My greatest achievement? The establishment of the Napoleonic Code! A legal framework that brought order and clarity to our society. It was a true revolution in governance!");
                achievementModule.AddOption("That sounds impactful.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, achievementModule));
            });

        greeting.AddOption("What do you think about your legacy?",
            player => true,
            player =>
            {
                DialogueModule legacyModule = new DialogueModule("My legacy is built on the principles of the Revolution, my love for France, and the sacrifices of countless soldiers. Ensure my tales are passed down; they shall inspire future generations.");
                legacyModule.AddOption("I will remember your words.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, legacyModule));
            });

        greeting.AddOption("Tell me about your campaigns.",
            player => true,
            player =>
            {
                DialogueModule campaignsModule = new DialogueModule("Each of my campaigns has its own story, challenges, and lessons. If you study them closely, you may learn the art of war and leadership. Would you like to hear about a specific campaign?");
                campaignsModule.AddOption("Yes, tell me about the Campaign in Egypt.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule egyptModule = new DialogueModule("Ah, Egypt! A land of mysteries and treasures. My campaign there was not just about conquest but also about knowledge. I discovered the wonders of the ancient world and the rich culture of the Egyptians.");
                        egyptModule.AddOption("Fascinating! What did you learn?",
                            p => true,
                            p => 
                            {
                                DialogueModule learningModule = new DialogueModule("I learned the importance of understanding the culture of your enemies and the value of diplomacy. Knowledge is power, after all.");
                                learningModule.AddOption("I see! Thank you for sharing.",
                                    pla => true,
                                    pla => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                p.SendGump(new DialogueGump(p, learningModule));
                            });
                        pl.SendGump(new DialogueGump(pl, egyptModule));
                    });
                campaignsModule.AddOption("What about the Russian campaign?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule russiaModule = new DialogueModule("Ah, Russia... A vast land of mystery and challenge. My campaign there was one of the harshest experiences. The cold was relentless, but so was my determination. A lesson learned in the fragility of ambition.");
                        russiaModule.AddOption("That sounds harsh indeed.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, russiaModule));
                    });
                player.SendGump(new DialogueGump(player, campaignsModule));
            });

        greeting.AddOption("What do you think of your enemies?",
            player => true,
            player =>
            {
                DialogueModule enemiesModule = new DialogueModule("Enemies are merely obstacles in the path of greatness. Each opponent has something to teach us—whether it be through their strength or their folly.");
                enemiesModule.AddOption("Wise words, indeed.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, enemiesModule));
            });

        greeting.AddOption("Do you have a book to recommend?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                    cooldownModule.AddOption("I'll come back later.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule bookModule = new DialogueModule("Then you must read \"The Campaigns of Napoleon\". Here, take this. It's a rare edition. Use it wisely. Knowledge will be your greatest ally.");
                    bookModule.AddOption("Thank you!",
                        pl => true,
                        pl =>
                        {
                            pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, bookModule));
                }
            });

        greeting.AddOption("What are your thoughts on strategy?",
            player => true,
            player =>
            {
                DialogueModule strategyModule = new DialogueModule("Strategy is the soul of warfare. It requires foresight, adaptability, and the ability to predict your enemy's moves. A well-crafted strategy can turn the tide of battle.");
                strategyModule.AddOption("What makes a good strategist?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule goodStrategistModule = new DialogueModule("A good strategist must be observant, patient, and knowledgeable about both their own strengths and weaknesses as well as those of their adversaries.");
                        goodStrategistModule.AddOption("I'll remember that.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, goodStrategistModule));
                    });
                player.SendGump(new DialogueGump(player, strategyModule));
            });

        greeting.AddOption("Tell me about your ideals.",
            player => true,
            player =>
            {
                DialogueModule idealsModule = new DialogueModule("My ideals are rooted in the principles of the Revolution: liberty, equality, fraternity! I believe in the power of the people and their right to govern themselves.");
                idealsModule.AddOption("Those are powerful ideals.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, idealsModule));
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

    public NapoleonBone(Serial serial) : base(serial) { }
}
