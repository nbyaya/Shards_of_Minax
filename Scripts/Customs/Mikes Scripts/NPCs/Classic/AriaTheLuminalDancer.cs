using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AriaTheLuminalDancer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public AriaTheLuminalDancer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aria the Luminal Dancer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(50);
        SetDex(130);
        SetInt(120);
        SetHits(80);

        // Appearance
        AddItem(new FancyDress(1187));
        AddItem(new TribalMask(1186));
        AddItem(new Longsword() { Name = "Aria's Whisper" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public AriaTheLuminalDancer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Aria, the Luminal Dancer, a visitor from distant realms filled with shimmering lights. How may I enlighten you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I am Aria the Luminal Dancer, a visitor from distant realms! I dance among the stars, sharing tales of light and harmony.");
                whoModule.AddOption("Tell me more about your realms.",
                    p => true,
                    p =>
                    {
                        DialogueModule realmsModule = new DialogueModule("The realms I hail from are known as Lumina, a dimension of light and energy where everything flows in perfect harmony. Unlike this world, Lumina is not bound by the physical limitations you know. There, emotions and thoughts shape reality, and the very air shimmers with the glow of our collective energies.");
                        realmsModule.AddOption("How does life in Lumina differ from here?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule lifeModule = new DialogueModule("In Lumina, there is no concept of day and night as you have here. Instead, the sky is filled with radiant colors that shift based on the emotions of the people. Our homes are formed from pure energy, and they transform depending on the needs and desires of their inhabitants. There is no conflict, for we communicate through a deeper understanding that transcends words.");
                                lifeModule.AddOption("What do you mean by emotions shaping reality?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule emotionModule = new DialogueModule("In Lumina, emotions are a powerful force. When we feel joy, the environment around us blossoms with vibrant hues and gentle melodies. When sorrow touches us, the world dims, and the lights grow softer. It is said that our ancestors learned to harness these emotions to create structures, heal the wounded, and even travel across realms. Imagine a place where your happiness could literally make flowers bloom around you.");
                                        emotionModule.AddOption("That sounds incredible. Can anyone learn this?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule learnModule = new DialogueModule("Yes, though it requires deep introspection and control over one's inner self. Only those who have truly mastered their emotions can shape the world around them. The ancient Luminals trained for decades to reach this level of harmony. It is not an easy path, but those who persevere are rewarded with the ability to bring their inner world to life.");
                                                learnModule.AddOption("Tell me about the ancient Luminals.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule ancientModule = new DialogueModule("The ancient Luminals were the first beings to harness the energy of Lumina. They were philosophers, artists, and explorers who sought to understand the fabric of our dimension. They built great monuments of light that still stand today, each one a testament to their understanding of harmony. Some say they even discovered how to merge their essence with the stars, becoming one with the universe.");
                                                        ancientModule.AddOption("Merge with the stars? What does that mean?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule starsModule = new DialogueModule("To merge with the stars is to become one with the cosmic energy that flows through all things. The ancient Luminals believed that by achieving perfect harmony within themselves, they could transcend their physical forms and join the stars, where they would exist as beings of pure light and knowledge. It is the ultimate form of enlightenment, where the self is no longer separate from the universe.");
                                                                starsModule.AddOption("Is that something you aspire to?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule aspireModule = new DialogueModule("Perhaps one day. For now, I am content to dance among the stars and share my light with those I meet. The journey to enlightenment is a long one, and I still have much to learn. But every step, every dance, brings me closer to understanding the mysteries of Lumina.");
                                                                        aspireModule.AddOption("Thank you for sharing your story.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, aspireModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, starsModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, ancientModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, learnModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, emotionModule));
                                    });
                                lifeModule.AddOption("What do people do in Lumina?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule activitiesModule = new DialogueModule("In Lumina, we pursue the arts, meditation, and exploration of the inner and outer worlds. There are no boundaries between what you might call magic and science. Artists paint with light, and musicians compose symphonies that echo across the dimension, resonating with the energy of all who hear them. Every Luminal contributes to the collective beauty of our realm, striving to uplift one another.");
                                        activitiesModule.AddOption("It sounds like a utopia.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, activitiesModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, lifeModule));
                            });
                        p.SendGump(new DialogueGump(p, realmsModule));
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("What is harmony?",
            player => true,
            player =>
            {
                DialogueModule harmonyModule = new DialogueModule("Harmony is the balance between the mind, body, and spirit. I have learned the ways of inner peace from the ancient sages of my realm. They hold ancient secrets. Would you like to know more?");
                harmonyModule.AddOption("Tell me about these ancient secrets.",
                    p => true,
                    p =>
                    {
                        DialogueModule secretsModule = new DialogueModule("The ancient sages held the power to merge with the universe, listening to its whispers and understanding its desires. They left behind clues for the worthy. Seek the monolith of echoes if you wish to uncover their teachings.");
                        secretsModule.AddOption("I will seek the monolith.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, secretsModule));
                    });
                player.SendGump(new DialogueGump(player, harmonyModule));
            });

        greeting.AddOption("What about the stars?",
            player => true,
            player =>
            {
                DialogueModule starsModule = new DialogueModule("The stars hold stories and mysteries that few dare to explore. I have danced on many a star, absorbing their tales and wisdom. One such tale speaks of a hidden treasure.");
                starsModule.AddOption("Tell me about the hidden treasure.",
                    p => true,
                    p =>
                    {
                        DialogueModule treasureModule = new DialogueModule("The treasure is not of gold or gems, but of knowledge and memories of ages past. Many have searched for it, but only those pure of heart have found it. Will you embark on this quest?");
                        treasureModule.AddOption("I will embark on the quest.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You feel a sense of purpose fill your heart as you prepare for the journey.");
                            });
                        treasureModule.AddOption("I need more time to decide.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, treasureModule));
                    });
                player.SendGump(new DialogueGump(player, starsModule));
            });

        greeting.AddOption("Can you teach me about the sacred dance?",
            player => true,
            player =>
            {
                DialogueModule danceModule = new DialogueModule("The sacred dance is a sequence of movements that align the dancer with the universe's rhythm. When performed under the crescent moon, it is said to bestow visions. Tread carefully, for not all visions are pleasant.");
                danceModule.AddOption("I wish to learn the sacred dance.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Aria gracefully demonstrates the sacred dance, her movements embodying the universe's rhythm.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, danceModule));
            });

        greeting.AddOption("What do you know about lights?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule lightsModule = new DialogueModule("Lights are manifestations of energy, each color and hue representing a different emotion and vibration. When I dance, I channel these vibrations, bringing warmth and joy to all who witness. As thanks for your interest, take this token of appreciation.");
                    lightsModule.AddOption("Thank you for the token.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        });
                    player.SendGump(new DialogueGump(player, lightsModule));
                }
            });

        greeting.AddOption("Farewell, Aria.",
            player => true,
            player =>
            {
                player.SendMessage("Aria smiles warmly and continues her graceful dance.");
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