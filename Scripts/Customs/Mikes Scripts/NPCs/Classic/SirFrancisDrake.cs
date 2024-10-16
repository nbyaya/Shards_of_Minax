using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

[CorpseName("the corpse of Sir Francis Drake")]
public class SirFrancisDrake : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SirFrancisDrake() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sir Francis Drake";
        Body = 0x190; // Human male body

        // Stats
        SetStr(110);
        SetDex(100);
        SetInt(60);
        SetHits(80);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1105 });
        AddItem(new LeatherChest() { Hue = 1105 });
        AddItem(new LeatherGloves() { Hue = 1105 });
        AddItem(new TricorneHat() { Hue = 1105 });
        AddItem(new Boots() { Hue = 1105 });
        AddItem(new Cutlass() { Name = "Sir Drake's Blade" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("Ahoy there, I be Sir Francis Drake! How can I assist ye?");

        greeting.AddOption("Tell me about your adventures.",
            player => true,
            player =>
            {
                DialogueModule adventuresModule = new DialogueModule("Ah, me adventures be many! But let me tell ye of the time I was pulled through a time portal and served under Minax, the dark sorceress.");
                adventuresModule.AddOption("What was it like working for Minax?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule minaxModule = new DialogueModule("Working for Minax be like sailing through a storm with no compass. She offered power and riches, but the cost was me soul.");
                        minaxModule.AddOption("How did ye escape her clutches?",
                            p => true,
                            p =>
                            {
                                DialogueModule escapeModule = new DialogueModule("It took cunning and bravery. I gathered a band of fellow captives, and together we plotted to turn her own magic against her. It was a perilous journey, but freedom was worth the risk!");
                                escapeModule.AddOption("What happened during the escape?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule escapeDetails = new DialogueModule("We faced her minions and treacherous landscapes. But through the chaos, I discovered a hidden artifact that granted us passage back through the portal. It be a blade of pure light, forged by ancient hands.");
                                        escapeDetails.AddOption("Tell me more about the artifact.",
                                            pw => true,
                                            pw => 
                                            {
                                                player.SendGump(new DialogueGump(player, new DialogueModule("This blade be known as the Lightbringer. It cuts through darkness and can dispel magic. If ye ever find yerself in a pinch, seek it out!")));
                                            });
                                        player.SendGump(new DialogueGump(player, escapeDetails));
                                    });
                                player.SendGump(new DialogueGump(player, escapeModule));
                            });
                        player.SendGump(new DialogueGump(player, minaxModule));
                    });
                player.SendGump(new DialogueGump(player, adventuresModule));
            });

        greeting.AddOption("What can you tell me about the cursed isle?",
            player => true,
            player =>
            {
                DialogueModule cursedIsleModule = new DialogueModule("That cursed isle, filled with dangerous creatures and hidden traps. But the legend says there's a gem there that can control the seas. Seek it out if ye dare!");
                cursedIsleModule.AddOption("What dangers await on the isle?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ye may face vicious sea serpents and spirits of the damned. But beware the whispers that call ye deeper into the darkness!")));
                    });
                player.SendGump(new DialogueGump(player, cursedIsleModule));
            });

        greeting.AddOption("Can you tell me about Tortuga?",
            player => true,
            player =>
            {
                DialogueModule tortugaModule = new DialogueModule("Ah, Tortuga! A haven for pirates like me. I once won a mysterious amulet in a dice game there. If you help me with a small task, it's yours.");
                tortugaModule.AddOption("What task do you need help with?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule taskModule = new DialogueModule("I need ye to find a rare gemstone from the ruins nearby. The amulet be tied to its power, and only then can I part with it.");
                        taskModule.AddOption("Where can I find this gemstone?",
                            p => true,
                            p => 
                            {
                                player.SendGump(new DialogueGump(player, new DialogueModule("The gemstone lies deep within the caverns of the Forgotten Isles. Many have sought it, but few have returned!")));
                            });
                        player.SendGump(new DialogueGump(player, taskModule));
                    });
                player.SendGump(new DialogueGump(player, tortugaModule));
            });

        greeting.AddOption("What do you know about the gem?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Legend speaks of its power to command the waves and calm the fiercest storms. But many have sought it and failed. Return it to me, and I'll grant you a piece of me own treasure.")));
            });

        return greeting;
    }

    public SirFrancisDrake(Serial serial) : base(serial) { }

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
