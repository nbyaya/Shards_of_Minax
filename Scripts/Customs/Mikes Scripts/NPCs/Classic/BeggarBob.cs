using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BeggarBob : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BeggarBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Beggar Bob";
        Body = 0x190; // Human male body

        // Stats
        SetStr(40);
        SetDex(30);
        SetInt(20);
        SetHits(45);

        // Appearance
        AddItem(new LongPants(902)); // Pants with hue 902
        AddItem(new Shirt(2500));    // Shirt with hue 2500
        AddItem(new Sandals(902));   // Sandals with hue 902

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;

        SpeechHue = 0; // Default speech hue
    }

    public BeggarBob(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, kind stranger. I am Beggar Bob. Life has not been kind to me, but perhaps you can spare a moment to listen to my story.");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule storyModule = new DialogueModule("Once, I was a merchant of rare artifacts, respected and wealthy. But a shipwreck off the Misty Isles took it all from me. Now, I find myself here, begging to survive.");
                storyModule.AddOption("What happened after the shipwreck?",
                    p => true,
                    p =>
                    {
                        DialogueModule hardshipModule = new DialogueModule("I was cast ashore, penniless and alone. Silverbrook, my home, is now nothing but ruins and memories. Fate can be cruel, but here I am, making the best of what little is left.");
                        hardshipModule.AddOption("That sounds difficult. How do you keep going?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule hopeModule = new DialogueModule("Kindness of strangers like you keeps me going. A warm meal or a kind word, they remind me of the good still left in the world. Even in these hard times, I find small joys - a sunny day, the laughter of children.");
                                hopeModule.AddOption("Your resilience is inspiring.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                hopeModule.AddOption("Perhaps I could offer some help.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("Your kindness means more than you know. If you truly wish to help, maybe you could gather a few supplies for me, or simply spread kindness to others in my name.");
                                        helpModule.AddOption("What supplies do you need?",
                                            pq => true,
                                            pq =>
                                            {
                                                p.SendMessage("Beggar Bob needs some basic supplies: a loaf of bread, some water, and a warm blanket.");
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        helpModule.AddOption("I will see what I can do.",
                                            pw => true,
                                            pw =>
                                            {
                                                p.SendMessage("You decide to help Beggar Bob in whatever way you can.");
                                            });
                                        helpModule.AddOption("I must be on my way.",
                                            pe => true,
                                            pe =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, helpModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, hopeModule));
                            });
                        hardshipModule.AddOption("I am sorry to hear that. Farewell, Bob.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, hardshipModule));
                    });
                storyModule.AddOption("It must have been hard to lose everything.",
                    p => true,
                    p =>
                    {
                        DialogueModule lossModule = new DialogueModule("Indeed, it has been a challenge. But I have learned to appreciate the small joys in life, like the kindness of strangers and the warmth of a sunny day.");
                        lossModule.AddOption("Stay strong, Bob.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        lossModule.AddOption("Do you ever wish for your old life back?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule longingModule = new DialogueModule("Of course I do. I remember the comforts, the respect, and the security. But dwelling on the past serves no purpose. Now, all I have is the present and the hope for a kinder future.");
                                longingModule.AddOption("Maybe I can help make your present a little better. Do you need anything?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule goldRequestModule = new DialogueModule("Well, if you're offering, a small donation of gold could certainly ease my burden. Even a single gold coin could mean the difference between a cold night and a warm meal.");
                                        goldRequestModule.AddOption("I can spare some gold for you.",
                                            plb => plb.Backpack.GetAmount(typeof(Gold)) >= 1,
                                            plb =>
                                            {
                                                int goldToGive = 10; // Give 10 gold
                                                plb.Backpack.ConsumeTotal(typeof(Gold), goldToGive);
                                                plb.SendMessage($"You hand Beggar Bob {goldToGive} gold coins.");
                                                DialogueModule gratitudeModule = new DialogueModule("Thank you, kind soul. Your generosity will not be forgotten. Every small act of kindness makes the world a better place.");
                                                gratitudeModule.AddOption("You are welcome, Bob. Stay strong.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, gratitudeModule));
                                            });
                                        goldRequestModule.AddOption("I cannot help right now.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, goldRequestModule));
                                    });
                                longingModule.AddOption("I understand. Stay strong, Bob.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, longingModule));
                            });
                        p.SendGump(new DialogueGump(p, lossModule));
                    });
                player.SendGump(new DialogueGump(player, storyModule));
            });

        greeting.AddOption("Can you tell me about humility?",
            player => true,
            player =>
            {
                DialogueModule humilityModule = new DialogueModule("Humility is the virtue of selflessness and modesty. I have little, but I strive to live a life that upholds this virtue. Do you understand humility, traveler?");
                humilityModule.AddOption("I think I do.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Beggar Bob nods approvingly.");
                    });
                humilityModule.AddOption("I could learn more.",
                    p => true,
                    p =>
                    {
                        DialogueModule learnMoreModule = new DialogueModule("To be humble is to think less of oneself and more of others. Selflessness can bring joy, even in times of great suffering.");
                        learnMoreModule.AddOption("Thank you for the wisdom.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, learnMoreModule));
                    });
                humilityModule.AddOption("How can I show humility?",
                    p => true,
                    p =>
                    {
                        DialogueModule showHumilityModule = new DialogueModule("One of the best ways to show humility is to help others in need without expecting anything in return. It can be as simple as giving food to the hungry, aiding a lost traveler, or even sharing your wealth with those less fortunate.");
                        showHumilityModule.AddOption("Perhaps I can share some of my gold with you.",
                            pl => pl.Backpack.GetAmount(typeof(Gold)) >= 5,
                            pl =>
                            {
                                int goldToGive = 5;
                                pl.Backpack.ConsumeTotal(typeof(Gold), goldToGive);
                                pl.SendMessage($"You hand Beggar Bob {goldToGive} gold coins.");
                                DialogueModule gratitudeModule = new DialogueModule("Your selflessness humbles me. You have shown true humility, traveler. Thank you for your kindness.");
                                gratitudeModule.AddOption("Glad I could help.",
                                    plc => true,
                                    plc =>
                                    {
                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, gratitudeModule));
                            });
                        showHumilityModule.AddOption("I am not able to give right now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, showHumilityModule));
                    });
                player.SendGump(new DialogueGump(player, humilityModule));
            });

        greeting.AddOption("Do you have anything for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, yes. For your kindness, take this small token. It is not much, but I hope it brings you a smile.");
                    rewardModule.AddOption("Thank you, Bob.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye, Bob.",
            player => true,
            player =>
            {
                player.SendMessage("Beggar Bob waves as you walk away.");
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