using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class NecroNina : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public NecroNina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Necro Nina";
        Body = 0x190; // Human female body

        // Stats
        SetStr(130);
        SetDex(70);
        SetInt(100);
        SetHits(100);

        // Appearance
        AddItem(new Robe() { Hue = 38 });
        AddItem(new Sandals() { Hue = 38 });
        AddItem(new BoneGloves() { Name = "Nina's Necrotic Nails" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public NecroNina(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, mortal. I am Necro Nina, a mistress of the dark arts.");

        greeting.AddOption("Tell me about your powers.",
            player => true,
            player =>
            {
                DialogueModule powerModule = new DialogueModule("True power lies not in brute force, but in knowledge and understanding of the arcane. But beware, such knowledge comes with a price.");
                powerModule.AddOption("What do you mean by that?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreatePowerDetailsModule())));
                player.SendGump(new DialogueGump(player, powerModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I delve into the forbidden arts of necromancy. It's a path filled with risks but also great rewards.");
                jobModule.AddOption("What rewards do you speak of?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                jobModule.AddOption("How did you become a necromancer?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateBackgroundModule())));
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Can you demonstrate your rituals?",
            player => true,
            player =>
            {
                DialogueModule ritualModule = new DialogueModule("My rituals are a bridge between the living and the dead. Through them, I can converse with spirits. Would you like to assist me?");
                ritualModule.AddOption("Yes, I would like to help.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("You prepare to assist in the ritual.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                ritualModule.AddOption("Maybe another time.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, ritualModule));
            });

        greeting.AddOption("What do you think of the eight virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The eight virtues are limited in understanding. I seek a deeper, darker interpretation through my rituals.");
                virtuesModule.AddOption("What do you mean by a 'darker interpretation'?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateVirtuesModule())));
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Can you grant me a token of power?",
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
                    player.SendMessage("Ah, a brave soul. Here, take this token of power. Use it wisely and remember, every gift comes with a cost.");
                    player.AddToBackpack(new NecromancyAugmentCrystal());
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return greeting;
    }

    private DialogueModule CreatePowerDetailsModule()
    {
        DialogueModule powerDetails = new DialogueModule("The knowledge of necromancy is not just about raising the dead. It involves understanding the flow of life and death.");
        powerDetails.AddOption("What other powers do you possess?",
            pl => true,
            pl =>
            {
                DialogueModule otherPowers = new DialogueModule("I can summon spirits, command the undead, and manipulate life force itself. But each spell demands a price—sacrifice is often required.");
                otherPowers.AddOption("What kind of sacrifice?",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateSacrificeModule())));
                pl.SendGump(new DialogueGump(pl, otherPowers));
            });
        return powerDetails;
    }

    private DialogueModule CreateSacrificeModule()
    {
        DialogueModule sacrificeModule = new DialogueModule("Sacrifices can vary from the trivial—a handful of herbs—to something more profound, like a piece of one's own vitality. Each choice shapes the outcome of the magic.");
        sacrificeModule.AddOption("I see. Sounds dangerous.",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        return sacrificeModule;
    }

    private DialogueModule CreateBackgroundModule()
    {
        DialogueModule backgroundModule = new DialogueModule("Once, I was a scholar of the arcane arts. My thirst for knowledge led me down forbidden paths, seeking secrets hidden from the light of day.");
        backgroundModule.AddOption("What secrets did you discover?",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, CreateSecretsModule())));
        return backgroundModule;
    }

    private DialogueModule CreateSecretsModule()
    {
        DialogueModule secretsModule = new DialogueModule("I uncovered rituals that could bind spirits, spells that could manipulate time, and the dark truth of necromancy itself. But knowledge is a double-edged sword.");
        secretsModule.AddOption("What happened then?",
            pl => true,
            pl =>
            {
                DialogueModule consequenceModule = new DialogueModule("I became a pariah among my peers, hunted for my practices. But in the shadows, I found power, and now I wield it.");
                consequenceModule.AddOption("You sound powerful.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                pl.SendGump(new DialogueGump(pl, consequenceModule));
            });
        return secretsModule;
    }

    private DialogueModule CreateVirtuesModule()
    {
        DialogueModule virtuesModule = new DialogueModule("The virtues are a guide for the living, but I believe they can be manipulated. For example, the virtue of compassion can lead to weakness in the eyes of the dead.");
        virtuesModule.AddOption("That sounds twisted.",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        virtuesModule.AddOption("What do you seek in the shadows?",
            pl => true,
            pl =>
            {
                DialogueModule seekModule = new DialogueModule("I seek knowledge, power, and perhaps a way to transcend this mortal coil. The shadows hold secrets that could grant me eternal life.");
                seekModule.AddOption("Eternal life? Is that possible?",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, CreateEternalLifeModule())));
                pl.SendGump(new DialogueGump(pl, seekModule));
            });
        return virtuesModule;
    }

    private DialogueModule CreateEternalLifeModule()
    {
        DialogueModule eternalLifeModule = new DialogueModule("It is said that by mastering the art of necromancy, one can achieve a form of immortality. However, it often requires great sacrifice and a deep understanding of the soul.");
        eternalLifeModule.AddOption("What sacrifices would be needed?",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, CreateSacrificeModule())));
        eternalLifeModule.AddOption("That sounds horrifying.",
            pl => true,
            pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
        return eternalLifeModule;
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
