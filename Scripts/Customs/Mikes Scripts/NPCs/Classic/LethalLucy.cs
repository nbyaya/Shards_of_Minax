using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Lethal Lucy")]
public class LethalLucy : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LethalLucy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lethal Lucy";
        Body = 0x191; // Female human body

        // Stats
        Str = 155;
        Dex = 62;
        Int = 28;
        Hits = 108;

        // Appearance
        AddItem(new PlainDress() { Hue = 1150 });
        AddItem(new Sandals() { Hue = 1175 });
        AddItem(new Cloak() { Hue = 1120 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
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
        DialogueModule greeting = new DialogueModule("I am Lethal Lucy, the one they whisper about in the dark. How may I assist you?");
        
        greeting.AddOption("Tell me about your health.",
            player => true,
            player => {
                DialogueModule healthModule = new DialogueModule("My health? It's as twisted as my soul.");
                healthModule.AddOption("What do you mean by twisted?",
                    pl => true,
                    pl => {
                        DialogueModule twistedModule = new DialogueModule("It is not just my physical form; my very essence is tainted. I carry burdens that weigh heavily on my spirit.");
                        twistedModule.AddOption("What burdens do you carry?",
                            p => true,
                            p => {
                                DialogueModule burdensModule = new DialogueModule("My past is filled with regrets and sorrows. Each choice I made has led me further into darkness.");
                                p.SendGump(new DialogueGump(p, burdensModule));
                            });
                        pl.SendGump(new DialogueGump(pl, twistedModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => {
                DialogueModule jobModule = new DialogueModule("My job? I'm a harvester of souls, a reaper of hope. It's a dark path I walk.");
                jobModule.AddOption("Why do you call it a dark path?",
                    pl => true,
                    pl => {
                        DialogueModule darkPathModule = new DialogueModule("Because every soul I harvest leaves behind a story untold, a life unfinished. I carry their echoes with me.");
                        pl.SendGump(new DialogueGump(pl, darkPathModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you know about virtue?",
            player => true,
            player => {
                DialogueModule virtueModule = new DialogueModule("The truest virtue is Humility. In my darkness, it's all I have left. Do you understand?");
                virtueModule.AddOption("Yes, I understand.",
                    pl => true,
                    pl => {
                        DialogueModule yesVirtueModule = new DialogueModule("Humility teaches that even in darkness, one can find a glimmer of light. Can you find that glimmer in me?");
                        yesVirtueModule.AddOption("I see it in your eyes.",
                            p => true,
                            p => {
                                DialogueModule eyesModule = new DialogueModule("Thank you for your kindness. It's rare to find someone who looks beyond the surface.");
                                p.SendGump(new DialogueGump(p, eyesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, yesVirtueModule));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("What happened to you?",
            player => true,
            player => {
                DialogueModule pastModule = new DialogueModule("Long ago, I was not the Lethal Lucy you see. I was but a simple maiden, caught in the webs of fate.");
                pastModule.AddOption("What changed you?",
                    pl => true,
                    pl => {
                        DialogueModule changeModule = new DialogueModule("I encountered betrayal from the one I trusted most. It twisted my soul and changed my path forever.");
                        changeModule.AddOption("Who betrayed you?",
                            p => true,
                            p => {
                                DialogueModule betrayalModule = new DialogueModule("My sister, the Enchantress Evelyn, sought power above all else. Her ambition drove a wedge between us.");
                                p.SendGump(new DialogueGump(p, betrayalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, changeModule));
                    });
                player.SendGump(new DialogueGump(player, pastModule));
            });

        greeting.AddOption("Tell me about your sister.",
            player => true,
            player => {
                DialogueModule sisterModule = new DialogueModule("Evelyn resides in a hidden lair. She's the key to breaking my curse, though I've lost hope of reconciliation.");
                sisterModule.AddOption("Can you forgive her?",
                    pl => true,
                    pl => {
                        DialogueModule forgivenessModule = new DialogueModule("Forgiveness is a difficult path. I struggle daily with the weight of my feelings towards her.");
                        pl.SendGump(new DialogueGump(pl, forgivenessModule));
                    });
                player.SendGump(new DialogueGump(player, sisterModule));
            });

        greeting.AddOption("What about the sorcerer?",
            player => true,
            player => {
                DialogueModule sorcererModule = new DialogueModule("Ah, the sorcerer, a being of immense power and cruelty. His name is Malakar, and his magic knows no bounds.");
                sorcererModule.AddOption("What did he do to you?",
                    pl => true,
                    pl => {
                        DialogueModule sorcererActionModule = new DialogueModule("He cursed me, binding my fate to the harvest of souls. I cannot escape it, no matter how hard I try.");
                        pl.SendGump(new DialogueGump(pl, sorcererActionModule));
                    });
                player.SendGump(new DialogueGump(player, sorcererModule));
            });

        greeting.AddOption("Can you give me a reward?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player => {
                player.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                lastRewardTime = DateTime.UtcNow;
                player.SendMessage("Ah, you've proven yourself attentive. Very well. Take this.");
            });

        greeting.AddOption("What do you wish for?",
            player => true,
            player => {
                DialogueModule wishModule = new DialogueModule("I wish for freedom from this curse, to live without the shadows that cling to me.");
                wishModule.AddOption("How can I help you?",
                    pl => true,
                    pl => {
                        DialogueModule helpModule = new DialogueModule("If you can find a way to confront Evelyn or Malakar, perhaps I might find peace. But be warned, both are formidable.");
                        pl.SendGump(new DialogueGump(pl, helpModule));
                    });
                player.SendGump(new DialogueGump(player, wishModule));
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

    public LethalLucy(Serial serial) : base(serial) { }
}
