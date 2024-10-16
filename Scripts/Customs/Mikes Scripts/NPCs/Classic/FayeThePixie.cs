using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class FayeThePixie : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public FayeThePixie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Faye the Pixie";
        Body = 0x191; // Human female body

        // Stats
        SetStr(50);
        SetDex(90);
        SetInt(80);

        SetHits(40);

        // Appearance
        AddItem(new Kilt() { Hue = 1163 });
        AddItem(new FemaleLeatherChest() { Hue = 1164 });
        AddItem(new PlateGloves() { Hue = 1165 });
        AddItem(new FireballWand() { Name = "Faye's Wand" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public FayeThePixie(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Faye the Pixie, a creature of the woods. What brings you here today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am but a fleeting spirit of the forest, untouched by the passage of time. My role is to observe and protect the secrets of this ancient forest.");
                aboutModule.AddOption("What secrets do you protect?",
                    p => true,
                    p =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Many tales lie hidden within the trees, shadows, and streams of this forest. Secrets of forgotten magic, lost travelers, and hidden realms. I am here to ensure they remain undisturbed.");
                        secretsModule.AddOption("Tell me about the forgotten magic.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule magicModule = new DialogueModule("The forgotten magic is a powerful force that once flowed freely through the Fay Wilds. It was the magic of creation, growth, and transformation, used by the ancient beings to shape the world around them. Over time, many of these secrets were lost, but traces still linger in the deepest parts of the forest.");
                                magicModule.AddOption("Where can I find these traces?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule tracesModule = new DialogueModule("The traces of forgotten magic can be found in sacred groves, hidden glades, and within the ancient standing stones that dot the landscape. But beware, traveler, for these places are guarded by powerful spirits who do not take kindly to intruders.");
                                        tracesModule.AddOption("I will seek them out.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendMessage("Faye nods approvingly. 'May the forest guide you, brave traveler.'");
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        tracesModule.AddOption("That sounds too dangerous.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, tracesModule));
                                    });
                                magicModule.AddOption("Thank you for the information.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, magicModule));
                            });
                        secretsModule.AddOption("Tell me about the lost travelers.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule travelersModule = new DialogueModule("Many travelers have wandered into the Fay Wilds, drawn by its beauty and mystery. Some became lost, unable to find their way out, while others chose to stay, enchanted by the magic of the forest. Their spirits now roam these woods, some peaceful, others restless.");
                                travelersModule.AddOption("Can I help the restless spirits?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule helpSpiritsModule = new DialogueModule("To help the restless spirits, you must understand their sorrow. Each spirit has a story, a reason they cannot move on. If you listen to them and aid them in finding closure, they may find peace.");
                                        helpSpiritsModule.AddOption("Where do I start?",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendMessage("Faye smiles softly. 'Listen to the wind, traveler. The spirits will find you when you are ready.'");
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        helpSpiritsModule.AddOption("I will consider it.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, helpSpiritsModule));
                                    });
                                travelersModule.AddOption("Thank you for telling me.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, travelersModule));
                            });
                        secretsModule.AddOption("Thank you for sharing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, secretsModule));
                    });
                aboutModule.AddOption("What disturbances have you witnessed?",
                    p => true,
                    p =>
                    {
                        DialogueModule disturbancesModule = new DialogueModule("Of late, there have been disturbances—dark shadows that shouldn't be. The balance of nature feels threatened, and I sense an unsettling presence in the woods.");
                        disturbancesModule.AddOption("Tell me more about these dark shadows.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule shadowsModule = new DialogueModule("The dark shadows are not natural. They come from a place beyond the forest, seeking to corrupt and consume. They thrive on fear and despair, and they spread like a sickness, tainting everything they touch.");
                                shadowsModule.AddOption("How can they be stopped?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule stopShadowsModule = new DialogueModule("To stop the shadows, one must bring light to the darkness. Courage, hope, and the magic of the forest are your greatest allies. There are ancient wards hidden throughout the forest that can repel the shadows, but they must be activated by someone with a pure heart.");
                                        stopShadowsModule.AddOption("Where can I find these wards?",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendMessage("Faye points to the east. 'The wards are hidden in the sacred groves. Seek them out, and may your heart remain untainted.'");
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        stopShadowsModule.AddOption("I will do my best.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, stopShadowsModule));
                                    });
                                shadowsModule.AddOption("Thank you for the warning.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, shadowsModule));
                            });
                        disturbancesModule.AddOption("I will keep an eye out for any disturbances.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, disturbancesModule));
                    });
                aboutModule.AddOption("Thank you for sharing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is the whisper of leaves?",
            player => true,
            player =>
            {
                DialogueModule whisperModule = new DialogueModule("The whisper of leaves is the voice of the forest. It speaks to those who listen carefully, sharing tales of ancient times and hidden wisdom.");
                whisperModule.AddOption("Can you tell me a tale from ancient times?",
                    p => true,
                    p =>
                    {
                        DialogueModule taleModule = new DialogueModule("Long ago, before the first cities were built, the Fay Wilds were home to powerful beings known as the Eldertrees. These sentient trees could speak, walk, and even shape the world around them. They were the guardians of balance, ensuring that all life thrived in harmony.");
                        taleModule.AddOption("What happened to the Eldertrees?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule eldertreeModule = new DialogueModule("The Eldertrees stood for millennia, but as the world changed, so too did their power wane. Some fell to the axes of those who sought to exploit their magic, while others went into a deep slumber, their roots extending far beneath the earth. It is said that a few still remain, hidden in the most remote corners of the Fay Wilds, waiting for a time when they are needed once more.");
                                eldertreeModule.AddOption("Can they be awakened?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule awakenModule = new DialogueModule("To awaken an Eldertree is no small task. It requires a connection to the forest, a deep understanding of its rhythms, and an offering of something pure and selfless. Many have tried, but only those who truly understand the balance of nature have succeeded.");
                                        awakenModule.AddOption("I will try to awaken them.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendMessage("Faye smiles gently. 'May your heart guide you well, traveler.'");
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        awakenModule.AddOption("That sounds beyond my abilities.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, awakenModule));
                                    });
                                eldertreeModule.AddOption("Thank you for the story.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, eldertreeModule));
                            });
                        taleModule.AddOption("Thank you for sharing this tale.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, taleModule));
                    });
                whisperModule.AddOption("I would like to learn from the forest.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("The forest welcomes your curiosity, traveler.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, whisperModule));
            });

        greeting.AddOption("Can I receive a reward?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, wise traveler! For those who show true understanding and respect for the forest, there is indeed a reward. Here, take this. It's a small token, but it might aid you on your journey.");
                    rewardModule.AddOption("Thank you for the reward.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new LightningAugmentCrystal()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Faye smiles warmly at you.");
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