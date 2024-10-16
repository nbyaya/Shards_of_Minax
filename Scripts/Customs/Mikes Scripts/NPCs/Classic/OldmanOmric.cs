using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Oldman Omric")]
public class OldmanOmric : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public OldmanOmric() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Oldman Omric";
        Body = 0x190; // Human male body

        // Stats
        Str = 78;
        Dex = 45;
        Int = 115;
        Hits = 68;

        // Appearance
        AddItem(new LongPants() { Hue = 38 });
        AddItem(new Tunic() { Hue = 1447 });
        AddItem(new Shoes() { Hue = 2126 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public OldmanOmric(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Oldman Omric, the alchemist.");

        greeting.AddOption("How are you?", 
            player => true, 
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I am in good health, for an old man, but... my back has been troubling me lately.")));
            });

        greeting.AddOption("Tell me about your back pain.", 
            player => true, 
            player => 
            {
                DialogueModule painModule = new DialogueModule("Ah, it is a constant burden. I have to take a special herb every day, or the pain becomes unbearable. Let me tell you more.");
                painModule.AddOption("What kind of herb do you take?", 
                    pl => true, 
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The herb is called the Soothing Sage. It grows in the Moonlit Glade, a mystical place where the moonlight bathes the earth in silvery hues.")));
                    });
                painModule.AddOption("How does it help you?", 
                    pl => true, 
                    pl => 
                    {
                        DialogueModule helpModule = new DialogueModule("The Soothing Sage has properties that ease inflammation and relieve pain. Without it, I struggle to even get out of bed!");
                        helpModule.AddOption("That sounds terrible.", 
                            p => true, 
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Indeed! I often find myself reminiscing about my younger days, filled with vigor and adventure.")));
                            });
                        helpModule.AddOption("Is there anything else you could do?", 
                            p => true, 
                            p => 
                            {
                                DialogueModule optionsModule = new DialogueModule("I have considered seeking out powerful healing potions, but the ingredients are rare and often guarded. I rely on the Soothing Sage for now.");
                                optionsModule.AddOption("What ingredients do you need?", 
                                    pla => true, 
                                    pla => 
                                    {
                                        pla.SendGump(new DialogueGump(pla, new DialogueModule("I would need Crystal Dew, collected at dawn, and the elusive Midnight Blossom, which blooms only at midnight.")));
                                    });
                                p.SendGump(new DialogueGump(p, optionsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, helpModule));
                    });
                painModule.AddOption("How did you come to rely on this herb?", 
                    pl => true, 
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("It all started many years ago. After a particularly harrowing adventure, I was left with injuries that never fully healed. The Soothing Sage became my remedy.")));
                    });
                player.SendGump(new DialogueGump(player, painModule));
            });

        greeting.AddOption("What do you think about kingdoms?", 
            player => true, 
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("There were many great rulers and wars that shaped our history. But through it all, alchemy remained, adapting and evolving with time.")));
            });

        greeting.AddOption("Can you give me an elixir?", 
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10), 
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Would you like a sample? This particular elixir can restore vitality and mend minor wounds. Consider it a gift from an old man.")));
                player.AddToBackpack(new ManaDrainAugmentCrystal()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            });

        greeting.AddOption("What ingredients do you have?", 
            player => true, 
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Some of these are dangerous in the wrong hands. From mandrake roots to phoenix feathers, each has its unique properties.")));
            });

        greeting.AddOption("Tell me about your youth.", 
            player => true, 
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Ah, those were the days! Adventures, close encounters, and the thrill of discovery. I once traveled with a band of heroes seeking the lost city.")));
            });

        return greeting;
    }

    private DialogueModule CreateTaskModule()
    {
        DialogueModule taskModule = new DialogueModule("I need a rare herb, known as the Moonlit Fern, which blooms only under a full moon. Retrieve it for me, and I shall grant you an unspecified reward.");
        taskModule.AddOption("I'll get the Moonlit Fern.", 
            pl => true, 
            pl => 
            {
                pl.SendMessage("You set off to find the Moonlit Fern.");
            });
        taskModule.AddOption("Sounds difficult, maybe later.", 
            pl => true, 
            pl => 
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return taskModule;
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
