using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class TorkTheThundercaller : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TorkTheThundercaller() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tork the Thundercaller";
        Body = 0x190; // Human male body

        // Stats
        SetStr(130);
        SetDex(100);
        SetInt(60);
        SetHits(100);

        // Appearance
        AddItem(new ChainCoif() { Hue = 1191 });
        AddItem(new WarFork() { Name = "Tork's Spear" });

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
        DialogueModule greeting = new DialogueModule("I am Tork the Thundercaller, a child of storms! Have you ever tasted the wonders of dinosaur meat?");

        greeting.AddOption("Dinosaur meat? Tell me more!",
            player => true,
            player => 
            {
                DialogueModule dinosaurMeatModule = new DialogueModule("Ah, dinosaur meat is a delicacy unlike any other! Its rich flavor and texture are unmatched. I’ve developed a deep fondness for it since my travels began in these strange lands.");
                
                dinosaurMeatModule.AddOption("Where can I find it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule findMeatModule = new DialogueModule("You can find dinosaur meat in the jungles of the Lost Isles. The mighty beasts roam freely, and the thrill of the hunt is invigorating! Just beware of their ferocity.");
                        
                        findMeatModule.AddOption("What types of dinosaurs can I find?",
                            p => true,
                            p =>
                            {
                                DialogueModule typesModule = new DialogueModule("You’ll encounter the agile Velociraptors and the towering Brachiosaurus. Each offers its own unique taste. The raptors are lean and flavorful, while the Brachiosaurus provides a hearty meal that’s perfect for sharing.");
                                
                                typesModule.AddOption("How do you prepare the meat?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule prepareModule = new DialogueModule("Preparation is key! I usually marinate it with herbs and spices I gather from the surrounding flora, then roast it over an open flame. The aroma is irresistible!");
                                        
                                        prepareModule.AddOption("What herbs do you recommend?",
                                            pw => true,
                                            pw =>
                                            {
                                                DialogueModule herbsModule = new DialogueModule("Ah, the best herbs are Wild Thyme and Firebloom. They enhance the meat’s flavor and bring out its natural juices. It’s a feast worthy of the gods!");
                                                herbsModule.AddOption("Can I help you gather some herbs?",
                                                    pla => true,
                                                    pla =>
                                                    {
                                                        pla.SendMessage("You set off to gather herbs, eager to help Tork.");
                                                    });
                                                p.SendGump(new DialogueGump(p, herbsModule));
                                            });
                                        prepareModule.AddOption("What if I don’t know how to cook?",
                                            ple => true,
                                            ple =>
                                            {
                                                DialogueModule noCookModule = new DialogueModule("Not to worry! I can teach you the ways of cooking. It’s quite simple once you grasp the basics. Would you like a lesson?");
                                                noCookModule.AddOption("Yes, please teach me!",
                                                    pr => true,
                                                    pr =>
                                                    {
                                                        p.SendGump(new DialogueGump(p, new DialogueModule("To start, always ensure your fire is just right—hot enough to sear but not so hot it burns the meat!")));
                                                    });
                                                noCookModule.AddOption("Maybe later.",
                                                    pt => true,
                                                    pt => 
                                                    {
                                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, noCookModule));
                                            });
                                        p.SendGump(new DialogueGump(p, prepareModule));
                                    });
                                p.SendGump(new DialogueGump(p, typesModule));
                            });
                        findMeatModule.AddOption("I want to go on an adventure to find some!",
                            ply => true,
                            ply =>
                            {
                                player.SendMessage("You prepare for a journey to the Lost Isles, excitement bubbling within you!");
                            });
                        player.SendGump(new DialogueGump(player, findMeatModule));
                    });
                dinosaurMeatModule.AddOption("What do you like most about it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule favoriteModule = new DialogueModule("It’s the adventure of the hunt and the camaraderie it fosters. Sharing a meal with friends after a successful hunt is what makes it truly special.");
                        favoriteModule.AddOption("That sounds wonderful!",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, favoriteModule));
                    });
                player.SendGump(new DialogueGump(player, dinosaurMeatModule));
            });

        greeting.AddOption("What else do you enjoy about these lands?",
            player => true,
            player => 
            {
                DialogueModule landsModule = new DialogueModule("These lands are rich with life and adventure! Every corner holds a new discovery, and the people I've met are as varied as the creatures here.");
                
                landsModule.AddOption("Tell me about the people.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule peopleModule = new DialogueModule("The inhabitants here are intriguing! Some are wise sages, while others are fierce warriors. They all have stories to share, filled with trials and triumphs.");
                        peopleModule.AddOption("What trials have they faced?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Many have battled the wild beasts for survival, while others seek to protect their homes from the elemental forces that rage across the land.")));
                            });
                        pl.SendGump(new DialogueGump(pl, peopleModule));
                    });
                
                landsModule.AddOption("What kind of adventures have you had?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule adventuresModule = new DialogueModule("I've traversed dense jungles, climbed treacherous mountains, and even navigated stormy seas! Each journey brings its own challenges and rewards.");
                        adventuresModule.AddOption("What was the most dangerous?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Encountering a pack of hungry raptors was quite the thrill! Their speed is unmatched, and their hunger insatiable. But the experience is worth every heartbeat!")));
                            });
                        pl.SendGump(new DialogueGump(pl, adventuresModule));
                    });
                
                player.SendGump(new DialogueGump(player, landsModule));
            });

        return greeting;
    }

    private bool CanGiveToken(Mobile from)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private void GiveToken(Mobile from)
    {
        if (CanGiveToken(from))
        {
            from.AddToBackpack(new MaceFightingAugmentCrystal()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
            from.SendMessage("Here, take this amulet. It carries a fragment of a storm's might.");
        }
        else
        {
            from.SendMessage("I have no reward right now. Please return later.");
        }
    }

    public TorkTheThundercaller(Serial serial) : base(serial) { }

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
