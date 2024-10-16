using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Twister Terry")]
public class TwisterTerry : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TwisterTerry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Twister Terry";
        Body = 0x190; // Human male body

        // Stats
        SetStr(160);
        SetDex(60);
        SetInt(110);
        SetHits(110);

        // Appearance
        AddItem(new LongPants() { Hue = 44 });
        AddItem(new Tunic() { Hue = 44 });
        AddItem(new Sandals() { Hue = 1156 });
        AddItem(new LeatherGloves() { Name = "Terry's Twisting Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Twister Terry, master of the mat! How can I assist you, adventurer?");

        greeting.AddOption("Tell me about Twisting Techniques.",
            player => true,
            player =>
            {
                DialogueModule twistingTechniquesModule = new DialogueModule("Ah, the ancient art of Twisting! It encompasses not just physical prowess but mental discipline. Would you like to hear about its history, techniques, or perhaps a specific technique?");
                twistingTechniquesModule.AddOption("What is the history of Twisting?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule historyModule = new DialogueModule("Twisting Techniques originated on the distant planet of the former Planar Imperium, where warriors combined physical skill with the energy of the cosmos. Over generations, these techniques evolved into a martial art that emphasizes fluidity and adaptability.");
                        historyModule.AddOption("What happened to the Planar Imperium?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The Planar Imperium fell due to internal strife and external invasions. However, the techniques and knowledge of its warriors endure, passed down through dedicated practitioners like myself.")));
                            });
                        historyModule.AddOption("What are some legendary Twisters?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Many renowned Twisters emerged from the Imperium, like Yaro the Infinite, whose twists could deflect spells, and Maris the Unyielding, known for her unmatched strength in grappling.")));
                            });
                        player.SendGump(new DialogueGump(player, historyModule));
                    });
                twistingTechniquesModule.AddOption("What techniques can I learn?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule techniquesModule = new DialogueModule("There are many techniques, each with its own focus. Some notable ones include the Spiral Grapple, Whirlwind Throw, and the Ethereal Twist. Which one piques your interest?");
                        techniquesModule.AddOption("Tell me about the Spiral Grapple.",
                            p => true,
                            p =>
                            {
                                DialogueModule spiralGrappleModule = new DialogueModule("The Spiral Grapple is a defensive maneuver that allows you to redirect an opponent's force while positioning yourself for a counterattack. It's all about timing and awareness.");
                                spiralGrappleModule.AddOption("How can I master it?",
                                    plq => true,
                                    plq =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("To master the Spiral Grapple, practice your timing and observation skills. Visualize the flow of energy and learn to move in harmony with it.")));
                                    });
                                spiralGrappleModule.AddOption("What is the philosophy behind it?",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The philosophy of the Spiral Grapple teaches us that sometimes, the best defense is to become one with the force that is coming at you. Flow like water, and you shall not be broken.")));
                                    });
                                player.SendGump(new DialogueGump(player, spiralGrappleModule));
                            });
                        techniquesModule.AddOption("Tell me about the Whirlwind Throw.",
                            p => true,
                            p =>
                            {
                                DialogueModule whirlwindThrowModule = new DialogueModule("The Whirlwind Throw utilizes the momentum of your opponent against them. With a swift pivot and a forceful twist, you can send them sprawling.");
                                whirlwindThrowModule.AddOption("How can I learn this?",
                                    ple => true,
                                    ple =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("To learn the Whirlwind Throw, focus on your center of gravity and your opponent's movements. Practice with a partner to develop the necessary timing and strength.")));
                                    });
                                whirlwindThrowModule.AddOption("What is its origin?",
                                    plr => true,
                                    plr =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The Whirlwind Throw was inspired by the natural whirlwinds that sweep across the plains of the Imperium. It symbolizes the harmony between strength and grace.")));
                                    });
                                player.SendGump(new DialogueGump(player, whirlwindThrowModule));
                            });
                        techniquesModule.AddOption("Tell me about the Ethereal Twist.",
                            p => true,
                            p =>
                            {
                                DialogueModule etherealTwistModule = new DialogueModule("The Ethereal Twist focuses on agility and speed, allowing you to evade attacks while positioning for a counter. It requires a deep understanding of your own body and the space around you.");
                                etherealTwistModule.AddOption("How do I develop agility?",
                                    plt => true,
                                    plt =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Agility can be developed through exercises that emphasize flexibility and balance. Try incorporating yoga or acrobatics into your training regime.")));
                                    });
                                etherealTwistModule.AddOption("Is it difficult to learn?",
                                    ply => true,
                                    ply =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Like any skill, it requires dedication. The more you practice the Ethereal Twist, the more intuitive it will become. Persistence is key!")));
                                    });
                                player.SendGump(new DialogueGump(player, etherealTwistModule));
                            });
                        player.SendGump(new DialogueGump(player, techniquesModule));
                    });
                player.SendGump(new DialogueGump(player, twistingTechniquesModule));
            });

        greeting.AddOption("What else can you tell me?",
            player => true,
            player =>
            {
                DialogueModule moreInfoModule = new DialogueModule("In addition to techniques, Twisting is about the journey of self-discovery. Would you like to hear about training, philosophy, or perhaps a story from my adventures?");
                moreInfoModule.AddOption("Tell me about your training.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule trainingModule = new DialogueModule("My training involved years of rigorous practice and meditation. I traveled to various lands, seeking out masters of the art and absorbing their wisdom.");
                        trainingModule.AddOption("Did you face any challenges?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Indeed, the journey was fraught with challenges. I faced opponents who tested my resolve and mentors who pushed me beyond my limits. Each trial shaped me into who I am today.")));
                            });
                        trainingModule.AddOption("What was the most important lesson?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The most important lesson was to embrace failure as a teacher. Each defeat provided insight that led to eventual success. Perseverance is the true mark of a master.")));
                            });
                        player.SendGump(new DialogueGump(player, trainingModule));
                    });
                moreInfoModule.AddOption("What is the philosophy of Twisting?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule philosophyModule = new DialogueModule("The philosophy behind Twisting emphasizes balance between mind and body, the harmony of action and reaction. It teaches us to adapt to circumstances while staying true to our core values.");
                        philosophyModule.AddOption("How can I apply this philosophy?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("You can apply this philosophy to every aspect of life. Whether in battle or daily challenges, find balance, stay aware, and adapt your approach as needed.")));
                            });
                        philosophyModule.AddOption("Can you share a personal story?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Once, I faced a formidable opponent who overwhelmed me with brute strength. Instead of fighting back head-on, I remembered to flow like water and outmaneuver him, turning the tide of battle.")));
                            });
                        player.SendGump(new DialogueGump(player, philosophyModule));
                    });
                moreInfoModule.AddOption("Do you have any stories from your adventures?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule storiesModule = new DialogueModule("Ah, countless tales! I’ve traveled to enchanted forests, battled ancient beasts, and even danced with the stars. Which type of story would you prefer? A battle tale, a lesson learned, or a humorous encounter?");
                        storiesModule.AddOption("Tell me a battle tale.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("In one memorable fight against a shadowy creature, I had to rely on my Twisting Techniques to evade its deadly attacks. With every twist, I found a new opportunity to strike, ultimately turning the tide.")));
                            });
                        storiesModule.AddOption("What lesson did you learn?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("In the depths of the Whispering Caves, I learned the importance of patience. Rushing in led to mistakes, while taking time to observe the environment granted me clarity.")));
                            });
                        storiesModule.AddOption("Any humorous encounters?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Once, I accidentally challenged a goat to a wrestling match, thinking it was an ancient guardian. The goat won, and I learned to never underestimate even the smallest of opponents!")));
                            });
                        player.SendGump(new DialogueGump(player, storiesModule));
                    });
                player.SendGump(new DialogueGump(player, moreInfoModule));
            });

        return greeting;
    }

    public TwisterTerry(Serial serial) : base(serial) { }

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
