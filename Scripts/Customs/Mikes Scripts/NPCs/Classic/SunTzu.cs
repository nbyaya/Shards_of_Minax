using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Sun Tzu")]
public class SunTzu : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SunTzu() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sun Tzu";
        Body = 0x190; // Human male body

        // Stats
        SetStr(85);
        SetDex(80);
        SetInt(140);
        SetHits(75);

        // Appearance
        AddItem(new Robe() { Hue = 1175 });
        AddItem(new Sandals() { Hue = 1175 });
        AddItem(new Spellbook() { Name = "Art of War" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
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
        DialogueModule greeting = new DialogueModule("I am Sun Tzu, the master of strategy. How may I impart wisdom to you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My health is irrelevant, but my spirit endures."))));
        
        greeting.AddOption("What is your job?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My 'job' is to impart wisdom to those who seek it."))));
        
        greeting.AddOption("What can you tell me about battles?",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("Do you think yourself a warrior, or merely a pawn?"))));
        
        greeting.AddOption("What about strategy?",
            player => true,
            player => 
            {
                DialogueModule strategyModule = new DialogueModule("Strategy is about understanding the deeper intricacies of conflict. Would you like to explore its principles, tactics, or perhaps historical examples?");
                strategyModule.AddOption("Principles of Strategy.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateStrategyPrinciplesModule())));
                strategyModule.AddOption("Tactics in Conflict.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateTacticsModule())));
                strategyModule.AddOption("Historical Examples.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateHistoricalExamplesModule())));
                player.SendGump(new DialogueGump(player, strategyModule));
            });

        greeting.AddOption("Do you have a quest for me?",
            player => true,
            player => 
            {
                DialogueModule questModule = new DialogueModule("Very well. I have a task that will test your wits and determination. Succeed, and you shall receive a token of my appreciation.");
                questModule.AddOption("What is the task?",
                    p => true,
                    p => 
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("I have no reward right now. Please return later.")));
                        }
                        else
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Your determination is commendable. Here, take this as a sign of my respect for your spirit's strength.") 
                            {
                                // Reward the player

                            }));
                        }
                    });
                player.SendGump(new DialogueGump(player, questModule));
            });

        return greeting;
    }

    private DialogueModule CreateStrategyPrinciplesModule()
    {
        DialogueModule principlesModule = new DialogueModule("The principles of strategy revolve around deception, environment, and direct conflict. Of these, which intrigues you the most?");
        principlesModule.AddOption("Deception.",
            player => true,
            player => 
            {
                DialogueModule deceptionModule = new DialogueModule("Deception is a powerful tool; it allows one to manipulate the battlefield and the minds of foes. How do you perceive deception?");
                deceptionModule.AddOption("It is a necessary evil.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Indeed. Those who master the art of deception often control the flow of battle."))));
                deceptionModule.AddOption("It undermines trust.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("True, but in war, trust is a luxury that can lead to demise."))));
                principlesModule.AddOption("Back to principles.",
                    playerq => true,
                    playerq => player.SendGump(new DialogueGump(player, principlesModule)));
                player.SendGump(new DialogueGump(player, deceptionModule));
            });

        principlesModule.AddOption("Environment.",
            player => true,
            player => 
            {
                DialogueModule environmentModule = new DialogueModule("Understanding your surroundings can turn the tide of any conflict. Use the terrain to your advantage. Do you grasp the importance of the environment?");
                environmentModule.AddOption("Yes, it can be decisive.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Indeed. Knowing the lay of the land can lead to ambushes or safe retreats."))));
                environmentModule.AddOption("Not really.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Understanding the battlefield is crucial. Knowledge is power."))));
                principlesModule.AddOption("Back to principles.",
                    playerw => true,
                    playerw => player.SendGump(new DialogueGump(player, principlesModule)));
                player.SendGump(new DialogueGump(player, environmentModule));
            });

        principlesModule.AddOption("Direct conflict.",
            player => true,
            player => 
            {
                DialogueModule conflictModule = new DialogueModule("Direct conflict requires both strength and strategy. Choose your battles wisely. Are you prepared for direct confrontation?");
                conflictModule.AddOption("I am ready for any battle.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Bravery is commendable, but recklessness can lead to defeat."))));
                conflictModule.AddOption("I prefer to avoid conflict.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Wise choice. A true strategist knows when to fight and when to flee."))));
                principlesModule.AddOption("Back to principles.",
                    playere => true,
                    playere => player.SendGump(new DialogueGump(player, principlesModule)));
                player.SendGump(new DialogueGump(player, conflictModule));
            });

        return principlesModule;
    }

    private DialogueModule CreateTacticsModule()
    {
        DialogueModule tacticsModule = new DialogueModule("Tactics involve the specific actions taken to achieve victory in battle. Would you like to learn about ambushes, formations, or psychological warfare?");
        
        tacticsModule.AddOption("Ambushes.",
            player => true,
            player => 
            {
                DialogueModule ambushModule = new DialogueModule("Ambushes can catch the enemy off guard, turning their strength against them. What do you think makes a successful ambush?");
                ambushModule.AddOption("Surprise is key.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Indeed, timing and location are crucial for a successful ambush."))));
                ambushModule.AddOption("Preparation and planning.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Absolutely. A well-prepared ambush can turn the tide of battle."))));
                tacticsModule.AddOption("Back to tactics.",
                    playerr => true,
                    playerr => player.SendGump(new DialogueGump(player, tacticsModule)));
                player.SendGump(new DialogueGump(player, ambushModule));
            });

        tacticsModule.AddOption("Formations.",
            player => true,
            player => 
            {
                DialogueModule formationModule = new DialogueModule("Proper formations can maximize your forces' strengths while minimizing weaknesses. Do you know any effective formations?");
                formationModule.AddOption("The phalanx.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("The phalanx is a strong defensive formation, great for holding lines."))));
                formationModule.AddOption("The flanking maneuver.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Flanking can disrupt enemy lines, creating opportunities for victory."))));
                tacticsModule.AddOption("Back to tactics.",
                    playert => true,
                    playert => player.SendGump(new DialogueGump(player, tacticsModule)));
                player.SendGump(new DialogueGump(player, formationModule));
            });

        tacticsModule.AddOption("Psychological warfare.",
            player => true,
            player => 
            {
                DialogueModule psyWarModule = new DialogueModule("Psychological warfare can demoralize your enemies before the battle even begins. Have you considered its impact?");
                psyWarModule.AddOption("It can be devastating.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Absolutely. Fear can be a more powerful weapon than steel."))));
                psyWarModule.AddOption("I prefer direct confrontation.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("A valid approach, but sometimes the mind is the best weapon."))));
                tacticsModule.AddOption("Back to tactics.",
                    playery => true,
                    playery => player.SendGump(new DialogueGump(player, tacticsModule)));
                player.SendGump(new DialogueGump(player, psyWarModule));
            });

        return tacticsModule;
    }

    private DialogueModule CreateHistoricalExamplesModule()
    {
        DialogueModule historyModule = new DialogueModule("History is filled with examples of brilliant strategies. Would you like to hear about famous battles or notable generals?");
        
        historyModule.AddOption("Famous battles.",
            player => true,
            player => 
            {
                DialogueModule battleModule = new DialogueModule("The Battle of Alesia showcases the brilliance of strategy, where Caesar encircled the Gauls. What aspect intrigues you most?");
                battleModule.AddOption("The encirclement tactic.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("The encirclement proved decisive, illustrating the importance of terrain and timing."))));
                battleModule.AddOption("The morale of troops.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Morale is crucial in battle; it can determine the outcome before a single blow is struck."))));
                historyModule.AddOption("Back to history.",
                    playeru => true,
                    playeru => player.SendGump(new DialogueGump(player, historyModule)));
                player.SendGump(new DialogueGump(player, battleModule));
            });

        historyModule.AddOption("Notable generals.",
            player => true,
            player => 
            {
                DialogueModule generalModule = new DialogueModule("Generals like Hannibal and Alexander changed the course of history with their tactics. Which general interests you?");
                generalModule.AddOption("Hannibal.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Hannibal's use of elephants and his tactics at Cannae are legendary."))));
                generalModule.AddOption("Alexander.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, new DialogueModule("Alexander's rapid conquests showcase the effectiveness of bold strategy."))));
                historyModule.AddOption("Back to history.",
                    playeri => true,
                    playeri => player.SendGump(new DialogueGump(player, historyModule)));
                player.SendGump(new DialogueGump(player, generalModule));
            });

        return historyModule;
    }

    public SunTzu(Serial serial) : base(serial) { }

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
