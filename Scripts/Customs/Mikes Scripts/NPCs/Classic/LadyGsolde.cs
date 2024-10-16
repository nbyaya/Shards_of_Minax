using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lady Gsolde")]
public class LadyGsolde : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LadyGsolde() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lady Gsolde";
        Body = 0x191; // Human female body

        // Stats
        SetStr(152);
        SetDex(64);
        SetInt(24);
        SetHits(107);

        // Appearance
        AddItem(new LeatherSkirt() { Hue = 1300 });
        AddItem(new ChainChest() { Hue = 1300 });
        AddItem(new PlateHelm() { Hue = 1300 });
        AddItem(new PlateGloves() { Hue = 1300 });
        AddItem(new Boots() { Hue = 1300 });

        Hue = Utility.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public LadyGsolde(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Lady Gsolde, a woman of impeccable taste and refinement. What do you want?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                DialogueModule healthModule = new DialogueModule("Why do you care about my health? It's not as if anyone in this wretched place cares about me.");
                healthModule.AddOption("Is there anything I can do to help?",
                    p => true,
                    p => 
                    {
                        DialogueModule helpModule = new DialogueModule("If only there were something to cure my sadness... Perhaps you could bring me something of value from the outside world?");
                        helpModule.AddOption("What do you need?",
                            pl => true,
                            pl => 
                            {
                                DialogueModule needsModule = new DialogueModule("A rare gemstone or an exquisite flower would brighten my spirits. Would you seek these for me?");
                                needsModule.AddOption("I will find a gemstone.",
                                    pq => true,
                                    pq => 
                                    {
                                        player.SendMessage("You set off in search of a rare gemstone.");
                                    });
                                needsModule.AddOption("I'll look for a beautiful flower.",
                                    pw => true,
                                    pw => 
                                    {
                                        player.SendMessage("You set off to find a beautiful flower.");
                                    });
                                needsModule.AddOption("Perhaps another time.",
                                    pe => true,
                                    pe => 
                                    {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, needsModule));
                            });
                        p.SendGump(new DialogueGump(p, helpModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What do you do here?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("My 'job'? I'm here, aren't I? Isn't that enough to satisfy your curiosity?");
                jobModule.AddOption("What do you do when you're not working?",
                    p => true,
                    p => 
                    {
                        DialogueModule freeTimeModule = new DialogueModule("I reminisce about the grandeur of my past life. I used to attend grand balls and lavish feasts. Now, I merely exist here, surrounded by shadows.");
                        player.SendGump(new DialogueGump(player, freeTimeModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What about battles?",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Do you think you're valiant? Prove it! Tell me, what would you do if you were stuck in a place like this?");
                battlesModule.AddOption("I would rally the people to fight back.",
                    p => true,
                    p => 
                    {
                        DialogueModule rallyModule = new DialogueModule("A noble thought! But do you think the people here still have the spirit to fight? Perhaps a champion is needed to lead them.");
                        rallyModule.AddOption("I can be that champion.",
                            pl => true,
                            pl => 
                            {
                                player.SendMessage("You declare your intention to be a champion.");
                            });
                        p.SendGump(new DialogueGump(p, rallyModule));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("Tell me about the curse.",
            player => true,
            player =>
            {
                DialogueModule curseModule = new DialogueModule("This place, it's a shadow of the world I once knew. Everything changed after the dark mage cast his curse upon my family. Do you know of this curse?");
                curseModule.AddOption("No, tell me more.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule detailsModule = new DialogueModule("The curse has drained the life and color from this land. My family was once powerful and respected, but now we are mere echoes of our former selves.");
                        player.SendGump(new DialogueGump(player, detailsModule));
                    });
                player.SendGump(new DialogueGump(player, curseModule));
            });

        greeting.AddOption("What do you know of the enchanted amulet?",
            player => true,
            player =>
            {
                DialogueModule amuletModule = new DialogueModule("Being stuck is a mindset. My great-grandfather once told me about an enchanted amulet that could free a person's spirit. Would you like to hear about it?");
                amuletModule.AddOption("Yes, what does it do?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule powerModule = new DialogueModule("The amulet is said to grant the wearer immense wisdom and the ability to see the true nature of their surroundings. But it is lost, hidden away in a place few dare to tread.");
                        powerModule.AddOption("Where can I find it?",
                            p => true,
                            p => 
                            {
                                DialogueModule locationModule = new DialogueModule("Legends speak of it being guarded by a fearsome beast in the depths of the Whispering Forest. Many have sought it, but few return.");
                                player.SendGump(new DialogueGump(player, locationModule));
                            });
                        pl.SendGump(new DialogueGump(pl, powerModule));
                    });
                player.SendGump(new DialogueGump(player, amuletModule));
            });

        greeting.AddOption("Tell me about your misery.",
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
                    DialogueModule miseryModule = new DialogueModule("Misery can be a powerful motivator. There's a hidden chamber within the Fallen Keep that holds a treasure. I can tell you more, but only if you prove your loyalty. Bring me a raven's feather, and I shall reveal its secrets. For your effort, you shall be rewarded.");
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.AddToBackpack(new TasteIDAugmentCrystal()); // Give the reward
                    player.SendGump(new DialogueGump(player, miseryModule));
                }
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
