using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EmperorJulius : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EmperorJulius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Emperor Julius";
        Body = 0x190; // Human male body

        // Stats
        SetStr(125);
        SetDex(85);
        SetInt(90);

        SetHits(85);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1122 });
        AddItem(new LeatherChest() { Hue = 1122 });
        AddItem(new LeatherGloves() { Hue = 1122 });
        AddItem(new LeatherCap() { Hue = 1122 });
        AddItem(new OrderShield() { Name = "Emperor Julius' Shield" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize the last reward time to a past date
    }

    public EmperorJulius(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Emperor Julius, ruler of these lands. What would you like to know?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Emperor Julius, ruler of these lands. My ancestors have governed this realm for generations, always striving for peace and prosperity.");
                identityModule.AddOption("Tell me about your ancestors.",
                    p => true,
                    p =>
                    {
                        DialogueModule ancestorsModule = new DialogueModule("My ancestors built this empire with dedication and valor. They fought wars, brokered peace, and laid the foundation for the prosperity you see today.");
                        ancestorsModule.AddOption("What wars did they fight?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule warsModule = new DialogueModule("There were many wars, but the most significant was the Battle of the Crimson Plains. It was a decisive conflict that unified the eastern tribes under our banner. My ancestor, Emperor Valerius, led the charge himself, wielding the legendary Blade of Unity.");
                                warsModule.AddOption("Tell me more about Emperor Valerius.",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule valeriusModule = new DialogueModule("Emperor Valerius was a visionary leader. He believed in the unity of all people under a single banner. His tactics were unmatched, and his diplomacy was as sharp as his blade. It was said that he could turn enemies into allies with a single conversation.");
                                        valeriusModule.AddOption("How did he manage to unify the tribes?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule unifyModule = new DialogueModule("Valerius offered the tribes something they had never known before: stability and prosperity. He promised fair representation and protection from external threats. His words, combined with his strength on the battlefield, convinced even the most skeptical leaders to join him.");
                                                unifyModule.AddOption("That must have been difficult.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, unifyModule));
                                            });
                                        valeriusModule.AddOption("Thank you for sharing.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, valeriusModule));
                                    });
                                warsModule.AddOption("That sounds fascinating.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, warsModule));
                            });
                        ancestorsModule.AddOption("What other achievements did they have?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule achievementsModule = new DialogueModule("My ancestors were not only warriors but also scholars and builders. They established the Great Library of Aurelia, a repository of knowledge gathered from across the realm. They also built aqueducts that brought fresh water to even the most remote villages.");
                                achievementsModule.AddOption("Tell me more about the Great Library.",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule libraryModule = new DialogueModule("The Great Library of Aurelia was founded by Emperor Cassius. He believed that knowledge was the key to a prosperous society. The library contains texts on philosophy, science, history, and even magic. Scholars from all over the world came to study there.");
                                        libraryModule.AddOption("Can anyone visit the library?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule visitModule = new DialogueModule("The library is open to all who seek knowledge. However, some of the more sensitive texts, particularly those on magic, are restricted and require special permission to access.");
                                                visitModule.AddOption("Why restrict some texts?",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule restrictModule = new DialogueModule("Knowledge is power, and power can be dangerous in the wrong hands. The restricted texts contain spells and secrets that could cause great harm if misused. Emperor Cassius believed that such knowledge should be protected.");
                                                        restrictModule.AddOption("That makes sense.",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                plllll.SendGump(new DialogueGump(plllll, CreateGreetingModule()));
                                                            });
                                                        pllll.SendGump(new DialogueGump(pllll, restrictModule));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, visitModule));
                                            });
                                        libraryModule.AddOption("Thank you for the information.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, libraryModule));
                                    });
                                achievementsModule.AddOption("That's impressive.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, achievementsModule));
                            });
                        ancestorsModule.AddOption("That's inspiring.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, ancestorsModule));
                    });
                identityModule.AddOption("Thank you, Emperor.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in good health, thanks for asking! It is my duty to remain strong for my people.");
                healthModule.AddOption("Glad to hear that.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your role here?",
            player => true,
            player =>
            {
                DialogueModule roleModule = new DialogueModule("My job is to maintain peace and order in this realm. The virtue of justice guides my decisions. Do you value justice?");
                roleModule.AddOption("Yes, I do.",
                    p => true,
                    p =>
                    {
                        DialogueModule justiceModule = new DialogueModule("Then you and I share a common value. Remember, in the balance of justice, all are equal.");
                        justiceModule.AddOption("I will remember that.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, justiceModule));
                    });
                roleModule.AddOption("Not particularly.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, roleModule));
            });

        greeting.AddOption("Can you tell me about these lands?",
            player => true,
            player =>
            {
                DialogueModule landsModule = new DialogueModule("These vast lands have been under the rule of my ancestors for generations. We've faced many challenges to keep them united.");
                landsModule.AddOption("What kind of challenges?",
                    p => true,
                    p =>
                    {
                        DialogueModule challengeModule = new DialogueModule("The realm is vast and diverse, with many cultures and challenges. My duty is to ensure all feel represented and heard.");
                        challengeModule.AddOption("Tell me about the different cultures.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule cultureModule = new DialogueModule("Our realm is home to many cultures, each with its own traditions and beliefs. The northern highlanders are known for their fierce independence and warrior spirit, while the coastal cities are centers of trade and culture, bustling with merchants from distant lands.");
                                cultureModule.AddOption("What about the highlanders?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule highlanderModule = new DialogueModule("The highlanders value strength and honor above all. They live in harsh, mountainous terrain and have adapted to survive in the most challenging conditions. Their loyalty to their clan is unmatched, and they are formidable allies in times of war.");
                                        highlanderModule.AddOption("They sound impressive.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, highlanderModule));
                                    });
                                cultureModule.AddOption("What about the coastal cities?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule coastalModule = new DialogueModule("The coastal cities are vibrant and diverse. They are the heart of our trade networks, with goods coming from across the seas. The people there are open-minded and inventive, always looking for new opportunities and ways to improve their lives.");
                                        coastalModule.AddOption("I would love to visit someday.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, coastalModule));
                                    });
                                cultureModule.AddOption("Thank you for sharing.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, cultureModule));
                            });
                        challengeModule.AddOption("That's a noble duty.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, challengeModule));
                    });
                player.SendGump(new DialogueGump(player, landsModule));
            });

        greeting.AddOption("Can I be rewarded for kindness?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Acts of kindness, no matter how small, have the power to change the world. As a token of my gratitude, take this reward for your concern.");
                    rewardModule.AddOption("Thank you, Emperor.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Emperor Julius nods solemnly as you depart.");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime); // Save last reward time
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime(); // Load last reward time
    }
}