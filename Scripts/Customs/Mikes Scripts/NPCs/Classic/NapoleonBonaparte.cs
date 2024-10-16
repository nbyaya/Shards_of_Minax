using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Napoleon Bonaparte")]
public class NapoleonBonaparte : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public NapoleonBonaparte() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Napoleon Bonaparte";
        Body = 0x190; // Human male body
        Str = 100;
        Dex = 100;
        Int = 80;
        Hits = 70;

        // Appearance
        AddItem(new PlateChest() { Hue = 1150 });
        AddItem(new PlateLegs() { Hue = 1150 });
        AddItem(new PlateGloves() { Hue = 1150 });
        AddItem(new PlateHelm() { Hue = 1150 });
        AddItem(new Boots() { Hue = 1150 });
        AddItem(new Halberd() { Name = "Napoleon's Halberd" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

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
        DialogueModule greeting = new DialogueModule("I am Napoleon Bonaparte, the Emperor of France! What brings you to me, traveler?");

        greeting.AddOption("Tell me about your conquests.",
            player => true,
            player => 
            {
                DialogueModule conquestModule = new DialogueModule("Ah, my conquests! From the heights of Austerlitz to the valleys of Egypt, I have expanded my empire far and wide. Each battle was a test of my resolve and strategy.");
                conquestModule.AddOption("Which battle was your greatest victory?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The Battle of Austerlitz, without doubt! It was a masterclass in tactics, defeating the combined forces of Russia and Austria. They did not see me coming!")));
                    });
                conquestModule.AddOption("Tell me about your campaigns in Egypt.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the Egyptian campaign! A mix of exploration and warfare. The Pyramids stood tall as we marched, and the sands whispered secrets of ancient times.")));
                    });
                player.SendGump(new DialogueGump(player, conquestModule));
            });

        greeting.AddOption("What do you think of your legacy?",
            player => true,
            player => 
            {
                DialogueModule legacyModule = new DialogueModule("My legacy? It is a tapestry of triumphs and tragedies. History will remember my name—whether as a hero or a tyrant depends on the storyteller.");
                legacyModule.AddOption("What do you hope they will remember?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I hope they remember my ambition, my reforms, and my contributions to civil law. A man must be remembered for his deeds, not merely his titles.")));
                    });
                player.SendGump(new DialogueGump(player, legacyModule));
            });

        greeting.AddOption("How do you define greatness?",
            player => true,
            player => 
            {
                DialogueModule greatnessModule = new DialogueModule("Greatness is a relentless pursuit! It requires courage, vision, and the ability to inspire others. One must be willing to take risks to achieve true greatness.");
                greatnessModule.AddOption("Do you believe you achieved greatness?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! I have shaped Europe, influenced governance, and instilled the Napoleonic Code. Yet, greatness is subjective—some may view me as a villain.")));
                    });
                player.SendGump(new DialogueGump(player, greatnessModule));
            });

        greeting.AddOption("Tell me about your rise to power.",
            player => true,
            player => 
            {
                DialogueModule riseModule = new DialogueModule("My rise was not without its challenges! From humble beginnings as an artillery officer to the heights of Emperor, I seized opportunities and leveraged my strengths.");
                riseModule.AddOption("What challenges did you face?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The political turmoil of the Revolution was daunting. But I navigated through it with cunning and resolve, always positioning myself for success.")));
                    });
                player.SendGump(new DialogueGump(player, riseModule));
            });

        greeting.AddOption("What are your thoughts on strategy?",
            player => true,
            player => 
            {
                DialogueModule strategyModule = new DialogueModule("Strategy is the cornerstone of victory! It is the art of planning and the science of execution. One must always anticipate the enemy’s moves.");
                strategyModule.AddOption("Can you give an example?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("During the Battle of Jena, I outmaneuvered the Prussian army by using rapid movements and feigned retreats. They were left bewildered and defeated.")));
                    });
                player.SendGump(new DialogueGump(player, strategyModule));
            });

        greeting.AddOption("What do you think of your enemies?",
            player => true,
            player => 
            {
                DialogueModule enemiesModule = new DialogueModule("Enemies are a necessary part of life. They challenge us to grow stronger and wiser. However, some I consider worthy adversaries, while others are mere fools.");
                enemiesModule.AddOption("Who do you see as your greatest enemy?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Perhaps the Duke of Wellington. Our clashes were legendary, culminating at Waterloo. It was a battle that would seal my fate.")));
                    });
                player.SendGump(new DialogueGump(player, enemiesModule));
            });

        greeting.AddOption("Do you have any advice for aspiring leaders?",
            player => true,
            player => 
            {
                DialogueModule adviceModule = new DialogueModule("To lead is to serve! A true leader must listen to their people and adapt. Surround yourself with wise counsel and remain steadfast in your vision.");
                adviceModule.AddOption("What if your vision fails?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Failure is but a stepping stone! Learn from it, adjust your strategy, and rise again. Resilience is key in leadership.")));
                    });
                player.SendGump(new DialogueGump(player, adviceModule));
            });

        greeting.AddOption("What do you think about diplomacy?",
            player => true,
            player => 
            {
                DialogueModule diplomacyModule = new DialogueModule("Diplomacy is an art form! It requires finesse and the ability to read between the lines. Sometimes, a strong hand is necessary, but negotiations can save bloodshed.");
                diplomacyModule.AddOption("Have you ever regretted a diplomatic decision?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Regret is a heavy burden. I often ponder whether I could have averted conflict through better negotiations. Alas, history is written with such dilemmas.")));
                    });
                player.SendGump(new DialogueGump(player, diplomacyModule));
            });

        greeting.AddOption("What do you think of art and culture?",
            player => true,
            player => 
            {
                DialogueModule cultureModule = new DialogueModule("Art and culture are the soul of a nation! They inspire and elevate the human spirit. I have supported the arts and sciences to foster a thriving society.");
                cultureModule.AddOption("What is your favorite piece of art?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the works of Jacques-Louis David! His paintings capture the spirit of the Revolution and my era so vividly. Art must reflect the times.")));
                    });
                player.SendGump(new DialogueGump(player, cultureModule));
            });

        greeting.AddOption("Do you believe in destiny?",
            player => true,
            player => 
            {
                DialogueModule destinyModule = new DialogueModule("Destiny is a curious concept. I believe we carve our own paths, yet sometimes we encounter circumstances that seem preordained.");
                destinyModule.AddOption("Have you faced moments of destiny?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Many times! My rise during the chaos of the Revolution felt fated. I seized my opportunities and never looked back.")));
                    });
                player.SendGump(new DialogueGump(player, destinyModule));
            });

        return greeting;
    }

    public NapoleonBonaparte(Serial serial) : base(serial) { }

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
