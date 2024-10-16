using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CaoCao : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CaoCao() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cao Cao";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(100);
        SetInt(80);

        SetHits(70);
        SetMana(100);
        SetStam(100);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 10;

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1175 });
        AddItem(new LeatherChest() { Hue = 1175 });
        AddItem(new LeatherGloves() { Hue = 1175 });
        AddItem(new LeatherCap() { Hue = 1175 });
        AddItem(new Boots() { Hue = 1175 });
        AddItem(new Broadsword() { Name = "Cao Cao's Sword" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public CaoCao(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Cao Cao, a humble scholar from the East. What knowledge do you seek?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Cao Cao, a seeker of wisdom. I have journeyed from the distant city of Luoyang, where the ancient libraries hold secrets of the past. I am also known as a leader, and my ambition is to bring unity to the land.");
                aboutModule.AddOption("What motivated you to try and unify China?",
                    p => true,
                    p =>
                    {
                        DialogueModule motivationModule = new DialogueModule("The land was in chaos, divided among warlords who cared little for the common people. I believed that only through unity could we bring peace and stability to the land. It was not an easy path, and many battles were fought.");
                        motivationModule.AddOption("Tell me about the major battles you fought.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule battlesModule = new DialogueModule("Ah, the battles were many, and each one was a test of strategy and resilience. One of the greatest battles was the Battle of Guandu, where I faced off against Yuan Shao, a formidable warlord with superior numbers. It was a pivotal moment, where strategy and timing allowed us to overcome a seemingly impossible challenge.");
                                battlesModule.AddOption("How did you manage to win against Yuan Shao?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule guanduModule = new DialogueModule("Yuan Shao had numbers, but he lacked decisiveness and adaptability. I cut off his supply lines, causing his troops to falter. The turning point came when Xu You, a former advisor of Yuan Shao, defected to my side and revealed crucial information about their supply depot. With this knowledge, we struck swiftly, and the tide turned in our favor.");
                                        guanduModule.AddOption("What happened after the Battle of Guandu?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule aftermathModule = new DialogueModule("After Guandu, Yuan Shao's forces were in disarray, and it allowed us to gain control of the northern territories. But the road to unification was still fraught with challenges. The warlords Liu Bei and Sun Quan also had their own ambitions, and they would become my next adversaries.");
                                                aftermathModule.AddOption("Tell me about your conflicts with Liu Bei and Sun Quan.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule redCliffsModule = new DialogueModule("The Battle of Red Cliffs was perhaps my most humbling defeat. Liu Bei and Sun Quan formed an alliance against me, and their strategist, Zhuge Liang, along with Zhou Yu, outmaneuvered my forces. The southern waters were treacherous, and their fire attack devastated my fleet. It was a bitter lesson on the dangers of overextending one's power.");
                                                        redCliffsModule.AddOption("How did you recover from the defeat at Red Cliffs?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule recoveryModule = new DialogueModule("After the defeat at Red Cliffs, I focused on consolidating my power in the north. I learned to be cautious and to respect the strength of alliances. I fortified my holdings, improved my administration, and continued to seek capable men to serve under me. It was through resilience and adaptation that I was able to maintain my influence.");
                                                                recoveryModule.AddOption("Who were some of the capable men who served you?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule generalsModule = new DialogueModule("I was fortunate to have many talented individuals by my side. Xu Chu was my loyal bodyguard, a warrior of great strength. Xiahou Dun, my cousin, was a fierce general who would charge fearlessly into battle. Xun Yu and Guo Jia were brilliant strategists, their counsel invaluable in times of need. It is through the strength of such men that I was able to pursue my ambitions.");
                                                                        generalsModule.AddOption("What was your relationship like with your officers?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule relationshipModule = new DialogueModule("I valued talent above all else. I treated those who were capable with respect and rewarded them handsomely. However, I was also known to be strict, for I believed that discipline was necessary to achieve greatness. There were times when my decisions were harsh, but it was all for the greater good of unifying the land.");
                                                                                relationshipModule.AddOption("Did you ever doubt your mission?",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        DialogueModule doubtModule = new DialogueModule("There were moments of doubt, especially when the losses were great, and the burden of leadership weighed heavily upon me. The chaos of war brings much suffering, and there were times I questioned if the end justified the means. But I always reminded myself that a united land would bring an end to the endless cycles of war and suffering.");
                                                                                        doubtModule.AddOption("Your resolve is admirable.",
                                                                                            plh => true,
                                                                                            plh =>
                                                                                            {
                                                                                                plh.SendGump(new DialogueGump(plh, CreateGreetingModule()));
                                                                                            });
                                                                                        plg.SendGump(new DialogueGump(plg, doubtModule));
                                                                                    });
                                                                                relationshipModule.AddOption("Thank you for sharing your story.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, relationshipModule));
                                                                            });
                                                                        generalsModule.AddOption("Thank you for telling me about your officers.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, generalsModule));
                                                                    });
                                                                recoveryModule.AddOption("Thank you for the insight.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, recoveryModule));
                                                            });
                                                        redCliffsModule.AddOption("That must have been difficult.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, redCliffsModule));
                                                    });
                                                aftermathModule.AddOption("Thank you for sharing your experiences.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, aftermathModule));
                                            });
                                        guanduModule.AddOption("Thank you for the story.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, guanduModule));
                                    });
                                battlesModule.AddOption("That sounds fascinating.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, battlesModule));
                            });
                        motivationModule.AddOption("Thank you for sharing your motivations.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, motivationModule));
                    });
                aboutModule.AddOption("Tell me about Luoyang.",
                    p => true,
                    p =>
                    {
                        DialogueModule luoyangModule = new DialogueModule("Luoyang is a city of rich history and culture. Its libraries are vast, and its scrolls speak of heroes, wisdom, and the mysteries of the universe.");
                        luoyangModule.AddOption("It sounds like a wonderful place.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, luoyangModule));
                    });
                aboutModule.AddOption("Thank you for the introduction.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you have any wisdom to share?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("Wisdom is the light that dispels ignorance. Meditation and study are key to finding inner peace and understanding the world without illusions.");
                wisdomModule.AddOption("Can you teach me to meditate?",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            DialogueModule rewardModule = new DialogueModule("Meditation brings clarity and peace. Here, take this as a token to begin your journey.");
                            rewardModule.AddOption("Thank you.",
                                pl => true,
                                pl =>
                                {
                                    pl.AddToBackpack(new MaxxiaScroll());
                                    lastRewardTime = DateTime.UtcNow;
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, rewardModule));
                        }
                    });
                wisdomModule.AddOption("Thank you for the wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("What can you tell me about the East?",
            player => true,
            player =>
            {
                DialogueModule eastModule = new DialogueModule("The East is a land of ancient wisdom and mysteries. I come from Luoyang, a city known for its history and its dedication to knowledge.");
                eastModule.AddOption("It must have been a fascinating journey.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, eastModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Cao Cao bows respectfully to you.");
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