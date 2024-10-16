using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class GrokTheGoblin : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public GrokTheGoblin() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Grok the Goblin";
        Body = 0x14; // Goblin body

        // Stats
        SetStr(120);
        SetDex(70);
        SetInt(50);
        SetHits(80);

        // Appearance
        Hue = 1160;
        AddItem(new ShortPants() { Hue = 1161 });
        AddItem(new Shirt() { Hue = 1161 });
        AddItem(new LeatherCap() { Hue = 1162 });
        AddItem(new Dagger() { Name = "Grok's Dagger" });

        HairItemID = 0x204F; // Hair ID for Goblin
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Me Grok, mighty Goblin! How can I help you, strong one?");
        
        greeting.AddOption("What do you do?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateJobModule()));
            });

        greeting.AddOption("Tell me about the cave.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateCaveModule()));
            });

        greeting.AddOption("What do you want?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateProofModule()));
            });

        greeting.AddOption("I need to prove my strength.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateStrengthModule()));
            });

        return greeting;
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("Grok guards the Goblin cave! Strong Goblin like me keep treasure safe.");
        jobModule.AddOption("What treasures do you have?",
            player => true,
            player =>
            {
                DialogueModule treasureModule = new DialogueModule("In cave, we have shinies like gems, gold, and rare artifacts! But they not for weaklings! You strong enough to help?");
                treasureModule.AddOption("I can help!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateProofModule()));
                    });
                treasureModule.AddOption("Maybe not right now.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, treasureModule));
            });
        return jobModule;
    }

    private DialogueModule CreateCaveModule()
    {
        DialogueModule caveModule = new DialogueModule("Inside cave, many treasures hidden! Not for weaklings! Maybe Grok share secret if you prove worthy.");
        caveModule.AddOption("What kind of secrets?",
            player => true,
            player =>
            {
                DialogueModule secretsModule = new DialogueModule("Secrets of Goblins! How to sneak, how to steal, and how to be mighty like Grok! You want to learn?");
                secretsModule.AddOption("Yes, teach me!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateStrengthModule()));
                    });
                secretsModule.AddOption("No, I just want the treasure.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateProofModule()));
                    });
                player.SendGump(new DialogueGump(player, secretsModule));
            });
        return caveModule;
    }

    private DialogueModule CreateProofModule()
    {
        DialogueModule proofModule = new DialogueModule("To prove worthy, you bring Grok a rare gem from deep inside dungeon. Do this, and Grok might share cave secrets and give you reward!");
        
        if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
        {
            proofModule.AddOption("What about the reward?",
                player => true,
                player =>
                {
                    proofModule.NPCText = "I have no reward right now. Please return later.";
                    player.SendGump(new DialogueGump(player, proofModule));
                });
        }
        else
        {
            proofModule.AddOption("I'll bring you a gem.",
                player => true,
                player =>
                {
                    player.AddToBackpack(new MaxxiaScroll()); // Example reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    proofModule.NPCText = "Thank you! Bring me the gem and I will reward you!";
                    player.SendGump(new DialogueGump(player, proofModule));
                });
        }

        proofModule.AddOption("Forget it.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return proofModule;
    }

    private DialogueModule CreateStrengthModule()
    {
        DialogueModule strengthModule = new DialogueModule("Strength is not just muscles! It’s about being smart and sneaky! Prove you can do both.");
        
        strengthModule.AddOption("How can I prove my strength?",
            player => true,
            player =>
            {
                DialogueModule proveSmartModule = new DialogueModule("You must solve a riddle! What has roots as nobody sees, Is taller than trees, Up, up it goes, And yet never grows?");
                proveSmartModule.AddOption("A mountain!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateVictoryModule()));
                    });
                proveSmartModule.AddOption("I don’t know.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateFailureModule()));
                    });
                player.SendGump(new DialogueGump(player, proveSmartModule));
            });

        return strengthModule;
    }

    private DialogueModule CreateVictoryModule()
    {
        DialogueModule victoryModule = new DialogueModule("You are smart! Grok like you! Now, show strength by bringing me a rare gem!");
        victoryModule.AddOption("I’ll find it!",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateProofModule()));
            });
        return victoryModule;
    }

    private DialogueModule CreateFailureModule()
    {
        DialogueModule failureModule = new DialogueModule("Not everyone can be smart! But Grok believes you can try again! Want another riddle?");
        failureModule.AddOption("Yes, give me another riddle.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateStrengthModule()));
            });
        failureModule.AddOption("No, I’m done.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });
        return failureModule;
    }

    public GrokTheGoblin(Serial serial) : base(serial) { }

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
