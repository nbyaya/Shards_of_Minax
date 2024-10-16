using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Shadow Sam")]
public class ShadowSam : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ShadowSam() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Shadow Sam";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(80);
        SetInt(100);
        SetHits(90);

        // Appearance
        AddItem(new NinjaTabi() { Hue = 1175 });
        AddItem(new ShortPants() { Hue = 1109 });
        AddItem(new AssassinSpike());
        AddItem(new LeatherNinjaBelt() { Hue = 1109 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize reward time
    }

    public ShadowSam(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Shadow Sam, a rogue in the shadows!");

        greeting.AddOption("What types of people do you enjoy dealing with?",
            player => true,
            player =>
            {
                DialogueModule typesModule = new DialogueModule("Ah, you wish to know about my... preferred targets? It’s quite the interesting topic.");
                typesModule.AddOption("Yes, tell me more!",
                    p => true,
                    p =>
                    {
                        DialogueModule favoriteTypesModule = new DialogueModule("I find particular satisfaction in dealing with certain types. For instance, those who flaunt their wealth and power often make tempting targets.");
                        favoriteTypesModule.AddOption("Why wealthy individuals?",
                            pl => true,
                            pla =>
                            {
                                DialogueModule wealthyModule = new DialogueModule("Wealthy individuals often have more to lose. The thrill of slipping through their defenses and leaving them empty-handed is exhilarating.");
                                wealthyModule.AddOption("What about powerful people?",
                                    pq => true,
                                    pq =>
                                    {
                                        DialogueModule powerfulModule = new DialogueModule("Powerful individuals tend to be arrogant. Their overconfidence makes them easy prey. The real challenge is not just taking them down, but outsmarting them.");
                                        powerfulModule.AddOption("Do you enjoy the challenge?",
                                            plw => true,
                                            plw =>
                                            {
                                                DialogueModule challengeModule = new DialogueModule("Absolutely! The thrill of the hunt is what drives me. Each target is a puzzle waiting to be solved.");
                                                challengeModule.AddOption("What other types do you prefer?",
                                                    pe => true,
                                                    pe =>
                                                    {
                                                        DialogueModule otherTypesModule = new DialogueModule("Oh, I have a taste for those who seek to harm the innocent. Villains and thugs often cross my path.");
                                                        otherTypesModule.AddOption("Tell me about your encounters with villains.",
                                                            plar => true,
                                                            plar => 
                                                            {
                                                                DialogueModule villainsModule = new DialogueModule("I've had my fair share of encounters. The most memorable was a mercenary group terrorizing a village. Dealing with them was a pleasure.");
                                                                villainsModule.AddOption("How did you handle them?",
                                                                    pt => true,
                                                                    pt =>
                                                                    {
                                                                        DialogueModule handlingModule = new DialogueModule("I infiltrated their camp under cover of night, silently picking them off one by one. It’s almost an art form, you see.");
                                                                        handlingModule.AddOption("What about their leaders?",
                                                                            ply => true,
                                                                            ply =>
                                                                            {
                                                                                DialogueModule leadersModule = new DialogueModule("Taking down a leader sends a powerful message. It’s like cutting off the head of a serpent. Without it, the rest are lost.");
                                                                                pla.SendGump(new DialogueGump(pla, leadersModule));
                                                                            });
                                                                        p.SendGump(new DialogueGump(p, handlingModule));
                                                                    });
                                                                pla.SendGump(new DialogueGump(pla, villainsModule));
                                                            });
                                                        pla.SendGump(new DialogueGump(pla, otherTypesModule));
                                                    });
                                                p.SendGump(new DialogueGump(p, challengeModule));
                                            });
                                        p.SendGump(new DialogueGump(p, powerfulModule));
                                    });
                                p.SendGump(new DialogueGump(p, wealthyModule));
                            });
                        player.SendGump(new DialogueGump(player, favoriteTypesModule));
                    });
                greeting.AddOption("What about the innocent?",
                    playeru => true,
                    playeru =>
                    {
                        DialogueModule innocentModule = new DialogueModule("Ah, the innocent... I prefer to protect them, not harm them. It's the corrupt that need to fear my blade.");
                        innocentModule.AddOption("Do you ever regret your actions?",
                            pl => true,
                            pla =>
                            {
                                DialogueModule regretModule = new DialogueModule("Regret? Perhaps in some cases. But in the end, I follow my own code. The weak need a guardian in this cruel world.");
                                pla.SendGump(new DialogueGump(pla, regretModule));
                            });
                        player.SendGump(new DialogueGump(player, innocentModule));
                    });
                player.SendGump(new DialogueGump(player, typesModule));
            });

        greeting.AddOption("What about your favorite kill?",
            player => true,
            player =>
            {
                DialogueModule favoriteKillModule = new DialogueModule("My favorite kill? Ah, that would be the corrupt merchant of Yew. He exploited the poor, and I took great pleasure in seeing him brought low.");
                favoriteKillModule.AddOption("How did you plan that one?",
                    pl => true,
                    pla =>
                    {
                        DialogueModule planModule = new DialogueModule("I spent weeks watching him, learning his routines, and identifying his weaknesses. It was all about timing and precision.");
                        planModule.AddOption("Was it worth the effort?",
                            p => true,
                            p =>
                            {
                                DialogueModule worthModule = new DialogueModule("Every ounce of effort was worth it. The satisfaction of seeing justice served is unmatched.");
                                worthModule.AddOption("Do you think about your targets often?",
                                    plp => true,
                                    plp =>
                                    {
                                        DialogueModule thoughtModule = new DialogueModule("Occasionally, yes. Each target teaches me something new. It's the nature of the hunt.");
                                        pla.SendGump(new DialogueGump(pla, thoughtModule));
                                    });
                                p.SendGump(new DialogueGump(p, worthModule));
                            });
                        pla.SendGump(new DialogueGump(pla, planModule));
                    });
                player.SendGump(new DialogueGump(player, favoriteKillModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => CanGiveReward(player),
            player =>
            {
                GiveReward(player);
            });

        return greeting;
    }

    private bool CanGiveReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveReward(PlayerMobile player)
    {
        if (CanGiveReward(player))
        {
            Say("Ah, you've proven yourself brave. Here's a little something for your troubles.");
            player.AddToBackpack(new MaxxiaScroll());
            lastRewardTime = DateTime.UtcNow;
        }
        else
        {
            Say("I have no reward right now. Please return later.");
        }
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
