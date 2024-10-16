using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

public class Pepin : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Pepin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Pepin";
        Body = 0x190; // Human male body

        // Stats
        SetStr(60);
        SetDex(50);
        SetInt(120);
        SetHits(50);

        // Appearance
        AddItem(new Robe() { Hue = 1155 });
        AddItem(new Sandals() { Hue = 0 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
        SpeechHue = 0; // Default speech hue
    }

	public Pepin(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Pepin, the healer of Tristram. What brings you to this troubled land?");

        greeting.AddOption("I've come to learn about the hero who slew Diablo.",
            player => true,
            player =>
            {
                DialogueModule heroModule = new DialogueModule("Ah, the legendary hero who faced Diablo himself. A brave soul who came to save Tristram from darkness. Would you like to hear about his tale?");
                
                heroModule.AddOption("Yes, please tell me his story.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule storyModule = new DialogueModule("This hero was known for his courage and strength. He fought valiantly against the Prime Evil, wielding power and skill that few could match. However, in his victory, a terrible curse was bestowed upon him. Would you like to know what happened next?");
                        
                        storyModule.AddOption("What curse befell him?",
                            p => true,
                            p =>
                            {
                                DialogueModule curseModule = new DialogueModule("The curse transformed him, warping his very essence. Though he had vanquished Diablo, the darkness within tainted his soul. He became a wanderer, forever haunted by the memories of his battles. His triumph turned to tragedy. Would you like to hear about his struggles after the battle?");
                                
                                curseModule.AddOption("Yes, I want to know more.",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule struggleModule = new DialogueModule("In the aftermath of his victory, he sought redemption. He believed that by healing the land and its people, he could atone for the darkness within him. But each attempt to heal was met with failure as the shadows clung to him. Do you wish to know about his encounters with the people of Tristram?");
                                        
                                        struggleModule.AddOption("Absolutely, tell me about them.",
                                            pw => true,
                                            pw =>
                                            {
                                                DialogueModule encountersModule = new DialogueModule("The people of Tristram were initially grateful for his deeds, but as time went on, they began to see the darkness in his eyes. They whispered among themselves, fearing that he was a harbinger of doom. The hero, in his attempts to help, often found himself shunned. Do you wish to know how he tried to win their trust back?");
                                                
                                                encountersModule.AddOption("Yes, what did he do?",
                                                    ple => true,
                                                    ple =>
                                                    {
                                                        DialogueModule trustModule = new DialogueModule("The hero began to offer his aid to the wounded and sick, providing healing potions and remedies. He sought me out to learn the secrets of healing, hoping to alleviate the suffering around him. Would you like to hear about a specific remedy he sought?");
                                                        
                                                        trustModule.AddOption("What remedy did he seek?",
                                                            p2 => true,
                                                            p2 =>
                                                            {
                                                                DialogueModule remedyModule = new DialogueModule("He was particularly interested in a potion I crafted—a blend of moonlit herbs and the essence of compassion. This potion was said to bring clarity and hope. I taught him the secrets, but alas, the darkness in him resisted the light. Would you like to know if he succeeded?");
                                                                
                                                                remedyModule.AddOption("Did he succeed?",
                                                                    p3 => true,
                                                                    p3 =>
                                                                    {
                                                                        DialogueModule successModule = new DialogueModule("In moments of clarity, he would use the potion to heal those in need, and they would see glimpses of the hero they once revered. But the shadows would soon return, and despair would follow. It was a never-ending battle for him. Would you like to hear about his last attempt?");
                                                                        
                                                                        successModule.AddOption("Yes, tell me about his last attempt.",
                                                                            pl2 => true,
                                                                            pl2 =>
                                                                            {
                                                                                DialogueModule lastAttemptModule = new DialogueModule("In his final attempt, he ventured into the heart of the cathedral, seeking to confront the darkness within. He hoped that by facing it head-on, he could find peace. Many believed he perished there, lost to the shadows forever. But some say he still roams the land, seeking redemption. Do you wish to learn about how his tale affects Tristram today?");
                                                                                
                                                                                lastAttemptModule.AddOption("Yes, how does it affect Tristram?",
                                                                                    p4 => true,
                                                                                    p4 =>
                                                                                    {
                                                                                        DialogueModule effectModule = new DialogueModule("The hero's tale serves as a cautionary reminder to the people. They honor his bravery but remain wary of the darkness that can dwell within even the greatest of heroes. It teaches them the importance of compassion and the eternal struggle between light and dark. Is there anything else you wish to know?");
                                                                                        
                                                                                        effectModule.AddOption("What lessons can we learn from his story?",
                                                                                            pl3 => true,
                                                                                            pl3 =>
                                                                                            {
                                                                                                pl3.SendGump(new DialogueGump(pl3, new DialogueModule("We learn that even in our darkest moments, compassion can guide us. The hero's struggle reminds us that redemption is a path worth pursuing, no matter how difficult it may be. It's a call to face our shadows, to embrace our humanity. If you carry this lesson, then his sacrifice has not been in vain.")));
                                                                                            });

                                                                                        effectModule.AddOption("No, I want to speak of something else.",
                                                                                            pl3 => true,
                                                                                            pl3 =>
                                                                                            {
                                                                                                pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                                                            });

                                                                                        p4.SendGump(new DialogueGump(p4, effectModule));
                                                                                    });
                                                                                
                                                                                lastAttemptModule.AddOption("I want to know more about the hero.",
                                                                                    pl3 => true,
                                                                                    pl3 =>
                                                                                    {
                                                                                        pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule()));
                                                                                    });

                                                                                pl2.SendGump(new DialogueGump(pl2, lastAttemptModule));
                                                                            });
                                                                        
                                                                        remedyModule.AddOption("I wish to learn more about potions.",
                                                                            pl3 => true,
                                                                            pl3 =>
                                                                            {
                                                                                pl3.SendGump(new DialogueGump(pl3, CreatePotionModule()));
                                                                            });

                                                                        p3.SendGump(new DialogueGump(p3, successModule));
                                                                    });
                                                                
                                                                remedyModule.AddOption("Can you tell me more about the essence of compassion?",
                                                                    p2e => true,
                                                                    p2e =>
                                                                    {
                                                                        p2.SendGump(new DialogueGump(p2, new DialogueModule("The essence of compassion is not merely an ingredient; it is a state of being. It involves understanding the pain of others and seeking to alleviate it. It is the light that can push back the shadows if only one allows it to shine.")));
                                                                    });

                                                                p2.SendGump(new DialogueGump(p2, remedyModule));
                                                            });
                                                        
                                                        ple.SendGump(new DialogueGump(ple, trustModule));
                                                    });
                                                
                                                pw.SendGump(new DialogueGump(pw, encountersModule));
                                            });
                                        
                                        player.SendGump(new DialogueGump(player, struggleModule));
                                    });
                                
                                p.SendGump(new DialogueGump(p, curseModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, storyModule));
                    });
                
                player.SendGump(new DialogueGump(player, heroModule));
            });

        greeting.AddOption("What about Tristram?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Tristram has seen better days. The shadows of its past linger, and the echoes of lost heroes can still be felt in the air. It is a town of hope and despair, resilience and sorrow. Would you like to learn about how to help its people?")));
            });

        greeting.AddOption("Do you sell healing supplies?",
            player => true,
            player =>
            {
                DialogueModule shopModule = new DialogueModule("I do have some healing supplies. Feel free to browse my collection, as they may aid you on your journey.");
                shopModule.AddOption("Let me see what you have.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Pepin shows you his collection of alchemical supplies.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                shopModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, shopModule));
            });

        return greeting;
    }

    private DialogueModule CreatePotionModule()
    {
        DialogueModule potionModule = new DialogueModule("Potions are crafted with great care. Each has its unique properties and effects. What would you like to know?");
        
        potionModule.AddOption("How do I create a healing potion?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("To create a healing potion, you need the essence of moonlit herbs and clear water. Combine them carefully, and you may restore vitality to those in need.")));
            });

        potionModule.AddOption("What rare ingredients do you need?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Some rare ingredients include the Essence of Compassion and Dragon's Breath. Both are difficult to obtain but have powerful effects.")));
            });

        potionModule.AddOption("Maybe another time.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        return potionModule;
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
