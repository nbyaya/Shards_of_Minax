using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class JasperTheGambler : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public JasperTheGambler() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jasper the Gambler";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(80);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new FancyShirt(1150)); // A striking red fancy shirt
        AddItem(new LongPants(1109)); // Dark pants to add some style
        AddItem(new Boots(1175)); // Some dark boots
        AddItem(new Bandana(1153)); // A bright blue bandana
        AddItem(new UnluckyDice(Utility.RandomMinMax(0, 3))); // A gambler wouldn't be without his dice!

        VirtualArmor = 15;
    }

    public JasperTheGambler(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule(player);
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule(PlayerMobile player)
    {
        DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! They call me Jasper, a man of luck, fate, and chance. But there is more to me than meets the eye. Are you willing to take a gamble today?");

        // Introduction to Jasper's world view
        greeting.AddOption("Why do you speak of fate and chance?", 
            p => true, 
            p =>
            {
                DialogueModule fateModule = new DialogueModule("Ah, fate is a curious thing, isn't it? Some call it luck, but I know better. Fate is the hand that guides us, the force that chooses who survives the flames and who perishes. I have seen things, friend, felt things, that most would not believe.");
                fateModule.AddOption("What have you seen?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule visionsModule = new DialogueModule("Visions, whispers in the night, prophecies of a world reborn in fire. You see, I am not just a gambler. I am a messenger of the inevitable, a leader of those chosen to survive the cleansing fire that will soon come upon us all.");
                        visionsModule.AddOption("A world reborn in fire?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule fireModule = new DialogueModule("Yes, the apocalypse—a cleansing fire that will purge the corruption of this world. The weak, the sinful, the unworthy will be burned away, and only those with the strength to see the truth will survive. My followers and I, we are the chosen, destined to rebuild this world in our image.");
                                fireModule.AddOption("Who are your followers?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule followersModule = new DialogueModule("We are few, but we are dedicated. We call ourselves the Ember Circle. We worship the fire, the force that will cleanse this world. Each of us knows our role in the coming days, and each of us has accepted our fate. The fire will come, and we will be ready.");
                                        followersModule.AddOption("What do you do as the leader?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule leaderModule = new DialogueModule("As the leader, I guide the faithful. I prepare them for what is to come, teach them to embrace the flames rather than fear them. It is a heavy burden, knowing the truth, but it is also a gift. The fire speaks to me, and I share its message.");
                                                leaderModule.AddOption("What is the message?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule messageModule = new DialogueModule("The message is simple: the old world is corrupt beyond saving. It must be burned so that a new one may rise. Only those who accept this truth, who are willing to sacrifice everything, will be worthy of the new world. The rest will be ash.");
                                                        messageModule.AddOption("How can I be worthy?", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule worthyModule = new DialogueModule("To be worthy, you must embrace the fire. You must cast aside fear and doubt, and trust in the cleansing flames. My followers and I, we have rituals, practices that prepare us for what is to come. Perhaps, if you prove yourself, you could join us.");
                                                                worthyModule.AddOption("What sort of rituals?", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule ritualModule = new DialogueModule("Ah, the rituals are not for the faint of heart. We test our devotion through trials of endurance, pain, and sacrifice. We cleanse our minds and bodies, preparing for the day when the fire will come. One of our most sacred rituals involves the emberstone—a relic said to hold the power of the flames.");
                                                                        ritualModule.AddOption("Tell me about the emberstone.", 
                                                                            plaaaaaaa => true, 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                DialogueModule emberstoneModule = new DialogueModule("The emberstone is a sacred artifact, passed down through generations of the faithful. It is said to contain a fragment of the cleansing fire itself. To touch it is to feel the power of the apocalypse, to know what is to come. Only the truly devoted are allowed to hold it, for it can burn the unworthy.");
                                                                                emberstoneModule.AddOption("Can I see it?", 
                                                                                    plaaaaaaaa => true, 
                                                                                    plaaaaaaaa =>
                                                                                    {
                                                                                        DialogueModule seeEmberstoneModule = new DialogueModule("In time, perhaps. First, you must prove your devotion. Bring me a WatermelonSliced, and I shall consider allowing you to see the emberstone. It may seem trivial, but every act of faith, no matter how small, brings you closer to the truth.");
                                                                                        seeEmberstoneModule.AddOption("I have a WatermelonSliced.", 
                                                                                            plaaaaaaaaa => HasWatermelonSliced(plaaaaaaaaa) && CanTradeWithPlayer(plaaaaaaaaa), 
                                                                                            plaaaaaaaaa =>
                                                                                            {
                                                                                                CompleteTrade(plaaaaaaaaa);
                                                                                            });
                                                                                        seeEmberstoneModule.AddOption("I don't have one right now.", 
                                                                                            plaaaaaaaaa => !HasWatermelonSliced(plaaaaaaaaa), 
                                                                                            plaaaaaaaaa =>
                                                                                            {
                                                                                                plaaaaaaaaa.SendMessage("No WatermelonSliced? Well, faith requires patience. Return when you have one, and we shall speak more.");
                                                                                                plaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaa, CreateGreetingModule(plaaaaaaaaa)));
                                                                                            });
                                                                                        seeEmberstoneModule.AddOption("I traded recently; I'll come back later.", 
                                                                                            plaaaaaaaaa => !CanTradeWithPlayer(plaaaaaaaaa), 
                                                                                            plaaaaaaaaa =>
                                                                                            {
                                                                                                plaaaaaaaaa.SendMessage("You must wait before we can trade again. Patience, traveler. The fire rewards those who are steadfast.");
                                                                                                plaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaa, CreateGreetingModule(plaaaaaaaaa)));
                                                                                            });
                                                                                        plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, seeEmberstoneModule));
                                                                                    });
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, emberstoneModule));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, ritualModule));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, worthyModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, messageModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, leaderModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, followersModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, fireModule));
                            });
                        pl.SendGump(new DialogueGump(pl, visionsModule));
                    });
                p.SendGump(new DialogueGump(p, fateModule));
            });

        // Additional Option: Ask about his past
        greeting.AddOption("Who were you before all of this?", 
            p => true, 
            p =>
            {
                DialogueModule pastModule = new DialogueModule("Before I was Jasper the Gambler, I was simply Jasper—a man lost in the chaos of this world. I wandered, seeking meaning, seeking purpose. I found none in the greed and corruption around me. It wasn't until I heard the whispers of the fire that I understood my true calling.");
                pastModule.AddOption("What changed you?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule changeModule = new DialogueModule("The world is broken, traveler. I saw the corruption, the decay, and I knew that it could not continue. I sought answers, and I found them in the flames. They spoke to me, showed me visions of a world purified. That is when I became who I am today—a leader, a guide, a harbinger of what must come.");
                        changeModule.AddOption("Do you regret anything?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule regretModule = new DialogueModule("Regret? No. The path I walk is not easy, but it is necessary. The old world must be destroyed, and someone must lead the chosen through the fire. I have sacrificed much, but it is worth it. When the flames come, we will be ready.");
                                regretModule.AddOption("I see. Thank you for sharing.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, regretModule));
                            });
                        changeModule.AddOption("That sounds difficult.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Difficult, yes. But the fire gives me strength.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, changeModule));
                    });
                p.SendGump(new DialogueGump(p, pastModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Jasper nods, smiling slyly. 'May fortune favor you, traveler. And remember, the fire watches.'");
            });

        return greeting;
    }

    private bool HasWatermelonSliced(PlayerMobile player)
    {
        // Check the player's inventory for WatermelonSliced
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WatermelonSliced)) != null;
    }

    private bool CanTradeWithPlayer(PlayerMobile player)
    {
        // Check if the player can trade, based on the 10-minute cooldown
        if (LastTradeTime.TryGetValue(player, out DateTime lastTrade))
        {
            return (DateTime.UtcNow - lastTrade).TotalMinutes >= 10;
        }
        return true;
    }

    private void CompleteTrade(PlayerMobile player)
    {
        // Remove the WatermelonSliced and give the UnluckyDice and MaxxiaScroll, then set the cooldown timer
        Item watermelon = player.Backpack.FindItemByType(typeof(WatermelonSliced));
        if (watermelon != null)
        {
            watermelon.Delete();
            player.AddToBackpack(new UnluckyDice());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the WatermelonSliced and receive an UnluckyDice and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a WatermelonSliced.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}