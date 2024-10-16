using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lyria Shadowsong")]
public class LyriaShadowsong : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LyriaShadowsong() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lyria Shadowsong";
        Body = 0x191; // Female body

        // Stats
        SetStr(80);
        SetDex(120);
        SetInt(70);
        SetHits(65);

        // Appearance
        AddItem(new NinjaTabi() { Hue = 1900 });
        AddItem(new LongPants() { Hue = 1900 });
        AddItem(new FancyShirt() { Hue = 1900 });
        AddItem(new LeatherGorget() { Hue = 1900 });
        AddItem(new Kama() { Name = "Lyria's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
    }

    public LyriaShadowsong(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Lyria Shadowsong, the outcast. What do you wish to discuss?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
                player.SendGump(new DialogueGump(player, new DialogueModule("My health is a constant reminder of the darkness that surrounds me. The shadows whisper secrets, yet they drain my vitality.")))
        );

        greeting.AddOption("What is your job?",
            player => true,
            player => 
                player.SendGump(new DialogueGump(player, new DialogueModule("I dwell in the shadows, shunned by my kin and society. I act as a bridge between the seen and unseen, trading in secrets.")))
        );

        greeting.AddOption("What do you know about darkness?",
            player => true,
            player => 
            {
                DialogueModule darknessModule = new DialogueModule("Do you seek power or revenge, stranger? The darkness can be a seductive mistress.");
                darknessModule.AddOption("I seek power.",
                    p => true,
                    p => 
                    {
                        DialogueModule powerModule = new DialogueModule("Power comes with a price. Are you willing to sacrifice your morals for strength?");
                        powerModule.AddOption("I am willing.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Then seek out the forbidden texts buried deep in the shadows. But beware, they may consume you.")));
                            });
                        powerModule.AddOption("No, I value my morals.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Wise choice. The true power lies in knowing oneself and not losing sight of what is right.")));
                            });
                        p.SendGump(new DialogueGump(p, powerModule));
                    });
                darknessModule.AddOption("I seek revenge.",
                    p => true,
                    p => 
                    {
                        DialogueModule revengeModule = new DialogueModule("Revenge is a double-edged sword. Whom do you wish to avenge?");
                        revengeModule.AddOption("Those who wronged me.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Revenge may bring temporary satisfaction, but it often leads to more pain. Think carefully before proceeding.")));
                            });
                        revengeModule.AddOption("My enemies in the Shadowsong clan.",
                            pl => true,
                            pl => 
                            {
                                DialogueModule clanModule = new DialogueModule("The Shadowsong clan is notorious for their betrayal. To succeed, you must learn their weaknesses.");
                                clanModule.AddOption("How can I learn their weaknesses?",
                                    p1 => true,
                                    p1 => 
                                    {
                                        p1.SendGump(new DialogueGump(p1, new DialogueModule("There are ancient tomes in their archives. If you can infiltrate, you might find valuable knowledge.")));
                                    });
                                p.SendGump(new DialogueGump(p, clanModule));
                            });
                        p.SendGump(new DialogueGump(p, revengeModule));
                    });
                player.SendGump(new DialogueGump(player, darknessModule));
            });

        greeting.AddOption("Tell me about your clan.",
            player => true,
            player => 
                player.SendGump(new DialogueGump(player, new DialogueModule("The Shadowsong clan was known for its mastery over the elements and its close bond with nature. However, my defiance led to my downfall.")))
        );

        greeting.AddOption("Do you have any forbidden knowledge?",
            player => true,
            player => 
            {
                DialogueModule forbiddenModule = new DialogueModule("The forbidden magics I delved into were ancient and powerful. Few dared to even speak of them, let alone practice. But if you are truly interested, I might teach you... for a price.");
                forbiddenModule.AddOption("Yes, I'm interested.",
                    p => true,
                    p => 
                    {
                        DialogueModule priceModule = new DialogueModule("Knowledge has its costs. I will require a rare artifact in exchange for my teachings. Will you fetch it?");
                        priceModule.AddOption("What artifact?",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("The Gem of Shadows, said to enhance one's affinity for dark magics. Bring it to me, and I will share my secrets.")));
                            });
                        priceModule.AddOption("No, I change my mind.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, priceModule));
                    });
                forbiddenModule.AddOption("No, thank you.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, forbiddenModule));
            });

        greeting.AddOption("What about revenge?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.AddToBackpack(new MusicianshipAugmentCrystal()); // Give the reward
                    player.SendGump(new DialogueGump(player, new DialogueModule("My vendetta is against those in the clan who shunned me. Take this as a token of potential partnership.")));
                }
            });

        greeting.AddOption("What do you know about shadows?",
            player => true,
            player => 
            {
                DialogueModule shadowsModule = new DialogueModule("The shadows are more than just a hiding place for me. They are allies, sources of strength, and occasionally, tormentors.");
                shadowsModule.AddOption("Can you teach me to harness their power?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule teachModule = new DialogueModule("Harnessing shadows is a delicate balance. It requires focus and strength. Do you have the resolve to withstand the darkness?");
                        teachModule.AddOption("I have the resolve!",
                            p1 => true,
                            p1 => 
                            {
                                p1.SendGump(new DialogueGump(p1, new DialogueModule("Very well. Begin by learning to meditate in the dark. Focus on the shadows around you; they will guide you.")));
                            });
                        teachModule.AddOption("I'm not sure anymore.",
                            p1 => true,
                            p1 => 
                            {
                                p1.SendGump(new DialogueGump(p1, new DialogueModule("Doubt can weaken your resolve. If you wish to learn, you must fully commit.")));
                            });
                        player.SendGump(new DialogueGump(player, teachModule));
                    });
                player.SendGump(new DialogueGump(player, shadowsModule));
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
