using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AuraTheAlchemist : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public AuraTheAlchemist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aura the Alchemist";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(40);
        SetInt(120);
        SetHits(70);

        // Appearance
        AddItem(new Robe(1156)); // Kimono with hue 1156
        AddItem(new ThighBoots(38)); // Thigh boots with hue 38
        AddItem(new LeatherGloves() { Name = "Aura's Protective Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public AuraTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Aura the Alchemist. What brings you to my humble corner of the mystic arts?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule nameModule = new DialogueModule("I am Aura, an alchemist who dabbles in the mystic arts. My work involves concocting potions and exploring the mysteries of alchemy.");
                nameModule.AddOption("Tell me more about alchemy.",
                    p => true,
                    p =>
                    {
                        DialogueModule alchemyModule = new DialogueModule("Alchemy is the art of transforming base materials into wondrous concoctions. But the true art lies beyond simple potions. Are you interested in the secrets of the Philosopher's Stone and the pursuit of immortality?");
                        alchemyModule.AddOption("Yes, tell me about the Philosopher's Stone.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule philosopherModule = new DialogueModule("The Philosopher's Stone... it is said to grant immortality and transform lead into gold. But the journey to create such a stone is fraught with danger and deep understanding of the unknown.");
                                philosopherModule.AddOption("What is required to create the Philosopher's Stone?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule requirementsModule = new DialogueModule("To create the Philosopher's Stone, you must obtain several rare ingredients: the Heart of a Phoenix, the Breath of a Dragon, Moonstone Dust, and the Essence of the Eternal Flame. Each of these is hidden away in dangerous places, guarded by creatures of legend.");
                                        requirementsModule.AddOption("Where can I find the Heart of a Phoenix?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule phoenixModule = new DialogueModule("The Heart of a Phoenix can be found only in the Nest of Embers, a place that lies beyond the molten mountains. It is said that the Phoenix guards its heart fiercely, and only one with true courage can face it.");
                                                phoenixModule.AddOption("I will find the Phoenix.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("You set off on a journey to find the Nest of Embers and face the Phoenix.");
                                                    });
                                                phoenixModule.AddOption("That sounds too dangerous.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, phoenixModule));
                                            });
                                        requirementsModule.AddOption("What about the Breath of a Dragon?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule dragonModule = new DialogueModule("The Breath of a Dragon can only be collected from a living dragon during its slumber. The dragons reside in the Caverns of Whispers, deep within the uncharted forests. You must be careful, for disturbing a dragon could mean certain death.");
                                                dragonModule.AddOption("I will attempt this task.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("You gather your courage and prepare for a dangerous journey to the Caverns of Whispers.");
                                                    });
                                                dragonModule.AddOption("That is too risky for me.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, dragonModule));
                                            });
                                        requirementsModule.AddOption("Tell me about Moonstone Dust.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule moonstoneModule = new DialogueModule("Moonstone Dust is collected under the light of a full moon from the cliffs of the Whispering Peaks. It is said that the dust sparkles only under moonlight, and many who have sought it have been lost to the cliffs.");
                                                moonstoneModule.AddOption("I will go to the Whispering Peaks.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("You set off towards the Whispering Peaks, determined to gather the elusive Moonstone Dust.");
                                                    });
                                                moonstoneModule.AddOption("It sounds too perilous.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, moonstoneModule));
                                            });
                                        requirementsModule.AddOption("What is the Essence of the Eternal Flame?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule flameModule = new DialogueModule("The Essence of the Eternal Flame can only be gathered from the Flame of Eternity, which burns in the depths of the Forgotten Sanctuary. The flame never extinguishes and is protected by ancient guardians who test the will of anyone seeking its power.");
                                                flameModule.AddOption("I will seek the Eternal Flame.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("You begin your quest to find the Forgotten Sanctuary and claim the Essence of the Eternal Flame.");
                                                    });
                                                flameModule.AddOption("Perhaps another time.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, flameModule));
                                            });
                                        requirementsModule.AddOption("I need to think about this.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, requirementsModule));
                                    });
                                philosopherModule.AddOption("What is the purpose of the Philosopher's Stone?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule purposeModule = new DialogueModule("The Philosopher's Stone is more than a tool for immortality. It represents the journey of self-discovery, enlightenment, and the mastery of nature's most profound mysteries. Only those who are worthy can understand its true power.");
                                        purposeModule.AddOption("How do I prove myself worthy?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule proveModule = new DialogueModule("To prove yourself, you must face challenges that test your courage, wisdom, and resilience. Many have tried and failed. Only those who persevere through the trials can unlock the stone's potential.");
                                                proveModule.AddOption("I am ready to face these challenges.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("Aura nods approvingly: 'Then your journey begins now. Seek the rare ingredients and face the trials ahead.'");
                                                    });
                                                proveModule.AddOption("I need more time to prepare.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, proveModule));
                                            });
                                        purposeModule.AddOption("This sounds daunting. Perhaps another time.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, purposeModule));
                                    });
                                philosopherModule.AddOption("This sounds like too much for me.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, philosopherModule));
                            });
                        alchemyModule.AddOption("No, I am not ready for such secrets.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, alchemyModule));
                    });
                nameModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Aura nods at you.");
                    });
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward for you right now. Please return later.");
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, eager are we? If you bring me the mandrake root, I shall bestow upon you a special concoction of my own making. Its effects? Well, that's a surprise.");
                    rewardModule.AddOption("Thank you, I'll bring the mandrake root.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendMessage("Aura gives you a special concoction.");
                        });
                    rewardModule.AddOption("I need more time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Tell me about the philosopher's stone.",
            player => true,
            player =>
            {
                DialogueModule philosopherModule = new DialogueModule("The philosopher's stone is said to transform base materials into gold, but it is also a symbol of enlightenment. Do you think you are ready to uncover its secrets?");
                philosopherModule.AddOption("Yes, tell me more.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Aura smiles mysteriously: 'Then prove your worth by facing the unknown.'");
                    });
                philosopherModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, philosopherModule));
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}