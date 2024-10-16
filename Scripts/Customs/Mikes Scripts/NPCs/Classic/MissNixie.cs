using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of Miss Nixie")]
public class MissNixie : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MissNixie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Miss Nixie";
        Body = 0x191; // Human female body

        // Stats
        SetStr(50);
        SetDex(70);
        SetInt(90);
        SetHits(45);

        // Appearance
        AddItem(new LeatherArms() { Name = "Nixie's Leather Sleeves" });
        AddItem(new LeatherChest() { Name = "Nixie's Leather Bustier" });
        AddItem(new LeatherGloves() { Name = "Nixie's Leather Gloves" });
        AddItem(new LeatherLegs() { Name = "Nixie's Leather Skirt" });
        AddItem(new LeatherCap() { Name = "Nixie's Leather Cap" });
        AddItem(new Boots() { Name = "Nixie's Boots" });
        AddItem(new Cloak() { Name = "Nixie's Mortar and Pestle" });

        Hue = Utility.RandomSkinHue();
        HairItemID = Utility.RandomList(0x203B, 0x203C); // Random female hair
        HairHue = Utility.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Ah, a curious traveler! I am Miss Nixie, the cunning thief! What brings you to my corner of the world?");

        greeting.AddOption("What do you do?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("Oh, I specialize in acquiring valuable items, shall we say? My skills allow me to relieve the unwary of their treasures! Are you intrigued?");
                jobModule.AddOption("Yes, tell me more!",
                    p => true,
                    p => 
                    {
                        DialogueModule moreJobModule = new DialogueModule("Thievery is an art, one must have finesse and a quick wit. I’ve pilfered jewels from under the noses of nobles! Do you have the heart for such work?");
                        moreJobModule.AddOption("Absolutely!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well! Let’s discuss some techniques!")));
                            });
                        moreJobModule.AddOption("No, I prefer a safer path.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("A wise choice, perhaps! Life as a thief is fraught with danger. Be cautious!")));
                            });
                        p.SendGump(new DialogueGump(p, moreJobModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Tell me about your escapades.",
            player => true,
            player =>
            {
                DialogueModule escapadesModule = new DialogueModule("Ah, my escapades are legendary! Once, I stole the crown jewels from the very castle itself! Guards were mere steps away.");
                escapadesModule.AddOption("How did you pull that off?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Timing, my friend! Timing is everything. I waited until the guards were busy with a feast, and I slipped in, light as a feather.")));
                    });
                escapadesModule.AddOption("What’s the most valuable thing you’ve taken?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the most precious item? A heart-shaped locket belonging to the duchess. Not for its material worth, but the story it carries...")));
                    });
                player.SendGump(new DialogueGump(player, escapadesModule));
            });

        greeting.AddOption("Do you have any valuable items?",
            player => true,
            player =>
            {
                DialogueModule valuableModule = new DialogueModule("You'd be surprised at the treasures people leave unattended! But the most valuable of all is knowledge. Ever wonder about the secrets of my craft?");
                valuableModule.AddOption("What do you mean?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The art of thievery isn't just about taking; it’s about understanding people and their habits. Would you like to learn a trick or two?")));
                    });
                valuableModule.AddOption("I think I’d rather not.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Wise choice! Knowledge can be a double-edged sword.")));
                    });
                player.SendGump(new DialogueGump(player, valuableModule));
            });

        greeting.AddOption("Can you teach me skills?",
            player => true,
            player =>
            {
                DialogueModule skillsModule = new DialogueModule("If you're truly interested, I might teach you a trick or two! But it'll cost you. Are you willing to pay the price?");
                skillsModule.AddOption("What is the price?",
                    p => true,
                    p =>
                    {
                        if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            p.SendMessage("Very well. Give me a moment... Here, this trinket will aid you in your endeavors. Use it wisely and remember, discretion is key.");
                            p.AddToBackpack(new MaxxiaScroll()); // Assuming MaxxiaScroll is a valid item
                            lastRewardTime = DateTime.UtcNow;
                        }
                    });
                player.SendGump(new DialogueGump(player, skillsModule));
            });

        greeting.AddOption("Why do you steal?",
            player => true,
            player =>
            {
                DialogueModule reasonModule = new DialogueModule("Ah, the thrill of the chase! But also necessity. Life as a thief is not just about greed; it's about survival. Have you ever felt desperation?");
                reasonModule.AddOption("I understand desperation.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Then you grasp the essence of my life! In this world, one must do what is needed to survive.")));
                    });
                reasonModule.AddOption("I prefer not to engage in such activities.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("And that is a noble path! Not everyone has the heart for a life on the edge.")));
                    });
                player.SendGump(new DialogueGump(player, reasonModule));
            });

        greeting.AddOption("Do you fear getting caught?",
            player => true,
            player =>
            {
                DialogueModule fearModule = new DialogueModule("Fear is part of the game! A thief must always be aware of their surroundings. It sharpens the mind. Ever had a close call?");
                fearModule.AddOption("Yes, once I nearly got caught!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the adrenaline rush! Did you manage to escape?")));
                    });
                fearModule.AddOption("No, I’m always careful.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Caution is wise. It’s better to avoid a dangerous situation than to test your luck.")));
                    });
                player.SendGump(new DialogueGump(player, fearModule));
            });

        greeting.AddOption("What are the risks of thievery?",
            player => true,
            player =>
            {
                DialogueModule risksModule = new DialogueModule("Oh, the risks are plenty! From the guards to vengeful victims, one misstep can cost you everything. But the rewards can be great! Are you willing to take those risks?");
                risksModule.AddOption("I live for risks!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("A true thrill-seeker! You might find yourself quite at home in this life.")));
                    });
                risksModule.AddOption("Not really, I prefer stability.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Wise choice! A stable life is often the happiest.")));
                    });
                player.SendGump(new DialogueGump(player, risksModule));
            });

        return greeting;
    }

    public MissNixie(Serial serial) : base(serial) { }

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
