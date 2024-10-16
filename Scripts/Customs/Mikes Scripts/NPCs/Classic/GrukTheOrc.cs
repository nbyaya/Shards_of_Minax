using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Gruk the Orc")]
public class GrukTheOrc : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public GrukTheOrc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Gruk the Orc";
        Body = 0x190; // Orc body
        Hue = 0x835; // Default orc hue

        // Stats
        Str = 100;
        Dex = 60;
        Int = 60;
        Hits = 70;

        // Appearance
        AddItem(new PlateChest() { Name = "Gruk's Chestplate" });
        AddItem(new PlateArms() { Name = "Gruk's Armguards" });
        AddItem(new PlateLegs() { Name = "Gruk's Leggings" });
        AddItem(new PlateHelm() { Name = "Gruk's Helmet" });
        AddItem(new PlateGloves() { Name = "Gruk's Gauntlets" });
        AddItem(new Drums() { Name = "Gruk's Drum" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(false);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public GrukTheOrc(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Gruk the Orc, the mighty drummer! What brings you to my presence?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                DialogueModule healthModule = new DialogueModule("My health is robust, for I drum with great vigor! Would you like to know more about my lifestyle?");
                healthModule.AddOption("Yes, please.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("I consume hearty meals of meat and vegetables, and I practice drumming daily to keep my spirits high!")));
                    });
                healthModule.AddOption("No, that's enough.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, greeting));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("My job is to keep the rhythm alive, with every beat of my drum! It's not just a job; it's my passion!");
                jobModule.AddOption("Do you perform often?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Indeed! I perform at clan gatherings, festivals, and sometimes even for travelers like yourself!")));
                    });
                jobModule.AddOption("What do you enjoy about it?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("The joy it brings to others fills my heart! Music connects us all, regardless of our differences.")));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think about music?",
            player => true,
            player => 
            {
                DialogueModule musicModule = new DialogueModule("Music can touch the soul and reveal one's true spirit. Dost thou understand the power of music?");
                musicModule.AddOption("Yes.",
                    p => true,
                    p => 
                    {
                        DialogueModule yesModule = new DialogueModule("Indeed, the power of music transcends words. Let us drum together in harmony! Do you play any instruments?");
                        yesModule.AddOption("I play the lute.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the lute! A fine instrument. Would you care to join me in a duet?")));
                            });
                        yesModule.AddOption("I only sing.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Singing is a beautiful form of expression! Your voice would blend well with my drumming!")));
                            });
                        yesModule.AddOption("I don't play anything.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("No matter! Music can be enjoyed by all, not just those who play instruments.")));
                            });
                        p.SendGump(new DialogueGump(p, yesModule));
                    });
                musicModule.AddOption("No, I don't.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Then allow me to enlighten you! Music can be a force of great change and unity.")));
                    });
                player.SendGump(new DialogueGump(player, musicModule));
            });

        greeting.AddOption("Tell me about your clan.",
            player => true,
            player => 
            {
                DialogueModule clanModule = new DialogueModule("Drumming Clan, yes. We believe in unity and brotherhood. Our drums carry our message far and wide. Would you like to hear about our traditions?");
                clanModule.AddOption("Yes, please!",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("We hold gatherings where all members share their skills and stories. It's a time of joy and learning!")));
                    });
                clanModule.AddOption("No, I think I've heard enough.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, greeting));
                    });
                player.SendGump(new DialogueGump(player, clanModule));
            });

        greeting.AddOption("Can I receive a reward?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("The power of music heals and inspires. For understanding this, I shall give you a reward!")));
                player.AddToBackpack(new MaxxiaScroll()); // Replace with actual item
                lastRewardTime = DateTime.UtcNow;
            });

        greeting.AddOption("What about unity?",
            player => true,
            player => 
            {
                DialogueModule unityModule = new DialogueModule("Unity is strength. When we drum together, even the heavens can hear us! But tell me, what does unity mean to you?");
                unityModule.AddOption("It means working together.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Yes! When we combine our efforts, we can achieve great things!")));
                    });
                unityModule.AddOption("It means being strong alone.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("A noble sentiment! But remember, even the strongest trees need the support of their roots.")));
                    });
                player.SendGump(new DialogueGump(player, unityModule));
            });

        greeting.AddOption("What do you think about nature?",
            player => true,
            player => 
            {
                DialogueModule natureModule = new DialogueModule("Nature is the source of all life and inspiration! It provides us with everything we need. Do you feel connected to nature?");
                natureModule.AddOption("Yes, very much.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("That is wonderful! Nature's beauty reflects in the music we create.")));
                    });
                natureModule.AddOption("Not really.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Ah, perhaps spending time outdoors would help you feel its rhythm.")));
                    });
                player.SendGump(new DialogueGump(player, natureModule));
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
