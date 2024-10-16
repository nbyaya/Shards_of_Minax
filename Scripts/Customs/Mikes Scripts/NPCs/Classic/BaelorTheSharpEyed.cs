using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BaelorTheSharpEyed : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BaelorTheSharpEyed() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Baelor the Sharp-Eyed";
        Body = 0x190; // Human male body

        // Stats
        SetStr(110);
        SetDex(80);
        SetInt(90);

        SetHits(110);

        // Appearance
        AddItem(new LongPants(1175));
        AddItem(new FancyShirt(1154));
        AddItem(new Boots(1904));
        AddItem(new Bow { Name = "Baelor's Bow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BaelorTheSharpEyed(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Baelor the Sharp-Eyed, master archer. How may I assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Baelor, a protector of these lands, with my bow always ready to fend off threats. I take pride in keeping these woods safe from bandits and dark magic.");
                aboutModule.AddOption("What threats do these lands face?",
                    p => true,
                    p =>
                    {
                        DialogueModule threatsModule = new DialogueModule("These lands are plagued by dire wolves, cunning bandits, and even rumors of dark sorcery. Have you ever encountered the bandits around here?");
                        threatsModule.AddOption("Yes, I've faced the bandits.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule banditsModule = new DialogueModule("The bandits are led by a ruthless leader named Skar. He is a dangerous man with a bounty on his head. If you can bring me proof of his defeat, you will be generously rewarded.");
                                banditsModule.AddOption("Who is Skar?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule skarModule = new DialogueModule("Skar is the leader of the bandits, and a thorn in the side of every honest traveler. He carries a unique insignia—a black raven. Bring that to me as proof of his defeat, and I will reward you.");
                                        skarModule.AddOption("I will take on the task to defeat Skar.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendMessage("You accept the challenge to hunt down Skar.");
                                            });
                                        skarModule.AddOption("That sounds too dangerous for me.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, skarModule));
                                    });
                                banditsModule.AddOption("I'll see if I can find Skar.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, banditsModule));
                            });
                        threatsModule.AddOption("No, tell me more about them.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, threatsModule));
                            });
                        p.SendGump(new DialogueGump(p, threatsModule));
                    });
                aboutModule.AddOption("Good to know. Farewell.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Baelor nods at you.");
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Tell me about the enchantments you use on your arrows.",
            player => true,
            player =>
            {
                DialogueModule enchantmentsModule = new DialogueModule("Ah, the enchantments! Each arrow is a craft of magic and precision. I enchant them for different purposes, depending on the threat. Let me tell you more.");
                enchantmentsModule.AddOption("Tell me about fire enchantments.",
                    p => true,
                    p =>
                    {
                        DialogueModule fireModule = new DialogueModule("The fire enchantments are some of my favorites. They ignite the arrows with a blazing flame, which can burn through armor and send foes fleeing in terror. I use a mix of brimstone and phoenix feathers to create this enchantment.");
                        fireModule.AddOption("What are the challenges in crafting fire arrows?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule challengeModule = new DialogueModule("The biggest challenge is controlling the flame. Too much brimstone, and the arrow becomes unstable, possibly exploding in my own hands. I must carefully balance the components to ensure each shot is lethal to my enemies, not to myself.");
                                challengeModule.AddOption("Sounds dangerous, but impressive.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                challengeModule.AddOption("Have you ever failed while making one?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule failureModule = new DialogueModule("Oh yes, I have failed a few times. One such time, I nearly set my entire camp ablaze. It taught me the importance of respect for the elements I use. Magic can be powerful, but it must be handled with care.");
                                        failureModule.AddOption("Thank you for sharing that.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, failureModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, challengeModule));
                            });
                        fireModule.AddOption("I'd like to know about another kind of enchantment.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, enchantmentsModule));
                            });
                        p.SendGump(new DialogueGump(p, fireModule));
                    });
                enchantmentsModule.AddOption("Tell me about frost enchantments.",
                    p => true,
                    p =>
                    {
                        DialogueModule frostModule = new DialogueModule("Frost enchantments are perfect for slowing down enemies. When struck, the arrow releases a chilling blast that freezes foes in place. I use ice crystals from the northern peaks and a sprinkle of powdered snow to craft these arrows.");
                        frostModule.AddOption("How effective are these against armored foes?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule armorModule = new DialogueModule("Against armored foes, frost arrows work best to slow them down, making their heavy armor a burden. While the frost itself may not pierce the armor, it certainly limits their movement, making them vulnerable to follow-up attacks.");
                                armorModule.AddOption("That sounds like a clever strategy.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, armorModule));
                            });
                        frostModule.AddOption("What are the components needed for frost enchantments?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule componentsModule = new DialogueModule("The components include ice crystals, as I mentioned, but also the essence of a frost elemental. The essence is particularly hard to obtain, as it requires careful extraction from a defeated elemental. Without it, the enchantment lacks the power to freeze effectively.");
                                componentsModule.AddOption("That must be risky to obtain.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule riskModule = new DialogueModule("Indeed, it is. Frost elementals are dangerous, not only for their cold attacks but for their resilience. Extracting their essence requires precision and courage. It's a task not for the faint of heart.");
                                        riskModule.AddOption("I appreciate your bravery.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, riskModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, componentsModule));
                            });
                        frostModule.AddOption("I'd like to hear about another enchantment.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, enchantmentsModule));
                            });
                        p.SendGump(new DialogueGump(p, frostModule));
                    });
                enchantmentsModule.AddOption("Tell me about poison enchantments.",
                    p => true,
                    p =>
                    {
                        DialogueModule poisonModule = new DialogueModule("Poison enchantments are the most insidious. I use venom extracted from forest spiders and blend it with herbs to create a toxin that seeps through wounds, weakening enemies over time. These arrows are ideal for wearing down tougher foes.");
                        poisonModule.AddOption("What kind of herbs do you use?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule herbsModule = new DialogueModule("I use nightshade and belladonna, both known for their potent effects. The key is blending them in just the right ratio so the poison doesn't kill instantly but instead weakens the enemy slowly, making them more vulnerable.");
                                herbsModule.AddOption("A slow death sounds terrifying.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, herbsModule));
                            });
                        poisonModule.AddOption("Are there risks in using these arrows?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule risksModule = new DialogueModule("Yes, handling poison is always risky. One mistake, even a small cut, could lead to disastrous consequences. I always take extreme precautions when crafting these arrows, ensuring my own safety as well as the effectiveness of the poison.");
                                risksModule.AddOption("You must be very careful.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, risksModule));
                            });
                        poisonModule.AddOption("I'd like to know about other enchantments.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, enchantmentsModule));
                            });
                        p.SendGump(new DialogueGump(p, poisonModule));
                    });
                enchantmentsModule.AddOption("Tell me about lightning enchantments.",
                    p => true,
                    p =>
                    {
                        DialogueModule lightningModule = new DialogueModule("Lightning enchantments are for when you need to make an impact. The arrow becomes a conduit for electrical energy, striking the target with a thunderous force. I use shards of storm crystals, charged during lightning storms, to create these arrows.");
                        lightningModule.AddOption("How do you charge the storm crystals?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule chargingModule = new DialogueModule("Charging storm crystals is a delicate process. I must set them up on the highest peak during a storm and let the lightning strike them naturally. Capturing the raw energy of the storm is essential for imbuing the arrows with true power.");
                                chargingModule.AddOption("That sounds extremely dangerous.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("It is. I've seen many attempts fail, and some have ended tragically. But when successful, the energy captured in the crystal gives the arrow unparalleled power, capable of paralyzing even the fiercest enemy.");
                                        dangerModule.AddOption("You take great risks for your craft.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, dangerModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, chargingModule));
                            });
                        lightningModule.AddOption("What are the effects of a lightning strike arrow?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule effectsModule = new DialogueModule("When a lightning arrow hits, it releases an electric shock that can paralyze enemies, leaving them vulnerable. The sheer force of the impact also causes disarray, giving allies an opportunity to strike. It is a powerful tool in any archer's arsenal.");
                                effectsModule.AddOption("That sounds incredibly effective.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, effectsModule));
                            });
                        lightningModule.AddOption("I'd like to know about another enchantment.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, enchantmentsModule));
                            });
                        p.SendGump(new DialogueGump(p, lightningModule));
                    });
                enchantmentsModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Baelor nods at you. May your arrows fly true.");
                    });
                player.SendGump(new DialogueGump(player, enchantmentsModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward for you right now. Please return later.");
                    cooldownModule.AddOption("Understood. I'll return later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Here, take these arrows. They are crafted for precision and will serve you well.");
                    rewardModule.AddOption("Thank you, Baelor.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new ArcheryAugmentCrystal());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Baelor waves you off with a nod.");
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