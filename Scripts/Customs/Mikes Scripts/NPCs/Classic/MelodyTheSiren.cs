using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Melody the Siren")]
public class MelodyTheSiren : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MelodyTheSiren() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Melody";
        Body = 0x191; // Human female body

        // Stats
        Str = 80;
        Dex = 40;
        Int = 100;
        Hits = 60;

        // Appearance
        AddItem(new Skirt() { Hue = 1159 });
        AddItem(new FemaleLeatherChest() { Hue = 1159 });
        AddItem(new LeatherGloves() { Hue = 1159 });
        AddItem(new Lute() { Name = "Melody's Lute" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public MelodyTheSiren(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I'm Melody the Siren, but don't expect me to sing for you!");

        greeting.AddOption("What do you do?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My \"job\"? I lure sailors to their doom with my enchanting voice. Happy now?");
                jobModule.AddOption("Isn't that dangerous?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule dangerModule = new DialogueModule("Dangerous? Not for me! But many sailors have met their end. It's a game of wit, after all.");
                        dangerModule.AddOption("What happens to them?",
                            p => true,
                            p =>
                            {
                                DialogueModule fateModule = new DialogueModule("Some become lost in the depths, while others might find their way back, forever haunted by the encounter.");
                                p.SendGump(new DialogueGump(p, fateModule));
                            });
                        pl.SendGump(new DialogueGump(pl, dangerModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you sing?",
            player => true,
            player =>
            {
                DialogueModule singModule = new DialogueModule("Why does everyone always want me to sing? Bring me a trinket from the depths, and maybe I'll consider it.");
                singModule.AddOption("What kind of trinket?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule trinketModule = new DialogueModule("I seek things of beauty: a glimmering shell, a lost sailor's charm, or perhaps a gem infused with ocean magic.");
                        pl.SendGump(new DialogueGump(pl, trinketModule));
                    });
                player.SendGump(new DialogueGump(player, singModule));
            });

        greeting.AddOption("Tell me about sailors.",
            player => true,
            player =>
            {
                DialogueModule sailorsModule = new DialogueModule("Yes, sailors. Men with dreams of adventure. They never expect the dangers that lurk below. There's a tale I remember...");
                sailorsModule.AddOption("What tale?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule taleModule = new DialogueModule("Long ago, a sailor named Elias charmed even the most vengeful of sirens. Instead of leading him to doom, they fell in love.");
                        taleModule.AddOption("What happened next?",
                            p => true,
                            p =>
                            {
                                DialogueModule nextModule = new DialogueModule("Their love sparked jealousy among the other sirens, leading to chaos. In the end, they vanished, leaving only whispers behind.");
                                p.SendGump(new DialogueGump(p, nextModule));
                            });
                        pl.SendGump(new DialogueGump(pl, taleModule));
                    });
                player.SendGump(new DialogueGump(player, sailorsModule));
            });

        greeting.AddOption("What about the legend?",
            player => true,
            player =>
            {
                DialogueModule legendModule = new DialogueModule("They say sirens used to be kind, guiding sailors safely home. But betrayal turned us into vengeful spirits.");
                legendModule.AddOption("What betrayal?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule betrayalModule = new DialogueModule("One sailor, smitten by a siren's song, promised her his heart. He betrayed her for another, and we turned cold.");
                        betrayalModule.AddOption("Is there any hope for sirens?",
                            p => true,
                            p =>
                            {
                                DialogueModule hopeModule = new DialogueModule("Perhaps! If a sailor can prove their worthiness, a siren's heart may soften. But it's a rare occurrence.");
                                p.SendGump(new DialogueGump(p, hopeModule));
                            });
                        pl.SendGump(new DialogueGump(pl, betrayalModule));
                    });
                player.SendGump(new DialogueGump(player, legendModule));
            });

        greeting.AddOption("I have a trinket.",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, you have something for me? Very well. I'll sing a short tune. But be careful what you wish for.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("What do you think of humans?",
            player => true,
            player =>
            {
                DialogueModule humanModule = new DialogueModule("Humans are curious creatures. Some are brave, while others are foolish. It's a fine line between the two.");
                humanModule.AddOption("Am I brave or foolish?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule braveFoolishModule = new DialogueModule("Perhaps a bit of both! But bravery in the face of danger is admirable, even if it borders on foolishness.");
                        pl.SendGump(new DialogueGump(pl, braveFoolishModule));
                    });
                player.SendGump(new DialogueGump(player, humanModule));
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
