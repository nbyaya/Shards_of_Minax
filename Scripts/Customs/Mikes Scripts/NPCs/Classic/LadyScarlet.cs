using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Lady Scarlet")]
public class LadyScarlet : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LadyScarlet() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lady Scarlet";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(120);
        SetInt(80);
        SetHits(60);

        // Appearance
        AddItem(new StuddedLegs() { Hue = 1645 });
        AddItem(new StuddedChest() { Hue = 1645 });
        AddItem(new StuddedGorget() { Hue = 1645 });
        AddItem(new StuddedArms() { Hue = 1645 });
        AddItem(new Boots() { Hue = 1645 });
        AddItem(new Kryss() { Name = "Lady Scarlet's Dagger" });

        Hue = Utility.RandomSkinHue();
        HairItemID = Utility.RandomList(0x203B, 0x203C); // Hair styles
        HairHue = Utility.RandomHairHue();

        // Initialize lastRewardTime
        lastRewardTime = DateTime.MinValue;
    }

    public LadyScarlet(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Lady Scarlet, an assassin by trade. What do you seek, traveler?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("My health is none of your concern, but it's sufficient for my line of work. It's a balance, you see, between the art of survival and the weight of my choices.");
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I am an assassin, a master of shadows and death. Each life I take is a thread woven into the fabric of destiny. But tell me, do you understand the cost of such a path?");
                jobModule.AddOption("What do you mean by cost?",
                    p => true,
                    p =>
                    {
                        DialogueModule costModule = new DialogueModule("Every kill has consequences, a ripple that spreads through lives and fates. It's not merely a job; it's a burden I bear.");
                        p.SendGump(new DialogueGump(p, costModule));
                    });
                jobModule.AddOption("Is there honor in your work?",
                    p => true,
                    p =>
                    {
                        DialogueModule honorModule = new DialogueModule("Honor? In this line of work, it's a fleeting shadow. Some seek it in blood, others in silence. I find my honor in the skill of my craft.");
                        p.SendGump(new DialogueGump(p, honorModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Tell me about battles.",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Oh, you want to know about battles, do you? Each clash is a dance of death, a test of skill and will. But tell me, do you think one can truly find honor in killing for gold?");
                battlesModule.AddOption("What do you think about mercenaries?",
                    p => true,
                    p =>
                    {
                        DialogueModule mercenaryModule = new DialogueModule("Mercenaries are a necessary evil. They fight for coin and glory, yet they often lack the conviction that fuels true warriors. What drives you in battle?");
                        mercenaryModule.AddOption("I fight for my people.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("A noble cause indeed. Your people must see you as their protector.")));
                            });
                        mercenaryModule.AddOption("I seek personal glory.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the allure of fame! A tempting path, but one often filled with betrayal and envy.")));
                            });
                        p.SendGump(new DialogueGump(p, mercenaryModule));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("What can you tell me about shadows?",
            player => true,
            player =>
            {
                DialogueModule shadowsModule = new DialogueModule("The shadows are both my ally and my cloak. They hide my presence and hold my secrets. Do you wish to know one?");
                shadowsModule.AddOption("Yes, tell me a secret.",
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
                            p.SendMessage("Very well, lean closer and I'll share one. But remember, knowledge can be a double-edged sword. Take this!");
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                    });
                shadowsModule.AddOption("Can shadows betray you?",
                    p => true,
                    p =>
                    {
                        DialogueModule betrayalModule = new DialogueModule("Ah, betrayal is a constant threat in my world. Shadows can conceal foes just as easily as they hide allies. Trust is a rare commodity.");
                        p.SendGump(new DialogueGump(p, betrayalModule));
                    });
                player.SendGump(new DialogueGump(player, shadowsModule));
            });

        greeting.AddOption("Have you killed kings?",
            player => true,
            player =>
            {
                DialogueModule kingsModule = new DialogueModule("Ah, kings. They live in luxury while their subjects suffer. Yet, they are just as mortal as any other when they meet my blade. But do you see them as tyrants or rulers?");
                kingsModule.AddOption("Tyrants, without question.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("A fair view. Many rulers forget their duties to their people in pursuit of power.")));
                    });
                kingsModule.AddOption("Rulers, trying their best.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("A more forgiving perspective. But often, it's the people who pay the price for their decisions.")));
                    });
                player.SendGump(new DialogueGump(player, kingsModule));
            });

        greeting.AddOption("What is wisdom?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("True wisdom is understanding the value of a life. Not every target needs to be killed; sometimes, there are other ways. Have you ever had to choose between mercy and duty?");
                wisdomModule.AddOption("Yes, it’s a difficult choice.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Indeed, such moments shape us. They test our resolve and redefine our paths.")));
                    });
                wisdomModule.AddOption("No, I’ve never faced that.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Count yourself fortunate. The world is filled with hard choices.")));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Do you have any enemies?",
            player => true,
            player =>
            {
                DialogueModule enemiesModule = new DialogueModule("Enemies are everywhere. But the most dangerous one is the one you don't see coming. I’ve made my fair share. What about you? Do you have enemies?");
                enemiesModule.AddOption("Yes, many of them.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("They often hide in plain sight. Stay vigilant.")));
                    });
                enemiesModule.AddOption("No, I prefer to keep peace.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Wise choice. Sometimes peace is the hardest battle to win.")));
                    });
                player.SendGump(new DialogueGump(player, enemiesModule));
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
