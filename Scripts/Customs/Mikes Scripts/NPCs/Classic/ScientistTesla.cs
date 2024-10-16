using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;

public class ScientistTesla : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ScientistTesla() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Scientist Tesla";
        Body = 0x190; // Human male body

        // Stats
        SetStr(85);
        SetDex(65);
        SetInt(115);
        SetHits(65);

        // Appearance
        AddItem(new LongPants() { Hue = 1157 });
        AddItem(new Doublet() { Hue = 1158 });
        AddItem(new Shoes() { Hue = 0 });
        AddItem(new FeatheredHat() { Hue = 1159 });
        AddItem(new FireballWand() { Name = "Tesla's Innovations" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ScientistTesla(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Scientist Tesla, a brilliant mind wasted on this wretched society! How may I assist you?");
        
        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("My health? What does it matter? I'm a mere pawn in this wretched game of life.");
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My so-called 'job' is to toil away in obscurity, conducting experiments that no one appreciates.");
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What discoveries have you made?",
            player => true,
            player =>
            {
                DialogueModule discoveriesModule = new DialogueModule("Do you have any idea how many groundbreaking discoveries I've made, only to have them ignored by the masses?");
                player.SendGump(new DialogueGump(player, discoveriesModule));
            });

        greeting.AddOption("What do you think of the company founded in your name?",
            player => true,
            player =>
            {
                DialogueModule companyModule = new DialogueModule("Ah, the so-called 'Tesla Inc.' Founded in my name, yet it seems more like a circus than a tribute to my work.");
                companyModule.AddOption("Why do you feel that way?",
                    p => true,
                    p =>
                    {
                        DialogueModule whyModule = new DialogueModule("They focus more on flashy products and marketing than on true scientific advancement. It's an insult to my legacy!");
                        whyModule.AddOption("Do you think they make real innovations?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule innovationsModule = new DialogueModule("Some claim they innovate, but I see a lot of noise and little substance. A true scientist seeks knowledge, not just profits.");
                                pl.SendGump(new DialogueGump(pl, innovationsModule));
                            });
                        whyModule.AddOption("That sounds harsh. What do you prefer?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule preferModule = new DialogueModule("I prefer a focus on genuine exploration of the unknown, not a relentless push for sales. Knowledge should be the goal.");
                                pl.SendGump(new DialogueGump(pl, preferModule));
                            });
                        p.SendGump(new DialogueGump(p, whyModule));
                    });
                player.SendGump(new DialogueGump(player, companyModule));
            });

        greeting.AddOption("What do you think of Elon Musk?",
            player => true,
            player =>
            {
                DialogueModule elonModule = new DialogueModule("Elon Musk? A blowhard! He touts himself as a visionary but lacks the humility and depth of real scientists.");
                elonModule.AddOption("What do you mean by that?",
                    p => true,
                    p =>
                    {
                        DialogueModule depthModule = new DialogueModule("True science requires patience, experimentation, and a willingness to learn from failure. He often skips that for the limelight.");
                        depthModule.AddOption("So you think he's not a real scientist?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule realScientistModule = new DialogueModule("Exactly! He may have a knack for business, but science isn't just about profits and publicity.");
                                pl.SendGump(new DialogueGump(pl, realScientistModule));
                            });
                        depthModule.AddOption("Isn't he doing anything good?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule goodModule = new DialogueModule("He does push some boundaries, but at what cost? Innovation should be ethical and responsible, not just for headlines.");
                                pl.SendGump(new DialogueGump(pl, goodModule));
                            });
                        p.SendGump(new DialogueGump(p, depthModule));
                    });
                player.SendGump(new DialogueGump(player, elonModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => CanGiveReward(),
            player =>
            {
                if (GiveReward(player))
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, science! The beacon of hope in a world filled with darkness. As a token of appreciation, take this.");
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
                else
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
            });

        return greeting;
    }

    private bool CanGiveReward()
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private bool GiveReward(Mobile player)
    {
        lastRewardTime = DateTime.UtcNow; // Update the timestamp
        player.AddToBackpack(new PoisoningAugmentCrystal()); // Give the reward
        return true;
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
