using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TaliaTheStargazer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TaliaTheStargazer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Talia the Stargazer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyDress(1150)); // A shimmering blue dress
        AddItem(new Sandals(1153)); // Light blue sandals
        AddItem(new HalfApron(1157)); // Star-patterned apron
        AddItem(new WizardsHat(1175)); // A wizard's hat with a celestial motif

        VirtualArmor = 8;
    }

    public TaliaTheStargazer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Talia, a stargazer fascinated by the cosmos and its mysteries. Once, I served as a soldier in a pre-war army, but now I wander as a mercenary, selling my skills to those who can afford them. The stars, however, provide some respite from my troubled past. Tell me, do you share a curiosity for the stars?");

        // Start with some general dialogue about her work
        greeting.AddOption("Tell me about your past. You mentioned being a soldier.",
            p => true,
            p =>
            {
                DialogueModule pastModule = new DialogueModule("Yes, I was once a soldier, fighting in wars that have long since lost their meaning. I served my nation, believing in a cause, but those days left me haunted by the lives I took. When the war ended, I found myself without purpose. Now, I work as a mercenary. The stars are my only solace, the only constant that reminds me there's more to this existence than bloodshed.");

                pastModule.AddOption("Do you feel regret for the things you've done?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule regretModule = new DialogueModule("Regret? It is not so simple. I have done terrible things, things I cannot forget. The faces of those who fell by my hand still visit me in my dreams. I have learned to be stoic, to bury those emotions deep within, but they surface in the quiet of the night. The stars help me keep my focus, reminding me that I am but a small part of a vast universe.");

                        regretModule.AddOption("How do you cope with the guilt?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule copingModule = new DialogueModule("I cope by surviving, by being resourceful. The war taught me to be ruthless, to make hard decisions that others shy away from. I use my skills to help those who can pay, but I also hope that, in some way, I can use my abilities to make a difference, perhaps to atone for my past. There is no easy path to redemption, only survival and small acts that might one day outweigh the sins.");

                                copingModule.AddOption("Do you believe you can find redemption?",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule redemptionModule = new DialogueModule("Redemption... I do not know if it is possible for someone like me. I was a soldier, and now a mercenary—my hands are stained with the blood of too many. I try to help where I can, to trade my skills for something more than gold, but there are nights when I think it will never be enough. The stars give me hope, however small, that there is still a chance.");

                                        redemptionModule.AddOption("Perhaps helping others is the first step.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                plaabc.SendMessage("Talia gives a faint smile, her eyes glistening with unshed tears. 'Perhaps you are right. Perhaps it is not too late.'");
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        redemptionModule.AddOption("Maybe some sins can never be forgiven.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                plaabc.SendMessage("Talia looks down, her expression hardening. 'You may be right. But as long as I draw breath, I will try to find my way.'");
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, redemptionModule));
                                    });
                                copingModule.AddOption("I understand. Survival is all we have sometimes.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia nods, her gaze distant. 'Indeed. Survival is often the only thing left, but it does not have to be devoid of meaning.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, copingModule));
                            });
                        regretModule.AddOption("That must be difficult to live with.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Talia looks up at the stars, her expression softening. 'It is. But I have chosen this path. I have to believe that one day, I will find peace.'");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, regretModule));
                    });

                pastModule.AddOption("Why become a mercenary after all that?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule mercenaryModule = new DialogueModule("The war left me with skills and nothing else. I was a soldier without a war, a weapon without a purpose. Becoming a mercenary was the only way I knew to survive. It allowed me to use my skills for something, even if it was just gold. Over time, I found ways to make my contracts mean more, to protect those who could not protect themselves, but I still sell my skills to the highest bidder. It is not a noble path, but it is my path.");

                        mercenaryModule.AddOption("Do you ever turn down contracts?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule contractModule = new DialogueModule("I do. I am ruthless, but I am not without principles. There are lines I will not cross, jobs I will not take. If a contract requires harm to the innocent or serves a cause I find abhorrent, I refuse it. I may be haunted by my past, but I will not willingly add to the burden if I can help it.");

                                contractModule.AddOption("That sounds like a difficult balance.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia nods solemnly. 'It is. But it is the only way I know to live with myself.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                contractModule.AddOption("You still have some honor left, then.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia gives a small, rueful smile. 'Perhaps. Or perhaps it is just a way to ease my conscience.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                pla.SendGump(new DialogueGump(pla, contractModule));
                            });
                        mercenaryModule.AddOption("Do you regret choosing this path?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule regretChoiceModule = new DialogueModule("Regret is a constant companion. But what choice did I have? The war ended, and I was left adrift. Becoming a mercenary was the only way I knew to continue. It allowed me to stay sharp, to remain useful. I regret the lives I've taken, but I do not regret surviving.");
                                regretChoiceModule.AddOption("Survival is a form of strength.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia nods, her gaze intense. 'Yes. And sometimes, it is the only strength we have.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                regretChoiceModule.AddOption("We all have to make our own way.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia sighs softly. 'Indeed. And we must live with the consequences of our choices.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                pla.SendGump(new DialogueGump(pla, regretChoiceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, mercenaryModule));
                    });
                p.SendGump(new DialogueGump(p, pastModule));
            });

        // General stargazing dialogue
        greeting.AddOption("Tell me about the stars.",
            p => true,
            p =>
            {
                DialogueModule starsModule = new DialogueModule("The stars are ancient and wise, untouched by the troubles of men. They watch over us, indifferent yet constant. When I look at them, I am reminded of how small we are, and how fleeting our troubles must be in the grand scheme of the universe.");

                starsModule.AddOption("Do you believe the stars hold answers?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule answersModule = new DialogueModule("I believe they hold answers, but not the kind we are often looking for. The stars do not speak directly, but they can guide us, remind us of our place, and give us the strength to carry on. They are a symbol of hope, distant and untouchable, yet always present.");

                        answersModule.AddOption("That's a comforting thought.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Talia smiles faintly. 'Yes, it is. Perhaps that is why I find myself drawn to them, despite everything.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        answersModule.AddOption("It sounds lonely.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Talia's eyes darken. 'It is. But sometimes, solitude is a necessary price to pay for peace.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, answersModule));
                    });
                starsModule.AddOption("Do you use the stars in your work?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule workModule = new DialogueModule("Yes. The stars can guide us, and those who understand their movements can predict much about the world. The AstroLabe, for instance, is a tool that allows one to harness the power of the stars, to understand their language. It is a device that speaks of the cosmos' secrets.");

                        workModule.AddOption("Can you teach me to use an AstroLabe?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule teachModule = new DialogueModule("It takes years of practice to truly understand the AstroLabe, but I can give you a starting point. The key is to understand the constellations and their significance. The AstroLabe aligns with the stars, and through it, you can gain insight into events yet to come, or places hidden to the naked eye.");

                                teachModule.AddOption("Thank you for the guidance.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia nods. 'May the stars guide your path, traveler.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                teachModule.AddOption("That sounds complicated.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Talia smiles gently. 'It is. But nothing worth learning is ever easy.'");
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                pla.SendGump(new DialogueGump(pla, teachModule));
                            });
                        pl.SendGump(new DialogueGump(pl, workModule));
                    });
                p.SendGump(new DialogueGump(p, starsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Talia smiles warmly as she turns her gaze back to the stars.");
            });

        return greeting;
    }

    private bool HasAstroLabe(PlayerMobile player)
    {
        // Check the player's inventory for AstroLabe
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AstroLabe)) != null;
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
        // Remove the AstroLabe and give the ExoticPlum and MaxxiaScroll, then set the cooldown timer
        Item astroLabe = player.Backpack.FindItemByType(typeof(AstroLabe));
        if (astroLabe != null)
        {
            astroLabe.Delete();
            player.AddToBackpack(new ExoticPlum());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the AstroLabe and receive an ExoticPlum and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have an AstroLabe.");
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