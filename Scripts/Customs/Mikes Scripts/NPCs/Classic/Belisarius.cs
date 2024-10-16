using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class Belisarius : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Belisarius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Belisarius";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(100);
        SetInt(60);

        SetHits(80);
        
        // Appearance
        AddItem(new PlateLegs() { Hue = 1153 });
        AddItem(new PlateChest() { Hue = 1153 });
        AddItem(new PlateGloves() { Hue = 1153 });
        AddItem(new PlateHelm() { Hue = 1153 });
        AddItem(new Boots() { Hue = 1153 });
        AddItem(new Halberd() { Name = "Belisarius's Halberd" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
    }

    public Belisarius(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Belisarius of Byzantium, a wanderer of time and space. How may I assist you?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Belisarius, a seeker of ancient mysteries and keeper of forgotten knowledge. I also served as a general in the campaigns to restore the Roman Empire to its former glory.");
                identityModule.AddOption("Tell me about your military campaigns.",
                    p => true,
                    p =>
                    {
                        DialogueModule campaignModule = new DialogueModule("I served under Emperor Justinian, striving to reclaim the lost territories of the Western Roman Empire. My campaigns took me from Africa to Italy, and even to the edges of the known world.");
                        campaignModule.AddOption("Tell me about the Vandalic War.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule vandalicWarModule = new DialogueModule("The Vandalic War was our first great endeavor. We set sail for Africa to face the Vandals, who had claimed the region after the fall of Rome. The journey was treacherous, but we landed near Carthage. We faced King Gelimer in battle and emerged victorious, reclaiming Africa for the Empire.");
                                vandalicWarModule.AddOption("How did you defeat the Vandals?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule defeatVandalsModule = new DialogueModule("Our victory was due to strategy and discipline. We surprised the Vandals, who were unprepared for our rapid march. We fought with courage, and the morale of the Vandals shattered as we pressed forward. Gelimer fled, and eventually, he surrendered, allowing us to restore Roman rule.");
                                        defeatVandalsModule.AddOption("What happened to Gelimer?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule gelimerModule = new DialogueModule("Gelimer was captured and brought before Emperor Justinian. He was treated with mercy and granted land in retirement. It was a reminder that even enemies could be treated with honor once defeated.");
                                                gelimerModule.AddOption("That's fascinating. Tell me more about your other campaigns.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, gelimerModule));
                                            });
                                        defeatVandalsModule.AddOption("Thank you for sharing. What came next?",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, defeatVandalsModule));
                                    });
                                vandalicWarModule.AddOption("What was the aftermath of the war?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule aftermathModule = new DialogueModule("After our victory, we worked to stabilize the region. We restored Roman law and rebuilt what had been destroyed. It was a time of hope, but also a time of great effort, as the scars of conflict ran deep.");
                                        aftermathModule.AddOption("Did the people accept Roman rule again?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule acceptanceModule = new DialogueModule("Most of the people were weary of war and accepted Roman rule in exchange for peace. However, there were still those who resisted, and it took time to fully integrate Africa back into the Empire.");
                                                acceptanceModule.AddOption("I see. Tell me about Italy.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, acceptanceModule));
                                            });
                                        aftermathModule.AddOption("It must have been challenging.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, aftermathModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, vandalicWarModule));
                            });
                        campaignModule.AddOption("Tell me about the Gothic War.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule gothicWarModule = new DialogueModule("The Gothic War was a long and arduous campaign to reclaim Italy from the Ostrogoths. We faced many battles, sieges, and hardships, but we fought on to bring Italy back under Roman control.");
                                gothicWarModule.AddOption("What was the most difficult part of the campaign?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule difficultModule = new DialogueModule("The siege of Rome was perhaps the most challenging. The city, once the heart of the Empire, had become a battleground. Supplies were scarce, and the Gothic forces were relentless. We held on through sheer determination and eventually broke the siege.");
                                        difficultModule.AddOption("How did you manage to break the siege?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule breakSiegeModule = new DialogueModule("It was through cunning and persistence. We used every tactic available: sorties, deception, and alliances with local forces. Eventually, the Goths grew weary and their morale faltered. We struck at the right moment, forcing them to lift the siege.");
                                                breakSiegeModule.AddOption("Amazing. What happened after Rome was freed?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule afterRomeModule = new DialogueModule("After the siege, we pursued the Gothic forces across Italy. It was a relentless pursuit, but we knew that to secure the peninsula, we had to defeat them completely. Battles were fought, cities were taken, and eventually, the Goths were defeated.");
                                                        afterRomeModule.AddOption("It must have been exhausting.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, afterRomeModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, breakSiegeModule));
                                            });
                                        difficultModule.AddOption("Thank you for sharing this story.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, difficultModule));
                                    });
                                gothicWarModule.AddOption("What was the outcome of the Gothic War?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule outcomeModule = new DialogueModule("In the end, we were victorious, but the cost was high. Italy was devastated, and the people were weary. Though we reclaimed the land, it was a bittersweet victory. The once-great cities were in ruins, and rebuilding would take many years.");
                                        outcomeModule.AddOption("It sounds like a hard-fought victory.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, outcomeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, gothicWarModule));
                            });
                        campaignModule.AddOption("Did you ever face betrayal during your campaigns?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule betrayalModule = new DialogueModule("Yes, betrayal was an ever-present danger. There were those who sought power for themselves, even at the cost of the Empire. Allies turned against us, and plots were hatched in secret. But through vigilance and loyalty to the Emperor, we overcame these challenges.");
                                betrayalModule.AddOption("How did you handle betrayal?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule handleBetrayalModule = new DialogueModule("Handling betrayal required both strength and wisdom. Some had to be confronted directly, while others were swayed back to our cause through diplomacy. Trust was fragile, and each decision had to be weighed carefully.");
                                        handleBetrayalModule.AddOption("It must have been difficult.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, handleBetrayalModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, betrayalModule));
                            });
                        campaignModule.AddOption("Thank you for sharing your stories.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, campaignModule));
                    });
                identityModule.AddOption("Farewell.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Belisarius nods solemnly.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Do you have a riddle for me?",
            player => true,
            player =>
            {
                DialogueModule riddleModule = new DialogueModule("If you seek knowledge, you must answer this riddle: What is the beginning of eternity, the end of time and space, the beginning of every end, and the end of every race?");
                riddleModule.AddOption("The answer is 'E'.",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            p.SendMessage("Ah, the answer is correct. You have a sharp mind. For your wisdom, I grant you this gift.");
                            p.AddToBackpack(new VelocityDeed());
                            lastRewardTime = DateTime.UtcNow;
                        }
                    });
                riddleModule.AddOption("I do not know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, riddleModule));
            });

        greeting.AddOption("Tell me about fate.",
            player => true,
            player =>
            {
                DialogueModule fateModule = new DialogueModule("Fate is the weaver of destinies, a force that binds us all. But with knowledge, one can influence the threads of fate.");
                fateModule.AddOption("How can I influence fate?",
                    p => true,
                    p =>
                    {
                        DialogueModule influenceModule = new DialogueModule("Through understanding, wisdom, and courage, one can change the course of destiny. But be warned, such power comes at a cost.");
                        influenceModule.AddOption("I understand. Thank you.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, influenceModule));
                    });
                fateModule.AddOption("Perhaps fate should be left alone.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, fateModule));
            });

        greeting.AddOption("Farewell.",
            player => true,
            player =>
            {
                player.SendMessage("Belisarius nods solemnly at you.");
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