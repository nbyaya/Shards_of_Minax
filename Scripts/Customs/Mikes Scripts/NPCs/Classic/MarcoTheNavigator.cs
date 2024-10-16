using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Marco the Navigator")]
public class MarcoTheNavigator : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MarcoTheNavigator() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marco the Navigator";
        Body = 0x190; // Human male body

        // Stats
        SetStr(50);
        SetDex(50);
        SetInt(70);
        SetHits(70);

        // Appearance
        AddItem(new LongPants() { Hue = 2126 });
        AddItem(new Doublet() { Hue = 1904 });
        AddItem(new ThighBoots() { Hue = 1904 });
        AddItem(new TricorneHat() { Hue = 2126 });
        AddItem(new Cloak() { Name = "Marco's Cloak" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public MarcoTheNavigator(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Marco the Navigator, a cartographer. What brings you to this part of the world?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutMe = new DialogueModule("Ah, where to begin? My journey as a navigator has taken me across vast oceans and through dense forests. I am deeply passionate about charting the unknown.");
                aboutMe.AddOption("What inspired you to become a navigator?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Ever since I was a child, I was fascinated by tales of explorers and their adventures. The idea of discovering new lands and cultures sparked a fire within me.")));
                    });
                aboutMe.AddOption("What are your greatest achievements?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("I mapped the treacherous waters of the Eastern Sea and uncovered a hidden isle filled with rare resources. Many adventurers owe their safe travels to my maps.")));
                    });
                player.SendGump(new DialogueGump(player, aboutMe));
            });

        greeting.AddOption("What do you know about exploration?",
            player => true,
            player =>
            {
                DialogueModule explorationModule = new DialogueModule("Exploration is not merely about discovery; it’s about understanding the stories behind every place. Each map I draw is filled with history and adventure.");
                explorationModule.AddOption("What is the greatest challenge you've faced?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("The fiercest storm I've encountered nearly sunk my ship. It taught me the value of respect for nature and the importance of preparation.")));
                    });
                explorationModule.AddOption("Tell me about the places you've been.",
                    p => true,
                    p =>
                    {
                        DialogueModule placesModule = new DialogueModule("I've traveled to the Frosted Peaks, the Burning Sands, and the Whispering Woods. Each place has its unique beauty and hidden dangers.");
                        placesModule.AddOption("Which place was the most dangerous?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("The Frosted Peaks are unforgiving. The blizzards can erase trails, and the creatures there are as fierce as they are majestic.")));
                            });
                        placesModule.AddOption("Which place was the most beautiful?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("The sunsets over the Emerald Coast are breathtaking. The sky ignites with colors that even the finest artists cannot replicate.")));
                            });
                        player.SendGump(new DialogueGump(player, placesModule));
                    });
                player.SendGump(new DialogueGump(player, explorationModule));
            });

        greeting.AddOption("Do you have any maps?",
            player => true,
            player =>
            {
                DialogueModule mapModule = new DialogueModule("Indeed! I always have maps available for those in search of adventure. I recently found an old map leading to a hidden treasure. Would you like to see it?");
                mapModule.AddOption("Yes, please!",
                    pl => CanGiveReward(pl),
                    pl =>
                    {
                        pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Here is the map! Best of luck on your adventures.")));
                    });
                mapModule.AddOption("What else do you have?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule moreMapsModule = new DialogueModule("I have maps of various regions: the Shadowlands, the Crystal Caverns, and the Serpent's Spine Mountains. Each one is a doorway to new adventures.");
                        moreMapsModule.AddOption("Tell me about the Shadowlands.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The Shadowlands are filled with ancient ruins and dark secrets. Many who enter never return, but those who do have tales that chill the soul.")));
                            });
                        moreMapsModule.AddOption("Tell me about the Crystal Caverns.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The Crystal Caverns sparkle with luminescent stones, but beware! They are home to many mystical creatures that guard their treasures fiercely.")));
                            });
                        moreMapsModule.AddOption("Tell me about the Serpent's Spine Mountains.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The Serpent's Spine is a treacherous range, known for its winding paths and fierce storms. Only the brave dare to venture there.")));
                            });
                        player.SendGump(new DialogueGump(player, moreMapsModule));
                    });
                player.SendGump(new DialogueGump(player, mapModule));
            });

        greeting.AddOption("What about uncharted regions?",
            player => true,
            player =>
            {
                DialogueModule unchartedModule = new DialogueModule("Ah, the uncharted regions hold the greatest mysteries! They beckon adventurers with promises of glory and treasure, but they often come with peril.");
                unchartedModule.AddOption("What is the greatest mystery you've encountered?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("I once stumbled upon ruins of a civilization long forgotten. Their writings spoke of a powerful artifact that could alter the very fabric of reality.")));
                    });
                unchartedModule.AddOption("Are there dangers in these regions?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Indeed! Many adventurers have fallen prey to both nature and the creatures that dwell in these areas. It requires skill and caution to navigate.")));
                    });
                player.SendGump(new DialogueGump(player, unchartedModule));
            });

        return greeting;
    }

    private bool CanGiveReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
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
