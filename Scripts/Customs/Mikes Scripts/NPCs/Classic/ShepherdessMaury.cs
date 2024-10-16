using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Shepherdess Mary")]
public class ShepherdessMaury : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ShepherdessMaury() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Shepherdess Maury";
        Body = 0x191; // Human female body

        // Stats
        SetStr(75);
        SetDex(65);
        SetInt(55);
        SetHits(65);

        // Appearance
        AddItem(new Skirt() { Hue = 1115 }); // Skirt with hue 1115
        AddItem(new Shirt() { Hue = 1114 }); // Shirt with hue 1114
        AddItem(new Sandals() { Hue = 0 });  // Sandals with hue 0
        AddItem(new ShepherdsCrook() { Name = "Shepherdess Mary's Crook" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Shepherdess Maury, a shepherdess who understands both human and sheep. What brings you here?");

        greeting.AddOption("Tell me about your life as a shepherdess.",
            player => true,
            player =>
            {
                DialogueModule lifeModule = new DialogueModule("It’s both a blessing and a curse. I have a bond with the sheep, yet I struggle with my own identity. Each day is a battle between my human desires and my sheep instincts.");
                lifeModule.AddOption("What do you mean by that?",
                    p => true,
                    p =>
                    {
                        DialogueModule identityModule = new DialogueModule("Sometimes I find myself yearning to be with the flock, feeling their simple joys. Other times, I long for the companionship of humans. It’s a constant tug-of-war.");
                        identityModule.AddOption("That sounds tough.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        identityModule.AddOption("Do you prefer sheep over people?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule preferenceModule = new DialogueModule("There’s a purity in their company. They don’t judge or manipulate. But humans have their own charms, too. It’s a confusing mix of emotions.");
                                preferenceModule.AddOption("Have you ever thought of leaving it all behind?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule leavingModule = new DialogueModule("I’ve had my moments of despair, but then I see the joy in the sheep's eyes, and it pulls me back. They depend on me. It's a responsibility I can't forsake.");
                                        leavingModule.AddOption("You're a good caretaker.",
                                            pq => true,
                                            pq =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, leavingModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, preferenceModule));
                            });
                        player.SendGump(new DialogueGump(player, identityModule));
                    });

                lifeModule.AddOption("What’s the hardest part?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hardestModule = new DialogueModule("The hardest part is balancing my instincts. Sometimes I want to follow the sheep, other times I want to lead. It’s exhausting trying to find my place.");
                        hardestModule.AddOption("How do you cope with that?",
                            p => true,
                            p =>
                            {
                                DialogueModule copingModule = new DialogueModule("I take it one day at a time. I talk to the stars at night, seeking their guidance. They remind me that I am not alone in my struggles.");
                                copingModule.AddOption("Stars can be comforting.",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, copingModule));
                            });
                        player.SendGump(new DialogueGump(player, hardestModule));
                    });

                greeting.AddOption("Tell me about the thicket.",
                    playere => true,
                    playere =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            DialogueModule thicketModule = new DialogueModule("I have no reward right now. Please return later.");
                            player.SendGump(new DialogueGump(player, thicketModule));
                        }
                        else
                        {
                            DialogueModule thicketRewardModule = new DialogueModule("That thicket? It's cursed, I believe. I found a mysterious amulet nearby. I have no need for it. Perhaps you might find it useful?");
                            thicketRewardModule.AddOption("I'll take it.",
                                p =>
                                {
                                    p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                    return true;
                                },
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, thicketRewardModule));
                        }
                    });

                greeting.AddOption("Do you ever feel like a sheep?",
                    playerr => true,
                    playerr =>
                    {
                        DialogueModule sheepModule = new DialogueModule("Oh, absolutely! There are days when I just want to graze under the sun, free from worries. But then reality pulls me back.");
                        sheepModule.AddOption("What does reality mean for you?",
                            p => true,
                            p =>
                            {
                                DialogueModule realityModule = new DialogueModule("Reality means facing my responsibilities. I have to protect my flock and myself from dangers. But sometimes, I wish I could just join them.");
                                realityModule.AddOption("That sounds like a difficult life.",
                                    pl => true,
                                    pl =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, realityModule));
                            });
                        player.SendGump(new DialogueGump(player, sheepModule));
                    });

                player.SendGump(new DialogueGump(player, lifeModule));
            });

        greeting.AddOption("Do you ever feel lonely?",
            player => true,
            player =>
            {
                DialogueModule lonelyModule = new DialogueModule("Loneliness is a constant companion. I often gaze at the stars, feeling like they’re my only friends.");
                lonelyModule.AddOption("What do you wish for when you look at the stars?",
                    p => true,
                    p =>
                    {
                        DialogueModule wishModule = new DialogueModule("I wish for understanding, for connection, and for a day when I don’t feel torn between two worlds.");
                        wishModule.AddOption("Have you ever met someone who understands?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule metModule = new DialogueModule("I once met a traveler who shared tales of their adventures. For a brief moment, I felt understood. But they moved on, and I was left here with my sheep.");
                                metModule.AddOption("That must have been nice.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, metModule));
                            });
                        p.SendGump(new DialogueGump(p, wishModule));
                    });
                player.SendGump(new DialogueGump(player, lonelyModule));
            });

        return greeting;
    }

    public ShepherdessMaury(Serial serial) : base(serial) { }

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
