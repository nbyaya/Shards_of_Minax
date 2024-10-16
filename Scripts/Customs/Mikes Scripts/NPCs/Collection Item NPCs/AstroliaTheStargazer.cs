using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AstroliaTheStargazer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AstroliaTheStargazer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Astrolia the Stargazer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(100);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1175)); // Dark blue robe, representing the night sky
        AddItem(new Sandals(1150)); // Light blue sandals
        AddItem(new WizardsHat(1153)); // Wizard hat in a matching blue
        AddItem(new DaggerSign()); // Custom item for the stargazing theme

        VirtualArmor = 10;
    }

    public AstroliaTheStargazer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a fellow seeker of the stars! My name is Astrolia. But I am more than that. Some call me 'mutant', others call me 'freak.' But I see myself as something more — a beacon for change in a world reluctant to accept what it does not understand.");

        // Start with an introduction and offer detailed topics
        greeting.AddOption("Tell me about yourself, Astrolia.", 
            p => true, 
            p =>
            {
                DialogueModule selfStoryModule = new DialogueModule("I am one of the Unseen. Mutants, we are called, though I consider myself a child of the stars. The world below shuns us, but I believe in the dawn of a new era — one where my kind are no longer feared. I fight not only for my people, but for a world where we all belong.");

                // Nested deeper dialogues about her past
                selfStoryModule.AddOption("What makes you different from others?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule differenceModule = new DialogueModule("I was born with gifts. Some call them curses. My body bears the marks of the cosmos — strange, alien, but filled with power. This difference, however, brings fear in those around me. It has always been that way. But I know deep inside that we are meant for more.");

                        differenceModule.AddOption("Why do people fear you?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule fearModule = new DialogueModule("Fear is the shadow of ignorance. They do not understand what we are, so they assume we are dangerous. People fear what they cannot explain, but I have found that fear can be softened by compassion. It is not their fault, but it is my mission to change their hearts.");
                                fearModule.AddOption("How do you stay hopeful despite the hostility?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule hopeModule = new DialogueModule("Hope is my guiding star. If we give up, then we lose everything. My tenacity comes from the belief that we are on the verge of something beautiful, a world that will finally embrace all who inhabit it. My kind deserve to stand under the same sky without fear or shame.");
                                        hopeModule.AddOption("Your resilience is inspiring.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, hopeModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, fearModule));
                            });

                        differenceModule.AddOption("You must have faced many challenges.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule challengeModule = new DialogueModule("Oh, many indeed. The distrust, the hostility — it weighs heavy on me at times. But I do not falter. The stars themselves are not easily dimmed, and neither am I. When the world rejects you, you have two choices: retreat into darkness, or rise up and shine even brighter.");
                                challengeModule.AddOption("You chose to shine.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, challengeModule));
                            });

                        pl.SendGump(new DialogueGump(pl, differenceModule));
                    });

                selfStoryModule.AddOption("Tell me about your people, the Unseen.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule unseenModule = new DialogueModule("The Unseen, the Outcasts, the Forgotten... we have many names, none of them kind. We are mutants, yes, born different, but we are just as much a part of this world as anyone else. We seek nothing more than a place to call our own — a place where our gifts are celebrated, not feared.");
                        
                        unseenModule.AddOption("What are your gifts?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule giftsModule = new DialogueModule("My gifts are as much a mystery to me as they are to others. I am able to sense the movements of the stars and predict celestial events. I also possess a connection to the energy around me, allowing me to see what others cannot. But these gifts come at a cost — isolation.");
                                giftsModule.AddOption("Do you ever wish to be normal?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule normalModule = new DialogueModule("I used to wish for that. To blend in, to live without the burden of being different. But over time, I realized that 'normal' is just another word for 'comfortable.' Growth, change, evolution — these come from discomfort. I no longer seek to be normal. I seek to be free.");
                                        normalModule.AddOption("That’s a powerful mindset.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, normalModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, giftsModule));
                            });
                        
                        unseenModule.AddOption("Do your people fight back?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule fightBackModule = new DialogueModule("Some of us do, yes. But violence is not my way. I fight with my voice, with my actions, with compassion. There are others, however, who have been pushed to the edge, who see no other path than to retaliate against those who wrong us. I fear that if the world does not change, more of us will fall into darkness.");
                                fightBackModule.AddOption("I hope they find peace.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, fightBackModule));
                            });

                        pl.SendGump(new DialogueGump(pl, unseenModule));
                    });

                p.SendGump(new DialogueGump(p, selfStoryModule));
            });

        // Stargazing and astronomy-related topics
        greeting.AddOption("Tell me about the stars you study.", 
            p => true, 
            p =>
            {
                DialogueModule starsModule = new DialogueModule("The stars are my sanctuary. Each one is a point of light in the darkness, a reminder that even in the vastness of space, we are never truly alone. They guide me, just as they have guided countless others throughout time.");

                starsModule.AddOption("What do the stars say about the future?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule futureModule = new DialogueModule("The future is a tapestry of possibility. The stars suggest that change is coming, a great upheaval that will challenge the old ways. Whether this change will be for good or ill depends on those of us who still have hope, who still believe in a brighter tomorrow.");
                        futureModule.AddOption("Do you believe in a bright future?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule believeModule = new DialogueModule("I do. I must. If I lose hope, then everything I fight for means nothing. The stars have shown me that we are on the cusp of something new, something better. But we must be patient, and we must be strong.");
                                believeModule.AddOption("That gives me hope too.", 
                                    plaaa => true, 
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, believeModule));
                            });
                        pl.SendGump(new DialogueGump(pl, futureModule));
                    });

                starsModule.AddOption("Can the stars guide me?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule guideModule = new DialogueModule("They can, if you are willing to listen. Each constellation tells a story, and within those stories are lessons for us all. Whether you seek wisdom, strength, or clarity, the stars will provide it if you watch and learn.");
                        guideModule.AddOption("I would like to learn more.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, guideModule));
                    });

                p.SendGump(new DialogueGump(p, starsModule));
            });

        // Trade interaction
        greeting.AddOption("Do you need anything?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am searching for an AmatureTelescope. If you bring me one, I will offer you a DaggerSign and a MaxxiaScroll as a reward. However, I can only make the trade every 10 minutes.");

                tradeIntroductionModule.AddOption("I have an AmatureTelescope.", 
                    pl => HasAmatureTelescope(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                tradeIntroductionModule.AddOption("I don’t have an AmatureTelescope.", 
                    pl => !HasAmatureTelescope(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have an AmatureTelescope.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                tradeIntroductionModule.AddOption("I traded recently; I'll return later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Astrolia nods, gazing up at the stars.");
            });

        return greeting;
    }

    private bool HasAmatureTelescope(PlayerMobile player)
    {
        // Check if the player has the AmatureTelescope in their inventory
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AmatureTelescope)) != null;
    }

    private bool CanTradeWithPlayer(PlayerMobile player)
    {
        // Check the trade cooldown (10 minutes)
        if (LastTradeTime.TryGetValue(player, out DateTime lastTrade))
        {
            return (DateTime.UtcNow - lastTrade).TotalMinutes >= 10;
        }
        return true;
    }

    private void CompleteTrade(PlayerMobile player)
    {
        // Remove the AmatureTelescope, give DaggerSign and MaxxiaScroll, and set cooldown
        Item amatureTelescope = player.Backpack.FindItemByType(typeof(AmatureTelescope));
        if (amatureTelescope != null)
        {
            amatureTelescope.Delete();
            player.AddToBackpack(new DaggerSign());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You trade the AmatureTelescope and receive a DaggerSign and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have an AmatureTelescope.");
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
