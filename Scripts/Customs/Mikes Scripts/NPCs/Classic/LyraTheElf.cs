using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class LyraTheElf : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LyraTheElf() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lyra the Elf";
        Body = 0x191; // Human female body

        // Stats
        SetStr(60);
        SetDex(100);
        SetInt(60);
        SetHits(50);

        // Appearance
        AddItem(new LeatherSkirt() { Hue = 1195 });
        AddItem(new LeatherBustierArms() { Hue = 1195 });
        AddItem(new LeatherCap() { Hue = 1195 });
        AddItem(new Boots() { Hue = 1195 });
        AddItem(new BambooFlute() { Name = "Lyra's Flute" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime
        SpeechHue = 0; // Default speech hue
    }

    public LyraTheElf(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Lyra the Elf, a bard with a song in my heart. How can I enchant your day?");
        
        greeting.AddOption("How are you today?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I am in good health, thank you for asking. The melodies of the forest keep my spirit alive.")));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job is to spread tales and melodies throughout the land. But tell me, do you have a favorite story?");
                jobModule.AddOption("I love tales of adventure.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, adventure! Those are the tales that inspire us to seek out our own journeys. Would you like to hear one?")));
                    });
                jobModule.AddOption("I prefer tales of romance.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Romance, how delightful! Love is a melody that transcends time. Let me share a beautiful story of a star-crossed love.")));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Tell me about battles.",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Life's grandest battles are often fought with words and melodies. Do you appreciate the power of art?");
                battlesModule.AddOption("Yes, I do.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule yesModule = new DialogueModule("Then you understand the true magic of the world, where every word and note can shape destiny. Would you like to hear a song about a legendary battle?");
                        yesModule.AddOption("Absolutely! I love a good song.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Here's a tale of the great warrior Aelthar, who fought valiantly against a dragon to save his village. The clash of steel and the roar of flames echoed through the mountains.")));
                            });
                        yesModule.AddOption("Maybe another time.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Very well. The songs will always be here, waiting for the right moment.")));
                            });
                        pl.SendGump(new DialogueGump(pl, yesModule));
                    });
                battlesModule.AddOption("No, not really.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, perhaps you will one day. Music has its way of reaching the heart, even in unexpected moments.")));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("What about elves?",
            player => true,
            player =>
            {
                DialogueModule elvesModule = new DialogueModule("Ah, elves! We have an affinity with nature and magic. Our age-old tales are sung from the ancient woods to the shimmering shores. What interests you about elves?");
                elvesModule.AddOption("I admire your connection to nature.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Nature is our greatest muse. The rustling leaves and flowing rivers inspire many of our songs. Have you ever felt the call of the wild?")));
                    });
                elvesModule.AddOption("Tell me about elven magic.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Elven magic is woven into the fabric of the world. We use it to heal, to protect, and to enhance our art. Would you like to learn a spell or two?")));
                    });
                player.SendGump(new DialogueGump(player, elvesModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later, when the sun rises on a new day.")));
                }
                else
                {
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.AddToBackpack(new MaxxiaScroll()); // Replace with actual item
                    player.SendGump(new DialogueGump(player, new DialogueModule("Indeed, the mirror holds many secrets. Here, I have a special looking glass for you, a gift for those who truly understand the essence of art.")));
                }
            });

        greeting.AddOption("Tell me about art.",
            player => true,
            player =>
            {
                DialogueModule artModule = new DialogueModule("Art is the mirror of our soul, reflecting both our deepest fears and highest hopes. What aspect of art intrigues you the most?");
                artModule.AddOption("The creation of art fascinates me.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Creating art is a journey of the heart. Each stroke and note carries a piece of the artist's spirit. Would you like to try your hand at it?")));
                    });
                artModule.AddOption("I prefer appreciating art.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the appreciation of art is a gift in itself. It allows us to connect with the emotions and stories woven into each creation. Do you have a favorite artist?")));
                    });
                player.SendGump(new DialogueGump(player, artModule));
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
